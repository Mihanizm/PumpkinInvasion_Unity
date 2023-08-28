using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasBehaviour : MonoBehaviour
{
    private string _startText = "WELCOME!\nPress SPACE to start";
    private string _loseText = "WASTED!\nPress SPACE to restart";
    private string _winText = "WIN!\nPress SPACE to restart";

    private GameObject _textObj;
    private TextMeshProUGUI _textMP;

    void Start()
    {
        _textObj = transform.Find("Text").gameObject;
        _textObj.SetActive(true);
        _textMP = _textObj.GetComponent<TextMeshProUGUI>();
        _textMP.text = _startText;
    }


    void Update()
    {
        if (_textObj.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (_textMP.text == _startText)
            {
                Hide();
            }
            else
            {
                Restart();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void Hide()
    {
        _textObj.SetActive(false);
    }

    public void Lose()
    {
        _textObj.SetActive(true);
        _textMP.text = _loseText;
    }

    public void Win()
    {
        _textObj.SetActive(true);
        _textMP.text = _winText;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
