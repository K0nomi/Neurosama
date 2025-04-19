using Neurosama.Content.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Furniture.Banners
{
    public class ErmFishBanner : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Banner>(), (int)Banner.StyleID.ErmFish);

            Item.width = 8;
            Item.height = 24;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(silver: 2);
        }
    }
}
