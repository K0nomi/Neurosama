using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using System.Collections.Generic;
using Terraria.Chat;

namespace Neurosama.Content.NPCs.Town
{
    // [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
    [AutoloadHead]
    public class Evil : ModNPC
    {
        public const string ShopName = "Shop";
        public int NumberOfTimesTalkedTo = 0;

        private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile NPCProfile;

        // TODO: unique evil sounds
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
            NPCID.Sets.AttackType[Type] = 1; // 0 = throwing, 1 = shooting, 2 = magic, 3 = melee
            NPCID.Sets.AttackTime[Type] = 60;
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
                .SetNPCAffection<Neuro>(AffectionLevel.Love) // cute sisters
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
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Neurosama.Bestiary.Evil"))
            ]);
        }

        public override bool PreAI()
        {
            if (NPC.ai[3] > 0)
            {
                NPC.ai[3]--;
            }

            return true;
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

                // TODO: this causes duplicate death messages, find a way to replace the default one
                // LegacyMisc.36 is "{0} has left!"
                //if (Main.netMode == NetmodeID.SinglePlayer) Main.NewText(Language.GetTextValue("LegacyMisc.36", NPC.FullName), 255, 25, 25);
                //else if (Main.netMode == NetmodeID.Server) ChatHelper.BroadcastChatMessage(NetworkText.FromKey("LegacyMisc.36", NPC.GetFullNetName()), new Color(255, 25, 25));
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

                // Player has to have a Turtle in order for Evil to spawn
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

        public override string GetChat()
        {
            WeightedRandom<string> chat = new();

            // Add dialogue for if Neuro is in world.
            int neuroNPC = NPC.FindFirstNPC(ModContent.NPCType<Neuro>());

            if (neuroNPC != -1)
            {
                string neuroNPCName = Main.npc[neuroNPC].GivenName;

                // Dialogue for if Neuro is in the world
                chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.NeuroDialogue1", neuroNPCName));
                //chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.NeuroDialogue2", neuroNPCName));
            }

            if (Main.bloodMoon)
            {
                // Dialogue for if it's a blood moon
                chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.BloodMoonDialogue1"));
                chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.BloodMoonDialogue2"));
            }

            // Regular dialogue
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.StandardDialogue1"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.StandardDialogue2"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.StandardDialogue3"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.StandardDialogue4"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.StandardDialogue5"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.StandardDialogue6"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.StandardDialogue7"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.StandardDialogue8"));
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.StandardDialogue9"));

            //chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Evil.RareDialogue"), 0.25);

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
                .Add<Items.Furniture.EvilFumo>()
                .Add<Items.Furniture.AbandonedArchive>()
                .Add<Items.Furniture.Donoplank>()
                .Add<Items.SwarmDrone>(Condition.DownedEyeOfCthulhu)
                .Add<Items.Iwannadie>(Condition.IsNpcShimmered) // shimmer test
            ;

            npcShop.Register();
        }

        // Queen statue only
        public override bool CanGoToStatue(bool toKingStatue) => !toKingStatue;

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        /*public override void DrawTownAttackGun(ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)
        {
            // TODO: harpoon is positioned wrong
            int itemType = ItemID.Harpoon;
            Main.GetItemDrawFrame(itemType, out item, out itemFrame);
            horizontalHoldoutOffset = (int)Main.DrawPlayerItemPos(1f, itemType).X - 12;
        }*/

        public override void TownNPCAttackShoot(ref bool inBetweenShots)
        {
            NPC target = null;
            float lowestDistance = float.MaxValue;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC potentialTarget = Main.npc[i];
                float distance = NPC.Distance(potentialTarget.Center);
                if (potentialTarget.active && potentialTarget.CanBeChasedBy() && distance < NPCID.Sets.DangerDetectRange[NPC.type] && distance < lowestDistance && Collision.CanHitLine(NPC.Center, 0, 0, potentialTarget.Center, 0, 0) && NPC.localAI[3] % NPCID.Sets.AttackTime[NPC.type] == 0)
                {
                    target = potentialTarget;
                    lowestDistance = distance;
                }
            }

            if (target != null && Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Add large cooldown because it is reset when the harpoon dies
                NPC.ai[3] = 450f;

                var handPosition = NPC.Center + new Vector2(NPC.direction * 12f, 0f);
                var unitVectorToTarget = (target.Top - handPosition).SafeNormalize(Vector2.Zero); // Maybe add height to target based on distance for better aiming?
                var projectile = Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), handPosition, unitVectorToTarget * 11f, ModContent.ProjectileType<Projectiles.EvilHarpoon>(), 20, 4f, ai2: NPC.whoAmI);
                projectile.npcProj = true;
                projectile.noDropItem = true;
            }
        }
    }
}