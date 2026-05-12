using MyCommittee.Models;

namespace MyCommittee.Repositories
{
    public interface ICommitteeRepository
    {
        IEnumerable<Committee> GetAll();
        Committee? GetById(int id);
        void Add(Committee committee);
        void Update(Committee committee);
        void Delete(int id);
    }
}
