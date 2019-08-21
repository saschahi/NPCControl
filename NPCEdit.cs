using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Config;


namespace NPCControl
{
    class NPCEdit : GlobalNPC
    {
        //static Dictionary<int, bool> npcInvincible;
        //static ConfigToucher Karl = new ConfigToucher();
        public static int Timer = 0;
        public static NPCConfig Karl = new NPCConfig();
        //public static List<int> ListeUnbesiegbarer = new List<int>();
        public static Dictionary<int, int> ListeUnbesiegbarer = new Dictionary<int, int>();
        public static Dictionary<int, int> ListeUnbesiegbarerLR = new Dictionary<int, int>();
        public override bool PreAI(NPC npc)
        {
            NPCDefinition test = new NPCDefinition(npc.type);


            //Unspawnbar
            if (Karl.DoNotSpawn.Contains(test))
            {
                npc.active = false;
                
                //If he doesn't spawn we can stop right here
                if (Timer > 3000)
                {
                    Karl = mod.GetConfig<NPCConfig>();
                    Timer = 0;
                }
                Timer++;
                return base.PreAI(npc);
            }


            //Unbesiegbar
            else if (Karl.MakeInvincible.Contains(test))
            {
                if (!npc.dontTakeDamage)
                {
                    if (!ListeUnbesiegbarer.ContainsKey(npc.type))
                    {
                        ListeUnbesiegbarer.Add(npc.type, npc.lifeMax);
                        ListeUnbesiegbarerLR.Add(npc.type, npc.lifeRegen);
                    }
                    if (npc.boss)
                    {
                        Main.NewText("Invincible Bosses won't be spawned", Color.Red);
                        npc.active = false;
                    }
                    else
                    {
                        npc.dontTakeDamage = true;
                        npc.dontTakeDamageFromHostiles = true;
                        npc.lifeRegen = npc.lifeMax;
                        if (npc.lifeMax < 1000000)
                        {
                            npc.lifeMax = 1000000;
                        }
                    }

                    //getting here means atleast 1 is active. going further would GUARANTEE that both are false.
                    if (Timer > 3000)
                    {
                        Karl = mod.GetConfig<NPCConfig>();
                        Timer = 0;
                    }
                    Timer++;
                    return base.PreAI(npc);
                }
                else
                {
                    if (Timer > 3000)
                    {
                        Karl = mod.GetConfig<NPCConfig>();
                        Timer = 0;
                    }
                    Timer++;
                    return base.PreAI(npc);
                }
            }

            //if both are false, Invincibility has to be undone.
            //regardless of how shitty of a way this is, this uses the least ressources.
            else if(ListeUnbesiegbarer.ContainsKey(npc.type))
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    if (Main.npc[i].active && Main.npc[i].type == npc.type)
                    {
                        Main.npc[i].dontTakeDamage = false;
                        Main.npc[i].dontTakeDamageFromHostiles = false;
                        Main.npc[i].lifeRegen = ListeUnbesiegbarerLR[npc.type];
                        Main.npc[i].lifeMax = ListeUnbesiegbarer[npc.type];
                        Main.npc[i].life = Main.npc[i].lifeMax;
                        
                    }


                }
                 

                ListeUnbesiegbarer.Remove(npc.type);
                ListeUnbesiegbarerLR.Remove(npc.type);
            }

            
            if (Timer > 3000)
            {
                Karl = mod.GetConfig<NPCConfig>();
                Timer = 0;
            }

            Timer++;
            return base.PreAI(npc);
        }
    }
}
