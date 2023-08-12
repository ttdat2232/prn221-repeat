using Application.Converters;
using Application.Exceptions;
using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork unitOfWork;

        public MembershipService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<MembershipDto> AddMemberShipAsync(MembershipCreateDto membership)
        {
            var student = await unitOfWork.Students.GetAsync(expression: s => s.Id == membership.StudentId && s.Status == StudentStatus.IN_PROGRESS, include: new string[] {nameof(Student.Memberships)})
                .ContinueWith(t => t.Result.Values.Count == 0 ? throw new NotFoundException(typeof(Student), membership.Id, this.GetType()) : t.Result.Values.Single());
            if (student.Memberships != null && student.Memberships.Any(ms => ms.ClubId == membership.ClubId))
                throw new AppException($"Member is already in this club");
            var club = await unitOfWork.Clubs
                .GetAsync(expression: c => c.Id == membership.ClubId).ContinueWith(t => t.Result.Values.Count == 0 ? throw new NotFoundException(typeof(Club), membership.ClubId, this.GetType()) : t.Result.Values.Single());
            var memberToAdd = AppConverter.ToEntity(membership);
            memberToAdd.ClubBoards = new List<ClubBoard>();
            if(membership.ClubBoardIds != null && membership.ClubBoardIds.Count > 0)
            {
                foreach(var id in membership.ClubBoardIds)
                {
                    var result = await unitOfWork.ClubBoards.GetAsync(expression: p => p.Id == id, include: new string[] {nameof(ClubBoard.Memberships)}, isDisableTracking: false)
                        .ContinueWith(t => t.Result.Values.Count == 0 ? throw new NotFoundException(typeof(ClubBoard), id, this.GetType()) : t.Result.Values.Single());
                    result.Memberships?.Add(memberToAdd);
                }
            }
            try
            {
                memberToAdd = await unitOfWork.Memberships.AddAsync(memberToAdd);
                return AppConverter.ToDto(memberToAdd);
            } catch(Exception)
            {
                throw new AppException("Add new membership failed");
            }
        }

        public async Task DeleteMembershipAsync(long id)
        {
            try
            {
                await unitOfWork.Memberships.DeleteAsync(id: id);
                if(await unitOfWork.CompleteAsync() <= 0)
                    throw new AppException("Deleted failed");
            }
            catch(Exception)
            {
                throw new AppException("Deleted failed");
            }
        }

        public async Task<MembershipDto> GetMemberShipByIdAsync(long id, MemberStatus status = MemberStatus.JOIN)
        {
            return await unitOfWork.Memberships.GetAsync(expression: m => m.Id == id && m.Status == status, include: new string[] {nameof(Membership.Student), nameof(Membership.Club)})
                .ContinueWith(t => t.Result.Values.Count > 0 ? AppConverter.ToDto(t.Result.Values.Single()) : throw new NotFoundException(typeof(Membership), id, this.GetType()));
        }

        public async Task<PaginationResult<MembershipDto>> GetMembershipByNameAsync(string name = "", MemberStatus status = MemberStatus.JOIN, int pageSize = 4, int pageIndex = 0)
        {
            var result = await unitOfWork.Memberships.GetAsync(
                expression: m => m.Student.Name.ToLower().Contains(name.ToLower()) && m.Status == status, 
                pageSize: pageSize, 
                pageIndex: pageIndex, 
                include: new string[] { nameof(Membership.Student), nameof(Membership.Club) });
            return new PaginationResult<MembershipDto>
            {
                PageCount = result.PageCount,
                PageIndex = result.PageIndex,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Values = result.Values.Select(m => AppConverter.ToDto(m)).ToList(),
            };
        }

        public async Task<MembershipDto> UpdateMembershipAsync(MembershipUpdateDto membership)
        {
            var membershipToUpdate = await unitOfWork.Memberships.GetAsync(expression: m => m.Id == membership.Id)
                .ContinueWith(t => t.Result.Values.Count <= 0 ? throw new NotFoundException(typeof(Membership), membership.Id, this.GetType()) : t.Result.Values.Single());
            AppConverter.ToEntity(membership, ref membershipToUpdate);
            try
            {
                membershipToUpdate = unitOfWork.Memberships.UpdateAsync(membershipToUpdate);
                return await unitOfWork.CompleteAsync() <= 0 ? throw new AppException("Cannot update") : AppConverter.ToDto(membershipToUpdate);
            }
            catch (Exception)
            {
                throw new AppException("Updated fail");
            }
        }
    }
}
