using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Adventurer.Magician;

public class ShurikenBunshin : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private GameObject _base;

	[SerializeField]
	private Action _jump;

	[SerializeField]
	private Action _attack;

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
		_jump.TryStart();
		while (_character.movement.velocity.y > 0f)
		{
			yield return null;
		}
		while (_jump.running)
		{
			yield return null;
		}
		_attack.TryStart();
		while (_attack.running)
		{
			yield return null;
		}
		((Component)this).gameObject.SetActive(false);
	}
}
