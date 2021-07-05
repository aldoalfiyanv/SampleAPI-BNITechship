using SampleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Data
{
    public interface IStudent : ICrud<Student>
    {
        IEnumerable<Student> GetByName(string studentname);
        int InsertWithIndentity(Student obj);
    }
}