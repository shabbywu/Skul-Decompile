using Characters.Abilities.Weapons.DavyJones;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs;

public sealed class DavyJonesEmptyMagazineConstraint : Constraint
{
	[SerializeField]
	private DavyJonesPassiveComponent _component;

	public override bool Pass()
	{
		return _component.IsEmpty();
	}
}
