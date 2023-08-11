using System;
using System.Runtime.CompilerServices;
using Data;
using GameResources;
using InControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInput;

namespace UI.Pause;

public class Controls : Dialogue
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static UnityAction _003C_003E9__20_0;

		public static Action<int> _003C_003E9__20_2;

		internal void _003CAwake_003Eb__20_0()
		{
			KeyMapper.Map.ResetToDefault();
		}

		internal void _003CAwake_003Eb__20_2(int v)
		{
			GameData.Settings.arrowDashEnabled = v == 1;
		}
	}

	[SerializeField]
	private Panel _panel;

	[SerializeField]
	private PressNewKey _pressNewKey;

	[SerializeField]
	private KeyBinder _up;

	[SerializeField]
	private KeyBinder _down;

	[SerializeField]
	private KeyBinder _left;

	[SerializeField]
	private KeyBinder _right;

	[SerializeField]
	private KeyBinder _attack;

	[SerializeField]
	private KeyBinder _jump;

	[SerializeField]
	private KeyBinder _dash;

	[SerializeField]
	private Toggle _arrowDash;

	[SerializeField]
	private KeyBinder _swap;

	[SerializeField]
	private KeyBinder _skill1;

	[SerializeField]
	private KeyBinder _skill2;

	[SerializeField]
	private KeyBinder _quintessence;

	[SerializeField]
	private KeyBinder _inventory;

	[SerializeField]
	private KeyBinder _interaction;

	[SerializeField]
	private Button _reset;

	[SerializeField]
	private Button _return;

	public override bool closeWithPauseKey => false;

	private void Awake()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		ButtonClickedEvent onClick = _reset.onClick;
		object obj = _003C_003Ec._003C_003E9__20_0;
		if (obj == null)
		{
			UnityAction val = delegate
			{
				KeyMapper.Map.ResetToDefault();
			};
			_003C_003Ec._003C_003E9__20_0 = val;
			obj = (object)val;
		}
		((UnityEvent)onClick).AddListener((UnityAction)obj);
		((UnityEvent)_return.onClick).AddListener((UnityAction)delegate
		{
			_panel.state = Panel.State.Menu;
		});
		_up.Initialize(KeyMapper.Map.Up, _pressNewKey);
		_down.Initialize(KeyMapper.Map.Down, _pressNewKey);
		_left.Initialize(KeyMapper.Map.Left, _pressNewKey);
		_right.Initialize(KeyMapper.Map.Right, _pressNewKey);
		_attack.Initialize(KeyMapper.Map.Attack, _pressNewKey);
		_jump.Initialize(KeyMapper.Map.Jump, _pressNewKey);
		_dash.Initialize(KeyMapper.Map.Dash, _pressNewKey);
		ArrowDashText();
		_arrowDash.value = (GameData.Settings.arrowDashEnabled ? 1 : 0);
		_arrowDash.onValueChanged += delegate(int v)
		{
			GameData.Settings.arrowDashEnabled = v == 1;
		};
		_swap.Initialize(KeyMapper.Map.Swap, _pressNewKey);
		_skill1.Initialize(KeyMapper.Map.Skill1, _pressNewKey);
		_skill2.Initialize(KeyMapper.Map.Skill2, _pressNewKey);
		_quintessence.Initialize(KeyMapper.Map.Quintessence, _pressNewKey);
		_inventory.Initialize(KeyMapper.Map.Inventory, _pressNewKey);
		_interaction.Initialize(KeyMapper.Map.Interaction, _pressNewKey);
	}

	protected override void OnEnable()
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		base.OnEnable();
		_up.UpdateKeyImageAndBindingSource();
		_down.UpdateKeyImageAndBindingSource();
		_left.UpdateKeyImageAndBindingSource();
		_right.UpdateKeyImageAndBindingSource();
		_attack.UpdateKeyImageAndBindingSource();
		_jump.UpdateKeyImageAndBindingSource();
		_dash.UpdateKeyImageAndBindingSource();
		ArrowDashText();
		_swap.UpdateKeyImageAndBindingSource();
		_skill1.UpdateKeyImageAndBindingSource();
		_skill2.UpdateKeyImageAndBindingSource();
		_quintessence.UpdateKeyImageAndBindingSource();
		_inventory.UpdateKeyImageAndBindingSource();
		_interaction.UpdateKeyImageAndBindingSource();
		KeyMapper.Map.OnSimplifiedLastInputTypeChanged += OnSimplifiedLastInputTypeChanged;
		OnSimplifiedLastInputTypeChanged(KeyMapper.Map.SimplifiedLastInputType);
		SetInitialFocus();
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameData.Settings.keyBindings = ((PlayerActionSet)KeyMapper.Map).Save();
	}

	private void SetInitialFocus()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Invalid comparison between Unknown and I4
		if ((int)KeyMapper.Map.SimplifiedLastInputType == 2)
		{
			Focus((Selectable)(object)_up.button);
			return;
		}
		((Component)_up).gameObject.SetActive(false);
		((Component)_up).gameObject.SetActive(true);
		((Component)_down).gameObject.SetActive(false);
		((Component)_down).gameObject.SetActive(true);
		((Component)_left).gameObject.SetActive(false);
		((Component)_left).gameObject.SetActive(true);
		((Component)_right).gameObject.SetActive(false);
		((Component)_right).gameObject.SetActive(true);
		((Component)_inventory).gameObject.SetActive(false);
		((Component)_inventory).gameObject.SetActive(true);
		Focus((Selectable)(object)_arrowDash);
	}

	private void OnSimplifiedLastInputTypeChanged(BindingSourceType bindingSourceType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		if ((int)bindingSourceType == 2)
		{
			((Selectable)_up.button).interactable = true;
			((Selectable)_down.button).interactable = true;
			((Selectable)_left.button).interactable = true;
			((Selectable)_right.button).interactable = true;
			((Selectable)_inventory.button).interactable = true;
			Focus((Selectable)(object)_up.button);
			return;
		}
		((Selectable)_up.button).interactable = false;
		((Selectable)_down.button).interactable = false;
		((Selectable)_left.button).interactable = false;
		((Selectable)_right.button).interactable = false;
		((Selectable)_inventory.button).interactable = false;
		((Component)_up).gameObject.SetActive(false);
		((Component)_up).gameObject.SetActive(true);
		((Component)_down).gameObject.SetActive(false);
		((Component)_down).gameObject.SetActive(true);
		((Component)_left).gameObject.SetActive(false);
		((Component)_left).gameObject.SetActive(true);
		((Component)_right).gameObject.SetActive(false);
		((Component)_right).gameObject.SetActive(true);
		((Component)_inventory).gameObject.SetActive(false);
		((Component)_inventory).gameObject.SetActive(true);
		Focus((Selectable)(object)_arrowDash);
	}

	private void ArrowDashText()
	{
		_arrowDash.SetTexts(Localization.GetLocalizedStrings("label/pause/settings/off", "label/pause/settings/on"));
	}
}
