using Characters.Operations;
using FX;
using UnityEngine;

namespace Characters.Projectiles.Operations.Customs;

public sealed class SummonOperationRunnerToEqaulSprite : Operation
{
	[Tooltip("오퍼레이션 프리팹")]
	[SerializeField]
	private OperationRunner _operationRunner;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private CustomFloat _scale = new CustomFloat(1f);

	[SerializeField]
	private CustomAngle _angle;

	[SerializeField]
	private PositionNoise _noise;

	[SerializeField]
	[Space]
	private bool _snapToGround;

	[Tooltip("땅을 찾기 위해 소환지점으로부터 아래로 탐색할 거리. 실패 시 그냥 소환 지점에 소환됨")]
	[SerializeField]
	private float _groundFindingDistance = 7f;

	[SerializeField]
	[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
	private bool _flipXByLookingDirection;

	[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
	[SerializeField]
	private bool _attachToOwner;

	private void OnDestroy()
	{
		_operationRunner = null;
	}

	public override void Run(IProjectile projectile)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		Character owner = projectile.owner;
		Vector3 val = (((Object)(object)_spawnPosition == (Object)null) ? ((Component)this).transform.position : _spawnPosition.position);
		if (_snapToGround)
		{
			RaycastHit2D val2 = Physics2D.Raycast(Vector2.op_Implicit(val), Vector2.down, _groundFindingDistance, LayerMask.op_Implicit(Layers.groundMask));
			if (RaycastHit2D.op_Implicit(val2))
			{
				val = Vector2.op_Implicit(((RaycastHit2D)(ref val2)).point);
			}
		}
		val += _noise.Evaluate();
		Vector3 val3 = default(Vector3);
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
		OperationInfos operationInfos = _operationRunner.Spawn().operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(val, Quaternion.Euler(val3));
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
		((Component)operationInfos).GetComponentInChildren<Animator>().Play(((Object)_spriteRenderer.sprite).name);
	}
}
