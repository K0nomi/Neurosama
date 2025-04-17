using Microsoft.Xna.Framework;
using Neurosama.Content.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Neurosama.Content.Tiles.Furniture
{
    public abstract class ModBanner : ModTile
    {
        public virtual int NPCType => 0;

        //public virtual int Style => 0;

        //public override string Texture => (GetType().Namespace + ".Banners").Replace('.', '/');

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(TileID.Banners, 0));
            TileObjectData.newTile.StyleLineSkip = 2;
            TileObjectData.newTile.StyleHorizontal = true;

            //TileObjectData.newTile.Style = Style;

            TileObjectData.addTile(Type);

            DustType = -1;
            LocalizedText name = Language.GetText("MapObject.Banner");
            AddMapEntry(new Color(13, 88, 130), name);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            Tile tile = Main.tile[i, j];
            TileObjectData data = TileObjectData.GetTileData(tile);
            int topLeftX = i - tile.TileFrameX / 18 % data.Width;
            int topLeftY = j - tile.TileFrameY / 18 % data.Height;
            if (WorldGen.IsBelowANonHammeredPlatform(topLeftX, topLeftY))
            {
                offsetY -= 8;
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (!closer)
            {
                Main.SceneMetrics.NPCBannerBuff[NPCType] = true;
                Main.SceneMetrics.hasBanner = true;
            }
        }
    }
    public class ErmFishBanner : ModBanner
    {
        public override int NPCType => ModContent.NPCType<ErmFish>();
        //public override int Style => 0;
    }
    public class ErmSharkBanner : ModBanner
    {
        public override int NPCType => ModContent.NPCType<ErmShark>();
        //public override int Style => 1;
    }
}
