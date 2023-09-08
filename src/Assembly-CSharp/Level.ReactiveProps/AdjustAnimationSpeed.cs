using UnityEngine;

namespace Level.ReactiveProps;

public class AdjustAnimationSpeed : MonoBehaviour
{
	[SerializeField]
	[MinMaxSlider(0f, 5f)]
	private Vector2 _animationSpeedRange;

	[SerializeField]
	private Animator[] _animators;

	private void Start()
	{
		for (int i = 0; i < _animators.Length; i++)
		{
			_animators[i].speed = Random.Range(_animationSpeedRange.x, _animationSpeedRange.y);
		}
	}
}
