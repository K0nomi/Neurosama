using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Neurosama.Content.Items.Furniture;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Neurosama.Common
{
    public class NeurosamaHeadLayer : PlayerDrawLayer
    {
        private static readonly Texture2D neuroFumoTexture = ModContent.Request<Texture2D>(typeof(NeuroFumo).FullName.Replace('.', '/')).Value;
        private static readonly Texture2D evilFumoTexture = ModContent.Request<Texture2D>(typeof(EvilFumo).FullName.Replace('.', '/')).Value;
        private static readonly Texture2D vedalFumoTexture = ModContent.Request<Texture2D>(typeof(VedalFumo).FullName.Replace('.', '/')).Value;

        public override bool IsHeadLayer => true;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            NeurosamaPlayer modPlayer = drawPlayer.GetModPlayer<NeurosamaPlayer>();

            if (modPlayer.neuroFumoEquipped & drawPlayer.armor[10].IsAir ||
                modPlayer.evilFumoEquipped & drawPlayer.armor[10].IsAir ||
                modPlayer.vedalFumoEquipped & drawPlayer.armor[10].IsAir ||
                modPlayer.neuroFumoVanityEquipped ||
                modPlayer.evilFumoVanityEquipped ||
                modPlayer.vedalFumoVanityEquipped)
            {
                return true;
            }
            return false;
        }

        public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0f)
            {
                return;
            }

            Player drawPlayer = drawInfo.drawPlayer;
            NeurosamaPlayer modPlayer = drawPlayer.GetModPlayer<NeurosamaPlayer>();

            // Calculate offset for head bob
            int secondaryOffset;

            var bodFrame = drawPlayer.bodyFrame;
            if (bodFrame.Y == bodFrame.Height * 7 || bodFrame.Y == bodFrame.Height * 8 || bodFrame.Y == bodFrame.Height * 9
                || bodFrame.Y == bodFrame.Height * 14 || bodFrame.Y == bodFrame.Height * 15 || bodFrame.Y == bodFrame.Height * 16)
                secondaryOffset = 2;
            else
                secondaryOffset = 0;

            // Calculate head position
            Vector2 headPosition = drawInfo.helmetOffset +
                new Vector2(
                    (int)(drawInfo.Position.X - drawInfo.drawPlayer.bodyFrame.Width / 2 + drawInfo.drawPlayer.width / 2),
                    (int)(drawInfo.Position.Y + drawInfo.drawPlayer.height - drawInfo.drawPlayer.bodyFrame.Height) - secondaryOffset)
                + drawInfo.drawPlayer.headPosition
                + drawInfo.headVect;

            headPosition -= Main.screenPosition;

            // Get dye shader
            int dyeShader = drawPlayer.dye?[0].dye ?? 0;

            // Draw the equipped items
            int totalTwins =
                modPlayer.neuroFumoEquipped.ToInt() +
                modPlayer.evilFumoEquipped.ToInt() +
                modPlayer.neuroFumoVanityEquipped.ToInt() +
                modPlayer.evilFumoVanityEquipped.ToInt();

            if (totalTwins == 0)
            {
                // No twins equipped, just draw Vedal Fumo if equipped
                if (modPlayer.vedalFumoEquipped || modPlayer.vedalFumoVanityEquipped)
                    DrawFumo(drawInfo, drawPlayer, dyeShader, headPosition, vedalFumoTexture);
            }
            else if (totalTwins == 1)
            {
                // Draw the single equipped twin
                if (modPlayer.neuroFumoEquipped || modPlayer.neuroFumoVanityEquipped)
                    DrawFumo(drawInfo, drawPlayer, dyeShader, headPosition, neuroFumoTexture);
                else if (modPlayer.evilFumoEquipped || modPlayer.evilFumoVanityEquipped)
                    DrawFumo(drawInfo, drawPlayer, dyeShader, headPosition, evilFumoTexture);

                // Draw Vedal Fumo on top if equipped in vanity
                Vector2 vedalFumoOffset = new(drawPlayer.direction == 1 ? -2 : 2, 6 - neuroFumoTexture.Height);
                
                if (modPlayer.vedalFumoVanityEquipped)
                    DrawFumo(drawInfo, drawPlayer, dyeShader, headPosition + vedalFumoOffset, vedalFumoTexture);
            }
            else if (totalTwins == 2)
            {
                // Draw the twin equipped in armour
                if (modPlayer.neuroFumoEquipped)
                    DrawFumo(drawInfo, drawPlayer, dyeShader, headPosition, neuroFumoTexture);
                else if (modPlayer.evilFumoEquipped)
                    DrawFumo(drawInfo, drawPlayer, dyeShader, headPosition, evilFumoTexture);

                // Draw the twin equipped in vanity on top
                Vector2 doubleFumoOffset = new(drawPlayer.direction == 1 ? -2 : 2, 6 - neuroFumoTexture.Height);

                if (modPlayer.neuroFumoVanityEquipped)
                    DrawFumo(drawInfo, drawPlayer, dyeShader, headPosition + doubleFumoOffset, neuroFumoTexture);
                else if (modPlayer.evilFumoVanityEquipped)
                    DrawFumo(drawInfo, drawPlayer, dyeShader, headPosition + doubleFumoOffset, evilFumoTexture);
            }
        }

        private static void DrawFumo(PlayerDrawSet drawInfo, Player drawPlayer, int dyeShader, Vector2 headPosition, Texture2D texture)
        {
            Vector2 position = new(headPosition.X, headPosition.Y - 2);
            Vector2 origin = new(texture.Width / 2f, texture.Height);

            SpriteEffects spriteEffect = drawPlayer.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            DrawData drawData = new(texture, position, null, drawInfo.colorArmorHead, 0f, origin, 1f, spriteEffect, 0);
            drawData.shader = dyeShader;

            drawInfo.DrawDataCache.Add(drawData);
        }
    }
}