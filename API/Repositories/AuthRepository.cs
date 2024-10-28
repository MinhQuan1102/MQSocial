using System;
using API.Data;
using API.Interfaces;
using AutoMapper;

namespace API.Repositories;

public class AuthRepository(DataContext context, IMapper mapper) : IAuthRepository
{
    private readonly DataContext _context = context;
    private readonly IMapper _mapper = mapper;
}
