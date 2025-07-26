using BookingRoom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookingRoom.Application.Common.Interfaces
{
    public interface IVillaNumberRepository : IRopository<VillaNumber>
    {
        void Update(VillaNumber entity);
    }
}
