﻿using Application.Converters;
using Application.Exceptions;
using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Application.Services
{
    public class ClubBoardService : IClubBoardService
    {
        private readonly IUnitOfWork unitOfWork;

        public ClubBoardService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<PaginationResult<ClubBoardDto>> GetClubBoardsByClubIdAsync(long clubId, int pageIndex = 0)
        {
            var result = await unitOfWork.ClubBoards
                .GetAsync(expression: cb => cb.ClubId == clubId, include: new string[] { nameof(ClubBoard.Memberships) },
                pageIndex: pageIndex,
                orderBy: c => c.OrderByDescending(cb => cb.Id))
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
            var result = await unitOfWork.ClubBoards.GetById(id);
            return AppConverter.ToDto(result);
        }

        public async Task<ClubBoardDto> AddClubBoardAsync(ClubBoardCreateDto clubBoard)
        {
            var club = await unitOfWork.Clubs.GetAsync(expression: c => c.Id == clubBoard.ClubId)
                .ContinueWith(t => t.Result.Values.Count <= 0 ? throw new NotFoundException(typeof(Club), clubBoard.ClubId ?? 0, GetType()) : t.Result.Values.First());
            var existed = await unitOfWork.ClubBoards.GetAsync(expression: cb => cb.Name.ToLower().Equals(clubBoard.Name.Trim().ToLower()));
            if (existed.Values.Count > 0)
                throw new AppException($"ClubBoard's name: {clubBoard.Name} is already existed");
            var entityToAdd = AppConverter.ToEntity(clubBoard);
            if (clubBoard.MembershipIds != null && clubBoard.MembershipIds.Count > 0)
            {
                entityToAdd.Memberships = new List<Membership>();
                foreach (var memberId in clubBoard.MembershipIds)
                {
                    var member = await unitOfWork.Memberships.GetAsync(expression: m => m.Id == memberId && m.Status == MemberStatus.JOIN, include: new string[] { nameof(Membership.ClubBoards) })
                        .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new NotFoundException(typeof(Membership), memberId, GetType()));
                    member.ClubBoards?.Add(entityToAdd);
                    unitOfWork.Memberships.Update(member);
                }
            }
            try
            {
                entityToAdd = await unitOfWork.ClubBoards.AddAsync(entityToAdd);
                return await unitOfWork.CompleteAsync() > 0 ? AppConverter.ToDto(entityToAdd) : throw new AppException("Added Failed");
            }
            catch (Exception)
            {
                throw new AppException("Error occurred");
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
            catch (Exception)
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
            catch (Exception)
            {
                throw new AppException("Deleted failed");
            }
        }

        public async Task AddMembersToBoard(long id, List<long> newMemberId)
        {
            var clubBoard = await unitOfWork.ClubBoards.GetById(id);
            if (!newMemberId.Any()) return;
            foreach (var memberId in newMemberId)
            {
                var member = await unitOfWork.Memberships.GetAsync(expression: m => m.Id == memberId).ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new NotFoundException(typeof(Membership), memberId, GetType()));
                clubBoard.Memberships?.Add(member);
            }
            try
            {
                clubBoard = unitOfWork.ClubBoards.Update(clubBoard);
                if (await unitOfWork.CompleteAsync() <= 0)
                    throw new AppException("Adding member failed");
            }
            catch (Exception)
            {
                throw new AppException("Error Occurred");
            }
        }

        public async Task RemoveMemberFromBoard(long memberId, long clubBoardId)
        {
            var member = await unitOfWork.Memberships.GetById(memberId);
            var clubBoard = await unitOfWork.ClubBoards.GetById(clubBoardId);
            clubBoard.Memberships = clubBoard.Memberships?.Where(m => m.Id != memberId).ToList();
            try
            {
                clubBoard = unitOfWork.ClubBoards.Update(clubBoard);
                if (await unitOfWork.CompleteAsync() <= 0)
                    throw new AppException($"Adding member to {clubBoard.Name} failed");
            }
            catch (Exception) { throw new AppException("Error occurred"); }
        }
    }
}
