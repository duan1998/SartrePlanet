using UnityEngine;
public class ArrowColor : MonoBehaviour
{
    public int ColorIndex;
    public Color[] colors = { Color.red, Color.blue, Color.yellow, Color.white };
    private void Awake()
    {
        ColorIndex = Random.Range(0, 4);
        GetComponent<SpriteRenderer>().color = colors[ColorIndex];
    }
}