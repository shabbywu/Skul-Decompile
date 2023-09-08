using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}PrioritySelectorIcon.png")]
[TaskDescription("가중치에 따라 실행되는 Selector, 한 번 선택이후 자식 노드의 성공 여부와 상관없이 선택된 노드만 실행됨")]
public class WeightedSelector : Composite
{
	[SerializeField]
	private List<float> _weights;

	private int currentChildIndex;

	private TaskStatus executionStatus;

	private WeightedRandomizer<int> _randomizer;

	public override void OnStart()
	{
		List<(int, float)> list = new List<(int, float)>(((ParentTask)this).children.Count);
		for (int i = 0; i < ((ParentTask)this).children.Count; i++)
		{
			list.Add((i, _weights[i]));
		}
		_randomizer = new WeightedRandomizer<int>(list);
		currentChildIndex = _randomizer.TakeOne();
	}

	public override int CurrentChildIndex()
	{
		return currentChildIndex;
	}

	public override bool CanExecute()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)executionStatus != 1)
		{
			return (int)executionStatus != 2;
		}
		return false;
	}

	public override void OnChildExecuted(TaskStatus childStatus)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		currentChildIndex++;
		executionStatus = childStatus;
	}

	public override void OnConditionalAbort(int childIndex)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		currentChildIndex = childIndex;
		executionStatus = (TaskStatus)0;
	}

	public override void OnEnd()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		executionStatus = (TaskStatus)0;
		currentChildIndex = 0;
	}

	public override string OnDrawNodeText()
	{
		string result = ((Task)this).OnDrawNodeText();
		if (_weights == null)
		{
			return result;
		}
		if (((ParentTask)this).children == null)
		{
			return result;
		}
		if (_weights.Count == ((ParentTask)this).children.Count)
		{
			((Task)this).NodeData.ColorIndex = 0;
		}
		else
		{
			((Task)this).NodeData.ColorIndex = 1;
		}
		return result;
	}
}
