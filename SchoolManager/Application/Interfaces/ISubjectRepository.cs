using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Application.Interfaces
{
    public interface ISubjectRepository
    {
        IReadOnlyList<Subject> GetAll();
        Subject? GetById(int Id);
        IReadOnlyList<Subject> GetByTeacherId(int teacherId);
        void Add(Subject subject);
        void Update(Subject subject);
        void Delete(int id);
    }
}
