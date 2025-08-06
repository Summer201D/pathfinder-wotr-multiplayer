using System;
using System.Reflection;
using HarmonyLib;

namespace WOTRMultiplayer.HarmonyPatches
{
    public static class PatchesUtils
    {
        public static bool IsHelperUnit(string unitId)
        {
            return !string.IsNullOrEmpty(unitId) && unitId.StartsWith("description-helper-", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetTranspilerTarget(MethodBase method)
        {
            var attr = method.GetCustomAttribute<HarmonyPatch>();
            var target = $"{attr.info.declaringType.Name}.{attr.info.methodName ?? attr.info.methodType?.ToString()} ({method.Name})";
            return target;
        }
    }
}
