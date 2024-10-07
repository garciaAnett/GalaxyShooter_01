using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public EnemyType type;
    public float speed = 2;
    float life=3;
    public float maxlife = 3;
    public float timeBtwShoot = 1.5f;
    float timer = 0;
    public float range = 1000;
    bool targetInRange = false;
    Transform target;
    public Transform firePoint;
    public float damage = 2f;
    public List<GameObject> powerUpPrefabs;
    public Bullet prefab;
    public float bulletSpeed = 5f;
    public ParticleSystem particle;
    public float chance = 5f;
    public Image lifebar;
    public int scorePoints=1;
    void Start()
    {
        GameObject ship = GameObject.FindGameObjectWithTag("Player");
       
        target = ship.transform;
        life = maxlife;
        lifebar.fillAmount = life / maxlife;
        transform.rotation = Quaternion.Euler(0, 180, 0); // Ajusta según el eje Y
    }

    void Update()
    {

        switch (type)
        {
            case EnemyType.Normal:
                MoveForward();
                break;
            case EnemyType.NormalShoot:
                MoveForward();
                Shoot();
                break;
            case EnemyType.Kamikase:
                if (targetInRange)
                {
                    RotateToTarget();
                    MoveForward(2);
                }
                else
                {
                    MoveForward();
                    SearchTarget();
                }
                break;
            case EnemyType.Sniper:
                if (targetInRange)
                {
                    RotateToTarget();
                    Shoot();
                }
                else
                {
                    MoveForward();
                    SearchTarget();
                }
                break;
        }
       
    }
    private void OnCollisionEnter(Collision obj)
    {
        if (obj.gameObject.CompareTag("Floor"))
        { Destroy(gameObject); }
    }
    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void MoveForward(float m)
    {
        transform.Translate(Vector3.forward * m * Time.deltaTime);
    }

    void RotateToTarget()
    {
        Vector3 dir = target.position - transform.position;
        float angleZ = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angleZ, 0);
     }

    void SearchTarget()
    {
        float distance = Vector3.Distance(transform.position, target.position); //c
        if(distance <= range)
        {
            targetInRange = true;
        }
        else
        {
            targetInRange = false;
        }
    }

    void Shoot()
    {
        if (timer < timeBtwShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            Bullet b = Instantiate(prefab, firePoint.position, transform.rotation);     
            b.damage = damage;
            b.speed = bulletSpeed;
        }
    }

    public void TakeDamage(float dmg)
    {
        life -= dmg;
        lifebar.fillAmount = life / maxlife;
        if (life<=0)
        {
            if (Random.Range(0,6f)<= chance) {
                int powerup = Random.Range(0, powerUpPrefabs.Count);
              //  Instantiate(powerUpPrefabs[powerup], transform.position, Quaternion.identity);
            }
            
            Instantiate(particle, transform.position, Quaternion.identity);
            Spawner.instance.AddScore(scorePoints); // la forma en la que se realizara el conteo de la muestre de los enemies
            
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") )
        {
            Player e = collision.gameObject.GetComponent<Player>(); // obtener ese enemigo su componente
            int powerup = Random.Range(0, powerUpPrefabs.Count);
          //  Instantiate(powerUpPrefabs[powerup], transform.position, Quaternion.identity);
          
            Instantiate(particle, transform.position, Quaternion.identity);

            Spawner.instance.AddScore(scorePoints); // la forma en la que se realizara el conteo de la muestre de los enemies

            e.TakeDamage(damage); // ejecutar el metodo danio al enemy
            Destroy(gameObject); //destroy bullet
        }
       
    }

}
public enum EnemyType
{
    Normal,
    NormalShoot,
    Kamikase,
    Sniper
   
}