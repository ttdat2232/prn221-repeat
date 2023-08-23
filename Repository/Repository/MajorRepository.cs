using Domain.Entities;
using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repositories.Repository.Base;

namespace Repositories.Repository
{
    public class MajorRepository : Repository<Major>, IMajorRepository
    {
        public MajorRepository(DbContext context) : base(context)
        {
        }
    }
}
