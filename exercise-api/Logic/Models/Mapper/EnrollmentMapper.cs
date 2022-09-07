using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Models.Mapper
{
    public class EnrollmentMapper : IMapper<EnrollmentDTO, Enrollment>
    {
        public Enrollment GetDAO(EnrollmentDTO enrollment)
        {
            return new Enrollment()
            {
                Id = enrollment.Id,
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId
            };
        }

        public EnrollmentDTO GetDTO(Enrollment enrollment)
        {
            EnrollmentDTO enrollmentDTO = new EnrollmentDTO()
            {
                Id = enrollment.Id,
                CourseId = enrollment.CourseId,
                StudentId = enrollment.StudentId
            };
            return enrollmentDTO;
        }

        public IEnumerable<EnrollmentDTO> GetDTO(IEnumerable<Enrollment> enrollments)
        {
            List<EnrollmentDTO> enrollmentDTOs = new List<EnrollmentDTO>();
            foreach (Enrollment enrollment in enrollments)
            {
                enrollmentDTOs.Add(GetDTO(enrollment));
            }
            return enrollmentDTOs;
        }
    }
}
