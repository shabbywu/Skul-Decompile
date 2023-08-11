using System;
using Runnables.Cost;
using UnityEditor;
using UnityEngine;

namespace Runnables;

public abstract class CurrencyAmount : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	public static readonly Type[] types = new Type[2]
	{
		typeof(CostEvent),
		typeof(Constant)
	};

	public abstract int GetAmount();
}
