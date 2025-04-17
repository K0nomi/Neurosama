using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Neurosama.Content.EmoteBubbles
{
    public abstract class ModTownEmote : ModEmoteBubble
    {
        // Redirect texture path.
        public override string Texture => (GetType().Namespace + ".NPCEmotes").Replace('.', '/');

        public override void SetStaticDefaults()
        {
            // Add NPC emotes to "Town" category.
            AddToCategory(EmoteID.Category.Town);
        }

        /// <summary>
        /// Which row of the sprite sheet is this NPC emote in?
        /// This is used to help get the correct frame rectangle for different emotes.
        /// </summary>
        public virtual int Row => 0;

        public override Rectangle? GetFrame()
        {
            return new Rectangle(EmoteBubble.frame * 34, 28 * Row, 34, 28);
        }

        public override Rectangle? GetFrameInEmoteMenu(int frame, int frameCounter)
        {
            return new Rectangle(frame * 34, 28 * Row, 34, 28);
        }
    }

    public class NeuroEmote : ModTownEmote
    {
        public override int Row => 0;
    }

    public class EvilEmote : ModTownEmote
    {
        public override int Row => 1;
    }
}
