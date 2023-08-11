using System;
using System.Collections.Generic;
using Data;
using Hardmode.Darktech;
using Singletons;
using UnityEngine;

namespace Level.Npc;

public sealed class AnotherWitchSelector : MonoBehaviour
{
	[Serializable]
	private class WitchsByLevel
	{
		[SerializeField]
		private GameObject[] _witchs;

		public GameObject GetCutsceneWitch()
		{
			return _witchs[0];
		}

		public GameObject GetRandomWitch()
		{
			return ExtensionMethods.Random<GameObject>((IEnumerable<GameObject>)_witchs);
		}

		public void Hide()
		{
			GameObject[] witchs = _witchs;
			for (int i = 0; i < witchs.Length; i++)
			{
				witchs[i].SetActive(false);
			}
		}
	}

	private readonly int _originalIdleHash = Animator.StringToHash("Original_Idle");

	private readonly int _originalIdleNoWitchHash = Animator.StringToHash("Original_Idle_NoWitch");

	private readonly int _blackOutHash = Animator.StringToHash("BlackOut_Loop");

	[SerializeField]
	private Animator _originalWitchAnimator;

	[SerializeField]
	private GameObject _originalInteractive;

	[SerializeField]
	private WitchsByLevel[] _witchByHardmodeLevel;

	private void Awake()
	{
		_originalInteractive.SetActive(false);
		WitchsByLevel[] witchByHardmodeLevel = _witchByHardmodeLevel;
		for (int i = 0; i < witchByHardmodeLevel.Length; i++)
		{
			witchByHardmodeLevel[i].Hide();
		}
		int clearedLevel = GameData.HardmodeProgress.clearedLevel;
		if (clearedLevel < 0)
		{
			_originalWitchAnimator.Play(_blackOutHash);
			return;
		}
		DarktechManager.DarktechByLevel darktechByLevel = Singleton<DarktechManager>.Instance.darkTechByLevels[clearedLevel];
		if (!Singleton<DarktechManager>.Instance.IsActivated(darktechByLevel.types[0]))
		{
			_originalWitchAnimator.Play(_originalIdleNoWitchHash);
			_witchByHardmodeLevel[clearedLevel].GetCutsceneWitch().SetActive(true);
			return;
		}
		if (MMMaths.Chance(1f / ((float)clearedLevel + 0.5f)))
		{
			_originalInteractive.SetActive(true);
			_originalWitchAnimator.Play(_originalIdleHash);
			return;
		}
		_originalWitchAnimator.Play(_originalIdleNoWitchHash);
		List<WitchsByLevel> list = new List<WitchsByLevel>();
		for (int j = 0; j < clearedLevel; j++)
		{
			list.Add(_witchByHardmodeLevel[j]);
		}
		ExtensionMethods.Random<WitchsByLevel>((IEnumerable<WitchsByLevel>)list).GetRandomWitch().SetActive(true);
	}
}
