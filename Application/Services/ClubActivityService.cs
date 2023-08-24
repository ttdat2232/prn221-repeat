using Application.Converters;
using Application.Exceptions;
using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;
using static Application.Utilities.Validator;

namespace Application.Services
{
    public class ClubActivityService : IClubActivityService
    {
        private readonly IUnitOfWork unitOfWork;

        public ClubActivityService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<PaginationResult<ClubActivityDto>> GetAllClubActivitiesByClubIdAsync(long clubId, int pageIndex = 0)
        {
            var activities = await unitOfWork.ClubActivities.GetAsync(
                expression: ca => ca.ClubId.HasValue && ca.ClubId.Value == clubId,
                pageIndex: pageIndex,
                orderBy: a => a.OrderByDescending(act => act.StartAt));
            return new PaginationResult<ClubActivityDto>
            {
                PageCount = activities.PageCount,
                PageIndex = activities.PageIndex,
                TotalCount = activities.TotalCount,
                TotalPages = activities.TotalPages,
                Values = activities.Values.Select(a => AppConverter.ToDto(a)).ToList(),
            };
        }

        public async Task<ClubActivityDto> GetClubActivityByIdAsync(long id)
        {
            var activity = await unitOfWork.ClubActivities.GetById(id);
            return AppConverter.ToDto(activity);
        }

        public async Task<ClubActivityDto> AddClubActivityAsync(ClubActivityCreateDto createClubActivity)
        {
            if (createClubActivity.StartAt <= DateTime.Now || createClubActivity.EndAt <= DateTime.Now)
                throw new AppException("Starting time or ending time is not valid");
            if (createClubActivity.StartAt >= createClubActivity.EndAt)
                throw new AppException("Starting time must be less than ending time");
            if (createClubActivity.ParticipatingMembersIds.Count <= 0)
                throw new AppException("Participants must not be empty");
            var club = await unitOfWork.Clubs.GetAsync(expression: c => c.Id == createClubActivity.ClubId, include: new string[] { nameof(Club.ClubActivities) })
                .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new NotFoundException(typeof(Club), createClubActivity.ClubId, GetType()));
            var entityToAdd = AppConverter.ToEntity(createClubActivity);
            entityToAdd.ClubId = club.Id;
            entityToAdd.Participants = new List<Participant>();
            if (club.ClubActivities != null && club.ClubActivities.Any())
            {
                foreach (var clubActivity in club.ClubActivities)
                {
                    if (clubActivity.Status != ActivityStatus.END && IsTimeConflict(createClubActivity.StartAt, createClubActivity.EndAt, clubActivity.StartAt, clubActivity.EndAt))
                        throw new AppException($"Time conflict with \"{clubActivity.Name}\" activity from {clubActivity.StartAt} to {clubActivity.EndAt}");
                }
            }
            foreach (var participatingMemberId in createClubActivity.ParticipatingMembersIds)
            {
                var member = await unitOfWork.Memberships.GetById(participatingMemberId);
                var clubActivities = await unitOfWork.ClubActivities.GetClubActivitiesByStudentIdAsync(member.StudentId);
                foreach (var act in clubActivities)
                {
                    if (act.Status != ActivityStatus.END && IsTimeConflict(createClubActivity.StartAt, createClubActivity.EndAt, act.StartAt, act.EndAt))
                    {
                        throw new AppException($"Member with ID: {member.Id} was in another activity from {act.StartAt} to {act.EndAt}");
                    }
                }
                entityToAdd.Participants.Add(new Participant
                {
                    ClubActivity = entityToAdd,
                    MembershipId = member.Id,
                    Status = ParticipantStatus.ACTIVE,
                });
            }
            try
            {
                entityToAdd = await unitOfWork.ClubActivities.AddAsync(entityToAdd);
                return await unitOfWork.CompleteAsync() > 0 ? AppConverter.ToDto(entityToAdd) : throw new AppException("Added failed");
            }
            catch (Exception)
            {
                throw new AppException("Added failed");
            }
        }

        public async Task<ClubActivityDto> UpdateActivity(ClubActivityUpdateDto update)
        {
            var entityToUpdate = await IsValidClubActivity(update);
            try
            {
                entityToUpdate = unitOfWork.ClubActivities.Update(entityToUpdate);
                return await unitOfWork.CompleteAsync() > 0 ? AppConverter.ToDto(entityToUpdate) : throw new AppException("Updated failed");
            }
            catch (Exception)
            {
                throw new AppException("Error occurred");
            }
            throw new NotImplementedException();
        }

        private async Task<ClubActivity> IsValidClubActivity(ClubActivityUpdateDto updateActivity)
        {
            if (updateActivity.StartAt <= DateTime.Now || updateActivity.EndAt <= DateTime.Now)
                throw new AppException("Starting time or ending time is not valid");
            if (updateActivity.StartAt >= updateActivity.EndAt)
                throw new AppException("Starting time must be less than ending time");
            var clubActivity = await unitOfWork.ClubActivities
                .GetAsync(expression: ca => ca.Id == updateActivity.Id, include: new string[] { nameof(ClubActivity.Participants) })
                .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new NotFoundException(typeof(ClubActivity), updateActivity.Id, GetType()));
            if (clubActivity.Status == ActivityStatus.END)
                throw new AppException("Cannot Update an END activity");
            var clubId = clubActivity.ClubId.HasValue ? clubActivity.ClubId.Value : 0;
            var club = await unitOfWork.Clubs.GetById(clubId);
            if (clubActivity.Status == ActivityStatus.END)
                throw new AppException("Cannot update an END activity");
            if (club.ClubActivities != null && club.ClubActivities.Count > 0)
            {
                foreach (var activity in club.ClubActivities)
                {
                    if (activity.Id == updateActivity.Id) continue;
                    if (IsTimeConflict(updateActivity.StartAt, updateActivity.EndAt, activity.StartAt, activity.EndAt))
                        throw new AppException($"Time conflict with \"{activity.Name} : {activity.StartAt} to {activity.EndAt}\" activity");
                }
            }
            if (updateActivity.ParticipatingMembersIds.Count > 0)
            {
                var existed = clubActivity.Participants?.Select(p => p.MembershipId).ToList();
                clubActivity.Participants?.Clear();
                foreach (var memberId in updateActivity.ParticipatingMembersIds)
                {
                    if (existed != null && existed.Any(e => e == memberId))
                        continue;
                    var member = await unitOfWork.Memberships.GetById(memberId);
                    var clubActivities = await unitOfWork.ClubActivities.GetClubActivitiesByStudentIdAsync(member.StudentId);
                    foreach (var act in clubActivities)
                    {
                        if (act.Status != ActivityStatus.END && IsTimeConflict(updateActivity.StartAt, updateActivity.EndAt, act.StartAt, act.EndAt))
                        {
                            throw new AppException($"Member was in another activity from {act.StartAt} to {act.EndAt}");
                        }
                    }
                    clubActivity.Participants?.Add(new Participant
                    {
                        ClubActivityId = clubActivity.Id,
                        Membership = member,
                        MembershipId = memberId,
                        Status = ParticipantStatus.ACTIVE
                    });
                }
            }
            return clubActivity;
        }

        public async Task DeleteActivityByIdAsync(long id)
        {
            var result = await unitOfWork.ClubActivities.GetAsync(expression: a => a.Id == id)
                .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new NotFoundException(typeof(ClubActivity), id, GetType()));
            if (result.Status == ActivityStatus.END)
                throw new AppException("Cannot delete an END activity");
            try
            {

                await unitOfWork.ClubActivities.DeleteAsync(id: id);
                if (await unitOfWork.CompleteAsync() <= 0)
                    throw new AppException("Deleted failed");
            }
            catch (Exception)
            {
                throw new AppException("Error occurred");
            }
        }

        public async Task UpdateStatusAllActivityAsync()
        {
            var activities = await unitOfWork.ClubActivities.GetAsync(expression: a => a.Status != ActivityStatus.END, isTakeAll: true);
            if (activities.Values.Any())
            {

                try
                {
                    foreach (var activity in activities.Values)
                    {
                        var startTimeRelation = DateTime.Compare(DateTime.Now, activity.StartAt);
                        var endTimeRelation = DateTime.Compare(DateTime.Now, activity.EndAt);
                        if (DateTime.Compare(DateTime.Now, activity.StartAt) >= 0 && activity.Status == ActivityStatus.UNSTART)
                            activity.Status = ActivityStatus.START;
                        if (DateTime.Compare(DateTime.Now, activity.EndAt) >= 0 && activity.Status == ActivityStatus.START)
                            activity.Status = ActivityStatus.END;
                        unitOfWork.ClubActivities.Update(activity);
                    }

                    await unitOfWork.CompleteAsync();
                }
                catch (Exception ex)
                {
                    throw new AppException(ex.Message);
                }
            }
        }
    }
}