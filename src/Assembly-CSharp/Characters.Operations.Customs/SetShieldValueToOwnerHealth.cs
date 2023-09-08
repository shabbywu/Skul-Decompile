using Characters.Abilities;
using UnityEngine;

namespace Characters.Operations.Customs;

public class SetShieldValueToOwnerHealth : CharacterOperation
{
	[SerializeField]
	private ShieldComponent _shieldComponent;

	public override void Run(Character owner)
	{
		_shieldComponent.baseAbility.amount = (float)owner.health.maximumHealth;
	}
}
