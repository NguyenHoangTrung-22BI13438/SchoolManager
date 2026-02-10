using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.Domain.Models;

namespace SchoolManager.Application.Interfaces
{
    
    public interface IClassroomSubjectRepository
    {
        IReadOnlyList<ClassroomSubject> GetAll();
        ClassroomSubject? GetById(int id);

        IReadOnlyList<ClassroomSubject> GetByClassroomId(int classroomId);
        IReadOnlyList<ClassroomSubject> GetBySubjectId(int subjectId);
        IReadOnlyList<ClassroomSubject> GetByTeacherId(int teacherId);

        void Add(ClassroomSubject assignment);
        void Update(ClassroomSubject assignment);
        void Delete(int id);
    }
}
