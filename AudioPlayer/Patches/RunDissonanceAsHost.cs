using System.Collections.Generic;
using System.Reflection.Emit;
using Dissonance;
using Dissonance.Integrations.MirrorIgnorance;
using Dissonance.Networking;
using HarmonyLib;
using NorthwoodLib.Pools;

namespace AudioPlayer.Patches
{
    [HarmonyPatch(typeof(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit>), nameof(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit>.RunAsDedicatedServer))]
    public static class RunDissonanceAsHost
    { 
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var newInstructions = ListPool<CodeInstruction>.Shared.Rent();
            
            newInstructions.AddRange(new CodeInstruction[]
            {
                new (OpCodes.Ldarg_0),
                new (OpCodes.Ldsfld, AccessTools.Field(typeof(Unit),nameof(Unit.None))),
                new (OpCodes.Ldsfld, AccessTools.Field(typeof(Unit),nameof(Unit.None))),
                new (OpCodes.Callvirt, AccessTools.Method(typeof(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit>), nameof(BaseCommsNetwork<MirrorIgnoranceServer, MirrorIgnoranceClient, MirrorConn, Unit, Unit>.RunAsHost)))
            });

            foreach (var instruction in newInstructions)
                yield return instruction;
            
            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }
    }
}