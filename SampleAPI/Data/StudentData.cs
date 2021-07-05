using SampleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Data
{
    public class StudentData : IStudent
    {
        private List<Student> lstStudent;

        public StudentData()
        {
            lstStudent = new List<Student>
            {
                new Student
                {
                    ID=1,
                    FirstMidName="Erick",
                    LastName="Kurniawan",
                    EnrollmentDate = DateTime.Now
                },
                new Student
                {
                    ID=2,
                    FirstMidName = "Adi",
                    LastName = "Nugroho",
                    EnrollmentDate = DateTime.Now
                }
            };
        }

        public void Delete(string id)
        {
            var result = GetById(id);
            if (result != null)
                lstStudent.Remove(result);
        }

        public IEnumerable<Student> GetAll()
        {
            return lstStudent;
        }

        public Student GetById(string id)
        {
            var result = (from std in lstStudent
                          where std.ID == Convert.ToInt32(id)
                          select std).FirstOrDefault();
            return result;
        }

        public IEnumerable<Student> GetByName(string studentname)
        {
            throw new NotImplementedException();
        }

        public void Insert(Student student)
        {
            lstStudent.Add(student);
        }

        public int InsertWithIndentity(Student obj)
        {
            throw new NotImplementedException();
        }

        public void Update(string id, Student student)
        {
            var result = GetById(id);
            if (result != null)
            {
                result.FirstMidName = student.FirstMidName;
                result.LastName = student.LastName;
                result.EnrollmentDate = student.EnrollmentDate;
            }
        }
    }
}