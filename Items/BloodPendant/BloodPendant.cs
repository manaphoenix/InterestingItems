using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UniqueItems.Items.BloodPendant
{
	class BloodPendant : ModItem
	{
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Damage charges this item, when charged release it do that damage out.");
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(0, 7, 40, 20);
            Item.rare = ItemRarityID.Pink;
            Item.accessory = true;
            Item.defense = 3;
        }

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            var mp = player.GetModPlayer<UniqueItemsPlayer>();

            mp.SoulEffect = true;
        }
	}
}
