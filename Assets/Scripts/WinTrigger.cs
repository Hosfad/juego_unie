using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinTrigger : MonoBehaviour
{
    public GameObject player;
    public Text timerText;
    public GameObject winCanvas;
    void Start()
    {
         winCanvas.SetActive(false);
         player = FindObjectOfType<PlayerController>().gameObject;
    }

    void OnTriggerEnter(Collider other)
    {
        Timer timer = player.GetComponent<Timer>();
        timer.enabled = false;
        winCanvas.SetActive(true);
        timer.Win();
        Time.timeScale = 0;
    }
}