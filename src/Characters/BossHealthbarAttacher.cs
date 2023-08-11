using Scenes;
using UI;
using UnityEngine;

namespace Characters;

public class BossHealthbarAttacher : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Character _character;

	[SerializeField]
	private BossHealthbarController.Type _type;

	private void Start()
	{
		if ((Object)(object)_character != (Object)null)
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.Open(_type, _character);
		}
	}
}
