using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("캐릭터간 거리 비교")]
public sealed class ComapreCharacterDistance : Conditional
{
	private enum Compare
	{
		GreatherThan,
		LessThan
	}

	private enum Axis
	{
		XAxis,
		YAxis,
		XYAxis
	}

	[SerializeField]
	private SharedCharacter _from;

	[SerializeField]
	private SharedCharacter _to;

	[SerializeField]
	private Axis _axis;

	[SerializeField]
	private SharedFloat _distance;

	[SerializeField]
	private Compare _comparer;

	public override TaskStatus OnUpdate()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_from).Value;
		Character value2 = ((SharedVariable<Character>)_to).Value;
		float value3 = ((SharedVariable<float>)_distance).Value;
		float num = 0f;
		switch (_axis)
		{
		case Axis.XYAxis:
			num = Vector2.Distance(Vector2.op_Implicit(((Component)value).transform.position), Vector2.op_Implicit(((Component)value2).transform.position));
			break;
		case Axis.XAxis:
			num = Mathf.Abs(((Component)value).transform.position.x - ((Component)value2).transform.position.x);
			break;
		case Axis.YAxis:
			num = Mathf.Abs(((Component)value).transform.position.y - ((Component)value2).transform.position.y);
			break;
		}
		switch (_comparer)
		{
		case Compare.GreatherThan:
			if (!(num >= value3))
			{
				return (TaskStatus)1;
			}
			return (TaskStatus)2;
		case Compare.LessThan:
			if (!(num <= value3))
			{
				return (TaskStatus)1;
			}
			return (TaskStatus)2;
		default:
			return (TaskStatus)1;
		}
	}
}
