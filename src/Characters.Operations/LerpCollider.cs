using System.Collections;
using UnityEngine;

namespace Characters.Operations;

public class LerpCollider : CharacterOperation
{
	[SerializeField]
	private BoxCollider2D _source;

	[SerializeField]
	private BoxCollider2D _dest;

	[SerializeField]
	private Curve _sourceToDestCurve;

	[FrameTime]
	[SerializeField]
	private float _term;

	[SerializeField]
	private Curve _destToSourceCurve;

	[SerializeField]
	private bool _bounce;

	private CoroutineReference _coroutineReference;

	private Vector2 _originSize;

	private Vector2 _originOffset;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		_originSize = _source.size;
		_originOffset = ((Collider2D)_source).offset;
	}

	public override void Run(Character owner)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		_coroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CRun(owner));
	}

	private IEnumerator CRun(Character owner)
	{
		Vector2 source = _source.size;
		Vector2 sourceOffset = ((Collider2D)_source).offset;
		Vector2 dest = _dest.size;
		Vector2 destOffset = ((Collider2D)_dest).offset;
		destOffset.x *= ((Component)this).transform.lossyScale.x;
		yield return CLerp(owner.chronometer.master, _sourceToDestCurve, source, sourceOffset, dest, destOffset);
		if (_bounce)
		{
			yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.master, _term);
			yield return CLerp(owner.chronometer.master, _destToSourceCurve, dest, destOffset, source, sourceOffset);
		}
	}

	private IEnumerator CLerp(Chronometer chronometer, Curve curve, Vector2 source, Vector2 sourceOffset, Vector2 dest, Vector2 destOffset)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		for (float elapsed = 0f; elapsed < curve.duration; elapsed += ((ChronometerBase)chronometer).deltaTime)
		{
			yield return null;
			_source.size = Vector2.Lerp(source, dest, curve.Evaluate(elapsed));
			((Collider2D)_source).offset = Vector2.Lerp(sourceOffset, destOffset, curve.Evaluate(elapsed));
		}
		_source.size = dest;
		((Collider2D)_source).offset = destOffset;
	}

	public override void Stop()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		_source.size = _originSize;
		((Collider2D)_source).offset = _originOffset;
		((CoroutineReference)(ref _coroutineReference)).Stop();
	}
}
