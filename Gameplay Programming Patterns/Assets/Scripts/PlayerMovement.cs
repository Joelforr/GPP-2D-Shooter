using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public Vector2 vel;
    public float grav;
    public float thrust;
    public float rotSpd;
    public ParticleSystem pSys;
    Rigidbody2D rb;
    public GameObject deathPart;
    AudioSource aud;
    public GameObject bullet;
    public float dragConst;
    Vector2 quadDrag;
    public Transform cursor;


    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        aud = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            //Instantiate(bullet, transform.position + (transform.up * .2f), Quaternion.Euler(0,0, transform.eulerAngles.z + 90f));
            Instantiate(bullet, transform.position + ((cursor.position - transform.position).normalized * .5f), Quaternion.Euler(0, 0, Geo.ToAng(cursor.position - transform.position)));
        }


    }

    private void FixedUpdate() {
        //vel.y -= grav;

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
            vel += Geo.ToVect(transform.eulerAngles.z+90f) * thrust;
            pSys.Play();
            if (!aud.isPlaying) aud.Play();
        } else {
            pSys.Stop();
            aud.Stop();
        }

        transform.rotation = Quaternion.Euler(0, 0, Geo.ToAng(cursor.position - transform.position) - 90f);

        /*
        if (Input.GetKey(KeyCode.RightArrow)) {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - rotSpd);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + rotSpd);
        }
        */

        quadDrag = -vel.normalized * dragConst * vel.sqrMagnitude;
        rb.MovePosition((Vector2)transform.position + vel + quadDrag);

    }

}
