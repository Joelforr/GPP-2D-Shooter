using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using BehaviorTree;

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

    private Tree<BasicEnemy> _tree;
    protected EnemyManager manager;

    // Use this for initialization
    void Start () {
        target = GameObject.Find("Ship").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        _tree = new Tree<BasicEnemy>(new Selector<BasicEnemy>(

            //Attack
            new Sequence<BasicEnemy>(
                new IsPlayerInRange(),
                new IsOffCooldown(),
                new FaceTargetEnemy(),
                new Fire()
                ),

            new Sequence<BasicEnemy>(
                new IsPlayerInRange(),
                new FaceTargetEnemy(),
                new MoveTowardEnemy()
                )


            )
        );
    }
	
	// Update is called once per frame
	void Update () {
        _tree.Update(this);
    }

    private void FixedUpdate()
    {

        if(currentCooldownFrame > 0)
        {
            currentCooldownFrame--;
        }
    }

    public virtual void FaceTarget()
    {
        Vector2 dir = GetDirection();
        if ((Vector2)transform.up != dir)
        {
            float ang = Geo.ToAng(dir);
            transform.eulerAngles = new Vector3(0, 0, ang - 90);
        }
    }

    public virtual Vector2 GetDirection()
    {
        return (target != null) ? (Vector2)(target.position - transform.position).normalized : Vector2.zero;
    }

    public virtual bool isTargetVisible()
    {
        return (target != null) ? (Vector2.Distance(target.transform.position, transform.position) <= minDist) : false;
    }

    public virtual void Move()
    {
        vel = (Vector2)transform.up * thrust;
        rb.MovePosition((Vector2)transform.position + vel);
    }

    public virtual void Shoot()
    {
            if (currentCooldownFrame <= 0)
            {
                Instantiate(bullet, transform.position + (transform.up * .2f), Quaternion.Euler(0, 0, transform.eulerAngles.z + 90f));
                currentCooldownFrame = cooldown;
            }
    }

    public virtual void OnShot()
    {
        manager.QueueDestruction(this);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.SendMessage("OnShot", null, SendMessageOptions.DontRequireReceiver);
        manager.QueueDestruction(this);
    }

    public void SetManager(EnemyManager manager)
    {
        this.manager = manager;
    }

    public void SpawnDeathPart()
    {
        Instantiate(deathPart, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // NODES
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ////////////////////
    // Conditions
    ////////////////////


    private class IsPlayerInRange : Node<BasicEnemy>
    {
        public override bool Update(BasicEnemy enemy)
        {
            var playerPos = enemy.target.position;
            var enemyPos = enemy.transform.position;
            return Vector3.Distance(playerPos, enemyPos) < enemy.minDist;
        }
    }

    private class IsOffCooldown : Node<BasicEnemy>
    {
        public override bool Update(BasicEnemy enemy)
        {
            return enemy.currentCooldownFrame <= 0;
        }
    }

    ///////////////////
    /// Actions
    ///////////////////
    private class FaceTargetEnemy : Node<BasicEnemy>
    {
        public override bool Update(BasicEnemy enemy)
        {

            Vector2 dir = enemy.GetDirection();

                float ang = Geo.ToAng(dir);
                enemy.transform.eulerAngles = new Vector3(0, 0, ang - 90);
                return (Vector2)enemy.transform.up == dir;
        }
    }

    private class Fire : Node<BasicEnemy>
    {
        public override bool Update(BasicEnemy enemy)
        {

            Instantiate(enemy.bullet, enemy.transform.position + (enemy.transform.up * .2f), Quaternion.Euler(0, 0, enemy.transform.eulerAngles.z + 90f));
            enemy.currentCooldownFrame = enemy.cooldown;
            return true;
        }
    }


    private class MoveTowardEnemy : Node<BasicEnemy>
    {
        public override bool Update(BasicEnemy enemy)
        {

            enemy.vel = (Vector2)enemy.transform.up * enemy.thrust;
            enemy.rb.MovePosition((Vector2)enemy.transform.position + enemy.vel);
            return true;
        }
    }
}
