using Characters.AI;
using UnityEngine;

namespace Runnables.Triggers;

public class AIFoundTarget : Trigger
{
	[SerializeField]
	private AIController _ai;

	protected override bool Check()
	{
		return (Object)(object)_ai.target != (Object)null;
	}
}
