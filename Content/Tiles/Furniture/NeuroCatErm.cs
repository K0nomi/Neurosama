using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Neurosama.Content.Tiles.Furniture
{
    public class NeuroCatErm : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.addTile(Type);

            // Reuse the item localization for the map entry
            AddMapEntry(new Color(120, 85, 60), ModContent.GetInstance<Items.Furniture.NeuroCatErm>().DisplayName);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
    }
}
