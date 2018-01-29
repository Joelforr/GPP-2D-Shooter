using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveEnemy : BasicEnemy {

    public float rotateSpeed;

	// Use this for initialization
	void Start () {
        target = GameObject.Find("Ship").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {

   

        if (targetVisible)
        {
            //FaceTarget();
            Move();

        }

        if (currentCooldownFrame > 0)
        {
            currentCooldownFrame--;
        }
    }

    public override void Move()
    {
        Vector2 dir = (target.position - transform.position).normalized;
        float crossZ = Vector3.Cross(dir, transform.up).z;

        vel = (Vector2)transform.up * thrust;

        rb.MoveRotation((transform.eulerAngles.z+90) + rotateSpeed * crossZ);
        rb.MovePosition((Vector2)transform.position + vel);
    
    }

    public override void Shoot()
    {
       
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.SendMessage("OnShot", null, SendMessageOptions.DontRequireReceiver);
        Instantiate(deathPart, transform.position, transform.rotation);
        Destroy(gameObject);
    }

}
