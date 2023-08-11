using Characters.Gear;
using Services;
using Singletons;
using UnityEngine;

namespace Characters;

public class DropGearOnDie : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Character _character;

	[SerializeField]
	private Characters.Gear.Gear _gear;

	[SerializeField]
	[Range(0f, 100f)]
	private int _chance;

	private void Awake()
	{
		_character.health.onDie += OnDie;
	}

	private void OnDie()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!_character.health.dead && MMMaths.PercentChance(_chance))
		{
			Singleton<Service>.Instance.levelManager.DropGear(_gear, ((Component)this).transform.position);
		}
	}
}
