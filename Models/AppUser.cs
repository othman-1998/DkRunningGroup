﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace webapp.Models
{
	public class AppUser : IdentityUser
	{

        public int? Pace { get; set; }
        public int? Milage { get; set; }
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
		public ICollection<Club>? Clubs { get; set; }
		public ICollection<Race>? Races { get; set; }
        public string Kilometer { get; internal set; }
    }
}

