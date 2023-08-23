using Application.Converters;
using Application.Exceptions;
using Application.Utilities;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Application.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IUnitOfWork unitOfWork;

        public ParticipantService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ParticipantDto> AddParticipantAsync(long memberId, long activityId)
        {
            var activity = await unitOfWork.ClubActivities.GetById(activityId);
            if (activity.Participants != null && activity.Participants.Any())
            {
                var existedMemberIds = activity.Participants.Where(p => p.Status != ParticipantStatus.LEAVE).Select(p => p.Membership?.Id);
                if (existedMemberIds.Contains(memberId))
                    throw new AppException($"Member with ID: {memberId} is already in this activity");
            }
            var member = await unitOfWork.Memberships.GetById(memberId);
            if (member.ParticipatedActivities != null && member.ParticipatedActivities.Any())
            {
                foreach (var participated in member.ParticipatedActivities)
                {
                    if (participated.ClubActivity != null && (Validator.IsTimeBetween(participated.ClubActivity.StartAt, activity.StartAt, activity.EndAt) || Validator.IsTimeBetween(participated.ClubActivity.EndAt, activity.StartAt, activity.EndAt)))
                        throw new AppException($"Member was already in another activity from {participated.ClubActivity.StartAt} to {participated.ClubActivity.EndAt}");
                }
            }
            var entityToAdd = new Participant
            {
                ClubActivityId = activityId,
                MembershipId = memberId,
                Status = ParticipantStatus.ACTIVE
            };
            try
            {
                entityToAdd = await unitOfWork.Participants.AddAsync(entityToAdd);
                return await unitOfWork.CompleteAsync() > 0 ? AppConverter.ToDto(entityToAdd) : throw new AppException("Added failed");
            }
            catch (Exception)
            {
                throw new AppException("Error Occurred");
            }
        }

        public async Task DeleteParticipantAsync(long memberId, long clubActivityId)
        {
            try
            {
                var activity = await unitOfWork.ClubActivities.GetById(clubActivityId);
                if (activity.Status == ActivityStatus.END)
                    throw new AppException("Cannot modify an End activity");
                if (activity.Participants != null && activity.Participants.Count == 1)
                    throw new AppException("Only 1 participant in this activity. The activity must have as least 1 participant");
                await unitOfWork.Participants.DeleteAsync(new Participant { MembershipId = memberId, ClubActivityId = clubActivityId });
                if (await unitOfWork.CompleteAsync() <= 0)
                    throw new AppException("Deleted failed");
            }
            catch (Exception)
            {
                throw new Exception("Error occurred");
            }
        }
    }
}
