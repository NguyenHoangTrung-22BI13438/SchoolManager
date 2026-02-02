using SchoolManager.Application.Interfaces;
using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Application.Services
{
    public class TeacherService
    {
        private readonly ITeacherRepository _teachers;
        private readonly ISubjectRepository _subjects;

        public TeacherService(
            ITeacherRepository teachers,
            ISubjectRepository subjects)
        {
            _teachers = teachers;
            _subjects = subjects;
        }

        public IReadOnlyList<Subject> GetSubjects(int teacherId)
        {
            return _subjects.GetByTeacherId(teacherId);
        }
    }
}
