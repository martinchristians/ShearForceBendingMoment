using System.Collections;
using UnityEngine;

public class Test_WaitUntilAnimIsDone : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animTrigger;

    [SerializeField] private bool checkIfAnimIsDone;

    private float _animTime;
    private bool _isAnimPlayed;
    private bool _isAnimationComplete;

    private void Awake()
    {
        checkIfAnimIsDone = false;
    }

    private void Update()
    {
        if (!animator || string.IsNullOrEmpty(animTrigger) || _isAnimationComplete) return;

        if (!_isAnimPlayed)
        {
            _isAnimPlayed = true;
            animator.SetTrigger(animTrigger);

            _animTime = animator.runtimeAnimatorController.animationClips[0].length;
            StartCoroutine(WaitUntilAnimDone());
        }

        StartCoroutine(WaitUntilAnimDone());
    }

    private IEnumerator WaitUntilAnimDone()
    {
        yield return new WaitForSeconds(_animTime);
        _isAnimationComplete = true;
    }
}