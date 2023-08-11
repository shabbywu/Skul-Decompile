using Characters.Abilities.Weapons.DavyJones;
using UnityEngine;

namespace Characters.Operations.Customs.DavyJones;

public sealed class PushCannonBall : CharacterOperation
{
	[SerializeField]
	private DavyJonesPassiveComponent _passive;

	[SerializeField]
	private CannonBallType _type;

	[SerializeField]
	private int _count = 1;

	public override void Run(Character owner)
	{
		_passive.Push(_type, _count);
	}
}
