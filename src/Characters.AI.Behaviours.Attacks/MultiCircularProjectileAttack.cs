using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks;

public class MultiCircularProjectileAttack : ActionAttack
{
	private Vector3 _originalScale;

	[SerializeField]
	private Transform[] _centerAxisPositions;

	[SerializeField]
	private Transform _weaponAxisPosition;

	public Vector3 lookTarget { get; set; }

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_originalScale = Vector3.one;
	}

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		_ = lookTarget;
		Vector3 val = lookTarget;
		base.result = Result.Doing;
		character.ForceToLookAt(val.x);
		Vector3 val2 = val - ((Component)character).transform.position;
		float num = Mathf.Atan2(val2.y, val2.x) * 57.29578f;
		Vector3 originalScale = _originalScale;
		if ((num > 90f && num < 270f) || num < -90f)
		{
			originalScale.x *= -1f;
		}
		_weaponAxisPosition.localScale = originalScale;
		Transform[] centerAxisPositions = _centerAxisPositions;
		foreach (Transform obj in centerAxisPositions)
		{
			Vector3 originalScale2 = _originalScale;
			if ((num > 90f && num < 270f) || num < -90f)
			{
				originalScale2.y *= -1f;
				originalScale2.x *= -1f;
			}
			obj.localScale = originalScale2;
		}
		if (attack.TryStart())
		{
			while (attack.running)
			{
				yield return null;
			}
			yield return idle.CRun(controller);
		}
	}
}
