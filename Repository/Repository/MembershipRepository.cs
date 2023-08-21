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
    public class MembershipRepository : Repository<Membership>, IMembershipRepository
    {
        public MembershipRepository(DbContext context) : base(context)
        {
        }

        public override async Task<Membership> GetById(object id)
        {
            var member = context.Set<Membership>().AsNoTracking().Where(m => m.Id == (long)id);
            var participant = context.Set<Participant>().AsNoTracking().Where(p => p.MembershipId == (long)id).Include(nameof(Participant.ClubActivity));
            var result = await member.ToListAsync().ContinueWith(t => t.Result.Any() ? t.Result.First() : throw new NotFoundException(typeof(Membership), id, GetType()));
            await participant.ToListAsync().ContinueWith(t =>
            {
                result.ParticipatedActivities = t.Result;
            });
            return result;
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
