using System.Collections;
using System.Collections.Generic;
using Characters.Abilities.Constraints;
using Characters.Operations;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions.FairyTaleSummon;

public class Spirit : MonoBehaviour
{
	private Character _owner;

	[Constraint.Subcomponent]
	[SerializeField]
	private Constraint.Subcomponents _constraints;

	[SerializeField]
	private Transform _slot;

	[SerializeField]
	private float _trackSpeed = 2.5f;

	[SerializeField]
	[Space]
	private Animator _animator;

	[Space]
	[SerializeField]
	private Collider2D _detectRange;

	[Space]
	[Tooltip("오퍼레이션 프리팹")]
	[SerializeField]
	private OperationRunner _operationRunner;

	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	private NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

	private float _attackCooldown;

	private void Awake()
	{
		((Behaviour)_detectRange).enabled = false;
	}

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CRun());
		ResetPosition();
		Singleton<Service>.Instance.levelManager.onMapLoaded += ResetPosition;
	}

	private void OnDisable()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded -= ResetPosition;
	}

	private void ResetPosition()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = ((Component)_slot).transform.position;
	}

	public void Initialize(Character owner)
	{
		_owner = owner;
	}

	public void Set(int attackCooldown, RuntimeAnimatorController graphic)
	{
		_attackCooldown = attackCooldown;
		_animator.runtimeAnimatorController = graphic;
	}

	private bool FindTarget(out Target target)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)_owner).gameObject));
		((Behaviour)_detectRange).enabled = true;
		_overlapper.OverlapCollider(_detectRange);
		((Behaviour)_detectRange).enabled = false;
		List<Target> components = ((IEnumerable<Collider2D>)_overlapper.results).GetComponents<Collider2D, Target>(clearList: true);
		if (components.Count == 0)
		{
			target = null;
			return false;
		}
		target = components[0];
		return true;
	}

	private void Move(float deltaTime)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = Vector3.Lerp(((Component)this).transform.position, ((Component)_slot).transform.position, deltaTime * _trackSpeed);
	}

	private IEnumerator CRun()
	{
		float time = 0f;
		while (true)
		{
			Target target;
			if (time < _attackCooldown)
			{
				yield return null;
				float deltaTime = _owner.chronometer.master.deltaTime;
				time += deltaTime;
				Move(deltaTime);
			}
			else if (!_constraints.components.Pass())
			{
				time -= _owner.chronometer.master.deltaTime;
				yield return null;
			}
			else if (!FindTarget(out target))
			{
				time = _attackCooldown - 0.25f;
			}
			else
			{
				time = 0f;
				yield return CSpawnOperationRunner(target);
			}
		}
	}

	private IEnumerator CSpawnOperationRunner(Target target)
	{
		Bounds bounds = target.collider.bounds;
		Vector3 val = ((Bounds)(ref bounds)).center - ((Component)this).transform.position;
		float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
		OperationInfos spawnedOperationInfos = _operationRunner.Spawn().operationInfos;
		((Component)spawnedOperationInfos).transform.SetPositionAndRotation(((Component)this).transform.position, Quaternion.Euler(0f, 0f, num));
		spawnedOperationInfos.Run(_owner);
		while (((Component)spawnedOperationInfos).gameObject.activeSelf)
		{
			yield return null;
		}
	}
}
