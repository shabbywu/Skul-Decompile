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
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		float interval = _interval;
		_repeatCoroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CRepeat());
		IEnumerator CRepeat()
		{
			for (int i = 0; i < _times; i++)
			{
				_toRepeat.Run(projectile);
				yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)projectile.owner.chronometer.projectile, interval);
			}
		}
	}
}
