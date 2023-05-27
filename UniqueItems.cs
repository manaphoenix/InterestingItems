using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace UniqueItems
{
	public class UniqueItems : ModSystem
	{
		public static ModKeybind SoulKey;
		internal UI.SoulChargeBar DebugUI;
		private UserInterface _interface;

		public override void AddRecipeGroups()
		{
			var evilGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " evil item", new int[]
			{
				ItemID.RottenChunk,
				ItemID.Vertebrae
			});
			RecipeGroup.RegisterGroup("InterestingItems:EvilItems", evilGroup);
		}

		public override void AddRecipes()
		{
			var recipe = Recipe.Create(ModContent.ItemType<Items.BloodPendant.BloodPendant>(), 1);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddIngredient(ItemID.CrossNecklace);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofLight);
			recipe.AddIngredient(ItemID.SoulofNight);
			recipe.AddRecipeGroup("InterestingItems:EvilItems", 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
			recipe = Recipe.Create(ModContent.ItemType<Items.EbonyGem.EbonyGem>(), 1);
			recipe.AddIngredient(ItemID.FallenStar);
			recipe.AddIngredient(ItemID.SandBlock, 20);
			recipe.AddIngredient(ItemID.Diamond);
			recipe.AddIngredient(ItemID.Deathweed);
			recipe.AddTile(TileID.Furnaces);
			recipe.Register();
			recipe = Recipe.Create(ModContent.ItemType<Items.ManaShield.ManaShield>(), 1);
			recipe.AddIngredient(ItemID.FallenStar, 5);
			recipe.AddIngredient(ItemID.Glass, 20);
			recipe.AddIngredient(ItemID.Diamond);
			recipe.AddIngredient(ItemID.CobaltShield);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.Register();
			recipe = Recipe.Create(ModContent.ItemType<Items.DiamondSconce.DiamondSconce>(), 1);
			recipe.AddIngredient(ItemID.Diamond, 3);
			recipe.AddIngredient(ItemID.Candle, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}

		public override void Load()
		{
			SoulKey = KeybindLoader.RegisterKeybind(Mod, "soul key", Microsoft.Xna.Framework.Input.Keys.V);
			if (!Main.dedServ)
			{
				DebugUI = new UI.SoulChargeBar();
				_interface = new UserInterface();
				DebugUI.Activate();
				_interface.SetState(DebugUI);
			}
		}

		public override void Unload()
		{
			SoulKey = null;
		}

		// UI STUFF?!
		
		public override void UpdateUI(GameTime gameTime)
		{
			if (UI.SoulChargeBar.Visible)
				_interface?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"YourMod: A Description",
					delegate
					{
						if (UI.SoulChargeBar.Visible)
							_interface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
		
	}
}