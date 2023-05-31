using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace UniqueItems.Items.DiamondSconce
{
	class DiamondSconce : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Increase Mining Speed by 50%");
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
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Diamond, 3)
				.AddIngredient(ItemID.Candle, 1)
				.AddTile(TileID.WorkBenches)
				.Register();
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.pickSpeed -= 0.5f; // 50% faster mining speed
		}
	}
}
