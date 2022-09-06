using Logic.Managers.Interfaces;
using Logic.Models;
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
    [Route("api/classes")]
    public class ClassController : Controller
    {
        private readonly IClassManager _ClassManager;

        public ClassController(IClassManager ClassManager)
        {
            _ClassManager = ClassManager;
        }

        /// <summary>
        /// Returns all Classs
        /// </summary>
        /// <remarks>Get all Classs</remarks>
        /// <response code="200">Success or some expected Error</response>
        [ProducesResponseType(typeof(MiddlewareResponse<IEnumerable<ClassDTO>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet]
        public async Task<ActionResult> GeAllClasses()
        {
            return Ok(new MiddlewareResponse<IEnumerable<ClassDTO>>(await _ClassManager.AllClass()));
        }

        /// <summary>
        /// Returns a specific Class
        /// </summary>
        /// <remarks>Get Class by Id, Class id must be a unique identifier</remarks>
        /// <response code="200">Success or some expected Error</response>
        [ProducesResponseType(typeof(MiddlewareResponse<ClassDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpGet("{ClassId}")]
        public async Task<ActionResult> GetClass([FromRoute, Required] Guid ClassId)
        {
            return Ok(new MiddlewareResponse<ClassDTO>(await _ClassManager.ClassById(ClassId)));
        }

        /// <summary>
        /// Creates a new Class and returns it as it is in DB
        /// </summary>
        /// <param name="ClassDTO">
        /// The new Class with its information
        /// ## Body 
        /// 
        ///     {
        ///        "title" : "Programacion II",
        ///        "description" : "OOP"
        ///     }
        /// </param>
        /// <response code="200">Success or some expected Error</response>
        [ProducesResponseType(typeof(MiddlewareResponse<ClassDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPost]
        public async Task<ActionResult> PostClass([FromBody, Required] ClassDTO ClassDTO)
        {
            return Ok(new MiddlewareResponse<ClassDTO>(await _ClassManager.Create(ClassDTO)));
        }

        /// <summary>
        /// Deletes a specific Class
        /// </summary>
        /// <param name="ClassId">Id of the Class to delete.</param>
        /// <remarks>Delete Class by Id. Class id is a unique identifier</remarks>
        /// <response code="200">True on success, or some expected Error</response>
        [ProducesResponseType(typeof(MiddlewareResponse<bool>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{ClassId}")]
        public async Task<ActionResult> DeleteClass([FromRoute, Required] Guid ClassId)
        {
            return Ok(new MiddlewareResponse<bool>(await _ClassManager.DeleteClass(ClassId)));
        }

        /// <summary>
        /// Updates an Class and returns it as it is in DB
        /// </summary>
        /// <param name="ClassId">unique identifier of Class id</param>
        /// <param name="Class">
        /// Class data to update
        /// ## Body 
        /// 
        ///     {
        ///        "title" : "Programacion II",
        ///        "description" : "OOP"
        ///     }
        /// </param>
        /// <response code="200">Success or some expected Error</response>
        [ProducesResponseType(typeof(MiddlewareResponse<ClassDTO>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [HttpPut]
        [Route("{ClassId}")]
        public async Task<ActionResult> PutClass([FromRoute, Required] Guid ClassId, [FromBody, Required] ClassDTO Class)
        {
            return Ok(new MiddlewareResponse<ClassDTO>(await _ClassManager.UpdateById(ClassId, Class)));
        }
    }
}
