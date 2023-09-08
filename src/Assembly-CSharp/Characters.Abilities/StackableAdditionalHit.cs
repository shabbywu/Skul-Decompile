using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities;

[Serializable]
public class StackableAdditionalHit : Ability
{
	public class Instance : AbilityInstance<StackableAdditionalHit>
	{
		private float _remainCooldownTime;

		private int _remainCount;

		private Dictionary<Character, int> _targetStacks;

		internal Instance(Character owner, StackableAdditionalHit ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_targetStacks = new Dictionary<Character, int>(128);
			_remainCount = ability._applyCount;
			if (ability._applyCount == 0)
			{
				_remainCount = int.MaxValue;
			}
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
			Singleton<Service>.Instance.levelManager.onMapLoaded += HandleOnMapLoaded;
		}

		private void HandleOnMapLoaded()
		{
			_targetStacks.Clear();
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
			Singleton<Service>.Instance.levelManager.onMapLoaded -= HandleOnMapLoaded;
		}

		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)target.character == (Object)null || _remainCooldownTime > 0f || target.character.health.dead || !((Component)target.transform).gameObject.activeSelf || (ability._needCritical && !tookDamage.critical) || !ability._attackTypes[tookDamage.motionType] || !ability._damageTypes[tookDamage.attackType])
			{
				return;
			}
			if (_targetStacks.ContainsKey(target.character))
			{
				int value = Mathf.Min(ability._maxStack, _targetStacks[target.character] + 1);
				_targetStacks.Clear();
				_targetStacks.Add(target.character, value);
			}
			else
			{
				_targetStacks.Clear();
				_targetStacks.Add(target.character, 0);
			}
			if ((Object)(object)ability._targetPoint != (Object)null)
			{
				Bounds bounds = target.collider.bounds;
				Vector3 center = ((Bounds)(ref bounds)).center;
				bounds = target.collider.bounds;
				Vector3 size = ((Bounds)(ref bounds)).size;
				size.x *= ability._positionInfo.pivotValue.x;
				size.y *= ability._positionInfo.pivotValue.y;
				Vector3 position = center + size;
				ability._targetPoint.position = position;
			}
			if (ability._adaptiveForce)
			{
				ability._additionalHit.ChangeAdaptiveDamageAttribute(owner);
			}
			float additionalDamageAmount = ability._additionalDamageAmount;
			Damage damage = owner.stat.GetDamage(additionalDamageAmount, MMMaths.RandomPointWithinBounds(target.collider.bounds), ability._additionalHit);
			damage.multiplier += (float)_targetStacks[target.character] * ability._damageMultiplierPerStack;
			int stack = _targetStacks[target.character];
			HitEffectByStack[] hitEffectByStacks = ability._hitEffectByStacks;
			foreach (HitEffectByStack hitEffectByStack in hitEffectByStacks)
			{
				if (hitEffectByStack.Contains(stack))
				{
					hitEffectByStack.Spawn(target.character);
				}
			}
			((MonoBehaviour)owner).StartCoroutine(ability._targetOperationInfo.CRun(owner, target.character));
			owner.Attack(target, ref damage);
			_remainCooldownTime = ability._cooldownTime;
			_remainCount--;
			if (_remainCount == 0)
			{
				owner.ability.Remove(this);
			}
		}
	}

	[Serializable]
	private class HitEffectByStack
	{
		[SerializeField]
		private Vector2 _stackRange;

		[SerializeField]
		private EffectInfo _effectInfo;

		[SerializeField]
		private SoundInfo _attackSoundInfo;

		public bool Contains(int stack)
		{
			if ((float)stack >= _stackRange.x)
			{
				return (float)stack <= _stackRange.y;
			}
			return false;
		}

		public void Spawn(Character target)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			PersistentSingleton<SoundManager>.Instance.PlaySound(_attackSoundInfo, ((Component)target).transform.position);
			_effectInfo.Spawn(Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(((Collider2D)target.collider).bounds)));
		}

		public void Dispose()
		{
			if (_effectInfo != null)
			{
				_effectInfo.Dispose();
				_effectInfo = null;
			}
			if (_attackSoundInfo != null)
			{
				_attackSoundInfo.Dispose();
				_attackSoundInfo = null;
			}
		}
	}

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private int _applyCount;

	[SerializeField]
	private float _damageMultiplierPerStack;

	[SerializeField]
	private float _additionalDamageAmount;

	[SerializeField]
	private bool _adaptiveForce;

	[SerializeField]
	private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private bool _needCritical;

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	[SerializeField]
	private PositionInfo _positionInfo;

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _targetOperationInfo;

	[SerializeField]
	private HitEffectByStack[] _hitEffectByStacks;

	~StackableAdditionalHit()
	{
		if (_hitEffectByStacks == null)
		{
			return;
		}
		int i = 0;
		for (; i < _hitEffectByStacks.Length; i++)
		{
			if (_hitEffectByStacks[i] != null)
			{
				_hitEffectByStacks[i].Dispose();
				_hitEffectByStacks[i] = null;
			}
		}
		_hitEffectByStacks = null;
	}

	public override void Initialize()
	{
		base.Initialize();
		_targetOperationInfo.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
