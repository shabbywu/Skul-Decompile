using System;
using System.Collections;
using System.Globalization;
using Characters.Controllers;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using Scenes;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class EndingGameResult : MonoBehaviour
{
	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private AnimationCurve _curve;

	[SerializeField]
	private TextMeshProUGUI _playTime;

	[SerializeField]
	private TextMeshProUGUI _deaths;

	[SerializeField]
	private TextMeshProUGUI _kills;

	[SerializeField]
	private TextMeshProUGUI _eliteKills;

	[SerializeField]
	private TextMeshProUGUI _darkcites;

	[SerializeField]
	private TextMeshProUGUI _title;

	[SerializeField]
	private TextMeshProUGUI _subTitle;

	[SerializeField]
	private TextMeshProUGUI _yourEnemy;

	[SerializeField]
	private TextMeshProUGUI _stageName;

	[SerializeField]
	private Transform _gearListContainer;

	[SerializeField]
	private GearImageContainer _gearContainerPrefab;

	public bool animationFinished { get; private set; }

	private void OnEnable()
	{
		PlayerInput.blocked.Attach((object)this);
		Scene<GameBase>.instance.uiManager.pauseEventSystem.PushEmpty();
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0f);
		((MonoBehaviour)this).StartCoroutine(CAnimate());
		((TMP_Text)_playTime).text = new TimeSpan(0, 0, GameData.Progress.playTime).ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture);
		((TMP_Text)_deaths).text = GameData.Progress.deaths.ToString();
		((TMP_Text)_kills).text = GameData.Progress.kills.ToString();
		((TMP_Text)_eliteKills).text = GameData.Progress.eliteKills.ToString();
		((TMP_Text)_darkcites).text = GameData.Currency.darkQuartz.income.ToString();
		UpdateGearList();
	}

	private void OnDisable()
	{
		PlayerInput.blocked.Detach((object)this);
		Scene<GameBase>.instance.uiManager.pauseEventSystem.PopEvent();
		((ChronometerBase)Chronometer.global).DetachTimeScale((object)this);
	}

	private void UpdateGearList()
	{
		ExtensionMethods.Empty(_gearListContainer);
		Characters.Player.Inventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory;
		WeaponInventory weapon = inventory.weapon;
		QuintessenceInventory quintessence = inventory.quintessence;
		ItemInventory item = inventory.item;
		for (int i = 0; i < weapon.weapons.Length; i++)
		{
			Weapon weapon2 = weapon.weapons[i];
			if ((Object)(object)weapon2 != (Object)null)
			{
				GearImageContainer gearImageContainer = Object.Instantiate<GearImageContainer>(_gearContainerPrefab, _gearListContainer);
				gearImageContainer.image.sprite = weapon2.icon;
				((Graphic)gearImageContainer.image).SetNativeSize();
			}
		}
		for (int j = 0; j < quintessence.items.Count; j++)
		{
			Quintessence quintessence2 = quintessence.items[j];
			if ((Object)(object)quintessence2 != (Object)null)
			{
				GearImageContainer gearImageContainer2 = Object.Instantiate<GearImageContainer>(_gearContainerPrefab, _gearListContainer);
				gearImageContainer2.image.sprite = quintessence2.icon;
				((Graphic)gearImageContainer2.image).SetNativeSize();
			}
		}
		for (int k = 0; k < item.items.Count; k++)
		{
			Item item2 = item.items[k];
			if ((Object)(object)item2 != (Object)null)
			{
				GearImageContainer gearImageContainer3 = Object.Instantiate<GearImageContainer>(_gearContainerPrefab, _gearListContainer);
				gearImageContainer3.image.sprite = item2.icon;
				((Graphic)gearImageContainer3.image).SetNativeSize();
			}
		}
	}

	private IEnumerator CAnimate()
	{
		animationFinished = false;
		float time = 0f;
		Vector3 targetPosition = _container.transform.position;
		Vector3 position = targetPosition;
		position.y += 200f;
		for (; time < 1f; time += Time.unscaledDeltaTime)
		{
			_container.transform.position = Vector3.LerpUnclamped(position, targetPosition, _curve.Evaluate(time));
			yield return null;
		}
		_container.transform.position = targetPosition;
		animationFinished = true;
	}

	public void Show()
	{
		((Component)this).gameObject.SetActive(true);
	}

	public void Hide()
	{
		((Component)this).gameObject.SetActive(false);
	}
}
