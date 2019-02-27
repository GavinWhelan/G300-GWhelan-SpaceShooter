using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PickupBounce : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Sequence bounce = DOTween.Sequence();

        bounce.SetLoops(-1);
        bounce.Append(transform.DOScale(0.5f, 1));
        //bounce.Join(transform.DOShakePosition(0.5f, 0.5f));
        bounce.Append(transform.DOScale(1, 1));
        bounce.AppendInterval(1);

        //transform.DOScale(0.5f, 1).SetLoops(-1, LoopType.Yoyo).SetDelay(1)
    }
}
