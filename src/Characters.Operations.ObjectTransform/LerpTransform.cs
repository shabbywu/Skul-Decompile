using System.Collections;
using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public sealed class LerpTransform : CharacterOperation
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Curve _curve;

	[SerializeField]
	private Transform _source;

	[SerializeField]
	private Transform _dest;

	private CoroutineReference _reference;

	public override void Run(Character owner)
	{
		_reference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CLerp(owner.chronometer.master));
	}

	private IEnumerator CLerp(Chronometer chronometer)
	{
		float elapsed = 0f;
		_target.position = _source.position;
		_target.rotation = _source.rotation;
		_target.localScale = _source.localScale;
		for (; elapsed < _curve.duration; elapsed += chronometer.deltaTime)
		{
			yield return null;
			float num = _curve.Evaluate(elapsed);
			_target.position = Vector3.Lerp(_source.position, _dest.position, num);
			_target.rotation = Quaternion.Lerp(_source.rotation, _dest.rotation, num);
			_target.localScale = Vector3.Lerp(_source.localScale, _dest.localScale, num);
		}
		_target.position = _dest.position;
		_target.rotation = _dest.rotation;
		_target.localScale = _dest.localScale;
	}

	public override void Stop()
	{
		_reference.Stop();
	}
}
