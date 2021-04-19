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
		private readonly int SoulEffectRange = 256;
		private bool CanCrit = false;
		private float Knockback = 0;

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

		private void HitNPC(NPC npc)
		{
			npc.life -= (int)SoulCharge;
			npc.HitEffect(0, SoulCharge);
			Main.PlaySound(npc.HitSound, npc.position);

			Color color2 = (CanCrit ? CombatText.DamagedHostileCrit : CombatText.DamagedHostile);
			CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, (int)SoulCharge, CanCrit);

			if (npc.life <= 0)
				npc.checkDead();
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
							HitNPC(npc);
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
		private void AddSoulCharge(int amount)
		{
			if (SoulEffect && SoulCharge < (1000 + (40 * player.statDefense)))
			{
				SoulCharge += amount;
				Ui.SetText(SoulCharge.ToString());
			}

			SoulCharge = SoulCharge < (1000 + (40 * player.statDefense)) ? SoulCharge : (1000 + (40 * player.statDefense));
		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			AddSoulCharge(npc.damage);
		}

		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			AddSoulCharge(proj.damage);
		}
	}
}
