using Characters.AI;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations;

public sealed class SpawnRandomCharacter : CharacterOperation
{
	[SerializeField]
	private Character[] _characterPrefabs;

	[SerializeField]
	private Transform _position;

	[SerializeField]
	private bool _setPlayerAsTarget;

	[SerializeField]
	private bool _containInWave;

	public override void Run(Character owner)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		if (_characterPrefabs.Length != 0)
		{
			Character character = _characterPrefabs.Random();
			Character character2;
			if (_containInWave)
			{
				character2 = ((!((Object)(object)_position != (Object)null)) ? Object.Instantiate<Character>(character) : Object.Instantiate<Character>(character, _position.position, Quaternion.identity));
				Map.Instance.waveContainer.Attach(character2);
			}
			else
			{
				character2 = Object.Instantiate<Character>(character, _position.position, Quaternion.identity);
				((Component)character2).transform.parent = ((Component)Map.Instance).transform;
			}
			if (_setPlayerAsTarget)
			{
				AIController componentInChildren = ((Component)character2).GetComponentInChildren<AIController>();
				componentInChildren.target = Singleton<Service>.Instance.levelManager.player;
				componentInChildren.character.ForceToLookAt(((Component)componentInChildren.target).transform.position.x);
			}
		}
	}
}
