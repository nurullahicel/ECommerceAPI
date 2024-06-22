using ECommerceAPI.Application.Abstractions.Services;
using ECommerceAPI.Application.DTOs.User;
using ECommerceAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistence.Services
{
    public class UserService : IUserService
    {
      readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<CreateUserResponse> CreateAsync(CreateUser model)
        {
           IdentityResult result = await _userManager.CreateAsync(new()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Surname = model.Surname,
                UserName = model.Username,
                Email = model.Email

            }, model.Password);
            CreateUserResponse response = new() { Succeeded = result.Succeeded };


            if (result.Succeeded)
            { response.Message = "User created successfully"; }

            else
            {
                foreach (var error in result.Errors)
                { response.Message += $"{error.Code} - {error.Description} "; }
            }
            return response;
        }
    }
}
