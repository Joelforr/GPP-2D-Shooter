using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour {
    public Transform target;
    public float offset;
    public Vector3 velocity = Vector3.zero;
    public float smoothTime;
    // Use this for initialization
    void Start () {
		
	}

    private void Update()
    {
        //velocity = target.GetComponent<PlayerMovement>().vel;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (target != null)
        {

            if (target.position.x > transform.position.x + offset || target.position.x < transform.position.x - offset)
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), ref velocity, smoothTime);
            }

            if (target.position.y > transform.position.y + offset || target.position.y < transform.position.y - offset)
            {
                transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, target.position.y, transform.position.z), ref velocity, smoothTime);
            }
        }
    }

    void LateUpdate()
    {

    }
}
