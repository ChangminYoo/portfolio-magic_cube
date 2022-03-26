using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateBehaviour : StateMachineBehaviour
{
	[SerializeField]
	int key;

	public int Key { get { return key; } }

	public event Delegate<AnimatorStateInfo> EventStateEnter;
	//public event Delegate<AnimatorStateInfo> EventStateExit;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		EventStateEnter?.Invoke(stateInfo);
	}
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	//{
	//	EventStateExit?.Invoke(stateInfo);
	//}

}
