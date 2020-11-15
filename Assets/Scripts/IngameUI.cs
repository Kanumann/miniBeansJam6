using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IngameUI : MonoBehaviour
{
    public GameObject IGMenu;
    public GameObject UIText;
    public GameObject UImgHeavy;
    public GameObject UImgFeather;
    public GameObject UImgAlarm;
    public GameObject EndMenu;
    public GameObject UIEndText;
    public bool Running = true;
    public int decimals = 2;

    private GameManager instance;
    private TextMeshProUGUI textIG;
    private TextMeshProUGUI textEnd;
    private Image rImgHeavy;
    private Image rImgFeather;
    private Image rImgAlarm;
    private float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Hackermann war hier
        var list = FindObjectsOfType<GameManager>();
        if (list.Length > 1) throw new Exception("IngameUI error: found too many GammeManagers!");
        instance = list[0];

        textIG = UIText.GetComponent<TextMeshProUGUI>();
        textEnd = UIEndText.GetComponent<TextMeshProUGUI>();
        SetText();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Running) return;
        score += Time.deltaTime;
        SetText();
    }

    private void SetText()
    {
        string formatted = FormatTime(score);
        textIG.SetText(formatted);
        textEnd.SetText(formatted);
    }

    private string FormatTime(float s)
    {
        int ms = (int)Math.Round((s - Math.Truncate(s)) * Math.Pow(10, decimals));
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
        score = 0;
        SetText();
    }

    public void ShowIGMenu(bool InGame = true)
    {
        IGMenu.SetActive(InGame);
        EndMenu.SetActive(!InGame);
    }

    /*
    public void SetHeavy(bool visible)
    {
        UImgHeavy.SetActive(visible);
    }
    public void SetFeather(bool visible)
    {
        UImgFeather.SetActive(visible);
    }
    public void SetAlarm(bool visible)
    {
        UImgAlarm.SetActive(visible);
    }
    */

    ///////////////////////////////////
    /// PUBLIC API FOR UI BUTTONS
    ///////////////////////////////////

    public void ButtonRetry()
    {
        // TODO Call Gamemanager
    }

    public void ButtonExit()
    {
        // TODO using this?
        SceneManager.LoadScene(0);
    }
}
