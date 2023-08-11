using System;
using UnityEditor;
using UnityEngine;

namespace Runnables.Chances;

public abstract class Chance : MonoBehaviour
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
		typeof(Constant),
		typeof(ByValueComponent)
	};

	public abstract bool IsTrue();
}
