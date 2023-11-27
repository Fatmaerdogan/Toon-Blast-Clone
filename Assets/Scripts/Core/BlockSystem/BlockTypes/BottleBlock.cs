using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BottleBlock : Block
{
    public override BlockTypes blockType => BlockTypes.Bottle;
    public override Vector3 spriteSize => new Vector3(0.4f, 0.4f, 0.4f);
    private SpriteRenderer balloonSR;

    public override void DoTappedActions()
    {

    }
    public void Explode()
    {
        Events.bottlePopAudioPlayEvent?.Invoke();
        EffectsController.Instance.SpawnBalloonCrackEffect(transform.position);
        target = null;
        DOTween.Kill(gameObject);
        transform.DOKill();
        GoalPanel.Instance.DecereaseGoal(blockType);
        Destroy(gameObject);
    }
    public override void SetupBlock()
    {
        balloonSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        balloonSR.transform.localScale = spriteSize;
        balloonSR.sortingOrder = -(int)gridIndex.y + 1;
        balloonSR.transform.localPosition = new Vector3(0, -0.04f, 0);
    }
    public override void MoveToTarget(float arriveTime)
    {
        DOTween.Kill(transform);
        transform.DOKill();
        transform.DOMove(target.position, arriveTime).SetEase(Ease.OutBounce);
    }
    public override void UpdateSortingOrder()
    {
        if (!balloonSR)
            balloonSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        balloonSR.sortingOrder = -(int)gridIndex.y + 1;
    }
    public override void SetSortingLayerName(string layerName)
    {
        if (!balloonSR)
            balloonSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        balloonSR.sortingLayerName = layerName;
    }
    public override void SetSortingOrder(int index)
    {
        if (!balloonSR)
            balloonSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        balloonSR.sortingOrder = index;
    }
}
