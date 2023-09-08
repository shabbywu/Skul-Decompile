using System.Collections;
using Characters.Movements;
using Characters.Operations;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class GrabTo : CharacterHitOperation
{
	[SerializeField]
	private float _duration;

	[SerializeField]
	[Header("Destination")]
	private Collider2D _targetPlace;

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	[Header("Force")]
	private Curve _curve;

	[SerializeField]
	private bool _ignoreOtherForce = true;

	[SerializeField]
	private bool _expireOnGround;

	[Header("Hit")]
	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

	[SmashAttackVisualEffect.Subcomponent]
	[SerializeField]
	private SmashAttackVisualEffect.Subcomponents _effect;

	[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _onCollide;

	private IAttackDamage _attackDamage;

	private void Awake()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_onCollide.Initialize();
	}

	private void OnEnd(Push push, Character from, Character to, Push.SmashEndType endType, RaycastHit2D? raycastHit, Movement.CollisionDirection direction)
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

	private IEnumerator CRun(Character owner, Character target)
	{
		float elapsed = 0f;
		while (elapsed < _duration)
		{
			Vector2 val = ((!((Object)(object)_targetPlace != (Object)null)) ? Vector2.op_Implicit(_targetPoint.position) : MMMaths.RandomPointWithinBounds(_targetPlace.bounds));
			Vector2 force = val - Vector2.op_Implicit(((Component)target).transform.position);
			target.movement.push.ApplySmash(owner, force, _curve, _ignoreOtherForce, _expireOnGround, OnEnd);
			elapsed += Chronometer.global.deltaTime;
			yield return null;
		}
	}

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character target)
	{
		Character owner = projectile.owner;
		((MonoBehaviour)this).StartCoroutine(CRun(owner, target));
	}
}
