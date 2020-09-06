using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using System;
using System.Collections.Generic;
using Player = Exiled.Events.Handlers.Player;
using Server = Exiled.Events.Handlers.Server;

namespace BloodLust049
{
    public class BloodLust049 : Plugin<Config>
    {
        private static readonly Lazy<BloodLust049> LazyInstance = new Lazy<BloodLust049>(() => new BloodLust049());
        public static BloodLust049 Instance => LazyInstance.Value;

        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        public override string Name { get; } = "BloodLust049";
        public override string Author { get; } = "Steven4547466";
        public override Version Version { get; } = new Version(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(2, 1, 2);
        public override string Prefix { get; } = "BloodLust049";

        public List<CoroutineHandle> Coroutines = new List<CoroutineHandle>();

        private BloodLust049() { }

        public bool Scp049InGame { get; set; } = false;

        public Exiled.API.Features.Player Scp049 { get; set; } = null;
        public bool bloodLustActive { get; set; } = false;

        public Handlers.Player player { get; set; }

        public Handlers.Server server { get; set; }

        public bool mainCoroEnabled { get; set; }

        public CoroutineHandle Coro { get; set; }

        public override void OnEnabled()
        {
            if (BloodLust049.Instance.Config.IsEnabled == false) return;
            base.OnEnabled();
            Log.Info("BloodLust049 enabled.");
            RegisterEvents();
        }

        public override void OnDisabled()
        {
            base.OnDisabled();
            Log.Info("BloodLust049 disabled.");
            UnregisterEvents();
        }

        public override void OnReloaded()
        {
            base.OnReloaded();
            Log.Info("BloodLust049 reloading.");
        }

        public void RegisterEvents()
        {
            player = new Handlers.Player();
            Player.Died += player.OnDied;
            Player.Hurting += player.OnHurting;
            Player.ChangingRole += player.OnChangingRole;

            server = new Handlers.Server();
            Server.RoundStarted += server.OnRoundStarted;
            Server.RoundEnded += server.OnRoundEnded;
        }

        public void UnregisterEvents()
        {
            Log.Info("Events unregistered");
            mainCoroEnabled = false;

            Player.Died -= player.OnDied;
            Player.Hurting -= player.OnHurting;
            Player.ChangingRole -= player.OnChangingRole;

            player = null;

            Server.RoundStarted -= server.OnRoundStarted;
            Server.RoundEnded -= server.OnRoundEnded;

            server = null;

            foreach (CoroutineHandle handle in Coroutines)
            {
                Log.Debug($"Killed coro {handle}", BloodLust049.Instance.Config.Debug);
                Timing.KillCoroutines(handle);
            }

            Coroutines = null;
        }

        public IEnumerator<float> BloodLust()
        {
            Log.Debug("Coro started successfully", BloodLust049.Instance.Config.Debug);
            Random rnd = new Random();
            float firstTime = GetRandomNumber(rnd, Instance.Config.MinimumFirstTime, Instance.Config.MaximumFirstTime);
            yield return Timing.WaitForSeconds(firstTime);
            EnterBloodLust();
            yield return Timing.WaitForSeconds(Instance.Config.BloodLustDuration);
            LeaveBloodLust();
            while (Scp049InGame && mainCoroEnabled)
            {
                yield return Timing.WaitForSeconds(GetRandomNumber(rnd, Instance.Config.MinimumWaitTime, Instance.Config.MaximumWaitTime));
                EnterBloodLust();
                yield return Timing.WaitForSeconds(Instance.Config.BloodLustDuration);
                LeaveBloodLust();
            }
            mainCoroEnabled = false;
            Log.Debug($"Stopping Coro {Coro}", Instance.Config.Debug);
            Instance.Coroutines.Remove(Coro);
            Timing.KillCoroutines(Coro);
            yield break;
        }

        public void EnterBloodLust()
        {
            Log.Debug("Entered blood lust", BloodLust049.Instance.Config.Debug);
            bloodLustActive = true;
            if (Instance.Config.EnableAdrenalineEffect)
            {
                Invigorated adrenaline = Scp049.ReferenceHub.playerEffectsController.GetEffect<Invigorated>();
                adrenaline.Intensity = 5;
                Scp049.ReferenceHub.playerEffectsController.EnableEffect(adrenaline);
                Log.Debug("Adrenaline effect given", BloodLust049.Instance.Config.Debug);
            }
            
            if (Instance.Config.EnableBleedEffect)
            {
                Bleeding bleeding = Scp049.ReferenceHub.playerEffectsController.GetEffect<Bleeding>();
                Scp049.ReferenceHub.playerEffectsController.EnableEffect(bleeding);
                Log.Debug("Adrenaline effect given", BloodLust049.Instance.Config.Debug);
            }
            Scp207 scp207 = Scp049.ReferenceHub.playerEffectsController.GetEffect<Scp207>();
            scp207.Intensity = (byte) Instance.Config.Scp207Intensity;
            Scp049.ReferenceHub.playerEffectsController.EnableEffect(scp207);
            Log.Debug("207 effect given", BloodLust049.Instance.Config.Debug);
            Scp049.Broadcast(3, Instance.Config.BloodLustMessage);
        }

        public void LeaveBloodLust()
        {
            Log.Debug("Left blood lust", BloodLust049.Instance.Config.Debug);
            if (Instance.Config.EnableAdrenalineEffect) Scp049.ReferenceHub.playerEffectsController.DisableEffect<Invigorated>();
            if (Instance.Config.EnableBleedEffect) Scp049.ReferenceHub.playerEffectsController.DisableEffect<Bleeding>();
            Scp049.ReferenceHub.playerEffectsController.DisableEffect<Scp207>();
            bloodLustActive = false;
        }

        public float GetRandomNumber(Random rnd, double min, double max)
        {
            return (float)((rnd.NextDouble() * ((max - min) + 1)) + min);
        }
    }
}
