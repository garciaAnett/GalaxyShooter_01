using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.ParticleSystem;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class EnemyBoss : MonoBehaviour
{
    // Definición de estados de movimiento
    private enum MovementState
    {
        Moving1,
        Moving2,
        Returning
    }

    private MovementState currentState = MovementState.Moving1;

    public float speed = 3f;
    private float life;
    public float maxlife = 3f;

    public float timeBtwShoot = 1.5f;
    private float shootTimer = 0f;

    public float timeBtwMove = 10f; // Tiempo entre decisiones de movimiento
    private float moveTimer = 0f;

    public float range = 4f;
    private bool targetInRange = false;
    private Transform target;

    public Transform firePoint;
    public float damage = 2f;

    public List<GameObject> powerUpPrefabs;
    public Bullet prefab;
    public float bulletSpeed = 5f;

    public ParticleSystem particle;
    public float chance = 5f;

    public Image lifebar;
    public int scorePoints = 1;

    public float targetZ = 3f;

    private Vector3 initialPosition;

    void Start()
    {
        GameObject ship = GameObject.FindGameObjectWithTag("Player");
        if (ship != null)
        {
            target = ship.transform;
        }
      
        initialPosition = transform.position; // Almacenar posición inicial
        life = maxlife;
        UpdateLifeBar();
    }

    void Update()
    {
     
        HandleShooting(); // Manejar los disparos

        //HandleMovement(); // Manejar los movimientos basados en el estado
    }

   

    void HandleShooting()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= timeBtwShoot)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void HandleMovement()
    {
        moveTimer += Time.deltaTime;
        if (moveTimer >= timeBtwMove)
        {
            int moveChoice = Random.Range(0, 2);
            switch (moveChoice)
            {
                case 0:
                    currentState = MovementState.Moving1;
                    break;
                case 1:
                    currentState = MovementState.Moving2;
                    break;
            }
            moveTimer = 0f;
        }

        switch (currentState)
        {
            case MovementState.Moving1:
                Move1();
                break;
            case MovementState.Moving2:
                Move2();
                break;
            case MovementState.Returning:
                ReturnToPosition();
                break;
        }
    }

    void Move1()
    {
        if (targetInRange && target != null)
        {
            RotateToTarget();
            // Puedes agregar comportamientos adicionales para Move1 si lo deseas
        }
        else
        {
            MoveForward(speed);
            SearchTarget();
        }
    }

    void Move2()  // Función tipo kamikaze
    {
        if (target != null)
        {
            // Moverse rápidamente hacia el jugador
            Vector3 direction = (target.position - transform.position).normalized;
            transform.Translate(direction * speed * 2 * Time.deltaTime, Space.World);
        }
    }

    void ReturnToPosition()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, initialPosition, step);

        if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
        {
            currentState = MovementState.Moving1;
        }
    }

    void MoveForward(float m)
    {
        transform.Translate(Vector3.down * m * Time.deltaTime);
    }

    void RotateToTarget()
    {
        if (target != null)
        {
            Vector2 direction = target.position - transform.position;
            float angleZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0, 0, angleZ);
        }
    }

    void SearchTarget()
    {
        if (target != null)
        {
            float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), new Vector2(target.position.x, target.position.y));
            targetInRange = distance <= range;
        }
    }

    void Shoot()
    {
        if (prefab != null && firePoint != null)
        {
            Bullet b = Instantiate(prefab, firePoint.position, firePoint.rotation);
            b.damage = damage;
            b.speed = bulletSpeed;
        }
        else
        {
            Debug.LogError("Prefab de Bullet o FirePoint no está asignado.");
        }
    }

    public void TakeDamage(float dmg)
    {
        life -= dmg;
        UpdateLifeBar();
        if (life <= 0)
        {
            DropPowerUp();
            if (particle != null)
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }
            if (Spawner.instance != null)
            {
                Spawner.instance.AddScore(scorePoints);
            }
            Destroy(gameObject);
        }
    }

    void UpdateLifeBar()
    {
        if (lifebar != null)
        {
            lifebar.fillAmount = life / maxlife;
        }
    }

    void DropPowerUp()
    {
        if (powerUpPrefabs.Count > 0 && Random.Range(0f, 100f) <= chance)
        {
            int powerupIndex = Random.Range(0, powerUpPrefabs.Count);
            Instantiate(powerUpPrefabs[powerupIndex], transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player e = collision.gameObject.GetComponent<Player>();
            if (e != null)
            {
                e.TakeDamage(damage);
            }

            // Dropear power-up si corresponde
            if (Random.Range(0f, 100f) <= chance && powerUpPrefabs.Count > 0)
            {
                int powerup = Random.Range(0, powerUpPrefabs.Count);
                Instantiate(powerUpPrefabs[powerup], transform.position, Quaternion.identity);
            }

            // Instanciar partículas
            if (particle != null)
            {
                Instantiate(particle, transform.position, Quaternion.identity);
            }

            // Añadir puntos al Spawner
            if (Spawner.instance != null)
            {
                Spawner.instance.AddScore(scorePoints);
            }

            // Cambiar el estado a Returning si estaba en Move2
            if (currentState == MovementState.Moving2)
            {
                currentState = MovementState.Returning;
            }
        }
    }
}