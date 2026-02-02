using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManager.Domain.Models;

namespace SchoolManager.Application.Interfaces
{
    public interface IGradeRepository
    {
        IReadOnlyList<Grade> GetAll();
        Grade? GetById(int id);

        void Add(Grade grade);
        void Update(Grade grade);
        void Delete(int id);
    }
}
