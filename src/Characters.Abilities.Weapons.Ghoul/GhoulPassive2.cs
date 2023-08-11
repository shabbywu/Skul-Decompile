using System;
using System.Linq;
using Characters.Abilities.CharacterStat;
using Characters.Gear.Weapons.Gauges;
using Level;
using UnityEngine;

namespace Characters.Abilities.Weapons.Ghoul;

[Serializable]
public sealed class GhoulPassive2 : Ability, IAbilityInstance
{
	private class GreyHealthPassive
	{
		private float _freezeTime;

		private bool _decreasing;

		private float _decreasingSpeed;

		private float _elapsed;

		private GrayHealth _grayHealth;

		internal GreyHealthPassive(GrayHealth grayHealth, float decreasingSpeed, float freezeTime)
		{
			_freezeTime = freezeTime;
			_decreasingSpeed = decreasingSpeed;
			_elapsed = 0f;
			_grayHealth = grayHealth;
		}

		internal void Update(float deltaTime)
		{
			if (!(_grayHealth.maximum <= 0.0))
			{
				_elapsed += deltaTime;
				if (_elapsed > _freezeTime)
				{
					_decreasing = true;
				}
				else
				{
					_decreasing = false;
				}
				if (_decreasing)
				{
					_grayHealth.maximum -= _decreasingSpeed * ((ChronometerBase)Chronometer.global).deltaTime;
				}
			}
		}

		internal void Refresh()
		{
			_elapsed = 0f;
		}
	}

	[Header("회색 체력")]
	[SerializeField]
	private bool _used = true;

	[SerializeField]
	private float _grayHealthLifeTime;

	[Tooltip("초당 줄어드는 양")]
	[SerializeField]
	private float _grayHealthDecreaseSpeed = 1f;

	[SerializeField]
	private Curve _recoveryCurve;

	private GreyHealthPassive _grayHealthPassive;

	[Header("살덩이")]
	[SerializeField]
	private AttackTypeBoolArray _attackTypeFilter;

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private ValueGauge _gauge;

	[SerializeField]
	private Color _defaultColor;

	[SerializeField]
	private Color _canUseColor;

	[SerializeField]
	private DroppedGhoulFlesh _fleshPrefab;

	[Range(1f, 100f)]
	[SerializeField]
	private int _dropPossibility;

	private float _gaugeAnimationTime;

	private Vector2 _spawnOffset;

	[SerializeField]
	[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
	private float _iconStacksPerStack = 1f;

	[SerializeField]
	private int _minStack;

	[SerializeField]
	private int _maxStack;

	private int _stacks;

	[SerializeField]
	[Header("살덩이 스텟")]
	private Stat.Values _statPerStack;

	[SerializeField]
	private Curve _damageMultiperByStack;

	private float _cachedDamageMultiplierByStack;

	private Stat.Values _stat;

	[Header("컨슘")]
	[SerializeField]
	private string[] _consumeKey = new string[1] { "consume" };

	[SerializeField]
	private string[] _killConsumeKey = new string[1] { "kill" };

	[SerializeField]
	private string[] _damageMultiplierByStackKey = new string[1] { "gas" };

	[SerializeField]
	private int _consumeAbilityStackMultiplier = 2;

	[SerializeField]
	private StackableStatBonusComponent _consumeAbility;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon
	{
		get
		{
			if (_stacks <= 0)
			{
				return null;
			}
			return _defaultIcon;
		}
	}

	public float iconFillAmount => 1f - remainTime / base.duration;

	public int iconStacks => (int)((float)_stacks * _iconStacksPerStack);

	public bool expired => false;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		this.owner = owner;
		Bounds bounds = ((Component)_fleshPrefab).GetComponent<Collider2D>().bounds;
		Vector3 size = ((Bounds)(ref bounds)).size;
		_spawnOffset.y = size.y * 0.6f;
		_damageMultiperByStack.duration = _maxStack;
		if (_used)
		{
			_grayHealthPassive = new GreyHealthPassive(owner.health.grayHealth, _grayHealthDecreaseSpeed, _grayHealthLifeTime);
		}
		return this;
	}

	public void Attach()
	{
		remainTime = base.duration;
		_stat = _statPerStack.Clone();
		_stacks = 0;
		owner.stat.AttachValues(_stat);
		UpdateStack();
		owner.health.onTookDamage += AddGrayHealth;
		((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)OnOwnerGiveDamage);
		Character character = owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		Character character2 = owner;
		character2.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character2.onKilled, new Character.OnKilledDelegate(OnOwnerKill));
	}

	private void AddGrayHealth(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (_used && tookDamage.attackType != 0 && tookDamage.amount != 0.0)
		{
			if (!owner.health.shield.hasAny)
			{
				owner.health.grayHealth.maximum += tookDamage.amount;
			}
			_grayHealthPassive.Refresh();
		}
	}

	private bool OnOwnerGiveDamage(ITarget target, ref Damage damage)
	{
		if ((Object)(object)target.character == (Object)null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
		{
			return false;
		}
		string damageKey = damage.key;
		if (_damageMultiplierByStackKey.Any((string key) => damageKey.Equals(key, StringComparison.OrdinalIgnoreCase)))
		{
			damage.multiplier *= _cachedDamageMultiplierByStack;
		}
		return false;
	}

	private void OnOwnerKill(ITarget target, ref Damage damage)
	{
		if (!((Object)(object)target.character == (Object)null) && target.character.type != Character.Type.Dummy && target.character.type != Character.Type.Trap)
		{
			string damageKey = damage.key;
			if (_killConsumeKey.Any((string key) => damageKey.Equals(key, StringComparison.OrdinalIgnoreCase)))
			{
				AddStack();
			}
		}
	}

	public void Detach()
	{
		DestroyGrayHealth();
		owner.stat.DetachValues(_stat);
		owner.health.onTookDamage -= AddGrayHealth;
		((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)OnOwnerGiveDamage);
		Character character = owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
	}

	public void Refresh()
	{
	}

	public void UpdateTime(float deltaTime)
	{
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		UpdateStack();
		if (_stacks > 0)
		{
			remainTime -= deltaTime;
			if (remainTime <= 0f)
			{
				_stacks = 0;
			}
			if (_stacks >= _minStack)
			{
				_gaugeAnimationTime += deltaTime * 2f;
				if (_gaugeAnimationTime > 2f)
				{
					_gaugeAnimationTime = 0f;
				}
				_gauge.defaultBarGaugeColor.baseColor = Color.LerpUnclamped(_defaultColor, _canUseColor, (_gaugeAnimationTime < 1f) ? _gaugeAnimationTime : (2f - _gaugeAnimationTime));
			}
			else
			{
				_gauge.defaultBarGaugeColor.baseColor = _defaultColor;
			}
		}
		if (!(owner.health.grayHealth.maximum <= 0.0) && _used)
		{
			_grayHealthPassive.Update(deltaTime);
			UpdateCanHealAmount();
		}
	}

	public void Recover()
	{
		if (_used && owner.health.grayHealth.canHeal > 0.0)
		{
			owner.health.GrayHeal();
		}
		_cachedDamageMultiplierByStack = 1f + _damageMultiperByStack.Evaluate((float)_stacks);
		_stacks = 0;
		UpdateStack();
		if (_used)
		{
			_grayHealthPassive.Refresh();
		}
	}

	private void UpdateCanHealAmount()
	{
		double maximum = owner.health.grayHealth.maximum;
		float num = (float)_stacks / (float)_maxStack;
		owner.health.grayHealth.canHeal = maximum * (double)_recoveryCurve.Evaluate(num);
	}

	private void DestroyGrayHealth()
	{
		if (_used)
		{
			owner.health.grayHealth.maximum = 0.0;
		}
	}

	public void AddStack()
	{
		remainTime = base.duration;
		if (_stacks < _maxStack)
		{
			_stacks++;
		}
		UpdateStack();
	}

	private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)target.character == (Object)null) && target.character.type != Character.Type.Dummy && target.character.type != Character.Type.Trap && ((EnumArray<Damage.AttackType, bool>)_attackTypeFilter)[gaveDamage.attackType] && ((EnumArray<Damage.MotionType, bool>)_motionTypeFilter)[gaveDamage.motionType])
		{
			string damageKey = gaveDamage.key;
			if (_consumeKey.Any((string key) => damageKey.Equals(key, StringComparison.OrdinalIgnoreCase)))
			{
				AddStack();
			}
			else if (MMMaths.PercentChance(_dropPossibility))
			{
				_fleshPrefab.Spawn(Vector2.op_Implicit(gaveDamage.hitPoint + _spawnOffset), this);
			}
		}
	}

	private void UpdateStack()
	{
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
		{
			Stat.Value value = ((ReorderableArray<Stat.Value>)_stat).values[i];
			if (value.kindIndex == Stat.Kind.PhysicalAttackDamage.index && (Object)(object)_consumeAbility != (Object)null && owner.ability.Contains(_consumeAbility.ability))
			{
				value.value = ((ReorderableArray<Stat.Value>)_statPerStack).values[i].GetStackedValue(_stacks + (int)_consumeAbility.stack * _consumeAbilityStackMultiplier);
			}
			else
			{
				value.value = ((ReorderableArray<Stat.Value>)_statPerStack).values[i].GetStackedValue(_stacks);
			}
		}
		owner.stat.SetNeedUpdate();
		_gauge.Set(_stacks);
		if (_minStack >= _stacks)
		{
			_gauge.defaultBarGaugeColor.baseColor = _defaultColor;
		}
		else
		{
			_gauge.defaultBarGaugeColor.baseColor = _canUseColor;
		}
	}
}
