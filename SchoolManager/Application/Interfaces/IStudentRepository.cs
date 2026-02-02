using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Application.Interfaces
{
    public interface IStudentRepository
    {
        IReadOnlyList<Student> GetAll();
        Student? GetById(int id);

        void Add(Student student);
        void Update(Student student);
        void Delete(int id);
    }
}
