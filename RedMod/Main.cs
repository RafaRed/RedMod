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
        public const string pluginVersion = "1.2";
        public static RedModMain redModMain;
        public static Agent player;
        public static RoguePatcher patcher;
        public static bool GameStarted;

        protected static void HandleReader_patch(NetworkConnection __instance, NetworkReader reader, int receivedSize, int channelId)
        {

            try
            {
                ushort num = reader.ReadUInt16();
                short num2 = reader.ReadInt16();
                byte[] array = reader.ReadBytes((int)num);
                Debug.Log("RECIEVE-------------------");
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < (int)num; i++)
                {
                    stringBuilder.AppendFormat("{0:X2}", array[i]);
                    if (i > 150)
                    {
                        break;
                    }
                }

                Debug.Log("el: " + num.ToString() + " " + num2.ToString() + " " + stringBuilder);

                Debug.Log("channel: " + channelId.ToString());
                Debug.Log("connectionId: " + __instance.connectionId.ToString());
            }
            catch
            {

            }
        }
        public static void SendRPCInternal_patch(NetworkBehaviour __instance, NetworkWriter writer, int channelId, string rpcName)
        {
            try
            {
                Debug.Log("RPC -> : " + rpcName);
                String array = "";
                foreach (var e in writer.AsArray())
                {
                    array += e + ",";
                }
                Debug.Log("el: " + array.ToString());

                Debug.Log("channel: " + channelId.ToString());
                Debug.Log("instance: " + __instance.name.ToString());
            }
            catch
            {

            }
        }


        public static void AddStatusEffect_patch(ObjectMult __instance, string statusEffectName, bool showText, Agent causingAgent, NetworkInstanceId cameFromClient, bool dontPrevent, int specificTime)
        {
            Debug.Log("ADDSTATUS: " + __instance.gameObject.name + statusEffectName + showText + dontPrevent + specificTime);
            if (cameFromClient != null)
            {
                Debug.Log(cameFromClient.ToString());
            }

        }
        public static void CallCmdAddStatusEffect_patch(ObjectMult __instance, string statusEffectName, NetworkInstanceId causingAgentID, bool showText, bool dontPrevent, int specificTime)
        {
            Debug.Log("CALLCMD: " + __instance.gameObject.name);
            Debug.Log("CALLCMD: " + __instance.gameObject.name + statusEffectName + showText + dontPrevent + specificTime);

            if (causingAgentID != null)
            {
                Debug.Log(causingAgentID.ToString());
            }
            if (statusEffectName == "Giant")
            {
                // GameController.gameController.playerAgent.objectMultAgent.CallCmdAddStatusEffect("Invisible", causingAgentID, showText, dontPrevent, 9999);
            }

        }

        public static void RpcSetString_patch(byte stringType, string setting)
        {
            Debug.Log("RpcSetString_patch: " + setting);
            if(stringType == 30)
            {
                NetworkTest.Process(setting);
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

        public static void loadPrefix()
        {
            // redModMain.PatchPrefix(typeof(NetworkBehaviour), "SendRPCInternal", redModMain.GetType(), "SendRPCInternal_patch");
            // redModMain.PatchPrefix(typeof(NetworkConnection), "HandleReader", redModMain.GetType(), "HandleReader_patch");
        }

        public void Awake()
        {
            patcher = new RoguePatcher(this, GetType());
            //public void RpcSetString(byte stringType, string setting)
            this.PatchPrefix(typeof(ObjectMult), "RpcSetString", GetType(), "RpcSetString_patch");
            this.PatchPrefix(typeof(ObjectMult), "CmdSetString", GetType(), "CmdSetString_patch");
            //NetworkExtension.Load();
            //this.PatchPrefix(typeof(ObjectMult), "CallCmdAddStatusEffect", GetType(), "CallCmdAddStatusEffect_patch");
            //this.PatchPrefix(typeof(ObjectMult), "AddStatusEffect", GetType(), "AddStatusEffect_patch");

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
                if(player.gameObject.GetComponent<NetworkTest>() == null)
                {
                    player.gameObject.AddComponent<NetworkTest>();
                    Debug.Log("*********** ADDED NETWORK TEST ***********");
                }
                else
                {
                    Debug.Log("*********** ALREADY USING NETWORK TEST ***********");
                }
                //Debug.Log("*********** LOAD ***********");

            }
            else if ((!GameController.gameController.gameEventsStarted && GameStarted))
            {
                GameStarted = false;
                Reset();
                //Debug.Log("*********** UNLOAD ***********");
            }

            if (GameStarted)
            {
                updateClasses();
            }
            if (Input.GetKeyDown("space"))
            {
                player.gameObject.GetComponent<NetworkTest>().Buy();
                Debug.Log("*********** CALL TEST ***********");
            }
            //Buff.update();
        }
    }
}