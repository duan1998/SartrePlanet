using MPlot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlotUI : MonoBehaviour
{
    private Image m_bgImage;
    private Text m_text;

    private PlotGraph m_curPlots;

    public bool BShowing
    {
        get => transform.gameObject.activeInHierarchy;
    }
    private void Awake()
    {
        m_bgImage = transform.Find("Bg").GetComponent<Image>();
        m_text = transform.Find("Text").GetComponent<Text>();
    }

    


    // Update is called once per frame
    void Update()
    {
        if (BShowing && !bNext && Input.GetKeyDown(KeyCode.Space))
        {
            bNext = true;
            m_curPlots.NextPlot();
        }

    }

    bool bNext;
    public void ShowPlot(PlotGraph plots)
    {
        if (plots == null) return;
        m_curPlots = plots;
        m_curPlots.Restart();
        StartCoroutine("Play");
    }
    IEnumerator Play()
    {
        while (true)
        {
            if (m_curPlots.current == null)
            {
                m_curPlots = null;
                this.gameObject.SetActive(false);
                break;
            }
            Show(m_curPlots.current);
            bNext = false;
            yield return new WaitUntil(() => bNext);
        }
    }
    private void Show(MPlot.Plot plot)
    {
        m_bgImage.sprite = plot.bgSprite;
        m_text.text = plot.text;
    }


}
