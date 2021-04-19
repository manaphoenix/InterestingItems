using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace InterestingItems.UI
{
	public class DebugUI : UIState
	{
		public UIText text;
		public static bool Visible = false;
		public string OutputText = "0";

		public override void OnInitialize()
		{
			text = new UIText(OutputText);
			text.SetPadding(0);
			text.Left.Set(200f, 0f);
			text.Top.Set(100f, 0f);
			text.Width.Set(170f, 0f);
			text.Height.Set(70f, 0f);

			Append(text);
		}

		public override void Update(GameTime gameTime)
		{
			text.SetText("Soul Charge: " + OutputText);
		}

		public void SetText(string output)
		{
			OutputText = output;
		}
	}
}
