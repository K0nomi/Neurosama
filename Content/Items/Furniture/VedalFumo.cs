using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Furniture
{
    [AutoloadEquip(EquipType.Head)]
    public class VedalFumo : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.VedalFumo>());

            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 2);

            // TODO: render properly in player select menu
            Item.vanity = true;
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }
    }
}
