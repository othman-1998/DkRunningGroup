using System;
using webapp.Data.Enum;
using webapp.Models;

namespace webapp.ViewModels
{
	public class EditClubViewModel
	{

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
        public string? URL { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; } = new Address();
        public ClubCategory ClubCategory { get; set; }

	}
}

