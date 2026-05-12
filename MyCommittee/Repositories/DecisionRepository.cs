using MyCommittee.Data;
using MyCommittee.Models;

namespace MyCommittee.Repositories
{
    public class DecisionRepository : IDecisionRepository
    {
        private readonly ApplicationDbContext _context;

        public DecisionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Decision> GetAll()
        {
            return _context.Decisions.ToList();
        }

        public Decision? GetById(int id)
        {
            return _context.Decisions.Find(id);
        }

        public void Add(Decision decision)
        {
            _context.Decisions.Add(decision);
            _context.SaveChanges();
        }

        public void Update(Decision decision)
        {
            _context.Decisions.Update(decision);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var dec = _context.Decisions.Find(id);
            if (dec != null)
            {
                _context.Decisions.Remove(dec);
                _context.SaveChanges();
            }
        }
    }
}
