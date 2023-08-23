namespace Domain.Entities
{
    public class Participant
    {
        public long ClubActivityId { get; set; }
        public long MembershipId { get; set; }
        public ParticipantStatus Status { get; set; }
        public virtual ClubActivity? ClubActivity { get; set; }
        public virtual Membership? Membership { get; set; }
    }
    public enum ParticipantStatus
    {
        ACTIVE = 0,
        LEAVE = 1,
    }
}
