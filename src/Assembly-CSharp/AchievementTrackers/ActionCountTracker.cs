using System.Collections;
using Platforms;
using UnityEngine;

namespace AchievementTrackers;

public class ActionCountTracker : MonoBehaviour
{
	[SerializeField]
	private Type _achievement;

	[SerializeField]
	private int _count;

	[SerializeField]
	private int _timeout;

	private int _currentCount;

	public void AddCount()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		_currentCount++;
		if (_timeout > 0)
		{
			((MonoBehaviour)this).StopAllCoroutines();
			((MonoBehaviour)this).StartCoroutine(CTimeout());
		}
		if (_currentCount >= _count)
		{
			ExtensionMethods.Set(_achievement);
		}
	}

	private IEnumerator CTimeout()
	{
		yield return Chronometer.global.WaitForSeconds(_timeout);
		_currentCount = 0;
	}
}
