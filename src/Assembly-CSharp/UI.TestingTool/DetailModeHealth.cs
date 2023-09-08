using Characters;
using TMPro;
using UnityEngine;

namespace UI.TestingTool;

public sealed class DetailModeHealth : MonoBehaviour
{
	[SerializeField]
	private TMP_Text _text;

	private Character _owner;

	private float _healthCache;

	public void Initialize(Character owner)
	{
		_owner = owner;
	}

	private void Update()
	{
		if (!((Object)(object)_owner == (Object)null))
		{
			float num = (float)_owner.health.currentHealth;
			if (!Mathf.Approximately(_healthCache, num))
			{
				_healthCache = num;
				_text.text = num.ToString();
			}
		}
	}
}
