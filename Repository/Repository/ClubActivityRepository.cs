using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Repositories.Repository.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
