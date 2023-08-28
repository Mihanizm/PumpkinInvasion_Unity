using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishZoneBehaviour : MonoBehaviour
{

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.gameObject.GetComponent<PlayerBehaviour>().Win();
        }
    }
}
