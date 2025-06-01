using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public int attempts = 0;
    public Transform startPos;
    public GameObject obstacle;
    public GameObject border;
    public GameObject platform;
    public GameObject mainCamera;
    public AudioSource bgm;
    public float speed;
    public TMP_Text attemptsText;
    public GameObject[] coinImageList;
    public GameObject[] coinList;
    public Sprite goldCoinUI;
    public Sprite goldCoin;
    public Sprite emptyCoin;
    public GameObject player;
    public GameObject pausePanel;
    public GameObject pauseButton;
    public GameObject clearPanel;
    private Transformer _transformer;
    private int coinCount = 0;

    private bool _isPaused;
    private void Start()
    {
        _transformer = GetComponent<Transformer>();
    }

    private void FixedUpdate()
    {
        if (!_isPaused)
        {
            // 현재 위치에서 왼쪽으로 speed만큼 이동
            obstacle.transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }


    public void Restart()
    {
        attempts++;
        bgm.time = 0;
        bgm.Play();
        obstacle.transform.localPosition = Vector3.zero;
        UIupdate();
        coinCount = 0;
        foreach (var coin in coinList)
        {
            coin.GetComponent<CircleCollider2D>().enabled = true;
            coin.GetComponent<SpriteRenderer>().sprite = goldCoin;
        }
        _transformer.Transform();
        player.transform.localPosition = startPos.localPosition;
        mainCamera.GetComponent<CameraFollow>().enabled = true;
        clearPanel.SetActive(false);
    }

    private void UIupdate()
    {
        attemptsText.text = $"Attempts: {attempts.ToString()}";
        foreach (var item in coinImageList)
        {
            item.GetComponent<Image>().sprite = emptyCoin;
        }
    }

    public void GetCoin() // First, Second, Third
    {
        // Vector3(418.829956,2.4000001,0)
        // Vector3(685.07135,4.60999966,0)
        // Vector3(845.269958,2.14000034,0)
        coinList[coinCount].GetComponent<CircleCollider2D>().enabled = false;
        coinList[coinCount].GetComponent<SpriteRenderer>().sprite = null;
        coinImageList[coinCount].GetComponent<Image>().sprite = goldCoinUI;
        coinCount++;
    }

    public void Pause()
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            bgm.Pause();
        }
        else
        {
            bgm.UnPause();
        }
        pausePanel.SetActive(_isPaused);
        player.GetComponent<Rigidbody2D>().simulated = !_isPaused;
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void Resume()
    {
        Pause();
    }

    public void Platform(bool boolean)
    {
        var camPos = mainCamera.transform.position;
        var pos = new Vector3(camPos.x, camPos.y, 0.0f);
        border.transform.position = pos;
        border.SetActive(boolean);
        platform.SetActive(!boolean);
        mainCamera.GetComponent<CameraFollow>().enabled = !boolean;
    }

    public void GameClear()
    {
        _isPaused = true;
        player.GetComponent<Rigidbody2D>().simulated = false;
        clearPanel.SetActive(true);
        pauseButton.SetActive(false);
    }
}
