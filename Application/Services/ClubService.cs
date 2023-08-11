using Application.Converters;
using Application.Exceptions;
using Domain.Dtos;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClubService : IClubService
    {
        private readonly IUnitOfWork unitOfWork;

        public ClubService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ClubDto> GetClubById(long id)
        {
            var result = await unitOfWork.Clubs
                .GetAsync(expression: c => c.Id == id, pageSize: 1, include: new string[] { nameof(Club.ClubBoards), nameof(Club.Memberships), nameof(Club.ClubActivities) })
                .ContinueWith(t => t.Result.Values.First() ?? throw new AppException($"Not found Club with ID: {id}"));
            return AppConverter.ToDto(result);
        }
    }
}
