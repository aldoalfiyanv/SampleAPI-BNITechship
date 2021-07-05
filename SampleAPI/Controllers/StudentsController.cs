using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleAPI.Data;
using SampleAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private IStudent _student;
        public StudentsController(IStudent student)
        {
            _student = student;
        }

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            return _student.GetAll();
        }

        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return _student.GetById(id.ToString());
        }

        [HttpGet]
        [Route("StudentByName/{name}")]
        public IEnumerable<Student> StudentByName(string name)
        {
            return _student.GetByName(name);
        }

        [HttpGet]
        [Route("StudentQuery")]
        public string StudentQuery([FromQuery] Student nama)
        {
            return $"{nama.FirstMidName} {nama.LastName}";
        }

        [HttpPost]
        public IActionResult Post([FromBody] Student student)
        {
            try
            {
                _student.Insert(student);
                return Ok($"Berhasil menambahkan data student {student.FirstMidName} {student.LastName}");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPost]
        [Route("PostWithID")]
        public IActionResult PostWithID([FromBody] Student student)
        {
            try
            {
                var result = _student.InsertWithIndentity(student);
                return Ok($"Berhasil menambahkan Student ID: {result}");
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Student student)
        {
            try
            {
                _student.Update(id.ToString(), student);
                return Ok($"Data student id: {id} berhasil diupdate");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _student.Delete(id.ToString());
                return Ok($"Data student id:{id} berhasil didelete");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}