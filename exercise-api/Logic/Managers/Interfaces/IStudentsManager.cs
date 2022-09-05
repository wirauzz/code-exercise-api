using Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Managers.Interfaces
{
    public interface IStudentsManager
    {
        Task<IEnumerable<StudentDTO>> AllStudents();
        Task<StudentDTO> Create(StudentDTO newStudent);
        Task<StudentDTO> UpdateById(Guid studentId, StudentDTO studentStateDTO);
        Task<StudentDTO> StudentById(Guid studentId);
        Task<bool> DeleteStudent(Guid studentId);
    }
}
