using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GC;

public class BossEnemy : BasicEnemy {

    [SerializeField]
    private TaskManager tm = new TaskManager();
    public float activeSubEnemies = 0;
    private float maxhealth = 100;
    private float health = 100;
    private bool active = false;

    private float min;
    private float max;
    // Use this for initialization
    void Start () {
        min = transform.position.x - 4;
        max = transform.position.x + 7;
        tm.Do(new Scale(this.gameObject, Vector3.one, new Vector3(7, 7, 1), 2f))
           .Then(new Rotate(this.gameObject, transform.eulerAngles, transform.eulerAngles + 180f * new Vector3(0, 0, 1), 1f))
           .Then(new ActionTask(manager.SubSpawnCall))
           .Then(new ActionTask(() => active = true));
    }

    // Update is called once per frame
    void Update () {
        if (health / maxhealth > .7 && active == true && manager.active_enemies.Count == 1)
        {
            manager.SubSpawnCall();
        }
        else if (health / maxhealth < .7 && active == true)
        {
            Move();
            tm.Do(new ActionTask(Shoot))
                .Then(new Wait(.5f));
        }

        tm.Update();
	}

    public override void Move()
    {
        transform.position = new Vector3(Mathf.PingPong(Time.time * 2, max - min) + min, transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.SendMessage("OnShot", null, SendMessageOptions.DontRequireReceiver);
        //manager.QueueDestruction(this);
    }

    public override void OnShot()
    {
        if (active == true)
        {
            health -= 10;
            Debug.Log(health);
            if (health <= 0)
            {
                manager.QueueDestruction(this);
            }
        }
    }

    public override void Shoot()
    {
        base.Shoot();
    }
}
