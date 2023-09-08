using System;
using System.Collections;
using UnityEngine;

public class Chronometer : ChronometerBase
{
	public class Global : ChronometerBase
	{
		private const float _defaultFixedDeltaTime = 1f / 60f;

		public override ChronometerBase parent
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		public override float localTimeScale => _timeScales.total;

		public override float timeScale => _timeScales.total;

		public override float deltaTime => Time.unscaledDeltaTime * _timeScales.total;

		public override float smoothDeltaTime => Time.smoothDeltaTime;

		public override float fixedDeltaTime => Time.fixedDeltaTime;

		protected override void Update()
		{
			Time.timeScale = _timeScales.total;
			if (Time.timeScale > float.Epsilon)
			{
				Time.fixedDeltaTime = Time.timeScale * (1f / 60f);
			}
			else
			{
				Time.fixedDeltaTime = 10f;
			}
		}

		public IEnumerator WaitForSeconds(float seconds)
		{
			yield return (object)new WaitForSeconds(seconds);
		}
	}

	public static readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

	public static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();

	protected static ChronometerBase _global;

	public static readonly Global global = new Global();

	private ChronometerBase _parent;

	public override ChronometerBase parent
	{
		get
		{
			if (_parent == null)
			{
				return global;
			}
			return _parent;
		}
		set
		{
			_parent = value;
		}
	}

	public override float localTimeScale => _timeScales.total;

	public override float timeScale => parent.timeScale * _timeScales.total;

	public override float deltaTime => parent.deltaTime * _timeScales.total;

	public override float smoothDeltaTime => parent.smoothDeltaTime * _timeScales.total;

	public override float fixedDeltaTime => parent.fixedDeltaTime * _timeScales.total;

	public Chronometer()
	{
	}

	public Chronometer(ChronometerBase parent)
	{
		this.parent = parent;
	}

	protected override void Update()
	{
	}
}
