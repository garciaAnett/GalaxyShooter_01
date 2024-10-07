using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerType type = PowerType.Dmage;
    public float speed = 2f;
    public float timeToDestroy = 7f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,timeToDestroy); //como eliminar un power up despues de cierto tiempo
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.CompareTag("Player"))
        {
            Player p = obj.gameObject.GetComponent<Player>();

            switch (type)
            {
                case PowerType.Dmage:
                    p.TakeDamage(5);
                    break;
             
                case PowerType.ShootSpeed:
                    p.SpeedPro(0.1f);
                    break;
                case PowerType.BulletSpeed:
                    p.AddBulletSpeed(1);
                    break;
                case PowerType.SpeedMove:
                    p.AddSpeed(5);
                    break;
                case PowerType.Safe:
                    p.ActivateInvulnerability(30);
                    break;
                case PowerType.bulletDamage:
                    p.DamageBullet30(4);
                    break;
            }
            Destroy(gameObject);
        }
      
    }

    // Update is called once per frame
    public enum PowerType
    {
        Dmage,
        SpeedMove,
        BulletSpeed,
        ShootSpeed,
        Safe,
        bulletDamage
    }
}
