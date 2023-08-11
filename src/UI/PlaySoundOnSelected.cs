using FX;
using Singletons;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI;

public class PlaySoundOnSelected : EventTrigger
{
	private static GameObject _lastSelected;

	public SoundInfo soundInfo;

	public override void OnSelect(BaseEventData eventData)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		((EventTrigger)this).OnSelect(eventData);
		if (!((Object)(object)_lastSelected == (Object)(object)eventData.selectedObject))
		{
			_lastSelected = eventData.selectedObject;
			PersistentSingleton<SoundManager>.Instance.PlaySound(soundInfo, Vector3.zero);
		}
	}
}
