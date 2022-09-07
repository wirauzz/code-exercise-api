using Data;
using System;
using System.Collections.Generic;
using Logic.Managers.Interfaces;
using System.Text;
using Logic.Models.Mapper;
using Logic.Models;
using Data.Models;
using System.Threading.Tasks;
using Logic.Exceptions;
using Serilog;
using System.Linq;

namespace Logic.Managers
{
    public class EnrollmentManager : IEnrollmentManager
    {
        public readonly IUnitOfWork _uow;
        public readonly IMapper<EnrollmentDTO, Enrollment> _mapper;

        public EnrollmentManager(IUnitOfWork uow, IMapper<EnrollmentDTO, Enrollment> mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EnrollmentDTO>> AllEnrollments()
        {
            IEnumerable<Enrollment> getAllResult = await _uow.EnrollmentRepository.GetAllAsync();
            IEnumerable<EnrollmentDTO> EnrollmentDTOs = _mapper.GetDTO(getAllResult);
            return EnrollmentDTOs;
        }

        public async Task<EnrollmentDTO> Create(EnrollmentDTO Enrollment)
        {
            try
            {
                Student student = await _uow.StudentRepository.GetByIdAsync(Enrollment.StudentId);
                Class course = await _uow.ClassRepository.GetByIdAsync(Enrollment.CourseId);
                if (student == null)
                {
                    throw new LogicException("You must to add a existing student to create the Enrollment.");
                }
                if (course == null)
                {
                    throw new LogicException("You must to add a existing student create this Enrollment.");
                }
                Enrollment newEnrollment = _mapper.GetDAO(Enrollment);
                Enrollment createResult = await _uow.EnrollmentRepository.CreateAsync(newEnrollment);
                EnrollmentDTO createEnrollment = _mapper.GetDTO(createResult);
                return createEnrollment;
            }
            catch (LogicException ex)
            {
                string message = $"Error in EnrollmentManager -> Create(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }


        public async Task<bool> DeleteEnrollment(Guid EnrollmentId)
        {
            try
            {
                Enrollment EnrollmentToDelete = await _uow.EnrollmentRepository.GetByIdAsync(EnrollmentId);
                if (EnrollmentToDelete == null)
                {
                    throw new LogicException($"Can not find any Enrollment with id: {EnrollmentId}.");
                }
                else
                {
                    await _uow.EnrollmentRepository.DeleteAsync(EnrollmentToDelete);
                    return await _uow.EnrollmentRepository.GetByIdAsync(EnrollmentId) == null;
                }
            }
            catch (LogicException ex)
            {
                string message = $"Error in EnrollmentManager -> DeleteEnrollment(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }

        }

        public async Task<EnrollmentDTO> EnrollmentById(Guid EnrollmentId)
        {
            try
            {
                Enrollment getByIdResult = await _uow.EnrollmentRepository.GetByIdAsync(EnrollmentId);
                if (getByIdResult == null)
                {
                    throw new LogicException($"Can not find any Enrollment by id: {EnrollmentId}.");
                }
                EnrollmentDTO Enrollment = _mapper.GetDTO(getByIdResult);
                return Enrollment;
            }
            catch (LogicException ex)
            {
                string message = $"Error in EnrollmentManager -> EnrollmentById(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }

        public async Task<EnrollmentDTO> UpdateById(Guid EnrollmentId, EnrollmentDTO Enrollment)
        {
            try
            {
                Enrollment EnrollmentToEdit = await _uow.EnrollmentRepository.GetByIdAsync(EnrollmentId);
                Student student = await _uow.StudentRepository.GetByIdAsync(Enrollment.StudentId);
                Class course = await _uow.ClassRepository.GetByIdAsync(Enrollment.CourseId);
                if (EnrollmentToEdit == null)
                {
                    throw new LogicException($"The Enrollment request id {EnrollmentId} provided to update doesn't exist.");
                }
                else
                {
                    if (student == null)
                    {
                        throw new LogicException("You must an existing student to edit the Enrollment.");
                    }
                    if (course == null)
                    {
                        throw new LogicException("You must an existing course edit this Enrollment.");
                    }
                    EnrollmentToEdit.CourseId = Enrollment.CourseId;
                    EnrollmentToEdit.StudentId = Enrollment.StudentId;
                    Enrollment updateResult = await _uow.EnrollmentRepository.UpdateAsync(EnrollmentToEdit);
                    EnrollmentDTO editedEnrollment = _mapper.GetDTO(updateResult);
                    return editedEnrollment;
                }
            }
            catch (LogicException ex)
            {
                string message = $"Error in EnrollmentManager -> UpdateById(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<Class>> SearchEnrollmentsByUser(Guid studentId)
        {
            try
            {
                IEnumerable<EnrollmentDTO> enrollments = await AllEnrollments();
                Student student = await _uow.StudentRepository.GetByIdAsync(studentId);
                if(student == null)
                {
                    throw new LogicException("You must use an existing student to search their Enrollments.");
                }
                IEnumerable<EnrollmentDTO> resultEnrollments = enrollments.Where(e => e.StudentId == student.Id);
                IEnumerable<Class> classes = Enumerable.Empty<Class>();
                if (resultEnrollments != null)
                {
                    foreach (EnrollmentDTO enrollment in resultEnrollments)
                    {
                        Class courseInfo = await _uow.ClassRepository.GetByIdAsync(enrollment.CourseId);
                        classes = classes.Append(courseInfo);
                    }
                }
                return classes;
            }
            catch (LogicException ex)
            {
                string message = $"Error in EnrollmentManager -> SearchEnrollmentsByUser(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }

        public async Task<IEnumerable<Student>> SearchEnrollmentsByCourse(Guid courseId)
        {
            try
            {
                IEnumerable<EnrollmentDTO> enrollments = await AllEnrollments();
                Class course = await _uow.ClassRepository.GetByIdAsync(courseId);
                if (course == null)
                {
                    throw new LogicException("You must use an existing course to search their Enrollments.");
                }
                IEnumerable<EnrollmentDTO> resultEnrollments = enrollments.Where(e => e.CourseId == course.Id);
                IEnumerable<Student> students = Enumerable.Empty<Student>();
                if(resultEnrollments != null)
                {
                    foreach(EnrollmentDTO enrollment in resultEnrollments)
                    {
                        Student studentInfo = await _uow.StudentRepository.GetByIdAsync(enrollment.StudentId);
                        students = students.Append(studentInfo);
                    }
                }
                return students;
            }
            catch (LogicException ex)
            {
                string message = $"Error in EnrollmentManager -> SearchEnrollmentsByCourse(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }
    }
}
