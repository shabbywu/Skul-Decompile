using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory;

public class GearElement : Button
{
	public Action onSelected;

	[SerializeField]
	private Image _placeholder;

	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Image _shadowIcon;

	[SerializeField]
	private Image _setImage;

	[SerializeField]
	private Animator _setAnimator;

	[SerializeField]
	private Shadow _shadow;

	public override void OnSelect(BaseEventData eventData)
	{
		((Selectable)this).OnSelect(eventData);
		onSelected?.Invoke();
	}

	public void SetIcon(Sprite sprite)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)_placeholder).color = new Color(1f, 1f, 1f, 0f);
		((Behaviour)_icon).enabled = true;
		_icon.sprite = sprite;
		((Graphic)_icon).SetNativeSize();
		((Behaviour)_shadowIcon).enabled = true;
		_shadowIcon.sprite = sprite;
		((Graphic)_shadowIcon).SetNativeSize();
	}

	public void SetSetImage(Sprite image)
	{
		((Behaviour)_setImage).enabled = true;
		_setImage.sprite = image;
		((Graphic)_setImage).SetNativeSize();
	}

	public void SetSetAnimator(RuntimeAnimatorController animatorController)
	{
		((Behaviour)_setAnimator).enabled = true;
		_setAnimator.runtimeAnimatorController = animatorController;
		_setAnimator.Update(0f);
		((Graphic)_setImage).SetNativeSize();
	}

	public void DisableSetEffect()
	{
		((Behaviour)_setImage).enabled = false;
		((Behaviour)_setAnimator).enabled = false;
	}

	public void Deactivate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)_placeholder).color = Color.white;
		((Behaviour)_icon).enabled = false;
		((Behaviour)_shadowIcon).enabled = false;
		((Behaviour)_setImage).enabled = false;
		((Behaviour)_setAnimator).enabled = false;
	}
}
