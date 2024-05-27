using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void StartNewGame()
    {
        animator.SetBool("isDancing", false);
    }
    
    public void GameFinished()
    {
        animator.SetBool("isDancing", true);
    }
    
}