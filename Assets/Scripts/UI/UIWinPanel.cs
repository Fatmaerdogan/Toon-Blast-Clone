using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UIWinPanel : MonoBehaviour
{
    public List<GameObject> Stars = new List<GameObject>();
    private void OnEnable()
    {
        Invoke("UnlockEarnedStar", 1);
    }

    public void UnlockEarnedStar()
    {
        int StarEarned = 1;
        if (MovesPanel.Instance.Moves > 10) StarEarned += 1;
        if (MovesPanel.Instance.Moves > 20) StarEarned += 1;

        for (int i = 0; i < StarEarned; i++)
        {
            Stars[i].transform.DOScale(Vector3.one, 1f);
        }
    }
}
