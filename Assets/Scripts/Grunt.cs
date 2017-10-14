
using System.Collections;
using UnityEngine;

public class Grunt : EnemyController {
    public override IEnumerator Idle()
    {
        Velocity.x = 0;
        yield break; 
    }

    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform; 
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Controller2D = GetComponent<Controller2D>();
        _gravity =  -(2 * 5) / Mathf.Pow (0.5f, 2);
    }
    
    private void Update ()
    {
        if (Controller2D.collisions.above || Controller2D.collisions.below)
            Velocity.y = 0;
           // Debug.LogError("air");
            Velocity.y += _gravity * Time.deltaTime;  
            Controller2D.Move (Velocity * Time.deltaTime);
        Debug.Log("cucklife::"+Velocity);

    }
    
    public override IEnumerator Engage(GameObject go)
    {
        Velocity.x = Player.transform.position.x - transform.position.x;
        //Debug.LogError("xxxxxx"+Velocity.x);
        //Debug.LogError("player found");
        yield return new WaitForSeconds(0.5f);
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
            StartCoroutine(Engage(c.gameObject));
        }
    } 
      
}
