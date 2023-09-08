using System;
using FX;
using GameResources;
using Level;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Abilities;

public class AbilityBuff : MonoBehaviour, IAbility, IAbilityInstance
{
	[SerializeField]
	private AbilityBuffDisplay _display;

	[SerializeField]
	private Rarity _rarity;

	[SerializeField]
	[FormerlySerializedAs("_healAmount")]
	private int _healingPercent;

	[SerializeField]
	private int _durationMaps = 3;

	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private SoundInfo _lootSound;

	[SerializeField]
	private EffectInfo _loopEffect = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	[AbilityAttacher.Subcomponent]
	private AbilityAttacher.Subcomponents _abilityAttacher;

	[NonSerialized]
	public int remainMaps;

	private EffectPoolInstance _loopEffectInstance;

	private IStackable _stackable;

	private const string _prefix = "abilityBuff";

	protected string _keyBase => "abilityBuff/" + ((Object)this).name;

	public string displayName => Localization.GetLocalizedString(_keyBase + "/name");

	public string description => Localization.GetLocalizedString(_keyBase + "/desc");

	public Rarity rarity => _rarity;

	public int price { get; set; }

	public Character owner { get; private set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached { get; private set; }

	public Sprite icon => _icon;

	public float iconFillAmount => 0f;

	public int iconStacks
	{
		get
		{
			if (_stackable != null)
			{
				return (int)_stackable.stack;
			}
			return remainMaps;
		}
	}

	public bool expired => false;

	public float duration { get; set; }

	public int iconPriority => 0;

	public bool iconFillInversed => false;

	public bool iconFillFlipped => false;

	public bool removeOnSwapWeapon => false;

	public float stack
	{
		get
		{
			if (_stackable != null)
			{
				return _stackable.stack;
			}
			return 0f;
		}
		set
		{
			if (_stackable != null)
			{
				_stackable.stack = value;
			}
		}
	}

	public bool stackable => _stackable != null;

	public event Action onSold;

	public IAbilityInstance CreateInstance(Character owner)
	{
		return this;
	}

	private void Awake()
	{
		_stackable = ((Component)this).GetComponentInChildren<IStackable>();
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= OnMapChagned;
		}
	}

	public void Attach(Character character)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		((Component)_display).gameObject.SetActive(false);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_lootSound, ((Component)this).transform.position);
		owner = character;
		((Component)this).transform.parent = ((Component)owner).transform;
		((Component)this).transform.localPosition = Vector3.zero;
		if (_abilityAttacher.components.Length == 0)
		{
			return;
		}
		_abilityAttacher.Initialize(owner);
		if (stackable)
		{
			foreach (AbilityBuff item in owner.ability.GetInstancesByInstanceType<AbilityBuff>())
			{
				if (((Object)item).name.Equals(((Object)this).name, StringComparison.OrdinalIgnoreCase))
				{
					item.stack++;
					return;
				}
			}
		}
		owner.ability.Add(this);
	}

	public void Loot(Character character)
	{
		Attach(character);
		owner.health.PercentHeal((float)_healingPercent * 0.01f);
	}

	private void OnMapChagned(Map old, Map @new)
	{
		if (@new.waveContainer.enemyWaves.Length != 0)
		{
			remainMaps--;
			if (remainMaps == 0)
			{
				owner.ability.Remove(this);
				_abilityAttacher.StopAttach();
				Object.Destroy((Object)(object)((Component)this).gameObject);
			}
		}
	}

	public void UpdateTime(float deltaTime)
	{
	}

	public void Refresh()
	{
		remainTime = duration;
	}

	public void Attach()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		attached = true;
		remainMaps = _durationMaps;
		_loopEffectInstance = ((_loopEffect == null) ? null : _loopEffect.Spawn(((Component)owner).transform.position, owner));
		Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += OnMapChagned;
		_abilityAttacher.StartAttach();
	}

	public void Detach()
	{
		attached = false;
		if ((Object)(object)_loopEffectInstance != (Object)null)
		{
			_loopEffectInstance.Stop();
		}
		Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= OnMapChagned;
	}

	public void Initialize()
	{
		_display.Initialize(this);
		_display.price = price;
		_display.onLoot += Loot;
		_display.onLoot += delegate
		{
			this.onSold?.Invoke();
		};
	}

	public override string ToString()
	{
		return $"{((Object)this).name}|{remainMaps}|{stack}";
	}

	public static (string name, int durationMaps, int stack) Parse(string text)
	{
		string[] array = text.Split(new char[1] { '|' });
		return (array[0], int.Parse(array[1]), int.Parse(array[2]));
	}
}
