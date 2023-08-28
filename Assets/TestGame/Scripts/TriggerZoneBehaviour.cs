using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class TriggerZoneBehaviour : MonoBehaviour
{
    public int CountEnemy;
    public GameObject Enemy;

    private Transform _player;

    void Start()
    {
        
    }

    void Update()
    {
        if (CountEnemy == 0)
        {
            _player.GetComponent<PlayerBehaviour>().RunState();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _player = other.transform;
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < CountEnemy; i++)
        {
            Vector3 pos = _player.position;
            pos += new Vector3(Random.Range(20.0f,35.0f), 0, 0);
            GameObject gObj = Instantiate(Enemy, pos, new Quaternion(),this.transform);
            gObj.transform.Find("Skeleton").GetComponent<MeshRenderer>().sortingOrder += i;
        }
    }

    public void EnemyDead()
    {
        CountEnemy--;
    }

    public void TouchPlayer()
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag("Enemy"))
            {
                child.GetComponent<EnemyBehaviour>().Win();
            }
        }
    }
}
