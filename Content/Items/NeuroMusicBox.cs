using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items
{
    public class NeuroMusicBox : ModItem
    {
        public override void SetStaticDefaults()
        {
            ItemID.Sets.CanGetPrefixes[Type] = false; // music boxes can't get prefixes in vanilla
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.MusicBox; // recorded music boxes transform into the basic form in shimmer

            MusicLoader.AddMusicBox(Mod, MusicLoader.GetMusicSlot(Mod, "Assets/Music/LIFE"), ModContent.ItemType<NeuroMusicBox>(), ModContent.TileType<Tiles.NeuroMusicBox>());
        }

        public override void SetDefaults()
        {
            Item.DefaultToMusicBox(ModContent.TileType<Tiles.NeuroMusicBox>(), 0);
        }
    }
}
