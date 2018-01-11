
using System.Collections;
using UnityEngine;

public class Grunt : EnemyController
{

    private GruntAnimator _animator;
    private float _meleeDamage;
    private float enemyHp;
    
    public override IEnumerator Idle()
    {
        Velocity.x = 0;
        yield break; 
    }

    void Start()
    {
        _animator = GetComponent<GruntAnimator>();
        Player = GameObject.FindWithTag("Player").transform; 
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Controller2D = GetComponent<Controller2D>();
        _gravity =  -(2 * 5) / Mathf.Pow (0.5f, 2);
    }
    
    private void Update ()
    {
        if (enemyHp == 0)
            StartCoroutine("die");
        
        if (Controller2D.collisions.above || Controller2D.collisions.below)
            Velocity.y = 0;
            Velocity.y += _gravity * Time.deltaTime;  
            Controller2D.Move (Velocity * Time.deltaTime);
    }
    
    public override IEnumerator Engage(GameObject go)
    {
        Velocity.x = Player.transform.position.x - transform.position.x;
        yield return new WaitForSeconds(0.5f);
    }

    public override IEnumerator IsAttacked()
    {
        enemyHp = enemyHp - 1;
        return base.IsAttacked();
    }

    public virtual IEnumerator Melee(GameObject go)
    {
        if(_animator != null)
//        _animator.SetTrigger("melee");
        
        go.SendMessage("Damaged", _meleeDamage);
        yield return new WaitForSeconds(0.3f);
    } 
    
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            StartCoroutine(Engage(c.gameObject));
        }
    }
    void OnTriggerStay2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            _animator.SendMessage("Engage");
            StartCoroutine(Engage(c.gameObject));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Melee(other.gameObject));
        }
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            var bullet = other.gameObject.GetComponent<Bullet>();
            StartCoroutine("IsAttacked", other);
        }
    }
}
