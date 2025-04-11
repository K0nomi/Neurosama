using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Neurosama.Content.Tiles.Furniture
{
    // 12x6 tile that can be placed on a wall, and changes on right click or hitwire
    public class AbandonedArchive : ModTile
    {
        private const int TileWidth = 12;
        private const int TileHeight = 6;

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileID.Sets.FramesOnKillWall[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
            TileObjectData.newTile.Width = TileWidth;
            TileObjectData.newTile.Height = TileHeight;
            TileObjectData.newTile.CoordinateHeights = Enumerable.Repeat(16, TileWidth).ToArray();

            TileObjectData.addTile(Type);

            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(134, 31, 165), name);
        }

        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }

        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;

            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
            player.cursorItemIconID = ModContent.ItemType<Items.Furniture.AbandonedArchive>();
        }

        public override bool RightClick(int i, int j)
        {
            HitWire(i, j);
            return true;
        }

        public override void HitWire(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            // Find the coordinates of top left tile square through math
            int topLeftY = j - (tile.TileFrameY / 18) % TileHeight;
            int topLeftX = i - tile.TileFrameX / 18;

            short frameAdjustmentY = (short)(TileHeight * (tile.TileFrameY >= TileHeight * 18 ? -18 : 18));

            for (int y = topLeftY; y < topLeftY + TileHeight; y++)
            {
                for (int x = topLeftX; x < topLeftX + TileWidth; x++)
                {
                    Main.tile[x, y].TileFrameY += frameAdjustmentY;

                    // SkipWire all tile coordinates covered by this tile to make sure it doesnt activate multiple times
                    Wiring.SkipWire(x, y);
                }
            }

            // Avoid trying to send packets in singleplayer.
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendTileSquare(-1, topLeftX, topLeftY, TileWidth, TileHeight, TileChangeType.None);
            }
        }
    }
}
