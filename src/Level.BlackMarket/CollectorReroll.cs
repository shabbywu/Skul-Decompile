using System;
using Characters;
using Data;
using FX;
using GameResources;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Level.BlackMarket;

public class CollectorReroll : InteractiveObject
{
	private static readonly int _idleHash = Animator.StringToHash("Idle");

	private static readonly int _interactHash = Animator.StringToHash("Interact");

	private int[] _costs;

	private int _refreshCount;

	private int _freeRefreshCount;

	private const string _goldColor = "#FFDE37";

	private const string _notEnoughGoldColor = "#FF0000";

	[SerializeField]
	private SoundInfo _interactionFailedSound;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private TMP_Text _text;

	[SerializeField]
	private UnityEvent _onReroll;

	private bool canRefreshFree
	{
		get
		{
			if (Settings.instance.marketSettings.collectorFreeRefreshCount >= 1)
			{
				return _freeRefreshCount < Settings.instance.marketSettings.collectorFreeRefreshCount;
			}
			return false;
		}
	}

	private int cost
	{
		get
		{
			if (!canRefreshFree)
			{
				return _costs[Math.Min(_refreshCount, _costs.Length - 1)];
			}
			return 0;
		}
	}

	public event Action onInteracted;

	private void OnEnable()
	{
		_costs = Singleton<Service>.Instance.levelManager.currentChapter.collectorRefreshCosts;
		_animator.Play(_idleHash);
	}

	public override void InteractWith(Character character)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		_ = Settings.instance.marketSettings;
		if (canRefreshFree)
		{
			_freeRefreshCount++;
		}
		else
		{
			if (!GameData.Currency.gold.Consume(cost))
			{
				PersistentSingleton<SoundManager>.Instance.PlaySound(_interactionFailedSound, ((Component)this).transform.position);
				return;
			}
			_refreshCount++;
		}
		_animator.Play(_interactHash, 0, 0f);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		this.onInteracted?.Invoke();
		UnityEvent onReroll = _onReroll;
		if (onReroll != null)
		{
			onReroll.Invoke();
		}
	}

	private void Update()
	{
		string arg = (GameData.Currency.gold.Has(cost) ? "#FFDE37" : "#FF0000");
		_text.text = string.Format("{0}(<color={1}>{2}</color>)", Localization.GetLocalizedString("label/interaction/refresh"), arg, cost);
	}
}
