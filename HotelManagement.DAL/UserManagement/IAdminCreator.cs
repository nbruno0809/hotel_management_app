using HotelManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DAL.UserManagement
{
    public interface IAdminCreator
    {
        public Task CreateAdminRole();
        public Task CreateAdminUser();
    }
}
