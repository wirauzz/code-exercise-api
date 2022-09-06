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

namespace Logic.Managers
{
    public class ClassManager : IClassManager
    {
        public readonly IUnitOfWork _uow;
        public readonly IMapper<ClassDTO, Class> _mapper;

        public ClassManager(IUnitOfWork uow, IMapper<ClassDTO, Class> mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClassDTO>> AllClass()
        {
            IEnumerable<Class> getAllResult = await _uow.ClassRepository.GetAllAsync();
            IEnumerable<ClassDTO> ClassDTOs = _mapper.GetDTO(getAllResult);
            return ClassDTOs;
        }

        public async Task<ClassDTO> Create(ClassDTO Class)
        {
            try
            {
                if (string.IsNullOrEmpty(Class.Title))
                {
                    throw new LogicException("You must to add a FirstName to create the Class.");
                }
                if (string.IsNullOrEmpty(Class.Description))
                {
                    throw new LogicException("You must to add a LastName to create this Class.");
                }
                Class newClass = _mapper.GetDAO(Class);
                Class createResult = await _uow.ClassRepository.CreateAsync(newClass);
                ClassDTO createClass = _mapper.GetDTO(createResult);
                return createClass;
            }
            catch (LogicException ex)
            {
                string message = $"Error in ClassManager -> Create(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }


        public async Task<bool> DeleteClass(Guid ClassId)
        {
            try
            {
                Class ClassToDelete = await _uow.ClassRepository.GetByIdAsync(ClassId);
                if (ClassToDelete == null)
                {
                    throw new LogicException($"Can not find any Class with id: {ClassId}.");
                }
                else
                {
                    await _uow.ClassRepository.DeleteAsync(ClassToDelete);
                    return await _uow.ClassRepository.GetByIdAsync(ClassId) == null;
                }
            }
            catch (LogicException ex)
            {
                string message = $"Error in ClassManager -> DeleteClass(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }

        }

        public async Task<ClassDTO> ClassById(Guid ClassId)
        {
            try
            {
                Class getByIdResult = await _uow.ClassRepository.GetByIdAsync(ClassId);
                if (getByIdResult == null)
                {
                    throw new LogicException($"Can not find any Class by id: {ClassId}.");
                }
                ClassDTO Class = _mapper.GetDTO(getByIdResult);
                return Class;
            }
            catch (LogicException ex)
            {
                string message = $"Error in ClassManager -> ClassById(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }

        public async Task<ClassDTO> UpdateById(Guid ClassId, ClassDTO Class)
        {
            try
            {
                Class ClassToEdit = await _uow.ClassRepository.GetByIdAsync(ClassId);
                if (ClassToEdit == null)
                {
                    throw new LogicException($"The Class request id {ClassId} provided to update doesn't exist.");
                }
                else
                {
                    if (string.IsNullOrEmpty(Class.Title))
                    {
                        throw new LogicException("You must to add a FirstName to edit the Class.");
                    }
                    if (string.IsNullOrEmpty(Class.Description))
                    {
                        throw new LogicException("You must to add a LastName to edit this Class.");
                    }
                    ClassToEdit.Title = Class.Title;
                    ClassToEdit.Description = Class.Description;
                    Class updateResult = await _uow.ClassRepository.UpdateAsync(ClassToEdit);
                    ClassDTO editedClass = _mapper.GetDTO(updateResult);
                    return editedClass;
                }
            }
            catch (LogicException ex)
            {
                string message = $"Error in ClassManager -> UpdateById(){Environment.NewLine} Message: {ex.Message}{Environment.NewLine}";
                Log.Error(ex, $"{message}{Environment.NewLine} Stack trace: {Environment.NewLine}");
                throw new LogicException(ex.Message, ex.InnerException);
            }
        }
    }
}
