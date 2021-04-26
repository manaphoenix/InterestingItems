using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace UniqueItems
{
	public class UniqueItems : Mod
	{
		public static ModHotKey SoulKey;
		internal UI.DebugUI DebugUI;
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
			var recipe = new ModRecipe(this);
			recipe.AddIngredient(ItemID.SoulofMight, 5);
			recipe.AddIngredient(ItemID.CrossNecklace);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofLight);
			recipe.AddIngredient(ItemID.SoulofNight);
			recipe.AddRecipeGroup("InterestingItems:EvilItems", 5);

			recipe.AddTile(TileID.MythrilAnvil);

			recipe.SetResult(ModContent.ItemType<Items.BloodPendant.BloodPendant>());

			recipe.AddRecipe();
		}

		public override void Load()
		{
			SoulKey = RegisterHotKey("Activate Soul Item", "V");
			if (!Main.dedServ)
			{
				DebugUI = new UI.DebugUI();
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
			if (UI.DebugUI.Visible)
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
						if (UI.DebugUI.Visible)
							_interface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}