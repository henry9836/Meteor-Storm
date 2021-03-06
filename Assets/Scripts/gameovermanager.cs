﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameovermanager : MonoBehaviour
{
    public bool gameover = false;
    public float deathdistance = 60.0f;
    public float screenshotThreshold = 0.01f;
    public GameObject player;
    public GameObject canvas;
    private float fixedDeltaTime;

    void Awake()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = (1.0f / 60.0f);

        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, new Vector3(3.0f, 2.2f, -2.76f)) >= deathdistance && gameover == false)
        {
            player.transform.GetChild(0).GetComponent<MouseLook>().enabled = false;
            gameover = true;
            GameObject.Find("Canvas").gameObject.SetActive(false);
            canvas.gameObject.SetActive(true);
            canvas.transform.GetChild(0).GetComponent<Text>().text = "Score: " + this.gameObject.GetComponent<score>().thescore.ToString("F0");
            StartCoroutine(GameObject.Find("GameManager").GetComponent<musicmanager>().BGout());
            StartCoroutine(slow());
        }
    }

    public IEnumerator slow()
    {
        for (float t = 0.0f; t < 1.0f; t += Time.unscaledDeltaTime * 0.3f)
        {
            Time.timeScale = Mathf.Lerp(1.0f, screenshotThreshold, t);
            Time.fixedDeltaTime = .02f * Time.timeScale;

            yield return null;
        }

        Time.timeScale = 0.0f;
        Time.fixedDeltaTime = 0.0f;

        //take screenshot
        GetComponent<GameOverEvent>().ScreenShot();

        yield return new WaitForSecondsRealtime(1.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
