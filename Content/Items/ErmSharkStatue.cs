using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items
{
    public class ErmSharkStatue : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SharkStatue);
            Item.height = 26;
            Item.createTile = ModContent.TileType<Tiles.ErmSharkStatue>();
            Item.placeStyle = 0;
        }
    }
}