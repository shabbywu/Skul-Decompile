using System;
using System.Linq;
using FX;
using GameResources;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Enemies;

[Serializable]
public class CurseOfLight : Ability
{
	public class Instance : AbilityInstance<CurseOfLight>
	{
		private static float increasement = 0.1f;

		private static readonly Stat.Values _statPerStack = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 1f + increasement));

		private readonly EffectInfo _effectInfo;

		private readonly SoundInfo _soundInfo;

		private const int _phase = 3;

		private int _stacks;

		private Stat.Values _stat;

		private string floatingText => Localization.GetLocalizedString("floating/curseoflight");

		public override int iconStacks => _stacks;

		public override Sprite icon => SavableAbilityResource.instance.curseIcon;

		public Instance(Character owner, CurseOfLight ability)
			: base(owner, ability)
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			EffectInfo obj = new EffectInfo(SavableAbilityResource.instance.curseAttachEffect)
			{
				attachInfo = new EffectInfo.AttachInfo(attach: true, layerOnly: false, 1, EffectInfo.AttachInfo.Pivot.Bottom),
				trackChildren = false
			};
			SortingLayer val = SortingLayer.layers.Last();
			obj.sortingLayerId = ((SortingLayer)(ref val)).id;
			_effectInfo = obj;
			_soundInfo = new SoundInfo(SavableAbilityResource.instance.curseAttachSound);
		}

		protected override void OnAttach()
		{
			_stat = _statPerStack.Clone();
			_stacks = 1;
			SpawnEffects();
		}

		protected override void OnDetach()
		{
			Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack(0f);
			owner.stat.DetachValues(_stat);
		}

		public override void Refresh()
		{
			base.Refresh();
			_stacks++;
			if (_stacks == 3)
			{
				AttachStatBonus();
			}
			else if (_stacks % 3 == 0)
			{
				UpdateStack();
			}
			SpawnEffects();
		}

		private void AttachStatBonus()
		{
			Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack(_stacks);
			owner.stat.AttachValues(_stat);
			SpawnBuffText();
		}

		private void UpdateStack()
		{
			Scene<GameBase>.instance.uiManager.curseOfLightVignette.UpdateStack(_stacks);
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)_stat).values[i].value + (double)increasement;
			}
			owner.stat.SetNeedUpdate();
			SpawnBuffText();
		}

		private void SpawnEffects()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			PersistentSingleton<SoundManager>.Instance.PlaySound(_soundInfo, ((Component)owner).transform.position);
			_effectInfo.Spawn(((Component)owner).transform.position, owner);
		}

		private void SpawnBuffText()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			float num = (float)(_stacks / 3) * increasement * 100f;
			string text = string.Format(floatingText, num);
			Bounds bounds = ((Collider2D)owner.collider).bounds;
			Vector3 center = ((Bounds)(ref bounds)).center;
			Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(text, center);
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
