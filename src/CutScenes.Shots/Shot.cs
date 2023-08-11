using System;
using UnityEditor;
using UnityEngine;

namespace CutScenes.Shots;

public abstract class Shot : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	public static Type[] types = new Type[2]
	{
		typeof(EventInfos),
		typeof(SequenceInfos)
	};

	public abstract void Run();

	public abstract void SetNext(Shot next);
}
