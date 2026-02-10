using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Infrastructure.Json
{
    public class JsonClassroomRepository
        : JsonRepositoryBase<Classroom>, IClassroomRepository
    {
        public JsonClassroomRepository(string filePath) : base(filePath) { }

        public IReadOnlyList<Classroom> GetAll()
        => Load();
        public IReadOnlyList<Classroom> GetByGradeId(int gradeId)
        => Load().Where(c => c.GradeId == gradeId).ToList();

        public Classroom? GetById(int id)
        => Load().FirstOrDefault(c => c.Id == id);

        public void Add(Classroom classroom)
        {
            var classes = Load();
            classes.Add(classroom);
            Save(classes);
        }

        public void Update(Classroom classroom)
        {
            var classes = Load();
            var index = classes.FindIndex(c => c.Id == classroom.Id);
            if (index >= 0)
            {
                classes[index] = classroom;
                Save(classes);
            }
        }

        public void Delete(int id)
        {
            var classes = Load();
            classes.RemoveAll(c => c.Id == id);
            Save(classes);
        }
    }
}