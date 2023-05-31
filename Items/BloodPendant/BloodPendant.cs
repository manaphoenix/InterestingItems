using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace UniqueItems.Items.BloodPendant
{
	class BloodPendant : ModItem
	{
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.SoulofMight, 5)
				.AddIngredient(ItemID.CrossNecklace)
				.AddIngredient(ItemID.HallowedBar, 10)
				.AddIngredient(ItemID.SoulofLight)
				.AddIngredient(ItemID.SoulofNight)
				.AddRecipeGroup("InterestingItems:EvilItems", 5)
				.AddTile(TileID.MythrilAnvil)
				.Register();
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            var mp = player.GetModPlayer<UniqueItemsPlayer>();

            mp.SoulEffect = true;
        }
	}
}
