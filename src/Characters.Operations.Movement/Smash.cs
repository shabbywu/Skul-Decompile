using Characters.Movements;
using FX.SmashAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Movement;

public class Smash : TargetedCharacterOperation
{
	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private Transform _transfromOverride;

	[SerializeField]
	private PushInfo _pushInfo = new PushInfo(ignoreOtherForce: true);

	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	[SmashAttackVisualEffect.Subcomponent]
	private SmashAttackVisualEffect.Subcomponents _effect;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _onCollide;

	private IAttackDamage _attackDamage;

	public PushInfo pushInfo => _pushInfo;

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
		if (!((Object)(object)target == (Object)null) && target.liveAndActive)
		{
			if ((Object)(object)_transfromOverride == (Object)null)
			{
				target.movement.push.ApplySmash(owner, _pushInfo, OnEnd);
			}
			else
			{
				target.movement.push.ApplySmash(owner, _transfromOverride, _pushInfo, OnEnd);
			}
		}
	}
}
