using System.Collections.Generic;
using Characters.AI;
using FX;
using UnityEngine;

namespace Characters.Operations;

public sealed class CommandToSacrifice : CharacterOperation
{
	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private AIController _aiController;

	[SerializeField]
	private EffectInfo _effect;

	public override void Run(Character owner)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		List<Character> list = _aiController.FindEnemiesInRange(_range);
		if (list == null || list.Count <= 0)
		{
			return;
		}
		foreach (Character item in list)
		{
			SacrificeCharacter component = ((Component)item).GetComponent<SacrificeCharacter>();
			if (!((Object)(object)component == (Object)null))
			{
				component.Run();
				_effect.Spawn(((Component)component).transform.position);
			}
		}
	}
}
