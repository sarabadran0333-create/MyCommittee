using MyCommittee.Data;
using MyCommittee.Models;

namespace MyCommittee.Repositories
{
    public class CommitteeRepository : ICommitteeRepository
    {
        private readonly ApplicationDbContext _context;

        public CommitteeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Committee> GetAll()
        {
            return _context.Committees.ToList();
        }

        public Committee? GetById(int id)
        {
            return _context.Committees.Find(id);
        }

        public void Add(Committee committee)
        {
            _context.Committees.Add(committee);
            _context.SaveChanges();
        }

        public void Update(Committee committee)
        {
            _context.Committees.Update(committee);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var committee = _context.Committees.Find(id);
            if (committee != null)
            {
                _context.Committees.Remove(committee);
                _context.SaveChanges();
            }
        }
    }
}
