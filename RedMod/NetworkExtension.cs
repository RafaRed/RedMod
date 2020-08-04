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
    public static class NetworkExtension
    {


        [Command]
        public static void CmdShow()
        {
            GameObject bullet = GameObject.Instantiate(RedModMain.player.gameObject);
            Debug.Log("INVOKE2!!!!");
            NetworkServer.Spawn(bullet);
        }
        public static void test(this ObjectMult objectMult)
        {
            GameObject bullet = GameObject.Instantiate(RedModMain.player.gameObject);
            Debug.Log("INVOKE!!!!");
            NetworkServer.Spawn(bullet);
            CmdShow();
        }
        

            //RoguePatcher patcher = new RoguePatcher(RedModMain.redModMain, RedModMain.redModMain.GetType());

    }

    
}
