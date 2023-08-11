using System;
using System.Collections;
using FX;
using GameResources;
using InControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UserInput;

namespace UI;

public class PressingButton : MonoBehaviour
{
	private const float _pressingTime = 1f;

	[SerializeField]
	[Space]
	private bool _detectPressingSelf;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Image _iconOutline;

	[SerializeField]
	private TMP_Text _text;

	[Space]
	[SerializeField]
	private string _actionName;

	[Space]
	[SerializeField]
	private PlaySoundInfo _pressingSound;

	private PlayerAction _action;

	private CoroutineReference _pressing;

	public string description
	{
		get
		{
			return _text.text;
		}
		set
		{
			_text.text = value;
		}
	}

	public event Action onPressed;

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
		StopPressingSound();
		if (_detectPressingSelf)
		{
			((MonoBehaviour)this).StartCoroutine(CWaitForPressing());
		}
		UpdateImage();
	}

	private void OnDisable()
	{
		StopPressing();
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

	public void PlayPressingSound()
	{
		_pressingSound.Play();
	}

	public void StopPressingSound()
	{
		_pressingSound.Stop();
	}

	public void StopPressing()
	{
		((CoroutineReference)(ref _pressing)).Stop();
		_iconOutline.fillAmount = 1f;
		StopPressingSound();
	}

	public void SetPercent(float percent)
	{
		_iconOutline.fillAmount = percent;
	}

	private void OnLastInputTypeChanged(BindingSourceType bindingSourceType)
	{
		UpdateImage();
	}

	private void UpdateImage()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_icon != (Object)null)
		{
			((Graphic)_icon).SetNativeSize();
		}
		BindingSourceType simplifiedLastInputType = KeyMapper.Map.SimplifiedLastInputType;
		foreach (BindingSource binding in _action.Bindings)
		{
			BindingSourceType val = KeyMap.SimplifyBindingSourceType(binding.BindingSourceType);
			if (simplifiedLastInputType == val)
			{
				Sprite keyIconOrDefault = CommonResource.instance.GetKeyIconOrDefault(binding);
				if ((Object)(object)_icon != (Object)null)
				{
					_icon.sprite = keyIconOrDefault;
					((Graphic)_icon).SetNativeSize();
				}
				keyIconOrDefault = CommonResource.instance.GetKeyIconOrDefault(binding, outline: true);
				if ((Object)(object)_icon != (Object)null)
				{
					_iconOutline.sprite = keyIconOrDefault;
					((Graphic)_iconOutline).SetNativeSize();
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

	private IEnumerator CWaitForPressing()
	{
		while (true)
		{
			if (((OneAxisInputControl)_action).WasPressed)
			{
				_pressing = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CPressing());
			}
			yield return null;
		}
	}

	private IEnumerator CPressing()
	{
		PlayPressingSound();
		for (float time = 0f; time < 1f; time += Time.unscaledDeltaTime)
		{
			if (!((OneAxisInputControl)_action).IsPressed)
			{
				StopPressing();
				yield break;
			}
			yield return null;
			SetPercent(time / 1f);
		}
		StopPressingSound();
		_iconOutline.fillAmount = 1f;
		this.onPressed?.Invoke();
	}
}
