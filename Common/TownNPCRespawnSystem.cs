using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Neurosama.Common
{
    public class TownNPCRespawnSystem : ModSystem
    {
        public static bool unlockedNeuroSpawn = false;
        public static bool unlockedEvilSpawn = false;

        public override void ClearWorld()
        {
            unlockedNeuroSpawn = false;
            unlockedEvilSpawn = false;
        }

        public override void SaveWorldData(TagCompound tag)
        {
            tag[nameof(unlockedNeuroSpawn)] = unlockedNeuroSpawn;
            tag[nameof(unlockedEvilSpawn)] = unlockedEvilSpawn;
        }

        public override void LoadWorldData(TagCompound tag)
        {
            unlockedNeuroSpawn = tag.GetBool(nameof(unlockedNeuroSpawn));
            unlockedEvilSpawn = tag.GetBool(nameof(unlockedEvilSpawn));
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.WriteFlags(unlockedNeuroSpawn);
            writer.WriteFlags(unlockedEvilSpawn);
        }

        public override void NetReceive(BinaryReader reader)
        {
            reader.ReadFlags(out unlockedNeuroSpawn);
            reader.ReadFlags(out unlockedEvilSpawn);
        }
    }
}
