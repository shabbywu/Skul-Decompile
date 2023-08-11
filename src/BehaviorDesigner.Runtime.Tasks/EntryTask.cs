namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}EntryIcon.png")]
public class EntryTask : ParentTask
{
	public override int MaxChildren()
	{
		return 1;
	}
}
