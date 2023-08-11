using UnityEngine;
using UnityEngine.Events;

namespace Level;

[RequireComponent(typeof(Prop))]
public class EventOnDestroyedProp : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Prop _prop;

	[SerializeField]
	private UnityEvent _events;

	public void Start()
	{
		_prop.onDestroy += _events.Invoke;
	}
}
