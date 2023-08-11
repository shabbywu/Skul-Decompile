using System;
using Data;
using GameResources;
using Level;
using UnityEngine;

namespace Characters.Gear;

public abstract class Gear : MonoBehaviour
{
	public enum Type
	{
		Weapon,
		Item,
		Quintessence,
		Upgrade
	}

	public enum State
	{
		Dropped,
		Equipped
	}

	[Flags]
	public enum Tag
	{
		Carleon = 1,
		Skeleton = 2,
		Spirit = 4,
		Omen = 8,
		UpgradedOmen = 0x10
	}

	protected Action<Gear> _onDiscard;

	[Space]
	[Tooltip("입수가능")]
	public bool obtainable = true;

	[Tooltip("파괴가능")]
	public bool destructible = true;

	[Tooltip("해금해야 드랍되는지, obtainable이 false이면 어쨌든 입수 불가능이므로 주의")]
	public bool needUnlock;

	[SerializeField]
	private Sprite _unlockIcon;

	[SerializeField]
	[Space]
	private Rarity _rarity;

	[EnumFlag]
	[SerializeField]
	private Tag _gearTag;

	[SerializeField]
	[Space]
	protected Stat.Values _stat;

	[SerializeField]
	private DroppedGear _dropped;

	[SerializeField]
	private GameObject _equipped;

	[Space]
	[SerializeField]
	private string[] _setItemKeys;

	[SerializeField]
	private Sprite _setItemImage;

	[SerializeField]
	private RuntimeAnimatorController _setItemAnimator;

	[Tooltip("이 아이템을 소지하고 있을 경우 드랍될 수 없는 아이템")]
	[SerializeField]
	[Space]
	private string[] _groupItemKeys;

	private State _state;

	public Sprite unlockIcon
	{
		get
		{
			if (!((Object)(object)_unlockIcon == (Object)null))
			{
				return _unlockIcon;
			}
			return GearResource.instance.GetItemBuffIcon(((Object)this).name);
		}
	}

	public Sprite defaultUnlockIcon => _unlockIcon;

	public abstract Type type { get; }

	public Rarity rarity => _rarity;

	public Tag gearTag => _gearTag;

	public Stat.Values stat => _stat;

	public DroppedGear dropped => _dropped;

	public GameObject equipped => _equipped;

	public bool lootable { get; set; } = true;


	public string[] setItemKeys => _setItemKeys;

	public Sprite setItemImage => _setItemImage;

	public RuntimeAnimatorController setItemAnimator => _setItemAnimator;

	public string[] groupItemKeys => _groupItemKeys;

	protected abstract string _prefix { get; }

	protected string _keyBase => _prefix + "/" + ((Object)this).name;

	public string displayNameKey => _keyBase + "/name";

	public string displayName => Localization.GetLocalizedString(_keyBase + "/name");

	public string description => Localization.GetLocalizedString(_keyBase + "/desc");

	public string flavor => Localization.GetLocalizedString(_keyBase + "/flavor");

	public string typeDisplayName => Localization.GetLocalizedString("label/" + _prefix + "/name");

	public bool hasFlavor => !string.IsNullOrWhiteSpace(flavor);

	public Sprite icon => dropped.spriteRenderer.sprite;

	public virtual Sprite thumbnail => GearResource.instance.GetGearThumbnail(((Object)this).name) ?? icon;

	public virtual GameData.Currency.Type currencyTypeByDiscard => GameData.Currency.Type.Gold;

	public virtual int currencyByDiscard => 0;

	public State state
	{
		get
		{
			return _state;
		}
		set
		{
			if (_state != value)
			{
				_state = value;
				switch (_state)
				{
				case State.Dropped:
					OnDropped();
					break;
				case State.Equipped:
					OnEquipped();
					break;
				}
			}
		}
	}

	public Character owner { get; protected set; }

	public event Action onDropped;

	public event Action onEquipped;

	public event Action<Gear> onDiscard
	{
		add
		{
			_onDiscard = (Action<Gear>)Delegate.Combine(_onDiscard, value);
		}
		remove
		{
			_onDiscard = (Action<Gear>)Delegate.Remove(_onDiscard, value);
		}
	}

	protected virtual void Awake()
	{
		if ((Object)(object)_dropped != (Object)null)
		{
			_dropped.onLoot += OnLoot;
		}
		OnDropped();
	}

	public virtual void Initialize()
	{
		_dropped?.Initialize(this);
	}

	protected abstract void OnLoot(Character character);

	protected virtual void OnDropped()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.parent = ((Component)Map.Instance).transform;
		((Component)this).transform.localScale = Vector3.one;
		if ((Object)(object)_equipped != (Object)null)
		{
			_equipped.SetActive(false);
		}
		if ((Object)(object)_dropped != (Object)null)
		{
			((Component)_dropped).gameObject.SetActive(true);
		}
		this.onDropped?.Invoke();
	}

	protected virtual void OnEquipped()
	{
		if ((Object)(object)_dropped != (Object)null)
		{
			((Component)_dropped).gameObject.SetActive(false);
		}
		if ((Object)(object)_equipped != (Object)null)
		{
			_equipped.SetActive(true);
		}
		this.onEquipped?.Invoke();
	}
}
