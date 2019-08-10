using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Models.View.UnitConfigurations;
using Server.Data.Services.Abstraction;
using Server.Models.MapEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Api.Controllers
{
    [Authorize]
    [Route("api/unit-configurations")]
    public class UnitConfigurationsController : ControllerBase
    {
        private IUnitConfigurationsService unitConfigurationService;
        private readonly IMapper _mapper;

        public UnitConfigurationsController(
            IUnitConfigurationsService unitConfigurationService,
            IMapper mapper)
        {
            this.unitConfigurationService = unitConfigurationService;
            _mapper = mapper;
        }

        [HttpGet("{creatureType?}")]
        public IActionResult GetConfiguration(CreatureType? creatureType = null)
        {
            try
            {
                var configurations = this.unitConfigurationService.GetConfigurations(creatureType);

                if (configurations != null)
                {
                    var configsToReturn = configurations.ProjectTo<UnitConfigurationView>(_mapper.ConfigurationProvider);
                    return Ok(this.ConvertToDictionary(configsToReturn));
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // LogException(ex);
                return StatusCode(500, ex);
            }
        }

        private Dictionary<CreatureType, UnitConfigurationView> ConvertToDictionary(IQueryable<UnitConfigurationView> configsToReturn)
        {
            Dictionary<CreatureType, UnitConfigurationView> configs = new Dictionary<CreatureType, UnitConfigurationView>();

            foreach (var config in configsToReturn)
            {
                if (!configs.ContainsKey(config.Type))
                {
                    configs.Add(config.Type, config);
                }
            }

            return configs;
        }
    }
}
