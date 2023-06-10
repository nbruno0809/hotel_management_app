using HotelManagement.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.DAL.UserManagement
{
    public class AdminCreator : IAdminCreator
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly UserManager<HotelUser> _userManager;
        private readonly IConfiguration _configuration;
        public AdminCreator(
            RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<HotelUser> userManager,
            IConfiguration configuration) 
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task CreateAdminRole()
        {
            if (!await _roleManager.RoleExistsAsync(Roles.Administrator))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>() { Name=Roles.Administrator });
            }
        }

        public async Task CreateAdminUser()
        {
            if (!(await _userManager.GetUsersInRoleAsync(Roles.Administrator)).Any())
            {
                var email = _configuration["AdminCredentials:Email"];
                var password = _configuration["AdminCredentials:Password"];
                if (email == null || password == null)
                    throw new Exception($"The required deafult admin credentilas were not provided in the configuartion file." +
                        $"{nameof(email)} or {nameof(password)} is null.");

                var createResult = await _userManager.CreateAsync(new HotelUser()
                {
                    FirstName = "admin",
                    LastName = "admin",
                    Email = email,
                    UserName =email,
                  
                },password);

                if (!createResult.Succeeded)
                {
                    throw new Exception(string.Join(", ",createResult.Errors));
                }

                var user = await _userManager.FindByEmailAsync(email);
                var roleResult = await _userManager.AddToRoleAsync(user,Roles.Administrator);

                if (!roleResult.Succeeded)
                {
                    throw new Exception(string.Join(", ",roleResult.Errors));
                }

            }

        }
    }
}
