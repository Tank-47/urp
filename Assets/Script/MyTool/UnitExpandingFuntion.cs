using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitExpandingFuntion 
{
    /// <summary>
    /// check animation tag
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="tagName"></param>
    /// <param name="animationIndex"></param>
    /// <returns></returns>
    public static bool CheckAnimationTag(this Animator animator, string tagName, int animationIndex = 0)
    {
        return animator.GetCurrentAnimatorStateInfo(animationIndex).IsTag(tagName);
    }

    
    /// <summary>
    /// check animation name 
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="animationName"></param>
    /// <param name="animationIndex"></param>
    /// <returns></returns>
    public static bool CheckAnimationName(this Animator animator, string animationName, int animationIndex = 0)
    {
        return animator.GetCurrentAnimatorStateInfo(animationIndex).IsName(animationName);
    }

    
    /// <summary>
    /// look target direction
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="target"></param>
    /// <param name="self"></param>
    /// <param name="lerpTime"></param>
    /// <returns></returns>
    public static Quaternion LookOnTarget(this Transform transform, Transform target, Transform self, float lerpTime)
    {
        if (target == null) return self.rotation;
        
        //normalized vector
        Vector3 targetDirection = (target.position - self.position).normalized;
        Quaternion newQuaternion = Quaternion.LookRotation(targetDirection);

        return Quaternion.Lerp(self.rotation, newQuaternion, lerpTime * Time.deltaTime);
    }
}