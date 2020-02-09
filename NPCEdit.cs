using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.Config;


namespace NPCControl
{
    class NPCEdit : GlobalNPC
    {
        public static int Timer = 0;
        public static int Timermax = 200;
        public static NPCConfig Karl = new NPCConfig();
        public static Dictionary<int, bool> ListeUnbesiegbarer = new Dictionary<int, bool>();
        public static Dictionary<int, bool> ListeUnbesiegbarerLR = new Dictionary<int, bool>();
        //private NPCDefinition test;

        public override bool PreAI(NPC npc)
        {
            try
            {
                if (Timer >= Karl.TicksBetweenChecks)
                {
                    Timer = 0;
                    NPC editednpc = EditNPC(npc);
                    return base.PreAI(editednpc);
                }
                Timer++;
            }
            catch
            {
                GetNewConfig();
                if (Timer >= Karl.TicksBetweenChecks)
                {
                    Timer = 0;
                    NPC editednpc = EditNPC(npc);
                    return base.PreAI(editednpc);
                }
                Timer++;
            }
            return base.PreAI(npc);
        }
        
        // TESTSTUFF! DOESNT WORK! EDITSPAWNPOOL DOESN'T ALLOW EDIT OF VANILLA NPCS
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

        public override void SetDefaults(NPC npc)
        {
            try
            {
                NPC editednpc = EditNPC(npc);
                base.SetDefaults(editednpc);
            }
            catch
            {
                GetNewConfig();
                NPC editednpc = EditNPC(npc);
                base.SetDefaults(editednpc);
            }
            return;
        }

        public NPC EditNPC(NPC npc)
        {
            if (Karl == null)
            {
                return npc;
            }
            if (npc.active)
            {
                NPCDefinition test = new NPCDefinition(npc.type);

                //Do not Spawn
                if (Karl.DoNotSpawn.Contains(test))
                {
                    npc.active = false;
                    //if he's not allowed to exist, we can just stop here.
                    return npc;
                }

                //Invincible
                else if (Karl.MakeInvincible.Contains(test) || npc.townNPC && Karl.TownInvincible)
                {
                    if (!npc.dontTakeDamage)
                    {
                        //Make a "Backup" of the stats of the NPC before
                        if (!ListeUnbesiegbarer.ContainsKey(npc.type))
                        {
                            ListeUnbesiegbarer.Add(npc.type, npc.dontTakeDamage);
                            ListeUnbesiegbarerLR.Add(npc.type, npc.dontTakeDamageFromHostiles);
                        }
                        //Anti-Invincible-Boss-Maker
                        if (npc.boss && Karl.PreventInvincibleBosses)
                        {
                            Main.NewText("Invincible Bosses won't be spawned", Color.Red);
                            npc.active = false;
                        }
                        //Make NPC Invincible
                        else
                        {
                            //"best way imo is just to override CanBeHitByItem and CanBeHitByProjectile, then return false if the npc shouldn't be hittable, null otherwise"
                            if (npc.townNPC && Karl.TownInvincible && Karl.MakeInvincible.Contains(test))
                            {
                                npc.dontTakeDamage = false;
                                npc.dontTakeDamageFromHostiles = false;
                            }
                            else
                            {
                                npc.dontTakeDamage = true;
                                npc.dontTakeDamageFromHostiles = true;
                            }
                        }
                        return npc;
                    }
                    else
                    {
                        if (npc.townNPC && Karl.TownInvincible && Karl.MakeInvincible.Contains(test))
                        {
                            npc.dontTakeDamage = false;
                            npc.dontTakeDamageFromHostiles = false;
                        }
                        return npc;
                    }
                }

                //if both are false, Invincibility has to be undone (using the defaults saved above)
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
                return npc;
            }
            return npc;
        }

        //actually gets never called, but it's a nice have for the future
        public void GetNewConfig()
        {
            Karl = ModContent.GetInstance<NPCConfig>();
        }


        
    }
}
