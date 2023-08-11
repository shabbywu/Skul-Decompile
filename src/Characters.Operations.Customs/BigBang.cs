using System;
using Characters.Operations.Attack;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs;

public sealed class BigBang : CharacterOperation
{
	[SerializeField]
	[Header("Gain Energy")]
	private Transform _fireTransformContainer;

	[SerializeField]
	private float _radius = 20f;

	[SerializeField]
	[Subcomponent(typeof(MultipleFireProjectile))]
	private MultipleFireProjectile _multipleFireProjectile;

	public override void Initialize()
	{
		_multipleFireProjectile.Initialize();
	}

	public override void Run(Character owner)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		MakeFireTransfom();
		if (owner.lookingDirection == Character.LookingDirection.Left)
		{
			_fireTransformContainer.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			_fireTransformContainer.localScale = new Vector3(1f, 1f, 1f);
		}
		_multipleFireProjectile.Run(owner);
	}

	private void MakeFireTransfom()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		float num = 360 / _fireTransformContainer.childCount;
		float num2 = Random.Range(0f, num);
		for (int i = 0; i < _fireTransformContainer.childCount; i++)
		{
			Transform child = _fireTransformContainer.GetChild(i);
			((Component)child).transform.localPosition = Vector2.op_Implicit(new Vector2(Mathf.Cos(num2 * ((float)Math.PI / 180f)), Mathf.Sin(num2 * ((float)Math.PI / 180f))) * _radius);
			((Component)child).transform.localRotation = Quaternion.Euler(0f, 0f, 180f + num2);
			num2 += num;
		}
	}
}
