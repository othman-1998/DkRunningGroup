using System;
using Microsoft.EntityFrameworkCore;
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
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = await _context.Clubs
                .Where(r => r.AppUser != null && r.AppUser.Id == currentUser)
                .ToListAsync();

            // Log the count of user clubs
            Console.WriteLine($"User Clubs Count: {userClubs.Count}");

            return userClubs;
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = await _context.Races.Where(r => r.AppUser != null && r.AppUser.Id == currentUser).ToListAsync();
            return userRaces;
        }
    }
}

