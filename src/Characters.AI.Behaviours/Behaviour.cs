using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public abstract class Behaviour : MonoBehaviour
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public static readonly Type[] types = new Type[15]
		{
			typeof(Selector),
			typeof(Sequence),
			typeof(Height),
			typeof(Conditional),
			typeof(Count),
			typeof(Chance),
			typeof(CoolTime),
			typeof(UniformSelector),
			typeof(WeightedSelector),
			typeof(Repeat),
			typeof(RandomBehaviour),
			typeof(InfiniteLoop),
			typeof(Idle),
			typeof(SkipableIdle),
			typeof(TimeLoop)
		};

		public SubcomponentAttribute(bool allowCustom = true)
			: base(allowCustom, types)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<Behaviour>
	{
	}

	public enum Result
	{
		Fail,
		Doing,
		Success,
		Done
	}

	public Action onStart;

	public Action onEnd;

	protected List<Behaviour> _childs;

	public Result result { get; set; }

	private void Stop()
	{
		if (result.Equals(Result.Doing))
		{
			result = Result.Done;
		}
	}

	protected IEnumerator CExpire(AIController controller, Vector2 durationMinMax)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float num = Random.Range(durationMinMax.x, durationMinMax.y);
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)controller.character.chronometer.master, num);
		Stop();
	}

	protected IEnumerator CExpire(AIController controller, float duration)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)controller.character.chronometer.master, duration);
		Stop();
	}

	public abstract IEnumerator CRun(AIController controller);

	public void StopPropagation()
	{
		result = Result.Done;
		if (_childs == null)
		{
			return;
		}
		foreach (Behaviour child in _childs)
		{
			child?.StopPropagation();
		}
	}

	public override string ToString()
	{
		return ExtensionMethods.GetAutoName((object)this);
	}
}
