using RogueLibsCore;
using RogueLibsCore.Redmod;

namespace RedMod
{
    class BlankTrait
    {


        public static void loadTrait()
        {

            CustomTrait trait = RogueLibs.CreateCustomTrait("CoolTrait", true,
           new CustomNameInfo("Cool Trait"),
           new CustomNameInfo("This cool trait does a lot of cool stuff"));

            trait.IsActive = true;
            trait.Available = true;
            trait.AvailableInCharacterCreation = true;
            trait.CostInCharacterCreation = 0;
            trait.CanRemove = true;
            trait.CanSwap = true;
            trait.Upgrade = null;


        }
        public static void update()
        {
            if (RedModMain.player.statusEffects.hasTrait("CoolTrait"))
            {

            }
        }


    }
}
