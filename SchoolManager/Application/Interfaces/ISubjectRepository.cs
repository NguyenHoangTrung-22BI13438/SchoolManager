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
        IReadOnlyList<Subjects> GetAll();
        Subjects? GetById(int id);

        void Add(Subjects subject);
        void Update(Subjects subject);
        void Delete(int id);
    }
}
