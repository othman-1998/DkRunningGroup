using System;
using webapp.Data;
using webapp.Interfaces;
using webapp.Models;

namespace webapp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccesor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccesor;
        }


        public async Task<List<Club>> GetAllUserClubs()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;
            var userClubs = _context.Clubs.Where(r => r.AppUser.Id == currentUser.ToString());
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User;
            var userRaces = _context.Races.Where(r => r.AppUser.Id == currentUser.ToString());
            return userRaces.ToList();
        }
    }
}

