using System;
using webapp.Models;

namespace webapp.Interfaces
{
	public interface IDashboardRepository
	{

		Task<List<Race>> GetAllUserRaces();
        Task<List<Club>> GetAllUserClubs();
    }
}

