using System;
using System.Linq;
using Characters.Abilities.CharacterStat;
using Characters.Gear.Weapons.Gauges;
using FX;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Weapons;

[Serializable]
public class PrisonerPassive : Ability, IAbilityInstance
{
	[Serializable]
	private class Scroll
	{
		[SerializeField]
		private EffectInfo _effect;

		[Range(0f, 100f)]
		[SerializeField]
		private int _weight;

		[SerializeField]
		private bool _brutality;

		[SerializeField]
		private bool _tactics;

		[SerializeField]
		private bool _survival;

		public EffectInfo effect => _effect;

		public int weight => _weight;

		public bool brutality => _brutality;

		public bool tactics => _tactics;

		public bool survival => _survival;

		public Scroll(bool brutality, bool tactics, bool survival)
		{
			_brutality = brutality;
			_tactics = tactics;
			_survival = survival;
		}
	}

	[Header("Walk Easteregg")]
	[SerializeField]
	private CharacterAnimation _characterAnimation;

	[SerializeField]
	private AnimationClip _walk;

	[SerializeField]
	private AnimationClip _walk2;

	[SerializeField]
	private Stat.Values _easterEggStat;

	[SerializeField]
	private float _easterEggDuration;

	private float _remainEasterEggDuration;

	private bool _easterAttached;

	[Space]
	[SerializeField]
	[Range(1f, 100f)]
	private int _possibility;

	[SerializeField]
	private double _totalDamage;

	[SerializeField]
	private int _maxCellCount;

	private double _damageDealt;

	[SerializeField]
	[Space]
	private MotionTypeBoolArray _motionTypes;

	[SerializeField]
	private AttackTypeBoolArray _attackTypes;

	[SerializeField]
	private DamageAttributeBoolArray _attributes;

	[SerializeField]
	[Space]
	private ValueGauge _gauge;

	[SerializeField]
	private DroppedCell _cellPrefab;

	[SerializeField]
	[Header("Buff Stats(클릭해서 이동 가능)")]
	private StackableStatBonusComponent _brutalityStat;

	[SerializeField]
	private StackableStatBonusComponent _tacticsStat;

	[SerializeField]
	private StackableStatBonusComponent _survivalStat;

	[Header("Effects and Weights")]
	[SerializeField]
	private SoundInfo _scrollSound;

	[Space]
	[SerializeField]
	private Scroll _brutality;

	[SerializeField]
	private Scroll _tactics;

	[SerializeField]
	private Scroll _survival;

	[SerializeField]
	[Space]
	private Scroll _minotaurus;

	[SerializeField]
	private Scroll _assassin;

	[SerializeField]
	private Scroll _guardian;

	[SerializeField]
	[Space]
	private Scroll _epic;

	private Scroll[] _scrolls;

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

	public Sprite icon => _defaultIcon;

	public float iconFillAmount => 0f;

	public int iconStacks { get; protected set; }

	public bool expired => false;

	~PrisonerPassive()
	{
		_defaultIcon = null;
		_walk = null;
		_walk2 = null;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		return this;
	}

	private void AttachEasterEgg()
	{
		if (!_easterAttached)
		{
			_easterAttached = true;
			_remainEasterEggDuration = _easterEggDuration;
			owner.stat.AttachValues(_easterEggStat);
			_characterAnimation.SetWalk(_walk2);
		}
	}

	private void DetachEasterEgg()
	{
		if (_easterAttached)
		{
			_easterAttached = false;
			owner.stat.DetachValues(_easterEggStat);
			_characterAnimation.SetWalk(_walk);
		}
	}

	public void UpdateTime(float deltaTime)
	{
		if (_easterAttached)
		{
			_remainEasterEggDuration -= deltaTime;
			if (_remainEasterEggDuration < 0f)
			{
				DetachEasterEgg();
			}
		}
	}

	public void Refresh()
	{
	}

	public override void Initialize()
	{
		base.Initialize();
		_scrolls = new Scroll[7] { _brutality, _tactics, _survival, _minotaurus, _assassin, _guardian, _epic };
	}

	public void Attach()
	{
		_damageDealt = _totalDamage;
		Character character = owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		_gauge.onChanged += OnGaugeChanged;
	}

	public void Detach()
	{
		Character character = owner;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		_gauge.onChanged -= OnGaugeChanged;
	}

	private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		if (!_motionTypes[gaveDamage.motionType] || !_attackTypes[gaveDamage.attackType] || !_attributes[gaveDamage.attribute] || (Object)(object)target.character == (Object)null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
		{
			return;
		}
		_damageDealt += damageDealt;
		int num = 0;
		while (_damageDealt > _totalDamage)
		{
			_damageDealt -= _totalDamage;
			if (MMMaths.PercentChance(_possibility))
			{
				_cellPrefab.Spawn(Vector2.op_Implicit(gaveDamage.hitPoint), _gauge);
				num++;
				if (num >= _maxCellCount)
				{
					break;
				}
			}
		}
	}

	private Scroll GetScrollToObtain()
	{
		int num = _scrolls.Select((Scroll scroll) => scroll.weight).Sum();
		int num2 = Random.Range(0, num) + 1;
		for (int i = 0; i < _scrolls.Length; i++)
		{
			num2 -= _scrolls[i].weight;
			if (num2 <= 0)
			{
				return _scrolls[i];
			}
		}
		Debug.LogError((object)"Scroll index is exceeded!");
		return _scrolls.Random();
	}

	private void OnGaugeChanged(float oldValue, float newValue)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		if (!(newValue < _gauge.maxValue))
		{
			AttachEasterEgg();
			_gauge.Clear();
			PersistentSingleton<SoundManager>.Instance.PlaySound(_scrollSound, ((Component)owner).transform.position);
			Scroll scrollToObtain = GetScrollToObtain();
			scrollToObtain.effect.Spawn(((Component)owner).transform.position, owner);
			if (scrollToObtain.brutality)
			{
				owner.ability.Add(_brutalityStat.ability);
			}
			if (scrollToObtain.tactics)
			{
				owner.ability.Add(_tacticsStat.ability);
			}
			if (scrollToObtain.survival)
			{
				owner.ability.Add(_survivalStat.ability);
			}
		}
	}
}
