using System;
using Characters.Abilities.Weapons.DavyJones;
using Characters.Operations.Attack;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Customs.DavyJones;

public sealed class FireCannonBall : CharacterOperation
{
	[Serializable]
	private class ProjectileSelector
	{
		[Serializable]
		private struct ProjectileByCannonBallType
		{
			[SerializeField]
			internal CannonBallType type;

			[SerializeField]
			internal Projectile projectilePrefab;
		}

		[SerializeField]
		private ProjectileByCannonBallType[] _projectiles;

		public Projectile GetProjectile(DavyJonesPassiveComponent passive)
		{
			ProjectileByCannonBallType[] projectiles = _projectiles;
			for (int i = 0; i < projectiles.Length; i++)
			{
				ProjectileByCannonBallType projectileByCannonBallType = projectiles[i];
				if (passive.IsTop(projectileByCannonBallType.type))
				{
					return projectileByCannonBallType.projectilePrefab;
				}
			}
			return null;
		}
	}

	[SerializeField]
	private DavyJonesPassiveComponent _passive;

	[SerializeField]
	private ProjectileSelector _selector;

	[SerializeField]
	[Space]
	private CustomFloat _speedMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _damageMultiplier = new CustomFloat(1f);

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	[Space]
	private Transform _fireTransform;

	[SerializeField]
	private bool _group;

	[SerializeField]
	private bool _flipXByOwnerDirection;

	[SerializeField]
	private bool _flipYByOwnerDirection;

	[Space]
	[SerializeField]
	private FireProjectile.DirectionType _directionType;

	[SerializeField]
	private Reorderable _directions;

	private IAttackDamage _attackDamage;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		if ((Object)(object)_fireTransform == (Object)null)
		{
			_fireTransform = ((Component)this).transform;
		}
	}

	public override void Run(Character owner)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		CustomAngle[] values = ((ReorderableArray<CustomAngle>)(object)_directions).values;
		bool flipX = false;
		bool flipY = false;
		Projectile projectile = _selector.GetProjectile(_passive);
		_passive.Pop();
		HitHistoryManager hitHistoryManager = (_group ? new HitHistoryManager(15) : null);
		for (int i = 0; i < values.Length; i++)
		{
			float num;
			switch (_directionType)
			{
			case FireProjectile.DirectionType.RotationOfFirePosition:
			{
				Quaternion rotation = _fireTransform.rotation;
				num = ((Quaternion)(ref rotation)).eulerAngles.z + values[i].value;
				if (_fireTransform.lossyScale.x < 0f)
				{
					num = (180f - num) % 360f;
				}
				break;
			}
			case FireProjectile.DirectionType.OwnerDirection:
			{
				num = values[i].value;
				bool flag = owner.lookingDirection == Character.LookingDirection.Left || _fireTransform.lossyScale.x < 0f;
				flipX = _flipXByOwnerDirection && flag;
				flipY = _flipYByOwnerDirection && flag;
				num = (flag ? ((180f - num) % 360f) : num);
				break;
			}
			default:
				num = values[i].value;
				break;
			}
			Projectile component = ((Component)projectile.reusable.Spawn(_fireTransform.position, true)).GetComponent<Projectile>();
			((Component)component).transform.localScale = Vector3.one * _scale.value;
			component.Fire(owner, _attackDamage.amount * _damageMultiplier.value, num, flipX, flipY, _speedMultiplier.value, _group ? hitHistoryManager : null);
		}
	}
}
