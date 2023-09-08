using Characters;
using UnityEngine;

namespace FX;

public class ToggleEffect : MonoBehaviour
{
	private enum Type
	{
		effect,
		master,
		projectile,
		animation
	}

	[SerializeField]
	[GetComponent]
	private Animator _animator;

	[SerializeField]
	private Type _chronometerType;

	private Chronometer _chronometer;

	private void Start()
	{
		Character componentInParent = ((Component)this).GetComponentInParent<Character>();
		if ((Object)(object)componentInParent != (Object)null)
		{
			switch (_chronometerType)
			{
			case Type.effect:
				_chronometer = componentInParent.chronometer.effect;
				break;
			case Type.master:
				_chronometer = componentInParent.chronometer.master;
				break;
			case Type.projectile:
				_chronometer = componentInParent.chronometer.projectile;
				break;
			case Type.animation:
				_chronometer = componentInParent.chronometer.animation;
				break;
			default:
				_chronometer = componentInParent.chronometer.effect;
				break;
			}
		}
	}

	private void Update()
	{
		_animator.speed = ((_chronometer == null) ? Chronometer.global.timeScale : _chronometer.timeScale) / Time.timeScale;
	}
}
