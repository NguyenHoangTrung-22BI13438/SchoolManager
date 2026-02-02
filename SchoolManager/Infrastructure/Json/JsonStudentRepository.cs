using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Infrastructure.Json
{
    public class JsonStudentRepository
        : JsonRepositoryBase<Student>, IStudentRepository
    {
        public JsonStudentRepository(string filePath) : base(filePath) { }

        public IReadOnlyList<Student> GetAll()
            => Load();

        public Student? GetById(int id)
            => Load().FirstOrDefault(s => s.Id == id);

        public void Add(Student student)
        {
            var students = Load();
            students.Add(student);
            Save(students);
        }

        public void Update(Student student)
        {
            var students = Load();
            var index = students.FindIndex(s => s.Id == student.Id);
            if (index >= 0)
            {
                students[index] = student;
                Save(students);
            }
        }

        public void Delete(int id)
        {
            var students = Load();
            students.RemoveAll(s => s.Id == id);
            Save(students);
        }
    }
}
