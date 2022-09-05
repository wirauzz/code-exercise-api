using Logic.Managers.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Presenation.Controllers
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
            return Ok(await _studentManager.AllStudents());
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult> GetStudent([FromRoute, Required] Guid studentId)
        {
            return Ok(await _studentManager.StudentById(studentId));
        }

        [HttpPost]
        public async Task<ActionResult> PostStudent([FromBody, Required] StudentDTO studentDTO)
        {
            return Ok(await _studentManager.Create(studentDTO));
        }

        [HttpDelete("{studentId}")]
        public async Task<ActionResult> DeleteStudent([FromRoute, Required] Guid studentId)
        {
            return Ok(await _studentManager.DeleteStudent(studentId));
        }

        [HttpPut]
        [Route("{assetId}")]
        public async Task<ActionResult> PutAsset([FromRoute, Required] Guid studentId, [FromBody, Required] StudentDTO student)
        {
            return Ok(await _studentManager.UpdateById(studentId, student));
        }
    }
}
