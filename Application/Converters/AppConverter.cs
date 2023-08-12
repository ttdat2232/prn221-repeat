using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Application.Converters
{
    internal static class AppConverter
    {
        public static MembershipDto ToDto(Membership entity)
        {
            var result = new MembershipDto
            {
                Id = entity.Id,
                ClubName = entity.Club?.Name ?? "",
                Name = entity.Student?.Name ?? "",
                StudentId = entity.StudentId,
                Status = entity.Status,
                JoinDate = entity.JoinDate,
                Role = entity.Role
            };
            if(entity.LeaveDate.HasValue)
                result.JoinDate = entity.LeaveDate.Value;
            return result;
        }

        public static Membership ToEntity(MembershipCreateDto dto)
        {
            var membership = new Membership(studentId: dto.StudentId, clubId: dto.ClubId)
            {
                JoinDate = DateTime.Now,
                Role = dto.Role,
                Status = MemberStatus.JOIN,
            };
            return membership;
        }

        public static void ToEntity(MembershipUpdateDto dto, ref Membership entity)
        {
            entity.Id = dto.Id;
            entity.JoinDate = dto.JoinDate.HasValue ? dto.JoinDate.Value : entity.JoinDate;
            entity.LeaveDate = dto.LeaveDate.HasValue ? dto.LeaveDate.Value : entity.LeaveDate;
            entity.Status = dto.Status.HasValue ? dto.Status.Value : entity.Status;
            entity.Role = dto.Role.HasValue ? dto.Role.Value : entity.Role;
        }

        public static ClubDto ToDto(Club entity)
        {
            var clubBoards = new List<ClubBoardDto>();
            if(entity.ClubBoards != null && entity.ClubBoards.Count > 0)
            {
                clubBoards.AddRange(entity.ClubBoards.Select(cb => ToDto(cb)));
            }
            return new ClubDto
            {
                Id = entity.Id,
                CreateAt = entity.CreateAt,
                LogoUrl = entity.LogoUrl,
                Name = entity.Name,
                ClubBoardIds = clubBoards.Select(cb => cb.Id).ToList(),
                ClubBoards = clubBoards
            };
        }

        public static ClubBoardDto ToDto(ClubBoard entity)
        {
            return new ClubBoardDto
            {
                Id = entity.Id,
                Name = entity.Name,
                ClubId = entity.ClubId,
            };
        }

        public static StudentDto ToDto(Student entity)
        {
            return new StudentDto
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
