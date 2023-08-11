using Platforms;
using UnityEngine;

namespace AchievementTrackers;

public class ActiveTracker : MonoBehaviour
{
	[SerializeField]
	private Type _achievement;

	public void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		ExtensionMethods.Set(_achievement);
	}
}
