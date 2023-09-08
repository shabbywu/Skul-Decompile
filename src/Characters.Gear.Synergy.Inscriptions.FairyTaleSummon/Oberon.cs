using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities.Constraints;
using Characters.Operations;
using Characters.Operations.Fx;
using FX;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions.FairyTaleSummon;

public class Oberon : MonoBehaviour
{
	private const string _attackName = "Attack";

	private static readonly int _attackHash = Animator.StringToHash("Attack");

	private float _attackLength;

	private const string _idleName = "Idle";

	private static readonly int _idleHash = Animator.StringToHash("Idle");

	private float _idleLength;

	private const string _introName = "Intro";

	private static readonly int _introHash = Animator.StringToHash("Intro");

	private float _introLength;

	private const string _bombEndName = "SpiritBomb_End";

	private static readonly int _bombEndHash = Animator.StringToHash("SpiritBomb_End");

	private float _bombEndLength;

	private const string _bombLoopName = "SpiritBomb_Loop";

	private static readonly int _bombLoopHash = Animator.StringToHash("SpiritBomb_Loop");

	private float _bombLoopLength;

	private const string _bombReadyName = "SpiritBomb_Ready";

	private static readonly int _bombReadyHash = Animator.StringToHash("SpiritBomb_Ready");

	private float _bombReadyLength;

	private const string _thunderName = "SpiritThunder";

	private static readonly int _thunderHash = Animator.StringToHash("SpiritThunder");

	private float _thunderLength;

	private Character _owner;

	[Constraint.Subcomponent]
	[SerializeField]
	private Constraint.Subcomponents _constraints;

	[Header("Movement")]
	private Transform _slot;

	[SerializeField]
	private float _trackSpeed = 2.5f;

	[SerializeField]
	private float _floatAmplitude = 0.5f;

	[SerializeField]
	private float _floatFrequency = 1f;

	[Header("Graphic")]
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private EffectInfo _introEffect;

	[SerializeField]
	[Header("Galaxy Beam")]
	private Collider2D _attackDetectRange;

	[SerializeField]
	private EffectInfo _attackEffect;

	[SerializeField]
	private OperationRunner _attackOperationRunner;

	[SerializeField]
	private float _attackCooldown;

	private float _remainAttackCooldown;

	[Header("Spirit Thunder")]
	[SerializeField]
	private Collider2D _thunderDetectRange;

	[SerializeField]
	private EffectInfo _thunderEffect;

	[SerializeField]
	private OperationRunner _thunderOperationRunner;

	[SerializeField]
	private float _thunderCooldown;

	private float _remainThunderCooldown;

	[SerializeField]
	[Header("Spirit Nemesis")]
	private Collider2D _bombDetectRange;

	[SerializeField]
	private EffectInfo _bombEffect;

	[SerializeField]
	private Characters.Operations.Fx.ScreenFlash _bombScreenFlash;

	[SerializeField]
	private OperationRunner _bombOperationRunner;

	[SerializeField]
	private float _bombCooldown;

	[SerializeField]
	private Transform _bombSpawnPosition;

	private float _remainBombCooldown;

	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	private NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

	private RayCaster _groundFinder;

	private Vector3 _position;

	private float _floatingTime;

	private void Awake()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_attackDetectRange).enabled = false;
		((Behaviour)_thunderDetectRange).enabled = false;
		((Behaviour)_bombDetectRange).enabled = false;
		_groundFinder = new RayCaster
		{
			direction = Vector2.down,
			distance = 5f
		};
		((ContactFilter2D)(ref ((Caster)_groundFinder).contactFilter)).SetLayerMask(Layers.groundMask);
		_remainAttackCooldown = _attackCooldown;
		_remainThunderCooldown = _thunderCooldown;
		_remainBombCooldown = _bombCooldown;
		_bombScreenFlash.Initialize();
		Dictionary<string, AnimationClip> dictionary = _animator.runtimeAnimatorController.animationClips.ToDictionary((AnimationClip clip) => ((Object)clip).name);
		_idleLength = dictionary["Idle"].length;
		_introLength = dictionary["Intro"].length;
		_bombEndLength = dictionary["SpiritBomb_End"].length;
		_bombLoopLength = dictionary["SpiritBomb_Loop"].length;
		_bombReadyLength = dictionary["SpiritBomb_Ready"].length;
		_thunderLength = dictionary["SpiritThunder"].length;
	}

	public void Initialize(Character owner, Transform slot)
	{
		_owner = owner;
		_slot = slot;
		ResetPosition();
		((MonoBehaviour)this).StartCoroutine(CCooldown());
		((MonoBehaviour)this).StartCoroutine(CRun());
		Singleton<Service>.Instance.levelManager.onMapLoaded += ResetPosition;
	}

	private void OnDestroy()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded -= ResetPosition;
	}

	private void ResetPosition()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_slot == (Object)null))
		{
			((Component)this).transform.position = (_position = ((Component)_slot).transform.position);
		}
	}

	private void Move(float deltaTime)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_slot == (Object)null))
		{
			_position = Vector3.Lerp(_position, ((Component)_slot).transform.position, deltaTime * _trackSpeed);
			_floatingTime += deltaTime;
			Vector3 position = _position;
			position.y += Mathf.Sin(_floatingTime * (float)Math.PI * _floatFrequency) * _floatAmplitude;
			((Component)this).transform.position = position;
			_spriteRenderer.flipX = ((Component)_slot).transform.position.x - _position.x < 0f;
		}
	}

	private IEnumerator CCooldown()
	{
		while (true)
		{
			yield return null;
			if (_constraints.components.Pass())
			{
				_remainAttackCooldown -= _owner.chronometer.master.deltaTime;
				_remainThunderCooldown -= _owner.chronometer.master.deltaTime;
				_remainBombCooldown -= _owner.chronometer.master.deltaTime;
			}
		}
	}

	private IEnumerator CPlayAnimation(int hash, float length)
	{
		_animator.Play(hash);
		((Behaviour)_animator).enabled = false;
		float remain = length;
		while (remain > float.Epsilon)
		{
			float deltaTime = Chronometer.global.deltaTime;
			_animator.Update(deltaTime);
			remain -= deltaTime;
			yield return null;
		}
		((Behaviour)_animator).enabled = true;
	}

	private IEnumerator CRun()
	{
		_introEffect.Spawn(((Component)this).transform.position);
		yield return CPlayAnimation(_introHash, _introLength);
		while (true)
		{
			_animator.Play(_idleHash);
			yield return null;
			Move(_owner.chronometer.master.deltaTime);
			if (_remainAttackCooldown < 0f)
			{
				if (FindAttackTarget(out var target, _attackDetectRange))
				{
					_remainAttackCooldown = _attackCooldown;
					_animator.Play(_attackHash);
					_attackEffect.Spawn(((Component)this).transform.position);
					yield return CSpawnAttackOperationRunner(target);
				}
				else
				{
					_remainAttackCooldown = 0.5f;
				}
			}
			if (_remainThunderCooldown < 0f)
			{
				if (FindThunderPosition(out var position))
				{
					_remainThunderCooldown = _thunderCooldown;
					_animator.Play(_thunderHash);
					_thunderEffect.Spawn(((Component)this).transform.position);
					yield return CSpawnThunderOperationRunner(position);
				}
				else
				{
					_remainThunderCooldown = 0.5f;
				}
			}
			if (_remainBombCooldown < 0f)
			{
				if (FindAttackTarget(out var _, _bombDetectRange))
				{
					_remainBombCooldown = _bombCooldown;
					yield return CPlayAnimation(_bombReadyHash, _bombReadyLength);
					_animator.Play(_bombLoopHash);
					_bombEffect.Spawn(((Component)this).transform.position);
					_bombScreenFlash.Run(_owner);
					yield return CSpawnBombOperationRunner();
					yield return CPlayAnimation(_bombEndHash, _bombEndLength);
				}
				else
				{
					_remainBombCooldown = 0.5f;
				}
			}
		}
	}

	private bool FindAttackTarget(out Target target, Collider2D collider)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)_owner).gameObject));
		((Behaviour)collider).enabled = true;
		_overlapper.OverlapCollider(collider);
		((Behaviour)collider).enabled = false;
		List<Target> components = ((IEnumerable<Collider2D>)_overlapper.results).GetComponents<Collider2D, Target>(clearList: true);
		if (components.Count == 0)
		{
			target = null;
			return false;
		}
		target = components[0];
		return true;
	}

	private bool FindThunderPosition(out Vector3 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		position = Vector3.zero;
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)_owner).gameObject));
		((Behaviour)_thunderDetectRange).enabled = true;
		_overlapper.OverlapCollider(_thunderDetectRange);
		((Behaviour)_thunderDetectRange).enabled = false;
		List<Target> components = ((IEnumerable<Collider2D>)_overlapper.results).GetComponents<Collider2D, Target>(clearList: true);
		if (components.Count == 0)
		{
			return false;
		}
		Target target = components[0];
		((Caster)_groundFinder).origin = Vector2.op_Implicit(((Component)target).transform.position);
		RaycastHit2D val = ((Caster)_groundFinder).SingleCast();
		if (!RaycastHit2D.op_Implicit(val))
		{
			return false;
		}
		position = Vector2.op_Implicit(((RaycastHit2D)(ref val)).point);
		return true;
	}

	private IEnumerator CSpawnAttackOperationRunner(Target target)
	{
		Bounds bounds = target.collider.bounds;
		Vector3 val = ((Bounds)(ref bounds)).center - ((Component)this).transform.position;
		float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
		OperationInfos spawnedOperationInfos = _attackOperationRunner.Spawn().operationInfos;
		((Component)spawnedOperationInfos).transform.SetPositionAndRotation(((Component)this).transform.position, Quaternion.Euler(0f, 0f, num));
		spawnedOperationInfos.Run(_owner);
		while (((Component)spawnedOperationInfos).gameObject.activeSelf)
		{
			yield return null;
		}
	}

	private IEnumerator CSpawnThunderOperationRunner(Vector3 position)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		OperationInfos spawnedOperationInfos = _thunderOperationRunner.Spawn().operationInfos;
		((Component)spawnedOperationInfos).transform.SetPositionAndRotation(position, Quaternion.identity);
		spawnedOperationInfos.Run(_owner);
		while (((Component)spawnedOperationInfos).gameObject.activeSelf)
		{
			yield return null;
		}
	}

	private IEnumerator CSpawnBombOperationRunner()
	{
		OperationInfos spawnedOperationInfos = _bombOperationRunner.Spawn().operationInfos;
		((Component)spawnedOperationInfos).transform.SetPositionAndRotation(_bombSpawnPosition.position, Quaternion.identity);
		spawnedOperationInfos.Run(_owner);
		while (((Component)spawnedOperationInfos).gameObject.activeSelf)
		{
			yield return null;
		}
	}
}
