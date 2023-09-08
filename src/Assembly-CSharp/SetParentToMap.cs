using Level;
using UnityEngine;

public class SetParentToMap : MonoBehaviour
{
	private void Start()
	{
		((Component)this).transform.SetParent(((Component)Map.Instance).transform);
	}
}
