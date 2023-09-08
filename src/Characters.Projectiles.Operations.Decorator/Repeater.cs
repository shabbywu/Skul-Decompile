using System.Collections;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator;

public class Repeater : Operation
{
	[SerializeField]
	private int _times;

	[SerializeField]
	private float _interval;

	[SerializeField]
	[Subcomponent]
	private Operation _toRepeat;

	private CoroutineReference _repeatCoroutineReference;

	private void Awake()
	{
		if (_times == 0)
		{
			_times = int.MaxValue;
		}
	}

	public override void Run(IProjectile projectile)
	{
		float interval = _interval;
		_repeatCoroutineReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CRepeat());
		IEnumerator CRepeat()
		{
			for (int i = 0; i < _times; i++)
			{
				_toRepeat.Run(projectile);
				yield return projectile.owner.chronometer.projectile.WaitForSeconds(interval);
			}
		}
	}
}
