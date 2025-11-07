using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria;
using System.Linq;

namespace Neurosama.Common
{
    public class TwinTownNPCProfile(string[] textures, int[] headIndexes) : ITownNPCProfile
    {
        // Load textures only one time during mod load time.
        private readonly Asset<Texture2D>[] variants = textures.Select(texture => ModContent.Request<Texture2D>(texture)).ToArray();

        public int RollVariation() => 0;

        public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

        public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
        {
            return variants?.ElementAtOrDefault(npc.townNpcVariationIndex) ?? variants[0];
        }

        public int GetHeadTextureIndex(NPC npc)
        {
            int index = npc.townNpcVariationIndex;
            return (index >= 0 && index < headIndexes.Length) ? headIndexes[index] : headIndexes[0];
        }
    }
}
