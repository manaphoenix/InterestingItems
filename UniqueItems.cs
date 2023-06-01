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
			var candleGroup = new RecipeGroup(() => Language.GetTextValue("LegacyMisc.37") + " candle", new int[]
			{
				ItemID.Candle,
				ItemID.PlatinumCandle
			});
			RecipeGroup.RegisterGroup("InterestingItems:Candles", candleGroup);
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