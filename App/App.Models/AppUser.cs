using App.Models.Enums;
using App.Models.Heroes;
using App.Models.Towns;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
	public class AppUser : IdentityUser
	{
		private ICollection<Hero> heroes;
		private ICollection<Town> towns;

		public AppUser()
		{
			this.heroes = new HashSet<Hero>();
			this.towns = new HashSet<Town>();
			this.AvailableResources = new Dictionary<ResourceType, int>();
			this.AvailableResources.Add(ResourceType.Wood, 20);
			this.AvailableResources.Add(ResourceType.Stone, 20);
			this.AvailableResources.Add(ResourceType.Iron, 10);
			this.AvailableResources.Add(ResourceType.Gold, 10000);
			this.AvailableResources.Add(ResourceType.Gems, 5);
			this.AvailableResources.Add(ResourceType.Brimstone, 0);
		}

		//public int Id { get; set; }

		//public string Username { get; set; }

		//public string Email { get; set; }

		//public string Password { get; set; }

		//public bool IsActive { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager, string authenticationType)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
			// Add custom user claims here
			return userIdentity;
		}

		public virtual Dictionary<ResourceType, int> AvailableResources { get; set; }

		public virtual ICollection<Hero> Heroes
		{
			get { return this.heroes; }
			set { this.heroes = value; }
		}

		public virtual ICollection<Town> Towns
		{
			get { return this.towns; }
			set { this.towns = value; }
		}
	}
}
