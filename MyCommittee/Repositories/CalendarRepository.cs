using MyCommittee.Data;
using MyCommittee.Models;

namespace MyCommittee.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly ApplicationDbContext _context;

        public CalendarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Calendar> GetAll()
        {
            return _context.Calendars.ToList();
        }

        public Calendar? GetById(int id)
        {
            return _context.Calendars.Find(id);
        }

        public void Add(Calendar calendar)
        {
            _context.Calendars.Add(calendar);
            _context.SaveChanges();
        }

        public void Update(Calendar calendar)
        {
            _context.Calendars.Update(calendar);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var cal = _context.Calendars.Find(id);
            if (cal != null)
            {
                _context.Calendars.Remove(cal);
                _context.SaveChanges();
            }
        }
    }
}
