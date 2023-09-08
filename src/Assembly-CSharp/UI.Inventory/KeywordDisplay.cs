using System;
using System.Linq;
using Characters.Gear.Synergy.Inscriptions;
using Hardmode;
using InControl;
using Services;
using Singletons;
using UnityEngine;
using UserInput;

namespace UI.Inventory;

public class KeywordDisplay : MonoBehaviour
{
	[SerializeField]
	private KeywordElement[] _keywordElements;

	[SerializeField]
	private GameObject _detailFrame;

	[SerializeField]
	private GameObject _statFrame;

	[SerializeField]
	private GameObject _gear;

	[SerializeField]
	private GameObject _option;

	[SerializeField]
	private GameObject _upgrades;

	[SerializeField]
	private GameObject _viewDetailKeyGuide;

	private bool _needDetail;

	private int _count;

	public void UpdateElements()
	{
		_detailFrame.SetActive(false);
		_statFrame.SetActive(false);
		Inscription[] array = Singleton<Service>.Instance.levelManager.player.playerComponents.inventory.synergy.inscriptions.Where((Inscription keyword) => keyword.count > 0).OrderByDescending(delegate(Inscription keyword)
		{
			if (keyword.isMaxStep)
			{
				return 2;
			}
			return (keyword.step >= 1) ? 1 : 0;
		}).ThenByDescending((Inscription keyword) => keyword.count)
			.ToArray();
		_count = Math.Min(array.Length, _keywordElements.Length);
		KeywordElement[] keywordElements = _keywordElements;
		for (int i = 0; i < keywordElements.Length; i++)
		{
			((Component)keywordElements[i]).gameObject.SetActive(false);
		}
		for (int j = 0; j < _count; j++)
		{
			if (j < _keywordElements.Length / 2)
			{
				((Component)_keywordElements[j]).gameObject.SetActive(true);
			}
			_keywordElements[j].Set(array[j].key);
		}
	}

	private void OnDisable()
	{
		_gear.SetActive(true);
		_option.SetActive(true);
		for (int i = _keywordElements.Length / 2; i < _count; i++)
		{
			((Component)_keywordElements[i]).gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (!_detailFrame.activeSelf && ((OneAxisInputControl)KeyMapper.Map.UiInteraction2).IsPressed)
		{
			_detailFrame.SetActive(true);
			_statFrame.SetActive(true);
			_gear.SetActive(false);
			_upgrades.SetActive(false);
			_option.SetActive(false);
			for (int i = _keywordElements.Length / 2; i < _count; i++)
			{
				((Component)_keywordElements[i]).gameObject.SetActive(true);
			}
		}
		else if (_detailFrame.activeSelf && !((OneAxisInputControl)KeyMapper.Map.UiInteraction2).IsPressed)
		{
			_gear.SetActive(true);
			_option.SetActive(true);
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				_upgrades.SetActive(true);
			}
			_detailFrame.SetActive(false);
			_statFrame.SetActive(false);
			for (int j = _keywordElements.Length / 2; j < _count; j++)
			{
				((Component)_keywordElements[j]).gameObject.SetActive(false);
			}
		}
	}
}
