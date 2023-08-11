using GameResources;
using InControl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInput;

namespace UI.Pause;

public class KeyBinder : MonoBehaviour
{
	[SerializeField]
	private Button _button;

	[SerializeField]
	private Image _image;

	private PlayerAction _action;

	private BindingSource _bindingSource;

	public Button button => _button;

	public void Initialize(PlayerAction action, PressNewKey pressNewKey)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		((UnityEvent)_button.onClick).AddListener(new UnityAction(OnClick));
		_action = action;
		_action.OnBindingsChanged += UpdateKeyImageAndBindingSource;
		UpdateKeyImageAndBindingSource();
		void OnClick()
		{
			pressNewKey.ListenForBinding(action, _bindingSource);
		}
	}

	private void OnDisable()
	{
		_action.OnBindingsChanged -= UpdateKeyImageAndBindingSource;
	}

	private void Update()
	{
		UpdateKeyImageAndBindingSource();
	}

	public void UpdateKeyImageAndBindingSource()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		foreach (BindingSource binding in _action.Bindings)
		{
			if (KeyMap.SimplifyBindingSourceType(binding.BindingSourceType) == KeyMapper.Map.SimplifiedLastInputType)
			{
				_bindingSource = binding;
				break;
			}
		}
		if (CommonResource.instance.TryGetKeyIcon(_bindingSource, out var sprite, outline: true))
		{
			_image.sprite = sprite;
			((Graphic)_image).SetNativeSize();
		}
	}
}
