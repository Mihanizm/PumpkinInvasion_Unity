using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerBehaviour : MonoBehaviour
{
    public float Speed;

    private Rigidbody2D _rb;
    private Vector2 _movementVector;
    private SkeletonAnimation _skAnim;
    private AudioSource _audioSource;
    private GameObject _partSys;
    private CanvasBehaviour _canvasBeh;

    private enum States { Start, Idle, Run, Shoot, Shoot_Fail, Loose };
    private States _curState;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _movementVector = new Vector2(1, 0);
        _skAnim = transform.Find("Skeleton").gameObject.GetComponent<SkeletonAnimation>();
        _partSys = transform.Find("Muzzle").gameObject;
        _curState = States.Start;
        _audioSource = transform.GetComponent<AudioSource>();
        _canvasBeh = GameObject.Find("Canvas").GetComponent<CanvasBehaviour>();
    }

    void Update()
    {
        if (_curState == States.Start && Input.GetKeyDown(KeyCode.Space))
        {
            //canvasB.HiddenText();
            RunState();
        }

        if (_curState == States.Idle)
        {
            CheckClick();
        }
    }

    private void FixedUpdate()
    {
        if (_curState == States.Run)
        {
            _rb.MovePosition(_rb.position + _movementVector * Speed * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("FireZone"))
        {
            IdleState();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            LooseState();
        }
    }

    private void IdleState()
    {
        _skAnim.state.SetAnimation(0, "idle", true);
        _curState = States.Idle;
    }

    public void RunState()
    {
        _skAnim.state.SetAnimation(0, "run", true);
        _curState = States.Run;
    }

    private IEnumerator ShootState()
    {
       var track = _skAnim.state.SetAnimation(0, "shoot", false);
        _curState = States.Shoot;

        yield return new WaitForSpineAnimationComplete(track);
        IdleState();
    }

    private IEnumerator ShootFailState()
    {
        var track = _skAnim.state.SetAnimation(0, "shoot_fail", false);
        _curState = States.Shoot_Fail;

        yield return new WaitForSpineAnimationComplete(track);
        IdleState();
    }

    private void LooseState()
    {
        _skAnim.state.SetAnimation(0, "loose", false);
        _curState = States.Loose;
        _canvasBeh.Lose();
    }

    private void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _audioSource.PlayOneShot(_audioSource.clip);
            _partSys.SetActive(true);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    hit.transform.gameObject.GetComponent<EnemyBehaviour>().ReactToShot();
                    StartCoroutine(ShootState());
                }
                else
                {
                    StartCoroutine(ShootFailState());
                }
            }
            else
            {
                StartCoroutine(ShootFailState());
            }
        } 
    }

    public void Win()
    {
        IdleState();
        _canvasBeh.Win();
    }
}
