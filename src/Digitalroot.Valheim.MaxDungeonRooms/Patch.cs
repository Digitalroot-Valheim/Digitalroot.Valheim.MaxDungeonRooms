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

          var min = Main.Instance.MinRooms.Value;
          var max = Main.Instance.MaxRooms.Value;
          var applyChanges = true;

          switch (__instance.gameObject.name)
          {
            case "DG_ForestCrypt(Clone)":
              if (!Main.Instance.EnableForestCryptOverride.Value) break;
              min = Main.Instance.ForestCryptOverrideMinRooms.Value;
              max = Main.Instance.ForestCryptOverrideMaxRooms.Value;
              break;
          
            case "DG_SunkenCrypt(Clone)":
              if (!Main.Instance.EnableSunkenCryptOverride.Value) break;
              min = Main.Instance.SunkenCryptOverrideMinRooms.Value;
              max = Main.Instance.SunkenCryptOverrideMaxRooms.Value;
              break;

            case "DG_Cave(Clone)":
              if (!Main.Instance.EnableCaveOverride.Value) break;
              min = Main.Instance.CaveOverrideMinRooms.Value;
              max = Main.Instance.CaveOverrideMaxRooms.Value;
              break;

            case "DG_DvergrTown(Clone)":
              if (!Main.Instance.EnableDvergrTownOverride.Value) break;
              min = Main.Instance.DvergrTownOverrideMinRooms.Value;
              max = Main.Instance.DvergrTownOverrideMaxRooms.Value;
              break;

            default: // Room is unknown. Skip making any changes.
              applyChanges = false;
              break;
          }

          if (applyChanges)
          {
            __instance.m_minRooms = min;
            __instance.m_maxRooms = max;  
          }
          
          int i;
          for (i = 0; i < __instance.m_maxRooms; i++)
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
          Log.Debug(Main.Instance, "All required rooms have been placed, stopping generation");
          Log.Debug(Main.Instance, $"[{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}] __instance.gameObject.name : {__instance.gameObject.name}");
          Log.Debug(Main.Instance, $"[{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}] DungeonGenerator.m_placedRooms.Count : {DungeonGenerator.m_placedRooms.Count}");
          Log.Debug(Main.Instance, $"[{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}] Loc: {__instance.gameObject.transform.position}");
          Log.Debug(Main.Instance, $"Total attempts {i} of {__instance.m_minRooms}/{__instance.m_maxRooms}");
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
