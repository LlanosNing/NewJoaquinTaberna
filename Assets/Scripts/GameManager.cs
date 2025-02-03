using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private FadeScreen _fd;
    //public float waitToRespawn;
    //public float enemyRespawn;
    //public float pickupRespawn;

    public string levelToLoad;


    //public int gemCollected;

    //private PlayerController _pCReference;
    //private CheckpointController _cReference;
    //private UIController _uIReference;
    //private PlayerHealthController _pHReference;
    //private SlimePlayer _sPReference;

    //public GameObject[] horizontalEnemies;
    //public GameObject[] verticalEnemies;
    //public GameObject[] chasingEnemies;
    //public GameObject[] pickups;

    private void Awake()
    {
        _fd = GameObject.Find("FSmanager").GetComponent<FadeScreen>();
    }
    void Start()
    {
        _fd.enabled = true;
        //_pCReference = GameObject.Find("Player").GetComponent<PlayerController>();
        //_cReference = GameObject.Find("CheckpointController").GetComponent<CheckpointController>();
        //_uIReference = GameObject.Find("Canvas").GetComponent<UIController>();
        //_pHReference = GameObject.Find("Player").GetComponent<PlayerHealthController>();
        //_sPReference = GameObject.Find("Player").GetComponent<SlimePlayer>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //public void RespawnPlayer()
    //{
    //    StartCoroutine(RespawnPlayerCo());
    //}

    //public void RespawnEnemy()
    //{
    //    StartCoroutine(RespawnHorizontalEnemyCo());
    //    StartCoroutine(RespawnVerticalEnemyCo());
    //    StartCoroutine(RespawnChasingEnemyCo());
    //}
    //public void RespawnPickups()
    //{
    //    StartCoroutine(RespawnPickupsCo());
    //}

    //private IEnumerator RespawnPlayerCo()
    //{
    //    _pCReference.gameObject.SetActive(false);
    //    _sPReference.NormalStats();
    //    yield return new WaitForSeconds(waitToRespawn);
    //    _pCReference.gameObject.SetActive(true);
    //    _pCReference.transform.position = _cReference.spawnPoint;
    //    _pHReference.currentHealth = _pHReference.maxHealth;
    //    _uIReference.UpdateHealthDisplay();
    //}

    //private IEnumerator RespawnHorizontalEnemyCo()
    //{
    //    for (int i = 0; i < horizontalEnemies.Length; i++) //poner el nombre del array.Length
    //    {
    //        horizontalEnemies[i].SetActive(false);//con la i se recorre todos los elementos del array
    //    }
    //    for (int i = 0; i < horizontalEnemies.Length; i++)
    //    {
    //        horizontalEnemies[i].SetActive(true);
    //    }

    //    yield return new WaitForSeconds(enemyRespawn);
    //}
    //private IEnumerator RespawnVerticalEnemyCo()
    //{
    //    for (int i = 0; i < verticalEnemies.Length; i++)
    //    {
    //        verticalEnemies[i].SetActive(false);
    //    }
    //    for (int i = 0; i < verticalEnemies.Length; i++)
    //    {
    //        verticalEnemies[i].SetActive(true);
    //    }

    //    yield return new WaitForSeconds(enemyRespawn);
    //}
    //private IEnumerator RespawnChasingEnemyCo()
    //{
    //    for (int i = 0; i < chasingEnemies.Length; i++)
    //    {
    //        chasingEnemies[i].SetActive(false);
    //    }
    //    for (int i = 0; i < verticalEnemies.Length; i++)
    //    {
    //        chasingEnemies[i].SetActive(true);
    //    }

    //    yield return new WaitForSeconds(enemyRespawn);
    //}
    //private IEnumerator RespawnPickupsCo()
    //{
    //    for (int i = 0; i < pickups.Length; i++)
    //    {
    //        pickups[i].SetActive(false);
    //    }
    //    for (int i = 0; i < pickups.Length; i++)
    //    {
    //        pickups[i].SetActive(true);
    //    }

    //    yield return new WaitForSeconds(pickupRespawn);
    //}

    public void ExitLevel()
    {
        StartCoroutine(ExitLevelCo());
    }

    public IEnumerator ExitLevelCo()
    {
        yield return new WaitForSeconds(0f);
        SceneManager.LoadScene(levelToLoad);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Saliendo de la aplicacion");
    }
}
