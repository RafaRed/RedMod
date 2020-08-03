using System.Collections.Generic;
using RogueLibsCore;
using UnityEngine;

namespace RedMod
{
    class Neuralyzer
    {


        static void blank(Agent myPlayer, List<Agent> haters)
        {
            //myPlayer.objectSprite.Flash();
            //myPlayer.SpawnParticleEffect("BulletTrailMindControl", myPlayer.tr.position);
            myPlayer.gc.ScreenBump(1f, 30, myPlayer);
            //myPlayer.objectSprite.Flash();
            myPlayer.objectMultAgent.AgentFlash();
            myPlayer.gc.playerControl.Vibrate(myPlayer.isPlayer, 0.5f, 0.2f);
            for (int i = 0; i < haters.Count; i++)
            {
                //if (haters[i].relationships.GetRelCode(myPlayer) != relStatus.Aligned)
                {
                    haters[i].objectSprite.Flash();

                    /*haters[i].pathing = 0;
                    haters[i].movement.PathStop();
                    haters[i].brainUpdate.slowAIWait = 0;
                    if (haters[i].brain.Goals.Count > 0)
                    {
                        haters[i].brain.RemoveAllSubgoals(haters[i].brain.Goals[0]);
                    }
                    if (haters[i].brain.Goals.Count > 0)
                    {
                        haters[i].brain.Goals[0].Terminate();
                    }
                    haters[i].brain.Goals.Clear();
                    haters[i].mostRecentGoal = "DoNothing";
                    haters[i].mostRecentGoalCode = goalType.DoNothing;
                    haters[i].inCombat = false;
                    haters[i].inFleeCombat = false;
                    haters[i].gc.spawnerMain.SpawnStateIndicator(haters[i], "NoAnim");*/
                    haters[i].gc.audioHandler.Play(haters[i], "AgentAnnoyed");

                    myPlayer.objectMultAgent.SetRel(haters[i], "Neutral", true);

                    //haters[i].objectMultAgent.Set(myPlayer, 0);
                    haters[i].statusEffects.CreateBuffText("Neuralyzed");


                }
            }
        }

        static void flash()
        {
            Agent playerAgent = GameController.gameController.playerAgent;
            List<Agent> list = new List<Agent>();
            for (int i = 0; i < GameController.gameController.agentList.Count; i++)
            {
                Agent agent = GameController.gameController.agentList[i];
                if (Vector2.Distance(agent.tr.position, playerAgent.tr.position) < agent.LOSRange / playerAgent.hardToSeeFromDistance && agent != playerAgent)
                {
                    Relationship relationship = agent.relationships.GetRelationship(playerAgent);
                    if (relationship != null && agent.movement.HasLOSAgent360(playerAgent) && agent.movement.HasLOSAgent360(playerAgent))
                    {

                        relStatus relTypeCode = relationship.relTypeCode;
                        Relationship relationship2 = playerAgent.relationships.GetRelationship(agent);
                        relStatus relTypeCode2 = relationship2.relTypeCode;
                        //if (relTypeCode == relStatus.Annoyed || agent.relationships.GetRel(playerAgent) == "Hateful")
                        {
                            list.Add(agent);
                        }
                    }
                }
            }
            if (list != null)
            {
                blank(playerAgent, list);
            }



            //agent.relationships.SetRel(interactingAgent, "Neutral");

        }

        public static void LoadSkill()
        {
            Sprite skillSprite = RogueUtilities.ConvertToSprite(RedMod.Properties.Resources.questionmark);
            CustomAbility skill = RogueLibs.CreateCustomAbility("neuralyzer", skillSprite, true,
                new CustomNameInfo("Neuralyzer"),
                new CustomNameInfo("It has the ability to wipe the mind of anybody who sees the flash."),
                item =>
                {

                });


            skill.OnPressed = (item, agent) =>
            {
                if (item.invItemCount > 0) // is recharging
                    agent.gc.audioHandler.Play(agent, "CantDo");
                else
                {
                    flash();

                    agent.inventory.buffDisplay.specialAbilitySlot.MakeNotUsable();
                    // make special ability slot half-transparent
                    item.invItemCount = 4; // 100 x 0.13f = 13 seconds to recharge
                                           // or you can replace 100 with 13, and 0.13 with 1 to make it simpler
                }
            };

            skill.RechargeInterval = (item, agent)
                => item.invItemCount > 0 ? new WaitForSeconds(1f) : null;

            skill.Recharge = (item, agent) =>
            {
                if (item.invItemCount > 0 && agent.statusEffects.CanRecharge())
                { // if can recharge
                    item.invItemCount--;
                    if (item.invItemCount == 0) // ability recharged
                    {
                        agent.statusEffects.CreateBuffText("Recharged", agent.objectNetID);
                        agent.gc.audioHandler.Play(agent, "Recharge");
                        agent.inventory.buffDisplay.specialAbilitySlot.MakeUsable();
                        // make special ability slot fully visible
                    }
                }
            };
        }
    }


}

