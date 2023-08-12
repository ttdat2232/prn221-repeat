using Application.Exceptions;
using Application.Services;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.VisualBasic;
using Repositories.Repository;
using System.Data;

namespace ServiceTest
{
    [TestFixture]
    public class MembershipServiceTest : BaseSetUp
    {
        public static object[] NotExistMembers = 
        {
            new object[] { 1000, MemberStatus.JOIN },
            new object[] { 2, MemberStatus.LEAVE }
        };

        [Test]
        public async Task Should_return_true_when_checking_setup()
        {
            var students = await unitOfWork.Students.GetAsync(isTakeAll: true, include: new string[] {nameof(Student.Grade), nameof(Student.Major)}).ContinueWith(t => t.Result);
            bool hasGrade = students.Values[0].Grade != null;
            bool hasMajor = students.Values[0].Major != null;
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(unitOfWork);
                Assert.IsTrue(students.TotalCount == 10);
                Assert.IsTrue(hasGrade);
                Assert.IsTrue(hasMajor);
            });
        }

        [Test, Category("Adding")]
        public async Task Should_return_true_when_add_membership()
        {
            MembershipCreateDto dto = new MembershipCreateDto
            {
                Role = MemberRole.MEMBER,
                StudentId = 1,
                ClubId = 1
            };
            var newMember = await membershipService.AddMemberShipAsync(dto);
            Assert.That(newMember.Id == 11);
        }

        [Test, Category("Adding")]
        public void Should_thrown_AppException_when_ClubId_is_not_valid()
        {
            MembershipCreateDto dto = new MembershipCreateDto
            {
                Role = MemberRole.MEMBER,
                StudentId = 1,
                ClubId = 100
            };
            Assert.ThrowsAsync<AppException>(async () => await membershipService.AddMemberShipAsync(dto));
        }

        [Test, Category("Adding")]
        public void Should_thrown_AppException_when_StudentId_is_not_valid()
        {
            MembershipCreateDto dto = new MembershipCreateDto
            {
                Role = MemberRole.MEMBER,
                StudentId = 1000,
                ClubId = 100
            };
            Assert.ThrowsAsync<AppException>(async () => await membershipService.AddMemberShipAsync(dto));
        }
        [Test, Category("Adding")]
        public void Should_thrown_AppException_when_try_adding_student_already_in_club()
        {
            MembershipCreateDto dto = new MembershipCreateDto
            {
                Role = MemberRole.MEMBER,
                StudentId = 2,
                ClubId = 1
            };
            Assert.ThrowsAsync<AppException>(async () => await membershipService.AddMemberShipAsync(dto));
        }

        [Test]
        public async Task Should_assert_true_when_get_memberships()
        {
            var id = 12;
            var result = await membershipService.GetMemberShipByIdAsync(id);
            Assert.That(result != null && result.Id == id);
        }

        [Test]
        [TestCaseSource(nameof(NotExistMembers))]
        public void Should_throw_exception_when_get_invalid_member(int id, MemberStatus status)
        {
            Assert.ThrowsAsync<AppException>(async () => await membershipService.GetMemberShipByIdAsync(id, status));
        }

        [Test, Category("Update")]
        public async Task Should_assert_true_when_update_member()
        {
            var id = 12;
            var date = DateTime.Now.AddDays(1);
            var memberToUpdate = new MembershipUpdateDto
            {
                Id = id,
                JoinDate = date,
                LeaveDate = date,
                Status = MemberStatus.LEAVE
            };
            var result = await membershipService.UpdateMembershipAsync(memberToUpdate);
            var updated = await unitOfWork.Memberships.GetAsync(expression: m => m.Id == id).ContinueWith(t => t.Result.Values.Single());
            Assert.Multiple(() => 
            {
                Assert.That(updated.JoinDate == date);
                Assert.That(updated.LeaveDate == date);
                Assert.That(updated.LeaveDate == date);
                Assert.That(updated.Status == MemberStatus.LEAVE);
            });
        }
        [Test, Category("Update")]
        public void Should_throw_AppException_when_update_not_exist_member()
        {
            var memberToUpdate = new MembershipUpdateDto
            {
                Id = 1000
            };
            Assert.ThrowsAsync<AppException>(async () => await membershipService.UpdateMembershipAsync(memberToUpdate));
        }
        [Test, Category("Delete")]
        public async Task Should_return_true_when_delete_member()
        {
            var id = 13;
            await membershipService.DeleteMembershipAsync(id);
            bool mustBeDeleted = await unitOfWork.Memberships.GetAsync(expression: m => m.Id == id).ContinueWith(t => t.Result.Values.Count == 0);
            Assert.IsTrue(mustBeDeleted);
        }

    }
}