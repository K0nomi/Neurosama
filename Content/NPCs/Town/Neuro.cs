using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.Chat;
using Neurosama.Content.Projectiles.Minions;

namespace Neurosama.Content.NPCs.Town
{
    // [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all
    [AutoloadHead]
    public class Neuro : ModNPC
    {
        public const string ShopName = "Shop";
        public int NumberOfTimesTalkedTo = 0;

        private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile NPCProfile;

        // TODO: unique neuro sounds
        private static SoundStyle deathSound = new($"{nameof(Neurosama)}/Assets/Sounds/evil_aaaa");
        private static SoundStyle hitSound = new($"{nameof(Neurosama)}/Assets/Sounds/neuro_erf");

        public override void Load()
        {
            // Adds the Shimmer Head to the NPCHeadLoader
            ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 23; // The total amount of frames the NPC has

            NPCID.Sets.ExtraFramesCount[Type] = 9;
            NPCID.Sets.AttackFrameCount[Type] = 4;
            NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the NPC that it tries to attack enemies
            NPCID.Sets.AttackType[Type] = 0; // 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
            NPCID.Sets.AttackTime[Type] = 90;
            NPCID.Sets.AttackAverageChance[Type] = 30; // The denominator for the chance for a Town NPC to attack

            NPCID.Sets.ShimmerTownTransform[NPC.type] = true; // NPC has a shimmered form
            NPCID.Sets.ShimmerTownTransform[Type] = true; // Allows for this NPC to have a different texture after touching the Shimmer liquid

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 1f,
                Direction = -1
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            // TODO: better happiness thingies
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
                .SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike)
                .SetNPCAffection<Evil>(AffectionLevel.Love) // cute sisters
                .SetNPCAffection(NPCID.Dryad, AffectionLevel.Love)
                .SetNPCAffection(NPCID.Guide, AffectionLevel.Like)
                .SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike)
                .SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate)
            ;

            NPCProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture)),
                new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex)
            );
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true; // Sets NPC to be a Town NPC
            NPC.friendly = true; // NPC Will not attack player
            NPC.width = 18;
            NPC.height = 40;
            NPC.aiStyle = 7;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = hitSound;
            NPC.DeathSound = deathSound;
            NPC.knockBackResist = 0.5f;

            AnimationType = NPCID.PartyGirl;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
				// Preferred biome
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Beastiary element from localisation key		
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Neurosama.Bestiary.Neuro"))
            ]);
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                // Create 4 random smoke coulds to mimic angler and princess gores.
                List<int> gores = [
                    GoreID.Smoke1,
                    GoreID.Smoke2,
                    GoreID.Smoke3,
                ];

                for (int k = 0; k < 4; k++)
                {
                    int randomGore = gores[Main.rand.Next(gores.Count)];
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, randomGore, 1f);
                }

                // This is so sad neuro has died
                SoundEngine.PlaySound(NPC.DeathSound, NPC.position);

                // LegacyMisc.36 is "{0} has left!"
                if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(Language.GetTextValue("LegacyMisc.36", NPC.GivenName), 255, 25, 25);
                else if (Main.netMode == NetmodeID.Server) ChatHelper.BroadcastChatMessage(NetworkText.FromKey("LegacyMisc.36", NPC.GetGivenNetName()), new Color(255, 25, 25));

                // Deactivate npc
                NPC.active = false;
                NPC.netSkip = -1;

                // TODO: increase kill count porperly
            }
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        { // Requirements for the town NPC to spawn.

            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (!player.active)
                {
                    continue;
                }

                // Player has to have a Turtle in order for Neuro to spawn
                // TODO: better requirements
                if (player.inventory.Any(item => item.type == ItemID.Turtle || item.type == ItemID.TurtleJungle))
                {
                    return true;
                }
            }

            return false;
        }

        public override ITownNPCProfile TownNPCProfile()
        {
            return NPCProfile;
        }

        public override List<string> SetNPCNameList()
        {
            List<string> list = new();

            string conmmonGivenName = Language.GetTextValue("Mods.Neurosama.NPCs.Neuro.DisplayName");
            string rareGivenName = Language.GetTextValue("Mods.Neurosama.NPCs.Neuro.RareName");

            for (int i = 0; i < 15; i++)
            {
                list.Add(conmmonGivenName);
            }

            // 1/16 chance for rare name
            list.Add(rareGivenName);

            return list;
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new();

            int evilNPC = NPC.FindFirstNPC(ModContent.NPCType<Evil>());

            if (evilNPC != -1)
            {
                string evilNPCName = Main.npc[evilNPC].GivenName;

                // Dialogue for if Evil is in world
                chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.EvilDialogue1", evilNPCName));
                //chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.EvilDialogue2", evilNPCName));
            }

            if (Main.bloodMoon)
            {
                // Dialogue for if it's a blood moon
                chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.BloodMoonDialogue1"));
                //chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.BloodMoonDialogue2"));
            }

            if (Main.IsItRaining)
            {
                // Dialogue for if it's raining
                chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.RainDialogue1"));
                chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.RainDialogue2"));
            }

            // Regular dialogue
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.StandardDialogue1"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.StandardDialogue2"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.StandardDialogue3"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.StandardDialogue4"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.StandardDialogue5"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.StandardDialogue6"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.StandardDialogue7"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.StandardDialogue8"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.StandardDialogue9"));

            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Neuro.RareDialogue"), 0.25);

            return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("LegacyInterface.28");
            //button2 = Language.GetTextValue("Mods.Neurosama.UI.SayItBack");
        }

        public override void OnChatButtonClicked(bool firstButton, ref string shop)
        {
            if (firstButton)
            {
                shop = ShopName;
            }
        }

        public override void AddShops()
        {
            var npcShop = new NPCShop(Type, ShopName)
                .Add<Items.Furniture.NeuroCatErm>()
                .Add<Items.Furniture.NeuroFumo>()
                .Add<Items.Furniture.AbandonedArchive>()
                .Add<Items.Donowall>()
                .Add<Items.NeuroMusicBox>()
                //.Add<Items.SwarmPet>()
                .Add<Items.Iwannadie>(Condition.IsNpcShimmered) // shimmer test
            ;

            npcShop.Register();
        }

        // Queen statue only
        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        /* public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
			projType = ModContent.ProjectileType<SwarmDrone>();
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
			multiplier = 12f;
			randomOffset = 2f;
			// gravityCorrection is left alone.
		} */
    }
}