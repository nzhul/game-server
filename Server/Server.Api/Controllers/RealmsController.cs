using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Helpers;
using Server.Api.Models.View.Realms;
using Server.Data.Services.Abstraction;
using Server.Models;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RealmsController : Controller
    {
        private readonly IRealmsService _realmsService;
        private readonly IMapper _mapper;

        public RealmsController(IRealmsService realmsService, IMapper mapper)
        {
            _realmsService = realmsService;
            _mapper = mapper;
        }

        [HttpGet("{id}", Name = "GetRealm")]
        public async Task<IActionResult> GetRealm(int id)
        {
            var realm = await this._realmsService.GetRealm(id);

            var realmToReturn = _mapper.Map<RealmDetailedDto>(realm);

            return Ok(realmToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> GetRealmsList(QueryParams queryParams)
        {
            var realms = await this._realmsService.GetRealms(queryParams);

            var realmsToReturn = _mapper.Map<IEnumerable<RealmListItemDto>>(realms);

            //realmsToReturn = realmsToReturn.OrderByDescending(r => r.AvatarsCount);

            Response.AddPagination(realms.CurrentPage, realms.PageSize, realms.TotalCount, realms.TotalPages);

            return Ok(realmsToReturn);
        }
    }
}
