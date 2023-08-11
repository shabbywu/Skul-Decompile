using BehaviorDesigner.Runtime;
using Characters.AI;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations;

public class SpawnCharacter : CharacterOperation
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private Transform _position;

	[SerializeField]
	private bool _masterSlaveLink;

	[SerializeField]
	private bool _setPlayerAsTarget;

	[SerializeField]
	private bool _containInWave;

	public override void Run(Character owner)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		Character character = ((!((Object)(object)_position != (Object)null)) ? Object.Instantiate<Character>(_character) : Object.Instantiate<Character>(_character, _position.position, Quaternion.identity, ((Component)Map.Instance).transform));
		if (_containInWave)
		{
			Map.Instance.waveContainer.Attach(character);
		}
		if (_setPlayerAsTarget)
		{
			AIController componentInChildren = ((Component)character).GetComponentInChildren<AIController>();
			if ((Object)(object)componentInChildren != (Object)null)
			{
				componentInChildren.target = Singleton<Service>.Instance.levelManager.player;
				componentInChildren.character.ForceToLookAt(((Component)componentInChildren.target).transform.position.x);
			}
			else
			{
				BehaviorDesignerCommunicator component = ((Component)character).GetComponent<BehaviorDesignerCommunicator>();
				if ((Object)(object)component != (Object)null)
				{
					((SharedVariable<Character>)component.GetVariable<SharedCharacter>("Target")).Value = Singleton<Service>.Instance.levelManager.player;
				}
			}
		}
		if (_masterSlaveLink)
		{
			Master componentInChildren2 = ((Component)owner).GetComponentInChildren<Master>();
			Slave componentInChildren3 = ((Component)character).GetComponentInChildren<Slave>();
			componentInChildren2.AddSlave(componentInChildren3);
			componentInChildren3.Initialize(componentInChildren2);
		}
	}
}
