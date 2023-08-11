using System;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon;

public class SummonOperationRunnerAtTarget : TargetedCharacterOperation
{
	[Serializable]
	public class PositionInfo
	{
		public enum Pivot
		{
			Center,
			TopLeft,
			Top,
			TopRight,
			Left,
			Right,
			BottomLeft,
			Bottom,
			BottomRight,
			Custom
		}

		private static readonly EnumArray<Pivot, Vector2> _pivotValues = new EnumArray<Pivot, Vector2>((Vector2[])(object)new Vector2[10]
		{
			new Vector2(0f, 0f),
			new Vector2(-0.5f, 0.5f),
			new Vector2(0f, 0.5f),
			new Vector2(0.5f, 0.5f),
			new Vector2(-0.5f, 0f),
			new Vector2(0f, 0.5f),
			new Vector2(-0.5f, -0.5f),
			new Vector2(0f, -0.5f),
			new Vector2(0.5f, -0.5f),
			new Vector2(0f, 0f)
		});

		[SerializeField]
		private Pivot _pivot;

		[HideInInspector]
		[SerializeField]
		private Vector2 _pivotValue;

		public Pivot pivot => _pivot;

		public Vector2 pivotValue => _pivotValue;

		public PositionInfo()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			_pivot = Pivot.Center;
			_pivotValue = Vector2.zero;
		}

		public PositionInfo(bool attach, bool layerOnly, int layerOrderOffset, Pivot pivot)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			_pivot = pivot;
			_pivotValue = _pivotValues[pivot];
		}
	}

	private static short spriteLayer = short.MinValue;

	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	private OperationRunner _operationRunner;

	[SerializeField]
	private PositionInfo _positionInfo;

	[SerializeField]
	private bool _attachToTarget;

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private PositionNoise _noise;

	[SerializeField]
	[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
	private bool _flipXByLookingDirection;

	[SerializeField]
	[Tooltip("X축 플립")]
	private bool _flipX;

	[SerializeField]
	private bool _copyAttackDamage;

	private AttackDamage _attackDamage;

	protected void OnDestroy()
	{
		_operationRunner = null;
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
	}

	public override void Run(Character owner, Character target)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
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
		if (_flipX)
		{
			val.z = (180f - val.z) % 360f;
		}
		OperationRunner operationRunner = _operationRunner.Spawn();
		OperationInfos operationInfos = operationRunner.operationInfos;
		if (_copyAttackDamage && (Object)(object)_attackDamage != (Object)null)
		{
			operationRunner.attackDamage.minAttackDamage = _attackDamage.minAttackDamage;
			operationRunner.attackDamage.maxAttackDamage = _attackDamage.maxAttackDamage;
		}
		Vector3 position = ((Component)target).transform.position;
		position.x += ((Collider2D)target.collider).offset.x;
		position.y += ((Collider2D)target.collider).offset.y;
		Bounds bounds = ((Collider2D)target.collider).bounds;
		Vector3 size = ((Bounds)(ref bounds)).size;
		size.x *= _positionInfo.pivotValue.x;
		size.y *= _positionInfo.pivotValue.y;
		((Component)operationInfos).transform.SetPositionAndRotation(position + size, Quaternion.Euler(val));
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
		if (_flipX)
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f) * _scale.value;
		}
		if (_attachToTarget)
		{
			((Component)operationInfos).transform.parent = ((Component)target).transform;
		}
		operationInfos.Run(owner);
	}
}
