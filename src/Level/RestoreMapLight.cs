using UnityEngine;

namespace Level;

public class RestoreMapLight : MonoBehaviour
{
	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private float _changingTime = 1f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Map.Instance.RestoreLight(_changingTime);
	}
}
