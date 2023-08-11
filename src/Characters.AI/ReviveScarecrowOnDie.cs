using System.Collections;
using UnityEngine;

namespace Characters.AI;

public class ReviveScarecrowOnDie : MonoBehaviour
{
	[SerializeField]
	private Character[] _target;

	[SerializeField]
	private Character _origin;

	private void Start()
	{
		Character[] target = _target;
		foreach (Character character in target)
		{
			character.health.onDied += delegate
			{
				((MonoBehaviour)this).StartCoroutine(Revive(character));
			};
		}
	}

	private IEnumerator Revive(Character target)
	{
		yield return Chronometer.global.WaitForSeconds(3f);
		Character spawned = Object.Instantiate<Character>(_origin, ((Component)target).transform.position, Quaternion.identity, ((Component)this).transform);
		ScareCrawAI componentInChildren = ((Component)spawned).GetComponentInChildren<ScareCrawAI>();
		spawned.ForceToLookAt(target.lookingDirection);
		((Component)spawned).gameObject.SetActive(true);
		spawned.health.onDied += delegate
		{
			((MonoBehaviour)this).StartCoroutine(Revive(spawned));
		};
		componentInChildren.Appear();
		Object.Destroy((Object)(object)((Component)target).gameObject);
	}
}
