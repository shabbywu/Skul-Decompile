using System;
using Characters.Abilities;
using Characters.Operations;
using Characters.Utils;
using Services;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Relic : InscriptionInstance
{
	[Serializable]
	public sealed class RelicAbility : Ability
	{
		private sealed class Instance : AbilityInstance<RelicAbility>
		{
			private StackHistoryManager<Character> _historyManager;

			public Instance(Character owner, RelicAbility ability)
				: base(owner, ability)
			{
				_historyManager = new StackHistoryManager<Character>(128);
			}

			protected override void OnAttach()
			{
				Character character = owner;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
			}

			protected override void OnDetach()
			{
				_historyManager.ClearIfExpired();
				Character character = owner;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
			}

			private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
			{
				//IL_0103: Unknown result type (might be due to invalid IL or missing references)
				//IL_0108: Unknown result type (might be due to invalid IL or missing references)
				if (gaveDamage.attackType != Damage.AttackType.Additional)
				{
					Character character = target.character;
					_historyManager.ClearIfExpired();
					if (_historyManager.TryAddStack(character, 1, ability._maxHitDuringCooldown, ability._cooldownTime))
					{
						int num = Mathf.Min(ability._maxStack, (int)(owner.health.maximumHealth / (double)ability._healthPerStack));
						double num2 = character.health.currentHealth * (double)ability._damagePercentPerStack * 0.009999999776482582;
						double num3 = (double)num * num2;
						num3 = ((character.type != Character.Type.Boss && character.type != Character.Type.Adventurer) ? (num3 * (double)ability._damageMultiplierPerStackToNormal) : (num3 * (double)ability._damageMultiplierPerStackToBoss));
						Damage damage = owner.stat.GetDamage(Mathf.Clamp((float)num3, ability._damageRange.x, ability._damageRange.y), MMMaths.RandomPointWithinBounds(target.collider.bounds), ability._hitInfo);
						owner.TryAttackCharacter(target, ref damage);
						ability._targetPositionInfo.Attach(target.character, ability._targetPosition);
						((MonoBehaviour)owner).StartCoroutine(ability._onTarget.CRun(owner));
					}
				}
			}
		}

		[SerializeField]
		[Header("공격 설정")]
		private HitInfo _hitInfo;

		[SerializeField]
		private PositionInfo _targetPositionInfo;

		[SerializeField]
		private Transform _targetPosition;

		[SerializeField]
		[MinMaxSlider(0f, 9999999f)]
		private Vector2 _damageRange;

		[SerializeField]
		private int _healthPerStack;

		[SerializeField]
		private int _maxStack;

		[Range(0f, 100f)]
		[SerializeField]
		private float _damagePercentPerStack;

		[SerializeField]
		private float _damageMultiplierPerStackToNormal;

		[SerializeField]
		private float _damageMultiplierPerStackToBoss;

		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _onTarget;

		[Header("공격 대상 쿨타임 설정")]
		[SerializeField]
		private int _cooldownTime;

		[SerializeField]
		private int _maxHitDuringCooldown;

		public override void Initialize()
		{
			base.Initialize();
			_onTarget.Initialize();
		}

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	[SerializeField]
	[Header("2세트 효과")]
	private double _extraHealAmount;

	[SerializeField]
	[Header("4세트 효과")]
	private RelicAbility _ability;

	protected override void Initialize()
	{
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
	}

	public override void Attach()
	{
		base.character.health.onHealByGiver += HandleOnHealedByPotion;
		base.character.health.onHealed += OnHealed;
	}

	public override void Detach()
	{
		if (!Service.quitting)
		{
			base.character.health.onHealByGiver -= HandleOnHealedByPotion;
			base.character.health.onHealed -= OnHealed;
			base.character.ability.Remove(_ability);
		}
	}

	private void OnHealed(double healed, double overHealed)
	{
		if (!(healed < 1.0) && keyword.isMaxStep)
		{
			base.character.ability.Add(_ability);
		}
	}

	private void HandleOnHealedByPotion(ref Health.HealInfo info)
	{
		if (info.healthGiver == Health.HealthGiverType.Potion)
		{
			info.amount += _extraHealAmount;
		}
	}
}
