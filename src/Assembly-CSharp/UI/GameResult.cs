using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using Characters.Controllers;
using Characters.Gear.Items;
using Characters.Gear.Quintessences;
using Characters.Gear.Upgrades;
using Characters.Gear.Weapons;
using Characters.Player;
using Data;
using GameResources;
using Hardmode;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class GameResult : MonoBehaviour
{
	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private Image _panel;

	[SerializeField]
	private Sprite _hardmodePanel;

	[SerializeField]
	private Sprite _normalmodePanel;

	[SerializeField]
	private AnimationCurve _curve;

	[Header("Left Info")]
	[SerializeField]
	private TextMeshProUGUI _playTime;

	[SerializeField]
	private TextMeshProUGUI _deaths;

	[SerializeField]
	private TextMeshProUGUI _kills;

	[SerializeField]
	private TextMeshProUGUI _darkQuartz;

	[SerializeField]
	private TextMeshProUGUI _gold;

	[SerializeField]
	private TextMeshProUGUI _bone;

	[SerializeField]
	private TextMeshProUGUI _totalDamage;

	[SerializeField]
	private TextMeshProUGUI _totalTakingDamage;

	[SerializeField]
	private TextMeshProUGUI _bestDamage;

	[SerializeField]
	private TextMeshProUGUI _totalHeal;

	[SerializeField]
	private TextMeshProUGUI _encounterWeapon;

	[SerializeField]
	private TextMeshProUGUI _encounterItem;

	[SerializeField]
	private TextMeshProUGUI _encounterEssence;

	[Header("Right Info")]
	[SerializeField]
	private TextMeshProUGUI _title;

	[SerializeField]
	private TextMeshProUGUI _subTitle;

	[SerializeField]
	private Image _yourEnemy;

	[SerializeField]
	private Sprite _yourEnemyInNormal;

	[SerializeField]
	private Sprite _yourEnemyInHard;

	[SerializeField]
	private TextMeshProUGUI _stageName;

	[SerializeField]
	private RawImage _deathCam;

	[SerializeField]
	private GameObject _endingPortrait;

	[SerializeField]
	private Transform _gearListContainer;

	[SerializeField]
	private GearImageContainer _gearContainerPrefab;

	[SerializeField]
	private Transform _upgradeListListContainer;

	[SerializeField]
	private GearImageContainer _upgradeContainerPrefab;

	private readonly string _titleKey = "label/gameResult/title";

	private readonly string _normalSubTitleKey = "label/gameResult/subTitle";

	private readonly string _hardSubTitleKey = "label/gameResult/hardmode/subTitle";

	private readonly string _endingTitleKey = "label/gameResult/ending/title";

	private readonly string _endingSubTitleKey = "label/gameResult/ending/subTitle";

	private readonly string _endingStageNameKey = "label/gameResult/ending/stageName";

	private readonly string _hardmodeEndingFirstSubTitleKey = "label/gameResult/ending/hardmode/firstSubTitle";

	private readonly string _hardmodeEndingSubTitleKey = "label/gameResult/ending/hardmode/subTitle";

	public bool animationFinished { get; private set; }

	private void OnEnable()
	{
		PlayerInput.blocked.Attach(this);
		Chronometer.global.AttachTimeScale(this, 0f);
		((MonoBehaviour)this).StartCoroutine(CAnimate());
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			_panel.sprite = _hardmodePanel;
			_yourEnemy.sprite = _yourEnemyInHard;
		}
		else
		{
			_panel.sprite = _normalmodePanel;
			_yourEnemy.sprite = _yourEnemyInNormal;
		}
		((TMP_Text)_playTime).text = new TimeSpan(0, 0, GameData.Progress.playTime).ToString("hh\\:mm\\:ss", CultureInfo.InvariantCulture);
		((TMP_Text)_deaths).text = GameData.Progress.deaths.ToString();
		((TMP_Text)_kills).text = GameData.Progress.kills.ToString();
		((TMP_Text)_darkQuartz).text = GameData.Currency.darkQuartz.income.ToString();
		((TMP_Text)_gold).text = GameData.Currency.gold.income.ToString();
		((TMP_Text)_bone).text = GameData.Currency.bone.income.ToString();
		((TMP_Text)_totalDamage).text = GameData.Progress.totalDamage.ToString();
		((TMP_Text)_totalTakingDamage).text = GameData.Progress.totalTakingDamage.ToString();
		((TMP_Text)_bestDamage).text = GameData.Progress.bestDamage.ToString();
		((TMP_Text)_totalHeal).text = GameData.Progress.totalHeal.ToString();
		Characters.Player.Inventory inventory = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory;
		int encounterWeaponCount = GameData.Progress.encounterWeaponCount;
		WeaponInventory weapon = inventory.weapon;
		encounterWeaponCount += weapon.weapons.Count((Weapon element) => (Object)(object)element != (Object)null);
		((TMP_Text)_encounterWeapon).text = encounterWeaponCount.ToString();
		encounterWeaponCount = GameData.Progress.encounterItemCount;
		ItemInventory item = inventory.item;
		encounterWeaponCount += item.items.Count((Item element) => (Object)(object)element != (Object)null);
		((TMP_Text)_encounterItem).text = encounterWeaponCount.ToString();
		encounterWeaponCount = GameData.Progress.encounterItemCount;
		QuintessenceInventory quintessence = inventory.quintessence;
		encounterWeaponCount += quintessence.items.Count((Quintessence element) => (Object)(object)element != (Object)null);
		((TMP_Text)_encounterEssence).text = encounterWeaponCount.ToString();
		UpdateGearList();
		_deathCam.texture = (Texture)(object)CommonResource.instance.deathCamRenderTexture;
	}

	private void OnDisable()
	{
		PlayerInput.blocked.Detach(this);
		Chronometer.global.DetachTimeScale(this);
		_deathCam.texture = null;
	}

	private void UpdateGearList()
	{
		_gearListContainer.Empty();
		_upgradeListListContainer.Empty();
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
		if (!Singleton<HardmodeManager>.Instance.hardmode)
		{
			return;
		}
		UpgradeInventory upgrade = inventory.upgrade;
		for (int l = 0; l < upgrade.upgrades.Count; l++)
		{
			UpgradeObject upgradeObject = upgrade.upgrades[l];
			if ((Object)(object)upgradeObject != (Object)null)
			{
				GearImageContainer gearImageContainer4 = Object.Instantiate<GearImageContainer>(_upgradeContainerPrefab, _upgradeListListContainer);
				gearImageContainer4.image.sprite = upgradeObject.icon;
				((Graphic)gearImageContainer4.image).SetNativeSize();
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

	public void ShowEndResult()
	{
		((Component)this).gameObject.SetActive(true);
	}

	public void Show()
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			string localizedString = Localization.GetLocalizedString(_hardSubTitleKey);
			((TMP_Text)_subTitle).text = string.Format(localizedString, Singleton<HardmodeManager>.Instance.currentLevel);
		}
		else
		{
			((TMP_Text)_subTitle).text = Localization.GetLocalizedString(_normalSubTitleKey);
		}
		((TMP_Text)_title).text = Localization.GetLocalizedString(_titleKey);
		((TMP_Text)_stageName).text = Singleton<Service>.Instance.levelManager.currentChapter.stageName;
		((Component)_yourEnemy).gameObject.SetActive(true);
		((Component)this).gameObject.SetActive(true);
	}

	public void ShowEndingResult()
	{
		HardmodeManager instance = Singleton<HardmodeManager>.Instance;
		((TMP_Text)_title).text = Localization.GetLocalizedString(_endingTitleKey);
		if (instance.hardmode)
		{
			if (instance.currentLevel > instance.clearedLevel)
			{
				((TMP_Text)_subTitle).text = string.Format(Localization.GetLocalizedString(_hardmodeEndingFirstSubTitleKey), instance.currentLevel);
			}
			else
			{
				((TMP_Text)_subTitle).text = string.Format(Localization.GetLocalizedString(_hardmodeEndingSubTitleKey), instance.currentLevel);
			}
		}
		else
		{
			((TMP_Text)_subTitle).text = Localization.GetLocalizedString(_endingSubTitleKey);
		}
		((TMP_Text)_stageName).text = Localization.GetLocalizedString(_endingStageNameKey);
		((Component)_yourEnemy).gameObject.SetActive(false);
		((Component)this).gameObject.SetActive(true);
	}

	public void Hide()
	{
		((Component)this).gameObject.SetActive(false);
	}

	public void ShowEndingPortrait()
	{
		((Component)_deathCam).gameObject.SetActive(false);
		_endingPortrait.SetActive(true);
	}

	public void HideEndingPortrait()
	{
		((Component)_deathCam).gameObject.SetActive(true);
		_endingPortrait.SetActive(false);
	}
}
