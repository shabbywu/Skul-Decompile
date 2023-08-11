using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks;

public class CircularProjectileAttack : ActionAttack
{
	private Vector3 _originalScale;

	private float _originalDirection;

	[SerializeField]
	private Transform _centerAxisPosition;

	[SerializeField]
	private Transform _weaponAxisPosition;

	[SerializeField]
	private bool _continuousLooking;

	[SerializeField]
	private bool _autoAim;

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_originalScale = Vector3.one;
	}

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		Character target = controller.target;
		base.result = Result.Doing;
		Vector3 val = ((Component)target).transform.position - ((Component)character).transform.position;
		character.lookingDirection = ((!(val.x > 0f)) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		Vector3 val2 = ((Component)target).transform.position - ((Component)character).transform.position;
		float num = Mathf.Atan2(val2.y, val2.x) * 57.29578f;
		Vector3 originalScale = _originalScale;
		Vector3 originalScale2 = _originalScale;
		if ((num > 90f && num < 270f) || num < -90f)
		{
			originalScale.x *= -1f;
			originalScale2.y *= -1f;
			originalScale2.x *= -1f;
		}
		_weaponAxisPosition.localScale = originalScale;
		_centerAxisPosition.localScale = originalScale2;
		if (_autoAim)
		{
			yield return TakeAim(character, target);
		}
		if (!attack.TryStart())
		{
			yield break;
		}
		while (attack.running)
		{
			yield return null;
			if (_continuousLooking)
			{
				val = ((Component)target).transform.position - ((Component)character).transform.position;
				character.lookingDirection = ((!(val.x > 0f)) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
				val2 = ((Component)target).transform.position - ((Component)character).transform.position;
				num = Mathf.Atan2(val2.y, val2.x) * 57.29578f;
				originalScale = _originalScale;
				originalScale2 = _originalScale;
				if ((num > 90f && num < 270f) || num < -90f)
				{
					originalScale.x *= -1f;
					originalScale2.y *= -1f;
					originalScale2.x *= -1f;
				}
				_weaponAxisPosition.localScale = originalScale;
				_centerAxisPosition.localScale = originalScale2;
			}
		}
		yield return idle.CRun(controller);
	}

	private IEnumerator TakeAim(Character character, Character target)
	{
		while (attack.running)
		{
			Vector3 val = ((Component)target).transform.position - ((Component)character).transform.position;
			float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
			_centerAxisPosition.rotation = Quaternion.Euler(0f, 0f, _originalDirection + num);
			yield return null;
		}
	}
}
