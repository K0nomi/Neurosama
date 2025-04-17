using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Consumables
{
    public class ErmFishGold : ModItem
    {
        public override void SetDefaults()
        {
            // Steal defaults from goldfish
            Item.CloneDefaults(ItemID.GoldGoldfish);

            Item.width = 30;
            Item.height = 24;

            Item.makeNPC = ModContent.NPCType<NPCs.ErmFishGold>();
        }
    }
}
