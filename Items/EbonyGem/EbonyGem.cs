using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UniqueItems.Items.EbonyGem
{
	class EbonyGem : ModItem
	{
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Gives Mana Vampirism.");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 1;
            item.value = Item.sellPrice(0, 7, 40, 20);
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<UniqueItemsPlayer>();

            mp.ManaVampirism = true;
        }
    }
}
