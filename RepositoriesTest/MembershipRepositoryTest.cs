using Domain.Entities;
using Domain.Interfaces.Repositories.Base;
using Repositories.Repository;
using Repository.Models;

namespace RepositoriesTest
{
    public class MembershipRepositoryTest
    {
        ClubMembershipContext context;
        IRepository<Membership> membershipRepository;

        [SetUp]
        public void SetUp()
        {
            context = new ClubMembershipContext(isUseInMemories: true);
            context.Set<Grade>().AddRange(new Grade[]
            {
                new Grade {Id = "K16", StartAt = DateTime.Now},
                new Grade {Id = "K17", StartAt = DateTime.Now},
            });
            context.Set<Student>().AddRange(new Student[] {
                new Student { Birthday = DateTime.Now, Id = 1, Name = "Test", Status = StudentStatus.IN_PROGRESS, GradeId = "K16"},
                new Student { Birthday = DateTime.Now, Id = 2, Name = "Test3", Status = StudentStatus.IN_PROGRESS, GradeId = "K16"},
                new Student { Birthday = DateTime.Now, Id = 3, Name = "Test2", Status = StudentStatus.IN_PROGRESS, GradeId = "K16"},
                new Student { Birthday = DateTime.Now, Id = 4, Name = "Test1", Status = StudentStatus.IN_PROGRESS, GradeId = "K16"}
            });
            context.Set<Club>().AddAsync(new Club { Id = 1, LogoUrl = "......", CreateAt = DateTime.Now, Name = "Test" });
            context.SaveChanges();
            membershipRepository = new MembershipRepository(context);
        }

        [Test]
        public async Task Should_assert_true_when_add_Membership()
        {
            var membershipToAdd = new Membership(1, 1)
            {
                JoinDate = DateTime.Now,
                Role = MemberRole.MEMBER,
                Status = MemberStatus.JOIN,
            };
            var added = await membershipRepository.AddAsync(membershipToAdd);
            var changes = await context.SaveChangesAsync();
            Assert.Multiple(() =>
            {
                Assert.That(changes > 0);
                Assert.That(added.Id == 11);
                Assert.That(added.ClubId == 1);
                Assert.That(added.StudentId == 1);
            });
        }
    }
}
