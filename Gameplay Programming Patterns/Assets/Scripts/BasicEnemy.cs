using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour {

    public Transform target;
    public GameObject bullet;
    public GameObject deathPart;

    public float minDist;
    protected bool targetVisible;

    public int cooldown;
    protected int currentCooldownFrame;

    protected Rigidbody2D rb;
    public Vector2 vel;
    public float thrust;

    // Use this for initialization
    void Start () {
        target = GameObject.Find("Ship").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
        isVisible();
    }

    private void FixedUpdate()
    {
        if (targetVisible)
        {
            FaceTarget();
            Move();
            Shoot();
        }

        if(currentCooldownFrame > 0)
        {
            currentCooldownFrame--;
        }
    }

    public virtual void Move()
    {
        vel = (Vector2)transform.up * thrust;
        rb.MovePosition((Vector2)transform.position + vel);
    }

    public virtual void isVisible()
    {
        if (target != null) { 
            if (Vector2.Distance(target.transform.position, transform.position) <= minDist)
            {
                targetVisible = true;
            }
            else
            {
                targetVisible = false;
            }
        }
    }

    public virtual void FaceTarget()
    {
        if (target != null)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            if ((Vector2)transform.up != dir)
            {
                float ang = Geo.ToAng(dir);
                transform.eulerAngles = new Vector3(0, 0, ang - 90);
            }
        }
    }

    public virtual void Shoot()
    {
        if (target != null)
        {
            if (currentCooldownFrame <= 0)
            {
                Instantiate(bullet, transform.position + (transform.up * .2f), Quaternion.Euler(0, 0, transform.eulerAngles.z + 90f));
                currentCooldownFrame = cooldown;
            }
        }
    }

    public virtual void OnShot()
    {
        Instantiate(deathPart, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.SendMessage("OnShot", null, SendMessageOptions.DontRequireReceiver);
        Instantiate(deathPart, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
