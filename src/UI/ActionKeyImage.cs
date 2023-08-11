using System;
using GameResources;
using InControl;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI;

public class ActionKeyImage : MonoBehaviour
{
	[SerializeField]
	private Image _image;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private string _actionName;

	[SerializeField]
	private bool _outline;

	private PlayerAction _action;

	private void Awake()
	{
		_action = FindAction();
		if (_action == null)
		{
			throw new Exception("Couldn't found key " + _actionName);
		}
		KeyMapper.Map.OnSimplifiedLastInputTypeChanged += OnLastInputTypeChanged;
		_action.OnBindingsChanged += UpdateImage;
	}

	private void OnEnable()
	{
		UpdateImage();
	}

	private void Start()
	{
		UpdateImage();
	}

	private void OnDestroy()
	{
		KeyMapper.Map.OnSimplifiedLastInputTypeChanged -= OnLastInputTypeChanged;
		_action.OnBindingsChanged -= UpdateImage;
	}

	private void OnLastInputTypeChanged(BindingSourceType bindingSourceType)
	{
		UpdateImage();
	}

	private void UpdateImage()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_image != (Object)null)
		{
			((Graphic)_image).SetNativeSize();
		}
		BindingSourceType simplifiedLastInputType = KeyMapper.Map.SimplifiedLastInputType;
		foreach (BindingSource binding in _action.Bindings)
		{
			BindingSourceType val = KeyMap.SimplifyBindingSourceType(binding.BindingSourceType);
			if (simplifiedLastInputType == val)
			{
				Sprite keyIconOrDefault = CommonResource.instance.GetKeyIconOrDefault(binding, _outline);
				if ((Object)(object)_image != (Object)null)
				{
					_image.sprite = keyIconOrDefault;
					((Graphic)_image).SetNativeSize();
				}
				if ((Object)(object)_spriteRenderer != (Object)null)
				{
					_spriteRenderer.sprite = keyIconOrDefault;
				}
				break;
			}
		}
	}

	private PlayerAction FindAction()
	{
		foreach (PlayerAction action in ((PlayerActionSet)KeyMapper.Map).Actions)
		{
			if (action.Name.Equals(_actionName, StringComparison.OrdinalIgnoreCase))
			{
				return action;
			}
		}
		return null;
	}
}
