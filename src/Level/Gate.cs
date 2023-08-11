using System;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class Gate : InteractiveObject
{
	[Serializable]
	public class GateGraphicSetting : ReorderableArray<GateGraphicSetting.GateProperty>
	{
		[Serializable]
		public class GateProperty
		{
			[SerializeField]
			private Type _type;

			[SerializeField]
			private RuntimeAnimatorController _animator;

			[SerializeField]
			private GameObject _gameObject;

			public Type type => _type;

			public RuntimeAnimatorController animator => _animator;

			public GameObject gameObject => _gameObject;

			public GateProperty(Type type, RuntimeAnimatorController animator, GameObject gameObject)
			{
				_type = type;
				_animator = animator;
				_gameObject = gameObject;
			}

			public void ActivateGameObject()
			{
				if ((Object)(object)_gameObject != (Object)null)
				{
					_gameObject.SetActive(true);
				}
			}

			public void DeactivateGameObject()
			{
				if ((Object)(object)_gameObject != (Object)null)
				{
					_gameObject.SetActive(false);
				}
			}

			public void Dispose()
			{
				_animator = null;
				_gameObject = null;
			}
		}

		public GateGraphicSetting(params GateProperty[] gateProperties)
		{
			base.values = gateProperties;
		}

		public GateProperty GetPropertyOf(Type type)
		{
			GateProperty[] values = base.values;
			foreach (GateProperty gateProperty in values)
			{
				if (gateProperty.type.Equals(type))
				{
					return gateProperty;
				}
			}
			return null;
		}

		public void Dispose()
		{
			for (int i = 0; i < base.values.Length; i++)
			{
				base.values[i].Dispose();
				base.values[i] = null;
			}
			base.values = null;
		}
	}

	public enum Type
	{
		None,
		Normal,
		Grave,
		Chest,
		Npc,
		Terminal,
		Adventurer,
		Boss
	}

	[GetComponent]
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	[GetComponent]
	private Collider2D _collider;

	[SerializeField]
	private RuntimeAnimatorController _destoryed;

	[SerializeField]
	private RuntimeAnimatorController _destoryedForTerminal;

	[SerializeField]
	private GateGraphicSetting _gateGraphicSetting;

	private GateGraphicSetting.GateProperty _gateProperty;

	private Type _type;

	private NodeIndex _nodeIndex;

	private bool _used;

	private void OnDestroy()
	{
		_animator = null;
		_gateGraphicSetting.Dispose();
		_gateProperty = null;
	}

	public override void OnActivate()
	{
		base.OnActivate();
		if ((Object)(object)_animator != (Object)null)
		{
			_animator.Play(InteractiveObject._activateHash);
		}
		if (_type != 0)
		{
			_gateProperty.ActivateGameObject();
		}
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_animator.Play(InteractiveObject._deactivateHash);
		if (_type != 0)
		{
			_gateProperty.DeactivateGameObject();
		}
	}

	public override void InteractWith(Character character)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (!_used)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
			_used = true;
			Singleton<Service>.Instance.levelManager.LoadNextMap(_nodeIndex);
		}
	}

	public void ShowDestroyed(bool terminal)
	{
		((Behaviour)_collider).enabled = false;
		_animator.runtimeAnimatorController = (terminal ? _destoryedForTerminal : _destoryed);
		Deactivate();
	}

	public void Set(Type type, NodeIndex nodeIndex)
	{
		if (type == Type.None)
		{
			((Component)this).gameObject.SetActive(false);
			return;
		}
		_nodeIndex = nodeIndex;
		_gateProperty = _gateGraphicSetting.GetPropertyOf(type);
		_animator.runtimeAnimatorController = _gateProperty.animator;
		if (base.activated)
		{
			_animator.Play(InteractiveObject._activateHash);
			_gateProperty.ActivateGameObject();
		}
		else
		{
			_gateProperty.DeactivateGameObject();
		}
	}
}
