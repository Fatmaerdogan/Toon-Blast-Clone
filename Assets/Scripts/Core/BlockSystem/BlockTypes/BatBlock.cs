using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class BatBlock : Block
{
    public override BlockTypes blockType => BlockTypes.Bat;
    public override Vector3 spriteSize => new Vector3(0.25f, 0.25f, 0.25f);
    private SpriteRenderer batSR;

    public override void DoTappedActions()
    {

    }
    public override void SetupBlock()
    {
        batSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        batSR.transform.localScale = spriteSize;
        batSR.sortingOrder = -(int)gridIndex.y + 1;
        batSR.transform.localPosition = new Vector3(0, -0.04f, 0);
    }
    public override void MoveToTarget(float arriveTime)
    {
        DOTween.Kill(transform);
        transform.DOKill();
        BatSquezeEffect();
        transform.DOMove(target.position, arriveTime).SetEase(Ease.OutBounce).OnComplete(() =>
        {
            //call anim
            if ((int)gridIndex.y == LevelManager.Instance.CurrentLevelData.GameGrid.GridSizeY - 1)
            {
                ExplodeBat();
            }
          
        });
    }
    private void BatSquezeEffect()
    {
        if (!batSR)
        {
            batSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        }
        batSR.transform.DOScaleY(0.2f, 0.2f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            batSR.transform.DOScaleY(0.25f, 0.2f).SetEase(Ease.InOutBack);
        });
        batSR.transform.DOScaleX(0.35f, 0.2f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            batSR.transform.DOScaleX(0.25f, 0.2f).SetEase(Ease.InOutBack);
        });
    }
    private void ExplodeBat()
    {
        Events.batExplodeAudioPlayEvent?.Invoke();
        NeighbourManager.Instance.DoSingleObjAction(gridIndex);
        MovesPanel.Instance.Moves = MovesPanel.Instance.Moves - 1;
        target = null;
        DOTween.Kill(gameObject);
        transform.DOKill();
        Events.batLeavedGridEvent?.Invoke((int)gridIndex.x, (int)gridIndex.y);
        if (GoalPanel.Instance.CheckIsInGoals(blockType))
        {
            SetSortingLayerName("UI");
            SetSortingOrder(10);
            
            float arriveTime = 0.9f;
            Vector3 targetPos = GoalPanel.Instance.GetGoalPos(blockType);
            transform.DOMove(targetPos, arriveTime).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                GoalPanel.Instance.DecereaseGoal(blockType);
                Destroy(gameObject);
            });
        }
        else
        {
            Destroy(gameObject);
        }
        FillManager.Instance.Fill();
    }
    public override void UpdateSortingOrder()
    {
        if (!batSR)
            batSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        batSR.sortingOrder = -(int)gridIndex.y + 1;
    }
    public override void SetSortingLayerName(string layerName)
    {
        if (!batSR)
            batSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        batSR.sortingLayerName = layerName;
    }
    public override void SetSortingOrder(int index)
    {
        if (!batSR)
            batSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        batSR.sortingOrder = index;
    }

}
