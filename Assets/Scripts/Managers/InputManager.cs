using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    private bool canClick = false;
    void Update()
    {
        if (canClick)
        {

#if UNITY_EDITOR
        GetEditorInputs();
#else
		GetMobileTouches();
#endif

        }
    }
    private void GetEditorInputs()
    {
        if (Input.GetMouseButtonUp(0))
        {
            DetectHittedObject(Input.mousePosition);
        }
    }
    private void GetMobileTouches()
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase.Equals(TouchPhase.Ended))
        {
            DetectHittedObject(touch.position);
        }
    }
    private void DetectHittedObject(Vector3 touchedPos)
    {
        if (MovesPanel.Instance.Moves>0)
        {
            BoxCollider2D hittedCollider = Physics2D.OverlapPoint(mainCam.ScreenToWorldPoint(touchedPos)) as BoxCollider2D;
            if (hittedCollider)
            {
                GameObject hittedObj = hittedCollider.gameObject;
                if (hittedObj.CompareTag("Block"))
                {
                    hittedObj.gameObject.GetComponent<Block>().DoTappedActions();
                }
            }
        } 
    }
    private void EnableClicking()
    {
        if (LevelManager.Instance.isLevelActive)
        {
            canClick = true;
        }    
    }
    private void DisableClicking()
    {
        canClick = false;
    }
    private void OnEnable()
    {
        Events.levelLoadedEvent += EnableClicking;
        Events.levelSuccesedEvent += DisableClicking;
        Events.levelFailedEvent += DisableClicking;
        Events.bombStartedEvent += DisableClicking;
        Events.bombEndedEvent += EnableClicking;
        Events.bombSpawningEvent += DisableClicking;
        Events.bombSpawnDoneEvent += EnableClicking;
    }
    private void OnDisable()
    {
        Events.levelLoadedEvent -= EnableClicking;
        Events.bombStartedEvent -= DisableClicking;
        Events.bombEndedEvent -= EnableClicking;
        Events.levelSuccesedEvent -= DisableClicking;
        Events.levelFailedEvent -= DisableClicking;
        Events.bombSpawningEvent -= DisableClicking;
        Events.bombSpawnDoneEvent -= EnableClicking;
    }
}
