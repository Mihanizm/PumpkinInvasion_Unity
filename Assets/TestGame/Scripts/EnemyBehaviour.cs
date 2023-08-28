using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class EnemyBehaviour : MonoBehaviour
{
    public float Speed;

    private enum States { Run, Win, Lose };
    private States _curState;

    private AudioSource _audioSource;
    private GameObject _explosionPS;
    private GameObject _skeleton;
    private Rigidbody2D _rb;
    private Vector2 _movementVector;
    private SkeletonAnimation _skAnim;

    void Start()
    {
        _audioSource = transform.GetComponent<AudioSource>();
        _explosionPS = transform.Find("Explosion").gameObject;
        _skeleton = transform.Find("Skeleton").gameObject;
        _curState = States.Run;
        _rb = this.gameObject.GetComponent<Rigidbody2D>();
        _movementVector = new Vector2(-1,0);
        _skAnim = transform.Find("Skeleton").gameObject.GetComponent<SkeletonAnimation>();
    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_curState == States.Run)
        {
            _rb.MovePosition(_rb.position + _movementVector * Speed * Time.deltaTime);
        }
    }

    public void ReactToShot()
    {

        StartCoroutine(Dead());
    }

    private IEnumerator Dead()
    {
        _curState = States.Lose;
        _audioSource.PlayOneShot(_audioSource.clip);
        _explosionPS.SetActive(true);
        _skeleton.SetActive(false);
        yield return new WaitForSeconds(_audioSource.clip.length);

        this.transform.parent.gameObject.GetComponent<TriggerZoneBehaviour>().EnemyDead();
        Destroy(this.gameObject);
    }

    public void Win()
    {
        _skAnim.state.SetAnimation(0, "win", true);
        _curState = States.Win;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            this.transform.parent.GetComponent<TriggerZoneBehaviour>().TouchPlayer();
        }
    }
}
