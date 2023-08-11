using Characters.Usables;
using UnityEngine;

namespace Characters.Operations.Summon;

public sealed class SummonLiquid : CharacterOperation
{
	[SerializeField]
	private Liquid _liquid;

	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	[Space(5f)]
	[Header("Optional")]
	private LiquidMaster _liquidMaster;

	public override void Run(Character owner)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_liquidMaster == (Object)null)
		{
			_liquid.Spawn(Vector2.op_Implicit(_spawnPosition.position));
		}
		else
		{
			_liquid.Spawn(Vector2.op_Implicit(_spawnPosition.position), _liquidMaster);
		}
	}
}
