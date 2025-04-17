using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Neurosama.Content.Tiles
{
    public class ErmFishCage : ModTile
    {
        private const int ClonedTileID = TileID.SquirrelCage;

        public override void SetStaticDefaults()
        {
            TileID.Sets.CritterCageLidStyle[Type] = TileID.Sets.CritterCageLidStyle[ClonedTileID];
            Main.tileFrameImportant[Type] = Main.tileFrameImportant[ClonedTileID];
            Main.tileLavaDeath[Type] = Main.tileLavaDeath[ClonedTileID];
            Main.tileSolidTop[Type] = Main.tileSolidTop[ClonedTileID];
            Main.tileTable[Type] = Main.tileTable[ClonedTileID];
            AnimationFrameHeight = 54;

            TileObjectData.newTile.CopyFrom(TileObjectData.GetTileData(ClonedTileID, 0));
            TileObjectData.addTile(Type);

            DustType = DustID.Glass; // TODO: dust count is wrong

            // Reuse the item localization for the map entry
            AddMapEntry(new Color(122, 217, 232), ModContent.GetInstance<Items.ErmFishCage>().DisplayName);
        }

        public override void SetDrawPositions(int i, int j, ref int width, ref int offsetY, ref int height, ref short tileFrameX, ref short tileFrameY)
        {
            offsetY = 2;
            Main.critterCage = true;
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset)
        {
            Tile tile = Main.tile[i, j];

            int tileCageFrameIndex = TileDrawing.GetSmallAnimalCageFrame(i, j, tile.TileFrameX, tile.TileFrameY);
            frameYOffset = Main.squirrelCageFrame[tileCageFrameIndex] * AnimationFrameHeight;
        }
    }
}