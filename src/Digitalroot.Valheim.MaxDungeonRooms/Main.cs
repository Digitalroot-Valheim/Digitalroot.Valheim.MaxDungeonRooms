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
  public class Main : BaseUnityPlugin, ITraceableLogging
  {
    public const string Version = "1.0.0";
    public const string Name = "Digitalroot Max Dungeon Rooms";
    public const string Guid = "digitalroot.mods.maxdungeonrooms";
    public const string Namespace = "Digitalroot.Valheim." + nameof(MaxDungeonRooms);
    private Harmony _harmony;
    public static Main Instance;

    public readonly ConfigEntry<int> NexusId;
    public ConfigEntry<int> MinRooms;
    public ConfigEntry<int> MaxRooms;

    public Main()
    {
      Instance = this;
      NexusId  = Config.Bind("General", "NexusID",   1665, new ConfigDescription("Nexus mod ID for updates.", null, new ConfigurationManagerAttributes { IsAdminOnly = false, Browsable = false, ReadOnly = true }));
#if DEBUG
      EnableTrace = true;
      Log.RegisterSource(Instance);
#else
      EnableTrace = false;
#endif
      Log.Trace(Instance, $"{Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
    }

    [UsedImplicitly]
    private void Awake()
    {
      try
      {
        Log.Trace(Instance, $"{Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

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
        Log.Trace(Instance, $"{Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
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
