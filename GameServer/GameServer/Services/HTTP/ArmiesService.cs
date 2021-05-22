using Assets.Scripts.Network.Services.HTTP.Interfaces;

namespace Assets.Scripts.Network.Services.HTTP
{
    public class ArmiesService : BaseService, IArmiesService
    {
        public void UpdateArmyPosition(int armyId, int x, int y)
        {
            base.Put($"armies/{armyId}/{x}/{y}");
        }
    }
}
