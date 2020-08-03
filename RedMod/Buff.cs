using System.Collections.Generic;
using RogueLibsCore;
using UnityEngine;

namespace RedMod
{
    class Buff : MonoBehaviour
    {


        public static Agent thisAgent;
        public static Dictionary<InvItem, Agent> SoulUpdateList { get; set; }
        public static Dictionary<InvItem, float> SoulCooldowns { get; set; }

        public static InvItem SoulBreakerItem;
        public static bool recoverItem = false;



        public static void update()
        {
            List<InvItem> removal = new List<InvItem>();
            Dictionary<InvItem, float> newDictionary = new Dictionary<InvItem, float>();

            foreach (KeyValuePair<InvItem, float> pair in SoulCooldowns)
                newDictionary.Add(pair.Key, Mathf.Max(pair.Value - Time.fixedDeltaTime, 0f));
            SoulCooldowns = newDictionary;

            /*if(thisAgent != null && thisAgent.dead && recoverItem)
            {
				recoverItem = false;
				thisAgent.inventory.AddItem("EmptySoulStone", 1);
			}*/
            foreach (KeyValuePair<InvItem, Agent> pair in SoulUpdateList)
            {
                InvItem item = pair.Key;
                if (pair.Key == null || pair.Value == null)
                {
                    removal.Add(item);
                    continue;
                }
                if (pair.Value.dead)
                {
                    item.database.DestroyItem(item);
                    item.database.AddItem("EmptySoulStone", 1);
                    removal.Add(item);
                    if (SoulBreakerItem != null)
                    {
                        SoulBreakerItem.database.DestroyItem(SoulBreakerItem);
                        SoulBreakerItem = null;
                    }

                    item.agent.mainGUI.invInterface.HideDraggedItem();
                    item.agent.mainGUI.invInterface.HideTarget();
                }
            }
            foreach (InvItem item in removal)
            {
                SoulUpdateList.Remove(item);
                SoulCooldowns.Remove(item);
                if (SoulBreakerItem != null)
                {
                    SoulBreakerItem.database.DestroyItem(SoulBreakerItem);
                    SoulBreakerItem = null;
                }
            }
        }





        public static void multplayerMsg()
        {
            Agent player = GameController.gameController.playerAgent;
            Agent server = null;
            foreach (Agent a in GameController.gameController.agentList)
            {

                if (a.isPlayer != 0 && a.objectMult.isServer)
                {
                    server = a;
                }
            }

            Debug.Log("Agent: " + player.name);
            Debug.Log("Server: " + server.name);
            Debug.Log("Server Mult: " + server.objectMult);


        }

        public static void LoadSkill()
        {

            multplayerMsg();





            recoverItem = true;
            SoulCooldowns = new Dictionary<InvItem, float>();
            SoulUpdateList = new Dictionary<InvItem, Agent>();


            Sprite sprite3 = RogueUtilities.ConvertToSprite(Properties.Resources.BlankSoulStone);
            CustomItem emptySoulStone = RogueLibs.CreateCustomItem("EmptySoulStone", sprite3, true,
                new CustomNameInfo("Empty Soul Stone"),
                new CustomNameInfo("It can provide power to someone soul's."),
                item =>
                {
                    item.itemType = "Tool";
                    item.Categories.Add("Usable");
                    item.Categories.Add("Stealth");
                    item.Categories.Add("Weird");
                    item.itemValue = 0;
                    item.initCount = 1;
                    item.rewardCount = 1;
                    item.stackable = true;
                    item.hasCharges = false;
                    item.goesInToolbar = true;
                });
            emptySoulStone.TargetFilter = (item, agent, obj) => obj is Agent a && !a.dead;
            emptySoulStone.TargetObject = (item, agent, obj) =>
            {
                thisAgent = agent;
                item.invInterface.HideTarget();

                item.database.DestroyItem(item);
                InvItem newItem = item.database.AddItem("SoulStone", item.invItemCount);
                InvItem newItem2 = item.database.AddItem("SoulBreaker", item.invItemCount);
                InvItem newItem3 = item.database.AddItem("Giantizer", 10);
                InvItem newItem4 = item.database.AddItem("Banana", 10);
                SoulBreakerItem = newItem2;

                SoulUpdateList.Add(newItem, (Agent)obj);
                SoulCooldowns.Add(newItem, 0f);
            };
            emptySoulStone.SetTargetText(new CustomNameInfo("Bind"));




            Sprite sprite4 = RogueUtilities.ConvertToSprite(Properties.Resources.soulstone);
            CustomItem soulStone = RogueLibs.CreateCustomItem("SoulStone", sprite4, true,
                new CustomNameInfo("Soul Stone"),
                new CustomNameInfo("Combine with any consumable to inflict effects on the Soul."),
                item =>
                {
                    item.itemType = "Combine";
                    item.Categories.Add("Social");
                    item.Categories.Add("Stealth");
                    item.Categories.Add("Weird");
                    item.itemValue = 0;
                    item.initCount = 1;
                    item.rewardCount = 1;
                    item.stackable = true;
                    item.hasCharges = true;
                });
            soulStone.CombineFilter = (item, agent, otherItem) =>
            {

                if (otherItem.itemType == "Consumable") return true;
                if (otherItem.itemType == "Health") return true;
                if (otherItem.itemType == "Food") return true;
                if (item == otherItem) return true;

                return false;
            };
            /*soulStone.CombineItems = (item, agent, otherItem, slotNum) =>
			{

				foreach (var a in SoulCooldowns)
				{
					if (a.Key.invItemID == item.invItemID && a.Value > 0)
					{
						return;

					}
				}
				Agent target = null;
				foreach (var a in SoulUpdateList)
				{
					if (a.Key.invItemID == item.invItemID)
					{
						target = a.Value;

					}
				}

				
				if (otherItem.itemType == "Consumable")
					new ItemFunctions().UseItem(otherItem, target);
				else if(otherItem.itemType == "Health")
					new ItemFunctions().UseItem(otherItem, target);
				else if (otherItem.itemType == "Food")
					new ItemFunctions().UseItem(otherItem, target);


			};


			Sprite sprite5 = RogueUtilities.ConvertToSprite(Properties.Resources.SoulBreaker);
			CustomItem soulBreaker = RogueLibs.SetItem("SoulBreaker", sprite5,
				new CustomNameInfo("Soul Breaker"),
				new CustomNameInfo("Use me and release the soul!"),
				item =>
				{
					item.itemType = "Consumable";
					item.Categories.Add("Social");
					item.Categories.Add("Stealth");
					item.Categories.Add("Weird");
					item.itemValue = 0;
					item.initCount = 1;
					item.rewardCount = 0;
					item.stackable = true;
					item.hasCharges = true;
				});

			soulBreaker.UseItem = (item, agent) =>
			{
				List<InvItem> removal = new List<InvItem>();
				foreach (InvItem i in SoulUpdateList.Keys)
				{

					removal.Add(i);


				}
				
				foreach (InvItem i in removal)
				{
					SoulUpdateList.Remove(i);
					SoulCooldowns.Remove(i);
					i.agent.mainGUI.invInterface.HideDraggedItem();
					i.agent.mainGUI.invInterface.HideTarget();
					i.database.DestroyItem(i);
				}
				item.database.AddItem("EmptySoulStone", 1);

				item.database.SubtractFromItemCount(item, 1);


			};
			*/

        }


    }
}
