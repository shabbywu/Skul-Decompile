using Characters.Movements;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Movement;

public class SmashTo : TargetedCharacterOperation
{
	[Header("Destination")]
	[SerializeField]
	private Collider2D _targetPlace;

	[SerializeField]
	private Transform _targetPoint;

	[Header("Force")]
	[SerializeField]
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

	public override void Initialize()
	{
		_attackDamage = ((Component)this).GetComponentInParent<IAttackDamage>();
		_onCollide.Initialize();
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
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)target == (Object)null) && target.liveAndActive)
		{
			Vector2 val = ((!((Object)(object)_targetPlace != (Object)null)) ? Vector2.op_Implicit(_targetPoint.position) : MMMaths.RandomPointWithinBounds(_targetPlace.bounds));
			Vector2 force = val - Vector2.op_Implicit(((Component)target).transform.position);
			target.movement.push.ApplySmash(owner, force, _curve, _ignoreOtherForce, _expireOnGround, OnEnd);
		}
	}
}
