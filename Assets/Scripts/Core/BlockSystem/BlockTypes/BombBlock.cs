using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class BombBlock : Block
{
    public override BlockTypes blockType => BlockTypes.Bomb;
    public override Vector3 spriteSize => new Vector3(0.25f, 0.25f, 0.25f);
    private SpriteRenderer rocketLeftSR;
    private SpriteRenderer rocketRightSR;
    private BombDirection myDirection;
    private GameObject leftEffectObj;
    private GameObject rightEffectObj;
    public bool canTapped = true;
    public override void DoTappedActions()
    {
        if (canTapped)
        {
            Events.bombStartedEvent?.Invoke();
            canTapped = false;
            List<GameObject> firstList;
            List<GameObject> secondList;
            Vector3 targetPosFirst;
            Vector3 targetPosSecond;
            float moveDistance = 5f;
            float arriveTime = 1f;
            if (myDirection == BombDirection.Horizontal)
            {
                firstList = NeighbourManager.Instance.FindLeftBlocks(gridIndex);
                secondList = NeighbourManager.Instance.FindRightBlocks(gridIndex);
                targetPosFirst = transform.position + new Vector3(-moveDistance, 0, 0);
                targetPosSecond = transform.position + new Vector3(moveDistance, 0, 0);

                Sprite tmpSprite = rocketRightSR.sprite;
                rocketRightSR.sprite = rocketLeftSR.sprite;
                rocketLeftSR.sprite = tmpSprite;
            }
            else
            {
                firstList = NeighbourManager.Instance.FindUpBlocks(gridIndex);
                secondList = NeighbourManager.Instance.FindDownBlocks(gridIndex);
                targetPosFirst = transform.position + new Vector3(0, moveDistance, 0);
                targetPosSecond = transform.position + new Vector3(0, -moveDistance, 0);
            }

            
            NeighbourManager.Instance.DoSingleObjAction(gridIndex);

            MovesPanel.Instance.Moves = MovesPanel.Instance.Moves - 1;

            rightEffectObj.SetActive(true);
            leftEffectObj.SetActive(true);

            rocketRightSR.transform.DOMove(targetPosFirst, arriveTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(gameObject);
                target = null;
                FillManager.Instance.Fill();
                Events.bombEndedEvent?.Invoke();

            });
            
            for (int i = firstList.Count - 1; i >= 0; i--)
            {
                ExplodeHittedBlock(firstList[i], ((firstList.Count - 1) - i) * 0.12f);
            }

            rocketLeftSR.transform.DOMove(targetPosSecond, arriveTime).SetEase(Ease.Linear);
            for (int i = 0; i < secondList.Count; i++)
            {
                ExplodeHittedBlock(secondList[i], i * 0.12f);
            }
        }
        
    }

    public override void SetupBlock()
    {

        rocketLeftSR = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        rocketLeftSR.transform.localScale = spriteSize;
        rocketLeftSR.sortingOrder = -(int)gridIndex.y + 1;
        rocketRightSR = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();

        myDirection = (BombDirection)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(BombDirection)).Length);


        if (myDirection==BombDirection.Vertical)
        {
            rocketRightSR.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
            rocketLeftSR.transform.localEulerAngles = new Vector3(0f, 0, 90f);

            rocketLeftSR.transform.localPosition = new Vector3(0f, -0.12f, 0f);
            rocketRightSR.transform.localPosition = new Vector3(0f, 0.02f, 0f);
        }
        else
        {
            rocketLeftSR.transform.localPosition = new Vector3(-0.07f, 0f, 0f);
            rocketRightSR.transform.localPosition = new Vector3(0.07f, 0f, 0f);
        }
       

        rocketLeftSR.gameObject.name = "LeftRocketSprite";
        rocketRightSR.gameObject.name = "RightRocketSprite";

        leftEffectObj = EffectsController.Instance.GetRocketEffect();
        leftEffectObj.transform.SetParent(rocketLeftSR.transform);
        leftEffectObj.transform.localPosition = Vector3.zero;
        leftEffectObj.SetActive(false);

        rightEffectObj = EffectsController.Instance.GetRocketEffect();
        rightEffectObj.transform.SetParent(rocketRightSR.transform);
        rightEffectObj.transform.localPosition = Vector3.zero;
        rightEffectObj.SetActive(false);
    }
    public override void MoveToTarget(float arriveTime)
    {
        DOTween.Kill(transform);
        transform.DOKill();
        transform.DOMove(target.position, arriveTime).SetEase(Ease.OutBounce).OnComplete(() =>
        {

        });
    }
    public override void UpdateSortingOrder()
    {
        if (!rocketLeftSR)
            rocketLeftSR = gameObject.GetComponentInChildren<SpriteRenderer>();
        rocketLeftSR.sortingOrder = -(int)gridIndex.y + 1;

        if (!rocketRightSR)
            rocketRightSR = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();
        rocketRightSR.sortingOrder = -(int)gridIndex.y + 1;
    }
    public override void SetSortingLayerName(string layerName)
    {
        if (!rocketLeftSR)
            rocketLeftSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        rocketLeftSR.sortingLayerName = layerName;

        if (!rocketRightSR)
            rocketRightSR = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();

        rocketRightSR.sortingLayerName = layerName;
    }
    public override void SetSortingOrder(int index)
    {
        if (!rocketLeftSR)
            rocketLeftSR = gameObject.GetComponentInChildren<SpriteRenderer>();

        rocketLeftSR.sortingOrder = index;

        if (!rocketRightSR)
            rocketRightSR = gameObject.transform.GetChild(1).GetComponent<SpriteRenderer>();

        rocketRightSR.sortingOrder = index;
    }
    public void PlayRocketAnim(BombDirection direction)
    {
        Vector3 targetPosFirst;
        Vector3 targetPosSecond;
        float moveDistance = 5f;
        float arriveTime = 0.5f;
        if (direction == BombDirection.Horizontal)
        {
            targetPosFirst = transform.position + new Vector3(-moveDistance, 0, 0);
            targetPosSecond = transform.position + new Vector3(moveDistance, 0, 0);

            Sprite tmpSprite = rocketRightSR.sprite;
            rocketRightSR.sprite = rocketLeftSR.sprite;
            rocketLeftSR.sprite = tmpSprite;
        }
        else
        {
            targetPosFirst = transform.position + new Vector3(0, moveDistance, 0);
            targetPosSecond = transform.position + new Vector3(0, -moveDistance, 0);
        }

        rightEffectObj.SetActive(true);
        leftEffectObj.SetActive(true);

        rocketRightSR.transform.DOMove(targetPosFirst, arriveTime).SetEase(Ease.Linear).OnComplete(()=>
        {
            Destroy(gameObject);
        });
        rocketLeftSR.transform.DOMove(targetPosSecond, arriveTime).SetEase(Ease.Linear);
        
    }
    private void ExplodeHittedBlock(GameObject blockObj,float destroyTime)
    {
        Block curBlock = blockObj.gameObject.GetComponent<Block>();
        if (curBlock is CubeBlock)
        {
            curBlock.gameObject.GetComponent<CubeBlock>().canTapped = false;
            CubeTypes cubeType = blockObj.GetComponent<CubeBlock>().cubeType;
            Events.cubeExplosionAudioPlayEvent?.Invoke();
            EffectsController.Instance.SpawnCubeCrackEffect(blockObj.transform.position, cubeType);
            if (GoalPanel.Instance.CheckIsInGoals(cubeType))
            {
                GoalPanel.Instance.DecereaseGoal(cubeType);
            }
        }
        else if (curBlock is BatBlock)
        {
            Events.batExplodeAudioPlayEvent?.Invoke();

        }
        else if (curBlock is BombBlock)
        {
            curBlock.gameObject.GetComponent<BombBlock>().canTapped = false;
            curBlock.gameObject.GetComponent<BombBlock>().PlayRocketAnim(myDirection);
        }
        else if (curBlock is BottleBlock)
        {
            Events.bottlePopAudioPlayEvent?.Invoke();
            EffectsController.Instance.SpawnBalloonCrackEffect(blockObj.transform.position);
        }
        if (GoalPanel.Instance.CheckIsInGoals(curBlock.blockType))
        {
            GoalPanel.Instance.DecereaseGoal(curBlock.blockType);
        }
        curBlock.target = null;
        DOTween.Kill(blockObj);
        blockObj.transform.DOKill();  
        Destroy(blockObj, destroyTime);
    }
}
