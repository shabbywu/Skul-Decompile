using Characters.Projectiles;
using Characters.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace Characters.Operations.Attack;

public class FireProjectile : CharacterOperation
{
	public enum DirectionType
	{
		RotationOfFirePosition,
		OwnerDirection,
		Constant
	}

	[SerializeField]
	private Projectile _projectile;

	[SerializeField]
	private AssetReference _projectileReference;

	private AsyncOperationHandle<GameObject> _projectileReferenceHandle;

	[Space]
	[SerializeField]
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

	[FormerlySerializedAs("_flipY")]
	[SerializeField]
	private bool _flipYByOwnerDirection;

	[SerializeField]
	[Space]
	private DirectionType _directionType;

	[SerializeField]
	private Reorderable _directions;

	private IAttackDamage _attackDamage;

	public CustomFloat scale => _scale;

	private void Awake()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (_projectileReference != null && _projectileReference.RuntimeKeyIsValid())
		{
			_projectileReferenceHandle = _projectileReference.LoadAssetAsync<GameObject>();
		}
	}

	protected override void OnDestroy()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		base.OnDestroy();
		_projectile = null;
		if (_projectileReferenceHandle.IsValid())
		{
			Addressables.Release<GameObject>(_projectileReferenceHandle);
		}
	}

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		if (_attackDamage == null)
		{
			Debug.LogError((object)"AttackDamage가 없습니다");
		}
		if ((Object)(object)_fireTransform == (Object)null)
		{
			_fireTransform = ((Component)this).transform;
		}
	}

	public override void Run(Character owner)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_projectile == (Object)null && _projectileReferenceHandle.IsValid())
		{
			_projectile = _projectileReferenceHandle.WaitForCompletion().GetComponent<Projectile>();
		}
		CustomAngle[] values = ((ReorderableArray<CustomAngle>)(object)_directions).values;
		bool flipX = false;
		bool flipY = false;
		HitHistoryManager hitHistoryManager = (_group ? new HitHistoryManager(15) : null);
		for (int i = 0; i < values.Length; i++)
		{
			float num;
			switch (_directionType)
			{
			case DirectionType.RotationOfFirePosition:
			{
				Quaternion rotation = _fireTransform.rotation;
				num = ((Quaternion)(ref rotation)).eulerAngles.z + values[i].value;
				if (_fireTransform.lossyScale.x < 0f)
				{
					num = (180f - num) % 360f;
				}
				break;
			}
			case DirectionType.OwnerDirection:
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
			if (_attackDamage == null)
			{
				_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
				if (_attackDamage == null)
				{
					Debug.LogError((object)"발사 시점에도 AttackDamage가 없습니다");
					break;
				}
			}
			Projectile component = ((Component)_projectile.reusable.Spawn(_fireTransform.position, true)).GetComponent<Projectile>();
			((Component)component).transform.localScale = Vector3.one * _scale.value;
			component.Fire(owner, _attackDamage.amount * _damageMultiplier.value, num, flipX, flipY, _speedMultiplier.value, _group ? hitHistoryManager : null);
		}
	}
}
