using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoworkingReservations.Core.DTOs;
using CoworkingReservations.Core.Entities;
public interface IUserService
{
    Task RegisterUser(UserCreateDto dto);
    Task<(bool IsValid, User? User)> ValidateUser(UserLoginDto login);
    // ... otros métodos de usuarios
}

public class UserCreateDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}