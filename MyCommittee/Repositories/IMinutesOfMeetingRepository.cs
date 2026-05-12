using MyCommittee.Models;

namespace MyCommittee.Repositories
{
    public interface IMinutesOfMeetingRepository
    {
        IEnumerable<MinutesOfMeeting> GetAll();
        MinutesOfMeeting? GetById(int id);
        void Add(MinutesOfMeeting minutes);
        void Update(MinutesOfMeeting minutes);
        void Delete(int id);
    }
}
