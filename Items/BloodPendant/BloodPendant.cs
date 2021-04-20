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
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = Item.sellPrice(0, 7, 40, 20);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
            item.defense = 3;
        }

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            var mp = player.GetModPlayer<InterestingItemsPlayer>();

            mp.SoulEffect = true;
        }
	}
}
