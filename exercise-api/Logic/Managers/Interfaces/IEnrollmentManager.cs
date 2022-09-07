using Data.Models;
using Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Managers.Interfaces
{
    public interface IEnrollmentManager
    {
        Task<IEnumerable<EnrollmentDTO>> AllEnrollments();
        Task<IEnumerable<Class>> SearchEnrollmentsByUser(Guid studentId);
        Task<IEnumerable<Student>> SearchEnrollmentsByCourse(Guid courseId);
        Task<EnrollmentDTO> Create(EnrollmentDTO newEnrollment);
        Task<EnrollmentDTO> UpdateById(Guid enrollmentId, EnrollmentDTO enrollmentStateDTO);
        Task<EnrollmentDTO> EnrollmentById(Guid enrollmentId);
        Task<bool> DeleteEnrollment(Guid enrollmentId);
    }
}
