using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.SpecialMap;

public class FollowMovement : MonoBehaviour
{
	[Serializable]
	private class Target
	{
		public enum Type
		{
			Player,
			Character,
			Transform,
			ClosestTarget
		}

		[SerializeField]
		private Type _type;

		[SerializeField]
		private Character _character;

		[SerializeField]
		private Transform _transform;

		[SerializeField]
		private LayerMask _layerMask;

		[SerializeField]
		private Collider2D _range;

		[SerializeField]
		private Collider2D _ownerRange;

		[SerializeField]
		private bool _hasCharacter;

		private static NonAllocOverlapper _overlapper = new NonAllocOverlapper(15);

		internal Transform GetTransform()
		{
			return (Transform)(_type switch
			{
				Type.Transform => _transform, 
				Type.Player => ((Component)Singleton<Service>.Instance.levelManager.player).transform, 
				Type.Character => ((Component)_character).transform, 
				Type.ClosestTarget => GetClosestObject(), 
				_ => ((Component)Singleton<Service>.Instance.levelManager.player).transform, 
			});
		}

		private Transform GetClosestObject()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layerMask);
			List<Collider2D> list = _overlapper.OverlapCollider(_range).results.Where(delegate(Collider2D result)
			{
				if (!_hasCharacter)
				{
					return true;
				}
				Characters.Target component = ((Component)result).GetComponent<Characters.Target>();
				return !((Object)(object)component == (Object)null) && (Object)(object)component.character != (Object)null;
			}).ToList();
			if (list.Count == 0)
			{
				return null;
			}
			if (list.Count == 1)
			{
				return ((Component)list[0]).transform;
			}
			ColliderDistance2D val = Physics2D.Distance(list[0], _ownerRange);
			float num = ((ColliderDistance2D)(ref val)).distance;
			int index = 0;
			for (int i = 1; i < list.Count; i++)
			{
				val = Physics2D.Distance(list[i], _ownerRange);
				float distance = ((ColliderDistance2D)(ref val)).distance;
				if (num > distance)
				{
					index = i;
					num = distance;
				}
			}
			return ((Component)list[index]).transform;
		}
	}

	[SerializeField]
	private bool _floatingY;

	[SerializeField]
	private bool _floatingX;

	[SerializeField]
	private Transform _body;

	[SerializeField]
	private bool _startOnAwake;

	[SerializeField]
	private Target _target;

	[SerializeField]
	private CustomFloat _offsetX;

	[SerializeField]
	private CustomFloat _offsetY;

	[SerializeField]
	private float _trackSpeed;

	private void Awake()
	{
		if (_startOnAwake)
		{
			Run();
		}
	}

	public void Run()
	{
		((MonoBehaviour)this).StartCoroutine("CChase");
		if (_floatingY || _floatingX)
		{
			((MonoBehaviour)this).StartCoroutine("CFloat");
		}
	}

	public void Stop()
	{
		((MonoBehaviour)this).StopCoroutine("CChase");
		if (_floatingY || _floatingX)
		{
			((MonoBehaviour)this).StopCoroutine("CFloat");
		}
	}

	private IEnumerator CChase()
	{
		float elapsed = 0f;
		Transform targetTransform = _target.GetTransform();
		float offsetX = _offsetX.value;
		float offsetY = _offsetY.value;
		Vector2 val = default(Vector2);
		while (true)
		{
			if ((Object)(object)targetTransform == (Object)null || !((Component)targetTransform).gameObject.activeInHierarchy)
			{
				yield return null;
				targetTransform = _target.GetTransform();
				continue;
			}
			float deltaTime = Chronometer.global.deltaTime;
			elapsed += deltaTime;
			((Vector2)(ref val))._002Ector(targetTransform.position.x + offsetX, targetTransform.position.y + offsetY);
			((Component)this).transform.position = Vector3.Lerp(((Component)this).transform.position, Vector2.op_Implicit(val), deltaTime * _trackSpeed);
			yield return null;
		}
	}

	private IEnumerator CFloat()
	{
		float t = Random.Range(0f, (float)Math.PI);
		float floatAmplitude = 0.5f;
		float floatFrequency = 0.8f;
		while (true)
		{
			Vector3 zero = Vector3.zero;
			t += Chronometer.global.deltaTime;
			if (_floatingY)
			{
				zero.y = Mathf.Sin(t * (float)Math.PI * floatFrequency) * floatAmplitude;
			}
			if (_floatingX)
			{
				zero.x = Mathf.Sin(t * (float)Math.PI * floatFrequency / 2f) * floatAmplitude;
			}
			_body.localPosition = zero;
			yield return null;
		}
	}
}
