using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TyController : MonoBehaviour
{
    private GameObject parent;
    private Animator animator;

    private Ray footStepRay;

    void Start()
    {
        parent = transform.parent.gameObject;
        animator = GetComponent<Animator>();
    }

    public void TyIsFalling()
    {
        parent.GetComponent<PlayerController>().isFalling = true;
    }

    public void TyGotUp()
    {
        parent.GetComponent<PlayerController>().isFalling = false;
    }

    public void TyStep()
    {
        // TODO: SFX de andar
    }
  
    void Update()
    {
        
        if (animator.GetBool("isGrounded"))
        {
            footStepRay.origin = transform.position;
            footStepRay.direction = Vector3.down; 
            
        }
    }
}
