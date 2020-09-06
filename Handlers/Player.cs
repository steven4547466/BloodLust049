using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;

namespace BloodLust049.Handlers
{
    public class Player
    {
        public void OnDied(DiedEventArgs ev)
        {
            if(!BloodLust049.Instance.Config.DisableInstantRevive && ev.Killer.Role == RoleType.Scp049 && (BloodLust049.Instance.bloodLustActive || BloodLust049.Instance.Config.AlwaysInstantRevive))
            {
                Log.Debug("049 killed player, respawning", BloodLust049.Instance.Config.Debug);
                Timing.CallDelayed(0.5f, () =>
                {
                    ev.Target.Role = RoleType.Scp0492;
                });
            }

            if(ev.Target.Role == RoleType.Scp049) BloodLust049.Instance.Scp049InGame = false;
        }

        public void OnHurting(HurtingEventArgs ev)
        {
            if (BloodLust049.Instance.bloodLustActive && ev.Target.Role == RoleType.Scp049 && (ev.DamageType == DamageTypes.Scp207 || ev.DamageType == DamageTypes.Bleeding))
            {
                ev.IsAllowed = false;
            }
        }

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.NewRole == RoleType.Scp049 && !BloodLust049.Instance.Scp049InGame)
            {
                Log.Debug("New role 049", BloodLust049.Instance.Config.Debug);
                BloodLust049.Instance.Scp049 = ev.Player;
                BloodLust049.Instance.Scp049InGame = true;
                if (!BloodLust049.Instance.mainCoroEnabled)
                {
                    Log.Debug("Coro enabled", BloodLust049.Instance.Config.Debug);
                    BloodLust049.Instance.mainCoroEnabled = true;
                    BloodLust049.Instance.Coro = Timing.RunCoroutine(BloodLust049.Instance.BloodLust());
                    BloodLust049.Instance.Coroutines.Add(BloodLust049.Instance.Coro);
                }
            }
        }
    }
}
