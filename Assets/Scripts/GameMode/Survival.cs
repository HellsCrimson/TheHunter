using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Survival : MonoBehaviour
{
    public Survival Instance;
    
    [SerializeField] public int curWave = 0; // vague actuelle
    [SerializeField] public int zombiesWave;
    [SerializeField] public List<GameObject> zombieSpawed;
    
    [SerializeField] private int maxSimultZombie = 10;
    [SerializeField] private uint tempsEntreVague;

    [SerializeField] public int zombieKilled;
    
    GameObject audioManager;

    [SerializeField] private GameObject zombie;
    
    [SerializeField] SpawnManager spawnManager;

    [SerializeField] private Coroutine addZombie;

    public float currCountdownValue = 0;
    public bool waiting = false;

    void Awake()
    {
        Instance = this;
    }
    
    void Start()
    {
        if (Instance != this)
            Destroy(Instance);
        else
        {
            if (tempsEntreVague == 0)
                tempsEntreVague = 5;
            zombieSpawed = new List<GameObject>();
            StartCoroutine(StartCountdown(tempsEntreVague, NextWave));
            //audioManager.GetComponent<AudioManager>().PlaySound("StartGame");
        }
    }

    public void DeleteZombie()
    {
        zombieSpawed.RemoveAll(zombies => zombies == null);
        if (zombieSpawed.Count == 0)
            StartCoroutine(AddZombies());
    }

    void Update()
    {
        DeleteZombie();

        if (!waiting)
        {
            if (zombieSpawed.Count == 0 && zombiesWave == 0)
            {
                StartCoroutine(StartCountdown(tempsEntreVague, NextWave));
            }
        }
    }

    IEnumerator AddZombies() // spawn autant de zombie que possible (maxSimultZombie)
    {
        for (int i = 0; i < maxSimultZombie; i++)
        {
            if (zombiesWave > 0 && zombieSpawed.Count <= maxSimultZombie)
            {
                Transform spawn = spawnManager.GetSpawnPointOrdered();
                var newZombie = Instantiate(zombie, spawn.position, spawn.rotation);
                zombieSpawed.Add(newZombie);
                zombiesWave--;
                yield return new WaitForSeconds(1f);
            }
        }
    }

    void NextWave()
    {
        curWave++;
        zombiesWave = 5 + curWave;
        zombie.GetComponent<ZombieManager>().NextGen();
        StartCoroutine(AddZombies());
    }

    public IEnumerator StartCountdown(float countdownValue, System.Action onFinish)
    {
        waiting = true;
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        waiting = false;
        onFinish();
    }

    public void Killed()
    {
        zombieKilled++;
    }
}
