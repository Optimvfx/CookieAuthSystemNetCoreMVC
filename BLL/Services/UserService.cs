using AutoMapper;
using BLL.Models.Auth;
using BLL.Models.UserModels;
using Common.Exceptions.User;
using Common.Helpers;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services;

public class UserService
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UserService(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<User> RegisterUser(RegisterModel model)
    {
        if (await CheckUserExistByNick(model.Nick)) throw new NickAlreadyExistException();
        if (await  CheckUserExistByEmail(model.Email)) throw new EmailAlreadyExistException();

        var user = _mapper.Map<User>(model);
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
        return user;
    }
    
    public async Task<User> GetUserById(Guid id)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) throw new UserNotFoundException();
        return user;
    }

    public async Task<User> GetUserByCredentials(CredentialModel model)
    {
        var passwordHash = HashHelper.GetHash(model.Password);
        var user = await _db.Users.FirstOrDefaultAsync(u => u.PasswordHash == passwordHash && u.Email == model.Email);
        if (user == null) throw new UserNotFoundException();
        return user;
    }

    public async Task<bool> CheckUserExistByNick(string nick)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Nick == nick);
        return user != null;
    }

    public async Task<bool> CheckUserExistByEmail(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        return user != null;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email);
        if (user == null) throw new UserNotFoundException();
        return user;
    }

    public async Task DeleteUser(Guid userId)
    {
        var user = await GetUserById(userId);
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }
}
