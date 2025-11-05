using Neurosama.Common;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Furniture
{
    [AutoloadEquip(EquipType.Head)]
    public class NeuroFumo : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.Furniture.NeuroFumo>());

            Item.width = 32;
            Item.height = 32;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.buyPrice(0, 2);

            Item.vanity = true;
            ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<NeurosamaPlayer>().neuroFumoEquipped = true;
        }
    }
}
