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
        IReadOnlyList<Classrooms> GetAll();
        Classrooms? GetById(int id);

        void Add(Classrooms classroom);
        void Update(Classrooms classroom);
        void Delete(int id);
    }
}
