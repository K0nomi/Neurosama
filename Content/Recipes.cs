using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content
{
    internal class Recipes : ModSystem
    {
        public override void AddRecipes()
        {
            // Gold ermfish to golden delight
            Recipe.Create(ItemID.GoldenDelight)
                .AddIngredient<Items.Consumables.ErmFishGold>()
                .AddTile(TileID.CookingPots)
                .Register();
        }
    }
}
