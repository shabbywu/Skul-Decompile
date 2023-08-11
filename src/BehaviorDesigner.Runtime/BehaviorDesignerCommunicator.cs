using UnityEngine;

namespace BehaviorDesigner.Runtime;

[RequireComponent(typeof(BehaviorTree))]
public class BehaviorDesignerCommunicator : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private BehaviorTree _behaviorTree;

	private void Start()
	{
		if ((Object)(object)_behaviorTree == (Object)null)
		{
			_behaviorTree = ((Component)this).GetComponent<BehaviorTree>();
		}
	}

	public T GetVariable<T>(string variableName) where T : SharedVariable
	{
		return (T)(object)((Behavior)_behaviorTree).GetVariable(variableName);
	}

	public SharedVariable GetVariable(string variableName)
	{
		return ((Behavior)_behaviorTree).GetVariable(variableName);
	}

	public void SetVariable(string variableName, SharedVariable variableValue)
	{
		((Behavior)_behaviorTree).SetVariable(variableName, variableValue);
	}

	public void SetVariable<T>(string variableName, object variableValue) where T : SharedVariable
	{
		((SharedVariable)GetVariable<T>(variableName)).SetValue(variableValue);
	}
}
