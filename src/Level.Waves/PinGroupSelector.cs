using System.Collections.Generic;
using Characters;
using Hardmode;
using Singletons;
using UnityEngine;

namespace Level.Waves;

public sealed class PinGroupSelector : GroupSelector
{
	[SerializeField]
	private PinGroup _groupNormal;

	[SerializeField]
	[Header("하드모드에만 설정")]
	private PinGroup _groupA;

	[SerializeField]
	private PinGroup _groupB;

	[SerializeField]
	private PinGroup _groupC;

	private HardmodeManager.EnemyStep _currentStep;

	private PinGroup _selectedGroup;

	public override ICollection<Character> Load()
	{
		_currentStep = Singleton<HardmodeManager>.Instance.GetEnemyStep();
		switch (_currentStep)
		{
		case HardmodeManager.EnemyStep.Normal:
			_selectedGroup = _groupNormal;
			Object.Destroy((Object)(object)((Component)_groupA).gameObject);
			Object.Destroy((Object)(object)((Component)_groupB).gameObject);
			Object.Destroy((Object)(object)((Component)_groupC).gameObject);
			break;
		case HardmodeManager.EnemyStep.A:
			_selectedGroup = _groupA;
			Object.Destroy((Object)(object)((Component)_groupNormal).gameObject);
			Object.Destroy((Object)(object)((Component)_groupB).gameObject);
			Object.Destroy((Object)(object)((Component)_groupC).gameObject);
			break;
		case HardmodeManager.EnemyStep.B:
			_selectedGroup = _groupB;
			Object.Destroy((Object)(object)((Component)_groupNormal).gameObject);
			Object.Destroy((Object)(object)((Component)_groupA).gameObject);
			Object.Destroy((Object)(object)((Component)_groupC).gameObject);
			break;
		case HardmodeManager.EnemyStep.C:
			_selectedGroup = _groupC;
			Object.Destroy((Object)(object)((Component)_groupNormal).gameObject);
			Object.Destroy((Object)(object)((Component)_groupA).gameObject);
			Object.Destroy((Object)(object)((Component)_groupB).gameObject);
			break;
		}
		((Component)_selectedGroup).gameObject.SetActive(true);
		return _selectedGroup.Load();
	}
}
