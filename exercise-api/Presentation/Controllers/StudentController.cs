using Logic.Managers.Interfaces;
using Logic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Middleware;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
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

        /// <summary>
        /// Returns all Students
        /// </summary>
        /// <remarks>Get all Students</remarks>
        /// <response code="200">Success or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<IEnumerable<StudentDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GeAllStudens()
        {
            return Ok(new MiddlewareResponse<IEnumerable<StudentDTO>>(await _studentManager.AllStudents()));
        }

        /// <summary>
        /// Returns a specific Student
        /// </summary>
        /// <remarks>Get Student by Id, Student id must be a unique identifier</remarks>
        /// <response code="200">Success or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<StudentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{studentId}")]
        public async Task<ActionResult> GetStudent([FromRoute, Required] Guid studentId)
        {
            return Ok(new MiddlewareResponse<StudentDTO>(await _studentManager.StudentById(studentId)));
        }

        /// <summary>
        /// Creates a new Student and returns it as it is in DB
        /// </summary>
        /// <param name="studentDTO">
        /// The new Student with its information
        /// ## Body 
        /// 
        ///     {
        ///        "firstName" : "Wilson",
        ///        "lastName" : "Ramirez",
        ///     }
        /// </param>
        /// <response code="200">Success or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<StudentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<ActionResult> PostStudent([FromBody, Required] StudentDTO studentDTO)
        {
            return Ok(new MiddlewareResponse<StudentDTO>(await _studentManager.Create(studentDTO)));
        }

        /// <summary>
        /// Deletes a specific Student
        /// </summary>
        /// <param name="studentId">Id of the Student to delete.</param>
        /// <remarks>Delete Student by Id. Student id is a unique identifier</remarks>
        /// <response code="200">True on success, or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{studentId}")]
        public async Task<ActionResult> DeleteStudent([FromRoute, Required] Guid studentId)
        {
            return Ok(new MiddlewareResponse<bool>(await _studentManager.DeleteStudent(studentId)));
        }

        /// <summary>
        /// Updates an Student and returns it as it is in DB
        /// </summary>
        /// <param name="studentId">unique identifier of Student id</param>
        /// <param name="student">
        /// Student data to update
        /// ## Body 
        /// 
        ///     {
        ///        "firstName" : "Wilson",
        ///        "lastName" : "Ramirez",
        ///     }
        /// </param>
        /// <response code="200">Success or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<StudentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut]
        [Route("{studentId}")]
        public async Task<ActionResult> PutStudent([FromRoute, Required] Guid studentId, [FromBody, Required] StudentDTO student)
        {
            return Ok(new MiddlewareResponse<StudentDTO>(await _studentManager.UpdateById(studentId, student)));
        }

        /// <summary>
        /// Search a student by the first name.
        /// </summary>
        /// <param name="firstName">First name of the user.</param>
        /// <returns>Returns student information found by first name</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<IEnumerable<StudentDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("search")]
        public async Task<IActionResult> GetProfileByUserName([FromQuery] string firstName)
        {
            return Ok(new MiddlewareResponse<IEnumerable<StudentDTO>>(await _studentManager.SearchStudentByName(firstName)));
        }
    }
}
