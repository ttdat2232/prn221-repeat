using Application.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Base;

namespace Repositories.Repository
{
    public class ClubBoardRepository : Repository<ClubBoard>, IClubBoardRepository
    {
        public ClubBoardRepository(DbContext context) : base(context)
        {
        }

        public override async Task<ClubBoard> GetById(object id)
        {
            var key = (long)id;
            var result = await context.Set<ClubBoard>().Where(cb => cb.Id == key).Include(cb => cb.Memberships).ThenInclude(m => m.Student).ToListAsync();
            return result.Any() ? result.First() : throw new NotFoundException(typeof(ClubBoard), id, GetType());
        }
    }
}
