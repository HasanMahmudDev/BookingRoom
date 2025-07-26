using BookingRoom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingRoom.Application.Common.Interfaces
{
    public interface IVillaRepository : IRopository<Villa>
    {
        void Update(Villa entity);
        void Save();
    }
}
