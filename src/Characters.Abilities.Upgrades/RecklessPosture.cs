using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class RecklessPosture : Ability
{
	public sealed class Instance : AbilityInstance<RecklessPosture>
	{
		private EffectPoolInstance _loopEffect;

		private Stat.Values _stat;

		private bool _buffAttached;

		private float _remainBuffTime;

		public Instance(Character owner, RecklessPosture ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_stat = ability._baseStat.Clone();
			owner.stat.AttachValues(_stat);
			owner.health.onTookDamage += HandleOnTookDamage;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainBuffTime -= deltaTime;
			if (_buffAttached && _remainBuffTime <= 0f)
			{
				ResetStat();
			}
		}

		private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (ability._attackTypeFilter[tookDamage.attackType])
			{
				MultiplyStat();
			}
		}

		private void ResetStat()
		{
			_buffAttached = false;
			if ((Object)(object)_loopEffect != (Object)null)
			{
				_loopEffect.Stop();
				_loopEffect = null;
			}
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = ability._baseStat.values[i].value;
			}
			owner.stat.SetNeedUpdate();
		}

		private void MultiplyStat()
		{
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			_remainBuffTime = ability._buffDuration;
			_buffAttached = true;
			_loopEffect = ((ability._buffLoopEffect == null) ? null : ability._buffLoopEffect.Spawn(((Component)owner).transform.position, owner));
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = ability._baseStat.values[i].GetStackedValue(ability._multiplierWhenHit);
			}
			owner.stat.SetNeedUpdate();
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._buffAttachAudioClipInfo, ((Component)owner).transform.position);
		}

		protected override void OnDetach()
		{
			owner.health.onTookDamage -= HandleOnTookDamage;
			owner.stat.DetachValues(_stat);
		}
	}

	[SerializeField]
	private AttackTypeBoolArray _attackTypeFilter;

	[SerializeField]
	private EffectInfo _buffLoopEffect;

	[SerializeField]
	private SoundInfo _buffAttachAudioClipInfo;

	[SerializeField]
	private float _buffDuration;

	[SerializeField]
	private Stat.Values _baseStat;

	[SerializeField]
	private int _multiplierWhenHit;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
