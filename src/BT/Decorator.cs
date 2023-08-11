using UnityEditor;
using UnityEngine;

namespace BT;

public abstract class Decorator : Node
{
	[SerializeField]
	[Subcomponent(typeof(BehaviourTree))]
	protected BehaviourTree _subTree;

	protected override void DoReset(NodeState state)
	{
		_subTree.node.ResetState();
	}
}
