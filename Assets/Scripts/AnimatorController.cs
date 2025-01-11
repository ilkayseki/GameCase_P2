using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public Animator animator;
 

    void Awake()
    {
        // animator = GetComponent<Animator>();
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