using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Models.Mapper
{
    public interface IMapper <T, R>
    {
        T GetDTO(R dao);
        IEnumerable<T> GetDTO(IEnumerable<R> dao);
        R GetDAO(T dto);
    }
}
