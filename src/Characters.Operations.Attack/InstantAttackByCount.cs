using System;
using System.Collections.Generic;
using FX.BoundsAttackVisualEffect;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public sealed class InstantAttackByCount : CharacterOperation
{
	[SerializeField]
	private bool _adaptiveForce;

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

	[SerializeField]
	private Collider2D _attackRange;

	[Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _operationInfo;

	[BoundsAttackVisualEffect.Subcomponent]
	[SerializeField]
	private BoundsAttackVisualEffect.Subcomponents _effect;

	[SerializeField]
	[Tooltip("한 번에 공격 가능한 적의 수(프롭 포함), 특별한 경우가 아니면 기본값인 512로 두는 게 좋음.")]
	private int _maxHits = 512;

	[SerializeField]
	private int _damageMultiplierMaxCount;

	[SerializeField]
	private int _baseDamageMultiplierMaxCount;

	[SerializeField]
	private Curve _multiplierCurve;

	private NonAllocOverlapper _overlapper;

	private IAttackDamage _attackDamage;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(_maxHits);
		((Behaviour)_attackRange).enabled = false;
		Array.Sort(((SubcomponentArray<TargetedOperationInfo>)_operationInfo).components, (TargetedOperationInfo x, TargetedOperationInfo y) => x.timeToTrigger.CompareTo(y.timeToTrigger));
	}

	public override void Initialize()
	{
		base.Initialize();
		_operationInfo.Initialize();
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
	}

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_attackRange).enabled = true;
		Bounds bounds = _attackRange.bounds;
		_overlapper.OverlapCollider(_attackRange);
		((Behaviour)_attackRange).enabled = false;
		List<Target> components = GetComponentExtension.GetComponents<Collider2D, Target>((IEnumerable<Collider2D>)_overlapper.results, true);
		if (components.Count == 0)
		{
			return;
		}
		if (_adaptiveForce)
		{
			_hitInfo.ChangeAdaptiveDamageAttribute(owner);
		}
		for (int i = 0; i < components.Count; i++)
		{
			Target target = components[i];
			if (!((Object)(object)target == (Object)null) && !((Object)(object)target.character == (Object)null) && !((Object)(object)target.character == (Object)(object)owner) && target.character.liveAndActive)
			{
				Bounds bounds2 = target.collider.bounds;
				Bounds val = default(Bounds);
				((Bounds)(ref val)).min = Vector2.op_Implicit(MMMaths.Max(Vector2.op_Implicit(((Bounds)(ref bounds)).min), Vector2.op_Implicit(((Bounds)(ref bounds2)).min)));
				((Bounds)(ref val)).max = Vector2.op_Implicit(MMMaths.Min(Vector2.op_Implicit(((Bounds)(ref bounds)).max), Vector2.op_Implicit(((Bounds)(ref bounds2)).max)));
				Vector2 hitPoint = MMMaths.RandomPointWithinBounds(val);
				Damage damage = owner.stat.GetDamage(_attackDamage.amount, hitPoint, _hitInfo);
				if (_baseDamageMultiplierMaxCount != 0)
				{
					damage.percentMultiplier *= 1f + _multiplierCurve.Evaluate((float)components.Count / (float)_baseDamageMultiplierMaxCount);
				}
				if (_damageMultiplierMaxCount != 0)
				{
					damage.multiplier += _multiplierCurve.Evaluate((float)components.Count / (float)_damageMultiplierMaxCount);
				}
				((MonoBehaviour)this).StartCoroutine(_operationInfo.CRun(owner, target.character));
				_effect.Spawn(owner, val, in damage, target);
				if (!target.character.cinematic.value)
				{
					owner.TryAttackCharacter(target, ref damage);
				}
			}
		}
	}
}
