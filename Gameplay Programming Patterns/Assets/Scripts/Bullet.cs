using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    Rigidbody2D rb;
    public float spd;
    public GameObject deathPart;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rb.MovePosition((Vector2)transform.position + Geo.ToVect(transform.eulerAngles.z) * spd);
    }

    private void OnCollisionEnter2D(Collision2D coll) {
        coll.gameObject.SendMessage("OnShot", null, SendMessageOptions.DontRequireReceiver);
        Instantiate(deathPart, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
