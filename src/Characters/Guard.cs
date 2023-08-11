using System;
using System.Collections;
using Characters.Operations;
using FX;
using FX.SpriteEffects;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Characters;

public sealed class Guard : MonoBehaviour
{
	[Serializable]
	private class Poisition
	{
		[SerializeField]
		private Character.SizeForEffect _size;

		[SerializeField]
		private PositionInfo _positionInfo;

		public void TryAttach(Character owner, Transform target)
		{
			if (_size == owner.sizeForEffect)
			{
				_positionInfo.Attach(owner, target);
			}
		}
	}

	[SerializeField]
	private bool _frontOnly;

	[SerializeField]
	private bool _cancelActionOnBroken;

	[SerializeField]
	private bool _breakable;

	[Tooltip("breakable일 때 체력")]
	[SerializeField]
	private float _durability;

	[SerializeField]
	private float _lifeTime;

	[SerializeField]
	private float _chronoEffectUniqueTime = 0.2f;

	[SerializeField]
	private float _stoppingPowerOnBroken = 2f;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	[Space]
	private OperationInfos _onHitToOwner;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onHitToOwnerFromRangeAttack;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onHitToTarget;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onBreak;

	[Space]
	[SerializeField]
	private ChronoInfo _onHitToOwnerChronoInfo;

	[SerializeField]
	private ChronoInfo _onHitToTargetChronoInfo;

	[SerializeField]
	private SpriteEffectStack _spriteEffectStack;

	[SerializeField]
	private bool _attachPosition;

	[SerializeField]
	private Poisition[] _positionInfos;

	[SerializeField]
	private EnumArray<Character.SizeForEffect, float> _spriteSize;

	private Character _owner;

	private float _currentDurability;

	private float _lastHitTime;

	private CoroutineReference _cexpireReference;

	private bool _active;

	public bool active
	{
		get
		{
			return _active;
		}
		set
		{
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			_active = value;
			if (_attachPosition && (Object)(object)_owner != (Object)null)
			{
				Poisition[] positionInfos = _positionInfos;
				for (int i = 0; i < positionInfos.Length; i++)
				{
					positionInfos[i].TryAttach(_owner, ((Component)this).transform);
				}
			}
			if ((Object)(object)_spriteEffectStack != (Object)null && (Object)(object)_spriteEffectStack.mainRenderer != (Object)null && (Object)(object)_owner != (Object)null)
			{
				float num = _spriteSize[_owner.sizeForEffect];
				((Component)_spriteEffectStack.mainRenderer).transform.localScale = Vector2.op_Implicit(Vector2.one * num);
			}
			((Component)this).gameObject.SetActive(_active);
		}
	}

	public float currentDurability
	{
		get
		{
			return _currentDurability;
		}
		set
		{
			_currentDurability = value;
		}
	}

	public float durability
	{
		get
		{
			return _durability;
		}
		set
		{
			_durability = value;
		}
	}

	public static Guard Create(AssetReference reference)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Object.Instantiate<Guard>(Addressables.LoadAssetAsync<GameObject>((object)reference).WaitForCompletion().GetComponent<Guard>());
	}

	public void Initialize(Character owner)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		_owner = owner;
		((Component)this).transform.SetParent(owner.attachWithFlip.transform);
		((Component)this).transform.localScale = new Vector3(1f, 1f, 1f);
	}

	public void GuardUp()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		_currentDurability = _durability;
		_owner.health.onTakeDamage.Add(int.MinValue, (TakeDamageDelegate)Block);
		active = true;
		if (_lifeTime > 0f)
		{
			_cexpireReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CExpire());
		}
	}

	public void GuardDown()
	{
		((CoroutineReference)(ref _cexpireReference)).Stop();
		if ((Object)(object)_owner != (Object)null)
		{
			_owner.health.onTakeDamage.Remove((TakeDamageDelegate)Block);
		}
		active = false;
	}

	private void BreakGuard(Damage damage)
	{
		damage.stoppingPower = _stoppingPowerOnBroken;
		_onBreak.Run(_owner);
		if (_cancelActionOnBroken)
		{
			_owner.CancelAction();
		}
		GuardDown();
	}

	private IEnumerator CExpire()
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)_owner.chronometer.master, _lifeTime);
		GuardDown();
	}

	private bool Block(ref Damage damage)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		Attacker attacker = damage.attacker;
		if (damage.attackType == Damage.AttackType.Additional)
		{
			return false;
		}
		Vector3 position = ((Component)this).transform.position;
		if (IsBlocked(Vector2.op_Implicit(position), damage))
		{
			_currentDurability -= (float)damage.amount;
			if (_breakable && _currentDurability <= 0f)
			{
				BreakGuard(damage);
				return true;
			}
			damage.stoppingPower = 0f;
			if (damage.attackType == Damage.AttackType.Melee)
			{
				if (Time.time - _lastHitTime < _chronoEffectUniqueTime)
				{
					return true;
				}
				_lastHitTime = Time.time;
				_onHitToOwnerChronoInfo.ApplyGlobe();
				((Component)_onHitToOwner).gameObject.SetActive(true);
				_onHitToOwner.Run(_owner);
				((Component)_onHitToTarget).gameObject.SetActive(true);
				_onHitToTarget.Run(attacker.character);
				if ((Object)(object)_spriteEffectStack != (Object)null)
				{
					_spriteEffectStack.Add(new GuardHit());
				}
			}
			else if (damage.attackType == Damage.AttackType.Ranged || damage.attackType == Damage.AttackType.Projectile)
			{
				((Component)_onHitToOwnerFromRangeAttack).gameObject.SetActive(true);
				_onHitToOwnerFromRangeAttack.Run(_owner);
			}
			return true;
		}
		return false;
	}

	private bool IsBlocked(Vector2 center, Damage damage)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = damage.attacker.transform.position;
		if (damage.attackType == Damage.AttackType.Projectile)
		{
			val = Vector2.op_Implicit(damage.hitPoint);
		}
		if (_frontOnly && (_owner.lookingDirection != 0 || !(center.x < val.x)))
		{
			if (_owner.lookingDirection == Character.LookingDirection.Left)
			{
				return center.x > val.x;
			}
			return false;
		}
		return true;
	}
}
