using InterestingItems.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace InterestingItems
{
	class InterestingItemsPlayer : ModPlayer
	{
		public bool SoulEffect;
		public double SoulCharge;
		public DebugUI Ui;
		public bool CanCrit = false;
		public float knockback = 0;
		private readonly int SoulEffectRange = 256;

		public override void Initialize()
		{
			Ui = GetInstance<InterestingItems>().DebugUI;
		}

		public override void ResetEffects()
		{
			SoulEffect = false;
		}

		public override void clientClone(ModPlayer clientClone)
		{
			var clone = clientClone as InterestingItemsPlayer;
			clone.SoulEffect = SoulEffect;
			clone.SoulCharge = SoulCharge;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write(SoulEffect);
			packet.Write(SoulCharge);
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			InterestingItemsPlayer clone = clientPlayer as InterestingItemsPlayer;
			if (clone.SoulEffect != SoulEffect)
			{
				var packet = mod.GetPacket();
				packet.Write(SoulEffect);
				packet.Write(SoulCharge);
				packet.Send();
			}
		}

		public override void UpdateDead()
		{
			SoulCharge = 0;
			Ui.SetText(SoulCharge.ToString());
		}

		public override void OnRespawn(Player player)
		{
			Ui.SetText(SoulCharge.ToString());
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (InterestingItems.SoulKey.JustPressed && SoulEffect && SoulCharge > 0)
			{
				var hit = false;
				for (var i = 0; i < Main.maxNPCs; i++)
				{
					var npc = Main.npc[i];
					if (npc.CanBeChasedBy())
					{
						var dist = Vector2.Distance(npc.Center, player.Center);
						if (dist <= SoulEffectRange)
						{
							GetModNPC(npc.type).StrikeNPC(ref SoulCharge, 0, ref knockback, 0, ref CanCrit);
							hit = true;
						}
					}
				}
				if (hit)
				{
					SoulCharge = 0;
					Ui.SetText(SoulCharge.ToString());
				}
			}
		}

		public override void UpdateEquips(ref bool wallSpeedBuff, ref bool tileSpeedBuff, ref bool tileRangeBuff)
		{
			if (!SoulEffect)
			{
				SoulCharge = 0;
				DebugUI.Visible = false;
			} else
			{
				DebugUI.Visible = true;
			}
		}

		public override void Hurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit)
		{
			if (SoulEffect && SoulCharge < (1000 + (40 * player.statDefense)))
			{
				SoulCharge += (float)(damage * 0.25);
				Ui.SetText(SoulCharge.ToString());
			}

			SoulCharge = SoulCharge < (1000 + (40 * player.statDefense)) ? SoulCharge : (1000 + (40 * player.statDefense));
		}
	}
}
