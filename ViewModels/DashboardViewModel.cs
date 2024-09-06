using System;
using webapp.Models;

namespace webapp.ViewModels
{
	public class DashboardViewModel
	{

		public required List<Race> Races { get; set; }
        public required List<Club> Clubs { get; set; }


    }
}

