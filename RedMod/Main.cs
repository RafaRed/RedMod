using System;
using System.Text;
using BepInEx;
using RedMod;
using Rewired;
using UnityEngine;
using UnityEngine.Networking;



namespace RogueLibsCore.Redmod
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInDependency(RogueLibs.pluginGuid, "2.0")]
    public class RedModMain : BaseUnityPlugin
    {
        public const string pluginGuid = "rafared.streetsofrogue.redmod";
        public const string pluginName = "RedMod";
        public const string pluginVersion = "1.3";
        public static RedModMain redModMain;
        public static Agent player;
        public static RoguePatcher patcher;
        public static bool GameStarted;

       

        public static void RpcSetString_patch(byte stringType, string setting)
        {
            Debug.Log("RpcSetString_patch: " + setting);
            if(stringType == 30)
            {
                RedNetwork.Process(setting);
            }
            
        }
        public static void CmdSetString_patch(byte stringType, string setting)
        {
            Debug.Log("CmdSetString_patch: " + setting);
            if (stringType == 30)
            {
                //NetworkTest.Process(setting);
            }
        }

        public static void RpcSendChatAnnouncement_patch(string myMessage, string myMessage2, string myMessage3, int myMessage4)
        {
            if(myMessage == "JoinedGame")
            {
                RedNetwork.JoinedGame();
            }
        }

        public void Awake()
        {
            patcher = new RoguePatcher(this, GetType());

            this.PatchPrefix(typeof(ObjectMult), "RpcSetString", GetType(), "RpcSetString_patch");
            this.PatchPrefix(typeof(ObjectMult), "CmdSetString", GetType(), "CmdSetString_patch");
            this.PatchPrefix(typeof(ObjectMult), "RpcSendChatAnnouncement", GetType(), "RpcSendChatAnnouncement_patch");


            redModMain = this;
            SaitamaPunch.LoadSkill();
            RandomAbility.LoadSkill();
            //Buff.LoadSkill();
            Neuralyzer.LoadSkill();

            BerserkerTrait.loadTrait();
            

        }


        public static void Reset()
        {
            BerserkerTrait.Reset();
        }

        public static void updateClasses()
        {
            BerserkerTrait.update();
            
        }

        public void Update()
        {
            if (!GameStarted && (GameController.gameController.gameEventsStarted))
            {
                GameStarted = true;
                player = GameController.gameController.playerAgent;
                if(player.gameObject.GetComponent<RedNetwork>() == null)
                {
                    player.gameObject.AddComponent<RedNetwork>();
                    Debug.Log("*********** ADDED NETWORK LAYER ***********");
                }
                else
                {
                    Debug.Log("*********** ALREADY USING NETWORK LAYER ***********");
                }


            }
            else if ((!GameController.gameController.gameEventsStarted && GameStarted))
            {
                GameStarted = false;
                Reset();
            }

            if (GameStarted)
            {
                updateClasses();
            }
            /*if (Input.GetKeyDown("space"))
            {
                player.gameObject.GetComponent<RedNetwork>().Buy();
                Debug.Log("*********** CALL TEST ***********");
            }*/
            //Buff.update();
        }
    }
}