using Characters.Abilities.Weapons.DavyJones;
using UnityEngine;

namespace Characters.Operations.Customs.DavyJones;

public sealed class PopCannonBall : CharacterOperation
{
	[SerializeField]
	private DavyJonesPassiveComponent _passive;

	public override void Run(Character owner)
	{
		_passive.Pop();
	}
}
