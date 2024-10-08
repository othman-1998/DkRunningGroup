using System;
using webapp.Models;

namespace webapp.Interfaces
{
	public interface IUsersRepository
	{
		Task<IEnumerable<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);
		bool Add(AppUser user);
		bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();

    }
}

