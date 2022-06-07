using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UniqueItems.Items.ManaShield
{
	class ManaShield : ModItem
	{
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("50% of all damage is taken from mana first.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 7, 40, 20);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<UniqueItemsPlayer>();

            mp.ManaShield = true;
        }
    }
}
