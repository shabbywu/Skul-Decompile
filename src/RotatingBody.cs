using System;
using BT;
using BT.SharedValues;
using Characters;
using Level.Traps;
using UnityEngine;

public class RotatingBody : MonoBehaviour
{
	[SerializeField]
	private BehaviourTreeRunner _bTRunner;

	[SerializeField]
	private float _radius;

	[SerializeField]
	private float _speed;

	[SerializeField]
	private int _maxChildCount = 3;

	[SerializeField]
	private Orb[] _child;

	private Character _owner;

	private const string RADIUS_KEY = "radius";

	private const string SPEED_KEY = "speed";

	private const string CUREENTCHILDCOUNT_KEY = "currentChildCount";

	private void Awake()
	{
		float num = 0f;
		Orb[] child = _child;
		for (int i = 0; i < child.Length; i++)
		{
			child[i].Initialize(num);
			num += (float)Math.PI * 2f / (float)_child.Length;
		}
	}

	private void OnEnable()
	{
		Context context = Context.Create();
		_owner = ((Component)this).GetComponentInParent<Character>();
		if ((Object)(object)_owner == (Object)null)
		{
			Debug.LogError((object)(((object)this).ToString() + " must have an owner"));
		}
		context.Set(BT.Key.OwnerCharacter, new SharedValue<Character>(_owner));
		context.Set("radius", new SharedValue<float>(_radius));
		context.Set("speed", new SharedValue<float>(_speed));
		context.Set("currentChildCount", new SharedValue<float>(0f));
		_bTRunner.Run(context);
	}

	public void Rotate()
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_bTRunner.context.Get<Character>(BT.Key.OwnerCharacter) == (Object)null))
		{
			float radious = _bTRunner.context.Get<float>("radius");
			float amount = _bTRunner.context.Get<float>("speed") * _owner.chronometer.master.deltaTime;
			Orb[] child = _child;
			for (int i = 0; i < child.Length; i++)
			{
				child[i].MoveCenteredOn(((Component)this).transform.position, radious, amount);
			}
		}
	}
}
