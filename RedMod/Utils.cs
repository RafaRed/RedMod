using System.Collections.Generic;
using UnityEngine;

namespace RedMod
{



    class Utils
    {

        public static int getId(Agent agent)
        {
            int id = 0;
            if (GameController.gameController.playerAgent.localPlayer)
            {
                id = agent.agentID;
            }
            else
            {
                id = (int)agent.objectMult.netId.Value;
            }
            return id;
        }
        public static int getMyNetid()
        {
            int id = 0;
            if (GameController.gameController.playerAgent.localPlayer)
            {
                id = GameController.gameController.playerAgent.agentID;
            }
            else
            {
                id = (int)GameController.gameController.playerAgent.objectMult.netId.Value;
            }

            return id;
        }
        public static List<Agent> getAgentList(List<string> agents)
        {
            List<Agent> agentList = new List<Agent>();
            foreach (Agent a in GameController.gameController.agentList)
            {
                foreach (string netid in agents)
                {
                    if (Utils.getId(a) == int.Parse(netid))
                    {
                        agentList.Add(a);
                        break;
                    }
                }
            }
            return agentList;
        }
        public static Agent getPlayer(int netid)
        {
            Agent player = null;
            foreach (Agent a in GameController.gameController.playerAgentList)
            {

                if (Utils.getId(a) == netid)
                {
                    player = a;
                    break;
                }
            }
            return player;
        }
        public static List<Agent> GetAgentsInArea()
        {
            Agent playerAgent = GameController.gameController.playerAgent;
            List<Agent> list = new List<Agent>();
            for (int i = 0; i < GameController.gameController.agentList.Count; i++)
            {
                Agent agent = GameController.gameController.agentList[i];
                if (Vector2.Distance(agent.tr.position, playerAgent.tr.position) < agent.LOSRange / playerAgent.hardToSeeFromDistance && agent != playerAgent)
                {
                    Relationship relationship = agent.relationships.GetRelationship(playerAgent);
                    if (agent.movement.HasLOSAgent360(playerAgent) && agent.movement.HasLOSAgent360(playerAgent))
                    {
                        list.Add(agent);
                    }
                }

            }
            return list;
        }
    }
}
