using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items
{
    public class Donowall : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 400;
        }

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableWall(ModContent.WallType<Walls.Donowall>());

            Item.value = 5;
        }

        public override void AddRecipes()
        {
            // Tile -> 4x Wall
            CreateRecipe(4)
                .AddIngredient<Donobrick>()
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
}
