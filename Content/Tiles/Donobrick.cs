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
			Main.tileMergeDirt[Type] = true; // tile has special behavior when merging with dirt
			Main.tileBlockLight[Type] = true;
			Main.tileBrick[Type] = true; // presumably a variant of tileBlendAll that works with predefined tiles rather than any tile

            // TODO: doesn't create dust when hit by harpoon projectiles (probably others too) (Collision.HitTiles)
            DustType = DustID.Stone;
			HitSound = SoundID.Tink;

			AddMapEntry(new Color(174, 92, 70));
		}
	}
}
