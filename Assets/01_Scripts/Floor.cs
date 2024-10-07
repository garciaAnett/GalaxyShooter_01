using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Floor : MonoBehaviour
{
    public ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            Destroy(collision.gameObject); //destroy bullet
        }

    }
}
