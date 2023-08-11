using System;
using UnityEditor;
using UnityEngine;

namespace Runnables;

public abstract class Runnable : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<Runnable>
	{
		public void Run()
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Run();
			}
		}
	}

	public static readonly Type[] types = new Type[32]
	{
		typeof(Attacher),
		typeof(CharacterSetPositionTo),
		typeof(ChangeCameraZone),
		typeof(ChangeBackground),
		typeof(ClearStatus),
		typeof(ControlUI),
		typeof(ConsumeCurrency),
		typeof(Branch),
		typeof(DestroyObject),
		typeof(DropCurrency),
		typeof(DropCustomGear),
		typeof(DropGear),
		typeof(InvokeUnityEvent),
		typeof(LoadNextMap),
		typeof(LoadChapter),
		typeof(OpenUpgradePanel),
		typeof(PrintDebugLog),
		typeof(RunOperations),
		typeof(SetSoundEffectVolume),
		typeof(ShowStageInfo),
		typeof(ShowLineText),
		typeof(SpawnBuffFloatingText),
		typeof(TransitTo),
		typeof(TakeHealth),
		typeof(KillAllEnemy),
		typeof(Zoom),
		typeof(RunOperationInfos),
		typeof(RunAction),
		typeof(ControlHardmodeLevel),
		typeof(GameFadeInOut),
		typeof(SetAcheivement),
		typeof(StopPlayerStuckResolver)
	};

	public abstract void Run();
}
