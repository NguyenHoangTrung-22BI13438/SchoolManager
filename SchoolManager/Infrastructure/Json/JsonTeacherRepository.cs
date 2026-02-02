using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Infrastructure.Json
{
    public class JsonTeacherRepository
        : JsonRepositoryBase<Teacher>, ITeacherRepository
    {
        public JsonTeacherRepository(string filePath) : base(filePath) { }

        public IReadOnlyList<Teacher> GetAll()
            => Load();

        public Teacher? GetById(int id)
            => Load().FirstOrDefault(t => t.Id == id);

        public void Add(Teacher teacher)
        {
            var teachers = Load();
            teachers.Add(teacher);
            Save(teachers);
        }

        public void Update(Teacher teacher)
        {
            var teachers = Load();
            var index = teachers.FindIndex(t => t.Id == teacher.Id);
            if (index >= 0)
            {
                teachers[index] = teacher;
                Save(teachers);
            }
        }

        public void Delete(int id)
        {
            var teachers = Load();
            teachers.RemoveAll(t => t.Id == id);
            Save(teachers);
        }
    }
}
