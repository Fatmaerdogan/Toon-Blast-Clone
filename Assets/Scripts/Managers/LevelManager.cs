using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Level[] allLevels;
    [SerializeField] private Level currentLevelData;
    private int currentLevelIndex;
    public bool isLevelActive = false;
    public static LevelManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public Level CurrentLevelData
    {
        get { return currentLevelData; }
        set { currentLevelData = value; }
    }
    private void Start()
    {
        LoadLevel();
    }
    private void LoadLevel()
    {
        currentLevelIndex = PlayerPrefs.GetInt("Level", 0) % allLevels.Length;
        currentLevelData = allLevels[currentLevelIndex];
        GridManager.Instance.dataLevel = currentLevelData;
        GridManager.Instance.LoadGridData();
        GridManager.Instance.SpawnStartBlocks();
        GridManager.Instance.SetGridCornerSize();
        isLevelActive = true;
        Events.levelLoadedEvent?.Invoke();
    }
    public void LevelFailed()
    {
        if (isLevelActive)
        {
            isLevelActive = false;
            Events.levelFailedEvent?.Invoke();
        }
        
    }
    public void LevelSuccesed()
    {
        if (isLevelActive)
        {
            isLevelActive = false;
            currentLevelIndex += 1;
            PlayerPrefs.SetInt("Level", currentLevelIndex);
            Events.levelSuccesedEvent?.Invoke();
        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(0);
    }
    private void OnEnable()
    {
        Events.allGoalsEndedEvent += LevelSuccesed;
        Events.goalsFailedEvent += LevelFailed;
    }
    private void OnDisable()
    {
        Events.allGoalsEndedEvent -= LevelSuccesed;
        Events.goalsFailedEvent -= LevelFailed;
    }
}
