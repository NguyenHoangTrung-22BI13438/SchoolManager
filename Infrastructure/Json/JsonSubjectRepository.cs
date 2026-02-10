using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;

namespace SchoolManager.Infrastructure.Json
{
    public class JsonSubjectRepository
        : JsonRepositoryBase<Subject>, ISubjectRepository
    {
        public JsonSubjectRepository(string filePath) : base(filePath) { }

        public IReadOnlyList<Subject> GetAll()
            => Load();

        public Subject? GetById(int id)
            => Load().FirstOrDefault(s => s.Id == id);

        public void Add(Subject subject)
        {
            var subjects = Load();
            subjects.Add(subject);
            Save(subjects);
        }

        public void Update(Subject subject)
        {
            var subjects = Load();
            var index = subjects.FindIndex(s => s.Id == subject.Id);
            if (index >= 0)
            {
                subjects[index] = subject;
                Save(subjects);
            }
        }

        public void Delete(int id)
        {
            var subjects = Load();
            subjects.RemoveAll(s => s.Id == id);
            Save(subjects);
        }
    }
}