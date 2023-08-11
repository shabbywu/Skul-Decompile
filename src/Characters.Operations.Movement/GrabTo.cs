using System.Collections;
using Characters.Movements;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Movement;

public sealed class GrabTo : TargetedCharacterOperation
{
	[SerializeField]
	private bool _localCoordinate = true;

	[SerializeField]
	private float _duration;

	[SerializeField]
	[Header("Destination")]
	private Collider2D _targetPlace;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private Transform _targetPoint;

	[Header("Force")]
	[SerializeField]
	private Curve _curve;

	[SerializeField]
	private bool _ignoreOtherForce = true;

	[SerializeField]
	private bool _expireOnGround;

	[SerializeField]
	private bool _applyUnmoving;

	[Header("Hit")]
	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	[SmashAttackVisualEffect.Subcomponent]
	private SmashAttackVisualEffect.Subcomponents _effect;

	[Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _onCollide;

	private IAttackDamage _attackDamage;

	private CharacterStatus.ApplyInfo _statusInfo;

	private Character _target;

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_onCollide.Initialize();
		if (_applyUnmoving)
		{
			_statusInfo = new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Unmoving);
		}
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

	public override void Run(Character owner, Character target)
	{
		if (!((Object)(object)target == (Object)null) && target.liveAndActive && !target.movement.push.ignoreOtherForce)
		{
			_target = target;
			((MonoBehaviour)this).StartCoroutine(CRun(owner, target));
		}
	}

	private IEnumerator CRun(Character owner, Character target)
	{
		float elapsed = 0f;
		Vector2 _targetPointing = Vector2.zero;
		if ((Object)(object)_targetPlace != (Object)null)
		{
			Vector2 val = MMMaths.RandomPointWithinBounds(_targetPlace.bounds);
			if (_localCoordinate)
			{
				_targetPointing = val - Vector2.op_Implicit(((Component)owner).transform.position);
			}
			_targetPoint.position = Vector2.op_Implicit(val);
		}
		while (elapsed < _duration)
		{
			Vector2 force = ((!((Object)(object)_targetPlace != (Object)null) || !_localCoordinate) ? (Vector2.op_Implicit(_targetPoint.position) - Vector2.op_Implicit(((Component)target).transform.position)) : (Vector2.op_Implicit(((Component)owner).transform.position) + _targetPointing - Vector2.op_Implicit(((Component)target).transform.position)));
			target.movement.push.ApplySmash(owner, force, _curve, _ignoreOtherForce, _expireOnGround, OnEnd);
			if ((Object)(object)target.status != (Object)null && !target.status.unmovable)
			{
				owner.GiveStatus(target, _statusInfo);
			}
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			yield return null;
		}
	}

	private void Dispose(Character target)
	{
		if (!((Object)(object)target == (Object)null) && !((Object)(object)target.health == (Object)null) && !target.health.dead)
		{
			if (_applyUnmoving && (Object)(object)target.status != (Object)null)
			{
				target.ability.Remove(target.status.unmoving);
			}
			target.movement.push.Expire();
		}
	}

	private void OnDisable()
	{
		Dispose(_target);
	}
}
