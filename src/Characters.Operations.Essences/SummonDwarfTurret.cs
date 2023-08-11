using Characters.Abilities.Essences;
using Characters.Gear.Quintessences;
using Characters.Minions;
using UnityEngine;

namespace Characters.Operations.Essences;

public sealed class SummonDwarfTurret : CharacterOperation
{
	[SerializeField]
	private DwarfComponent _dwarf;

	[SerializeField]
	private DwarfTurret _dwarfTurret;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private MinionSetting _overrideSetting;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private Transform[] _spawnPositions;

	public override void Run(Character owner)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (owner.playerComponents == null)
		{
			return;
		}
		if (_spawnPositions.Length == 0)
		{
			Summon(owner, ((Component)owner).transform.position);
			return;
		}
		Transform[] spawnPositions = _spawnPositions;
		foreach (Transform val in spawnPositions)
		{
			Summon(owner, val.position);
		}
	}

	private void Summon(Character owner, Vector3 position)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		owner.playerComponents.minionLeader.Summon(_dwarfTurret.minion, position, _overrideSetting);
	}
}
