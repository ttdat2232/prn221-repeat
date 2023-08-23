using Application.Converters;
using Application.Exceptions;
using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Entities;
using Domain.Interfaces.Adapters;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Models;

namespace Application.Services
{
    public class ClubService : IClubService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IMembershipService membershipService;

        public ClubService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService, IMembershipService membershipService)
        {
            this.unitOfWork = unitOfWork;
            this.cloudinaryService = cloudinaryService;
            this.membershipService = membershipService;
        }

        public async Task<ClubDto> AddClubAsync(ClubCreateDto club)
        {
            var existed = await unitOfWork.Clubs.GetAsync(expression: c => c.Name.ToLower().Equals(club.Name.Trim().ToLower()));
            if (existed.Values.Count > 0)
                throw new AppException("Club's name is already existed");
            var entityToAdd = AppConverter.ToEntity(club);
            var president = await unitOfWork.Students.GetById(club.PresidentId);
            if (club.Image != null)
            {
                var logoURL = await cloudinaryService.UploadAsync(club.Image);
                entityToAdd.LogoUrl = logoURL;
            }
            try
            {
                entityToAdd = await unitOfWork.Clubs.AddAsync(entityToAdd);
                if (await unitOfWork.CompleteAsync() > 0)
                {
                    var presidentMember = new MembershipCreateDto()
                    {
                        ClubId = entityToAdd.Id,
                        Role = MemberRole.PRESIDENT,
                        StudentId = president.Id,
                    };
                    await membershipService.AddMemberShipAsync(presidentMember);
                    return AppConverter.ToDto(entityToAdd);
                }
                else
                    throw new AppException("Added failed");
            }
            catch (Exception)
            {
                throw new AppException("Error occurred");
            }
        }

        public async Task DeleteClubAsync(long id)
        {
            try
            {
                await unitOfWork.Clubs.DeleteAsync(id: id);
                if (await unitOfWork.CompleteAsync() <= 0)
                    throw new AppException("Deleted Failed");
            }
            catch (Exception)
            {
                throw new AppException("Error occurred");
            }
        }

        public async Task<ClubDto> GetClubByIdAsync(long id)
        {
            try
            {
                var result = await unitOfWork.Clubs.GetById(id);
                return AppConverter.ToDto(result);
            }
            catch (KeyNotFoundException)
            {
                throw new NotFoundException(entityNotFound: typeof(Club), id: id, classThrowException: GetType());
            }
            catch (Exception)
            {
                throw new AppException("Error occurred");
            }
        }

        public async Task<PaginationResult<ClubDto>> GetClubsAsync(int pageSize = 4, int pageIndex = 0)
        {
            var result = await unitOfWork.Clubs.GetAsync(pageIndex: pageIndex, pageSize: pageSize);
            return new PaginationResult<ClubDto>
            {
                PageCount = result.PageCount,
                PageIndex = result.PageIndex,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Values = result.Values.Select(c => AppConverter.ToDto(c)).ToList(),
            };
        }

        public async Task UpdateClubAsync(ClubUpdateDto club)
        {
            var entityToUpdate = await unitOfWork.Clubs.GetAsync(expression: c => c.Id == club.Id)
                .ContinueWith(t => t.Result.Values.Any() ? t.Result.Values.First() : throw new NotFoundException(typeof(Club), club.Id, GetType()));
            if (club.Name != null && club.Name.Trim() != string.Empty)
                entityToUpdate.Name = club.Name;
            if (club.Image != null)
            {
                await cloudinaryService.UploadAsync(club.Image).ContinueWith(t =>
                {
                    entityToUpdate.LogoUrl = t.Result;
                });
            }
            try
            {
                entityToUpdate = unitOfWork.Clubs.Update(entityToUpdate);
                if (await unitOfWork.CompleteAsync() <= 0)
                    throw new AppException("Updated fail");
            }
            catch (Exception)
            {
                throw new AppException("Error occurred");
            }
        }
    }
}
