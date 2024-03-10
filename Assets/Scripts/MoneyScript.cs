using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyScript : MonoBehaviour
{
    bool collecting = false;
    public int value = 1;

    public SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        
        collecting = false;
        MoneyManager.Instance.moneys.Add(this);
        DOTween.Kill(transform);
        transform.DOMoveX(transform.position.x + Random.Range(-0.5f, 0.5f), 2.5f).SetEase(Ease.OutCubic);
        transform.DOMoveY(1, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        transform.DOScale(Vector3.one * 1.4f, 3.23f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    private void OnDisable()
    {
        DOTween.Kill(transform);
    }

    public void StartCollecting()
    {

        if(collecting)
        {
            return;
        }
        collecting = true;
        DOTween.Kill(transform);
        transform.DOMove(PlayerController.Instance.transform.position, Random.Range(3f, 5f))
            .SetEase(Ease.InCubic)
            .OnComplete(() => Collect());
    }

    public void Collect()
    {
        MoneyManager.Instance.money+=value;
        gameObject.SetActive(false);
        value = 0;
    }
}
