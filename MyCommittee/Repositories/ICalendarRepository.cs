using MyCommittee.Models;

namespace MyCommittee.Repositories
{
    public interface ICalendarRepository
    {
        IEnumerable<Calendar> GetAll();
        Calendar? GetById(int id);
        void Add(Calendar calendar);
        void Update(Calendar calendar);
        void Delete(int id);
    }
}
