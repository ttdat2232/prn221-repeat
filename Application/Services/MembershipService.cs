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
                .ContinueWith(t => t.Result.Values.Count == 0 ? throw new AppException($"Not found student's Id {membership.Id}") : t.Result.Values.Single());
            if (student.Memberships != null && student.Memberships.Any(ms => ms.ClubId == membership.ClubId))
                throw new AppException($"Member is already in this club");
            var club = await unitOfWork.Clubs
                .GetAsync(expression: c => c.Id == membership.ClubId).ContinueWith(t => t.Result.Values.Count == 0 ? throw new AppException($"Not found club's Id {membership.ClubId}") : t.Result.Values.Single());
            if(membership.ClubBoardId != null && membership.ClubBoardId.Length > 0)
            {
                foreach(var id in membership.ClubBoardId)
                {
                    var result = await unitOfWork.ClubBoards.GetAsync(expression: p => p.Id == id);
                    if (result.Values.Count == 0)
                        throw new AppException($"Not found clubBoard's Id {id}");
                }
            }
            var memberToAdd = AppConverter.ToEntity(membership);
            try
            {
                memberToAdd = await unitOfWork.Memberships.AddAsync(memberToAdd);
                var change = await unitOfWork.CompleteAsync();
                return change > 0 ? AppConverter.ToDto(memberToAdd) : throw new AppException("Add new membership failed");
            } catch(Exception)
            {
                throw new AppException("Add new membership failed");
            }
        }

        public async Task DeleteMembershipAsync(long id)
        {
            try
            {
                await unitOfWork.Memberships.DeleteAsync(new Membership { Id = id });
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
            return await unitOfWork.Memberships.GetAsync(expression: m => m.Id == id && m.Status == status, include: new string[] {nameof(Student), nameof(Club)})
                .ContinueWith(t => t.Result.Values.Count > 0 ? AppConverter.ToDto(t.Result.Values.Single()) : throw new AppException($"Not found membership's Id {id}"));
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
                .ContinueWith(t => t.Result.Values.Count <= 0 ? throw new AppException($"Not found membership's Id {membership.Id}") : t.Result.Values.Single());
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
