using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Models.Mapper
{
    public class ClassMapper : IMapper<ClassDTO, Class>
    {
        public Class GetDAO(ClassDTO Class)
        {
            return new Class()
            {
                Id = Class.ClassId,
                Title = Class.Title,
                Description = Class.Description
            };
        }

        public ClassDTO GetDTO(Class Class)
        {
            ClassDTO ClassDTO = new ClassDTO()
            {
                ClassId = Class.Id,
                Title = Class.Title,
                Description = Class.Description
            };
            return ClassDTO;
        }

        public IEnumerable<ClassDTO> GetDTO(IEnumerable<Class> Classs)
        {
            List<ClassDTO> ClassDTOs = new List<ClassDTO>();
            foreach (Class Class in Classs)
            {
                ClassDTOs.Add(GetDTO(Class));
            }
            return ClassDTOs;
        }
    }
}
