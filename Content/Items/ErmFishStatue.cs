using Neurosama.Content.Items.Consumables;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items
{
    public class ErmFishStatue : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.FishStatue);
            Item.height = 26;
            Item.createTile = ModContent.TileType<Tiles.ErmFishStatue>();
            Item.placeStyle = 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.StoneBlock, 50)
                .AddIngredient<ErmFish>(5)
                .AddTile(TileID.HeavyWorkBench)
                .AddCondition(Condition.InGraveyard)
                .Register();
        }
    }
}