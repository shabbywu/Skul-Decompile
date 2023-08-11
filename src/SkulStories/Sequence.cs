using System;
using System.Collections;
using Scenes;
using UnityEditor;
using UnityEngine;

namespace SkulStories;

public abstract class Sequence : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	public static readonly Type[] types = new Type[9]
	{
		typeof(PlaceText),
		typeof(FadeInOut),
		typeof(ShowTexts),
		typeof(PlayNarration),
		typeof(WaitInput),
		typeof(ChangeScene),
		typeof(ScrollImage),
		typeof(RunCutSceneSequence),
		typeof(RunOperation)
	};

	[SerializeField]
	private bool _wait;

	protected Narration _narration;

	private void Awake()
	{
		_narration = Scene<GameBase>.instance.uiManager.narration;
	}

	public IEnumerator CCheckWait()
	{
		if (_wait && !_narration.skipped)
		{
			yield return CRun();
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CRun());
		}
	}

	public abstract IEnumerator CRun();
}
