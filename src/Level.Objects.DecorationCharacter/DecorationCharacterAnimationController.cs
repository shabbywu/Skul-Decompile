using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level.Objects.DecorationCharacter;

public class DecorationCharacterAnimationController : MonoBehaviour
{
	public readonly CharacterAnimationController.Parameter parameter = new CharacterAnimationController.Parameter();

	private List<CharacterAnimation> _animations = new List<CharacterAnimation>();

	private CharacterChronometer _chronometer;

	private void Update()
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _animations.Count; i++)
		{
			CharacterAnimation characterAnimation = _animations[i];
			characterAnimation.speed = _chronometer.animation.timeScale / Time.timeScale;
			((AnimatorVariable<bool>)(object)characterAnimation.parameter.walk).Value = parameter.walk;
			((AnimatorVariable<bool>)(object)characterAnimation.parameter.grounded).Value = parameter.grounded;
			((AnimatorVariable<float>)(object)characterAnimation.parameter.movementSpeed).Value = parameter.movementSpeed;
			((AnimatorVariable<float>)(object)characterAnimation.parameter.ySpeed).Value = parameter.ySpeed;
			((Component)characterAnimation).transform.localScale = (Vector3)(parameter.flipX ? new Vector3(-1f, 1f, 1f) : Vector3.one);
		}
	}

	public void Initialize(CharacterChronometer chronometer)
	{
		((Component)this).GetComponentsInChildren<CharacterAnimation>(true, _animations);
		_animations.ForEach(delegate(CharacterAnimation animation)
		{
			animation.Initialize();
		});
		_chronometer = chronometer;
	}
}
