using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.AI;
using UnityEngine;

namespace Level;

public class SpawnScarecrowOnLootActivate : MonoBehaviour
{
	[SerializeField]
	private MapReward _mapReward;

	[SerializeField]
	private GameObject _scareCrawPrefab;

	private List<ScareCrawAI> _scarCraws;

	private void Start()
	{
		_scarCraws = new List<ScareCrawAI>(((Component)this).transform.childCount);
		ScareCrawAI[] componentsInChildren = ((Component)this).GetComponentsInChildren<ScareCrawAI>();
		foreach (ScareCrawAI scareCraw in componentsInChildren)
		{
			scareCraw.character.health.onDied += delegate
			{
				((MonoBehaviour)this).StartCoroutine(Revive(scareCraw));
			};
			((Component)scareCraw.character).gameObject.SetActive(false);
			_scarCraws.Add(scareCraw);
		}
		_mapReward.onLoot += delegate
		{
			foreach (ScareCrawAI scarCraw in _scarCraws)
			{
				((Component)scarCraw.character).gameObject.SetActive(true);
				scarCraw.Appear();
			}
		};
	}

	private IEnumerator Revive(ScareCrawAI scareCraw)
	{
		yield return Chronometer.global.WaitForSeconds(3f);
		GameObject val = Object.Instantiate<GameObject>(_scareCrawPrefab, ((Component)scareCraw.character).transform.position, Quaternion.identity, ((Component)this).transform);
		Character component = val.GetComponent<Character>();
		ScareCrawAI scareCrawAI = val.GetComponentInChildren<ScareCrawAI>();
		component.ForceToLookAt(scareCraw.character.lookingDirection);
		((Component)component).gameObject.SetActive(true);
		component.health.onDied += delegate
		{
			((MonoBehaviour)this).StartCoroutine(Revive(scareCrawAI));
		};
		scareCrawAI.Appear();
		Object.Destroy((Object)(object)((Component)scareCraw).gameObject);
	}
}
