using System;

namespace Assets.Scripts.Network.Shared.NetMessages.Users
{
    // TODO: AuthRequest should not pass all this information.
    // Server should do another request to the server to fetch user details like:
    // MMR, GameId, BattleId etc.
    [Serializable]
    public class Net_AuthRequest : NetMessage
    {
        public Net_AuthRequest()
        {
            OperationCode = NetOperationCode.AuthRequest;
        }

        public int UserId { get; set; }

        public string Username { get; set; }

        public int MMR { get; set; }

        public string Token { get; set; }

        public int GameId { get; set; }

        public Guid? BattleId { get; set; }

        public bool IsValid()
        {
            bool result = true;

            if (this.UserId == 0)
            {
                result = false;
            }

            if (string.IsNullOrEmpty(this.Username))
            {
                result = false;
            }

            if (string.IsNullOrEmpty(this.Token))
            {
                result = false;
            }

            return result;
        }
    }
}