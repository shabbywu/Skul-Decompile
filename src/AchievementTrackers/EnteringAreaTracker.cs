using Characters;
using Platforms;
using UnityEngine;

namespace AchievementTrackers;

public class EnteringAreaTracker : MonoBehaviour
{
	[SerializeField]
	private Collider2D _area;

	[SerializeField]
	private Type _achievement;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)((Component)collision).GetComponent<Character>() == (Object)null))
		{
			ExtensionMethods.Set(_achievement);
		}
	}
}
