using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Wave_Controller : MonoBehaviour
{
    public bool destroyWave = false;

    public float nextWaveTimer = 12f;

    public GameObject banditSpawn, dogSpawn, orcSpawn;

    public int wave = 0;

    int maxWave = 11;

    [SerializeField]
    private AudioSource NewWaveSound;

    [SerializeField]
    private AudioClip AAA;

    [SerializeField]
    private TextMeshProUGUI WaveNumber;

    private void Awake()
    {
        WaveNumber = GameObject.Find("WaveCounter").GetComponent<TMPro.TextMeshProUGUI>();
        WaveNumber.text = "1";
    }
    // Update is called once per frame
    void Update()
    {
        

        // Keeps checking for total enemies with "Enemy" tag
        
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log("Enemies detected!!!!!!!!!: "+ totalEnemies.Length);

        
        

        if (totalEnemies.Length == 0 && wave + 1 < maxWave)
        {
            
            
            Debug.Log("NEW WAVE STARTING!");
            // Wave counter goes up, 15 second time before new wave starts, 
            nextWaveTimer -= Time.deltaTime;
            if (nextWaveTimer < 12.0f && nextWaveTimer > 11.9f)
            {
                NewWaveSound.Play();
            }

            if (nextWaveTimer <= 0.0f)
            {
                wave++;
                WaveNumber.text = wave.ToString();
                // Different Wave Spawns

                
                switch(wave)
                {
                    // Wave 1
                    case 1:

                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();

                        break;

                    // Wave 2
                    case 2:

                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();

                        break;

                    // Wave 3
                    case 3:

                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnDog();
                        SpawnDog();

                        break;

                    // Wave 4
                    case 4:

                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnDog();
                        SpawnDog();
                        SpawnDog();

                        break;

                    case 5:

                        SpawnDog();
                        SpawnDog();
                        SpawnDog();
                        SpawnDog();
                        SpawnDog();
                        SpawnDog();

                        break;

                    case 6:


                        SpawnOrc();
                        SpawnBandit();
                        SpawnBandit();

                        break;

                    case 7:

                        SpawnOrc();
                        SpawnOrc();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnDog();

                        break;

                    case 8:

                        SpawnOrc();
                        SpawnOrc();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnDog();
                        SpawnDog();

                        break;

                    case 9:

                        SpawnOrc();
                        SpawnOrc();
                        SpawnOrc();
                        SpawnOrc();
                        SpawnDog();
                        SpawnDog();

                        break;

                    case 10:

                        SpawnOrc();
                        SpawnOrc();
                        SpawnOrc();
                        SpawnOrc();
                        SpawnOrc();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnBandit();
                        SpawnDog();
                        SpawnDog();
                        SpawnDog();
                        SpawnDog();

                        break;

                    default:

                        
                       
                        break;
                }

                
                nextWaveTimer = 12f;
            }
            
        }

        else if(totalEnemies.Length == 0 && wave + 1 == maxWave)
        {
            wave++;
            Victory();
        }

        


        // Secret Button to destroy all enemies in the wave
        if( destroyWave )
        {
            for(int i = 0; i < totalEnemies.Length; i++) 
            {
                Destroy(totalEnemies[i]);
            }
      
        }
    }

    void Victory()
    {
        SceneManager.LoadScene("Victory", LoadSceneMode.Additive);
    }

    void SpawnBandit()
    {
        // Spawn bandit in random location across 4 different map edges
        int spawnSquare;

        spawnSquare = Random.Range(1, 5);

        Vector2 banditRandomSpawn;

        switch (spawnSquare)
        {
            case 1:

                banditRandomSpawn = new Vector2(Random.Range(-200, -190), Random.Range(-140, 140));
                Instantiate(banditSpawn, banditRandomSpawn, Quaternion.identity);
                
                break;

            case 2:

                banditRandomSpawn = new Vector2(Random.Range(-200, 200), Random.Range(130, 140));
                Instantiate(banditSpawn, banditRandomSpawn, Quaternion.identity);
                
                break;

            case 3:

                banditRandomSpawn = new Vector2(Random.Range(190, 200), Random.Range(-140, 140));
                Instantiate(banditSpawn, banditRandomSpawn, Quaternion.identity);
                
                break;

            case 4:

                banditRandomSpawn = new Vector2(Random.Range(-200, 200), Random.Range(-140, -130));
                Instantiate(banditSpawn, banditRandomSpawn, Quaternion.identity);
                
                break;

            default:
                
                break;
        }
       
        

            return;
    }

    void SpawnDog()
    {
        // Spawn dog in random location across map edges

        int spawnSquare;

        spawnSquare = Random.Range(1, 5);

        Vector2 dogRandomSpawn;

        switch (spawnSquare)
        {
            case 1:

                dogRandomSpawn = new Vector2(Random.Range(-200, -190), Random.Range(-140, 140));
                Instantiate(dogSpawn, dogRandomSpawn, Quaternion.identity);

                break;

            case 2:

                dogRandomSpawn = new Vector2(Random.Range(-200, 200), Random.Range(130, 140));
                Instantiate(dogSpawn, dogRandomSpawn, Quaternion.identity);

                break;

            case 3:

                dogRandomSpawn = new Vector2(Random.Range(190, 200), Random.Range(-140, 140));
                Instantiate(dogSpawn, dogRandomSpawn, Quaternion.identity);

                break;

            case 4:

                dogRandomSpawn = new Vector2(Random.Range(-200, 200), Random.Range(-140, -130));
                Instantiate(dogSpawn, dogRandomSpawn, Quaternion.identity);

                break;

            default:

                break;
        }



        return;
        
    }

    void SpawnOrc()
    {
        // Spawn orc in random location across map edges

        int spawnSquare;

        spawnSquare = Random.Range(1, 5);

        Vector2 orcRandomSpawn;

        switch (spawnSquare)
        {
            case 1:

                orcRandomSpawn = new Vector2(Random.Range(-200, -190), Random.Range(-140, 140));
                Instantiate(orcSpawn, orcRandomSpawn, Quaternion.identity);

                break;

            case 2:

                orcRandomSpawn = new Vector2(Random.Range(-200, 200), Random.Range(130, 140));
                Instantiate(orcSpawn, orcRandomSpawn, Quaternion.identity);

                break;

            case 3:

                orcRandomSpawn = new Vector2(Random.Range(190, 200), Random.Range(-140, 140));
                Instantiate(orcSpawn, orcRandomSpawn, Quaternion.identity);

                break;

            case 4:

                orcRandomSpawn = new Vector2(Random.Range(-200, 200), Random.Range(-140, -130));
                Instantiate(orcSpawn, orcRandomSpawn, Quaternion.identity);

                break;

            default:

                break;
        }

        return;
    }
}
