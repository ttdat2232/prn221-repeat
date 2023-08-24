using Domain.Entities;
using Domain.Interfaces.Repositories.Base;

namespace Domain.Interfaces.Repositories
{
    public interface IClubActivityRepository : IRepository<ClubActivity>
    {
        Task<List<ClubActivity>> GetClubActivitiesByStudentIdAsync(long id);
    }
}
