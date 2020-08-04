using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RedMod
{
    class FollowObject : MonoBehaviour
    {
        public GameObject follow = null;
        void Update()
        {
            if(follow != null)
            {
                transform.position = new Vector3(follow.transform.position.x-0.25f, follow.transform.position.y + 0.5f, follow.transform.position.z);
            }
        }

    }
}
