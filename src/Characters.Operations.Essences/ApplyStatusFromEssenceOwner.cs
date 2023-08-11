using Characters.Gear.Quintessences;
using UnityEngine;

namespace Characters.Operations.Essences;

public sealed class ApplyStatusFromEssenceOwner : CharacterOperation
{
	[SerializeField]
	private Quintessence _essence;

	[SerializeField]
	private CharacterStatus.ApplyInfo _status;

	[Range(1f, 100f)]
	[SerializeField]
	private int _chance = 100;

	public override void Run(Character owner, Character target)
	{
		if (MMMaths.PercentChance(_chance))
		{
			_essence.owner.GiveStatus(target, _status);
		}
	}

	public override void Run(Character target)
	{
		if (MMMaths.PercentChance(_chance))
		{
			_essence.owner.status?.Apply(null, _status);
		}
	}
}
