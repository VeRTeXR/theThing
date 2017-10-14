
using System.Collections;
using UnityEngine;

public class Grunt : EnemyController {
    public override IEnumerator Idle()
    {
        yield break; 
    }

    public override IEnumerator Engage(GameObject go)
    {
        Debug.LogError("player found");
        yield return new WaitForSeconds(0.5f);
    }


    void OnCollisionEnter2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            StartCoroutine(Engage(c.gameObject));
        }
    } 
}
