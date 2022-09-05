using Logic.Managers.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middleware;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/students")]
    public class StudentController : Controller
    {
        private readonly IStudentsManager _studentManager;

        public StudentController(IStudentsManager studentsManager)
        {
            _studentManager = studentsManager;
        }

        [HttpGet]
        public async Task<ActionResult> GeAllStudens()
        {
            return Ok(new MiddlewareResponse<IEnumerable<StudentDTO>>(await _studentManager.AllStudents()));
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult> GetStudent([FromRoute, Required] Guid studentId)
        {
            return Ok(new MiddlewareResponse<StudentDTO>(await _studentManager.StudentById(studentId)));
        }

        [HttpPost]
        public async Task<ActionResult> PostStudent([FromBody, Required] StudentDTO studentDTO)
        {
            return Ok(new MiddlewareResponse<StudentDTO>(await _studentManager.Create(studentDTO)));
        }

        [HttpDelete("{studentId}")]
        public async Task<ActionResult> DeleteStudent([FromRoute, Required] Guid studentId)
        {
            return Ok(new MiddlewareResponse<bool>(await _studentManager.DeleteStudent(studentId)));
        }

        [HttpPut]
        [Route("{studentId}")]
        public async Task<ActionResult> PutStudent([FromRoute, Required] Guid studentId, [FromBody, Required] StudentDTO student)
        {
            return Ok(new MiddlewareResponse<StudentDTO>(await _studentManager.UpdateById(studentId, student)));
        }
    }
}
