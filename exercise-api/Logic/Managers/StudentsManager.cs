using Data;
using Data.Models;
using Logic.Managers.Interfaces;
using Logic.Models;
using Logic.Models.Mapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Managers
{
    public class StudentsManager : IStudentsManager
    {
        public readonly IUnitOfWork _uow;
        public readonly IMapper<StudentDTO, Student> _mapper;

        public StudentsManager(IUnitOfWork uow, IMapper<StudentDTO, Student> mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDTO>> AllStudents()
        {
            IEnumerable<Student> getAllResult = await _uow.StudentRepository.GetAllAsync();
            IEnumerable<StudentDTO> studentDTOs = _mapper.GetDTO(getAllResult);
            return studentDTOs;
        }

        public async Task<StudentDTO> Create(StudentDTO student)
        {
            Student newStudent = _mapper.GetDAO(student);
            Student createResult = await _uow.StudentRepository.CreateAsync(newStudent);
            StudentDTO createStudent = _mapper.GetDTO(createResult);
            return createStudent;
        }


        public async Task<bool> DeleteStudent(Guid studentId)
        {
            Student studentToDelete = await _uow.StudentRepository.GetByIdAsync(studentId);
            await _uow.StudentRepository.DeleteAsync(studentToDelete);
            return await _uow.StudentRepository.GetByIdAsync(studentId) == null;
        }

        public async Task<StudentDTO> StudentById(Guid studentId)
        {
            Student getByIdResult = await _uow.StudentRepository.GetByIdAsync(studentId);
            StudentDTO student = _mapper.GetDTO(getByIdResult);
            return student;
        }

        public async Task<StudentDTO> UpdateById(Guid studentId, StudentDTO student)
        {
            Student studentToEdit = await _uow.StudentRepository.GetByIdAsync(studentId);
            studentToEdit.FirstName = student.FirstName;
            studentToEdit.LastName = student.LastName;
            Student updateResult = await _uow.StudentRepository.UpdateAsync(studentToEdit);
            StudentDTO editedStudent = _mapper.GetDTO(updateResult);
            return editedStudent;
        }
    }
}
