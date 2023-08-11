using UnityEngine;

public class RandomAnimationPlayTiming : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private AnimationClip _animation;

	private void Awake()
	{
		_animator.Play(((Object)_animation).name, 0, Random.Range(0f, 1f));
	}

	private void OnDestroy()
	{
		_animator = null;
		_animation = null;
	}
}
