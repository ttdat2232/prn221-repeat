using Application.Services;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using NUnit.Framework;
using Repositories.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTest
{
    [SetUpFixture]
    public class BaseSetUp
    {
        protected IUnitOfWork unitOfWork;
        protected IMembershipService membershipService;
        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            unitOfWork = new UnitOfWork(isUseInMemories: true);
            await SeedData();
            membershipService = new MembershipService(unitOfWork);
        }

        private async Task SeedData()
        {
            var majors = new List<Major>
            {
                new Major { Name  = "SE"},
                new Major { Name  = "SS"}
            };
            for(int i= 0; i < majors.Count; i++)
            {
                majors[i] = await unitOfWork.Majors.AddAsync(majors[i]);
            }
            var grade = new Grade { Id = "K16", StartAt = DateTime.Now };
            await unitOfWork.Grades.AddAsync(grade);
            await unitOfWork.Clubs.AddAsync(new Club
            {
                CreateAt = DateTime.Now,
                LogoUrl = ".....",
                Name = "Test",
            });
            var students = new List<Student>();
            for(int i = 0; i < 10; i++)
            {
                students.Add(new Student
                {
                    Birthday = DateTime.Now,
                    Name = Guid.NewGuid().ToString(),
                    Status = StudentStatus.IN_PROGRESS,
                    MajorId = new Random().NextInt64(0, 2),
                    GradeId = "K16"
                });
            }
            foreach(var student in students)
                await unitOfWork.Students.AddAsync(student);
            await unitOfWork.Memberships.AddAsync(new Membership(1, 2)
            {
                JoinDate = DateTime.Now,
                Status = MemberStatus.JOIN,
                Role = MemberRole.MEMBER,
            });
            await unitOfWork.CompleteAsync();
        }
    }
}
