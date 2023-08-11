using System.Collections;
using Platforms;
using UnityEngine;

namespace AchievementTrackers;

public class StayingTimeTracker : MonoBehaviour
{
	[SerializeField]
	private float _time;

	[SerializeField]
	private Type _achievement;

	private IEnumerator Start()
	{
		yield return Chronometer.global.WaitForSeconds(_time);
		ExtensionMethods.Set(_achievement);
	}
}
