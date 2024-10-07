using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{

    public float timeBtwSpawn = 1.5f;

    float timer = 0;
    public Transform leftPoint;
    public Transform rightPoint;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> objects;
    public int KilledEnemies = 0;
    public static Spawner instance;
    public Text KilledEnemiesText;
    public int score = 7;
    public Text scoreText;
    public Text gEnem;
    public GameObject enemyBossPrefab; 

    private bool bossSpawned = false; 


    private void Awake() // solo se ejecuta una vez
    {
        if(instance == null){
            instance = this;
        }
    }

    void Start()
    {
        gEnem.text = "G1";
        KilledEnemiesText.text = "KILLED ENEMIES: " + KilledEnemies++;
        scoreText.text = "SCORE: " + score;
    }
 

    void Update()
    { 
        SpawnEnemy();    
    }
    public void AddScore(int points){
        score+=points;
        scoreText.text = "SCORE: "+ score;
        KilledEnemiesText.text = "KILLED ENEMIES: " + KilledEnemies++;
        if(KilledEnemies > 5 && KilledEnemies < 11)
        { // G2
            timeBtwSpawn = 0.7f;
            gEnem.text = "G2";
        }
        else if(KilledEnemies > 10 ){ //G3
            gEnem.text = "G3 FINAL";
            SpawnBoss();
        }
    }
    void SpawnBoss()
    {
        if (enemyBossPrefab != null && !bossSpawned)
        {
            // Define una posición de spawn para el boss, por ejemplo, fuera de la pantalla en Y
            Vector3 spawnPosition = new Vector3(0, 3, 18); // Ajusta según tu escena
            Instantiate(enemyBossPrefab, spawnPosition, Quaternion.identity);
            bossSpawned = true;
            // 
        }
       
    }

    // cada enemy tiene su propio score que da al jugador puntos SCORE
    void SpawnEnemy()
    {
        if (bossSpawned == false) {

            if (timer < timeBtwSpawn)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                float x = Random.Range(leftPoint.position.x, rightPoint.position.x);
                int enemy = Random.Range(0, enemyPrefabs.Count);
                int obj = Random.Range(0, objects.Count);
                Vector3 newPos = new Vector3(x, transform.position.y, transform.position.z);  // modificacion del eje z
                Instantiate(enemyPrefabs[enemy], newPos, Quaternion.Euler(0, 180, 0));
                // Instantiate(objects[obj], newPos, Quaternion.Euler(0, 180, 0)); //modificacion para el eje y

            }
        }
    }
  
}
