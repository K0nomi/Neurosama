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

        public override void Initialize() => ResetEquips();
        public override void ResetEffects() => ResetEquips();
        public override void UpdateDead() => ResetEquips();

        public override void UpdateEquips()
        {
            if (Player.armor[10].type == ModContent.ItemType<NeuroFumo>())
            {
                neuroFumoVanityEquipped = true;
            }
            else if (Player.armor[10].type == ModContent.ItemType<EvilFumo>())
            {
                evilFumoVanityEquipped = true;
            }
        }

        private void ResetEquips()
        {
            neuroFumoEquipped = false;
            evilFumoEquipped = false;
            neuroFumoVanityEquipped = false;
            evilFumoVanityEquipped = false;
        }
    }
}
