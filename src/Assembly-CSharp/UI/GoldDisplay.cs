using Data;
using TMPro;
using UnityEngine;

namespace UI;

public class GoldDisplay : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _amount;

	private void Update()
	{
		((TMP_Text)_amount).text = GameData.Currency.gold.balance.ToString();
	}
}
