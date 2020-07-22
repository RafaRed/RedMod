using System;
using BepInEx;
using RogueLibsCore;
using UnityEngine;

using System.Collections;
using RogueLibsCore.Redmod;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace RedMod
{
    class RandomAbility : MonoBehaviour
    {

        public static Agent thisAgent;
        public static int abilityPrice = 3;

        public static void setStatus(string id)
        {

            /* List<Agent> agents = thisAgent.objectMultAgent.currentPlayerAgents;
             thisAgent.Say(agents.Count.ToString());
             foreach (Agent a in agents)
             {
                 thisAgent.Say(a.agentName);
             }*/
            thisAgent.Say(id.ToUpper()+"!");
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
            thisAgent.Say("Lose -"+count+"HP");
        }

        public static void addHP(int count) {
            thisAgent.statusEffects.ChangeHealth(count);
            thisAgent.Say("Heal +"+ count + "HP");
        }

        public static void randomGreatEffect()
        {
            System.Random rnd = new System.Random();
            int EffectMAX = 7;
            int effect = rnd.Next(0, EffectMAX+1);


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
            int effect = rnd.Next(0, EffectMAX+1);


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
                    recieveItem("Sword",100);
                    break;
                case 8:
                    recieveItem("QuickEscapeTeleporter", 1);
                    break;
            }
        }

        public static void randomBadEffect()
        {
            System.Random rnd = new System.Random();
            int EffectMAX = 5;
            int effect = rnd.Next(0, EffectMAX+1);


            switch (effect)
            {
                case 0:
                    removeHP(20);
                    break;
                case 1:
                    removeHP(40);
                    break;
                case 2:
                    recieveItem("Cigarettes", 1);
                    break;
                case 3:
                    removeHP(9999);
                    break;
                case 4:
                    setStatus("Frozen");
                    break;
                case 5:
                    setStatus("Confused");
                    break;


            }
        }

        public static IEnumerator rollTheDice()
        {
            InvDatabase invDatabase = thisAgent.inventory;
            InvItem money = invDatabase.FindItem("Money");
            if (!(abilityPrice == 0 || (money != null && money.invItemCount >= abilityPrice)))
            {
                thisAgent.Say("You need "+ abilityPrice.ToString() + " coins");
                yield return new WaitForSeconds(1f);

            }
            else
            {
               
                
                thisAgent.inventory.SubtractFromItemCount(money, abilityPrice);

                System.Random rnd = new System.Random();

                int lastNumber = 1;
                thisAgent.Say("Throw the dice");
                yield return new WaitForSeconds(1f);


                for (int i = 0; i < 15; i++)
                {
                    yield return new WaitForSeconds(0.2f);

                    int random = rnd.Next(1, 7);
                    thisAgent.Say("" + random.ToString());

                    if (i == 14)
                    {

                       
                        String result = "";
                        if (random == 1 || random == 2 || random == 3)
                        {
                            result = " That's bad.";
                        }
                        else if (random == 4 || random == 5)
                        {
                            result = " Nice!";
                        }
                        else if (random == 6)
                        {
                            result = " That's great!";
                        }

                        thisAgent.Say("Rolled: " + random.ToString() + result);
                        lastNumber = random;
                        yield return new WaitForSeconds(1f);


                    }
                    
                }
                yield return new WaitForSeconds(1);

                 if (lastNumber == 1 || lastNumber == 2 || lastNumber == 3)
                 {
                     randomBadEffect();
                 }
                 else if (lastNumber == 4 || lastNumber == 5)
                 {
                     randomNormalEffect();
                 }
                 else if (lastNumber == 6)
                 {
                     randomGreatEffect();
                 }
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
            CustomItem randomItem = RogueLibs.SetItem("randomAbility", questionmarkSprite,
                new CustomNameInfo("Random Ability"),
                new CustomNameInfo("???"),
                item =>
                {

                });



            CustomAbility randomAbility = RogueLibs.SetAbility(randomItem);

            randomAbility.OnPressed = (item, agent) =>
            {
                if (item.invItemCount > 0) // is recharging
                    agent.gc.audioHandler.Play(agent, "CantDo");
                else
                {
                    thisAgent = agent;
                    RandomEffect();

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
