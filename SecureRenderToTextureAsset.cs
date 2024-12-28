using FrooxEngine.ProtoFlux;
using HarmonyLib;
using ProtoFlux.Runtimes.Execution.Nodes.FrooxEngine.Rendering;
using ResoniteModLoader;
using System;
using System.Reflection;

namespace SecureRenderToTextureAsset;

public class SecureRenderToTextureAsset : ResoniteMod
{
    public override string Name => "SecureRenderToTextureAsset";
    public override string Author => "hantabaru1014";
    public override string Version => "1.0.0";
    public override string Link => "https://github.com/hantabaru1014/SecureRenderToTextureAsset";

    public override void OnEngineInit()
    {
        Harmony harmony = new Harmony("dev.baru.resonite.SecureRenderToTextureAsset");
        harmony.PatchAll();
    }

    [HarmonyPatch]
    class RunAsync_Prefix
    {
        static Type TargetClass()
        {
            return AccessTools.FirstInner(typeof(RenderToTextureAsset), t => t.Name.Contains("RunAsync"));
        }

        static MethodBase TargetMethod()
        {
            return AccessTools.Method(TargetClass(), "MoveNext");
        }

        public static bool Prefix(FrooxEngineContext ___context)
        {
            if (___context.Group.World.SessionId != ___context.Engine.WorldManager.FocusedWorld.SessionId)
            {
                Msg($"Blocked invalid render! node_world: {___context.Group.World.Name}, focused: {___context.Engine.WorldManager.FocusedWorld.Name}");
                return false;
            }
            return true;
        }
    }
}
