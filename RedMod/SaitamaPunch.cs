﻿using System;
using BepInEx;
using RogueLibsCore;
using UnityEngine;

namespace RedMod
{
    class SaitamaPunch
    {

        public static void LoadSkill()
        {
            Sprite punchSprite = RogueUtilities.ConvertToSprite(RedMod.Properties.Resources.punch);
            CustomItem punchItem = RogueLibs.SetItem("saitamaPunch", punchSprite,
                new CustomNameInfo("Saitamas punch"),
                new CustomNameInfo("Just one punch is enough"),
                item =>
                {

                });



            CustomAbility saitamaPunch = RogueLibs.SetAbility(punchItem);

            saitamaPunch.OnPressed = (item, agent) =>
            {
                if (item.invItemCount > 0) // is recharging
                    agent.gc.audioHandler.Play(agent, "CantDo");
                else
                {
                    agent.Say("HAHAHA");
                    agent.SetStrength(999);
                    agent.wallDestroyDetector.EnableIfPossible();

                    agent.inventory.buffDisplay.specialAbilitySlot.MakeNotUsable();
                    // make special ability slot half-transparent
                    item.invItemCount = 100; // 100 x 0.13f = 13 seconds to recharge
                                             // or you can replace 100 with 13, and 0.13 with 1 to make it simpler
                }
            };

            saitamaPunch.RechargeInterval = (item, agent)
                => item.invItemCount > 0 ? new WaitForSeconds(0f) : null;

            saitamaPunch.Recharge = (item, agent) =>
            {
                if (item.invItemCount > 0 && agent.statusEffects.CanRecharge())
                { // if can recharge
                    item.invItemCount--;
                    if (item.invItemCount == 0) // ability recharged
                    {
                        agent.statusEffects.CreateBuffText("Recharged", agent.objectNetID);
                        agent.gc.audioHandler.Play(agent, "Recharge");
                        agent.inventory.buffDisplay.specialAbilitySlot.MakeUsable();
                        // make special ability slot fully visible
                    }
                }
            };
        }
    }
}
