using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CameraFollow : MonoBehaviour
{
    private Transform Target;
    [SerializeField] private float XSmootingTime = .125f;
    [SerializeField] private float YSmootingTime = .125f;
    [SerializeField] private float ZSmootingTime = .125f;
    private Vector3 Distance;
    // Start is called before the first frame update
    private void Start()
    {
        if(Target!=null)
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
            Distance = -Target.position + transform.position;
        }
       

       
           
    }
    public void UpdateTarget(Transform t)
    {
        Target = t;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Target!=null)
        {
            transform.SmoothAxisFollow(Target.position + Distance, XSmootingTime, YSmootingTime, ZSmootingTime);
        }
        //transform.Follow(Target.position + Distance, false, false, true, true, SmootingTime);
      
    }
}
