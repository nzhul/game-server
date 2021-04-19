using Assets.Scripts.Network.Services.TCP.Interfaces;

namespace Assets.Scripts.Network.Services
{
    public class RequestManagerTcp
    {
        private static RequestManagerTcp _instance;

        public static RequestManagerTcp Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new RequestManagerTcp();
                }

                return _instance;
            }
        }

        public static IGameService GameService { get; private set; }
    }
}
