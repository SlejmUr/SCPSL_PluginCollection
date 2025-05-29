using CustomPlayerEffects;
using HarmonyLib;
using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader;
using LabApi.Loader.Features.Plugins;
using LabApiExtensions.Extensions;
using SimpleCustomRoles.Handler;
using SimpleCustomRoles.RoleGroup;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.SSS;

namespace SimpleCustomRoles;

internal class Main : Plugin<Config>
{
    public static Main Instance { get; private set; }

    #region Plugin Info
    public override string Author => "SlejmUr";
    public override string Name => "SimpleCustomRoles";
    public override Version Version => new(0, 5, 2);
    public override string Description => "Add simple YAML Support for creating custom roles.";
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
    #endregion

    public List<CustomRoleBaseInfo> InWaveRoles = [];
    public List<CustomRoleBaseInfo> ScpSpecificRoles = [];
    public List<CustomRoleBaseInfo> EscapeRoles = [];

    private ServerHandler serverHandler;
    private PlayerHandler playerHandler;
    private Scp0492Handler scp0492Handler;
    private Scp049Handler scp049Handler;
    private Scp096Handler scp096Handler;
    private Scp173Handler scp173Handler;
    private Scp330Handler scp330Handler;

    public List<RoleBaseGroup> RoleGroups = [];

    private Harmony Harmony;

    public override void Enable()
    {
        Instance = this;
        CustomDataStoreManagerExtended.EnsureExists<CustomRoleInfoStorage>();
        HelperTxts.WriteAll();
        Logic.Init();

        serverHandler = new();
        CustomHandlersManager.RegisterEventsHandler(serverHandler);

        playerHandler = new();
        CustomHandlersManager.RegisterEventsHandler(playerHandler);

        scp049Handler = new();
        CustomHandlersManager.RegisterEventsHandler(scp049Handler);

        scp0492Handler = new();
        CustomHandlersManager.RegisterEventsHandler(scp0492Handler);

        scp096Handler = new();
        CustomHandlersManager.RegisterEventsHandler(scp096Handler);

        scp173Handler = new();
        CustomHandlersManager.RegisterEventsHandler(scp173Handler);

        scp330Handler = new();
        CustomHandlersManager.RegisterEventsHandler(scp330Handler);

        StatusEffectBase.OnEnabled += SubHandle.StatusEffectBase_OnEnabled;
        Harmony.DEBUG = Config.Debug;
        Harmony = new("SimpleCustomRole");
        Harmony.PatchAll();
    }


    public override void Disable()
    {
        StatusEffectBase.OnEnabled -= SubHandle.StatusEffectBase_OnEnabled;


        CustomHandlersManager.RegisterEventsHandler(serverHandler);
        serverHandler = null;

        CustomHandlersManager.RegisterEventsHandler(playerHandler);
        playerHandler = null;

        CustomHandlersManager.RegisterEventsHandler(scp049Handler);
        scp049Handler = null;

        CustomHandlersManager.RegisterEventsHandler(scp0492Handler);
        scp0492Handler = null;

        CustomHandlersManager.RegisterEventsHandler(scp096Handler);
        scp096Handler = null;

        CustomHandlersManager.RegisterEventsHandler(scp173Handler);
        scp173Handler = null;

        CustomHandlersManager.RegisterEventsHandler(scp330Handler);
        scp330Handler = null;
        Logic.UnInit();
        RolesLoader.Clear();
        Harmony.UnpatchAll("SimpleCustomRole");
        Instance = null;
    }

    public override void LoadConfigs()
    {
        base.LoadConfigs();
        var list = this.LoadConfig<List<RoleBaseGroup>>("RoleGroups.yml");
        if (list != null)
            RoleGroups = list;
        else
            RoleGroups = [];
    }
}
