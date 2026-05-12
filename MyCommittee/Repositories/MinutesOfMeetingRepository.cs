using MyCommittee.Data;
using MyCommittee.Models;

namespace MyCommittee.Repositories
{
    public class MinutesOfMeetingRepository : IMinutesOfMeetingRepository
    {
        private readonly ApplicationDbContext _context;

        public MinutesOfMeetingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<MinutesOfMeeting> GetAll()
        {
            return _context.MinutesOfMeetings.ToList();
        }

        public MinutesOfMeeting? GetById(int id)
        {
            return _context.MinutesOfMeetings.Find(id);
        }

        public void Add(MinutesOfMeeting minutes)
        {
            _context.MinutesOfMeetings.Add(minutes);
            _context.SaveChanges();
        }

        public void Update(MinutesOfMeeting minutes)
        {
            _context.MinutesOfMeetings.Update(minutes);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var m = _context.MinutesOfMeetings.Find(id);
            if (m != null)
            {
                _context.MinutesOfMeetings.Remove(m);
                _context.SaveChanges();
            }
        }
    }
}
