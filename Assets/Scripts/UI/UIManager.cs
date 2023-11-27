using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject failMenu;

    private void OpenWinMenu()
    {
        winMenu.SetActive(true);
    }
    private void OpenFailMenu()
    {
        failMenu.SetActive(true);
    }
    private void OnEnable()
    {
        Events.levelFailedEvent += OpenFailMenu;
        Events.levelSuccesedEvent += OpenWinMenu;
    }
    private void OnDisable()
    {
        Events.levelFailedEvent -= OpenFailMenu;
        Events.levelSuccesedEvent -= OpenWinMenu;
    }
}
