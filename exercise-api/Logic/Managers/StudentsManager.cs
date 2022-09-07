using Data;
using Data.Models;
using Logic.Exceptions;
using Logic.Managers.Interfaces;
using Logic.Models;
using Logic.Models.Mapper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

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
            try
            {
                if (string.IsNullOrEmpty(student.FirstName))
                {
                    throw new LogicException("You must to add a FirstName to create the student.");
                }
                if (string.IsNullOrEmpty(student.LastName))
                {
                    throw new LogicException("You must to add a LastName to create this student.");
                }
                Student newStudent = _mapper.GetDAO(student);
                Student createResult = await _uow.StudentRepository.CreateAsync(newStudent);
                StudentDTO createStudent = _mapper.GetDTO(createResult);
                return createStudent;
            }
            catch (LogicException ex)
            {
                string message = $"Error in StudentManager -> Create(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }


        public async Task<bool> DeleteStudent(Guid studentId)
        {
            try
            {
                Student studentToDelete = await _uow.StudentRepository.GetByIdAsync(studentId);
                if (studentToDelete == null)
                {
                    throw new LogicException($"Can not find any student with id: {studentId}.");
                }
                else
                {
                    await _uow.StudentRepository.DeleteAsync(studentToDelete);
                    return await _uow.StudentRepository.GetByIdAsync(studentId) == null;
                }
            }
            catch (LogicException ex)
            {
                string message = $"Error in StudentManager -> DeleteStudent(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }

        }

        public async Task<IEnumerable<StudentDTO>> SearchStudentByName(string name)
        {
            IEnumerable<StudentDTO> students = await AllStudents();
            return students.Where(student => student.FirstName.ToLower().Trim().Contains(name.ToLower().Trim()));
        }

        public async Task<StudentDTO> StudentById(Guid studentId)
        {
            try
            {
                Student getByIdResult = await _uow.StudentRepository.GetByIdAsync(studentId);
                if(getByIdResult == null)
                {
                    throw new LogicException($"Can not find any Student by id: {studentId}.");
                }
                StudentDTO student = _mapper.GetDTO(getByIdResult);
                return student;
            }
            catch (LogicException ex)
            {
                string message = $"Error in StudentManager -> StudentById(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }

        public async Task<StudentDTO> UpdateById(Guid studentId, StudentDTO student)
        {
            try
            {
                Student studentToEdit = await _uow.StudentRepository.GetByIdAsync(studentId);
                if (studentToEdit == null)
                {
                    throw new LogicException($"The Student request id {studentId} provided to update doesn't exist.");
                }
                else
                {
                    if (string.IsNullOrEmpty(student.FirstName))
                    {
                        throw new LogicException("You must to add a FirstName to edit the student.");
                    }
                    if (string.IsNullOrEmpty(student.LastName))
                    {
                        throw new LogicException("You must to add a LastName to edit this student.");
                    }
                    studentToEdit.FirstName = student.FirstName;
                    studentToEdit.LastName = student.LastName;
                    Student updateResult = await _uow.StudentRepository.UpdateAsync(studentToEdit);
                    StudentDTO editedStudent = _mapper.GetDTO(updateResult);
                    return editedStudent;
                }
            }
            catch(LogicException ex)
            {
                string message = $"Error in StudentManager -> UpdateById(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }
    }
}
