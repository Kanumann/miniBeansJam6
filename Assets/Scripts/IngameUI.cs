using System;
using TMPro;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    public GameObject UIText;
    public bool Running = true;
    public int decimals = 2;

    private TextMeshProUGUI text;
    private float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = UIText.GetComponent<TextMeshProUGUI>();
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

    public void Reset()
    {
        text.SetText(FormatTime(score = 0));
    }
}
