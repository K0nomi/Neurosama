using System.Linq;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader.Utilities;
using Terraria.ModLoader;
using Terraria;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace Neurosama.Content.NPCs
{
    internal class ErmFishGold : ModNPC
    {
        private const int ClonedNPCID = NPCID.GoldGoldfish;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[ClonedNPCID];
            Main.npcCatchable[Type] = true;

            NPCID.Sets.CountsAsCritter[Type] = true;
            NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
            NPCID.Sets.TownCritter[Type] = true;

            // Shimmer into faeling, like other critters
            NPCID.Sets.ShimmerTransformToNPC[Type] = NPCID.Shimmerfly;

            // Immune to confused like goldfish
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Position = new Vector2(0, 4),
                IsWet = true,
                Velocity = 1f,
                Direction = -1
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            // Goes after ermfish in bestiary?
            NPCID.Sets.NormalGoldCritterBestiaryPriority.Append(Type);
            //NPCID.Sets.NormalGoldCritterBestiaryPriority.Insert(NPCID.Sets.NormalGoldCritterBestiaryPriority.IndexOf(ModContent.NPCType<ErmFish>()) + 1, Type);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(ClonedNPCID);

            // width = 12;
            NPC.height = 20;
            // HitSound = SoundID.NPCHit1; // TODO
            // DeathSound = SoundID.NPCDeath1; // TODO

            NPC.catchItem = ModContent.ItemType<Items.Consumables.ErmFishGold>();
            NPC.lavaImmune = false;
            AIType = ClonedNPCID;
            AnimationType = ClonedNPCID;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
                // Sets the spawning conditions and background for the bestiary entry
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,

				// Beastiary element from localisation key		
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Neurosama.Bestiary.ErmFishGold"))
            ]);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.WaterCritter.Chance * 0.00025f; // 1/400 * 0.1f
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.GoldCritter_LessOutline, 2 * hit.HitDirection, -2f);
                    if (Main.rand.NextBool(2))
                    {
                        dust.noGravity = true;
                        dust.scale = 1.2f * NPC.scale;
                    }
                    else
                    {
                        dust.scale = 0.7f * NPC.scale;
                    }
                }
            }
        }

        // TODO: gold critter passive particle effect
        // TODO: gold text for lifeform analyser rare creatures
    }
}
