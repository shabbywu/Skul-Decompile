using Characters;
using FX;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Witch;

public class TreeElement : MonoBehaviour, ISelectHandler, IEventSystemHandler
{
	private Panel _panel;

	[SerializeField]
	private Button _button;

	[SerializeField]
	private TMP_Text _level;

	[SerializeField]
	private GameObject _ready;

	[SerializeField]
	private GameObject _mastered;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private Image _deactivateMask;

	private WitchBonus.Bonus _bonus;

	[SerializeField]
	private SoundInfo _getAbility;

	[SerializeField]
	private SoundInfo _select;

	public bool interactable
	{
		get
		{
			return ((Selectable)_button).interactable;
		}
		set
		{
			((Selectable)_button).interactable = value;
		}
	}

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		UnityAction val = (UnityAction)delegate
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			PersistentSingleton<SoundManager>.Instance.PlaySound(_getAbility, ((Component)this).transform.position);
			_bonus.LevelUp();
			_level.text = _bonus.level.ToString();
			_panel.UpdateCurrentOption();
		};
		((UnityEvent)_button.onClick).AddListener(val);
	}

	public void Initialize(Panel panel)
	{
		_panel = panel;
	}

	public void Set(WitchBonus.Bonus bonus)
	{
		_bonus = bonus;
		_name.text = _bonus.displayName;
		_level.text = _bonus.level.ToString();
	}

	public void OnSelect(BaseEventData eventData)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_select, ((Component)this).transform.position);
		_panel.Set(_bonus);
	}

	private void Update()
	{
		((Behaviour)_deactivateMask).enabled = !_bonus.ready;
		if ((Object)(object)_ready != (Object)null && !_ready.activeSelf && _bonus.ready)
		{
			_ready.SetActive(true);
		}
		if ((Object)(object)_mastered != (Object)null && !_mastered.activeSelf && _bonus.level == _bonus.maxLevel)
		{
			_mastered.SetActive(true);
		}
	}
}
