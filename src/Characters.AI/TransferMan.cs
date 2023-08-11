using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.Operations;
using Level;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class TransferMan : AIController
{
	[SerializeField]
	private Behaviour _teleportToEnemy;

	[SerializeField]
	private Behaviour _transferToPlayer;

	[SerializeField]
	private Behaviour _transferToPlayerReady;

	[SerializeField]
	private Transform _teleportDest;

	[SerializeField]
	private Collider2D _transferRange;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onTransferStart;

	[SerializeField]
	private Transform _transferDest;

	private static readonly NonAllocOverlapper _overlapper;

	static TransferMan()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(64);
	}

	private void Awake()
	{
		_onTransferStart.Initialize();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override IEnumerator CProcess()
	{
		yield return null;
		yield return null;
		while (!base.dead)
		{
			if (base.stuned)
			{
				yield return null;
				continue;
			}
			if (TryToSetTeleportDest())
			{
				yield return _teleportToEnemy.CRun(this);
				yield return Chronometer.global.WaitForSeconds(1f);
				yield return _transferToPlayerReady.CRun(this);
				Transfer();
				yield return _transferToPlayer.CRun(this);
			}
			yield return Chronometer.global.WaitForSeconds(2f);
		}
	}

	private bool TryToSetTeleportDest()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		List<Character> allEnemies = Map.Instance.waveContainer.GetAllEnemies();
		if (allEnemies.Count == 0)
		{
			return false;
		}
		Character player = Singleton<Service>.Instance.levelManager.player;
		Collider2D collider = player.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)collider == (Object)null)
		{
			player.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
		}
		ExtensionMethods.Shuffle<Character>((IList<Character>)allEnemies);
		foreach (Character item in allEnemies)
		{
			Collider2D lastStandingCollider = item.movement.controller.collisionState.lastStandingCollider;
			if (!((Object)(object)lastStandingCollider == (Object)null) && (Object)(object)lastStandingCollider != (Object)(object)collider)
			{
				_teleportDest.position = ((Component)item).transform.position;
				return true;
			}
		}
		return false;
	}

	private void Transfer()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		((Component)_onTransferStart).gameObject.SetActive(true);
		_onTransferStart.Run(character);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
		ReadonlyBoundedList<Collider2D> results = _overlapper.OverlapCollider(_transferRange).results;
		Debug.Log((object)results.Count);
		Bounds bounds = _transferRange.bounds;
		foreach (Collider2D item in results)
		{
			if (!((Object)(object)((Component)item).GetComponent<Character>() == (Object)null))
			{
				float num = ((Component)item).transform.position.x - ((Bounds)(ref bounds)).center.x;
				float num2 = ((Component)item).transform.position.y - ((Bounds)(ref bounds)).min.y;
				((Component)item).transform.position = _transferDest.position + new Vector3(num, num2);
			}
		}
	}
}
