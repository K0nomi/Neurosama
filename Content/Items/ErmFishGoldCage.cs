using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items
{
    public class ErmFishGoldCage : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(TileID.GoldBunnyCage);
            Item.createTile = ModContent.TileType<Tiles.ErmFishGoldCage>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Terrarium)
                .AddIngredient(ModContent.ItemType<Consumables.ErmFishGold>())
                .SortAfterFirstRecipesOf(ModContent.ItemType<ErmFishCage>()) // Makes sure it goes after ermfish
                .Register();
        }
    }
}