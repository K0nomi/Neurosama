using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Chat;
using Microsoft.Xna.Framework;

namespace Neurosama.Content.NPCs.Town
{
    [AutoloadHead]
    public class Vedal : ModNPC
    {
        //private static int ShimmerHeadIndex;
        private static Profiles.StackedNPCProfile NPCProfile;

        public override void Load()
        {
            // Adds the Shimmer Head to the NPCHeadLoader
            //ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
        }

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 27;
            NPCID.Sets.ExtraFramesCount[Type] = 20; // The number of frames after the walking frames.
            NPCID.Sets.AttackFrameCount[Type] = 0; // Town Pets don't have any attacking frames.
            NPCID.Sets.ExtraTextureCount[Type] = 0;

            NPCID.Sets.DangerDetectRange[Type] = 250; // How far away the NPC will detect danger. Measured in pixels.
            NPCID.Sets.AttackType[Type] = -1;
            NPCID.Sets.AttackTime[Type] = -1;
            NPCID.Sets.AttackAverageChance[Type] = 1;

            NPCID.Sets.ShimmerTownTransform[Type] = false; // No shimmer variant atm
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Shimmer] = true; // Immune to shimmer
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true; // And confused

            NPCID.Sets.NPCFramingGroup[Type] = 4; // Party hat walking animation, Town Cat = 4, Town Dog = 5, Town Bunny = 6, Town Slimes = 7. Vedal is the same as the cat.
            NPCID.Sets.HatOffsetY[Type] = -4; // But his head is lower down 
            
            NPCID.Sets.IsTownPet[Type] = true; // NPC is a Town Pet
            NPCID.Sets.CannotSitOnFurniture[Type] = true; // TODO: vedal becomes an extreme alcoholic when sitting on furniture

            NPCID.Sets.TownNPCBestiaryPriority.Add(Type);

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new()
            {
                Velocity = 1f,
                Direction = -1
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

            NPCProfile = new Profiles.StackedNPCProfile(
                new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture))/*,
                new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex)*/
            );
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 10;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.damage = 10;
            NPC.defense = 15;
            NPC.lifeMax = 250;
            NPC.HitSound = SoundID.NPCHit1; // TODO: vedal hit sound (maybe a femboy noise)
            NPC.DeathSound = SoundID.NPCDeath27; // TODO: vedal death sound
            NPC.knockBackResist = 0.5f;
            NPC.housingCategory = 1; // Can share a house with a normal Town NPC. Could be cool to make it only neuro or evil
            AnimationType = NPCID.TownBunny;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange([
				// Preferred biome
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Beastiary element from localisation key		
				new FlavorTextBestiaryInfoElement(Language.GetTextValue("Mods.Neurosama.Bestiary.Vedal"))
            ]);
        }

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            // Check if Neuro or Evil is present in the world
            int evilNPC = NPC.FindFirstNPC(ModContent.NPCType<Evil>());
            int neuroNPC = NPC.FindFirstNPC(ModContent.NPCType<Neuro>());

            if (evilNPC != -1 || neuroNPC != -1)
            {
                return true;
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

            // Vedal dialogue is TODO
            chat.Add(Language.GetTextValue("Mods.Neurosama.Dialogue.Vedal.StandardDialogue1"));

            return chat;
        }

        public override void SetChatButtons(ref string button, ref string button2)
        {
            button = Language.GetTextValue("UI.PetTheAnimal"); // Probably a good idea to replace this for ved lol
        }
    }
}
