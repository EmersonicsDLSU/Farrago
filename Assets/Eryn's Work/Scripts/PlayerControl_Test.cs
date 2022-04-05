using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl_Test : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    bool isWalk = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
