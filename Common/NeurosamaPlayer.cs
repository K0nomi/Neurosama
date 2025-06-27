using Neurosama.Content.Items.Furniture;
using Terraria.ModLoader;

namespace Neurosama.Common
{
    public class NeurosamaPlayer : ModPlayer
    {
        public bool neuroFumoEquipped;
        public bool evilFumoEquipped;
        public bool neuroFumoVanityEquipped;
        public bool evilFumoVanityEquipped;
        public bool vedalFumoVanityEquipped;
        public bool vedalFumoEquipped;

        public override void Initialize() => ResetEquips();
        public override void ResetEffects() => ResetEquips();
        public override void UpdateDead() => ResetEquips();

        public override void UpdateVisibleAccessories() // UpdateEquips()
        {
            if (Player.armor[10].type == ModContent.ItemType<NeuroFumo>())
            {
                neuroFumoVanityEquipped = true;
            }
            else if (Player.armor[10].type == ModContent.ItemType<EvilFumo>())
            {
                evilFumoVanityEquipped = true;
            }
            else if (Player.armor[10].type == ModContent.ItemType<VedalFumo>())
            {
                vedalFumoVanityEquipped = true;
            }
            // This is only here because it doesn't show in player select when done the recommended way for some reason
            if (Player.armor[0].type == ModContent.ItemType<NeuroFumo>())
            {
                neuroFumoEquipped = true;
            }
            else if (Player.armor[0].type == ModContent.ItemType<EvilFumo>())
            {
                evilFumoEquipped = true;
            }
            else if (Player.armor[0].type == ModContent.ItemType<VedalFumo>())
            {
                vedalFumoEquipped = true;
            }
        }

        private void ResetEquips()
        {
            neuroFumoEquipped = false;
            evilFumoEquipped = false;
            neuroFumoVanityEquipped = false;
            evilFumoVanityEquipped = false;
            vedalFumoVanityEquipped = false;
            vedalFumoEquipped = false;
        }
    }
}
