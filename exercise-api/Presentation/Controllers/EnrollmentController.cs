using Data.Models;
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
    [Route("api/enrollments")]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentManager _enrollmentManager;

        public EnrollmentController(IEnrollmentManager enrollmentsManager)
        {
            _enrollmentManager = enrollmentsManager;
        }

        /// <summary>
        /// Returns all Enrollments
        /// </summary>
        /// <remarks>Get all Enrollments</remarks>
        /// <response code="200">Success or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<IEnumerable<EnrollmentDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GeAllStudens()
        {
            return Ok(new MiddlewareResponse<IEnumerable<EnrollmentDTO>>(await _enrollmentManager.AllEnrollments()));
        }

        /// <summary>
        /// Returns a specific Enrollment
        /// </summary>
        /// <remarks>Get Enrollment by Id, Enrollment id must be a unique identifier</remarks>
        /// <response code="200">Success or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<EnrollmentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{enrollmentId}")]
        public async Task<ActionResult> GetEnrollment([FromRoute, Required] Guid enrollmentId)
        {
            return Ok(new MiddlewareResponse<EnrollmentDTO>(await _enrollmentManager.EnrollmentById(enrollmentId)));
        }

        /// <summary>
        /// Creates a new Enrollment and returns it as it is in DB
        /// </summary>
        /// <param name="enrollmentDTO">
        /// The new Enrollment with its information
        /// ## Body 
        /// 
        ///     {
        ///        "CourseId" : "63e9d868-b357-41cb-a720-4e04008ccdf9",
        ///        "StudentId" : "aa33143e-d49d-4a87-9b79-7ae4cdd9a863",
        ///     }
        /// </param>
        /// <response code="200">Success or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<EnrollmentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<ActionResult> PostEnrollment([FromBody, Required] EnrollmentDTO enrollmentDTO)
        {
            return Ok(new MiddlewareResponse<EnrollmentDTO>(await _enrollmentManager.Create(enrollmentDTO)));
        }

        /// <summary>
        /// Deletes a specific Enrollment
        /// </summary>
        /// <param name="enrollmentId">Id of the Enrollment to delete.</param>
        /// <remarks>Delete Enrollment by Id. Enrollment id is a unique identifier</remarks>
        /// <response code="200">True on success, or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{enrollmentId}")]
        public async Task<ActionResult> DeleteEnrollment([FromRoute, Required] Guid enrollmentId)
        {
            return Ok(new MiddlewareResponse<bool>(await _enrollmentManager.DeleteEnrollment(enrollmentId)));
        }

        /// <summary>
        /// Updates an Enrollment and returns it as it is in DB
        /// </summary>
        /// <param name="enrollmentId">unique identifier of Enrollment id</param>
        /// <param name="enrollment">
        /// Enrollment data to update
        /// ## Body 
        /// 
        ///     {
        ///        "CourseId" : "63e9d868-b357-41cb-a720-4e04008ccdf9",
        ///        "StudentId" : "aa33143e-d49d-4a87-9b79-7ae4cdd9a863",
        ///     }
        /// </param>
        /// <response code="200">Success or some expected Error</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<EnrollmentDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut]
        [Route("{enrollmentId}")]
        public async Task<ActionResult> PutEnrollment([FromRoute, Required] Guid enrollmentId, [FromBody, Required] EnrollmentDTO enrollment)
        {
            return Ok(new MiddlewareResponse<EnrollmentDTO>(await _enrollmentManager.UpdateById(enrollmentId, enrollment)));
        }

        /// <summary>
        /// Search a enrollment by the first name.
        /// </summary>
        /// <param name="firstName">First name of the user.</param>
        /// <returns>Returns enrollment information found by first name</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<IEnumerable<EnrollmentDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("student")]
        public async Task<IActionResult> GetEnrollmentsOfStudents([FromQuery] Guid studentId)
        {
            return Ok(new MiddlewareResponse<IEnumerable<Class>>(await _enrollmentManager.SearchEnrollmentsByUser(studentId)));
        }

        /// <summary>
        /// Search a enrollment by the first name.
        /// </summary>
        /// <param name="firstName">First name of the user.</param>
        /// <returns>Returns enrollment information found by first name</returns>
        /// <response code="200">Success</response>
        /// <response code="500">Can not connect with the database</response>
        [ProducesResponseType(typeof(MiddlewareResponse<IEnumerable<EnrollmentDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("course")]
        public async Task<IActionResult> GetEnrollmentsOfClasses([FromQuery] Guid courseId)
        {
            return Ok(new MiddlewareResponse<IEnumerable<Student>>(await _enrollmentManager.SearchEnrollmentsByCourse(courseId)));
        }
    }
}
