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
    public ConfigEntry<bool> EnableForestCryptOverride;
    public ConfigEntry<int> ForestCryptOverrideMinRooms;
    public ConfigEntry<int> ForestCryptOverrideMaxRooms;
    public ConfigEntry<bool> EnableSunkenCryptOverride;
    public ConfigEntry<int> SunkenCryptOverrideMinRooms;
    public ConfigEntry<int> SunkenCryptOverrideMaxRooms;
    public ConfigEntry<bool> EnableCaveOverride;
    public ConfigEntry<int> CaveOverrideMinRooms;
    public ConfigEntry<int> CaveOverrideMaxRooms;
    public ConfigEntry<bool> EnableDvergrTownOverride;
    public ConfigEntry<int> DvergrTownOverrideMinRooms;
    public ConfigEntry<int> DvergrTownOverrideMaxRooms;


    public Main()
    {
      try
      {
        #if DEBUG
        EnableTrace = true;
        #else
        EnableTrace = true;
        #endif
        Instance = this;
        NexusId = Config.Bind("1. General", "NexusID", 1665, new ConfigDescription("Nexus mod ID for updates", null, new ConfigurationManagerAttributes { Browsable = false, ReadOnly = true }));
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

        MinRooms = Config.Bind("1. General", "Min Rooms", 20, new ConfigDescription("Min number of rooms in a dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true, Order = 1}));
        MaxRooms = Config.Bind("1. General", "Max Rooms", 40, new ConfigDescription("Max number of rooms in a dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true, Order = 0}));
        
        // Blackforest
        EnableForestCryptOverride = Config.Bind("2. Forest Crypt Overrides", "Enable Forest Crypt Override", false, new ConfigDescription("Min number of rooms in a Forest Crypt dungeon.", tags: new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true, Order = 100, IsAdvanced = true}));
        ForestCryptOverrideMinRooms = Config.Bind("2. Forest Crypt Overrides", "Forest Crypt Min Rooms", 20, new ConfigDescription("Min number of rooms in a Forest Crypt dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true, Order = 1, IsAdvanced = true}));
        ForestCryptOverrideMaxRooms = Config.Bind("2. Forest Crypt Overrides", "Forest Crypt Max Rooms", 40, new ConfigDescription("Max number of rooms in a Forest Crypt dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true, Order = 0, IsAdvanced = true}));
        
        // Swamp
        EnableSunkenCryptOverride = Config.Bind("3. Sunken Crypt Overrides", "Enable Sunken Crypt Override", false, new ConfigDescription("Min number of rooms in a Sunken Crypt dungeon.", tags: new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true, Order = 100, IsAdvanced = true}));
        SunkenCryptOverrideMinRooms = Config.Bind("3. Sunken Crypt Overrides", "Sunken Crypt Min Rooms", 20, new ConfigDescription("Min number of rooms in a Sunken Crypt dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true, Order = 1, IsAdvanced = true}));
        SunkenCryptOverrideMaxRooms = Config.Bind("3. Sunken Crypt Overrides", "Sunken Crypt Max Rooms", 30, new ConfigDescription("Max number of rooms in a Sunken Crypt dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes { IsAdminOnly = true, Browsable = true, Order = 0, IsAdvanced = true}));

        // Mount
        EnableCaveOverride = Config.Bind("4. Cave Overrides", "Enable Cave Override", false, new ConfigDescription("Min number of rooms in a Cave dungeon.", tags: new ConfigurationManagerAttributes {IsAdminOnly = true, Browsable = true, Order = 100, IsAdvanced = true }));
        CaveOverrideMinRooms = Config.Bind("4. Cave Overrides", "Cave Min Rooms", 20, new ConfigDescription("Min number of rooms in a Cave dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes {IsAdminOnly = true, Browsable = true, Order = 1, IsAdvanced = true }));
        CaveOverrideMaxRooms = Config.Bind("4. Cave Overrides", "Cave Max Rooms", 40, new ConfigDescription("Max number of rooms in a Cave dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes {IsAdminOnly = true, Browsable = true, Order = 0, IsAdvanced = true }));

        // Mistlands
        EnableDvergrTownOverride = Config.Bind("5. Dvergr Town Overrides", "Enable Dvergr Town Override", false, new ConfigDescription("Min number of rooms in a Dvergr Town dungeon.", tags: new ConfigurationManagerAttributes {IsAdminOnly = true, Browsable = true, Order = 100, IsAdvanced = true }));
        DvergrTownOverrideMinRooms = Config.Bind("5. Dvergr Town Overrides", "Dvergr Town Min Rooms", 20, new ConfigDescription("Min number of rooms in a Dvergr Town dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes {IsAdminOnly = true, Browsable = true, Order = 1, IsAdvanced = true }));
        DvergrTownOverrideMaxRooms = Config.Bind("5. Dvergr Town Overrides", "Dvergr Town Max Rooms", 40, new ConfigDescription("Max number of rooms in a Dvergr Town dungeon.", new AcceptableValueRange<int>(10, 1000), new ConfigurationManagerAttributes {IsAdminOnly = true, Browsable = true, Order = 0, IsAdvanced = true }));

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
