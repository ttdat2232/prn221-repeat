using Domain.Dtos;
using Domain.Dtos.Creates;
using Domain.Dtos.Updates;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.Services
{
    public interface IClubBoardService
    {
        Task<PaginationResult<ClubBoardDto>> GetClubBoardByClubIdAsync(long clubId);
        Task<ClubBoardDto> GetClubBoardByIdAsync(long id);
        Task<ClubBoardDto> AddClubBoardAsync(ClubBoardCreateDto clubBoard);
        Task<ClubBoardDto> UpdateClubBoardAsync(ClubBoardUpdateDto updateClubBoard);
    }
}
