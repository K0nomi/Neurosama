using Microsoft.Xna.Framework;
using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Neurosama.Content.EmoteBubbles
{
    public abstract class ModChatEmote : ModEmoteBubble
    {
        // Redirect texture path.
        public override string Texture => (GetType().Namespace + ".ChatEmotes").Replace('.', '/');

        public override void SetStaticDefaults()
        {
            // Add Chat emotes to "General" category.
            AddToCategory(EmoteID.Category.Items);
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

    public class SmileEmote : ModChatEmote
    {
        public override int Row => 0;
    }
}
