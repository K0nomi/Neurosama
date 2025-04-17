using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Tiles
{
    public class Donobrick : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMergeDirt[Type] = true; // Tile has special behavior when merging with dirt
            Main.tileBrick[Type] = true; // Presumably a variant of tileBlendAll that works with predefined tiles rather than any tile

            DustType = DustID.Stone;
            HitSound = SoundID.Tink;

            AddMapEntry(new Color(174, 92, 70));
        }
    }
}
