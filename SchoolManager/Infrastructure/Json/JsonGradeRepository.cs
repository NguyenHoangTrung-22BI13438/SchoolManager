using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Infrastructure.Json
{
    public class JsonGradeRepository
        : JsonRepositoryBase<Grade>, IGradeRepository
    {
        public JsonGradeRepository(string filePath) : base(filePath) { }

        public IReadOnlyList<Grade> GetAll()
            => Load();

        public Grade? GetById(int id)
            => Load().FirstOrDefault(g => g.Id == id);

        public IReadOnlyList<Grade> GetByStudentId(int studentId)
            => Load().Where(g => g.StudentId == studentId).ToList();

        public IReadOnlyList<Grade> GetBySubjectId(int subjectId)
            => Load().Where(g => g.SubjectId == subjectId).ToList();

        public void Add(Grade grade)
        {
            var grades = Load();
            grades.Add(grade);
            Save(grades);
        }

        public void Update(Grade grade)
        {
            var grades = Load();
            var index = grades.FindIndex(g => g.Id == grade.Id);
            if (index >= 0)
            {
                grades[index] = grade;
                Save(grades);
            }
        }

        public void Delete(int id)
        {
            var grades = Load();
            grades.RemoveAll(g => g.Id == id);
            Save(grades);
        }
    }
}