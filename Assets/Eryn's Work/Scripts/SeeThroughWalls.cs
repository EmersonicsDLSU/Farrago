using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SeeThroughWalls : MonoBehaviour
{
    public GameObject cam;
    public GameObject target;
    public LayerMask mylayermask;
    private CinemachineVirtualCamera CVCam;
    private  CinemachineBrain cinemachineBrain;

    //Emerson's change
    private void Start()
    {
        cinemachineBrain = this.GetComponent<CinemachineBrain>();

        if(target == null || target.tag == "PlayerModel")
        {
            target = GameObject.FindGameObjectWithTag("PlayerModel");
        }
        if(CVCam.Follow == null)
        {
            CVCam.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, (target.transform.position - cam.transform.position).normalized, out hit,
            Mathf.Infinity))
        {
            Debug.DrawRay(cam.transform.position, (target.transform.position - cam.transform.position).normalized, Color.blue);
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("spheremask"))
            {
                //Debug.Log("HIT");
                target.transform.localScale = new Vector3(0, 0, 0);
            }
            else
            {
                //Debug.Log("NOT HIT");
                //target.transform.localScale = new Vector3(3, 3, 3);
            }
        }
    }
}
