using System.Collections;
using System.Text;
using Characters.Gear.Synergy.Inscriptions;
using Characters.Player;
using Services;
using Singletons;
using TMPro;
using UnityEngine;

public class SynergyDisplay : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _text;

	private Inventory _inventory;

	private readonly StringBuilder _stringBuilder = new StringBuilder();

	private IEnumerator Start()
	{
		while ((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null)
		{
			yield return null;
		}
		_inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory;
		_inventory.onUpdated += UpdateText;
		UpdateText();
	}

	private void UpdateText()
	{
		EnumArray<Inscription.Key, Inscription> inscriptions = _inventory.synergy.inscriptions;
		_stringBuilder.Clear();
		foreach (Inscription item in inscriptions)
		{
			if (item.count != 0)
			{
				_stringBuilder.AppendFormat("{0} ({1})\n", item.name, item.count);
				if (item.step != 0)
				{
					_stringBuilder.AppendLine();
				}
			}
		}
		_text.text = _stringBuilder.ToString();
	}
}
