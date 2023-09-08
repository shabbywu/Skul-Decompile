using Characters.Movements;
using Characters.Operations.Movement;
using FX.CastAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Attack;

public class CastAttackInfo : MonoBehaviour
{
	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

	[SerializeField]
	private ChronoInfo _chronoToGlobe;

	[SerializeField]
	private ChronoInfo _chronoToOwner;

	[SerializeField]
	private ChronoInfo _chronoToTarget;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operationsToOwner;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _operationsToCharacter;

	[CastAttackVisualEffect.Subcomponent]
	[SerializeField]
	private CastAttackVisualEffect.Subcomponents _effect;

	internal HitInfo hitInfo => _hitInfo;

	internal OperationInfo.Subcomponents operationsToOwner => _operationsToOwner;

	internal TargetedOperationInfo.Subcomponents operationsToCharacter => _operationsToCharacter;

	internal CastAttackVisualEffect.Subcomponents effect => _effect;

	internal PushInfo pushInfo { get; private set; }

	internal void Initialize()
	{
		_operationsToOwner.Initialize();
		_operationsToCharacter.Initialize();
		TargetedOperationInfo[] components = operationsToCharacter.components;
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
