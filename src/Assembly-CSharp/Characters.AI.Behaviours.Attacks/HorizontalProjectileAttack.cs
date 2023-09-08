using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours.Attacks;

public class HorizontalProjectileAttack : ActionAttack
{
	[SerializeField]
	private Transform _weapon;

	private Vector3 _originalScale;

	private float _originalDircetion;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		_originalScale = ((Component)this).transform.localScale;
		Quaternion rotation = ((Component)this).transform.rotation;
		_originalDircetion = ((Quaternion)(ref rotation)).eulerAngles.z;
	}

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		base.result = Result.Doing;
		Vector3 val = ((Component)controller.target).transform.position - ((Component)character).transform.position;
		_ = val.x;
		_ = 0f;
		if (val.x < 0f)
		{
			character.lookingDirection = Character.LookingDirection.Left;
			Vector3 originalScale = _originalScale;
			originalScale.y *= -1f;
			_weapon.localScale = originalScale * -1f;
		}
		else
		{
			character.lookingDirection = Character.LookingDirection.Right;
			_weapon.localScale = _originalScale;
		}
		if (attack.TryStart())
		{
			gaveDamage = false;
			yield return attack.CWaitForEndOfRunning();
			if (!gaveDamage)
			{
				base.result = Result.Success;
				yield return character.chronometer.animation.WaitForSeconds(1.5f);
			}
			else
			{
				base.result = Result.Fail;
			}
		}
		else
		{
			base.result = Result.Fail;
		}
	}
}
