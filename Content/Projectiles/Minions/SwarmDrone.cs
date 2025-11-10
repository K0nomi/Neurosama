using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Projectiles.Minions
{
    public class SwarmDrone : ModProjectile
    {
        private Vector2 idleOffset = new(0f, -32f);
        private Vector2 targetOffset = new(0f, -80f);

        private int shootCooldown;

        public override void SetStaticDefaults()
        {
            // Sets the amount of frames this minion has on its spritesheet
            Main.projFrames[Projectile.type] = 4;
            // This is necessary for right-click targeting
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
        }

        public sealed override void SetDefaults()
        {
            Projectile.width = 42;
            Projectile.height = 24;
            Projectile.tileCollide = false; // Makes the minion go through tiles freely

            // These below are needed for a minion weapon
            Projectile.friendly = true; // Controls if it deals damage to enemies on contact
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 0.9f;
            Projectile.penetrate = -1; // Needed so the minion doesn't despawn on collision with enemies or tiles

            shootCooldown = 20;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return false;
        }

        // The AI of this minion is split into multiple methods to avoid bloat. This method just passes values between calls actual parts of the AI.
        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            // Filter the projectiles and get their count
            // TODO: Find some way to do this per tick rather than per minion per tick
            IEnumerable<Projectile> matchingMinions = Main.projectile.Where(p => p.active && p.type == Projectile.type && p.owner == owner.whoAmI);
            int minionCount = matchingMinions.Count();

            // Find the position of the current minion
            int minionPosition = matchingMinions.ToList().FindIndex(p => p.identity == Projectile.identity);

            if (!CheckActive(owner))
            {
                return;
            }

            //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(((float)minionPosition / (float)minionCount).ToString()), new Color(0xEE, 0xDD, 0xFF));

            DoGeneralBehavior(owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition, minionPosition);
            SearchForTargets(owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter);
            DoMovement(foundTarget, distanceFromTarget, targetCenter, distanceToIdlePosition, vectorToIdlePosition, minionPosition, minionCount);
            DoVisuals();

            float minionIndexAsPercentage = (float)minionPosition / (float)minionCount;
            if (foundTarget && Main.GameUpdateCount % shootCooldown == (int)(minionIndexAsPercentage * shootCooldown) % shootCooldown)
            {
                Vector2 shootVector = targetCenter - Projectile.Center;
                shootVector.Normalize();
                shootVector *= 10f;
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectile(new EntitySource_Parent(Main.player[Projectile.owner]), Projectile.Center, shootVector, ProjectileID.MiniRetinaLaser, Projectile.damage, 0f);
                }
            }

            // Update frame
            if (++Projectile.frameCounter >= Main.projFrames[Projectile.type])
            {
                Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                Projectile.frameCounter = 0;
            }
        }

        // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
        private bool CheckActive(Player owner)
        {
            if (owner.dead || !owner.active)
            {
                owner.ClearBuff(ModContent.BuffType<Buffs.SwarmDrone>());

                return false;
            }

            if (owner.HasBuff(ModContent.BuffType<Buffs.SwarmDrone>()))
            {
                Projectile.timeLeft = 2;
            }

            return true;
        }

        private void DoGeneralBehavior(Player owner, out Vector2 vectorToIdlePosition, out float distanceToIdlePosition, int minionPosition)
        {
            var ticks = Main.GameUpdateCount;
            Vector2 ownerCentre = owner.Center;

            float frequency = 0.005f + (minionPosition / 3 * 0.0017f % 0.003f); // Vary the frequency
            float phaseShift = (float)(minionPosition / 3 * -1.4f % Math.PI + minionPosition * 0.45f % (Math.PI / 2f)); // Vary the phase shift
            float amplitudeScale = 8f + (minionPosition * 1.7f % 3f); // Vary the amplitude scale

            float hoverOffset = (float)Math.Sin(frequency * ticks + phaseShift) * amplitudeScale;

            Vector2 idlePosition = ownerCentre + idleOffset + new Vector2(0f, hoverOffset);
            idlePosition.X += (minionPosition / 3 * -48 - 48) * owner.direction; // Make columns
            idlePosition.Y += ((minionPosition + 1) % 3 - 1) * 24; // Make rows

            vectorToIdlePosition = idlePosition - Projectile.Center;
            distanceToIdlePosition = vectorToIdlePosition.Length();

            if (Main.myPlayer == owner.whoAmI && distanceToIdlePosition > 2000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                Projectile.position = idlePosition;
                Projectile.velocity *= 0.1f;
                Projectile.netUpdate = true;
            }

            // other stuff to make the idle more interesting?

        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            float maxDistance = 1000f;

            distanceFromTarget = maxDistance;
            targetCenter = Projectile.Center;
            foundTarget = false;

            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (foundTarget)
                return;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.CanBeChasedBy())
                    continue;

                float distance = npc.Center.Distance(Projectile.Center);
                bool isClosest = distance < distanceFromTarget;
                bool isInRange = distance <= maxDistance;
                bool hasLineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                bool closeEnoughToTargetRegardless = distance < 100f;

                if (isClosest && isInRange && (hasLineOfSight || closeEnoughToTargetRegardless))
                {
                    distanceFromTarget = distance;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }
        }

        private void DoMovement(bool foundTarget, float distanceFromTarget, Vector2 targetCenter, float distanceToIdlePosition, Vector2 vectorToIdlePosition, int minionPosition, int minionCount)
        {
            float speed = 16f;
            float inertia = 20f;

            if (foundTarget && distanceToIdlePosition < 2000f)
            {
                if (distanceFromTarget > 40f)
                {
                    // We want to be *above* the target
                    Vector2 direction = targetCenter - Projectile.Center;
                    direction += targetOffset;

                    float droneRelativeId = (float)minionPosition / (float)minionCount;
                    var ticks = Main.GameUpdateCount;
                    Vector2 circleOffset = new((float)Math.Sin((double)(droneRelativeId * (float)Math.PI * 2f + (float)ticks / 60f)), 0.3f * (float)Math.Cos((double)(droneRelativeId * (float)Math.PI * 2f + (float)ticks / 60f)));
                    direction += circleOffset * 100f;
                    direction.Normalize();
                    direction *= speed;

                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
                }
            }
            else
            {
                // We should be idling
                if (distanceToIdlePosition > 500f)
                {
                    speed = 24f;
                    inertia = 40f;
                }
                else
                {
                    speed = 10f;
                    inertia = 60f;
                }

                if (distanceToIdlePosition > 20f)
                {
                    vectorToIdlePosition.Normalize();
                    vectorToIdlePosition *= speed;
                    Projectile.velocity = (Projectile.velocity * (inertia - 1) + vectorToIdlePosition) / inertia;
                }
                else
                {
                    vectorToIdlePosition.Normalize();
                    if (vectorToIdlePosition.HasNaNs())
                    {
                        vectorToIdlePosition = Vector2.Zero;
                    }
                    vectorToIdlePosition *= 0.2f;
                    Projectile.velocity = vectorToIdlePosition + (Projectile.oldVelocity * 0.9f);
                }
            }
        }

        private void DoVisuals()
        {
            Projectile.rotation = Math.Clamp(Projectile.velocity.X * 0.1f, (float)Math.PI * -0.25f, (float)Math.PI * 0.25f);
        }
    }
}