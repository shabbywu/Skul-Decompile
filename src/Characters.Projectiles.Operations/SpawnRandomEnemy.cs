using System.Collections.Generic;
using Characters.AI;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public class SpawnRandomEnemy : Operation
{
	[SerializeField]
	private Character[] _characters;

	[SerializeField]
	private bool _setPlayerAsTarget;

	[Range(0f, 10f)]
	[SerializeField]
	private float _distribution;

	[Range(1f, 10f)]
	[SerializeField]
	private int _repeatCount;

	[SerializeField]
	private bool _containInWave = true;

	public override void Run(IProjectile projectile)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		Vector3 position = projectile.transform.position;
		for (int i = 0; i < _repeatCount; i++)
		{
			float num = Random.Range(0f - _distribution, _distribution);
			Random.Range(0f - _distribution, _distribution);
			Vector3 val = position;
			val.x += Random.Range(0f - _distribution, _distribution);
			val.y += Random.Range(0f - _distribution, _distribution);
			Character character = Object.Instantiate<Character>(ExtensionMethods.Random<Character>((IEnumerable<Character>)_characters), val, Quaternion.identity);
			character.ForceToLookAt((num < 0f) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			if (_setPlayerAsTarget)
			{
				((Component)character).GetComponentInChildren<AIController>().target = player;
			}
			if (_containInWave)
			{
				Map.Instance.waveContainer.Attach(character);
			}
		}
	}
}
