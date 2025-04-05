using Exiled.API.Features;
using MapEditorReborn.Events.Handlers;
using MapEditorReborn.API.Features;
using MapEditorReborn.Events.EventArgs;
using Exiled.API.Enums;

namespace Vault
{
    public class VaultMain : Plugin<Config>
    {
        public VaultMain Instance;

        public AnimationController SafeDoorAnim;
        public override PluginPriority Priority => PluginPriority.Last;
        public override void OnEnabled()
        {
            Instance = this;
            Schematic.SchematicSpawned += Spawned;
            Schematic.ButtonInteracted += ButtonInteracted;
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            Schematic.SchematicSpawned -= Spawned;
            base.OnDisabled();
        }

        public void Spawned(SchematicSpawnedEventArgs ev)
        {
            Log.Info(ev.Name);
            if (ev.Name != "SafeDoor")
                return;
            SafeDoorAnim = AnimationController.Get(ev.Schematic);
            //SafeDoorAnim.Play("SafeAnim");
        }

        public void ButtonInteracted(ButtonInteractedEventArgs ev)
        {
            if (SafeDoorAnim.AttachedSchematic != ev.Schematic)
                return;
            // Animate opening
            // Use  the name of one of the logged anim here.
            SafeDoorAnim.Play("SafeAnim");
        }
    }
}
