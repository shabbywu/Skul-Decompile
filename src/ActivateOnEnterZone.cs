using UnityEngine;

public class ActivateOnEnterZone : MonoBehaviour
{
	[SerializeField]
	private GameObject _target;

	[GetComponent]
	[SerializeField]
	private Collider2D _collider;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_target.gameObject.SetActive(true);
		Object.Destroy((Object)(object)_collider);
		Object.Destroy((Object)(object)_target, 10f);
	}
}
