using Characters.Abilities.Customs;
using UnityEngine;

namespace Characters.Operations.Customs.GraveDigger;

public sealed class SummonGrave : CharacterOperation
{
	[SerializeField]
	private GraveDiggerPassiveComponent _passive;

	[Space]
	[SerializeField]
	private Transform[] _summonPoints;

	public override void Run(Character owner)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Transform[] summonPoints = _summonPoints;
		foreach (Transform val in summonPoints)
		{
			_passive.SpawnGrave(val.position);
		}
	}
}
