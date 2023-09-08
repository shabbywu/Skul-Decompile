using System;
using System.Collections;
using System.Linq;
using Characters.Controllers;
using Characters.Gear;
using Characters.Gear.Weapons;
using Data;
using FX;
using FX.SpriteEffects;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Player;

public sealed class WeaponInventory : MonoBehaviour, IAttackDamage
{
	public delegate void OnChangeDelegate(Weapon old, Weapon @new);

	[GetComponent]
	[SerializeField]
	private Character _character;

	[SerializeField]
	[GetComponent]
	private PlayerInput _input;

	[SerializeField]
	private Weapon[] _defaultSkins;

	private const float _swapCooldown = 8f;

	private float _remainCooldown;

	public readonly Weapon[] weapons = new Weapon[2];

	private EffectInfo _switchEffect;

	private Weapon _default;

	public int currentIndex { get; private set; } = -1;


	public Weapon current { get; private set; }

	public Weapon polymorphWeapon { get; private set; }

	public Weapon polymorphOrCurrent
	{
		get
		{
			if (!((Object)(object)polymorphWeapon == (Object)null))
			{
				return polymorphWeapon;
			}
			return current;
		}
	}

	public Weapon next
	{
		get
		{
			int num = currentIndex;
			do
			{
				num++;
				if (num == weapons.Length)
				{
					num = 0;
				}
				if (num == currentIndex)
				{
					return null;
				}
			}
			while ((Object)(object)weapons[num] == (Object)null);
			return weapons[num];
		}
	}

	public IAttackDamage weaponAttackDamage { get; private set; }

	public float amount => weaponAttackDamage.amount;

	public float reaminCooldownPercent => _remainCooldown / 8f;

	public bool swapReady { get; private set; } = true;


	public event Action onSwap;

	public event OnChangeDelegate onChanged;

	public event Action onSwapReady;

	private void Awake()
	{
		EquipDefault();
		_switchEffect = new EffectInfo(CommonResource.instance.swapEffect)
		{
			chronometer = _character.chronometer.effect
		};
	}

	private void EquipDefault()
	{
		int num = ((Singleton<Service>.Instance.levelManager.currentChapter.type != Chapter.Type.Castle) ? GameData.Generic.skinIndex : 0);
		_default = _defaultSkins[num].Instantiate();
		ForceEquip(_default);
	}

	private void OnDisable()
	{
		PlayerInput.blocked.Detach(this);
	}

	private void Update()
	{
		if (_remainCooldown > 0f)
		{
			swapReady = false;
			_remainCooldown -= _character.chronometer.master.deltaTime * _character.stat.GetSwapCooldownSpeed();
			return;
		}
		if (!swapReady)
		{
			this.onSwapReady?.Invoke();
			swapReady = true;
		}
		_remainCooldown = 0f;
	}

	private void StartUse(Weapon weapon)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		((Component)weapon).gameObject.SetActive(true);
		_character.CancelAction();
		_character.InitializeActions();
		_character.animationController.Initialize();
		_character.animationController.ForceUpdate();
		_character.collider.size = weapon.hitbox.size;
		((Collider2D)_character.collider).offset = ((Collider2D)weapon.hitbox).offset;
		weaponAttackDamage = ((Component)weapon).GetComponent<IAttackDamage>();
		weapon.StartUse();
	}

	private void EndUse(Weapon weapon)
	{
		weapon.EndUse();
		_character.stat.DetachValues(weapon.stat);
	}

	public void UpdateSkin()
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			Weapon weapon = weapons[i];
			if ((Object)(object)weapon == (Object)null)
			{
				continue;
			}
			bool flag = false;
			Weapon[] defaultSkins = _defaultSkins;
			foreach (Weapon weapon2 in defaultSkins)
			{
				if (((Object)weapon).name.Equals(((Object)weapon2).name, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				_default = _defaultSkins[GameData.Generic.skinIndex].Instantiate();
				ForceEquipAt(_default, i);
			}
		}
	}

	public bool NextWeapon(bool force = false)
	{
		if (!force && (_remainCooldown > 0f || _character.stunedOrFreezed))
		{
			return false;
		}
		for (int i = 1; i < weapons.Length; i++)
		{
			int num = (currentIndex + i) % weapons.Length;
			if ((Object)(object)weapons[num] != (Object)null)
			{
				ChangeWeaponWithSwitchAction(num);
				_remainCooldown = 8f;
				return true;
			}
		}
		return false;
	}

	private void ChangeWeapon(int index)
	{
		Unpolymorph();
		polymorphWeapon = null;
		((Component)current).gameObject.SetActive(false);
		current.EndUse();
		currentIndex = index;
		current = weapons[currentIndex];
		StartUse(current);
		this.onSwap?.Invoke();
	}

	private void ChangeWeaponWithSwitchAction(int index)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		Effects.SpritePoolObject spritePoolObject = Effects.sprite.Spawn();
		SpriteRenderer spriteRenderer = spritePoolObject.spriteRenderer;
		SpriteRenderer spriteRenderer2 = current.characterAnimation.spriteRenderer;
		spriteRenderer.sprite = spriteRenderer2.sprite;
		((Component)spriteRenderer).transform.localScale = ((Component)spriteRenderer2).transform.lossyScale;
		((Component)spriteRenderer).transform.SetPositionAndRotation(((Component)spriteRenderer2).transform.position, ((Component)spriteRenderer2).transform.rotation);
		spriteRenderer.flipX = spriteRenderer2.flipX;
		spriteRenderer.flipY = spriteRenderer2.flipY;
		((Renderer)spriteRenderer).sortingLayerID = ((Renderer)spriteRenderer2).sortingLayerID;
		((Renderer)spriteRenderer).sortingOrder = ((Renderer)spriteRenderer2).sortingOrder - 1;
		spriteRenderer.color = new Color(71f / 85f, 0.18039216f, 1f);
		((Renderer)spriteRenderer).sharedMaterial = MaterialResource.color;
		spritePoolObject.FadeOut(_character.chronometer.effect, AnimationCurve.Linear(0f, 0f, 1f, 1f), 0.5f);
		ChangeWeapon(index);
		current.StartSwitchAction();
		_switchEffect.Spawn(((Component)this).transform.position);
	}

	public void LoseAll()
	{
		Unpolymorph();
		current.EndUse();
		ChangeWeapon(0);
		for (int i = 1; i < weapons.Length; i++)
		{
			Weapon weapon = weapons[i];
			if (!((Object)(object)weapon == (Object)null))
			{
				Drop(weapon);
				this.onChanged?.Invoke(weapon, null);
				Object.Destroy((Object)(object)((Component)weapon).gameObject);
			}
		}
		_character.InitializeActions();
		_character.animationController.Initialize();
		_character.animationController.ForceUpdate();
	}

	public void Unequip(Weapon weapon)
	{
		int num = -1;
		for (int i = 0; i < weapons.Length; i++)
		{
			if ((Object)(object)weapon == (Object)(object)weapons[i])
			{
				num = i;
				break;
			}
		}
		if (num != -1)
		{
			Drop(weapon);
			_character.InitializeActions();
			_character.animationController.Initialize();
			_character.animationController.ForceUpdate();
			weapons[num] = null;
			this.onChanged?.Invoke(weapon, null);
		}
	}

	private void Drop(Weapon weapon)
	{
		EndUse(weapon);
		weapon.state = Characters.Gear.Gear.State.Dropped;
	}

	public Weapon Equip(Weapon weapon)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		int index = currentIndex;
		for (int i = 0; i < weapons.Length; i++)
		{
			if ((Object)(object)weapons[i] == (Object)null)
			{
				index = i;
				break;
			}
		}
		_character.spriteEffectStack.Add(new EasedColorOverlay(int.MaxValue, Color.white, new Color(1f, 1f, 1f, 0f), new Curve(AnimationCurve.Linear(0f, 0f, 1f, 1f), 1f, 0.15f)));
		return EquipAt(weapon, index);
	}

	public Weapon EquipAt(Weapon weapon, int index)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		Unpolymorph();
		for (int i = 0; i < weapons.Length; i++)
		{
			if (index != i && (Object)(object)weapons[i] != (Object)null)
			{
				((Component)weapons[i]).gameObject.SetActive(false);
				weapons[i].EndUse();
			}
		}
		Weapon weapon2 = weapons[index];
		if ((Object)(object)weapon2 != (Object)null)
		{
			Drop(weapon2);
		}
		current = weapon;
		((Component)current).transform.parent = _character.@base;
		((Component)current).transform.localPosition = Vector3.zero;
		((Component)current).transform.localScale = Vector3.one;
		StartUse(current);
		weapons[index] = weapon;
		currentIndex = index;
		this.onChanged?.Invoke(weapon2, weapon);
		return weapon2;
	}

	public void ForceEquip(Weapon weapon)
	{
		weapon.state = Characters.Gear.Gear.State.Equipped;
		int index = currentIndex;
		for (int i = 0; i < weapons.Length; i++)
		{
			if ((Object)(object)weapons[i] == (Object)null)
			{
				index = i;
				break;
			}
		}
		ForceEquipAt(weapon, index);
	}

	public void ForceEquipAt(Weapon weapon, int index)
	{
		weapon.SetOwner(_character);
		((Component)weapon).gameObject.SetActive(true);
		weapon.state = Characters.Gear.Gear.State.Equipped;
		Weapon weapon2 = weapons[index];
		EquipAt(weapon, index);
		if ((Object)(object)weapon2 != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)weapon2).gameObject);
		}
	}

	public void Polymorph(Weapon target)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)target == (Object)null)
		{
			Debug.Log((object)"[WeaponInventory] Polymorph target is null.");
			return;
		}
		Unpolymorph();
		polymorphWeapon = target;
		((Component)current).gameObject.SetActive(false);
		EndUse(current);
		polymorphWeapon.SetOwner(_character);
		((Component)polymorphWeapon).gameObject.SetActive(true);
		polymorphWeapon.state = Characters.Gear.Gear.State.Equipped;
		((Component)polymorphWeapon).transform.parent = _character.@base;
		((Component)polymorphWeapon).transform.localPosition = Vector3.zero;
		((Component)polymorphWeapon).transform.localScale = Vector3.one;
		StartUse(polymorphWeapon);
		this.onChanged?.Invoke(current, polymorphWeapon);
	}

	public void Unpolymorph()
	{
		if (!((Object)(object)polymorphWeapon == (Object)null))
		{
			Weapon weapon = polymorphWeapon;
			polymorphWeapon = null;
			((Component)weapon).gameObject.SetActive(false);
			EndUse(weapon);
			((Component)weapon).transform.parent = null;
			StartUse(current);
			this.onChanged?.Invoke(weapon, current);
		}
	}

	public void ReduceSwapCooldown(float second)
	{
		_remainCooldown -= second;
		if (_remainCooldown < 0f)
		{
			_remainCooldown = 0f;
		}
	}

	public void SetSwapCooldown(float second)
	{
		_remainCooldown = second;
		if (_remainCooldown < 0f)
		{
			_remainCooldown = 0f;
		}
	}

	public void ResetSwapCooldown()
	{
		_remainCooldown = 0f;
	}

	public int GetCountByCategory(Weapon.Category category)
	{
		int num = 0;
		Weapon[] array = weapons;
		foreach (Weapon weapon in array)
		{
			if (!((Object)(object)weapon == (Object)null) && weapon.category == category)
			{
				num++;
			}
		}
		return num;
	}

	public int GetCountByRarity(Rarity rarity)
	{
		int num = 0;
		Weapon[] array = weapons;
		foreach (Weapon weapon in array)
		{
			if (!((Object)(object)weapon == (Object)null) && weapon.rarity == rarity)
			{
				num++;
			}
		}
		return num;
	}

	public void UpgradeCurrentWeapon()
	{
		if (current.rarity == Rarity.Legendary || ((Object)current).name.Equals("Skul", StringComparison.OrdinalIgnoreCase))
		{
			Debug.Log((object)"각성할 수 없는 헤드입니다");
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CUpgradeCurrentWeapon());
		}
	}

	public IEnumerator CUpgradeCurrentWeapon()
	{
		current.UnapplyAllSkillChanges();
		string[] skillKeys = current.currentSkills.Select((SkillInfo skill) => skill.key).ToArray();
		yield return CWaitForUpgrade(skillKeys);
	}

	private IEnumerator CWaitForUpgrade(string[] skillKeys)
	{
		WeaponRequest request = current.nextLevelReference.LoadAsync();
		while (!request.isDone)
		{
			yield return null;
		}
		Weapon weapon = Singleton<Service>.Instance.levelManager.DropWeapon(request, ((Component)this).transform.position);
		current.destructible = false;
		weapon.SetSkills(skillKeys);
		ForceEquipAt(weapon, currentIndex);
	}

	public bool Has(string weaponKey)
	{
		for (int i = 0; i < weapons.Length; i++)
		{
			if (!((Object)(object)weapons[i] == (Object)null) && ((Object)weapons[i]).name.Equals(weaponKey, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}
}
