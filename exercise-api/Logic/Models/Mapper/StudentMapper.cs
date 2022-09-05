using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Models.Mapper
{
    public class StudentMapper : IMapper<StudentDTO, Student>
    {
        public Student GetDAO(StudentDTO student)
        {
            return new Student()
            {
                Id = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName
            };
        }

        public StudentDTO GetDTO(Student student)
        {
            StudentDTO studentDTO = new StudentDTO()
            {
                StudentId = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName
            };
            return studentDTO;
        }

        public IEnumerable<StudentDTO> GetDTO(IEnumerable<Student> students)
        {
            List<StudentDTO> studentDTOs = new List<StudentDTO>();
            foreach(Student student in students)
            {
                studentDTOs.Add(GetDTO(student));
            }
            return studentDTOs;
        }
    }
}
