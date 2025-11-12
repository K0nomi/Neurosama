using Terraria.GameContent.UI;
using Terraria.ModLoader;

namespace Neurosama.Content.EmoteBubbles
{
    public class TutelEmote : ModEmoteBubble
    {
        public override void SetStaticDefaults()
        {
            AddToCategory(EmoteID.Category.General);
        }
    }
}
