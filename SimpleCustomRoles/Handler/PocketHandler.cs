using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Arguments.ServerEvents;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Stores;
using LabApi.Features.Wrappers;
using LabApiExtensions.Enums;
using LabApiExtensions.Extensions;
using MEC;
using SimpleCustomRoles.Helpers;
using SimpleCustomRoles.RoleInfo;
using SimpleCustomRoles.RoleYaml;
using SimpleCustomRoles.RoleYaml.Enums;

namespace SimpleCustomRoles.Handler;

public class PocketHandler : CustomEventsHandler
{
    // TODO: Add feature for custom roles to always/never escape, disable to enter to pocket.

    public override void OnPlayerEnteredPocketDimension(PlayerEnteredPocketDimensionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRoleStorage(ev.Player, out var storage))
            return;
        storage.ChangeScale();
    }

    public override void OnPlayerLeftPocketDimension(PlayerLeftPocketDimensionEventArgs ev)
    {
        if (!CustomRoleHelpers.TryGetCustomRoleStorage(ev.Player, out var storage))
            return;
        if (!ev.IsSuccessful)
            return;
        storage.ChangeScale();
    } 
}
