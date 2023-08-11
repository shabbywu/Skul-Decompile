using UnityEngine;

namespace Characters.Gear.Synergy;

[CreateAssetMenu]
public class SynergySettings : ScriptableObject
{
	[SerializeField]
	private InscriptionSettingsByKey _settings;

	public InscriptionSettingsByKey settings => _settings;
}
