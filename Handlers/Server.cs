using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System.Linq;

namespace BloodLust049.Handlers
{
    public class Server
    {
        public void OnRoundStarted()
        {
            try
            {
                Log.Debug("Round started, finding 049", BloodLust049.Instance.Config.Debug);
                Exiled.API.Features.Player scp049 = Exiled.API.Features.Player.List.First(p => p.Role == RoleType.Scp049);
                Log.Debug("049 found", BloodLust049.Instance.Config.Debug);
                BloodLust049.Instance.Scp049 = scp049;
                BloodLust049.Instance.Scp049InGame = true;
                BloodLust049.Instance.mainCoroEnabled = true;
                BloodLust049.Instance.Coro = Timing.RunCoroutine(BloodLust049.Instance.BloodLust());
                BloodLust049.Instance.Coroutines.Add(BloodLust049.Instance.Coro);
            }catch { } // Scp 049 not found, disregard
        }

        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Log.Debug("Round ended", BloodLust049.Instance.Config.Debug);
            BloodLust049.Instance.Scp049 = null;
            BloodLust049.Instance.Scp049InGame = false;
        }
    }
}
