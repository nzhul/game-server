using System;
using System.Threading.Tasks;

namespace GameServer.Utilities
{
    public static class HttpUtilities
    {
        // Fire and forget
        public static void FF(Action action, string errorMessage)
        {
            Task.Run(() =>
            {
                try
                {
                    action.Invoke();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{errorMessage} Ex: {ex}");
                }
            });
        }
    }
}
