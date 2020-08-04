using System;
using System.Text;
using BepInEx;
using RedMod;
using UnityEngine;
using UnityEngine.Networking;
using RogueLibsCore;
using RogueLibsCore.Redmod;

namespace RedMod
{
	class NetworkTest : MonoBehaviour
	{
		public static void JoinedGame()
        {
			string chatPlayerColor = GameController.gameController.playerAgent.objectMult.GetChatPlayerColor(GameController.gameController.playerAgent.playerColor);
			if (GameController.gameController.playerAgent.objectMult.gc.serverPlayer)
			{
				
				GameController.gameController.playerAgent.objectMult.CallRpcSetString(30, "1|" + GameController.gameController.playerAgent.objectMult.netId.Value + "|" + GameController.gameController.playerAgent.objectMult.playerUniqueID + "|"+chatPlayerColor+"|"+"isServer");
			}
			else
			{
				GameController.gameController.playerAgent.objectMult.CallCmdSetString(30, "1|" + GameController.gameController.playerAgent.objectMult.netId.Value + "|"+GameController.gameController.playerAgent.objectMult.playerUniqueID + "|" + chatPlayerColor +"|"+ "isClient");
			}
		}
		public static void Test(string[] redData)
		{
			int agentId = int.Parse(redData[1]);
			if (GameController.gameController.playerAgent.objectMult.netId.Value == agentId)
			{
				GameController.gameController.playerAgent.statusEffects.AddStatusEffect("Giant");
			}


		}

		public static void addBanner(int playerid, bool isServer) {
			Sprite redClientBanner = RogueUtilities.ConvertToSprite(RedMod.Properties.Resources.redclient);
			Sprite redServerBanner = RogueUtilities.ConvertToSprite(RedMod.Properties.Resources.redserver);
			foreach (Agent player in GameController.gameController.playerAgentList)
            {
				if(player.objectMult.netId.Value == playerid)
                {
					
                    if (player.gameObject.transform.Find("Banner") == null)
                    {
						
						GameObject banner;
						banner = new GameObject("Banner");
						banner.transform.position = new Vector3(0, 0, 0);
						banner.transform.localPosition = new Vector3(0, 0, 0);
						banner.AddComponent<FollowObject>();
						banner.GetComponent<FollowObject>().follow = player.gameObject;

						


						banner.AddComponent<SpriteRenderer>();
                        if (isServer)
                        {
							banner.GetComponent<SpriteRenderer>().sprite = redServerBanner;
						}
                        else
                        {
							banner.GetComponent<SpriteRenderer>().sprite = redClientBanner;
						}
						break;
					}
					
				}
            }
		}

		public static void ShowUsingRedMod(string[] redData)
        {
			bool isServer = redData[4] == "isServer";
			
			string text = "";
            if (isServer)
            {
				text = string.Concat(new string[]
				{
					"<color=",
					redData[3],
					">",
					redData[2],
					"(SERVER)",
					" </color><color=green>",
					"is running RedMod!",
					"</color>"
				});
			}
            else
            {
				text = string.Concat(new string[]
				{
					"<color=",
					redData[3],
					">",
					redData[2],
					" </color><color=green>",
					"is using RedMod!",
					"</color>"
				});
			}
			
			bool scrollToBottom = true;
			if (GameController.gameController.playerAgent.gc.chatLog.scrollBar.value != 0f && GameController.gameController.playerAgent.gc.chatLog.scrollBarGo.activeSelf)
			{
				scrollToBottom = false;
			}
			GameController.gameController.playerAgent.gc.chatLog.AddChatlogText(text, scrollToBottom);
			
			addBanner(int.Parse(redData[1]), isServer);


		}

		public static void Process(string data)
		{
			string[] redData = data.Split('|');
			int function = int.Parse(redData[0]);

			switch (function)
			{
				case 0:
					Test(redData);
					break;
                case 1:
					ShowUsingRedMod(redData);
					break;
			}

		}

		public void Buy()
		{

			foreach(Agent a in GameController.gameController.playerAgentList)
            {
				if(a.objectMult.netId != GameController.gameController.playerAgent.objectMult.netId)
                {
					if (GameController.gameController.playerAgent.objectMult.gc.serverPlayer)
					{
						GameController.gameController.playerAgent.objectMult.CallRpcSetString(30, "0|" + a.objectMult.netId);
					}
					else
					{
						GameController.gameController.playerAgent.objectMult.CallCmdSetString(30, "0|" + a.objectMult.netId);
					}
				}

            }
			
		}



	}
}
