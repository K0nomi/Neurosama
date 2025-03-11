using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Tiles
{
	public class Donobrick : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
            //TODO: tileBlendAll causes tileMergeDirt to be ignored, find a fix so it behaves like red brick (for example)
            Main.tileMergeDirt[Type] = true; // tile has special behavior when merging with dirt
            Main.tileBlendAll[Type] = true; // adjacent tiles will connect (blend) with this 
            Main.tileBlockLight[Type] = true;

			DustType = DustID.Stone;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(174, 92, 70));
		}
    }
}