using App.Models.Towns;
using System.Collections.Generic;

namespace App.Data.Services
{
	public interface ITownsService
	{
		IEnumerable<ITown> GetUserTowns();
		IEnumerable<IBuilding> GetAllBuildings();
		IEnumerable<IBuilding> GetBuildingsByTownId(int id);
	}
}
