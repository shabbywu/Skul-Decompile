using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}StackedConditionalIcon.png")]
[TaskDescription("Allows multiple conditional tasks to be added to a single node.")]
public class StackedConditional : Conditional
{
	public enum ComparisonType
	{
		Sequence,
		Selector
	}

	[InspectTask]
	public Conditional[] conditionals;

	[Tooltip("Specifies if the tasks should be traversed with an AND (Sequence) or an OR (Selector).")]
	public ComparisonType comparisonType;

	[Tooltip("Should the tasks be labeled within the graph?")]
	public bool graphLabel;

	public override void OnAwake()
	{
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).GameObject = ((Task)this).gameObject;
				((Task)conditionals[i]).Transform = ((Task)this).transform;
				((Task)conditionals[i]).Owner = ((Task)this).Owner;
				((Task)conditionals[i]).OnAwake();
			}
		}
	}

	public override void OnStart()
	{
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).OnStart();
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
		if (conditionals != null)
		{
			for (int i = 0; i < conditionals.Length; i++)
			{
				if (conditionals[i] != null)
				{
					TaskStatus val = ((Task)conditionals[i]).OnUpdate();
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
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).OnFixedUpdate();
			}
		}
	}

	public override void OnLateUpdate()
	{
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).OnLateUpdate();
			}
		}
	}

	public override void OnEnd()
	{
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).OnEnd();
			}
		}
	}

	public override void OnTriggerEnter2D(Collider2D other)
	{
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).OnTriggerEnter2D(other);
			}
		}
	}

	public override void OnTriggerExit2D(Collider2D other)
	{
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).OnTriggerExit2D(other);
			}
		}
	}

	public override void OnCollisionEnter2D(Collision2D collision)
	{
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).OnCollisionEnter2D(collision);
			}
		}
	}

	public override void OnCollisionExit2D(Collision2D collision)
	{
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).OnCollisionExit2D(collision);
			}
		}
	}

	public override string OnDrawNodeText()
	{
		if (conditionals == null || !graphLabel)
		{
			return string.Empty;
		}
		string text = string.Empty;
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += "\n";
				}
				text += ((object)conditionals[i]).GetType().Name;
			}
		}
		return text;
	}

	public override void OnReset()
	{
		if (conditionals == null)
		{
			return;
		}
		for (int i = 0; i < conditionals.Length; i++)
		{
			if (conditionals[i] != null)
			{
				((Task)conditionals[i]).OnReset();
			}
		}
	}
}
