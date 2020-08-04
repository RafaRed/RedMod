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

		public static void Test(string[] redData)
		{
			int agentId = int.Parse(redData[1]);
			if (GameController.gameController.playerAgent.objectMult.netId.Value == agentId)
			{
				GameController.gameController.playerAgent.statusEffects.AddStatusEffect("Giant");
			}


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
