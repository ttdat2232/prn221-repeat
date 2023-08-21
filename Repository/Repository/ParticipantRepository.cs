using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class ParticipantRepository : Repository<Participant>, IParticipantRepository
    {
        public ParticipantRepository(DbContext context) : base(context)
        {
        }

        public override async Task DeleteAsync(Participant entity)
        {
            var result = await context.Set<Participant>()
                .Where(p => p.ClubActivityId == entity.ClubActivityId && p.MembershipId == entity.MembershipId).Include(p => p.ClubActivity).FirstAsync()
                .ContinueWith(t => t.Result ?? throw new NotFoundException(typeof(Participant), $"{entity.ClubActivityId} {entity.MembershipId}", GetType()));
            if (result.ClubActivity != null && result.ClubActivity.Status == ActivityStatus.END)
                throw new AppException("Cannot modify because activity is ended");
            context.Entry(result).State = EntityState.Deleted;
        }
    }
}
