using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using webapp.Models;

namespace webapp.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser>
	{

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {

		}

		// here we are brining in the database tables from our db
		public DbSet<Race> Races { get; set; }
		public DbSet<Club> Clubs { get; set; }
		public DbSet<Address> Addresses { get; set; }
	}
}

