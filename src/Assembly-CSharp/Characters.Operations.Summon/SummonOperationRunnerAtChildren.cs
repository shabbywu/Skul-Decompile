using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Summon;

public sealed class SummonOperationRunnerAtChildren : CharacterOperation
{
	private static short spriteLayer = short.MinValue;

	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	internal OperationRunner _operationRunner;

	[SerializeField]
	[Space]
	private Transform _spawnPositionParent;

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private PositionNoise _noise;

	[Space]
	[SerializeField]
	[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
	private bool _flipXByLookingDirection;

	[Tooltip("X축 플립")]
	[SerializeField]
	private bool _flipX;

	[SerializeField]
	[Space]
	private bool _snapToGround;

	[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
	[SerializeField]
	private float _groundFindingDistance = 7f;

	[SerializeField]
	[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
	[Space]
	private bool _attachToOwner;

	[SerializeField]
	private bool _copyAttackDamage;

	private AttackDamage _attackDamage;

	protected override void OnDestroy()
	{
		base.OnDestroy();
		_operationRunner = null;
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
	}

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val3 = default(Vector3);
		foreach (Transform item in _spawnPositionParent)
		{
			Vector3 val = item.position;
			if (_snapToGround)
			{
				RaycastHit2D val2 = Physics2D.Raycast(Vector2.op_Implicit(val), Vector2.down, _groundFindingDistance, LayerMask.op_Implicit(Layers.groundMask));
				if (RaycastHit2D.op_Implicit(val2))
				{
					val = Vector2.op_Implicit(((RaycastHit2D)(ref val2)).point);
				}
			}
			val += _noise.Evaluate();
			((Vector3)(ref val3))._002Ector(0f, 0f, _angle.value);
			int num;
			if (_flipXByLookingDirection)
			{
				num = ((owner.lookingDirection == Character.LookingDirection.Left) ? 1 : 0);
				if (num != 0)
				{
					val3.z = (180f - val3.z) % 360f;
				}
			}
			else
			{
				num = 0;
			}
			if (_flipX)
			{
				val3.z = (180f - val3.z) % 360f;
			}
			OperationRunner operationRunner = _operationRunner.Spawn();
			OperationInfos operationInfos = operationRunner.operationInfos;
			((Component)operationInfos).transform.SetPositionAndRotation(val, Quaternion.Euler(val3));
			if (_copyAttackDamage && (Object)(object)_attackDamage != (Object)null)
			{
				operationRunner.attackDamage.minAttackDamage = _attackDamage.minAttackDamage;
				operationRunner.attackDamage.maxAttackDamage = _attackDamage.maxAttackDamage;
			}
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
			operationInfos.Run(owner);
			if (_attachToOwner)
			{
				((Component)operationInfos).transform.parent = ((Component)this).transform;
			}
		}
	}
}
