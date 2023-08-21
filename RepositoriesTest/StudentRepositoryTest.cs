using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Base;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Repositories.Repository;
using Repository.Models;

namespace RepositoriesTest
{
    public class StudentRepositoryTest
    {
        ClubMembershipContext context;
        IRepository<Student> studentRepository;
        public static IEnumerable<TestCaseData> NewStudents
        {
            get
            {
                yield return new TestCaseData(new Student { Birthday = DateTime.Now, GradeId = "K16", Name = $"Name {Guid.NewGuid}", Status = StudentStatus.IN_PROGRESS});
                yield return new TestCaseData(new Student { Birthday = DateTime.Now, GradeId = "K16", Name = $"Name {Guid.NewGuid}", Status = StudentStatus.IN_PROGRESS});
                yield return new TestCaseData(new Student { Birthday = DateTime.Now, GradeId = "K16", Name = $"Name {Guid.NewGuid}", Status = StudentStatus.IN_PROGRESS});
                yield return new TestCaseData(new Student { Birthday = DateTime.Now, GradeId = "K16", Name = $"Name {Guid.NewGuid}", Status = StudentStatus.IN_PROGRESS});
            }
        }
        [SetUp]
        public void Setup()
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
            context.SaveChanges();
            studentRepository = new StudentRepository(context);
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
        [Test]
        public async Task Should_assert_true_when_get_all_students()
        {
            var expected = context.Set<Student>().Count();
            var actual = await studentRepository.GetAsync(isTakeAll: true).ContinueWith(t => t.Result.TotalCount);
            Assert.That(actual == expected, $"expected is {expected} but actual is {actual}");
        }

        [Test]
        [TestCaseSource(nameof(NewStudents))]
        public async Task Should_assert_true_when_add_students(Student student)
        {
            var result = await studentRepository.AddAsync(student);
            var changes = await context.SaveChangesAsync();
            Assert.Multiple(() =>
            {
                Assert.That(changes > 0);
                Assert.That(result.Id != 0);
                Assert.That(result.Name.Equals(student.Name));
                Assert.That(result.Status.Equals(student.Status));
                Assert.That(result.GradeId.Equals(student.GradeId));
            });
        }
        [Test]
        public async Task Should_return_empty_list_when_get_student()
        {
            var actual = await studentRepository.GetAsync(expression: s => s.Id == 10000).ContinueWith(t => t.Result.TotalCount);
            Assert.That(actual == 0, () => Notification(0, actual));
        }
        private string Notification(object expected, object actual)
        {
            return $"expected is {expected} but actual is {actual}";
        }

        [Test]
        public async Task Should_assert_true_when_update_student()
        {
            var studentToUpdate = await context.Set<Student>().Where(m => m.Id == 1).FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Cannot found");
            var newName = "Test name change";
            studentToUpdate.Name = newName;
            context.ChangeTracker.Clear();
            var updated = studentRepository.Update(studentToUpdate);
            var changes = await context.SaveChangesAsync();
            Assert.That(changes > 0, () => Notification(">1", changes));
        }
    }
}