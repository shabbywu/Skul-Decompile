using UnityEngine;

namespace FX;

public class ClearTrailOnDisable : MonoBehaviour
{
	[SerializeField]
	private TrailRenderer _trailRenderer;

	private void OnDisable()
	{
		_trailRenderer.Clear();
	}
}
