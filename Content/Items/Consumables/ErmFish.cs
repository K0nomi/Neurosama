using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Consumables
{
    public class ErmFish : ModItem
    {
        public override void SetDefaults()
        {
            // Steal defaults from goldfish
            Item.CloneDefaults(ItemID.Goldfish);

            Item.width = 30;
            Item.height = 24;

            Item.makeNPC = ModContent.NPCType<NPCs.ErmFish>();
            Item.rare = ItemRarityID.Blue;
        }
    }
}
