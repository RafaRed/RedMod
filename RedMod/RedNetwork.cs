using UnityEngine;
using UnityEngine.Networking;


namespace RogueLibsCore.Redmod
{
    class RedNetwork : ObjectMult
    {
        public static int kCmdCmdAddStatusEffectOthers = -1778623922;
        public static int kRpcRpcAddStatusEffectOthers = -1778623923;

        static RedNetwork()
        {
            NetworkBehaviour.RegisterCommandDelegate(typeof(RedNetwork), RedNetwork.kCmdCmdAddStatusEffectOthers, new NetworkBehaviour.CmdDelegate(RedNetwork.InvokeCmdCmdAddStatusEffectOthers));
            NetworkBehaviour.RegisterRpcDelegate(typeof(RedNetwork), RedNetwork.kRpcRpcAddStatusEffectOthers, new NetworkBehaviour.CmdDelegate(RedNetwork.InvokeRpcRpcAddStatusEffectOthers));
            NetworkCRC.RegisterBehaviour("RedNetwork", 0);

            //NetworkServer.RegisterHandler(0, RpcAddStatusEffectOthers);
            Debug.Log("RegisterCommandDelegate and RegisterRpcDelegate");
            RedModMain.loadPrefix();
        }


        protected static void InvokeRpcRpcAddStatusEffectOthers(NetworkBehaviour obj, NetworkReader reader)
        {
            Debug.LogError("InvokeRpcAddStatusEffectOthers");
            if (!NetworkClient.active)
            {
                Debug.LogError("RPC RpcAddStatusEffectOthers called on server.");
                return;
            }
        ((RedNetwork)obj).RpcAddStatusEffectOthers(reader.ReadString(), reader.ReadNetworkId(), reader.ReadNetworkId(), reader.ReadBoolean(), reader.ReadNetworkId(), reader.ReadBoolean(), (int)reader.ReadPackedUInt32());
        }

        protected static void InvokeCmdCmdAddStatusEffectOthers(NetworkBehaviour obj, NetworkReader reader)
        {
            Debug.LogError("InvokeCmdAddStatusEffectOthers");
            if (!NetworkServer.active)
            {
                Debug.LogError("Command CmdAddStatusEffectOthers called on client.");
                return;
            }
        ((RedNetwork)obj).CmdAddStatusEffectOthers(reader.ReadString(), reader.ReadBoolean(), reader.ReadNetworkId(), reader.ReadNetworkId(), reader.ReadNetworkId(), reader.ReadBoolean(), (int)reader.ReadPackedUInt32());
        }



        public void AddStatusEffectOthers(string statusEffectName, bool showText, Agent other, Agent causingAgent, NetworkInstanceId cameFromClient, bool dontPrevent, int specificTime)
        {
            Debug.Log("AddStatusEffectOthers");

            for (int i = 0; i < 30; i++)
            {
                Debug.Log("Handler: " + i + " " + base.connectionToClient.CheckHandler((short)i));
            }
            for (int i = 0; i < 30; i++)
            {
                Debug.Log("ObjectHandler: " + i + " " + GameController.gameController.playerAgent.objectMult.connectionToClient.CheckHandler((short)i));
            }

            if (GameController.gameController.playerAgent.gc.multiplayerMode)
            {
                NetworkInstanceId causingAgentID = NetworkInstanceId.Invalid;
                if (causingAgent != null)
                {
                    causingAgentID = causingAgent.objectNetID;
                }

                NetworkInstanceId otherAgentID = NetworkInstanceId.Invalid;
                if (other != null)
                {
                    otherAgentID = other.objectNetID;
                }

                if (!GameController.gameController.playerAgent.objectMult.gc.serverPlayer && GameController.gameController.playerAgent.objectMultAgent.isLocalPlayer && cameFromClient != NetworkInstanceId.Invalid)
                {
                    CallCmdAddStatusEffectOthers(statusEffectName, showText, otherAgentID, causingAgentID, cameFromClient, dontPrevent, specificTime);
                    Debug.Log("CmdAddStatusEffect: " + statusEffectName);
                }
                else if (GameController.gameController.playerAgent.objectMult.gc.serverPlayer)
                {
                    CallRpcAddStatusEffectOthers(statusEffectName, otherAgentID, causingAgentID, showText, cameFromClient, dontPrevent, specificTime);
                    Debug.Log("RpcAddStatusEffect: " + statusEffectName);
                }
            }
        }

        public void CallCmdAddStatusEffectOthers(string statusEffectName, bool showText, NetworkInstanceId otherAgentID, NetworkInstanceId causingAgentID, NetworkInstanceId cameFromClient, bool dontPrevent, int specificTime)
        {
            Debug.Log("CallCmdAddStatusEffectOthers on " + this.gameObject.name);



            if (!NetworkClient.active)
            {
                Debug.LogError("Command function CmdAddStatusEffect called on server.");
                return;
            }
            if (GameController.gameController.playerAgent.objectMult.isServer)
            {
                CmdAddStatusEffectOthers(statusEffectName, showText, otherAgentID, causingAgentID, cameFromClient, dontPrevent, specificTime);
                return;
            }




            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write((short)((ushort)5));
            networkWriter.WritePackedUInt32((uint)RedNetwork.kCmdCmdAddStatusEffectOthers);
            networkWriter.Write(GameController.gameController.playerAgent.objectMult.GetComponent<NetworkIdentity>().netId);
            networkWriter.Write(statusEffectName);
            networkWriter.Write(otherAgentID);
            networkWriter.Write(causingAgentID);
            networkWriter.Write(showText);
            networkWriter.Write(dontPrevent);
            networkWriter.WritePackedUInt32((uint)specificTime);

            base.SendCommandInternal(networkWriter, 0, "CmdAddStatusEffectOthers");


        }

        [Command]
        public void CmdAddStatusEffectOthers(string statusEffectName, bool showText, NetworkInstanceId other, NetworkInstanceId causingAgentID, NetworkInstanceId cameFromClient, bool dontPrevent, int specificTime)
        {
            Debug.Log("CmdAddStatusEffectOthers");
            Agent agent = null;
            Agent agentOther = null;
            if (causingAgentID != NetworkInstanceId.Invalid)
            {
                GameObject causingAgentGameObject = NetworkServer.FindLocalObject(causingAgentID);
                GameObject otherAgentGameObject = NetworkServer.FindLocalObject(other);
                if (agentOther != null)
                {
                    agent = causingAgentGameObject.GetComponent<Agent>();
                    agentOther = otherAgentGameObject.GetComponent<Agent>();

                    agentOther.SetJustHitByAgent(agent);
                    agentOther.justHitByAgent2 = agent;
                    agentOther.lastHitByAgent = agent;
                }
            }
            agentOther.statusEffects.AddStatusEffect(statusEffectName, showText, agent, agent.objectNetID, dontPrevent, specificTime);
        }

        public void CallRpcAddStatusEffectOthers(string statusEffectName, NetworkInstanceId other, NetworkInstanceId causingAgentID, bool showText, NetworkInstanceId cameFromClient, bool dontPrevent, int specificTime)
        {
            Debug.Log("CallRpcAddStatusEffectOthers");
            if (!NetworkServer.active)
            {
                Debug.LogError("RPC Function RpcAddStatusEffectOthers called on client.");
                return;
            }
            Debug.Log("WRITE: " + GameController.gameController.playerAgent.objectMult.GetComponent<NetworkIdentity>().netId);
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write((short)((ushort)2));
            networkWriter.WritePackedUInt32((uint)RedNetwork.kRpcRpcAddStatusEffectOthers);
            networkWriter.Write(GameController.gameController.playerAgent.objectMult.GetComponent<NetworkIdentity>().netId);
            networkWriter.Write(statusEffectName);
            networkWriter.Write(other);
            networkWriter.Write(causingAgentID);
            networkWriter.Write(showText);
            networkWriter.Write(cameFromClient);
            networkWriter.Write(dontPrevent);
            networkWriter.WritePackedUInt32((uint)specificTime);
            base.SendRPCInternal(networkWriter, 0, "RpcAddStatusEffectOthers");
        }


        [ClientRpc]
        public void RpcAddStatusEffectOthers(string statusEffectName, NetworkInstanceId other, NetworkInstanceId causingAgentID, bool showText, NetworkInstanceId cameFromClient, bool dontPrevent, int specificTime)
        {
            Debug.Log("RpcAddStatusEffect");
            Agent agent = null;
            Agent agentOther = null;
            if (cameFromClient != NetworkInstanceId.Invalid)
            {
                GameObject gameObject = ClientScene.FindLocalObject(other);

                if (gameObject != null)
                {
                    agent = gameObject.GetComponent<Agent>();
                }

            }
            bool flag = false;
            if (agent != null && agent.localPlayer)
            {
                flag = true;
            }
            if (!this.gc.serverPlayer && !flag)
            {
                Agent causingAgent = null;
                if (causingAgentID != NetworkInstanceId.Invalid)
                {
                    GameObject gameObject2 = ClientScene.FindLocalObject(causingAgentID);
                    if (gameObject2 != null)
                    {
                        causingAgent = gameObject2.GetComponent<Agent>();
                    }
                }
                agent.statusEffects.AddStatusEffect(statusEffectName, false, causingAgent, NetworkInstanceId.Invalid, dontPrevent, specificTime);
            }
        }
    }
}
