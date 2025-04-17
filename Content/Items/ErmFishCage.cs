using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items
{
    public class ErmFishCage : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(TileID.BunnyCage);
            Item.createTile = ModContent.TileType<Tiles.ErmFishCage>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Terrarium)
                .AddIngredient(ModContent.ItemType<Consumables.ErmFish>())
                .Register();
        }
    }
}