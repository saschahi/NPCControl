using System.Collections.Generic;
using Terraria.ModLoader.Config;
using Terraria;
using Terraria.ID;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.ModLoader;
using System.ComponentModel;

namespace NPCControl
{
    public class NPCConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        // tModLoader will automatically populate a public static field named Instance with the active instance of this ModConfig. (It will unload it too.)
        // This reduces code from "mod.GetConfig<ExampleConfigServer>().DisableExampleWings" to "ExampleConfigServer.Instance.DisableExampleWings". It's just a style choice.
        public static NPCConfig Instance;

        [Label("Time in Ticks between NPC Checks")]
        [Range(1, 500)]
        [Slider]
        [DefaultValue(30)]
        public int TicksBetweenChecks;

        [Label("Invincible NPCs")]
        [Tooltip("All NPCs in here will become Invincible")]
        public List<NPCDefinition> MakeInvincible { get; set; } = new List<NPCDefinition>();

        [Label("Make All Town-NPCs invincible")]
        [Tooltip("Will automatically make Town-NPCs invincible (Guide etc.) without having to put them in the list above")]
        [DefaultValue(false)]
        public bool TownInvincible { get; set; } = new bool();

        //Looking into making this possible...

        //[Label("Make all Critters Invincible")]
        //[Tooltip("Will automatically make Critters invincible (Bunnies, squirrels, etc.) without having to put them in the list above")]
        //public bool CritterInvincible { get; set; } = new bool();

        [Label("Prevent Invincible Bosses")]
        [Tooltip("If on, will not allow the spawning of Invinvcible bosses")]
        [DefaultValue(true)]
        public bool PreventInvincibleBosses { get; set; } = new bool();

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

        public override void OnChanged()
        {
            NPCEdit.Karl = ModContent.GetInstance<NPCConfig>();
        }
    }
}
