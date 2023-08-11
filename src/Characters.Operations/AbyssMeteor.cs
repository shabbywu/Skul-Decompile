using System;
using System.Collections;
using Characters.Actions;
using Characters.Operations.SetPosition;
using Characters.Projectiles;
using FX;
using UnityEngine;

namespace Characters.Operations;

public class AbyssMeteor : CharacterOperation
{
	public enum DirectionType
	{
		OwnerDirection,
		Constant
	}

	[SerializeField]
	private ChargeAction _chargeAction;

	[SerializeField]
	[Space]
	[Tooltip("차지를 하나도 안 했을 때 프로젝타일 개수")]
	private int _projectileCountMin = 1;

	[Tooltip("풀차지 했을 때 프로젝타일 개수")]
	[SerializeField]
	private int _projectileCountMax = 5;

	[SerializeField]
	[Space]
	[Tooltip("프로젝타일 발사 간격")]
	private CustomFloat _fireInterval;

	[SerializeField]
	[Space]
	private EffectInfo _spawnEffect;

	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private bool _flipXByOwnerDirection;

	[SerializeField]
	private bool _flipYByOwnerDirection;

	private IAttackDamage _attackDamage;

	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private float _distance;

	[SerializeField]
	private float _horizontalNoise;

	[Policy.Subcomponent(true)]
	[SerializeField]
	private Policy _policy;

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
		Character.LookingDirection lookingDirection = owner.lookingDirection;
		for (int i = 0; i < count; i++)
		{
			Fire(owner, lookingDirection);
			yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.animation, _fireInterval.value);
		}
	}

	private void Fire(Character owner, Character.LookingDirection lookingDirection)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.right * _distance;
		float num = (MMMaths.RandomBool() ? _angle.value : (180f - _angle.value));
		float num2 = num * ((float)Math.PI / 180f);
		float num3 = Mathf.Cos(num2);
		float num4 = Mathf.Sin(num2);
		Vector2 val2 = default(Vector2);
		((Vector2)(ref val2))._002Ector(val.x * num3 - val.y * num4, val.x * num4 + val.y * num3);
		float num5 = (MMMaths.RandomBool() ? Random.Range(0f, _horizontalNoise) : Random.Range(0f - _horizontalNoise, 0f));
		Vector2 position = _policy.GetPosition(owner);
		((Vector2)(ref position))._002Ector(position.x + num5, position.y);
		float num6 = num + 180f;
		Vector2 val3 = MMMaths.Vector3ToVector2(Vector2.op_Implicit(position)) + val2;
		if (_spawnEffect != null)
		{
			_spawnEffect.Spawn(Vector2.op_Implicit(val3), owner, num6);
		}
		((Component)_projectile.reusable.Spawn(Vector2.op_Implicit(val3), true)).GetComponent<Projectile>().Fire(owner, _attackDamage.amount, num6);
	}
}
