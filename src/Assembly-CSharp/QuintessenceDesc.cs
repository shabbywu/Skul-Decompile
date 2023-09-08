using GameResources;
using UnityEngine;
using UnityEngine.UI;

public class QuintessenceDesc : MonoBehaviour
{
	[SerializeField]
	private Text _effectOfUse;

	[SerializeField]
	private Text _description;

	public string text
	{
		get
		{
			return _description.text;
		}
		set
		{
			_description.text = value;
		}
	}

	private void Awake()
	{
		_effectOfUse.text = Localization.GetLocalizedString("quintessence_effectOfUse");
	}
}
