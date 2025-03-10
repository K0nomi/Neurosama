using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class NeuroVanityWig : ModItem
	{
		public override void SetDefaults() {
			Item.width = 22;
			Item.height = 30;

            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 3);
            Item.vanity = true;
            Item.maxStack = 1;
        }
	}
}
