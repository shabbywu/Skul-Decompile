using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Operations.Customs.GraveDigger;

public sealed class SummonMinionFromGraves : CharacterOperation
{
	[SerializeField]
	private GraveDiggerGraveContainer _graveContainer;

	[SerializeField]
	[Space]
	private Minion _minionPrefab;

	[SerializeField]
	private int _maxCount;

	public override void Run(Character owner)
	{
		_graveContainer.SummonMinionFromGraves(_minionPrefab, _maxCount);
	}
}
