using App.Web.Controllers.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using App.Data;

namespace App.Web.Controllers
{
    //[Authorize]
    public class ValuesController : ApiController
    {
        private IUnitOfWork Data;

        public ValuesController(IUnitOfWork data)
        {
            this.Data = data;
        }

        // GET api/values
        public IEnumerable<string> Get()
        {
            var alltowns = this.Data.Towns.All();
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
