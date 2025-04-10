using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Consumables
{
    public class PineapplePizza : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;

            Main.RegisterItemAnimation(Type, new DrawAnimationVertical(int.MaxValue, 3));

            ItemID.Sets.FoodParticleColors[Item.type] = [
                new(162, 119, 88),
                new(255, 217, 12),
                new(226, 191, 191)
            ];

            ItemID.Sets.IsFood[Type] = true;
        }

        public override void SetDefaults()
        {
            Item.DefaultToFood(22, 22, BuffID.WellFed, 18000);
            Item.RebuildTooltip();
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 20); // 20 neuros
        }
    }
}