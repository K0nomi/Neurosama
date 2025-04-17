using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Furniture
{
    public class NeuroCatErm : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.NeuroCatErm>());

            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Expert;
            Item.value = Item.buyPrice(0, 0, 50);
        }
    }
}
