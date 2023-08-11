using System.Collections;
using System.Collections.Generic;
using Characters.Gear.Weapons;
using FX.Connections;
using UnityEngine;

namespace Characters.Operations.Customs;

public class ConnectToTarget : TargetedCharacterOperation
{
	private class ConnectionCoroutine
	{
		public Character target { get; private set; }

		public Connection connection { get; private set; }

		public Coroutine coroutine { get; private set; }

		public ConnectionCoroutine(MonoBehaviour coroutineReference, Character target, Connection connection, float duration)
		{
			this.target = target;
			this.connection = connection;
			coroutine = coroutineReference.StartCoroutine(CRun(target.chronometer.master, duration));
		}

		private IEnumerator CRun(Chronometer chronometer, float duration)
		{
			float elapsed = 0f;
			while (elapsed < duration && connection.connecting && target.liveAndActive)
			{
				elapsed += ((ChronometerBase)chronometer).deltaTime;
				yield return null;
			}
			if (connection.connecting)
			{
				connection.Disconnect();
			}
		}
	}

	[SerializeField]
	[GetComponentInParent(false)]
	private Weapon _weapon;

	[SerializeField]
	private ConnectionPool _connectionPool;

	[SerializeField]
	private float _duration;

	private List<ConnectionCoroutine> _connectings = new List<ConnectionCoroutine>();

	private MonoBehaviour coroutineReference => (MonoBehaviour)(object)_connectionPool;

	private void OnDisable()
	{
		_connectings.Clear();
	}

	public override void Run(Character owner, Character target)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)target == (Object)null) && target.liveAndActive)
		{
			DisconnectIfConnected(target);
			Connection connection = _connectionPool.GetConnection();
			Bounds bounds = ((Collider2D)owner.collider).bounds;
			Vector2 endOffset = default(Vector2);
			((Vector2)(ref endOffset))._002Ector(0f, ((Bounds)(ref bounds)).size.y * 0.5f);
			bounds = ((Collider2D)target.collider).bounds;
			Vector2 startOffset = default(Vector2);
			((Vector2)(ref startOffset))._002Ector(0f, ((Bounds)(ref bounds)).size.y * 0.5f);
			connection.Connect(((Component)target).transform, startOffset, ((Component)owner).transform, endOffset);
			ConnectionCoroutine connectionCoroutine = new ConnectionCoroutine(coroutineReference, target, connection, _duration);
			AddDisconnectAction(connectionCoroutine);
			_connectings.Add(connectionCoroutine);
		}
	}

	private void DisconnectIfConnected(Character target)
	{
		if (TryGetConnectionCoroutine(target, out var result))
		{
			result.connection.Disconnect();
		}
	}

	private void AddDisconnectAction(ConnectionCoroutine connectionCoroutine)
	{
		connectionCoroutine.connection.OnDisconnect += OnDisconnect;
		void OnDisconnect()
		{
			coroutineReference.StopCoroutine(connectionCoroutine.coroutine);
			_connectings.Remove(connectionCoroutine);
			connectionCoroutine.connection.OnDisconnect -= OnDisconnect;
		}
	}

	private bool TryGetConnectionCoroutine(Character target, out ConnectionCoroutine result)
	{
		foreach (ConnectionCoroutine connecting in _connectings)
		{
			if ((Object)(object)connecting.target == (Object)(object)target)
			{
				result = connecting;
				return true;
			}
		}
		result = null;
		return false;
	}
}
