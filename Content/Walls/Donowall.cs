using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Walls
{
    public class Donowall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;

            DustType = DustID.Stone;

            AddMapEntry(new Color(82, 40, 29));
        }
    }
}