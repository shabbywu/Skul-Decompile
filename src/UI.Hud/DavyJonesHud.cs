using System;
using Characters.Abilities.Weapons.DavyJones;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud;

public sealed class DavyJonesHud : MonoBehaviour
{
	[SerializeField]
	private GameObject _container;

	[Header("탄환")]
	[SerializeField]
	private Image[] _cannonImages;

	[SerializeField]
	private float[] _hueValue;

	[SerializeField]
	private EnumArray<CannonBallType, Sprite> _sprites;

	[SerializeField]
	private Animator[] _effects;

	[SerializeField]
	private EnumArray<CannonBallType, RuntimeAnimatorController> _effectAnimators;

	public void ShowHUD()
	{
		_container.gameObject.SetActive(true);
	}

	public void HideAll()
	{
		_container.gameObject.SetActive(false);
		HideAllCannonBall();
	}

	public void HideAllCannonBall()
	{
		Image[] cannonImages = _cannonImages;
		for (int i = 0; i < cannonImages.Length; i++)
		{
			((Component)cannonImages[i]).gameObject.SetActive(false);
		}
		Animator[] effects = _effects;
		for (int i = 0; i < effects.Length; i++)
		{
			((Component)effects[i]).gameObject.SetActive(false);
		}
	}

	public void SetCannonBall(int index, CannonBallType type)
	{
		_cannonImages[index].sprite = _sprites[type];
		((Component)_cannonImages[index]).gameObject.SetActive(true);
		if (Convert.ToInt32(type) % 2 == 1)
		{
			((Component)_effects[index]).gameObject.SetActive(true);
			_effects[index].runtimeAnimatorController = _effectAnimators[type];
		}
	}
}
