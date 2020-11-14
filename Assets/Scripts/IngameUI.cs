using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class IngameUI : MonoBehaviour
{
    public GameObject UIText;
    public GameObject UImgHeavy;
    public GameObject UImgFeather;
    public GameObject UImgAlarm;
    public bool Running = true;
    public int decimals = 2;

    private TextMeshProUGUI text;
    private Image rImgHeavy;
    private Image rImgFeather;
    private Image rImgAlarm;
    private float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = UIText.GetComponent<TextMeshProUGUI>();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Running) return;
        score += Time.deltaTime;
        text.SetText(FormatTime(score));
    }

    private string FormatTime(float s)
    {
        int ms = (int) Math.Round((s - Math.Truncate(s)) * Math.Pow(10, decimals));
        int sec = (int)s % 60;
        int min = (int)s / 60;
        int hrs = (int)min / 60;
        min = min % 60;
        return $"{min:00}:{sec:00},{ms}";
    }


    ///////////////////////////////////
    /// PUBLIC API FOR GAME MANAGER
    ///////////////////////////////////


    public void Reset()
    {
        text.SetText(FormatTime(score = 0));
    }

    // TODO TBD in GameManager?
    public void SetHeavy(bool visible)
    {
        UImgHeavy.SetActive(visible);
    }

    // TODO TBD in GameManager?
    public void SetFeather(bool visible)
    {
        UImgFeather.SetActive(visible);
    }

    // TODO TBD in GameManager?
    public void SetAlarm(bool visible)
    {
        UImgAlarm.SetActive(visible);
    }
}
