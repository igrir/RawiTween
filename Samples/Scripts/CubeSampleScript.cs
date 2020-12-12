using System.Collections;
using RawiTween;
using UnityEngine;

public class CubeSampleScript : MonoBehaviour
{
    public Ease ease;

    public void Start()
    {
        StartCoroutine(WaitAndRun());
    }

    IEnumerator WaitAndRun()
    {
        yield return new WaitForSeconds(5);
        var position = this.transform.position;
        Vector3 targetPos = new Vector3(
            position.x + Random.Range(-10,10),
            position.y + Random.Range(-10,10),
            position.z + Random.Range(-10,10));
        this.transform.MoveTo(targetPos, 3).SetEase(ease).OnFinish(() =>
        {
            Debug.Log("Completed");
        });
    }
}
