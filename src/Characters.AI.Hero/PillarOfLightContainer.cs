using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Hero;

public class PillarOfLightContainer : MonoBehaviour
{
	[SerializeField]
	private Transform _container;

	private List<PillarOfLight> _pillars;

	private void Start()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		_pillars = new List<PillarOfLight>(_container.childCount);
		foreach (Transform item in _container)
		{
			PillarOfLight component = ((Component)item).GetComponent<PillarOfLight>();
			if ((Object)(object)component == (Object)null)
			{
				Debug.LogError((object)"child has not Pillar Of Light");
			}
			_pillars.Add(component);
		}
	}

	public void AddPillar(PillarOfLight pillar)
	{
		((Component)pillar).transform.SetParent(_container);
	}

	public void Sign(Character owner)
	{
		foreach (PillarOfLight pillar in _pillars)
		{
			pillar.Sign(owner);
		}
	}

	public void Attack(Character owner)
	{
		foreach (PillarOfLight pillar in _pillars)
		{
			pillar.Attack(owner);
		}
	}
}
