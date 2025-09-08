using Neurosama.Content.Tiles;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Neurosama.Common.Systems
{
	// add statue to the statue pool
	public class StatueWorldGen : ModSystem
	{
		public override void Load() {
			// Using a MonoMod detour, we can affect Terraria methods that otherwise have no tModLoader hook present. mhm exampleMod
			On_WorldGen.SetupStatueList += On_WorldGen_SetupStatueList;
		}

		private void On_WorldGen_SetupStatueList(On_WorldGen.orig_SetupStatueList orig) {
			// Call the original SetupStatueList method, this initializes GenVars.statueList with data. so true
			orig();

			// The vanilla game has an array of statue types that we'll be adding ours to.
			int startIndex = GenVars.statueList.Length; // Save the original length of the vanilla list to use later.

			// statues array for worldgen.
			// set shouldBeWired to true to make traps.
			(int type, bool shouldBeWired, ushort placeStyle)[] statueTypesToAdd = [
				(ModContent.TileType<ErmFishStatue>(), false, 0),
                //(ModContent.TileType<ErmSharkStatue>(), false, 0),
            ];

			// Make space in the statueList array.
			Array.Resize(ref GenVars.statueList, GenVars.statueList.Length + statueTypesToAdd.Length);

			// And then add Point16s of (TileID, PlaceStyle) to it. mhm exampleMod
			for (int i = 0; i < statueTypesToAdd.Length; i++) {
				int arrayIndex = startIndex + i;
				(int statueType, bool shouldBeWired, ushort placeStyle) = statueTypesToAdd[i];

				GenVars.statueList[arrayIndex] = new Point16(statueType, placeStyle);

				if (shouldBeWired) {
					GenVars.StatuesWithTraps.Add(arrayIndex);
				}
			}
		}
	}
}
