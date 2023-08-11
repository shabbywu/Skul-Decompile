using Platforms;
using TMPro;
using UnityEngine;

namespace UI;

public class PlatformLoaderMessageDisplay : MonoBehaviour
{
	[SerializeField]
	private PlatformLoader _platformLoader;

	[SerializeField]
	private TMP_Text _text;

	private void Update()
	{
		if ((Object)(object)_platformLoader == (Object)null)
		{
			Object.Destroy((Object)(object)this);
		}
		else
		{
			_text.text = _platformLoader.message;
		}
	}
}
