using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Server.Data.Services.Abstraction;

namespace Server.Api.Controllers
{
    public class RealmsController : Controller
    {
        private readonly IRealmsService _realmsService;
        private readonly IMapper _mapper;

        public RealmsController()
        {

        }
    }
}
