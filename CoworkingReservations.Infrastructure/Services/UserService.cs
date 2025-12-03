using AutoMapper;
using CoworkingReservations.Core.DTOs;
using CoworkingReservations.Core.Entities;
using CoworkingReservations.Core.Enum;
using CoworkingReservations.Core.Exceptions;
using CoworkingReservations.Core.Interfaces;
using Microsoft.EntityFrameworkCore; // Necesario para Linq async

namespace CoworkingReservations.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService; // Inyectamos el hasher

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task RegisterUser(UserCreateDto dto)
        {
            // Validar si existe email (usando repositorio genérico con LINQ, o crea un método en repositorio)
            var users = await _unitOfWork.UserRepository.GetAll(); // Ojo: Idealmente usar un GetByEmail en repositorio
            if (users.Any(u => u.Email == dto.Email))
                throw new BussinesException("El email ya está registrado");

            var user = _mapper.Map<User>(dto);

            // 1. Hashear password
            user.Password = _passwordService.Hash(dto.Password);
            user.Role = RoleType.User; // Rol por defecto
            user.IsActive = true;
            user.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<(bool IsValid, User? User)> ValidateUser(UserLoginDto login)
        {
            // Buscar usuario por email (Esto es temporal, idealmente usar Dapper o repo específico)
            var users = await _unitOfWork.UserRepository.GetAll();
            var user = users.FirstOrDefault(x => x.Email == login.Email);

            if (user == null) return (false, null);

            // 2. Verificar password
            bool isValid = _passwordService.Check(user.Password, login.Password);

            return (isValid, user);
        }

        [Serializable]
        private class BussinesException : Exception
        {
            public BussinesException()
            {
            }

            public BussinesException(string? message) : base(message)
            {
            }

            public BussinesException(string? message, Exception? innerException) : base(message, innerException)
            {
            }
        }
    }
}