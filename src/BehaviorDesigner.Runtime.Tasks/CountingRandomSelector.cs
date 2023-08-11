using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}PrioritySelectorIcon.png")]
[TaskDescription("자식 노드가 실행 횟수만큼 랜덤하게 실행됩니다.")]
public class CountingRandomSelector : Composite
{
	[SerializeField]
	private List<Vector2Int> _counts;

	private int currentChildIndex;

	private TaskStatus executionStatus;

	private List<(int, int)> _childIndics;

	public override void OnAwake()
	{
		((Task)this).OnAwake();
		_childIndics = new List<(int, int)>(((ParentTask)this).children.Count);
		UpdateCount();
	}

	public override void OnStart()
	{
		if (TryTakeOne(out var index))
		{
			currentChildIndex = _childIndics[index].Item1;
			return;
		}
		UpdateCount();
		if (TryTakeOne(out index))
		{
			currentChildIndex = _childIndics[index].Item1;
		}
		else
		{
			Debug.LogError((object)"count는 최소 1 이상 이어야 합니다");
		}
	}

	private void UpdateCount()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		_childIndics.Clear();
		for (int i = 0; i < ((ParentTask)this).children.Count; i++)
		{
			List<(int, int)> childIndics = _childIndics;
			int item = i;
			Vector2Int val = _counts[i];
			int x = ((Vector2Int)(ref val)).x;
			val = _counts[i];
			childIndics.Add((item, Random.Range(x, ((Vector2Int)(ref val)).y + 1)));
		}
	}

	private bool TryTakeOne(out int index)
	{
		ExtensionMethods.Shuffle<(int, int)>((IList<(int, int)>)_childIndics);
		for (int i = 0; i < _childIndics.Count; i++)
		{
			if (_childIndics[i].Item2 > 0)
			{
				index = i;
				_childIndics[index] = (_childIndics[index].Item1, _childIndics[index].Item2 - 1);
				return true;
			}
		}
		index = -1;
		return false;
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
		if (_counts == null)
		{
			return result;
		}
		if (((ParentTask)this).children == null)
		{
			return result;
		}
		if (_counts.Count == ((ParentTask)this).children.Count)
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
