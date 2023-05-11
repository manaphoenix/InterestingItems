using Terraria.Audio;
using UniqueItems.UI;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.DataStructures;

namespace UniqueItems
{
	class UniqueItemsPlayer : ModPlayer
	{
		public bool SoulEffect;
		public double SoulCharge;
		public bool ManaVampirism;
		public bool ManaShield;
		public SoulChargeBar Ui;
		private readonly bool CanCrit = false;

		public override void Initialize()
		{
			Ui = GetInstance<UniqueItems>().DebugUI;
		}

		public override void ResetEffects()
		{
			SoulEffect = false;
			ManaVampirism = false;
			ManaShield = false;
		}

		public override void clientClone(ModPlayer clientClone)
		{
			var clone = clientClone as UniqueItemsPlayer;
			clone.SoulEffect = SoulEffect;
			clone.SoulCharge = SoulCharge;
			clone.ManaVampirism = ManaVampirism;
			clone.ManaShield = ManaShield;
		}

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write(SoulEffect);
			packet.Write(SoulCharge);
			packet.Write(ManaVampirism);
			packet.Write(ManaShield);
			packet.Send(toWho, fromWho);
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			UniqueItemsPlayer clone = clientPlayer as UniqueItemsPlayer;
			if (clone.SoulEffect != SoulEffect)
			{
				var packet = Mod.GetPacket();
				packet.Write(SoulEffect);
				packet.Write(SoulCharge);
				
				packet.Send();
			}
			if (clone.ManaVampirism != ManaVampirism)
			{
				var packet = Mod.GetPacket();
				packet.Write(ManaVampirism);
				packet.Send();
			}
			if (clone.ManaShield != ManaShield)
			{
				var packet = Mod.GetPacket();
				packet.Write(ManaShield);
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
			SoundEngine.PlaySound((SoundStyle)npc.HitSound, npc.position);

			Color color2 = (CanCrit ? CombatText.DamagedHostileCrit : CombatText.DamagedHostile);
			CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), color2, (int)SoulCharge, CanCrit);

			if (npc.life <= 0)
				npc.checkDead();
		}

		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (UniqueItems.SoulKey.JustPressed && SoulEffect && SoulCharge > 0)
			{
				var hit = false;
				var cen = Player.Center;
				var SoulEffectRange = (8 + (Player.statDefense / 8))*16;
				for (var i = 0; i < Main.maxNPCs; i++)
				{
					var npc = Main.npc[i];
					if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, cen) <= SoulEffectRange)
					{
						HitNPC(npc);
						hit = true;
					}
				}
				if (hit)
				{
					SoulCharge = 0;
					Ui.SetText(SoulCharge.ToString());
				}
			}
		}

		public override void UpdateEquips()
		{
			if (!SoulEffect)
			{
				SoulCharge = 0;
				SoulChargeBar.Visible = false;
			}
			else
			{
				SoulChargeBar.Visible = true;
			}
		}

		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
		{
			if (ManaShield)
			{
				var redir = damage / 2;
				var cache = Player.statMana;
				Player.statMana = redir <= Player.statMana ? (Player.statMana - redir) : 0;
				redir -= redir - cache < 0 ? 0 : redir - cache;
				damage -= redir;
			}

			return true;
		}

		private void AddSoulCharge(int amount)
		{
			if (SoulEffect && SoulCharge < (1000 + (40 * Player.statDefense)))
			{
				SoulCharge += amount * 2;
				Ui.SetText(SoulCharge.ToString());
			}

			SoulCharge = SoulCharge < (1000 + (40 * Player.statDefense)) ? SoulCharge : (1000 + (40 * Player.statDefense));
		}

		public override void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			AddSoulCharge(npc.damage);
		}

		public override void OnHitByProjectile(Projectile proj, int damage, bool crit)
		{
			AddSoulCharge(proj.damage);
		}

		private void ManaVampirismEffect(int damage, int life)
		{
			if (ManaVampirism && damage > life && Player.statMana < Player.statManaMax2)
			{
				var amount = damage - life;
				var max = Player.statManaMax2 * 0.2;
				var total = amount <= max ? amount : (int)max;
				Player.statMana += total;
				CombatText.NewText(new Rectangle((int)Player.position.X, (int)Player.position.Y, Player.width, Player.height), CombatText.HealMana, total, false);
			}
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			ManaVampirismEffect(proj.damage, target.life);
		}

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			ManaVampirismEffect(item.damage, target.life);
		}
	}
}
