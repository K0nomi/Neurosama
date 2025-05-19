using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace Neurosama.Common
{
    internal class TimeoutGlobalNPC : GlobalNPC
    {
        public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
        {
            // Check if the item is the ban hammer
            if (item.type == ModContent.ItemType<Content.Items.Weapons.BanHammer>())
            {
                CombatText.NewText(new Rectangle((int)player.Center.X, (int)player.Center.Y, 0, 0), new Color(255, 191, 191), $"timeout {npc.FullName.Replace(" ", "_").ToLower()}");
            }
        }
    }
}
