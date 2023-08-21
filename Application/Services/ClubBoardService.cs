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
    public class ClubBoardService : IClubBoardService
    {
        private readonly IUnitOfWork unitOfWork;

        public ClubBoardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<PaginationResult<ClubBoardDto>> GetClubBoardByClubIdAsync(long clubId)
        {
            var result = await unitOfWork.ClubBoards
                .GetAsync(expression: cb => cb.Id == clubId, include: new string[] {nameof(ClubBoard.Memberships)}, isTakeAll: true)
                .ContinueWith(t => t.Result);
            return new PaginationResult<ClubBoardDto>
            {
                PageCount = result.TotalCount,
                PageIndex = result.PageIndex,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Values = result.Values.Select(v => AppConverter.ToDto(v)).ToList(),
            };
        }
        public async Task<ClubBoardDto> GetClubBoardByIdAsync(long id)
        {
            return await unitOfWork.ClubBoards.GetAsync(expression: cb => cb.Id == id, include: new string[] { nameof(ClubBoard.Memberships), nameof(ClubBoard.Club)})
                .ContinueWith(t => t.Result.Values.Count <= 0 ? throw new NotFoundException(typeof(ClubBoard), id, GetType()) : AppConverter.ToDto(t.Result.Values.First()));
        }

        public async Task<ClubBoardDto> AddClubBoardAsync(ClubBoardCreateDto clubBoard)
        {
            var club = await unitOfWork.Clubs.GetAsync(expression: c => c.Id == clubBoard.ClubId)
                .ContinueWith(t => t.Result.Values.Count <= 0 ? throw new NotFoundException(typeof(Club), clubBoard.ClubId ?? 0, GetType()) : t.Result.Values.First());
            var existed = await unitOfWork.ClubBoards.GetAsync(expression: cb => cb.Name.ToLower().Equals(clubBoard.Name.ToLower()));
            if (existed.Values.Count > 0)
                throw new AppException($"ClubBoard's name: {clubBoard.Name} is already existed");
            var entityToAdd = AppConverter.ToEntity(clubBoard);
            if (clubBoard.MembershipIds != null && clubBoard.MembershipIds.Count > 0)
            {
                entityToAdd.Memberships = new List<Membership>();
                foreach(var memberId in clubBoard.MembershipIds)
                {
                    var member = await unitOfWork.Memberships.GetAsync(expression: m => m.Id == memberId && m.Status == MemberStatus.JOIN)
                        .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new NotFoundException(typeof(Membership), memberId, GetType()));
                    entityToAdd.Memberships.Add(member);
                }
            }
            try
            {
                entityToAdd = await unitOfWork.ClubBoards.AddAsync(entityToAdd);
                return await unitOfWork.CompleteAsync() > 0 ? AppConverter.ToDto(entityToAdd) : throw new AppException("Added Failed");
            }
            catch(Exception)
            {
                throw new AppException("Added Failed");
            }
        }

        public async Task<ClubBoardDto> UpdateClubBoardAsync(ClubBoardUpdateDto updateClubBoard)
        {
            var entityToUpdate = await unitOfWork.ClubBoards.GetAsync(expression: cb => cb.Id == updateClubBoard.Id, include: new string[] { nameof(ClubBoard.Memberships) })
                .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new NotFoundException(typeof(ClubBoard), updateClubBoard.Id, GetType()));
            var club = await unitOfWork.Clubs.GetAsync(expression: c => c.Id == updateClubBoard.ClubId)
                .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new NotFoundException(typeof(Club), updateClubBoard.ClubId, GetType()));
            AppConverter.ToEntity(updateClubBoard, ref entityToUpdate);
            if (updateClubBoard.MembershipIds != null && updateClubBoard.MembershipIds.Any())
            {
                entityToUpdate.Memberships?.Clear();
                foreach (var memberId in updateClubBoard.MembershipIds)
                {
                    var memeber = await unitOfWork.Memberships.GetAsync(expression: m => m.Id == memberId)
                        .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.Single() : throw new NotFoundException(typeof(Membership), memberId, GetType()));
                    entityToUpdate.Memberships?.Add(memeber);
                }
            }
            try
            {
                var result = unitOfWork.ClubBoards.Update(entityToUpdate);
                return await unitOfWork.CompleteAsync() > 0 ? AppConverter.ToDto(result) : throw new AppException("Updated failed");
            }
            catch(Exception)
            {
                throw new AppException("Updated failed");
            }
        }
        public async Task DeleteClubBoardByIdAsync(long clubBoardId)
        {
            try
            {
                await unitOfWork.ClubBoards.DeleteAsync(clubBoardId);
                if (await unitOfWork.CompleteAsync() <= 0)
                    throw new AppException("Deleted failed");
            }
            catch(Exception)
            {
                throw new AppException("Deleted failed");
            }
        }
    }
}
