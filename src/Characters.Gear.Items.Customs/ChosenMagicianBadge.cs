using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.Gear.Items.Customs;

public sealed class ChosenMagicianBadge : MonoBehaviour
{
	[SerializeField]
	private Item _item;

	[SerializeField]
	private float _radius = 10f;

	[SerializeField]
	private float _fastTime;

	[SerializeField]
	private float _normalSpeed = 100f;

	[SerializeField]
	private float _fastSpeed = 300f;

	[SerializeField]
	private ChosenMagicianBadgeOrb[] _orbs;

	private float _remainTime;

	private bool _fast;

	private void Start()
	{
		float num = 0f;
		ChosenMagicianBadgeOrb[] orbs = _orbs;
		for (int i = 0; i < orbs.Length; i++)
		{
			orbs[i].Initialize(num);
			num += (float)Math.PI * 2f / (float)_orbs.Length;
		}
	}

	private void OnEnable()
	{
		_item.owner.onStartAction += HandleOnStartAction;
	}

	private void OnDisable()
	{
		_item.owner.onStartAction -= HandleOnStartAction;
	}

	private void HandleOnStartAction(Characters.Actions.Action action)
	{
		if (action.type == Characters.Actions.Action.Type.Skill)
		{
			_fast = true;
			_remainTime = _fastTime;
			ChosenMagicianBadgeOrb[] orbs = _orbs;
			for (int i = 0; i < orbs.Length; i++)
			{
				orbs[i].ChangeToFast();
			}
		}
	}

	public void Show()
	{
		((Component)this).gameObject.SetActive(true);
	}

	private void Update()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		_remainTime -= ((ChronometerBase)Chronometer.global).deltaTime;
		((Component)this).transform.Rotate(Vector3.forward, (_fast ? _fastSpeed : _normalSpeed) * ((ChronometerBase)Chronometer.global).deltaTime, (Space)1);
		if (_remainTime <= 0f && _fast)
		{
			_fast = false;
			ChosenMagicianBadgeOrb[] orbs = _orbs;
			for (int i = 0; i < orbs.Length; i++)
			{
				orbs[i].ChangeToNormal();
			}
		}
	}
}
