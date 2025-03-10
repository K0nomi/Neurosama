using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Tiles
{
	public class Donobrick : ModTile
	{
		public override void SetStaticDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true; // tile has special behavior when merging with dirt
			Main.tileBlendAll[Type] = true; // adjascent tiles will connect (blend) with this tile
			Main.tileBlockLight[Type] = true;

			DustType = DustID.Stone;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(174, 92, 70));
		}
    }
}