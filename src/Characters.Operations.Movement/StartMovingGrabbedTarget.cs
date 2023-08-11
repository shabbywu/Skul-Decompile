using System.Collections;
using System.Collections.Generic;
using Characters.Movements;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Operations.Movement;

public sealed class StartMovingGrabbedTarget : CharacterOperation
{
	[SerializeField]
	private GrabBoard _grabBoard;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private Transform _grabber;

	[SerializeField]
	private float _duration;

	[Header("Destination")]
	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private Collider2D _targetPlace;

	[SerializeField]
	private Transform _targetTransform;

	[Header("Force")]
	[SerializeField]
	private Curve _curve;

	[SerializeField]
	private bool _ignoreOtherForce = true;

	[SerializeField]
	private bool _expireOnGround;

	[SerializeField]
	[Header("Hit")]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	[SmashAttackVisualEffect.Subcomponent]
	private SmashAttackVisualEffect.Subcomponents _effect;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _onCollide;

	private IAttackDamage _attackDamage;

	private CharacterStatus.ApplyInfo _statusInfo;

	private Dictionary<Target, Vector2> _relatedDirectionDict;

	private bool _grabRunning;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_onCollide.Initialize();
		_statusInfo = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Unmoving);
		_relatedDirectionDict = new Dictionary<Target, Vector2>(_grabBoard.maxTargetCount);
	}

	public override void Run(Character owner)
	{
		_relatedDirectionDict.Clear();
		if ((Object)(object)_grabber == (Object)null)
		{
			_grabber = ((Component)owner).transform;
		}
		((MonoBehaviour)this).StartCoroutine(Grab(owner));
	}

	private IEnumerator Grab(Character owner)
	{
		_grabRunning = true;
		float elapsed = 0f;
		while (elapsed < _duration)
		{
			foreach (Target target in _grabBoard.targets)
			{
				if ((Object)(object)target == (Object)null)
				{
					Debug.Log((object)"Grabbed Target is null");
					continue;
				}
				if ((Object)(object)_targetPlace != (Object)null && !_relatedDirectionDict.ContainsKey(target))
				{
					Vector2 value = MMMaths.RandomPointWithinBounds(_targetPlace.bounds) - Vector2.op_Implicit(_grabber.position);
					_relatedDirectionDict.Add(target, value);
				}
				if ((Object)(object)target.character.status != (Object)null && !target.character.status.unmovable)
				{
					owner.GiveStatus(target.character, _statusInfo);
				}
				Vector2 force = ((!((Object)(object)_targetPlace != (Object)null)) ? (Vector2.op_Implicit(_targetTransform.position) - Vector2.op_Implicit(((Component)target).transform.position)) : (Vector2.op_Implicit(_grabber.position) + _relatedDirectionDict[target] - Vector2.op_Implicit(((Component)target).transform.position)));
				target.character.movement.push.ApplySmash(owner, force, _curve, _ignoreOtherForce, _expireOnGround, OnEnd);
			}
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			yield return null;
		}
		Dispose();
	}

	public override void Stop()
	{
		base.Stop();
		Dispose();
	}

	private void Dispose()
	{
		if (!_grabRunning)
		{
			return;
		}
		_grabRunning = false;
		_relatedDirectionDict.Clear();
		foreach (Target target in _grabBoard.targets)
		{
			if (!((Object)(object)target == (Object)null) && !((Object)(object)target.character == (Object)null) && !((Object)(object)target.character.health == (Object)null) && !target.character.health.dead)
			{
				if ((Object)(object)target.character.status != (Object)null)
				{
					target.character.ability.Remove(target.character.status.unmoving);
				}
				target.character.movement.push.Expire();
			}
		}
		((MonoBehaviour)this).StopAllCoroutines();
	}

	private void OnEnd(Push push, Character from, Character to, Push.SmashEndType endType, RaycastHit2D? raycastHit, Characters.Movements.Movement.CollisionDirection direction)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		if (endType == Push.SmashEndType.Collide)
		{
			((MonoBehaviour)this).StartCoroutine(_onCollide.CRun(from, to));
			Stat stat = from.stat;
			double baseDamage = _attackDamage.amount;
			RaycastHit2D value = raycastHit.Value;
			Damage damage = stat.GetDamage(baseDamage, ((RaycastHit2D)(ref value)).point, _hitInfo);
			TargetStruct targetStruct = new TargetStruct(to);
			from.TryAttackCharacter(targetStruct, ref damage);
			_effect.Spawn(to, push, raycastHit.Value, direction, damage, targetStruct);
		}
	}

	private void OnDisable()
	{
		if (_grabRunning)
		{
			Stop();
		}
	}
}
