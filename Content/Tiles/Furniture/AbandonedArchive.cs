using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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
            TileObjectData.newTile.CoordinateHeights = Enumerable.Repeat(16, TileHeight).ToArray();
            TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.newTile.StyleMultiplier = 2;

            TileObjectData.newTile.Origin = new Point16(5, 3); // Centred with bottom left bias like paintings  

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

            int style = TileObjectData.GetTileStyle(Main.tile[i, j]);
            player.cursorItemIconID = TileLoader.GetItemDropFromTypeAndStyle(Type, style);
        }

        public override bool RightClick(int i, int j)
        {
            SoundEngine.PlaySound(SoundID.Mech, new Vector2(i * 16, j * 16));
            ToggleTile(i, j);
            return true;
        }

        public override void HitWire(int i, int j)
        {
            ToggleTile(i, j);
        }

        public static void ToggleTile(int i, int j)
        {
            Tile tile = Main.tile[i, j];

            // Find the coordinates of top left tile square through math
            int topLeftY = j - tile.TileFrameY / 18 % TileHeight;
            int topLeftX = i - tile.TileFrameX / 18;

            short frameAdjustmentY = (short)(TileHeight * (tile.TileFrameY >= TileHeight * 18 ? -18 : 18));

            for (int x = topLeftX; x < topLeftX + TileWidth; x++)
            {
                for (int y = topLeftY; y < topLeftY + TileHeight; y++)
                {
                    Main.tile[x, y].TileFrameY += frameAdjustmentY;

                    // SkipWire all tile coordinates covered by this tile to make sure it doesnt activate multiple times
                    if (Wiring.running)
                    {
                        Wiring.SkipWire(x, y);
                    }
                }
            }

            // Avoid trying to send packets in singleplayer.
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                NetMessage.SendTileSquare(-1, topLeftX, topLeftY, TileWidth, TileHeight);
            }
        }
    }
}
