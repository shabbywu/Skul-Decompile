using System.Collections;
using Characters;
using FX;
using UI;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public sealed class FieldDeathKnight : FieldNpc
{
	[SerializeField]
	private DeathKnightReward _reward;

	[SerializeField]
	private Transform _dropPosition;

	[SerializeField]
	private EffectInfo _dropEffect;

	[SerializeField]
	private SoundInfo _dropSound;

	protected override NpcType _type => NpcType.FieldDeathKnight;

	protected override void Interact(Character character)
	{
		base.Interact(character);
		switch (_phase)
		{
		case Phase.Initial:
		case Phase.Greeted:
			((MonoBehaviour)this).StartCoroutine(CGreetingAndConfirm(character));
			break;
		case Phase.Gave:
			((MonoBehaviour)this).StartCoroutine(CChat());
			break;
		}
	}

	private IEnumerator CGreetingAndConfirm(Character character, object confirmArg = null)
	{
		yield return LetterBox.instance.CAppear();
		_npcConversation.skippable = true;
		int lastIndex = 3;
		for (int j = 0; j < lastIndex; j++)
		{
			yield return _npcConversation.CConversation(base._greeting[j]);
		}
		Object.Instantiate<DeathKnightReward>(_reward, ((Component)_dropPosition).transform.position, Quaternion.identity, ((Component)Map.Instance).transform);
		_phase = Phase.Gave;
		for (int j = lastIndex; j < base._greeting.Length; j++)
		{
			yield return _npcConversation.CConversation(base._greeting[j]);
		}
		LetterBox.instance.Disappear();
	}
}
