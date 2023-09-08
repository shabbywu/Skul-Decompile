using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.Gear.Quintessences;
using Characters.Gear.Weapons;
using Characters.Player;
using Hardmode;
using Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud;

public class HeadupDisplay : MonoBehaviour
{
	private Character _character;

	private WeaponInventory _weaponInventory;

	private ItemInventory _itemInventory;

	private QuintessenceInventory _quintessenceInventory;

	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private GameObject _rightBottomWithMinimap;

	[SerializeField]
	private GameObject _rightBottomWithoutMinimap;

	[SerializeField]
	private AbilityIconDisplay _abilityIconDisplay;

	[SerializeField]
	private DavyJonesHud _davyJonesHud;

	[SerializeField]
	private CharacterHealthBar _healthBar;

	[SerializeField]
	private HealthValue _healthValue;

	[SerializeField]
	private GaugeBar _gaugeBar;

	[SerializeField]
	private BossHealthbarController _bossHealthBar;

	[SerializeField]
	private DarkEnemyHealthbarController _darkEnemyHealthBar;

	[SerializeField]
	private Image _currentWeapon;

	[SerializeField]
	private Image _nextWeapon;

	[SerializeField]
	private Image _nextWeaponSilenceMask;

	[SerializeField]
	private Image _changeWeaponCooldown;

	[SerializeField]
	private ActionIcon[] _skills;

	[SerializeField]
	private ActionIcon[] _subskills;

	[SerializeField]
	private EssenceIcon _quintessence;

	[SerializeField]
	private Animator _swapReadyEffect;

	[SerializeField]
	private CurrencyDisplay[] _heartQuartzDisplays;

	private CoroutineReference _cPlaySwapReadyEffectReference;

	public AbilityIconDisplay abilityIconDisplay => _abilityIconDisplay;

	public BossHealthbarController bossHealthBar => _bossHealthBar;

	public DarkEnemyHealthbarController darkEnemyHealthBar => _darkEnemyHealthBar;

	public DavyJonesHud davyJonesHud => _davyJonesHud;

	public bool visible
	{
		get
		{
			return _container.activeSelf;
		}
		set
		{
			_container.SetActive(value);
		}
	}

	public bool minimapVisible
	{
		get
		{
			return _rightBottomWithMinimap.activeSelf;
		}
		set
		{
			_rightBottomWithMinimap.SetActive(value);
			_rightBottomWithoutMinimap.SetActive(!value);
		}
	}

	public void Initialize(Character player)
	{
		_character = player;
		_weaponInventory = ((Component)player).GetComponent<WeaponInventory>();
		_itemInventory = ((Component)player).GetComponent<ItemInventory>();
		_quintessenceInventory = ((Component)player).GetComponent<QuintessenceInventory>();
		_abilityIconDisplay.Initialize(player);
		_healthBar.Initialize(player);
		_healthValue.Initialize(player.health, player.health.shield);
		_weaponInventory.onSwap += UpdateGauge;
		_weaponInventory.onChanged += OnWeaponChange;
		_weaponInventory.onSwapReady += SpawnSwapReadyEffect;
	}

	private void SpawnSwapReadyEffect()
	{
		if (((Behaviour)this).isActiveAndEnabled)
		{
			_cPlaySwapReadyEffectReference.Stop();
			_cPlaySwapReadyEffectReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CPlaySwapReadyEffect());
		}
	}

	private IEnumerator CPlaySwapReadyEffect()
	{
		((Component)_swapReadyEffect).gameObject.SetActive(true);
		_swapReadyEffect.Play(0, 0, 0f);
		Chronometer.Global global = Chronometer.global;
		AnimatorStateInfo currentAnimatorStateInfo = _swapReadyEffect.GetCurrentAnimatorStateInfo(0);
		yield return global.WaitForSeconds(((AnimatorStateInfo)(ref currentAnimatorStateInfo)).length);
		((Component)_swapReadyEffect).gameObject.SetActive(false);
	}

	private void OnWeaponChange(Weapon old, Weapon @new)
	{
		UpdateGauge();
	}

	private void UpdateGauge()
	{
		_gaugeBar.gauge = _weaponInventory.polymorphOrCurrent.gauge;
	}

	private void SetActive(GameObject gameObject, bool value)
	{
		if (gameObject.activeSelf != value)
		{
			gameObject.SetActive(value);
		}
	}

	private void OnDisable()
	{
		((Component)_swapReadyEffect).gameObject.SetActive(false);
	}

	private void Update()
	{
		if ((Object)(object)_character == (Object)null)
		{
			return;
		}
		_currentWeapon.sprite = _weaponInventory.polymorphOrCurrent.mainIcon;
		Weapon next = _weaponInventory.next;
		if ((Object)(object)next == (Object)null)
		{
			SetActive(((Component)((Component)_nextWeapon).transform.parent).gameObject, value: false);
			_changeWeaponCooldown.fillAmount = 0f;
			for (int i = 0; i < _subskills.Length; i++)
			{
				SetActive(((Component)_subskills[i]).gameObject, value: false);
			}
		}
		else
		{
			SetActive(((Component)((Component)_nextWeapon).transform.parent).gameObject, value: true);
			SetActive(((Component)_nextWeaponSilenceMask).gameObject, _character.silence.value);
			_nextWeapon.sprite = next.subIcon;
			_nextWeapon.preserveAspect = true;
			_changeWeaponCooldown.fillAmount = _weaponInventory.reaminCooldownPercent;
			List<SkillInfo> currentSkills = _weaponInventory.next.currentSkills;
			for (int j = 0; j < _subskills.Length; j++)
			{
				ActionIcon actionIcon = _subskills[j];
				if (j >= currentSkills.Count)
				{
					SetActive(((Component)actionIcon).gameObject, value: false);
					continue;
				}
				SkillInfo skillInfo = currentSkills[j];
				SetActive(((Component)actionIcon).gameObject, value: true);
				actionIcon.icon.sprite = skillInfo.cachedIcon;
				actionIcon.icon.preserveAspect = true;
				actionIcon.action = skillInfo.action;
				actionIcon.cooldown = skillInfo.action.cooldown;
			}
		}
		List<SkillInfo> currentSkills2 = _weaponInventory.polymorphOrCurrent.currentSkills;
		for (int k = 0; k < _skills.Length; k++)
		{
			ActionIcon actionIcon2 = _skills[k];
			if (k >= currentSkills2.Count)
			{
				SetActive(((Component)actionIcon2).gameObject, value: false);
				continue;
			}
			SkillInfo skillInfo2 = currentSkills2[k];
			SetActive(((Component)actionIcon2).gameObject, value: true);
			actionIcon2.icon.sprite = skillInfo2.cachedIcon;
			actionIcon2.icon.preserveAspect = true;
			actionIcon2.action = skillInfo2.action;
			actionIcon2.cooldown = skillInfo2.action.cooldown;
		}
		if ((Object)(object)_quintessenceInventory.items[0] == (Object)null)
		{
			SetActive(((Component)_quintessence).gameObject, value: false);
		}
		else
		{
			SetActive(((Component)_quintessence).gameObject, value: true);
			SetQuintessenceInfo(_quintessence, _quintessenceInventory.items[0]);
		}
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			for (int l = 0; l < _heartQuartzDisplays.Length; l++)
			{
				((Component)_heartQuartzDisplays[l]).gameObject.SetActive(true);
			}
		}
		else
		{
			for (int m = 0; m < _heartQuartzDisplays.Length; m++)
			{
				((Component)_heartQuartzDisplays[m]).gameObject.SetActive(false);
			}
		}
		static void SetQuintessenceInfo(EssenceIcon essenceIcon, Quintessence quintessence)
		{
			essenceIcon.icon.sprite = quintessence.hudIcon;
			essenceIcon.icon.preserveAspect = true;
			essenceIcon.cooldown = quintessence.cooldown;
			essenceIcon.constraints = quintessence.constraints;
		}
	}
}
