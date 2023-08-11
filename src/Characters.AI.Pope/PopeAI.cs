using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope;

public sealed class PopeAI : AIController
{
	[Serializable]
	private class Animation
	{
		[SerializeField]
		private AnimationClip _phase2IdleClip;

		[SerializeField]
		private CharacterAnimation _characterAnimation;

		public void SetNextClip()
		{
			if ((Object)(object)_characterAnimation == (Object)null)
			{
				Debug.LogError((object)"Character Animation is null");
				return;
			}
			_characterAnimation.SetIdle(_phase2IdleClip);
			_characterAnimation.SetWalk(_phase2IdleClip);
		}

		public void Dispose()
		{
			_phase2IdleClip = null;
			_characterAnimation = null;
		}
	}

	[SerializeField]
	private Animation _idle;

	[SerializeField]
	[Subcomponent(typeof(Sequence))]
	private Sequence _sequence;

	private IEnumerator _sequenceCoroutine;

	private new void OnEnable()
	{
		base.OnEnable();
	}

	private void OnDestroy()
	{
		_idle.Dispose();
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		while (true)
		{
			_sequenceCoroutine = _sequence.CRun(this);
			yield return _sequenceCoroutine;
			yield return Chronometer.global.WaitForSeconds(1f);
		}
	}

	public void StartCombat()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	public void NextPhase()
	{
		((MonoBehaviour)this).StopAllCoroutines();
		_idle.SetNextClip();
		_sequence.NextPhase();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(CCheckStun());
	}
}
