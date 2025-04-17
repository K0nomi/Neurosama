using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Neurosama.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class NeuroVanityUniform : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 18;

            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 3);
            Item.vanity = true;
            Item.maxStack = 1;
        }
    }
}
