using Logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Managers.Interfaces
{
    public interface IClassManager
    {
        Task<IEnumerable<ClassDTO>> AllClass();
        Task<ClassDTO> Create(ClassDTO newClass);
        Task<ClassDTO> UpdateById(Guid ClassId, ClassDTO ClassStateDTO);
        Task<ClassDTO> ClassById(Guid ClassId);
        Task<bool> DeleteClass(Guid ClassId);
    }
}
