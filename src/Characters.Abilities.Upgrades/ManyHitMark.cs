using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class ManyHitMark : Ability
{
	[Serializable]
	public sealed class ManyHitMarkTarget : Ability
	{
		public sealed class Instance : AbilityInstance<ManyHitMarkTarget>
		{
			internal class StackInfo
			{
				internal EffectPoolInstance stackEffectInstance;

				internal float rotationZ;
			}

			private int _stack;

			private StackInfo[] _stackInfos;

			public Instance(Character owner, ManyHitMarkTarget ability)
				: base(owner, ability)
			{
				_stackInfos = new StackInfo[3];
			}

			protected override void OnAttach()
			{
				_stack = -1;
				AddStack();
			}

			public override void Refresh()
			{
				base.Refresh();
				AddStack();
			}

			protected override void OnDetach()
			{
				for (int i = 0; i < _stackInfos.Length; i++)
				{
					StackInfo stackInfo = _stackInfos[i];
					if (stackInfo != null && (Object)(object)stackInfo.stackEffectInstance != (Object)null)
					{
						stackInfo.stackEffectInstance.Stop();
						stackInfo.stackEffectInstance = null;
					}
					_stackInfos[i] = null;
				}
				_stackInfos = null;
			}

			private void AddStack()
			{
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_003c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0059: Unknown result type (might be due to invalid IL or missing references)
				//IL_005e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0063: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Unknown result type (might be due to invalid IL or missing references)
				_stack++;
				Bounds bounds = ((Collider2D)owner.collider).bounds;
				Vector2 val = Vector2.op_Implicit(((Bounds)(ref bounds)).center);
				Vector2 insideUnitCircle = Random.insideUnitCircle;
				bounds = ((Collider2D)owner.collider).bounds;
				Vector2 val2 = val + insideUnitCircle * Vector2.op_Implicit(((Bounds)(ref bounds)).extents) * 0.7f;
				int num = Random.Range(0, 360);
				EffectPoolInstance effectPoolInstance = ability._stackEffect.Spawn(Vector2.op_Implicit(val2), owner, num);
				((Component)effectPoolInstance).transform.SetParent(owner.attachWithFlip.transform);
				StackInfo stackInfo = new StackInfo
				{
					stackEffectInstance = effectPoolInstance,
					rotationZ = num
				};
				_stackInfos[_stack] = stackInfo;
				if (_stack >= ability._maxStack - 1)
				{
					_stack = 0;
					Hit();
				}
			}

			private void Hit()
			{
				//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
				//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
				//IL_0034: Unknown result type (might be due to invalid IL or missing references)
				for (int i = 0; i < _stackInfos.Length; i++)
				{
					StackInfo stackInfo = _stackInfos[i];
					if (stackInfo != null && (Object)(object)stackInfo.stackEffectInstance != (Object)null)
					{
						ability._hitEffect.Spawn(((Component)stackInfo.stackEffectInstance).transform.position, owner, stackInfo.rotationZ);
						stackInfo.stackEffectInstance.Stop();
						stackInfo.stackEffectInstance = null;
					}
				}
				ability._manyHitMark.additionHitInfo.ChangeAdaptiveDamageAttribute(ability._attacker);
				Damage damage = ability._attacker.stat.GetDamage(ability._manyHitMark.additionalDamageAmount, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), ability._manyHitMark.additionHitInfo);
				PersistentSingleton<SoundManager>.Instance.PlaySound(ability._manyHitMark.attackSoundInfo, ((Component)owner).transform.position);
				((MonoBehaviour)ability._attacker).StartCoroutine(ability._manyHitMark._targetOperationInfo.CRun(ability._attacker, owner));
				ability._attacker.Attack(owner, ref damage);
				owner.ability.Remove(this);
			}
		}

		[SerializeField]
		private int _maxStack;

		[SerializeField]
		private EffectInfo _stackEffect;

		[SerializeField]
		private EffectInfo _hitEffect;

		private Character _attacker;

		private ManyHitMark _manyHitMark;

		public void SetAttacker(Character attacker, ManyHitMark manyHitMark)
		{
			_attacker = attacker;
			_manyHitMark = manyHitMark;
		}

		public override IAbilityInstance CreateInstance(Character owner)
		{
			return new Instance(owner, this);
		}
	}

	public sealed class Instance : AbilityInstance<ManyHitMark>
	{
		private List<Character> _targets;

		private List<float> _times;

		public Instance(Character owner, ManyHitMark ability)
			: base(owner, ability)
		{
			_targets = new List<Character>(128);
			_times = new List<float>(128);
		}

		protected override void OnAttach()
		{
			ability._manyHitMarkTarget.SetAttacker(owner, ability);
			owner.onGiveDamage.Add(int.MinValue, HandleOnGiveDamage);
			Singleton<Service>.Instance.levelManager.onMapLoaded += HandleOnMapLoaded;
		}

		private void HandleOnMapLoaded()
		{
			_targets.Clear();
			_targets.Clear();
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			if (damage.@null)
			{
				return false;
			}
			if (damage.amount == 0.0)
			{
				return false;
			}
			Character character = target.character;
			if ((Object)(object)character == (Object)null)
			{
				return false;
			}
			if (!ability._attackType[damage.attackType])
			{
				return false;
			}
			if (!ability._motionType[damage.motionType])
			{
				return false;
			}
			if (!ability._targetType[character.type])
			{
				return false;
			}
			int num = _targets.IndexOf(character);
			if (num == -1)
			{
				_targets.Add(character);
				_times.Add(Time.time);
			}
			else
			{
				if (Time.time - _times[num] < ability._cooldownTimePerTarget)
				{
					return false;
				}
				_times[num] = Time.time;
			}
			character.ability.Add(ability._manyHitMarkTarget);
			return false;
		}

		protected override void OnDetach()
		{
			_targets.Clear();
			_times.Clear();
			owner.onGiveDamage.Remove(HandleOnGiveDamage);
			Singleton<Service>.Instance.levelManager.onMapLoaded -= HandleOnMapLoaded;
		}
	}

	[Header("공격")]
	[SerializeField]
	private AttackTypeBoolArray _attackType;

	[SerializeField]
	private MotionTypeBoolArray _motionType;

	[SerializeField]
	private CharacterTypeBoolArray _targetType;

	[SerializeField]
	private float _additionalDamageAmount;

	[SerializeField]
	private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private PositionInfo _positionInfo;

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _targetOperationInfo;

	[SerializeField]
	private SoundInfo _attackSoundInfo;

	[Header("타겟 효과")]
	[SerializeField]
	private float _cooldownTimePerTarget;

	[SerializeField]
	private ManyHitMarkTarget _manyHitMarkTarget;

	public HitInfo additionHitInfo => _additionalHit;

	public float additionalDamageAmount => _additionalDamageAmount;

	public SoundInfo attackSoundInfo => _attackSoundInfo;

	public override void Initialize()
	{
		base.Initialize();
		_manyHitMarkTarget.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
