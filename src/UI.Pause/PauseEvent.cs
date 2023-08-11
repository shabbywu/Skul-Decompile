using System;
using UnityEditor;
using UnityEngine;

namespace UI.Pause;

public abstract class PauseEvent : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	public static Type[] types = new Type[4]
	{
		typeof(PauseMenuPopUp),
		typeof(StorySkip),
		typeof(CreditExit),
		typeof(Empty)
	};

	public abstract void Invoke();
}
