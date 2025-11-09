using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Neurosama.Content.Tiles.Furniture
{
    public class VedalFumo : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            //TileID.Sets.FramesOnKillWall[Type] = true;

            // Create normal
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;

            TileObjectData.newTile.DrawYOffset = 2;

            // Anchor data to allow it to sit on the neuros
            TileObjectData.newTile.AnchorAlternateTiles = [ModContent.TileType<NeuroFumo>(), ModContent.TileType<EvilFumo>()];
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.AlternateTile, TileObjectData.newTile.Width, 0);

            // I have no idea why you need these lines
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = true;

            // Create flipped ver
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1); // Facing right will use the second texture style

            // Add tile
            TileObjectData.addTile(Type);

            // Reuse the item localization for the map entry
            AddMapEntry(new Color(49, 167, 76), ModContent.GetInstance<Items.Furniture.VedalFumo>().DisplayName);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }

        public override bool CanPlace(int i, int j)
        {
            // The tile at (i, j) is the bottom left by default with TileObjectData.Style2x2
            Tile tileBelowLeft =  Main.tile[i,     j + 1];
            Tile tileBelowRight = Main.tile[i + 1, j + 1];

            // FrameX 18 or 54 (top right) of fumo is below the left side
            if ((tileBelowLeft.TileType == ModContent.TileType<NeuroFumo>() || tileBelowLeft.TileType == ModContent.TileType<EvilFumo>()) &&
                (tileBelowLeft.TileFrameX == 18 || tileBelowLeft.TileFrameX == 54))
            {
                return false;
            }

            // FrameX 0 or 36 (top left) of fumo is below the right side
            if ((tileBelowRight.TileType == ModContent.TileType<NeuroFumo>() || tileBelowRight.TileType == ModContent.TileType<EvilFumo>()) &&
                (tileBelowRight.TileFrameX == 0 || tileBelowRight.TileFrameX == 36))
            {
                return false;
            }

            // Tutel should be either directly on a fumo or not at all, so allow place
            return true;
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            Tile tile = Main.tile[i, j];
            int left = i - (tile.TileFrameX / 18 % 2);
            int top = j - (tile.TileFrameY / 18 % 2);

            Tile tileBelowLeft = Main.tile[left, top + 2];

            if (tileBelowLeft.TileType == ModContent.TileType<NeuroFumo>() || tileBelowLeft.TileType == ModContent.TileType<EvilFumo>())
            {
                offsetY += 4;
            }
        }
    }
}
