using Dissonance;
using Dissonance.Integrations.MirrorIgnorance;
using Dissonance.Networking;
using HarmonyLib;

namespace AudioPlayer.Patches
{
    [HarmonyPatch(typeof(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit>), nameof(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit>.RunAsDedicatedServer))]
    public static class RunDissonanceAsHost
    {
        public static bool Prefix(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit> __instance)
        {
            __instance.RunAsHost(Unit.None, Unit.None);
            return false;
        }
    }
}