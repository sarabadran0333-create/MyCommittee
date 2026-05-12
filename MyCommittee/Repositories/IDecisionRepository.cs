using MyCommittee.Models;

namespace MyCommittee.Repositories
{
    public interface IDecisionRepository
    {
        IEnumerable<Decision> GetAll();
        Decision? GetById(int id);
        void Add(Decision decision);
        void Update(Decision decision);
        void Delete(int id);
    }
}
