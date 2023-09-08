using System;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using UnityEngine;

namespace Characters.Gear.Synergy;

public class Synergy : MonoBehaviour
{
	public readonly EnumArray<Inscription.Key, Inscription> inscriptions = new EnumArray<Inscription.Key, Inscription>();

	[SerializeField]
	private SynergySettings _synergySettings;

	[SerializeField]
	private GameObject _container;

	public event Action onChanged;

	public void Initialize(Character character)
	{
		for (int i = 0; i < inscriptions.Count; i++)
		{
			inscriptions.Array[i] = new Inscription();
			ref InscriptionSettings reference = ref _synergySettings.settings.Array[i];
			inscriptions.Array[i].Initialize(inscriptions.Keys[i], reference, character);
		}
	}

	public void UpdateBonus()
	{
		for (int i = 0; i < inscriptions.Count; i++)
		{
			inscriptions.Array[i].Update();
		}
		this.onChanged?.Invoke();
	}

	private void OnDestroy()
	{
		if (Service.quitting)
		{
			return;
		}
		foreach (Inscription inscription in inscriptions)
		{
			inscription?.Clear();
		}
	}
}
