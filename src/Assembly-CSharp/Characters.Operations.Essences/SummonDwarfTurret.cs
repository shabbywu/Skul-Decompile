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
	[Information("비워둘 경우 Default 설정 값을 적용", InformationAttribute.InformationType.Info, false)]
	private MinionSetting _overrideSetting;

	[SerializeField]
	[Information("비워둘 경우 플레이어 위치에 1마리 소환, 그 외에는 지정된 위치마다 소환됨", InformationAttribute.InformationType.Info, false)]
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
