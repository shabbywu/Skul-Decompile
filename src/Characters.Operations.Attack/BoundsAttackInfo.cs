using Characters.Movements;
using Characters.Operations.Movement;
using FX.BoundsAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public class BoundsAttackInfo : MonoBehaviour
{
	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

	[SerializeField]
	private ChronoInfo _chronoToGlobe;

	[SerializeField]
	private ChronoInfo _chronoToOwner;

	[SerializeField]
	private ChronoInfo _chronoToTarget;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operationsToOwner;

	[Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _operationInfo;

	[BoundsAttackVisualEffect.Subcomponent]
	[SerializeField]
	private BoundsAttackVisualEffect.Subcomponents _effect;

	internal HitInfo hitInfo => _hitInfo;

	internal OperationInfo.Subcomponents operationsToOwner => _operationsToOwner;

	internal TargetedOperationInfo.Subcomponents operationInfo => _operationInfo;

	internal BoundsAttackVisualEffect.Subcomponents effect => _effect;

	internal PushInfo pushInfo { get; private set; }

	internal void Initialize()
	{
		_operationsToOwner.Initialize();
		_operationInfo.Initialize();
		TargetedOperationInfo[] components = ((SubcomponentArray<TargetedOperationInfo>)_operationInfo).components;
		foreach (TargetedOperationInfo targetedOperationInfo in components)
		{
			if (targetedOperationInfo.operation is Knockback knockback)
			{
				pushInfo = knockback.pushInfo;
				break;
			}
			if (targetedOperationInfo.operation is Smash smash)
			{
				pushInfo = smash.pushInfo;
			}
		}
	}

	internal void ApplyChrono(Character owner)
	{
		_chronoToGlobe.ApplyGlobe();
		_chronoToOwner.ApplyTo(owner);
	}

	internal void ApplyChrono(Character owner, Character target)
	{
		_chronoToGlobe.ApplyGlobe();
		_chronoToOwner.ApplyTo(owner);
		_chronoToTarget.ApplyTo(target);
	}
}
