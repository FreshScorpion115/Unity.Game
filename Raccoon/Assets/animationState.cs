using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationState : MonoBehaviour
{
    Animator animator;
    public KeyCode sprintKey;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(sprintKey))
            {
                animator.SetBool("IsRunning", true);
            }
            if (Input.GetKeyUp(sprintKey))
            {
                animator.SetBool("IsRunning", false);
            }
        
    }
}
