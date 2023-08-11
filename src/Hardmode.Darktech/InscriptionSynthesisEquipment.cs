using Characters.Gear.Synergy.Inscriptions;
using Data;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class InscriptionSynthesisEquipment : MonoBehaviour
{
	public static readonly int increasement = 1;

	[SerializeField]
	private InscriptionSynthesisEquipmentSlot[] _slots;

	private void Start()
	{
		int num = 0;
		InscriptionSynthesisEquipmentSlot[] slots = _slots;
		for (int i = 0; i < slots.Length; i++)
		{
			slots[i].Initialize(this, num);
			if (num < GameData.HardmodeProgress.InscriptionSynthesisEquipment.count)
			{
				num++;
				continue;
			}
			break;
		}
	}

	public bool IsSelectable(InscriptionSynthesisEquipmentSlot from, Inscription.Key key)
	{
		if (key == Inscription.Key.SunAndMoon || key == Inscription.Key.Masterpiece || key == Inscription.Key.Sin || key == Inscription.Key.Omen)
		{
			return false;
		}
		InscriptionSynthesisEquipmentSlot[] slots = _slots;
		foreach (InscriptionSynthesisEquipmentSlot inscriptionSynthesisEquipmentSlot in slots)
		{
			if (!((Object)(object)inscriptionSynthesisEquipmentSlot == (Object)(object)from) && inscriptionSynthesisEquipmentSlot.selected.HasValue && inscriptionSynthesisEquipmentSlot.selected.Value == key)
			{
				return false;
			}
		}
		return true;
	}
}
