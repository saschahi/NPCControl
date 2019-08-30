using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Config;


namespace NPCControl
{
    class NPCEdit : GlobalNPC
    {
        //public static int Timer = 0;
        public static int Timer2 = 0;
        public static NPCConfig Karl = new NPCConfig();
        public static Dictionary<int, bool> ListeUnbesiegbarer = new Dictionary<int, bool>();
        public static Dictionary<int, bool> ListeUnbesiegbarerLR = new Dictionary<int, bool>();
        //private NPCDefinition test;

        public override bool PreAI(NPC npc)
        {
            if (npc.active)
            {
                NPCDefinition test = new NPCDefinition(npc.type);


                //Unspawnbar
                if (Karl.DoNotSpawn.Contains(test))
                {

                    npc.active = false;

                    //for some reason this breaks my mod...
                    //Main.NewText(npc.TypeName + " was deactivated by an Admin", Color.Red);


                    /*If he doesn't spawn we can stop right here
                    if (Timer > 3000)
                    {
                        Karl = mod.GetConfig<NPCConfig>();
                        Timer = 0;
                    }
                    Timer++;*/
                    return base.PreAI(npc);
                }


                //Unbesiegbar
                else if (Karl.MakeInvincible.Contains(test))
                {
                    if (!npc.dontTakeDamage)
                    {
                        if (!ListeUnbesiegbarer.ContainsKey(npc.type))
                        {
                            ListeUnbesiegbarer.Add(npc.type, npc.dontTakeDamage);
                            ListeUnbesiegbarerLR.Add(npc.type, npc.dontTakeDamageFromHostiles);
                        }
                        if (npc.boss)
                        {
                            Main.NewText("Invincible Bosses won't be spawned", Color.Red);
                            npc.active = false;

                        }
                        else
                        {
                            //"best way imo is just to override CanBeHitByItem and CanBeHitByProjectile, then return false if the npc shouldn't be hittable, null otherwise"
                            npc.dontTakeDamage = true;
                            npc.dontTakeDamageFromHostiles = true;
                        }

                        //getting here means atleast 1 is active. going further would GUARANTEE that both are false.
                        /*If he doesn't spawn we can stop right here
                        if (Timer > 3000)
                        {
                            Karl = mod.GetConfig<NPCConfig>();
                            Timer = 0;
                        }
                        Timer++;*/
                        return base.PreAI(npc);
                    }
                    else
                    {
                        /*If he doesn't spawn we can stop right here
                        if (Timer > 3000)
                        {
                           Karl = mod.GetConfig<NPCConfig>();
                            Timer = 0;
                        }
                        Timer++;*/
                        return base.PreAI(npc);
                    }
                }

                //if both are false, Invincibility has to be undone.
                //regardless of how shitty of a way this is, this uses the least ressources.
                else if (ListeUnbesiegbarer.ContainsKey(npc.type))
                {
                    for (int i = 0; i < Main.npc.Length; i++)
                    {
                        if (Main.npc[i].active && Main.npc[i].type == npc.type)
                        {
                            Main.npc[i].dontTakeDamage = ListeUnbesiegbarer[npc.type];
                            Main.npc[i].dontTakeDamageFromHostiles = ListeUnbesiegbarerLR[npc.type];
                            Main.npc[i].life = Main.npc[i].lifeMax;
                        }
                    }
                    ListeUnbesiegbarer.Remove(npc.type);
                    ListeUnbesiegbarerLR.Remove(npc.type);
                }


                /*If he doesn't spawn we can stop right here
                if (Timer > 3000)
                {
                    Karl = mod.GetConfig<NPCConfig>();
                    Timer = 0;
                }
                Timer++;*/
                return base.PreAI(npc);
            }
            return base.PreAI(npc);
        }

        /*public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            NPCDefinition test = new NPCDefinition();
            IDictionary<int, float> blub = new Dictionary<int, float>();

            foreach (var item in pool)
            {
                test = new NPCDefinition(item.Key + 1);
                if (!Karl.DoNotSpawn.Contains(test))
                {
                    blub.Add(item);
                }
            }
            base.EditSpawnPool(blub, spawnInfo);
        }*/
        
        public void GetNewConfig()
        {
            Karl = mod.GetConfig<NPCConfig>();
        }


        
    }
}
