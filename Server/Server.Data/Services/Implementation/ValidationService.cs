using System;
using Server.Data.Services.Abstraction;
using Server.Models.Realms;

namespace Server.Data.Services.Implementation
{
    public class ValidationService : IValidationService
    {
        public bool IsValidPosition(Region region, int x, int y)
        {
            if (region == null || string.IsNullOrEmpty(region.MatrixString))
            {
                throw new ArgumentException("Missing region or region matrix");
            }

            return region.Matrix[y, x] == 0; // TODO: might have to swap x and y
        }
    }
}
