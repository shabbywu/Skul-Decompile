using System;
using FX;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class TimeBomb : Ability
{
	public sealed class Instance : AbilityInstance<TimeBomb>
	{
		private EffectPoolInstance _activeEffectInstance;

		private EffectPoolInstance _deactiveEffectInstance;

		public Instance(Character owner, TimeBomb ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			_activeEffectInstance = ability._activeEffect.Spawn(((Component)owner).transform.position, owner);
			_deactiveEffectInstance = ability._deactiveEffect.Spawn(((Component)owner).transform.position, owner);
		}

		protected override void OnDetach()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			_activeEffectInstance.renderer.color = new Color(255f, 255f, 255f, 255f);
			_activeEffectInstance.Stop();
			_deactiveEffectInstance.Stop();
			_activeEffectInstance = null;
			_deactiveEffectInstance = null;
		}

		public void UpdateEffect(float alpha)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)_activeEffectInstance == (Object)null))
			{
				Color color = _activeEffectInstance.renderer.color;
				if (alpha >= 1f)
				{
					color.a = 2f - alpha;
				}
				else
				{
					color.a = alpha;
				}
				_activeEffectInstance.renderer.color = color;
			}
		}

		public void Explode()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)_activeEffectInstance == (Object)null))
			{
				_activeEffectInstance.renderer.color = new Color(255f, 255f, 255f, 255f);
				_activeEffectInstance.Stop();
				if (!((Object)(object)_deactiveEffectInstance == (Object)null))
				{
					_deactiveEffectInstance.Stop();
					ability._giver.Attack(owner);
					base.remainTime = 0f;
				}
			}
		}
	}

	[SerializeField]
	private EffectInfo _activeEffect = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private EffectInfo _deactiveEffect = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private TimeBombGiverComponent _giver;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
