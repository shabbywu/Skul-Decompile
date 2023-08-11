using System.Collections.Generic;
using Characters;
using Characters.Abilities.Blessing;
using Runnables;
using UnityEngine;

namespace CutScenes.Shots.Events.Customs;

public sealed class ApplyBlessing : Event
{
	[SerializeField]
	private TextKeyCache _nameKeyCache;

	[SerializeField]
	private TextKeyCache _chatKeyCache;

	[SerializeField]
	private Runnables.Target _target;

	[SerializeField]
	private Blessing[] _blessings;

	[SerializeField]
	private Animator _holyGrailAnimator;

	public override void Run()
	{
		Character character = _target.character;
		Blessing blessing = Object.Instantiate<Blessing>(ExtensionMethods.Random<Blessing>((IEnumerable<Blessing>)_blessings));
		blessing.Apply(character);
		_holyGrailAnimator.Play(((Object)blessing.clip).name);
		_nameKeyCache.key = blessing.activatedNameKey;
		_chatKeyCache.key = blessing.activatedChatKey;
	}
}
