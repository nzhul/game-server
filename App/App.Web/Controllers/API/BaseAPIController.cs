using App.Data;
using System.Web.Http;

namespace App.Web.Controllers.API
{
	public class BaseAPIController : ApiController
	{
		protected IUnitOfWork Data;
		public BaseAPIController(IUnitOfWork data)
		{
			this.Data = data;
		}

		public BaseAPIController()
			: this(new UnitOfWork())
		{

		}
	}
}