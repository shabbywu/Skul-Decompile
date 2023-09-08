using UnityEngine;

namespace FX;

public class UpdateAnimatorOnEnable : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private CustomFloat _deltaTime;

	private void OnEnable()
	{
		_animator.Update(_deltaTime.value);
	}
}
