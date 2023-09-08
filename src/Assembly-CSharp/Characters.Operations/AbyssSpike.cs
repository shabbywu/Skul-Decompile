using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.Projectiles;
using FX;
using UnityEngine;

namespace Characters.Operations;

public class AbyssSpike : CharacterOperation
{
	public enum DirectionType
	{
		OwnerDirection,
		Constant
	}

	[SerializeField]
	private ChargeAction _chargeAction;

	[Tooltip("차지를 하나도 안 했을 때 프로젝타일 개수")]
	[SerializeField]
	[Space]
	private int _projectileCountMin = 1;

	[SerializeField]
	[Tooltip("풀차지 했을 때 프로젝타일 개수")]
	private int _projectileCountMax = 10;

	[SerializeField]
	[Space]
	[Tooltip("프로젝타일 발사 간격")]
	private CustomFloat _fireInterval;

	[SerializeField]
	[Space]
	private EffectInfo _spawnEffect;

	[SerializeField]
	private Projectile _incompleteProjectile;

	[SerializeField]
	private Projectile _completeProjectile;

	[SerializeField]
	private bool _flipXByOwnerDirection;

	[SerializeField]
	private bool _flipYByOwnerDirection;

	[SerializeField]
	private DirectionType _directionType;

	[SerializeField]
	private CustomAngle.Reorderable _directions = new CustomAngle.Reorderable(new CustomAngle(0f));

	private IAttackDamage _attackDamage;

	[SerializeField]
	private Collider2D _area;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
	}

	public override void Run(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CFire(owner));
	}

	private IEnumerator CFire(Character owner)
	{
		int count = (int)((float)(_projectileCountMax - _projectileCountMin) * _chargeAction.chargedPercent) + _projectileCountMin;
		Projectile projectile = ((_chargeAction.chargedPercent < 1f) ? _incompleteProjectile : _completeProjectile);
		Bounds bounds = _area.bounds;
		Character.LookingDirection lookingDirection = owner.lookingDirection;
		for (int i = 0; i < count; i++)
		{
			Fire(owner, projectile, bounds, lookingDirection);
			yield return owner.chronometer.animation.WaitForSeconds(_fireInterval.value);
		}
	}

	private void Fire(Character owner, Projectile projectile, Bounds bounds, Character.LookingDirection lookingDirection)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		CustomAngle[] values = _directions.values;
		List<Vector2> list = new List<Vector2>(values.Length);
		for (int i = 0; i < values.Length; i++)
		{
			list.Add(MMMaths.RandomPointWithinBounds(bounds));
		}
		if (_directionType == DirectionType.OwnerDirection)
		{
			for (int j = 0; j < values.Length; j++)
			{
				float value = values[j].value;
				if (_spawnEffect != null)
				{
					_spawnEffect.Spawn(Vector2.op_Implicit(list[j]), owner, value);
				}
				bool flag = lookingDirection == Character.LookingDirection.Left;
				bool flipX = _flipXByOwnerDirection && flag;
				bool flipY = _flipYByOwnerDirection && flag;
				value = (flag ? ((180f - value) % 360f) : value);
				((Component)projectile.reusable.Spawn(Vector2.op_Implicit(list[j]), true)).GetComponent<Projectile>().Fire(owner, _attackDamage.amount, value, flipX, flipY);
			}
			return;
		}
		for (int k = 0; k < values.Length; k++)
		{
			float value = values[k].value;
			if (_spawnEffect != null)
			{
				_spawnEffect.Spawn(Vector2.op_Implicit(list[k]), owner, value);
			}
			if (((Component)_area).transform.lossyScale.x < 0f)
			{
				value = (180f - value) % 360f;
			}
			((Component)projectile.reusable.Spawn(Vector2.op_Implicit(list[k]), true)).GetComponent<Projectile>().Fire(owner, _attackDamage.amount, value);
		}
	}
}
