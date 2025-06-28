using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Neurosama.Content.Tiles.Furniture
{
    public class EvilFumo : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            //TileID.Sets.FramesOnKillWall[Type] = true;

            // Create normal
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;

            // I have no idea why you need these lines
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;

            // Create flipped ver
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;

            // Add tiles
            TileObjectData.addAlternate(1); // Facing right will use the second texture style
            TileObjectData.addTile(Type);

            // Reuse the item localization for the map entry
            AddMapEntry(new Color(209, 185, 177), ModContent.GetInstance<Items.Furniture.EvilFumo>().DisplayName);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
    }
}
