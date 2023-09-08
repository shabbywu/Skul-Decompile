using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects;

public abstract class QuintessenceEffect : MonoBehaviour
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(allowCustom: true, QuintessenceEffect.types)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<QuintessenceEffect>
	{
		public void Invoke(Quintessence quintessence)
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Invoke(quintessence);
			}
		}
	}

	public static readonly Type[] types = new Type[4]
	{
		typeof(AttachAbility),
		typeof(AttachAbilityToTargetOnGaveStatus),
		typeof(RunOperations),
		typeof(RunAction)
	};

	public void Invoke(Quintessence quintessence)
	{
		OnInvoke(quintessence);
	}

	protected abstract void OnInvoke(Quintessence quintessence);
}
