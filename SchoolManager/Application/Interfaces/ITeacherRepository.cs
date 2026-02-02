using SchoolManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManager.Application.Interfaces
{
    public interface ITeacherRepository
    {
        IReadOnlyList<Teacher> GetAll();
        Teacher? GetById(int id);

        void Add(Teacher teacher);
        void Update(Teacher teacher);
        void Delete(int id);
    }
}
