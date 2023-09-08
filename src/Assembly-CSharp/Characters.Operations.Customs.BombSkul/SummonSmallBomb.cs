using Characters.Abilities.Customs;
using FX;
using UnityEngine;
using UnityEngine.Rendering;

namespace Characters.Operations.Customs.BombSkul;

public class SummonSmallBomb : CharacterOperation
{
	private static short spriteLayer = short.MinValue;

	[SerializeField]
	private BombSkulPassiveComponent _passvieComponent;

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

	[Tooltip("주인이 바라보고 있는 방향에 따라 X축으로 플립")]
	[SerializeField]
	private bool _flipXByLookingDirection;

	[SerializeField]
	[Tooltip("X축 플립")]
	private bool _flipX;

	[SerializeField]
	private bool _copyAttackDamage;

	[SerializeField]
	private Vector2 _minVelocity;

	[SerializeField]
	private Vector2 _maxVelocity;

	private AttackDamage _attackDamage;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<AttackDamage>();
	}

	public override void Run(Character owner)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
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
		if (_flipX)
		{
			val2.z = (180f - val2.z) % 360f;
		}
		OperationRunner operationRunner = _operationRunner.Spawn();
		_passvieComponent.RegisterSmallBomb(operationRunner);
		OperationInfos operationInfos = operationRunner.operationInfos;
		((Component)operationInfos).transform.SetPositionAndRotation(val, Quaternion.Euler(val2));
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
		((Component)operationRunner).GetComponent<Rigidbody2D>().velocity = MMMaths.RandomVector2(_minVelocity, _maxVelocity);
	}
}
