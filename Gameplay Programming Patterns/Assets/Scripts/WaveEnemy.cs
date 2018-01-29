using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEnemy : BasicEnemy {

    public float stationaryRotationSpd;

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
            Shoot();
            Move();
        }

        if (currentCooldownFrame > 0)
        {
            currentCooldownFrame--;
        }
    }

    public override void Move()
    {
        rb.MoveRotation(transform.eulerAngles.z + stationaryRotationSpd);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.SendMessage("OnShot", null, SendMessageOptions.DontRequireReceiver);
        Instantiate(deathPart, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
