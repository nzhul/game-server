using System;
using System.Diagnostics;

namespace GameServer.Utilities
{
    public static class Timer
    {
        public static float GetElapsedTime()
        {
            return (float)(DateTime.UtcNow - Process.GetCurrentProcess().StartTime.ToUniversalTime()).TotalSeconds;
        }
    }
}
