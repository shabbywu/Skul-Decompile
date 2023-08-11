using Characters.Operations;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Projectiles.Operations;

public sealed class SummonOperationRunnerOnHitPoint : HitOperation
{
	private static short spriteLayer = short.MinValue;

	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	private OperationRunner _operationRunner;

	[SerializeField]
	private CustomFloat _offsetX;

	[SerializeField]
	private CustomFloat _offsetY;

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private PositionNoise _noise;

	[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
	[SerializeField]
	private bool _flipXByLookingDirection;

	[SerializeField]
	[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
	private bool _attachToOwner;

	[Header("Interporlation")]
	[Tooltip("콜라이더 끝에 걸쳤을 때 자연스럽게 보이기 위해 위치 보간")]
	[SerializeField]
	private bool _interpolatedPosition;

	[SerializeField]
	private float _interpolatedSize;

	private void OnDestroy()
	{
		_operationRunner = null;
	}

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		Character owner = projectile.owner;
		Vector2 result = ((RaycastHit2D)(ref raycastHit)).point + Vector2.op_Implicit(_noise.Evaluate());
		((Vector2)(ref result))._002Ector(result.x + _offsetX.value, result.y + _offsetY.value);
		if (_interpolatedPosition)
		{
			GetInterpolatedPosition(projectile, raycastHit, ref result);
		}
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(0f, 0f, _angle.value);
		int num;
		if (_flipXByLookingDirection)
		{
			num = ((owner.lookingDirection == Character.LookingDirection.Left) ? 1 : 0);
			if (num != 0)
			{
				val.z = (180f - val.z) % 360f;
			}
		}
		else
		{
			num = 0;
		}
		OperationRunner operationRunner = _operationRunner.Spawn();
		OperationInfos operationInfos = operationRunner.operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(Vector2.op_Implicit(result), Quaternion.Euler(val));
		SortingGroup component = ((Component)operationRunner).GetComponent<SortingGroup>();
		if ((Object)(object)component != (Object)null)
		{
			component.sortingOrder = spriteLayer++;
		}
		if (num != 0)
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f) * _scale.value;
		}
		else
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f) * _scale.value;
		}
		operationInfos.Run(owner);
		if (_attachToOwner)
		{
			((Component)operationInfos).transform.parent = ((Component)this).transform;
		}
	}

	private void GetInterpolatedPosition(IProjectile projectile, RaycastHit2D hit, ref Vector2 result)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		float num = _interpolatedSize / 2f;
		Bounds bounds = ((RaycastHit2D)(ref hit)).collider.bounds;
		_ = ((RaycastHit2D)(ref hit)).point;
		float num2 = projectile.transform.position.x + num;
		float num3 = projectile.transform.position.x - num;
		if (num2 > ((Bounds)(ref bounds)).max.x)
		{
			result = new Vector2(((Bounds)(ref bounds)).max.x - num, result.y);
		}
		if (num3 < ((Bounds)(ref bounds)).min.x)
		{
			result = new Vector2(((Bounds)(ref bounds)).min.x + num, result.y);
		}
	}
}
