using System.Collections.Generic;
using Terraria.ModLoader.Config;

namespace NPCControl
{
    public class ConfigSpagetthi
    {
        
        public List<NPCDefinition> MakeInvincible { get; set; } = new List<NPCDefinition>();

        public List<NPCDefinition> DoNotSpawn { get; set; } = new List<NPCDefinition>();

        public ConfigSpagetthi(List<NPCDefinition> MakeInvincible, List<NPCDefinition> DoNotSpawn)
        {
            this.DoNotSpawn = DoNotSpawn;
            this.MakeInvincible = MakeInvincible;
        }
        public ConfigSpagetthi()
        {

        }
    }
}
