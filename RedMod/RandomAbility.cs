using System;
using System.Collections;
using System.Collections.Generic;
using RogueLibsCore;
using RogueLibsCore.Redmod;
using UnityEngine;

namespace RedMod
{
    class RandomAbility : MonoBehaviour
    {

        public static Agent thisAgent;
        public static int abilityPrice = 12;
        public static bool activated = false;


        public static void buffEverybody(string id)
        {

            ObjectMult server = null;

            List<Agent> players = new List<Agent>();
            foreach (Agent a in GameController.gameController.agentList)
            {
                if (a.isPlayer != 0) players.Add(a);
                if (a.isPlayer != 0 && a.objectMult.isServer)
                {
                    server = a.objectMult;
                }
            }



            foreach (Agent a in players)
            {

                if (activated)
                {
                    a.statusEffects.RemoveStatusEffect(id);
                }
                else
                {

                    GameController.gameController.playerAgent.objectMult.CallCmdAddStatusEffect(id, a.objectNetID, true, true, 5);
                    a.statusEffects.AddStatusEffect(id, a, a, a.objectMult.IsFromClient(), true, 1);
                    // RedNetwork.AddStatusEffectOthers(id, true, GameController.gameController.playerAgent, a, GameController.gameController.playerAgent.objectMult.IsFromClient(), true, -1);
                    //a.objectMult.AddStatusEffect(id, true, a, a.objectMult.IsFromClient(), true, -1);
                    //a.objectMult.CallCmdAddStatusEffect(id, a.objectNetID, true, true, 5);
                    //a.objectMult.CallRpcAddStatusEffect(id, a.objectNetID, true, a.objectMult.IsFromClient(), true, 5);
                    //a.objectMult.AddStatusEffect(id,true,thisAgent,thisAgent.objectMult.IsFromClient(),true,-1);
                    //Debug.Log("server:" + a.objectMult.ToString());
                    /* if (!thisAgent.objectMult.gc.serverPlayer && thisAgent.objectMultAgent.isLocalPlayer && thisAgent.objectMult.IsFromClient() != NetworkInstanceId.Invalid)
                     {
                         a.objectMult.CallCmdAddStatusEffect(id, thisAgent.objectNetID, true, true, 5);
                         Debug.Log("CmdAddStatusEffect: " + id);
                     }
                     else if (thisAgent.objectMult.gc.serverPlayer)
                     {
                         thisAgent.objectMult.CallRpcAddStatusEffect(id, thisAgent.objectNetID, true, thisAgent.objectMult.IsFromClient(), true, 5);
                         Debug.Log("RpcAddStatusEffect: " + id);
                     }*/


                    //thisAgent.objectMult.CallRpcAddStatusEffect(id, a.objectMult.IsFromClient(), true, a.objectMult.IsFromClient(), false, 5);
                    //Debug.Log("RpcAddStatusEffect: " + id);

                    //a.statusEffects.AddStatusEffect(id);

                    //a.statusEffects.ChangeStatusEffectTime(id, 10);

                    //a.objectMult.AddStatusEffect(id,true,thisAgent,a.objectMult.IsFromClient(),false,-1);
                    //a.objectMult.CallCmdAddStatusEffect(id, a.objectMult.IsFromClient(),true, false, -1);

                }
            }
            activated = !activated;


        }

        public static void setStatus(string id)
        {

            /* List<Agent> agents = thisAgent.objectMultAgent.currentPlayerAgents;
             thisAgent.Say(agents.Count.ToString());
             foreach (Agent a in agents)
             {
                 thisAgent.Say(a.agentName);
             }*/
            thisAgent.Say(id.ToUpper() + "!");
            thisAgent.statusEffects.AddStatusEffect(id);
            // AddStatusEffect("Giant",true,thisAgent,thisAgent.objectNetID,true, 3);
            //thisAgent.objectMultAgent.RemoveStatusEffect("Giant", true, thisAgent.objectNetID);
            //thisAgent.Say("GIANT");
        }
        public static void recieveItem(String id, int count)
        {

            thisAgent.inventory.AddItemOrDrop(id, count);
            thisAgent.Say(id);
        }

        public static void removeHP(int count)
        {
            thisAgent.statusEffects.ChangeHealth(-count);
            thisAgent.Say("Lose -" + count + "HP");
        }

        public static void addHP(int count)
        {
            thisAgent.statusEffects.ChangeHealth(count);
            thisAgent.Say("Heal +" + count + "HP");
        }

        public static void randomGreatEffect()
        {
            System.Random rnd = new System.Random();
            int EffectMAX = 7;
            int effect = rnd.Next(0, EffectMAX + 1);


            switch (effect)
            {
                case 0:
                    setStatus("Invincible");
                    break;
                case 1:
                    recieveItem("Revolver", 60);
                    break;
                case 2:
                    recieveItem("RocketLauncher", 3);
                    break;
                case 3:
                    setStatus("AlwaysCrit");
                    break;
                case 4:
                    recieveItem("HardHat", 1);
                    break;
                case 5:
                    recieveItem("Hypnotizer2", 1);
                    break;
                case 6:
                    recieveItem("Resurrection", 1);
                    break;
                case 7:
                    setStatus("Giant");
                    break;




            }
        }

        public static void randomNormalEffect()
        {
            System.Random rnd = new System.Random();
            int EffectMAX = 8;
            int effect = rnd.Next(0, EffectMAX + 1);


            switch (effect)
            {
                case 0:
                    addHP(30);
                    break;
                case 1:
                    addHP(60);
                    break;
                case 2:
                    recieveItem("Lockpick", 1);
                    break;
                case 3:
                    recieveItem("HotFud", 3);
                    break;
                case 4:
                    recieveItem("WallBypasser", 1);
                    break;
                case 5:
                    setStatus("Strength");
                    break;
                case 6:
                    setStatus("IncreaseAllStats");
                    break;
                case 7:
                    recieveItem("Sword", 100);
                    break;
                case 8:
                    recieveItem("QuickEscapeTeleporter", 1);
                    break;
            }
        }

        public static void randomBadEffect()
        {
            System.Random rnd = new System.Random();
            int EffectMAX = 8;
            int effect = rnd.Next(0, EffectMAX + 1);


            switch (effect)
            {
                case 0:
                    removeHP(20);
                    break;
                case 1:
                    removeHP(30);
                    break;
                case 2:
                    removeHP(30);
                    break;
                case 3:
                    if (rnd.Next(0, 100) < 90)
                    {
                        removeHP(9999);
                    }
                    else
                    {
                        removeHP(9);
                    }

                    break;
                case 4:
                    setStatus("Frozen");
                    break;
                case 5:
                    setStatus("Confused");
                    break;
                case 6:
                    removeHP(40);
                    break;
                case 7:
                    recieveItem("Cigarettes", 1);
                    break;
                case 8:
                    recieveItem("Cigarettes", 1);
                    break;


            }
        }

        public static IEnumerator rollTheDice()
        {
            InvDatabase invDatabase = thisAgent.inventory;
            InvItem money = invDatabase.FindItem("Money");
            if (!(abilityPrice == 0 || (money != null && money.invItemCount >= abilityPrice)))
            {
                thisAgent.Say("You need " + abilityPrice.ToString() + " coins");
                yield return new WaitForSeconds(1f);

            }
            else
            {


                thisAgent.inventory.SubtractFromItemCount(money, abilityPrice);

                System.Random rnd = new System.Random();

                int lastNumber = 1;
                thisAgent.Say("Throw the dice");
                yield return new WaitForSeconds(1f);


                for (int i = 0; i < 7; i++)
                {
                    yield return new WaitForSeconds(0.3f);

                    int random = rnd.Next(1, 7);
                    thisAgent.Say("" + random.ToString());

                    if (i == 6)
                    {


                        String result = "";
                        if (random == 1 || random == 2)
                        {
                            result = " That's bad.";
                        }
                        else if (random == 3 || random == 4)
                        {
                            result = " Nice!";
                        }
                        else if (random == 5 || random == 6)
                        {
                            result = " That's great!";
                        }

                        thisAgent.Say("Rolled: " + random.ToString() + result);
                        lastNumber = random;
                        yield return new WaitForSeconds(1f);


                    }

                }
                yield return new WaitForSeconds(1);

                if (lastNumber == 1 || lastNumber == 2)
                {
                    randomBadEffect();
                }
                else if (lastNumber == 3 || lastNumber == 4 || lastNumber == 5)
                {
                    randomNormalEffect();
                }
                else if (lastNumber == 6)
                {
                    randomGreatEffect();
                }
                //buffEverybody("Giant");
                //recieveItem("Revolver", 100);

            }




        }

        public static void RandomEffect()
        {

            RedModMain.redModMain.StartCoroutine(rollTheDice());
        }

        public static void LoadSkill()
        {


            Sprite questionmarkSprite = RogueUtilities.ConvertToSprite(RedMod.Properties.Resources.questionmark);
            CustomAbility randomAbility = RogueLibs.CreateCustomAbility("randomAbility", questionmarkSprite, true,
                new CustomNameInfo("Random Ability"),
                new CustomNameInfo("???"),
                item =>
                {

                });




            randomAbility.OnPressed = (item, agent) =>
            {
                RedNetwork redNetwork = GameController.gameController.playerAgent.objectMult.gameObject.AddComponent(typeof(RedNetwork)) as RedNetwork;
                if (item.invItemCount > 0) // is recharging
                    agent.gc.audioHandler.Play(agent, "CantDo");
                else
                {
                    thisAgent = agent;
                    RandomEffect();
                    //InvItem newItem3 = GameController.gameController.playerAgent.inventory.AddItem("Giantizer", 2);
                    /*List<Agent> players = new List<Agent>();
                    foreach (Agent a in GameController.gameController.playerAgentList)
                    {
                        
                        players.Add(a);
                        Debug.Log("NetId: "+ a.objectNetID);
                        GameController.gameController.playerAgent.GetComponent<RedNetwork>().AddStatusEffectOthers("Giant", true, GameController.gameController.playerAgent, a, GameController.gameController.playerAgent.objectMult.IsFromClient(), true, -1);
                        //a.statusEffects.AddStatusEffect("Invisible", true, GameController.gameController.playerAgent, a.objectNetID, false, -1);
                        //GameController.gameController.playerAgent.statusEffects.AddStatusEffectSpecial("Invisible", a, GameController.gameController.playerAgent, false);
                        //a.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(this.GetComponent<NetworkIdentity>().connectionToClient);
                        //a.objectMultAgent.CallCmdAddStatusEffectSpecial("Invisible", GameController.gameController.playerAgent.objectNetID, true, false, -1);

                    }*/

                    //GameController.gameController.playerAgent.objectMultAgent.CallCmdAddStatusEffect("Invisible", GameController.gameController.playerAgent.objectNetID, true, false, -1);
                    //GameController.gameController.playerAgent.statusEffects.AddStatusEffect("Invisible", true, GameController.gameController.playerAgent, GameController.gameController.playerAgent.objectNetID, false, -1);
                    //GameController.gameController.hostPlayer.objectMultAgent.CallRpcAddStatusEffect("Invisible", GameController.gameController.hostPlayer.objectNetID,true, GameController.gameController.hostPlayer.objectMultAgent.IsFromClient(),  false, -1);
                    //GameController.gameController.playerAgent.objectMultAgent.AddStatusEffect("Invisible", true, GameController.gameController.playerAgent, GameController.gameController.hostPlayer.objectMultAgent.IsFromClient(), true, -1);
                    agent.inventory.buffDisplay.specialAbilitySlot.MakeNotUsable();
                    // make special ability slot half-transparent
                    item.invItemCount = 100; // 100 x 0.13f = 13 seconds to recharge
                                             // or you can replace 100 with 13, and 0.13 with 1 to make it simpler
                }
            };

            randomAbility.RechargeInterval = (item, agent)
                => item.invItemCount > 0 ? new WaitForSeconds(0.01f) : null;

            randomAbility.Recharge = (item, agent) =>
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
