using System.Collections.Generic;
using Terraria.ModLoader.Config;


namespace NPCControl
{
    public class NPCConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        // tModLoader will automatically populate a public static field named Instance with the active instance of this ModConfig. (It will unload it too.)
        // This reduces code from "mod.GetConfig<ExampleConfigServer>().DisableExampleWings" to "ExampleConfigServer.Instance.DisableExampleWings". It's just a style choice.
        public static NPCConfig Instance;

      
        [Label("Invincible NPCs")]
        [Tooltip("All NPCs in here will become Invincible")]
        public List<NPCDefinition> MakeInvincible { get; set; } = new List<NPCDefinition>();

        [Label("Disabled Enemies")]
        [Tooltip("All NPCs in here will be prevented from Spawning")]
        public List<NPCDefinition> DoNotSpawn { get; set; } = new List<NPCDefinition>();

        

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            if (NPCControl.instance.herosmod != null)
            {
                //find a better alternative?
                if (NPCControl.instance.herosmod.Call("HasPermission", whoAmI, NPCControl.heropermission) is bool result && result)
                {
                    return true;
                }

            }
            else
            {
                return true;
            }
            return false;
        }
    }
}
