using System.Collections.Generic;
using UnityEngine;

namespace RedMod
{



    class Utils
    {
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
