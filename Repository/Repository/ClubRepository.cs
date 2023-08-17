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
    public class ClubRepository : Repository<Club>, IClubRepository
    {
        public ClubRepository(DbContext context) : base(context)
        {
        }

        public override async Task<Club> GetById(object id)
        {
            long key = (long)id;
            IQueryable<Club> club = context.Set<Club>().AsNoTracking().Where(c => c.Id == key);
            IQueryable<ClubActivity> clubActivities = context.Set<ClubActivity>().AsNoTracking().Where(ca => ca.ClubId == key);
            IQueryable<Membership> members = context.Set<Membership>().AsNoTracking().Where(m => m.ClubId == key).Include(nameof(Membership.Student));
            IQueryable<ClubBoard> clubBoards = context.Set<ClubBoard>().AsNoTracking().Where(cb => cb.ClubId == key);
            var result = await club.ToListAsync().ContinueWith(t => t.Result.First() ?? throw new KeyNotFoundException());
            result.ClubBoards = await clubBoards.ToListAsync();
            result.Memberships = await members.ToListAsync();
            result.ClubActivities = await clubActivities.ToListAsync();
            return result;
        }
    }
}
