using webapp.Data.Enum;
using webapp.Models;

namespace webapp.ViewModels
{
	public class CreateClubViewModel
	{

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Address Address { get; set; } = new Address();
        public IFormFile? Image { get; set; }
        public ClubCategory ClubCategory { get; set; }
        public string? AppUserId { get; set; }

    }
}

