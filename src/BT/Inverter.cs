namespace BT;

public class Inverter : Decorator
{
	protected override NodeState UpdateDeltatime(Context context)
	{
		NodeState nodeState = _subTree.Tick(context);
		return nodeState switch
		{
			NodeState.Success => NodeState.Fail, 
			NodeState.Fail => NodeState.Success, 
			_ => nodeState, 
		};
	}
}
