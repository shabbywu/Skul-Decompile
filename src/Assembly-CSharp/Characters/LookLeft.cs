using UnityEngine;

namespace Characters;

[ExecuteAlways]
public class LookLeft : MonoBehaviour
{
	private Character _character;

	private void Awake()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (Application.isPlaying && ((Behaviour)this).isActiveAndEnabled)
		{
			((Component)this).transform.localScale = Vector3.one;
			_character = ((Component)this).GetComponent<Character>();
			if ((Object)(object)_character == (Object)null)
			{
				((Component)this).transform.localScale = new Vector3(-1f, 1f, 0f);
			}
			else
			{
				_character.lookingDirection = Character.LookingDirection.Left;
			}
			Object.Destroy((Object)(object)this);
		}
	}
}
