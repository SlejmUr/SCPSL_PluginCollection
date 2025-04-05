using LabApi.Events.CustomHandlers;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using Respawning;
using SimpleCustomRoles.Handler;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.SSS;

namespace SimpleCustomRoles;

internal class Main : Plugin<Config>
{
    public static Main Instance { get; private set; }

    #region Plugin Info
    public override string Author => "SlejmUr";
    public override string Name => "SimpleCustomRoles";
    public override Version Version => new(0, 4, 1);
    public override string Description => "Add simple YAML Support for creating custom roles.";
    public override Version RequiredApiVersion => new(LabApiProperties.CompiledVersion);
    #endregion
    public RolesLoader RolesLoader;
    public List<CustomRoleInfo> RegularRoles;
    public List<CustomRoleInfo> InWaveRoles;
    public List<CustomRoleInfo> AfterDeathRoles;
    public List<CustomRoleInfo> ScpSpecificRoles;
    public List<CustomRoleInfo> EscapeRoles;

    private ServerHandler serverHandler;
    private PlayerHandler playerHandler;
    private Scp0492Handler scp0492Handler;
    private Scp049Handler scp049Handler;
    private Scp096Handler scp096Handler;
    private Scp173Handler scp173Handler;
    private Scp330Handler scp330Handler;


    public override void Enable()
    {
        Instance = this;
        CustomDataStoreManagerExtended.EnsureExists<CustomRoleInfoStorage>();
        HelperTxts.WriteAll();
        RolesLoader = new();
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

        WaveManager.OnWaveSpawned += PlayerHandler.RespawnManager_ServerOnRespawned;
    }

    public override void Disable()
    {
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

        WaveManager.OnWaveSpawned -= PlayerHandler.RespawnManager_ServerOnRespawned;
        Logic.UnInit();
        RolesLoader.Clear();
        RolesLoader = null;
        Instance = null;
    }
}
