using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs;

public class GraveDiggerGraveConstraint : Constraint
{
	[SerializeField]
	private GraveDiggerGraveContainer _container;

	public override bool Pass()
	{
		return _container.hasActivatedGrave;
	}
}
