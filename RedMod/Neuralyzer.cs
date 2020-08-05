using System.Collections.Generic;
using Light2D;
using RogueLibsCore;
using RogueLibsCore.Redmod;
using UnityEngine;

namespace RedMod
{
    class Neuralyzer
    {


        public static void blank(string[] redData)
        {
            Agent agent = Utils.getPlayer(int.Parse(redData[1]));
            List<Agent> agentlist = Utils.getAgentList(NetworkPackage.read_string_list(redData[2]));
            if(Utils.getId(agent) == Utils.getMyNetid())
            {
                agent.gc.ScreenBump(1f, 30, agent);
                agent.gc.playerControl.Vibrate(agent.isPlayer, 0.5f, 0.2f);
            }
            
            agent.objectSprite.Flash();
           
            foreach(Agent e in agentlist)
            {
                //if (haters[i].relationships.GetRelCode(myPlayer) != relStatus.Aligned)
                {
                    e.objectSprite.Flash();

                    e.pathing = 0;
                    e.movement.PathStop();
                    e.brainUpdate.slowAIWait = 0;
                    if (e.brain.Goals.Count > 0)
                    {
                        e.brain.RemoveAllSubgoals(e.brain.Goals[0]);
                    }
                    if (e.brain.Goals.Count > 0)
                    {
                        e.brain.Goals[0].Terminate();
                    }
                    e.relationships.SetRel(agent, "Neutral", true);
                    e.brain.Goals.Clear();
                    e.mostRecentGoal = "DoNothing";
                    e.mostRecentGoalCode = goalType.DoNothing;
                    e.inCombat = false;
                    e.inFleeCombat = false;
                    e.gc.spawnerMain.SpawnStateIndicator(e, "NoAnim");
                    e.gc.audioHandler.Play(e, "AgentAnnoyed");

                    

                    //haters[i].objectMultAgent.Set(myPlayer, 0);
                    e.statusEffects.CreateBuffText("Neuralyzed");


                }
            }
        }

        public static void flash(int agentid)
        {
            Agent playerAgent = Utils.getPlayer(agentid);
            List<string> list = new List<string>();
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
                           
                            list.Add(Utils.getId(agent).ToString());

                        }
                    }
                }
            }
            if (list != null && list.Count>0)
            {
                NetworkPackage package = new NetworkPackage(2);
                package.write(Utils.getId(GameController.gameController.playerAgent).ToString());
                package.write_string_list(list);
                RedNetwork.SendPackage(package);
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
            skill.Available = true;
            skill.CostInCharacterCreation = 9;

            skill.OnPressed = (item, agent) =>
            {
                if (item.invItemCount > 0) // is recharging
                    agent.gc.audioHandler.Play(agent, "CantDo");
                else
                {
                    flash(Utils.getMyNetid());
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

