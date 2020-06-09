using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera main;
    public float duration=0.3f, magnitude=0.01f;
    private void Awake()
    {
        main = this;
    }
    public IEnumerator Shake()
    {
        Vector3 orignalPosition = transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            transform.position = transform.position+new Vector3(x, y, -10f);
            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(0.02f);
        }
        transform.position = orignalPosition;
    }
}
