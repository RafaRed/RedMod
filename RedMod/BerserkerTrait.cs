using System.Collections.Generic;
using RogueLibsCore;
using RogueLibsCore.Redmod;
using UnityEngine;

namespace RedMod
{
    class BerserkerTrait
    {

        public static bool berserkerMode = false;
        public static bool particleEffect = false;
        public GameObject particle;
        public static float timer = 0;
        public static bool returnNormal = false;
        public static bool setSkinToRed = false;
        public static Color32[] colors;



        public static void loadTrait()
        {

            CustomTrait trait = RogueLibs.CreateCustomTrait("WarCry", true,
            new CustomNameInfo("War Cry"),
            new CustomNameInfo("When you are low, you will be the enemies nightmare."));

            trait.IsActive = true;

            trait.Available = true;
            trait.AvailableInCharacterCreation = true;
            trait.CostInCharacterCreation = 6;
            trait.CanRemove = true;
            trait.CanSwap = true;
            trait.Upgrade = null;
            berserkerMode = false;
            returnNormal = false;
            setSkinToRed = false;


        }
        public static void Reset()
        {
            berserkerMode = false;
            returnNormal = false;

        }

        public static void update()
        {
            // Debug.Log("update");
            if (berserkerMode)
            {
                if (setSkinToRed)
                {
                    setSkinToRed = false;
                    RedModMain.player.objectSprite.agentColorDirty = false;

                    List<tk2dSprite> agentSpriteList3 = RedModMain.player.objectSprite.agentHitbox.agentSpriteList;

                    colors = new Color32[agentSpriteList3.Count];


                    for (int k = 0; k < agentSpriteList3.Count; k++)
                    {
                        if (!RedModMain.player.objectSprite.agent.usesNewHead)
                        {
                            if (RedModMain.player.objectSprite.agent.zombified)
                            {
                                agentSpriteList3[k].color = Color.yellow;
                            }
                            else if (RedModMain.player.objectSprite.agent.shapeShifter)
                            {
                                agentSpriteList3[k].color = Color.red;
                            }
                            else
                            {
                                colors[k] = agentSpriteList3[k].color;
                                agentSpriteList3[k].color = new Color32(254, 0, 0, 254);
                            }
                        }

                    }
                }
                //GameObject particle = RedModMain.player.gc.spawnerMain.SpawnParticleEffect("Fire", RedModMain.player.tr.position, 0f);
                //particle.transform.position = RedModMain.player.tr.position;
                if (timer <= 0)
                {

                    RedModMain.player.gc.spawnerMain.SpawnParticleEffect("Hack", RedModMain.player.tr.position, 0f);
                    timer = 2;
                }
                else
                {
                    timer -= Time.deltaTime;
                }

            }
            else
            {

                if (returnNormal)
                {
                    returnNormal = false;
                    RedModMain.player.objectSprite.agentColorDirty = false;
                    /* RedModMain.player.objectSprite.SetRenderer("Normal");
                     if (RedModMain.player.objectSprite.frozen)
                     {
                         RedModMain.player.objectSprite.agentColorDirty = true;
                     }*/
                    //RedModMain.player.objectSprite.sprH.color = RedModMain.player.objectSprite.playFieldObject.gc.normalColor;
                    //RedModMain.player.objectSprite.curShader[0] = "Normal";
                    List<tk2dSprite> agentSpriteList3 = RedModMain.player.objectSprite.agentHitbox.agentSpriteList;
                    for (int k = 0; k < agentSpriteList3.Count; k++)
                    {
                        if (!RedModMain.player.objectSprite.agent.usesNewHead)
                        {
                            if (RedModMain.player.objectSprite.agent.zombified)
                            {
                                agentSpriteList3[k].color = Color.yellow;
                            }
                            else if (RedModMain.player.objectSprite.agent.shapeShifter)
                            {
                                agentSpriteList3[k].color = Color.red;
                            }
                            else
                            {
                                agentSpriteList3[k].color = colors[k];
                                //RedModMain.player.objectSprite.SetAgentOff(0);

                            }
                        }

                    }
                }
            }



            if (RedModMain.player.statusEffects.hasTrait("WarCry"))
            {
                if (!berserkerMode && RedModMain.player.health < (RedModMain.player.healthMax / 2))
                {
                    berserkerMode = true;
                    setSkinToRed = true;
                    RedModMain.player.gc.audioHandler.Play(RedModMain.player, "UseNecronomicon");
                    RedModMain.player.Say("AAAAAAAAAAAA!!!");
                    RedModMain.player.gc.ScreenBump(1f, 30, RedModMain.player);
                    RedModMain.player.SetStrength(RedModMain.player.strengthStatMod + 4);
                    RedModMain.player.SetSpeed(RedModMain.player.speedStatMod + 2);
                    RedModMain.player.gc.playerControl.Vibrate(RedModMain.player.isPlayer, 0.5f, 0.2f);
                    Debug.Log("Berserker on!");



                }
                else if (RedModMain.player.health > (RedModMain.player.healthMax / 2) && berserkerMode)
                {
                    berserkerMode = false;
                    RedModMain.player.SetStrength(RedModMain.player.strengthStatMod - 4);
                    RedModMain.player.SetSpeed(RedModMain.player.speedStatMod - 2);
                    Debug.Log("Berserker off!");
                    returnNormal = true;

                }

            }
        }


    }
}
