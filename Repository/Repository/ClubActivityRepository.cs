using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Base;

namespace Repositories.Repository
{
    public class ClubActivityRepository : Repository<ClubActivity>, IClubActivityRepository
    {
        public ClubActivityRepository(DbContext context) : base(context)
        {
        }

        public override async Task<ClubActivity> GetById(object id)
        {
            var clubActivity = context.Set<ClubActivity>().AsNoTracking().Where(ca => ca.Id == (long)id).Include(ca => ca.Club);
            var participants = context.Set<Participant>().AsNoTracking().Where(p => p.ClubActivityId == (long)id).Include(p => p.Membership).ThenInclude(m => m.Student);
            var result = await clubActivity.ToListAsync().ContinueWith(t => t.Result.Any() ? t.Result.First() : throw new NotFoundException(typeof(ClubActivity), (long)id, GetType()));
            result.Participants = await participants.ToListAsync();
            return result;
        }

        public async Task<List<ClubActivity>> GetClubActivitiesByStudentIdAsync(long id)
        {
            var members = await context.Set<Membership>().AsNoTracking().Include(m => m.ParticipatedActivities).Where(m => m.StudentId == id).ToListAsync();
            var activities = new List<ClubActivity>();
            foreach (var member in members)
            {
                if (member.ParticipatedActivities != null && member.ParticipatedActivities.Any())
                {
                    foreach (var participate in member.ParticipatedActivities)
                    {
                        var clubActivities = await context.Set<ClubActivity>().AsNoTracking().Where(a => a.Id == participate.ClubActivityId).ToListAsync();
                        activities.AddRange(clubActivities);
                    }
                }
            }
            return activities;
        }
    }
}
