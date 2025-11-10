using Mono.Cecil.Cil;
using MonoMod.Cil;
using Neurosama.Common;
using Neurosama.Content.NPCs.Town;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace Neurosama
{
    public class Neurosama : Mod
    {
        private delegate bool orig_get_IsShimmerVariant(NPC self);

        public override void Load()
        {
            // Hook into NPC.IsShimmerVariant {get;}
            MethodInfo targetMethod = typeof(NPC).GetMethod("get_IsShimmerVariant", BindingFlags.Instance | BindingFlags.Public);
            MonoModHooks.Add(targetMethod, On_get_IsShimmerVariant);

            // Hook into NPC.AI_007_TownEntities
            IL_NPC.AI_007_TownEntities += IL_NPC_AI_007_TownEntities;

        }

        private static bool On_get_IsShimmerVariant(orig_get_IsShimmerVariant orig, NPC self)
        { // The twins have more variants and need to use the least significant bit to determine shimmer state
            if (self.type == ModContent.NPCType<Neuro>() || self.type == ModContent.NPCType<Evil>())
            {
                return self.townNpcVariationIndex % 2 == 1;
            }
            return orig(self);
        }

        private void IL_NPC_AI_007_TownEntities(ILContext il)
        { // Patch TownNPC AI so that the right variant is chosen when shimmering a twin
            try
            {
                ILCursor c = new(il);

                // Find the right IL instruction (Where townNpcVariationIndex is set)
                FieldInfo townNpcVariationIndexField = typeof(NPC).GetField("townNpcVariationIndex", BindingFlags.Instance | BindingFlags.Public);
                c.TryGotoNext(i => i.MatchStfld(townNpcVariationIndexField));

                // push 'this' (the NPC instance)
                c.Emit(OpCodes.Ldarg_0);

                // Call a delegate using the original value and NPC from the stack.
                c.EmitDelegate<Func<int, NPC, int>>((computed, npc) =>
                {
                    if (npc.type == ModContent.NPCType<Neuro>() || npc.type == ModContent.NPCType<Evil>())
                        return npc.townNpcVariationIndex ^ 1; // Is a twin, only flip least significant bit
                    return computed; // Not a twin, push original value back
                });
            }
            catch (Exception e)
            {
                // If there are any failures with the IL editing, this will dump the IL to Logs/ILDumps/Neurosama/
                throw new ILPatchFailureException(this, il, e);
            }
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            var msgType = (NeurosamaMessageType)reader.ReadByte();

            switch (msgType)
            {
                case NeurosamaMessageType.ToggleTwinVariant:
                    int npcID = reader.ReadInt32();

                    // Sync to clients if being run on server
                    if (Main.dedServ)
                    {
                        ModPacket packet = GetPacket();
                        packet.Write((byte)msgType);
                        packet.Write(npcID);
                        packet.Send(ignoreClient: whoAmI); // Don't send to the person who pressed the button, it already changed for them
                    }

                    // Toggle the variant if it's nwero or eliv
                    NPC npc = Main.npc[npcID];
                    if (npc.ModNPC is Neuro neuro)
                    {
                        neuro.ToggleVariant();
                        break;
                    }
                    else if (npc.ModNPC is Evil evil)
                    {
                        //evil.ToggleVariant();
                        break;
                    }

                    Logger.WarnFormat("Can't change variant, NPC is not a twin: {0}", npc.type);
                    break;
                default:
                    Logger.WarnFormat("Unknown Message type: {0}", msgType);
                    break;
            }
        }
    }
}