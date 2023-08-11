using Level;
using Runnables;
using UnityEngine;

namespace CutScenes;

public sealed class OnPropDestroyedInvoker : MonoBehaviour
{
	[SerializeField]
	private Prop _prop;

	[SerializeField]
	private Runnable _runable;

	private void Awake()
	{
		_prop.onDestroy += _runable.Run;
	}
}
