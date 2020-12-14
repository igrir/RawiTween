using System;
using System.Collections;
using RawiTween;
using UnityEngine;
using Random = UnityEngine.Random;

public class CubeSampleScript : MonoBehaviour
{
    public Ease ease;

    public void Start()
    {
        var position = this.transform.position;
        Vector3 targetPos = new Vector3(
            position.x + Random.Range(-10,10),
            position.y + Random.Range(-10,10),
            position.z + Random.Range(-10,10));
        this.transform.TweenPosition(targetPos, 3).SetEase(ease);
    }

    private void OnMouseDown()
    {
        this.transform
            .TweenLocalScale(new Vector3(0, 0, 0), 1f)
            .SetEase(Ease.InBounce)
            .OnFinish(() =>
            {
                if(this != null)
                    Destroy(this.gameObject);
            });
    }

}
