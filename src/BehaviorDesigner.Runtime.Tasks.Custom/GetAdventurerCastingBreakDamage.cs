using Characters;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom;

[TaskDescription("스테이지별로 보정된 모험가 CastingBreakDamage를 반환합니다")]
public sealed class GetAdventurerCastingBreakDamage : Action
{
	[SerializeField]
	private SharedFloat _storeResult;

	private float _originValue;

	public override void OnAwake()
	{
		_originValue = ((SharedVariable<float>)_storeResult).Value;
	}

	public override TaskStatus OnUpdate()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		float num = _originValue * currentChapter.currentStage.adventurerCastingBreakDamageMultiplier;
		if (GameData.HardmodeProgress.hardmode)
		{
			float castingBreakDamageMultiplier = HardmodeLevelInfo.instance.GetEnemyStatInfoByType(Character.Type.Adventurer).castingBreakDamageMultiplier;
			num *= castingBreakDamageMultiplier;
		}
		((SharedVariable)_storeResult).SetValue((object)num);
		return (TaskStatus)2;
	}
}
