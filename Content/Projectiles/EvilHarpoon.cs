using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Projectiles
{
    public class EvilHarpoon : ModProjectile
    {
        private static readonly Asset<Texture2D> ChainTexture = ModContent.Request<Texture2D>(typeof(EvilHarpoon).FullName.Replace('.', '/') + "_Chain");

        private enum AIState
        {
            Launching,
            Retracting,
        }

        private AIState CurrentAIState
        {
            get => (AIState)Projectile.ai[0];
            set => Projectile.ai[0] = (float)value;
        }

        public int GravityDelayTimer
        {
            get => (int)Projectile.ai[1];
            set => Projectile.ai[1] = value;
        }

        public NPC Attacker
        {
            get => Main.npc[(int)Projectile.ai[2]];
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Harpoon);

            Projectile.aiStyle = 0;
            Projectile.localNPCHitCooldown = 10; // How often the projectile can hit an NPC
        } 

        public override void AI()
        {
            NPC attacker = Attacker;

            // Kill the projectile if the attacker dies or gets too far
            if (!attacker.active || Vector2.Distance(Projectile.Center, attacker.Center) > 1600f)
            {
                Projectile.Kill();
                return;
            }

            float retractSpeed = 20f; // The speed the projectile will have while retracting
            float maxLaunchLength = 800f; // How far the projectile's chain can stretch before being forced to retract when in launched state
            float gravity = 0.3f;
            int gravityDelay = 20; // Higher than what matches vanilla so eliv doesnt miss everything

            switch (CurrentAIState)
            {
                case AIState.Launching:
                    {
                        // Apply gravity after a delay
                        if (GravityDelayTimer >= gravityDelay)
                        {
                            Projectile.velocity.Y += gravity;
                        }
                        else
                        {
                            GravityDelayTimer++;
                        }

                        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

                        if (Projectile.Distance(attacker.Center) >= maxLaunchLength)
                        {
                            // Projectile went too far, switch to retracting
                            CurrentAIState = AIState.Retracting;
                            Projectile.netUpdate = true; // ?
                        }

                        attacker.direction = (attacker.Center.X < Projectile.Center.X).ToDirectionInt();

                        break;
                    }
                case AIState.Retracting:
                    {
                        Projectile.tileCollide = false;

                        if (Projectile.Distance(attacker.Center) <= retractSpeed)
                        {
                            Projectile.Kill(); // Kill the projectile once it is close enough to eliv
                            return;
                        }

                        Vector2 unitVectorTowardsAttacker = Projectile.DirectionTo(attacker.Center).SafeNormalize(Vector2.Zero);
                        Projectile.velocity = unitVectorTowardsAttacker * retractSpeed;

                        // Subtract instead of add to make the projectile face away
                        Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.ToRadians(90f);

                        break;
                    }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Vector2 velocity = Projectile.velocity;

            // Force retraction
            CurrentAIState = AIState.Retracting;
            Projectile.netUpdate = true;

            // Play the sound and create dust
            Collision.HitTiles(Projectile.position, velocity, Projectile.width, Projectile.height);
            SoundEngine.PlaySound(SoundID.Dig, Projectile.position);

            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (CurrentAIState == AIState.Retracting)
            {
                // Don't need to worry if you already hit something
                return;
            }

            // Force retraction
            CurrentAIState = AIState.Retracting;
            Projectile.netUpdate = true;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            // Always knock the enemy away, even if the harpoon is returning
            modifiers.HitDirectionOverride = (Attacker.Center.X < target.Center.X).ToDirectionInt();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            //TODO chain is offset and starts past the projectile when travelling clockwise around the npc.

            NPC attacker = Attacker;

            Vector2 attackerArmPosition = attacker.Center + new Vector2(attacker.direction * 12f, 0f); //Main.GetPlayerArmPosition(Projectile)
            //attackerArmPosition.Y -= attacker.gfxOffY;

            //Asset<Texture2D> chainTexture = TextureAssets.Chain;
            Asset<Texture2D> chainTexture = ChainTexture;

            Vector2 chainOrigin = chainTexture.Size() / 2f;
            Vector2 chainDrawPosition = Projectile.Center - Projectile.velocity.SafeNormalize(Vector2.Zero) * 21f * (CurrentAIState == AIState.Retracting ? -1 : 1); // go back half the texture width as the centre is the tip, invert when retracting
            Vector2 vectorFromProjectileToAttackerArms = attackerArmPosition.MoveTowards(chainDrawPosition, 4f) - chainDrawPosition;
            Vector2 unitVectorFromProjectileToAttackerArms = vectorFromProjectileToAttackerArms.SafeNormalize(Vector2.Zero);
            float chainSegmentLength = chainTexture.Height();
            if (chainSegmentLength == 0)
            {
                chainSegmentLength = 10; // When the chain texture is being loaded, the height is 0 which would cause infinite loops.
            }
            float chainRotation = unitVectorFromProjectileToAttackerArms.ToRotation() + MathHelper.PiOver2;
            int chainCount = 0;
            float chainLengthRemainingToDraw = vectorFromProjectileToAttackerArms.Length() + chainSegmentLength / 2f;

            // This while loop draws the chain texture from the projectile to the player, looping to draw the chain texture along the path
            while (chainLengthRemainingToDraw > 0f)
            {
                // This code gets the lighting at the current tile coordinates
                Color chainDrawColor = Lighting.GetColor((int)chainDrawPosition.X / 16, (int)(chainDrawPosition.Y / 16f));

                // Here, we draw the chain texture at the coordinates
                Main.spriteBatch.Draw(chainTexture.Value, chainDrawPosition - Main.screenPosition, null, chainDrawColor, chainRotation, chainOrigin, 1f, SpriteEffects.None, 0f);

                // chainDrawPosition is advanced along the vector back to the player by the chainSegmentLength
                chainDrawPosition += unitVectorFromProjectileToAttackerArms * chainSegmentLength;
                chainCount++;
                chainLengthRemainingToDraw -= chainSegmentLength;
            }

            // Draw the harpoon projectile
            Asset<Texture2D> projectileTexture = TextureAssets.Projectile[Projectile.type]; // ProjectileID.Harpoon / Projectile.type
            Main.spriteBatch.Draw(projectileTexture.Value, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, Projectile.Size / 2f, Projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void OnKill(int timeLeft)
        {
            // Reset cooldown as projectile is gone
            Attacker.ai[3] = 0;
        }
    }
}
