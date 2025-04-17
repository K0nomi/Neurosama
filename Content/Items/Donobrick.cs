using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items
{
    public class Donobrick : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 100;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Donobrick>());
            Item.width = 12;
            Item.height = 12;
            Item.value = 20;
        }

        public override void AddRecipes()
        {
            // 4x Wall -> Tile
            CreateRecipe()
                .AddIngredient<Donowall>(4)
                .AddTile(TileID.WorkBenches)
                .Register();

            // 2x Platform -> Tile
            CreateRecipe()
                .AddIngredient<Furniture.Donoplank>(2)
                .Register();
        }
    }
}
