using MyCommittee.Models;

namespace MyCommittee.Repositories
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAll();
        Member? GetById(int id);
        void Add(Member member);
        void Update(Member member);
        void Delete(int id);
        bool Exists(int id);
    }
}
