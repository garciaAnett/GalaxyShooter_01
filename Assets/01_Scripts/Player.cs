using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    public float damage = 1f;
    public float speed = 2f;
    public float timeBtwShoot = 1.5f;
    public int ammo = 10;
    int currentAmmo;
    float life = 60;
    public float maxLife = 60;
    float timer = 0;
    bool canShoot = true;
    public Rigidbody rb;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Bullet prefab;
    public float bulletSpeed = 5f;
    public ParticleSystem particle;
    private bool isInvulnerable = false;  
    private float invulnerabilityTimer = 0f;
    public Image lifebar;
  //  public Text lifeText;

    void Start()
    {
        Debug.Log("Inició el juego");
        currentAmmo = ammo;
        life=maxLife;
      //  lifeText.text = "LIFE: " + life;
        lifebar.fillAmount = life/maxLife; 
    }

    void Update()
    {
        if (isInvulnerable)
        {
            invulnerabilityTimer -= Time.deltaTime;

            if (invulnerabilityTimer <= 0)
            {
                isInvulnerable = false;
                Debug.Log("Invulnerabilidad desactivada.");
            }
        }

        Movement();
        Reload();
        CheckIfCanShoot();
        Shoot();
       // lifeText.text = "LIFE: " + life;
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(x, 0,z) * speed;
    }

    void Shoot()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canShoot && currentAmmo > 0)
        {
            Bullet b = Instantiate(prefab, firePoint.position, 
                            transform.rotation);
            b.damage= damage;
            b.speed = bulletSpeed;
         //   currentAmmo--;
            canShoot = false;
        }
    }

    void Reload()
    {
        if(currentAmmo == 0 && Input.GetKeyDown(KeyCode.R))
        {
            currentAmmo = ammo;
        }
    }

    void CheckIfCanShoot()
    {
        if (timer < timeBtwShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            canShoot = true;
        }
    }

    public void TakeDamage(float dmg)
    {
        if (!isInvulnerable)
        {

            life -= dmg;
         //   lifeText.text = "LIFE: " + life;
            Debug.Log("Se quito -5 de vida");
            lifebar.fillAmount = life / maxLife;
            if (life <= 0)
            {
                Destroy(gameObject);
                Instantiate(particle, transform.position, Quaternion.identity);
                SceneManager.LoadScene("Gaming");
            }
        }
    }
    public void AddBulletSpeed(int value)
    {
        bulletSpeed += value;
        Debug.Log("Se aumento la velocidad de la bala");
    }
    public void AddSpeed(int value)
    {
        speed += value;
        Debug.Log("Se aumento la velocidad del player");
    }
    public void SpeedPro(float value)
    {
        timeBtwShoot += value;
        Debug.Log("Se aumento la velocidad del proyectil");
    }
    public void ActivateInvulnerability(float duration)
    {
        isInvulnerable = true;
        invulnerabilityTimer = duration;  // Configura el temporizador con la duración de la invulnerabilidad
        Debug.Log("Invulnerabilidad activada por " + duration + " segundos.");
    }
    public void DamageBullet30(float value)
    {
        float randomValue = Random.Range(0f, 1f);
        Debug.Log(" VALOR RANDOM "+randomValue);
        if (randomValue <= 0.3)
        {
            prefab.damage = value;
            Debug.Log("Los daños de la bala aumentan +4");
        }else{
            Debug.Log("Te toco un Damage Bullet");
        }   
    }

}