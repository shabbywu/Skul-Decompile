using System;
using Characters.Gear.Weapons;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Weapons.GrimReaper;

[Serializable]
public class GrimReaperPassive : Ability, IAbilityInstance
{
	[SerializeField]
	[Header("스킬 강화")]
	private Weapon _weapon;

	[SerializeField]
	private SkillInfo[] _skills;

	[SerializeField]
	private int _enhancedStackPoint;

	[SerializeField]
	private SkillInfo[] _enhancedSkills;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onEnhancedSkill;

	[SerializeField]
	private int _enhanced2StackPoint;

	[SerializeField]
	private SkillInfo[] _enhanced2Skills;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onEnhancedSkill2;

	[Header("영혼 생성")]
	[SerializeField]
	private GrimReaperSoul _grimReaperSoul;

	[SerializeField]
	[Range(1f, 100f)]
	private int _possibility;

	[SerializeField]
	private CharacterTypeBoolArray _characterType;

	[SerializeField]
	[Header("영혼 스텟")]
	private Stat.Values _statPerStack;

	private Stat.Values _stat;

	private int _stack;

	private Weapon _currentWeapon;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime
	{
		get
		{
			return 0f;
		}
		set
		{
		}
	}

	public bool attached => true;

	public Sprite icon
	{
		get
		{
			if (stack <= 0 || !((Object)(object)_currentWeapon == (Object)(object)_weapon))
			{
				return null;
			}
			return _defaultIcon;
		}
	}

	public float iconFillAmount => 0f;

	public int iconStacks => stack;

	public bool expired => false;

	public int stack
	{
		get
		{
			return _stack;
		}
		set
		{
			_stack = value;
			UpdateStat();
		}
	}

	public void Attach()
	{
		_currentWeapon = owner.playerComponents.inventory.weapon.current;
		owner.playerComponents.inventory.weapon.onSwap += UpdateCurreuntWeapon;
		owner.playerComponents.inventory.weapon.onChanged += Weapon_onChanged;
		_stat = _statPerStack.Clone();
		owner.stat.AttachOrUpdateValues(_stat);
		Character character = owner;
		character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(OnKilledEnemy));
		SetEnhanceSkill();
		UpdateStat();
	}

	private void Weapon_onChanged(Weapon old, Weapon @new)
	{
		UpdateCurreuntWeapon();
	}

	private void UpdateCurreuntWeapon()
	{
		_currentWeapon = owner.playerComponents.inventory.weapon.current;
		if ((Object)(object)_currentWeapon != (Object)(object)_weapon)
		{
			owner.stat.DetachValues(_stat);
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(OnKilledEnemy));
		}
		else
		{
			owner.stat.AttachOrUpdateValues(_stat);
			Character character2 = owner;
			character2.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character2.onKilled, new Character.OnKilledDelegate(OnKilledEnemy));
			Character character3 = owner;
			character3.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character3.onKilled, new Character.OnKilledDelegate(OnKilledEnemy));
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		_onEnhancedSkill.Initialize();
		_onEnhancedSkill2.Initialize();
		return this;
	}

	public void Detach()
	{
		owner.playerComponents.inventory.weapon.onSwap -= UpdateCurreuntWeapon;
		owner.playerComponents.inventory.weapon.onChanged -= Weapon_onChanged;
		owner.stat.DetachValues(_stat);
		Character character = owner;
		character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(OnKilledEnemy));
		if (stack >= _enhanced2StackPoint)
		{
			_weapon.DetachSkillChanges(_skills, _enhanced2Skills);
		}
		else if (stack >= _enhancedStackPoint)
		{
			_weapon.DetachSkillChanges(_skills, _enhancedSkills);
		}
	}

	public void Refresh()
	{
	}

	public void UpdateTime(float deltaTime)
	{
	}

	private void OnKilledEnemy(ITarget target, ref Damage damage)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)target.character == (Object)null) && ((EnumArray<Character.Type, bool>)_characterType)[target.character.type] && MMMaths.PercentChance(_possibility))
		{
			GrimReaperSoul grimReaperSoul = _grimReaperSoul;
			Bounds bounds = ((Collider2D)target.character.collider).bounds;
			grimReaperSoul.Spawn(((Bounds)(ref bounds)).center, this);
		}
	}

	public void AddStack()
	{
		stack++;
		TryToEnhanceSkill();
		UpdateStat();
	}

	private void TryToEnhanceSkill()
	{
		if (stack == _enhancedStackPoint)
		{
			_weapon.AttachSkillChanges(_skills, _enhancedSkills);
			((MonoBehaviour)owner).StartCoroutine(_onEnhancedSkill.CRun(owner));
		}
		else if (stack == _enhanced2StackPoint)
		{
			_weapon.DetachSkillChanges(_skills, _enhancedSkills);
			_weapon.AttachSkillChanges(_skills, _enhanced2Skills);
			((MonoBehaviour)owner).StartCoroutine(_onEnhancedSkill2.CRun(owner));
		}
	}

	public void SetEnhanceSkill()
	{
		if (stack >= _enhancedStackPoint && stack < _enhanced2StackPoint)
		{
			_weapon.AttachSkillChanges(_skills, _enhancedSkills);
		}
		else if (stack >= _enhanced2StackPoint)
		{
			_weapon.AttachSkillChanges(_skills, _enhanced2Skills);
		}
	}

	private void UpdateStat()
	{
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)_statPerStack).values[i].GetStackedValue(stack);
		}
		owner.stat.SetNeedUpdate();
	}
}
