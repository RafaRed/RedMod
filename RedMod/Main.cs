using System;
using BepInEx;
using RedMod;
using RogueLibsCore;
using UnityEngine;

namespace RogueLibsCore.Redmod
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    [BepInDependency(RogueLibs.pluginGuid, "1.4")]
    public class RedModMain : BaseUnityPlugin
    {
        public const string pluginGuid = "rafared.streetsofrogue.redmod";
        public const string pluginName = "RedMod";
        public const string pluginVersion = "1.1";
        public static RedModMain redModMain;

        public void Awake()
        {
            redModMain = this;
            SaitamaPunch.LoadSkill();
            RandomAbility.LoadSkill();
            
        }
    }
}