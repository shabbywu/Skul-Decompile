using System;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class GreatHerosArmor : Ability
{
	[Serializable]
	public sealed class Debuff : Ability
	{
		public sealed class Instance : AbilityInstance<Debuff>
		{
			private double _stackedDamage;

			private float _beforeSignTime;

			public Instance(Character owner, Debuff ability)
				: base(owner, ability)
			{
			}

			protected override void OnAttach()
			{
				_stackedDamage = 0.0;
				_beforeSignTime = 0f;
				owner.health.onTookDamage += HandleOnTookDamage;
			}

			protected override void OnDetach()
			{
				owner.health.onTookDamage -= HandleOnTookDamage;
			}

			public override void UpdateTime(float deltaTime)
			{
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_007a: Unknown result type (might be due to invalid IL or missing references)
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				base.UpdateTime(deltaTime);
				if (!owner.status.stuned)
				{
					Attack();
				}
				if (owner.status.stun.remainTime < ability._signTime && !(owner.status.stun.remainTime < _beforeSignTime))
				{
					Transform targetPoint = ability._targetPoint;
					Bounds bounds = ((Collider2D)owner.collider).bounds;
					targetPoint.position = ((Bounds)(ref bounds)).center;
					ability._signOperations.Run(owner);
					_beforeSignTime = owner.status.stun.remainTime;
				}
			}

			private void HandleOnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
			{
				if (((EnumArray<Damage.AttackType, bool>)ability._stackDamageType)[tookDamage.attackType])
				{
					_stackedDamage += damageDealt;
				}
			}

			private void Attack()
			{
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Unknown result type (might be due to invalid IL or missing references)
				ability._component.amount = Mathf.Min((float)(_stackedDamage * ability._damageConversionRatio), ability._maxBaseDamage.value);
				Transform targetPoint = ability._targetPoint;
				Bounds bounds = ((Collider2D)owner.collider).bounds;
				targetPoint.position = ((Bounds)(ref bounds)).center;
				Character player = Singleton<Service>.Instance.levelManager.player;
				ability._operations.Run(player);
				owner.ability.Remove(this);
			}

			private void HandleTargetOnReleaseStun(Character attacker, Character target)
			{
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
				//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
				double num = _stackedDamage * ability._damageConversionRatio;
				Damage damage = new Damage(attacker, (double)ability._baseDamage.value * num, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Item, 1.0, 0f, 0.0, 1.0, 1.0, canCritical: false, @null: false, 0.0, 0.0, 0);
				Transform targetPoint = ability._targetPoint;
				Bounds bounds = ((Collider2D)owner.collider).bounds;
				targetPoint.position = ((Bounds)(ref bounds)).center;
				attacker.Attack(owner, ref damage);
				ability._operations.Run(owner);
				owner.ability.Remove(this);
			}
		}

		[SerializeField]
		private GreatHerosArmorComponent _component;

		[SerializeField]
		private AttackTypeBoolArray _stackDamageType;

		[SerializeField]
		private float _signTime;

		[SerializeField]
		private CustomFloat _baseDamage;

		[SerializeField]
		private double _damageConversionRatio;

		[SerializeField]
		private CustomFloat _maxBaseDamage;

		[SerializeField]
		[CharacterOperation.Subcomponent]
		private CharacterOperation.Subcomponents _signOperations;

		[SerializeField]
		[CharacterOperation.Subcomponent]
		private CharacterOperation.Subcomponents _operations;

		[SerializeField]
		private Transform _targetPoint;

		public override void Initialize()
		{
			base.Initialize();
			_operations.Initialize();
			_signOperations.Initialize();
		}

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	public class Instance : AbilityInstance<GreatHerosArmor>
	{
		public Instance(Character owner, GreatHerosArmor ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.status.onApplyStun += HandleOnApplyStun;
		}

		private void HandleOnApplyStun(Character attacker, Character target)
		{
			target.ability.Add(ability._debuff);
		}

		protected override void OnDetach()
		{
			owner.status.onApplyStun -= HandleOnApplyStun;
		}
	}

	[SerializeField]
	[Header("스턴 대상")]
	private Debuff _debuff;

	public override void Initialize()
	{
		_debuff.Initialize();
		base.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
