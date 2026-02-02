using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.Application.Interfaces;

namespace SchoolManager.Application.Services
{
    public class SubjectService
    {
        private readonly ISubjectRepository _subjects;

        public SubjectService(ISubjectRepository subjects)
        {
            _subjects = subjects;
        }

        public void AssignTeacher(int subjectId, int teacherId)
        {
            var subject = _subjects.GetById(subjectId)
                ?? throw new Exception("Subject not found");

            subject.TeacherId = teacherId;
            _subjects.Update(subject);
        }
    }
}
