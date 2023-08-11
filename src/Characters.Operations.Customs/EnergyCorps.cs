using System;
using System.Collections;
using Characters.AI;
using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;

namespace Characters.Operations.Customs;

public class EnergyCorps : CharacterOperation
{
	[Serializable]
	private class FireEnergyCrops
	{
		internal enum DirectionType
		{
			RotationOfFirePosition,
			OwnerDirection,
			Constant
		}

		[SerializeField]
		internal Projectile projectile;

		[SerializeField]
		internal bool group;

		[SerializeField]
		internal bool platformTarget;

		[SerializeField]
		internal DirectionType directionType;

		[SerializeField]
		internal Reorderable directions;
	}

	[SerializeField]
	private float _interval;

	[SerializeField]
	private AIController _controller;

	[SerializeField]
	private Transform _energyCorpsContainer;

	[SerializeField]
	[Header("Fire Projectile")]
	private FireEnergyCrops _fireEnergyCrops;

	private HitHistoryManager _hitHistoryManager;

	private IAttackDamage _attackDamage;

	private Coroutine _cReference;

	public override void Initialize()
	{
		base.Initialize();
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
	}

	public override void Run(Character owner)
	{
		_hitHistoryManager = (_fireEnergyCrops.group ? new HitHistoryManager(15) : null);
		_cReference = ((MonoBehaviour)this).StartCoroutine(CRun(owner));
	}

	private IEnumerator CRun(Character owner)
	{
		Character target = _controller.target;
		foreach (Transform item in _energyCorpsContainer)
		{
			Transform val = item;
			Bounds bounds;
			float num;
			if (_fireEnergyCrops.platformTarget)
			{
				bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
				num = ((Bounds)(ref bounds)).max.y;
			}
			else
			{
				float y = ((Component)target).transform.position.y;
				bounds = ((Collider2D)target.collider).bounds;
				num = y + ((Bounds)(ref bounds)).extents.y;
			}
			Vector3 val2 = new Vector3(((Component)target).transform.position.x, num) - ((Component)val).transform.position;
			float num2 = Mathf.Atan2(val2.y, val2.x) * 57.29578f;
			val.rotation = Quaternion.Euler(0f, 0f, num2);
			((Component)val).gameObject.SetActive(false);
			FireProjectile(owner, val);
			yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.master, _interval);
		}
	}

	private void FireProjectile(Character owner, Transform firePosition)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		CustomAngle[] values = ((ReorderableArray<CustomAngle>)(object)_fireEnergyCrops.directions).values;
		if (_fireEnergyCrops.directionType == FireEnergyCrops.DirectionType.RotationOfFirePosition)
		{
			for (int i = 0; i < values.Length; i++)
			{
				Projectile component = ((Component)_fireEnergyCrops.projectile.reusable.Spawn(firePosition.position, true)).GetComponent<Projectile>();
				float amount = _attackDamage.amount;
				Quaternion localRotation = firePosition.localRotation;
				component.Fire(owner, amount, ((Quaternion)(ref localRotation)).eulerAngles.z + values[i].value, firePosition.lossyScale.x < 0f);
			}
		}
		else if (_fireEnergyCrops.directionType == FireEnergyCrops.DirectionType.OwnerDirection)
		{
			for (int j = 0; j < values.Length; j++)
			{
				((Component)_fireEnergyCrops.projectile.reusable.Spawn(firePosition.position, true)).GetComponent<Projectile>().Fire(owner, _attackDamage.amount, values[j].value, owner.lookingDirection == Character.LookingDirection.Left);
			}
		}
		else
		{
			for (int k = 0; k < values.Length; k++)
			{
				((Component)_fireEnergyCrops.projectile.reusable.Spawn(firePosition.position, true)).GetComponent<Projectile>().Fire(owner, _attackDamage.amount, values[k].value, flipX: false, flipY: false, 1f, _fireEnergyCrops.group ? _hitHistoryManager : null);
			}
		}
	}

	public override void Stop()
	{
		base.Stop();
		if (_cReference != null)
		{
			((MonoBehaviour)this).StopCoroutine(_cReference);
		}
	}
}
