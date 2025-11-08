using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Weapons
{
    public class Iwannadie : ModItem
    {
        // test item lol

        public override void SetDefaults()
        {
            Item.damage = 1;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 80;
            Item.height = 80;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 4f;
            Item.value = 10000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            Item.shoot = ModContent.ProjectileType<Projectiles.EvilHarpoon>();
            Item.shootSpeed = 11f;
            Item.noMelee = false;
        }

        /*public override void AddRecipes()
        {
            // My beloved
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .AddIngredient(ItemID.Wood, 1)
                .AddIngredient(ModContent.ItemType<Consumables.ErmFishGold>(), 69)
                .AddTile(ModContent.TileType<Tiles.Furniture.NeuroCatErm>())
                .AddTile(TileID.WorkBenches)
                .Register();
        }*/
    }
}