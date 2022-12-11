using BepInEx;
using BepInEx.Configuration;
using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Utils;
using System;
using System.Reflection;

namespace Digitalroot.Valheim.MaxDungeonRooms
{
  [BepInPlugin(Guid, Name, Version)]
  [BepInDependency(Jotunn.Main.ModGuid)]
  [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
  public partial class Main : BaseUnityPlugin, ITraceableLogging
  {
    private Harmony _harmony;
    public static Main Instance;

    public static ConfigEntry<int> NexusId;
    public ConfigEntry<int> MinRooms;
    public ConfigEntry<int> MaxRooms;

    public Main()
    {
      try
      {
        #if DEBUG
        EnableTrace = true;
        #else
        EnableTrace = false;
        #endif
        Instance = this;
        NexusId = Config.Bind("General", "NexusID", 1665, new ConfigDescription("Nexus mod ID for updates", null, new ConfigurationManagerAttributes { Browsable = false, ReadOnly = true }));
        Log.RegisterSource(Instance);
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod()?.Name}()");
      }
      catch (Exception e)
      {
        ZLog.LogError(e);
      }
    }

    [UsedImplicitly]
    private void Awake()
    {
      try
      {
        Log.Trace(Instance, $"{Namespace}.{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}.{MethodBase.GetCurrentMethod()?.Name}");

        Config.SaveOnConfigSet = true;
        
        MinRooms = Config.Bind("General", "Min Rooms", 20, new ConfigDescription("Min number of rooms in a dungeon.", new AcceptableValueRange<int>(20, 1000), new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true}));
        MaxRooms = Config.Bind("General", "Max Rooms", 40, new ConfigDescription("Max number of rooms in a dungeon.", new AcceptableValueRange<int>(30, 1000), new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true}));

        _harmony = Harmony.CreateAndPatchAll(typeof(Main).Assembly, Guid);
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
      try
      {
        Log.Trace(Instance, $"{Namespace}.{MethodBase.GetCurrentMethod()?.DeclaringType?.Name}.{MethodBase.GetCurrentMethod()?.Name}");
        _harmony?.UnpatchSelf();
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    #region Implementation of ITraceableLogging

    /// <inheritdoc />
    public string Source => Namespace;

    /// <inheritdoc />
    public bool EnableTrace { get; }

    #endregion
  }
}
