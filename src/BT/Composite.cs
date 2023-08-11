using UnityEditor;
using UnityEngine;

namespace BT;

public abstract class Composite : Node
{
	[SerializeField]
	[Subcomponent(typeof(BehaviourTree))]
	protected BehaviourTree.Subcomponents _child;

	protected override void DoReset(NodeState state)
	{
		ResetChild();
	}

	private void ResetChild()
	{
		for (int i = 0; i < ((SubcomponentArray<BehaviourTree>)_child).components.Length; i++)
		{
			((SubcomponentArray<BehaviourTree>)_child).components[i].node.ResetState();
		}
	}
}
