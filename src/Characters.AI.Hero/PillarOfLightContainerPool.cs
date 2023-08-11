using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Hero;

public class PillarOfLightContainerPool : MonoBehaviour
{
	[SerializeField]
	private Transform _container;

	[SerializeField]
	private float _delay;

	private List<PillarOfLightContainer> _pool;

	private void Start()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		_pool = new List<PillarOfLightContainer>(_container.childCount);
		foreach (Transform item in _container)
		{
			Transform val = item;
			_pool.Add(((Component)val).GetComponent<PillarOfLightContainer>());
		}
	}

	public void Run(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CRun(owner));
	}

	private IEnumerator CRun(Character owner)
	{
		PillarOfLightContainer selected = ExtensionMethods.Random<PillarOfLightContainer>((IEnumerable<PillarOfLightContainer>)_pool);
		((Component)selected).gameObject.SetActive(true);
		selected.Sign(owner);
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.master, _delay);
		selected.Attack(owner);
	}
}
