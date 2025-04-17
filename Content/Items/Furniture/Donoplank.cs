using Terraria;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Furniture
{
    public class Donoplank : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 200;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.Donoplank>());
            Item.width = 8;
            Item.height = 10;
            Item.value = 10;
        }

        public override void AddRecipes()
        {
            // Tile -> 2x Platform
            CreateRecipe(2)
                .AddIngredient<Donobrick>()
                .Register();
        }
    }
}