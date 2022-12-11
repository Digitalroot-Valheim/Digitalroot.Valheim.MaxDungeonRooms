using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace Digitalroot.Valheim.MaxDungeonRooms
{
  [UsedImplicitly]
  public class Patch
  {
    [UsedImplicitly]
    [HarmonyPatch(typeof(DungeonGenerator))]
    public class PatchDungeonGeneratorPlaceRooms
    {
      [HarmonyPrefix, HarmonyPriority(Priority.Normal)]
      [HarmonyPatch(typeof(DungeonGenerator), nameof(DungeonGenerator.PlaceRooms))]
      // ReSharper disable once InconsistentNaming
      public static bool Prefix([NotNull] ref DungeonGenerator __instance, ZoneSystem.SpawnMode mode)
      {
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}.{MethodBase.GetCurrentMethod()?.Name}");
          Log.Trace(Main.Instance, $"[{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}] {__instance.gameObject.name}");
          __instance.m_minRooms = Main.Instance.MinRooms.Value;
          __instance.m_maxRooms = Main.Instance.MaxRooms.Value;

          for (var i = 0; i < __instance.m_maxRooms; i++)
          {
            // Log.Trace(Main.Instance, $"i : {i}, DungeonGenerator.m_placedRooms.Count : {DungeonGenerator.m_placedRooms.Count}");
            __instance.PlaceOneRoom(mode);
            // Log.Trace(Main.Instance, $"__instance.CheckRequiredRooms() : {__instance.CheckRequiredRooms()}");
            // Log.Trace(Main.Instance, $"DungeonGenerator.m_placedRooms.Count > __instance.m_minRooms : {DungeonGenerator.m_placedRooms.Count > __instance.m_minRooms}");
            // Log.Trace(Main.Instance, $"i+1 < __instance.m_maxRooms : {i+1 < __instance.m_maxRooms}");
            if (__instance.CheckRequiredRooms() && DungeonGenerator.m_placedRooms.Count > __instance.m_minRooms)
            {
              ZLog.Log("All required rooms have been placed, stopping generation");
              break;
            }

            if (i + 1 > __instance.m_maxRooms)
            {
              Log.Trace(Main.Instance, $"i : {i}, DungeonGenerator.m_placedRooms.Count : {DungeonGenerator.m_placedRooms.Count}");
              Log.Trace(Main.Instance, $"__instance.CheckRequiredRooms() : {__instance.CheckRequiredRooms()}");
              Log.Trace(Main.Instance, $"DungeonGenerator.m_placedRooms.Count > __instance.m_minRooms : {DungeonGenerator.m_placedRooms.Count > __instance.m_minRooms}");
              Log.Trace(Main.Instance, $"i+1 < __instance.m_maxRooms : {i+1 < __instance.m_maxRooms}");
            }
          }
          Log.Trace(Main.Instance, $"[{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}] __instance.gameObject.name : {__instance.gameObject.name}");
          Log.Trace(Main.Instance, $"[{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}] DungeonGenerator.m_placedRooms.Count : {DungeonGenerator.m_placedRooms.Count}");
          Log.Trace(Main.Instance, $"[{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}] Loc: {__instance.gameObject.transform.position}");
        }
        catch (Exception e)
        {
          Log.Error(Main.Instance, e);
        }

        return false;
      }
    }
  }
}
