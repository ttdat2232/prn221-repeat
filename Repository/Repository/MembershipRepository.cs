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
    public class MembershipRepository : Repository<Membership>, IMembershipRepository
    {
        public MembershipRepository(DbContext context) : base(context)
        {
        }

        public override async Task<Membership> AddAsync(Membership entity)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Memberships ON");
                await context.AddAsync(entity);
                await context.SaveChangesAsync();
                await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Memberships OFF");
                await transaction.CommitAsync();
                return entity;
            }
        }
    }
}
