using UnityEngine;

namespace CutScenes.Shots.Events;

public class PlayAnimation : Event
{
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private AnimationClip _animation;

	private void OnDestroy()
	{
		_animator = null;
		_animation = null;
	}

	public override void Run()
	{
		_animator.Play(((Object)_animation).name);
	}
}
