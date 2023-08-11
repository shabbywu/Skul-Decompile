using Services;
using Singletons;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("현재 챕터의 Stage Index를 저장합니다.")]
public sealed class GetStageIndex : Action
{
	public SharedInt storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable)storeResult).SetValue((object)Singleton<Service>.Instance.levelManager.currentChapter.stageIndex);
		return (TaskStatus)2;
	}
}
