using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace UniqueItems.UI
{
	public class SoulChargeBar : UIState
	{
		public UIText text;
		public UIImage image;
		public UIElement frame;
		public static bool Visible = false;
		public string OutputText = "0";

		public override void OnInitialize()
		{
			frame = new UIElement();
			frame.Left.Set(-frame.Width.Pixels - 620f, 1f);
			frame.Top.Set(30f, 0);
			frame.Width.Set(256f, 0);
			frame.Height.Set(64f, 0);

			image = new UIImage(ModContent.Request<Texture2D>("UniqueItems/UI/SoulChargeBar"));
			image.Left.Set(0f, 0f);
			image.Top.Set(0f, 0f);
			image.Width.Set(256f, 0f);
			image.Height.Set(64f, 0f);
			
			text = new UIText(OutputText);
			text.Left.Set(64f, 0f);
			text.Top.Set(24f, 0f);
			text.Width.Set(180f, 0f);
			text.Height.Set(12f, 0f);

			frame.Append(image);
			frame.Append(text);
			Append(frame);
		}

		public override void Update(GameTime gameTime)
		{
			text.SetText(OutputText + "%");
		}

		public void SetText(string output)
		{
			OutputText = output;
		}
	}
}
