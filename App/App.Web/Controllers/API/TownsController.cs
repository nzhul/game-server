using App.Web.ViewModels;
using App.Web.Controllers.API;
using System.Linq;
using System.Web.Http;

namespace App.Web.Controllers.API
{
	public class TownsController : BaseAPIController
	{


		[HttpGet]
		public IHttpActionResult GetBuildings()
		{
			return Ok(this.Data.Buildings.All().Select(b => new BuildingViewModel
			{
				Id = b.Id,
				Name = b.Name,
				Description = b.Description,
				Level = b.Level
			}));
		}

		[HttpGet]
		public IHttpActionResult UpdateResourceCost()
		{
			var theVillage = this.Data.Buildings.All().FirstOrDefault();
			theVillage.ResourceCosts.Gold = 500;
			this.Data.SaveChanges();

			return Ok();
		}

	}
}
