using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;

namespace SchoolManager.Infrastructure.Json
{
    public class JsonClassroomSubjectRepository
        : JsonRepositoryBase<ClassroomSubject>, IClassroomSubjectRepository
    {
        public JsonClassroomSubjectRepository(string filePath) : base(filePath) { }

        public IReadOnlyList<ClassroomSubject> GetAll()
            => Load();

        public ClassroomSubject? GetById(int id)
            => Load().FirstOrDefault(cs => cs.Id == id);

        public IReadOnlyList<ClassroomSubject> GetByClassroomId(int classroomId)
            => Load().Where(cs => cs.ClassroomId == classroomId).ToList();

        public IReadOnlyList<ClassroomSubject> GetBySubjectId(int subjectId)
            => Load().Where(cs => cs.SubjectId == subjectId).ToList();

        public IReadOnlyList<ClassroomSubject> GetByTeacherId(int teacherId)
            => Load().Where(cs => cs.TeacherId == teacherId).ToList();

        public void Add(ClassroomSubject assignment)
        {
            var assignments = Load();

            // Auto-generate ID if not set
            if (assignment.Id == 0)
            {
                assignment.Id = assignments.Count > 0
                    ? assignments.Max(a => a.Id) + 1
                    : 1;
            }

            assignments.Add(assignment);
            Save(assignments);
        }

        public void Update(ClassroomSubject assignment)
        {
            var assignments = Load();
            var index = assignments.FindIndex(a => a.Id == assignment.Id);
            if (index >= 0)
            {
                assignments[index] = assignment;
                Save(assignments);
            }
        }

        public void Delete(int id)
        {
            var assignments = Load();
            assignments.RemoveAll(a => a.Id == id);
            Save(assignments);
        }
    }
}
