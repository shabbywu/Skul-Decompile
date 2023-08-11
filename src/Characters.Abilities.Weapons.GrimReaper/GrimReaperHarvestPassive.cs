using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.GrimReaper;

[Serializable]
public class GrimReaperHarvestPassive : Ability, IAbilityInstance
{
	[SerializeField]
	[Header("영혼 생성")]
	private GrimReaperSoul _grimReaperSoul;

	[SerializeField]
	private string _attackKey;

	[SerializeField]
	[Range(1f, 100f)]
	private int _possibility;

	[SerializeField]
	private CharacterTypeBoolArray _characterType;

	[SerializeField]
	[Header("영혼 스텟")]
	private int _maxStack;

	[SerializeField]
	private Stat.Values _statPerStack;

	private Stat.Values _stat;

	private int _stack;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon
	{
		get
		{
			if (_stack <= 0)
			{
				return null;
			}
			return _defaultIcon;
		}
	}

	public float iconFillAmount => remainTime / base.duration;

	public int iconStacks => _stack;

	public bool expired => false;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		return this;
	}

	public void Attach()
	{
		_stack = 0;
		_stat = _statPerStack.Clone();
		owner.stat.AttachValues(_stat);
		Character character = owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		remainTime = _duration;
		UpdateStack();
	}

	public void Detach()
	{
		owner.stat.DetachValues(_stat);
		Character character = owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
	}

	private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)target.character == (Object)null) && ((EnumArray<Character.Type, bool>)_characterType)[target.character.type] && gaveDamage.key.Equals(_attackKey, StringComparison.OrdinalIgnoreCase) && MMMaths.PercentChance(_possibility))
		{
			GrimReaperSoul grimReaperSoul = _grimReaperSoul;
			Bounds bounds = ((Collider2D)target.character.collider).bounds;
			grimReaperSoul.Spawn(((Bounds)(ref bounds)).center, this);
		}
	}

	public void Refresh()
	{
	}

	public void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
		if (remainTime <= 0f)
		{
			Reset();
		}
	}

	public void Reset()
	{
		_stack = 0;
		UpdateStack();
	}

	public void AddStack()
	{
		remainTime = _duration;
		_stack = Mathf.Clamp(_stack + 1, 0, _maxStack);
		UpdateStack();
	}

	private void UpdateStack()
	{
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
		{
			((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)_statPerStack).values[i].GetStackedValue(_stack);
		}
		owner.stat.SetNeedUpdate();
	}
}
