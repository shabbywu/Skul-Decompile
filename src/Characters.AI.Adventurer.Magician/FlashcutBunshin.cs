using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Adventurer.Magician;

public class FlashcutBunshin : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private GameObject _body;

	[SerializeField]
	private Action _action;

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CAttack());
	}

	private IEnumerator CAttack()
	{
		Collider2D lastStandingCollider = _character.movement.controller.collisionState.lastStandingCollider;
		while ((Object)(object)lastStandingCollider == (Object)null)
		{
			yield return null;
			lastStandingCollider = _character.movement.controller.collisionState.lastStandingCollider;
		}
		Character character = _character;
		Bounds bounds = lastStandingCollider.bounds;
		character.ForceToLookAt(((Bounds)(ref bounds)).center.x);
		_action.TryStart();
		while (_action.running)
		{
			yield return null;
		}
		((Component)this).gameObject.SetActive(false);
	}
}
