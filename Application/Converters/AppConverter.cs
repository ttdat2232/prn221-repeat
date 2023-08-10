using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Application.Converters
{
    internal static class AppConverter
    {
        public static MembershipDto ToDto(Membership entity)
        {
            return new MembershipDto
            {
                Id = entity.Id,
                ClubName = entity.Club?.Name ?? "",
                Name = entity.Student?.Name ?? "",
                StudentId = entity.StudentId
            };
        }

        public static Membership ToEntity(MembershipCreateDto dto)
        {
            var clubBoards = new List<ClubBoard>();
            if (dto.ClubBoardId != null && dto.ClubBoardId.Length > 0)
                foreach (var item in dto.ClubBoardId)
                    clubBoards.Add(new ClubBoard() { Id = item });
            var membership = new Membership(studentId: dto.StudentId, clubId: dto.ClubId)
            {
                JoinDate = DateTime.Now,
                Role = dto.Role,
                Status = MemberStatus.JOIN,
            };
            membership.ClubBoards = clubBoards;
            return membership;
        }

        public static void ToEntity(MembershipUpdateDto dto, ref Membership entity)
        {
            entity.Id = dto.Id;
            entity.JoinDate = dto.JoinDate.HasValue ? dto.JoinDate.Value : entity.JoinDate;
            entity.LeaveDate = dto.LeaveDate.HasValue ? dto.LeaveDate.Value : entity.LeaveDate;
            entity.Status = dto.MemberStatus.HasValue ? dto.MemberStatus.Value : entity.Status;
            entity.Role = dto.Role.HasValue ? dto.Role.Value : entity.Role;
        }
    }
}
