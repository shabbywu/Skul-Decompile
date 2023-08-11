using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Allows multiple action tasks to be added to a single node.")]
[TaskIcon("{SkinColor}StackedActionIcon.png")]
public class StackedAction : Action
{
	public enum ComparisonType
	{
		Sequence,
		Selector
	}

	[InspectTask]
	public Action[] actions;

	[Tooltip("Specifies if the tasks should be traversed with an AND (Sequence) or an OR (Selector).")]
	public ComparisonType comparisonType;

	[Tooltip("Should the tasks be labeled within the graph?")]
	public bool graphLabel;

	public override void OnAwake()
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).GameObject = ((Task)this).gameObject;
				((Task)actions[i]).Transform = ((Task)this).transform;
				((Task)actions[i]).Owner = ((Task)this).Owner;
				((Task)actions[i]).OnAwake();
			}
		}
	}

	public override void OnStart()
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).OnStart();
			}
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		if (actions != null)
		{
			for (int i = 0; i < actions.Length; i++)
			{
				if (actions[i] != null)
				{
					TaskStatus val = ((Task)actions[i]).OnUpdate();
					if (comparisonType == ComparisonType.Sequence && (int)val == 1)
					{
						return (TaskStatus)1;
					}
					if (comparisonType == ComparisonType.Selector && (int)val == 2)
					{
						return (TaskStatus)2;
					}
				}
			}
			if (comparisonType == ComparisonType.Sequence)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		}
		return (TaskStatus)1;
	}

	public override void OnFixedUpdate()
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).OnFixedUpdate();
			}
		}
	}

	public override void OnLateUpdate()
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).OnLateUpdate();
			}
		}
	}

	public override void OnEnd()
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).OnEnd();
			}
		}
	}

	public override void OnTriggerEnter2D(Collider2D other)
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).OnTriggerEnter2D(other);
			}
		}
	}

	public override void OnTriggerExit2D(Collider2D other)
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).OnTriggerExit2D(other);
			}
		}
	}

	public override void OnCollisionEnter2D(Collision2D collision)
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).OnCollisionEnter2D(collision);
			}
		}
	}

	public override void OnCollisionExit2D(Collision2D collision)
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).OnCollisionExit2D(collision);
			}
		}
	}

	public override string OnDrawNodeText()
	{
		if (actions == null || !graphLabel)
		{
			return string.Empty;
		}
		string text = string.Empty;
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text += ((object)actions[i]).GetType().Name;
			}
		}
		return text;
	}

	public override void OnReset()
	{
		if (actions == null)
		{
			return;
		}
		for (int i = 0; i < actions.Length; i++)
		{
			if (actions[i] != null)
			{
				((Task)actions[i]).OnReset();
			}
		}
	}
}
