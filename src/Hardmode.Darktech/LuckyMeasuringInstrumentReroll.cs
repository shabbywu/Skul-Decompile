using System;
using System.Collections;
using Characters;
using Data;
using FX;
using GameResources;
using Platforms;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Hardmode.Darktech;

public class LuckyMeasuringInstrumentReroll : InteractiveObject
{
	private static readonly int _idleFirstHash = Animator.StringToHash("LMI_First_Stop");

	private static readonly int _interactFirstHash = Animator.StringToHash("LMI_First_Move");

	private static readonly int _idleSecondHash = Animator.StringToHash("LMI_Second_Stop");

	private static readonly int _interactSecondHash = Animator.StringToHash("LMI_Second_Move");

	private static readonly int _endZeroHash = Animator.StringToHash("LMI_Deactivate");

	private static readonly int _endFirstHash = Animator.StringToHash("LMI_First_Get");

	private static readonly int _endSecondHash = Animator.StringToHash("LMI_End");

	[SerializeField]
	private SoundInfo _interactionFailedSound;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private TMP_Text _rerollPrice;

	[SerializeField]
	private TMP_Text _rerollCount;

	[SerializeField]
	private TMP_Text _text;

	[SerializeField]
	private UnityEvent _onReroll;

	private LuckyMeasuringInstrument _base;

	private int _cost;

	public LuckyMeasuringInstrument @base
	{
		get
		{
			return _base;
		}
		set
		{
			_base = value;
			if (@base.remainLootCount == 2)
			{
				_animator.Play(_idleFirstHash);
			}
			else if (@base.remainLootCount == 1)
			{
				_animator.Play(_idleSecondHash);
			}
			else
			{
				_animator.Play(_endSecondHash);
			}
		}
	}

	public int lootCount => @base.lootCount;

	public int remainLootCount => @base.remainLootCount;

	private GameData.Currency rerollCurrency => GameData.Currency.darkQuartz;

	public event Action onInteracted;

	private void OnEnable()
	{
		_cost = Singleton<DarktechManager>.Instance.setting.행운계측기설정.refreshPrice;
		UpdateInteractionGuide();
	}

	public void Initialize()
	{
		IntData refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
		int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
		if (refreshCount.value >= maxRefreshCount)
		{
			ExtensionMethods.Set((Type)70);
			Deactivate();
		}
	}

	public override void InteractWith(Character character)
	{
		if (!rerollCurrency.Consume(_cost))
		{
			FailReroll();
			return;
		}
		IntData refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
		int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
		if (refreshCount.value >= maxRefreshCount)
		{
			FailReroll();
		}
		else
		{
			Reroll();
		}
	}

	private void FailReroll()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactionFailedSound, ((Component)this).transform.position);
	}

	public void Reroll()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		int num = ((remainLootCount == lootCount) ? _interactFirstHash : _interactSecondHash);
		_animator.Play(num, 0, 0f);
		IntData refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
		int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
		refreshCount.value++;
		UpdateInteractionGuide();
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		this.onInteracted?.Invoke();
		UnityEvent onReroll = _onReroll;
		if (onReroll != null)
		{
			onReroll.Invoke();
		}
		if (refreshCount.value >= maxRefreshCount)
		{
			ExtensionMethods.Set((Type)70);
			Deactivate();
		}
	}

	private IEnumerator CPlayEndAnimation()
	{
		yield return Chronometer.global.WaitForSeconds(1f);
		PlayEndAnimation();
	}

	public void PlayZeroDeactivate()
	{
		_animator.Play(_endZeroHash, 0, 0f);
	}

	public void PlayFirstEnd()
	{
		_animator.Play(_endFirstHash, 0, 0f);
	}

	public void PlayEndAnimation()
	{
		_animator.Play(_endSecondHash, 0, 0f);
	}

	public override void OnDeactivate()
	{
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		base.OnDeactivate();
		IntData refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
		int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
		if (refreshCount.value > maxRefreshCount)
		{
			Debug.Log((object)$"remainlootCount 1 {remainLootCount}");
			if (remainLootCount == 2)
			{
				((Graphic)_rerollCount).color = Color.gray;
				UpdateInteractionGuide();
				PlayZeroDeactivate();
			}
			else if (remainLootCount == 1)
			{
				((Graphic)_rerollCount).color = Color.gray;
				UpdateInteractionGuide();
				PlayFirstEnd();
			}
			else
			{
				((MonoBehaviour)this).StartCoroutine(CPlayEndAnimation());
			}
			return;
		}
		Debug.Log((object)$"remainlootCount 2 {remainLootCount}");
		if (remainLootCount == 2)
		{
			((Graphic)_rerollCount).color = Color.gray;
			UpdateInteractionGuide();
			PlayZeroDeactivate();
		}
		else if (remainLootCount == 1)
		{
			((Graphic)_rerollCount).color = Color.gray;
			UpdateInteractionGuide();
			PlayFirstEnd();
		}
		else
		{
			PlayEndAnimation();
			_rerollCount.text = "-----";
		}
	}

	public override void OpenPopupBy(Character character)
	{
		base.OpenPopupBy(character);
		UpdateInteractionGuide();
	}

	private void UpdateInteractionGuide()
	{
		IntData refreshCount = GameData.HardmodeProgress.luckyMeasuringInstrument.refreshCount;
		int maxRefreshCount = Singleton<DarktechManager>.Instance.setting.행운계측기설정.maxRefreshCount;
		string text = (GameData.Currency.darkQuartz.Has(_cost) ? GameData.Currency.darkQuartz.colorCode : GameData.Currency.noMoneyColorCode);
		int num = maxRefreshCount - refreshCount.value;
		_rerollCount.text = $"{num}";
		_text.text = string.Format("{0}( {1}  <color=#{2}>{3}</color> )", Localization.GetLocalizedString("label/interaction/refresh"), GameData.Currency.darkQuartz.spriteTMPKey, text, _cost);
		if (num <= 0)
		{
			ClosePopup();
			_uiObject = null;
			_uiObjects = (GameObject[])(object)new GameObject[0];
		}
	}
}
