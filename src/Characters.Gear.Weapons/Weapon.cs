using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities;
using Characters.Actions;
using Characters.Controllers;
using Characters.Gear.Weapons.Gauges;
using Data;
using FX;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Weapons;

[RequireComponent(typeof(AttackDamage))]
public sealed class Weapon : Gear
{
	private class Assets
	{
		internal static EffectInfo destroyWeapon = new EffectInfo(CommonResource.instance.destroyWeapon);
	}

	public enum Category
	{
		Balance,
		Power,
		Speed,
		Ranged
	}

	public class SkillChangeMap
	{
		public readonly List<SkillInfo> originals = new List<SkillInfo>();

		public readonly List<SkillInfo> news = new List<SkillInfo>();

		public void Add(SkillInfo original, SkillInfo @new)
		{
			originals.Add(original);
			news.Add(@new);
		}

		public bool Remove(SkillInfo original, SkillInfo @new)
		{
			for (int i = 0; i < originals.Count; i++)
			{
				if ((Object)(object)originals[i] == (Object)(object)original && (Object)(object)news[i] == (Object)(object)@new)
				{
					originals.RemoveAt(i);
					news.RemoveAt(i);
					return true;
				}
			}
			return false;
		}
	}

	public readonly EnumArray<Characters.Actions.Action.Type, Characters.Actions.Action[]> actionsByType = new EnumArray<Characters.Actions.Action.Type, Characters.Actions.Action[]>();

	[SerializeField]
	private BoxCollider2D _hitbox;

	[SerializeField]
	private Category _category;

	[Range(0f, 2f)]
	[SerializeField]
	private int _skillSlots = 1;

	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private float _customWidth;

	[SerializeField]
	private Gauge _gauge;

	[SerializeField]
	[AbilityAttacher.Subcomponent]
	private AbilityAttacher.Subcomponents _abilityAttacher;

	[AbilityAttacher.Subcomponent]
	[SerializeField]
	private AbilityAttacher.Subcomponents _passiveAbilityAttacher;

	private readonly SkillChangeMap _skillChangeMap = new SkillChangeMap();

	public WeaponReference nextLevelReference;

	public override Type type => Type.Weapon;

	public override GameData.Currency.Type currencyTypeByDiscard => GameData.Currency.Type.Bone;

	public override int currencyByDiscard
	{
		get
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			if (base.dropped.price <= 0 && destructible)
			{
				return Settings.instance.bonesByDiscard[base.rarity];
			}
			return 0;
		}
	}

	protected override string _prefix => "weapon";

	public Category category => _category;

	public float customWidth => _customWidth;

	public Gauge gauge => _gauge;

	public string categoryDisplayName => Localization.GetLocalizedString(string.Format("{0}/{1}/{2}/{3}", "label", _prefix, "Category", _category));

	public string activeName => Localization.GetLocalizedString(base._keyBase + "/active/name");

	public string activeDescription => Localization.GetLocalizedString(base._keyBase + "/active/desc");

	public BoxCollider2D hitbox => _hitbox;

	public SkillInfo[] skills { get; private set; }

	public List<SkillInfo> currentSkills { get; private set; }

	public CharacterAnimation characterAnimation { get; private set; }

	public Sprite mainIcon { get; private set; }

	public Sprite subIcon { get; private set; }

	public bool upgradable => !string.IsNullOrEmpty(nextLevelReference.name);

	public event Action<Characters.Actions.Action> onStartAction;

	public event Action<Characters.Actions.Action> onEndAction;

	protected override void Awake()
	{
		base.Awake();
		Singleton<Service>.Instance.gearManager.RegisterWeaponInstance(this);
		InitializeSkills();
		AttachMinimapAgent();
		GetChildActions();
		characterAnimation = base.equipped.GetComponent<CharacterAnimation>();
	}

	private void OnDestroy()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected I4, but got Unknown
		if (Service.quitting)
		{
			return;
		}
		Singleton<Service>.Instance.gearManager.UnregisterWeaponInstance(this);
		_abilityAttacher.StopAttach();
		_passiveAbilityAttacher.StopAttach();
		_onDiscard?.Invoke(this);
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		if ((Object)(object)levelManager.player == (Object)null || !levelManager.player.liveAndActive)
		{
			return;
		}
		if (destructible)
		{
			GameData.Progress.encounterWeaponCount++;
			PersistentSingleton<SoundManager>.Instance.PlaySound(GlobalSoundSettings.instance.gearDestroying, ((Component)this).transform.position);
			Collider2D component = ((Component)base.dropped).GetComponent<Collider2D>();
			if ((Object)(object)component == (Object)null)
			{
				Assets.destroyWeapon.Spawn(((Component)this).transform.position);
			}
			else
			{
				EffectInfo destroyWeapon = Assets.destroyWeapon;
				Bounds bounds = component.bounds;
				destroyWeapon.Spawn(((Bounds)(ref bounds)).center);
			}
		}
		if (currencyByDiscard == 0)
		{
			return;
		}
		int count = 1;
		if (currencyByDiscard > 0)
		{
			Rarity val = base.rarity;
			switch ((int)val)
			{
			case 0:
				count = 4;
				break;
			case 1:
				count = 7;
				break;
			case 2:
				count = 13;
				break;
			case 3:
				count = 20;
				break;
			}
		}
		levelManager.DropBone(currencyByDiscard, count);
	}

	private void GetChildActions()
	{
		new List<Characters.Actions.Action>();
		new List<Characters.Actions.Action>();
		new List<Characters.Actions.Action>();
		new List<Characters.Actions.Action>();
		Characters.Actions.Action[] componentsInChildren = ((Component)this).GetComponentsInChildren<Characters.Actions.Action>(true);
		EnumArray<Characters.Actions.Action.Type, List<Characters.Actions.Action>> val = new EnumArray<Characters.Actions.Action.Type, List<Characters.Actions.Action>>();
		for (int i = 0; i < val.Keys.Count; i++)
		{
			val.Array[i] = new List<Characters.Actions.Action>();
		}
		Characters.Actions.Action[] array = componentsInChildren;
		foreach (Characters.Actions.Action action in array)
		{
			action.onStart += delegate
			{
				this.onStartAction?.Invoke(action);
			};
			action.onEnd += delegate
			{
				this.onEndAction?.Invoke(action);
			};
			val[action.type].Add(action);
		}
		for (int k = 0; k < val.Keys.Count; k++)
		{
			actionsByType.Array[k] = val.Array[k].ToArray();
		}
	}

	private void AttachMinimapAgent()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		GameObject val = new GameObject("MinimapAgent");
		val.transform.parent = base.equipped.transform;
		SpriteRenderer component = base.equipped.GetComponent<SpriteRenderer>();
		Bounds bounds = ((Collider2D)_hitbox).bounds;
		((Bounds)(ref bounds)).Expand(0.33f);
		MinimapAgentGenerator.Generate(val, bounds, Color.yellow, component);
	}

	private void InitializeSkills()
	{
		skills = (from skill in ((Component)this).GetComponentsInChildren<SkillInfo>(true)
			where ((Component)skill).gameObject.activeSelf
			select skill).ToArray();
		RandomizeSkills();
	}

	private void RandomizeSkills()
	{
		SetCurrentSkills();
		SetSkillButtons();
	}

	public void ApplyAllSkillChanges()
	{
		for (int i = 0; i < _skillChangeMap.originals.Count; i++)
		{
			int num = currentSkills.IndexOf(_skillChangeMap.originals[i]);
			if (num != -1)
			{
				ChangeSkill(num, _skillChangeMap.news[i], copyCooldown: true);
			}
		}
	}

	public void UnapplyAllSkillChanges()
	{
		for (int i = 0; i < _skillChangeMap.originals.Count; i++)
		{
			int num = currentSkills.IndexOf(_skillChangeMap.news[i]);
			if (num != -1)
			{
				ChangeSkill(num, _skillChangeMap.originals[i], copyCooldown: true);
			}
		}
	}

	public SkillInfo GetSkillWithoutSkillChanges(int index)
	{
		SkillInfo skillInfo = currentSkills[index];
		int num = _skillChangeMap.news.IndexOf(skillInfo);
		if (num == -1)
		{
			return skillInfo;
		}
		return _skillChangeMap.originals[num];
	}

	public void RerollSkills()
	{
		if (currentSkills.Count != skills.Length)
		{
			UnapplyAllSkillChanges();
			ILookup<bool, SkillInfo> lookup = skills.ToLookup((SkillInfo info) => info.hasAlways);
			SkillInfo[] array = lookup[true].ToArray();
			List<SkillInfo> excepts = new List<SkillInfo>(currentSkills.Where((SkillInfo info) => !info.hasAlways));
			if (excepts.Count > 1)
			{
				excepts.RemoveAt(ExtensionMethods.RandomIndex<SkillInfo>((IEnumerable<SkillInfo>)excepts));
			}
			List<SkillInfo> from = lookup[false].Where((SkillInfo info) => info.weight > 0 && !excepts.Contains(info)).ToList();
			int i;
			for (i = 0; i < array.Length; i++)
			{
				currentSkills[i] = array[i];
			}
			for (int j = i; j < _skillSlots; j++)
			{
				SkillInfo value = SkillInfo.WeightedRandomPop(from);
				currentSkills[j] = value;
			}
			ApplyAllSkillChanges();
			SetSkillButtons();
		}
	}

	public void SetSkills(string[] skillKeys, bool ignoreLevel = true)
	{
		ILookup<bool, SkillInfo> lookup = skills.ToLookup((SkillInfo info) => info.hasAlways);
		SkillInfo[] array = lookup[true].ToArray();
		List<SkillInfo> list = lookup[false].ToList();
		List<string> list2 = list.Select((SkillInfo skill) => skill.key).ToList();
		if (ignoreLevel)
		{
			for (int i = 0; i < list2.Count; i++)
			{
				string text = list2[i];
				int num = text.IndexOf('_');
				if (num >= 0)
				{
					list2[i] = text.Substring(0, num);
				}
			}
			for (int j = 0; j < skillKeys.Length; j++)
			{
				string text2 = skillKeys[j];
				int num2 = text2.IndexOf('_');
				if (num2 >= 0)
				{
					skillKeys[j] = text2.Substring(0, num2);
				}
			}
		}
		int k;
		for (k = 0; k < array.Length; k++)
		{
			currentSkills[k] = array[k];
		}
		foreach (string value in skillKeys)
		{
			for (int m = 0; m < list2.Count; m++)
			{
				if (k >= _skillSlots)
				{
					break;
				}
				if (list2[m].Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					currentSkills[k] = list[m];
					list.RemoveAt(m);
					list2.RemoveAt(m);
					k++;
					break;
				}
			}
		}
		for (int n = k; n < _skillSlots; n++)
		{
			SkillInfo value2 = SkillInfo.WeightedRandomPop(list);
			currentSkills[n] = value2;
		}
		SetSkillButtons();
	}

	private void SetCurrentSkills()
	{
		if (skills.Length < _skillSlots)
		{
			Debug.LogError((object)"Skill count is less than skill slots of the weapon.");
			return;
		}
		SkillInfo[] array = skills;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Initialize();
		}
		ILookup<bool, SkillInfo> lookup = skills.ToLookup((SkillInfo info) => info.hasAlways);
		SkillInfo[] array2 = lookup[true].ToArray();
		List<SkillInfo> list = lookup[false].Where((SkillInfo info) => info.weight > 0).ToList();
		if (array2.Length + list.Count == _skillSlots)
		{
			currentSkills = new List<SkillInfo>(array2);
			currentSkills.AddRange(list);
			return;
		}
		currentSkills = new List<SkillInfo>(new SkillInfo[_skillSlots]);
		int j;
		for (j = 0; j < array2.Length; j++)
		{
			currentSkills[j] = array2[j];
		}
		for (int k = j; k < _skillSlots; k++)
		{
			SkillInfo value = SkillInfo.WeightedRandomPop(list);
			currentSkills[k] = value;
		}
	}

	public void SetSkillButtons()
	{
		SkillInfo[] array = skills;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].action.button = Button.None;
		}
		SetActionButtonAt(0, Button.Skill);
		SetActionButtonAt(1, Button.Skill2);
		void SetActionButtonAt(int index, Button button)
		{
			if (index < currentSkills.Count)
			{
				Characters.Actions.Action component = ((Component)currentSkills[index]).GetComponent<Characters.Actions.Action>();
				if (!((Object)(object)component == (Object)null))
				{
					component.button = button;
				}
			}
		}
	}

	public override void Initialize()
	{
		base.Initialize();
		mainIcon = GearResource.instance.GetWeaponHudMainIcon(((Object)this).name);
		subIcon = GearResource.instance.GetWeaponHudSubIcon(((Object)this).name);
	}

	public void StartUse()
	{
		base.owner.stat.AttachValues(base.stat);
		_abilityAttacher.StartAttach();
		_passiveAbilityAttacher.StartAttach();
		foreach (SkillInfo currentSkill in currentSkills)
		{
			((Component)currentSkill).gameObject.SetActive(true);
		}
	}

	public void EndUse()
	{
		base.owner.stat.DetachValues(base.stat);
		_abilityAttacher.StopAttach();
		foreach (SkillInfo currentSkill in currentSkills)
		{
			((Component)currentSkill).gameObject.SetActive(false);
		}
	}

	public void StartSwitchAction()
	{
		Characters.Actions.Action[] array = actionsByType[Characters.Actions.Action.Type.Swap];
		for (int i = 0; i < array.Length && !array[i].TryStart(); i++)
		{
		}
	}

	protected override void OnLoot(Character character)
	{
		SetOwner(character);
		base.state = State.Equipped;
		character.playerComponents.inventory.weapon.Equip(this);
	}

	public void SetOwner(Character character)
	{
		base.owner = character;
	}

	protected override void OnEquipped()
	{
		base.OnEquipped();
		_abilityAttacher.Initialize(base.owner);
		_passiveAbilityAttacher.Initialize(base.owner);
	}

	protected override void OnDropped()
	{
		base.OnDropped();
		_passiveAbilityAttacher.StopAttach();
	}

	public Weapon Instantiate()
	{
		Weapon weapon = Object.Instantiate<Weapon>(this);
		((Object)weapon).name = ((Object)this).name;
		weapon.Initialize();
		return weapon;
	}

	public void RemoveSkill(string key)
	{
		for (int i = 0; i < currentSkills.Count; i++)
		{
			if (key.Equals(currentSkills[i].key, StringComparison.InvariantCultureIgnoreCase))
			{
				RemoveSkill(i);
				break;
			}
		}
	}

	public void RemoveSkill(int index)
	{
		((Component)currentSkills[index]).gameObject.SetActive(false);
		currentSkills.RemoveAt(index);
	}

	public void SwapSkillOrder()
	{
		if (currentSkills.Count >= 2)
		{
			ExtensionMethods.Swap<SkillInfo>((IList<SkillInfo>)currentSkills, 0, 1);
			SetSkillButtons();
		}
	}

	public void AttachSkillChange(SkillInfo original, SkillInfo @new, bool copyCooldown = false)
	{
		_skillChangeMap.Add(original, @new);
		int targetSkillIndex = currentSkills.IndexOf(original);
		ChangeSkill(targetSkillIndex, @new, copyCooldown);
	}

	public void AttachSkillChanges(SkillInfo[] originals, SkillInfo[] news, bool copyCooldown = false)
	{
		for (int i = 0; i < originals.Length; i++)
		{
			_skillChangeMap.Add(originals[i], news[i]);
			int num = currentSkills.IndexOf(originals[i]);
			if (num != -1)
			{
				ChangeSkill(num, news[i], copyCooldown);
			}
		}
	}

	public void DetachSkillChange(SkillInfo original, SkillInfo @new, bool copyCooldown = false)
	{
		if (_skillChangeMap.Remove(original, @new))
		{
			int targetSkillIndex = currentSkills.IndexOf(@new);
			ChangeSkill(targetSkillIndex, original, copyCooldown);
		}
	}

	public void DetachSkillChanges(SkillInfo[] originals, SkillInfo[] news, bool copyCooldown = false)
	{
		for (int i = 0; i < originals.Length; i++)
		{
			if (_skillChangeMap.Remove(originals[i], news[i]))
			{
				int num = currentSkills.IndexOf(news[i]);
				if (num != -1)
				{
					ChangeSkill(num, originals[i], copyCooldown);
				}
			}
		}
	}

	private void ChangeSkill(int targetSkillIndex, SkillInfo newSkill, bool copyCooldown = false)
	{
		if (targetSkillIndex >= 0 && targetSkillIndex < currentSkills.Count)
		{
			SkillInfo skillInfo = currentSkills[targetSkillIndex];
			currentSkills[targetSkillIndex] = newSkill;
			Button button = skillInfo.action.button;
			skillInfo.action.button = newSkill.action.button;
			newSkill.action.button = button;
			if (copyCooldown)
			{
				newSkill.action.cooldown.CopyCooldown(skillInfo.action.cooldown);
			}
		}
	}

	public void ChangeAction(Characters.Actions.Action targetAction, Characters.Actions.Action newAction)
	{
		SkillInfo component = ((Component)targetAction).GetComponent<SkillInfo>();
		SkillInfo component2 = ((Component)newAction).GetComponent<SkillInfo>();
		if ((Object)(object)component != (Object)null && (Object)(object)component2 != (Object)null)
		{
			Debug.LogError((object)"Please use ChangeSkill for action that has skill info.");
			return;
		}
		Button button = targetAction.button;
		targetAction.button = newAction.button;
		newAction.button = button;
	}
}
