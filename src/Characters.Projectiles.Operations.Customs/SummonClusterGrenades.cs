using Characters.Operations;
using FX;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Projectiles.Operations.Customs;

public sealed class SummonClusterGrenades : Operation
{
	[SerializeField]
	[Tooltip("오퍼레이션 프리팹")]
	private OperationRunner _operationRunner;

	[SerializeField]
	private Transform _spawnPosition;

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
	[Tooltip("체크하면 주인에 부착되며, 같이 움직임")]
	private bool _attachToOwner;

	[Space]
	[SerializeField]
	private Vector2[] _forces;

	[SerializeField]
	private float[] _speeds;

	public override void Run(IProjectile projectile)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _forces.Length; i++)
		{
			Rigidbody2D component = ((Component)Summon(projectile, _speeds[i])).GetComponent<Rigidbody2D>();
			Vector2 val = _forces[i] * projectile.firedDirection;
			val.y = math.abs(val.y);
			component.AddForce(val * 2f, (ForceMode2D)1);
		}
	}

	private OperationInfos Summon(IProjectile projectile, float speed)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		Character owner = projectile.owner;
		Vector3 val = (((Object)(object)_spawnPosition == (Object)null) ? ((Component)this).transform.position : (_spawnPosition.position + _noise.Evaluate()));
		Vector3 val2 = default(Vector3);
		((Vector3)(ref val2))._002Ector(0f, 0f, _angle.value);
		int num;
		if (_flipXByLookingDirection)
		{
			num = ((owner.lookingDirection == Character.LookingDirection.Left) ? 1 : 0);
			if (num != 0)
			{
				val2.z = (180f - val2.z) % 360f;
			}
		}
		else
		{
			num = 0;
		}
		OperationInfos operationInfos = _operationRunner.Spawn().operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(val, Quaternion.Euler(val2));
		if (num != 0)
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, -1f, 1f) * _scale.value;
		}
		else
		{
			((Component)operationInfos).transform.localScale = new Vector3(1f, 1f, 1f) * _scale.value;
		}
		operationInfos.Run(owner, speed);
		if (_attachToOwner)
		{
			((Component)operationInfos).transform.parent = ((Component)this).transform;
		}
		return operationInfos;
	}
}
