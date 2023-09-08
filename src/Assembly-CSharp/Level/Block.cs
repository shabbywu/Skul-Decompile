using UnityEngine;

namespace Level;

public class Block : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Collider2D _collider2D;

	public void Activate()
	{
		((Behaviour)_collider2D).enabled = true;
	}

	public void Deactivate()
	{
		((Behaviour)_collider2D).enabled = false;
	}
}
