using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext context;
        private bool disposed = false;

        public IMembershipRepository Memberships => memberships ?? (memberships = new MembershipRepository(context));

        public IClubActivityRepository ClubActivities => clubActivities ?? (clubActivities = new ClubActivityRepository(context));

        public IClubRepository Clubs => clubs ?? (clubs = new ClubRepository(context));

        public IStudentRepository Students => students ?? (students = new StudentRepository(context));

        public IGradeRepository Grades => grades ?? (grades = new GradeRepository(context));

        public IMajorRepository Majors => majors ?? (majors = new MajorRepository(context));

        public IParticipantRepository Participants => participants ?? (participants = new ParticipantRepository(context));

        public IClubBoardRepository ClubBoards => clubBoards ?? (clubBoards = new ClubBoardRepository(context));

        private IMembershipRepository memberships;
        private IClubActivityRepository clubActivities;
        private IClubRepository clubs;
        private IStudentRepository students;
        private IGradeRepository grades;
        private IMajorRepository majors;
        private IParticipantRepository participants;
        private IClubBoardRepository clubBoards;

        public UnitOfWork(bool isUseInMemories = false)
        {
            context = new ClubMembershipContext(isUseInMemories);
        }

        public int Complete()
        {
            return context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing) 
        {
            if (disposed) return;
            if (disposing)
            {
                context.Dispose();
            }
            disposed = true;
        }
        public async ValueTask DisposeAsync()
        {
            await context.DisposeAsync();
            Dispose(false);
            GC.SuppressFinalize(this);
        }
    }
}
