using Level;
using UnityEngine;

namespace Characters.Movements;

[RequireComponent(typeof(CharacterController2D))]
public class DynamicPlatformAttacher : MonoBehaviour
{
	private CharacterController2D _controller;

	private void Awake()
	{
		_controller = ((Component)this).GetComponent<CharacterController2D>();
		_controller.collisionState.belowCollisionDetector.OnEnter += OnBelowCollisionEnter;
		_controller.collisionState.belowCollisionDetector.OnExit += OnBelowCollisionExit;
	}

	private void OnDestroy()
	{
		_controller.collisionState.belowCollisionDetector.OnEnter -= OnBelowCollisionEnter;
		_controller.collisionState.belowCollisionDetector.OnExit -= OnBelowCollisionExit;
	}

	private void OnBelowCollisionEnter(RaycastHit2D hit)
	{
		DynamicPlatform component = ((Component)((RaycastHit2D)(ref hit)).collider).GetComponent<DynamicPlatform>();
		if (!((Object)(object)component == (Object)null))
		{
			component.Attach(_controller);
		}
	}

	private void OnBelowCollisionExit(RaycastHit2D hit)
	{
		if (!((Object)(object)((RaycastHit2D)(ref hit)).collider == (Object)null))
		{
			DynamicPlatform component = ((Component)((RaycastHit2D)(ref hit)).collider).GetComponent<DynamicPlatform>();
			if (!((Object)(object)component == (Object)null))
			{
				component.Detach(_controller);
			}
		}
	}
}
