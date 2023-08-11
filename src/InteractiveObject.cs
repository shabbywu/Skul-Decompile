using System;
using Characters;
using FX;
using Singletons;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
	protected static readonly Vector2 _popupUIOffset = new Vector2(5f, 2f);

	protected static readonly int _activateHash = Animator.StringToHash("Activate");

	protected static readonly int _deactivateHash = Animator.StringToHash("Deactivate");

	[SerializeField]
	protected CharacterInteraction.InteractionType _interactionType;

	public bool autoInteract;

	[Space]
	[SerializeField]
	protected SoundInfo _activateSound;

	[SerializeField]
	protected SoundInfo _deactivateSound;

	[SerializeField]
	protected SoundInfo _interactSound;

	[SerializeField]
	[Tooltip("모든 오브젝트에서 작동하는 건 아니며 코드상에서 직접 설정해주어야 함")]
	protected SoundInfo _interactFailSound;

	[SerializeField]
	[Space]
	protected GameObject _uiObject;

	[SerializeField]
	protected GameObject[] _uiObjects;

	[SerializeField]
	[Space]
	protected bool _activated = true;

	protected Character _character;

	[NonSerialized]
	public float pressingPercent;

	protected virtual bool _interactable => true;

	public virtual CharacterInteraction.InteractionType interactionType => _interactionType;

	public bool popupVisible => (Object)(object)_character != (Object)null;

	public bool activated
	{
		get
		{
			return _activated;
		}
		private set
		{
			_activated = value;
		}
	}

	public bool interactable => _interactable;

	protected virtual void Awake()
	{
		ClosePopup();
	}

	private void OnDisable()
	{
		_activated = false;
	}

	public void Activate()
	{
		_activated = true;
		OnActivate();
	}

	public void Deactivate()
	{
		ClosePopup();
		_activated = false;
		OnDeactivate();
	}

	public virtual void OnActivate()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_activateSound, ((Component)this).transform.position);
	}

	public virtual void OnDeactivate()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_deactivateSound, ((Component)this).transform.position);
	}

	public virtual void InteractWith(Character character)
	{
	}

	public virtual void InteractWithByPressing(Character character)
	{
	}

	public virtual void OpenPopupBy(Character character)
	{
		_character = character;
		pressingPercent = 0f;
		GameObject[] uiObjects = _uiObjects;
		foreach (GameObject val in uiObjects)
		{
			if (!((Object)(object)val == (Object)null) && !val.activeSelf)
			{
				val.SetActive(true);
			}
		}
		if (!((Object)(object)_uiObject == (Object)null) && !_uiObject.activeSelf && !autoInteract)
		{
			_uiObject.SetActive(true);
		}
	}

	public virtual void ClosePopup()
	{
		_character = null;
		GameObject[] uiObjects = _uiObjects;
		foreach (GameObject val in uiObjects)
		{
			if ((Object)(object)val != (Object)null)
			{
				val.SetActive(false);
			}
		}
		if ((Object)(object)_uiObject != (Object)null)
		{
			_uiObject.SetActive(false);
		}
	}
}
