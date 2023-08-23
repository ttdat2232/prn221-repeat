namespace Domain.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IMembershipRepository Memberships { get; }
        IClubActivityRepository ClubActivities { get; }
        IClubRepository Clubs { get; }
        IStudentRepository Students { get; }
        IGradeRepository Grades { get; }
        IMajorRepository Majors { get; }
        IParticipantRepository Participants { get; }
        IClubBoardRepository ClubBoards { get; }
        int Complete();
        Task<int> CompleteAsync();
    }
}
