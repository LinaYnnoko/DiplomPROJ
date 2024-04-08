using UnityEngine;

public class FallScript : StateMachineBehaviour
{
    GameObject pivot;
    Vector3 lastPivot;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        pivot = animator.gameObject.GetComponentInParent<PlayerAnimation>().pivot;
        animator.gameObject.GetComponentInParent<PlayerAnimation>().isMoveLocked = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lastPivot = pivot.transform.position;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponentInParent<PlayerAnimation>().isMoveLocked = false;
        animator.transform.position = lastPivot;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
