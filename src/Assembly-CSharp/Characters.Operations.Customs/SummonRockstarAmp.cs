using Characters.Gear.Weapons.Rockstar;
using UnityEngine;

namespace Characters.Operations.Customs;

public class SummonRockstarAmp : CharacterOperation
{
	[SerializeField]
	private Amp _rockstarAmp;

	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private bool _flipX;

	public override void Run(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_rockstarAmp.InstantiateAmp(_spawnPosition.position, _flipX);
	}
}
