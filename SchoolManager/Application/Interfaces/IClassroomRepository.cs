using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.Domain.Models;

namespace SchoolManager.Application.Interfaces
{
    public interface IClassroomRepository
    {
        IReadOnlyList<Classroom> GetAll();
        Classroom? GetById(int id);

        IReadOnlyList<Classroom> GetByGradeId(int gradeId);

        void Add(Classroom classroom);
        void Update(Classroom classroom);
        void Delete(int id);
    }
}
