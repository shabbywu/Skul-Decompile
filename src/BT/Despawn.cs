using Characters;
using UnityEngine;

namespace BT;

public sealed class Despawn : Node
{
	[SerializeField]
	private Minion _minion;

	protected override NodeState UpdateDeltatime(Context context)
	{
		_minion.Despawn();
		return NodeState.Success;
	}
}
