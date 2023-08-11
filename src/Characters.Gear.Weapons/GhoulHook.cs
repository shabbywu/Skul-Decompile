using System;
using System.Collections;
using Characters.Movements;
using FX;
using PhysicsUtils;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Weapons;

public class GhoulHook : MonoBehaviour
{
	private NonAllocCaster _caster = new NonAllocCaster(1);

	[SerializeField]
	private Weapon _weapon;

	[Space]
	[SerializeField]
	private Transform _fireOrigin;

	[SerializeField]
	private Transform _pullOrigin;

	[SerializeField]
	private Transform _flyOrigin;

	[SerializeField]
	[Space]
	private SpriteRenderer _chain;

	[SerializeField]
	private SpriteRenderer _head;

	[SerializeField]
	[Header("Fire")]
	private float _speed;

	[SerializeField]
	private float _distance;

	[SerializeField]
	private float _minDistanceForPlatform;

	[Header("Pull")]
	[SerializeField]
	private float _pullDelay;

	[SerializeField]
	private float _pullSpeed;

	[Tooltip("Pull Collider의 너비와 각도는 체인에 맞게 자동으로 조정됩니다. 높이만 설정하세요.")]
	[SerializeField]
	private BoxCollider2D _pullCollider;

	[SerializeField]
	[Header("Fly")]
	private float _flyDelay;

	[SerializeField]
	private float _flySpeed;

	[Tooltip("Fly 상태가 지속될 수 있는 최대 시간입니다. 이 시간이 넘어가면 도착여부에 관계없이 Fly가 끝납니다.")]
	[SerializeField]
	private float _flyTimeout;

	[Tooltip("Fly가 끝날 때 Vertical Velocity를 몇으로 설정하지를 정합니다. Fly가 끝나면서 살짝 뛰어오르는 연출을 위해 사용합니다.")]
	[SerializeField]
	private float _flyEndVerticalVelocity;

	[SerializeField]
	[Tooltip("Fly가 끝날 때 사운드를 재생합니다.")]
	private SoundInfo _flyEndSound;

	[SerializeField]
	private Movement.Config _flyMovmentConfig;

	private Transform _origin;

	public event Action onTerrainHit;

	public event Action onExpired;

	public event Action onPullEnd;

	public event Action onFlyEnd;

	private void Awake()
	{
		((Component)_chain).transform.parent = null;
		((Component)_head).transform.parent = null;
		((Component)_chain).gameObject.SetActive(false);
		((Component)_head).gameObject.SetActive(false);
	}

	private void LateUpdate()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)_chain).gameObject.activeSelf)
		{
			((Component)_chain).transform.position = _origin.position;
			float num = Vector2.Distance(Vector2.op_Implicit(((Component)_chain).transform.position), Vector2.op_Implicit(((Component)_head).transform.position));
			Vector2 size = _pullCollider.size;
			size.x = num + 0.5f;
			_pullCollider.size = size;
			Vector2 offset = ((Collider2D)_pullCollider).offset;
			offset.x = num * 0.5f;
			if (_weapon.owner.lookingDirection == Character.LookingDirection.Left)
			{
				offset.x *= -1f;
			}
			((Collider2D)_pullCollider).offset = offset;
			size = _chain.size;
			size.x = num;
			_chain.size = size;
			Vector3 right = ((Component)_head).transform.position - ((Component)_chain).transform.position;
			((Component)_chain).transform.right = right;
			((Component)_pullCollider).transform.right = right;
		}
	}

	private void OnDisable()
	{
		_weapon.owner.movement.configs.Remove(_flyMovmentConfig);
		((Component)_chain).gameObject.SetActive(false);
		((Component)_head).gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		Object.Destroy((Object)(object)((Component)_chain).gameObject);
		Object.Destroy((Object)(object)((Component)_head).gameObject);
	}

	public void Fire()
	{
		((MonoBehaviour)this).StopCoroutine("CFire");
		((MonoBehaviour)this).StartCoroutine("CFire");
	}

	private IEnumerator CFire()
	{
		_origin = _fireOrigin;
		((Component)_chain).gameObject.SetActive(true);
		((Component)_head).gameObject.SetActive(true);
		((Component)_head).transform.position = ((Component)_origin).transform.position;
		_ = ((Component)_weapon.owner).transform.lossyScale;
		((Component)_head).transform.localScale = ((Component)this).transform.lossyScale;
		float traveled = 0f;
		while (traveled < _distance)
		{
			yield return null;
			((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(Layers.terrainMaskForProjectile);
			float num = _speed * ((ChronometerBase)_weapon.owner.chronometer.animation).deltaTime;
			traveled += num;
			if (_weapon.owner.lookingDirection == Character.LookingDirection.Left)
			{
				num *= -1f;
			}
			Vector2 right = Vector2.right;
			_caster.RayCast(Vector2.op_Implicit(((Component)_head).transform.position), right, num);
			if (_caster.results.Count > 0)
			{
				RaycastHit2D val = _caster.results[0];
				if (((Component)((RaycastHit2D)(ref val)).collider).gameObject.layer != 19 || traveled > _minDistanceForPlatform)
				{
					Transform transform = ((Component)_head).transform;
					val = _caster.results[0];
					transform.position = Vector2.op_Implicit(((RaycastHit2D)(ref val)).point);
					yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)_weapon.owner.chronometer.animation, _flyDelay);
					this.onTerrainHit?.Invoke();
					yield break;
				}
			}
			((Component)_head).transform.Translate(Vector2.op_Implicit(right * num));
		}
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)_weapon.owner.chronometer.animation, _pullDelay);
		this.onExpired?.Invoke();
	}

	public void Pull()
	{
		((MonoBehaviour)this).StopCoroutine("CPull");
		((MonoBehaviour)this).StartCoroutine("CPull");
	}

	private IEnumerator CPull()
	{
		_origin = _pullOrigin;
		Vector3 headPosition = ((Component)_head).transform.position;
		float time = 0f;
		while (time < 1f)
		{
			yield return null;
			time += ((ChronometerBase)_weapon.owner.chronometer.animation).deltaTime * _pullSpeed;
			((Component)_head).transform.position = Vector2.op_Implicit(Vector2.LerpUnclamped(Vector2.op_Implicit(headPosition), Vector2.op_Implicit(((Component)_origin).transform.position), time));
		}
		((Component)_chain).gameObject.SetActive(false);
		((Component)_head).gameObject.SetActive(false);
		this.onPullEnd?.Invoke();
	}

	public void Fly()
	{
		((MonoBehaviour)this).StopCoroutine("CFly");
		((MonoBehaviour)this).StartCoroutine("CFly");
	}

	private IEnumerator CFly()
	{
		_origin = _flyOrigin;
		_weapon.owner.movement.configs.Add(2147483646, _flyMovmentConfig);
		float time = 0f;
		while (time < _flyTimeout)
		{
			yield return (object)new WaitForEndOfFrame();
			float deltaTime = ((ChronometerBase)_weapon.owner.chronometer.animation).deltaTime;
			time += deltaTime;
			((ContactFilter2D)(ref _caster.contactFilter)).SetLayerMask(Layers.terrainMask);
			float num = _flySpeed * deltaTime;
			Vector3 position = ((Component)_head).transform.position;
			Bounds bounds = ((Collider2D)_weapon.hitbox).bounds;
			Vector3 val = position - ((Bounds)(ref bounds)).center;
			((Vector3)(ref val)).Normalize();
			_caster.ColliderCast((Collider2D)(object)_weapon.hitbox, Vector2.op_Implicit(val), num);
			Vector3 val2 = val * num;
			Vector3 val3 = ((Component)_weapon.owner).transform.position + val2;
			if ((val.x > 0f && val3.x - ((Component)_head).transform.position.x > 0f) || (val.x < 0f && val3.x - ((Component)_head).transform.position.x < 0f) || _caster.results.Count > 0)
			{
				Transform transform = ((Component)_head).transform;
				RaycastHit2D val4 = _caster.results[0];
				transform.position = Vector2.op_Implicit(((RaycastHit2D)(ref val4)).point);
				PersistentSingleton<SoundManager>.Instance.PlaySound(_flyEndSound, ((Component)this).transform.position);
				_weapon.owner.movement.configs.Remove(_flyMovmentConfig);
				_weapon.owner.movement.verticalVelocity = _flyEndVerticalVelocity;
				((Component)_chain).gameObject.SetActive(false);
				((Component)_head).gameObject.SetActive(false);
				this.onFlyEnd?.Invoke();
				yield break;
			}
			_weapon.owner.movement.force = Vector2.op_Implicit(val2);
		}
		PersistentSingleton<SoundManager>.Instance.PlaySound(_flyEndSound, ((Component)this).transform.position);
		_weapon.owner.movement.configs.Remove(_flyMovmentConfig);
		((Component)_chain).gameObject.SetActive(false);
		((Component)_head).gameObject.SetActive(false);
		this.onFlyEnd?.Invoke();
	}
}
