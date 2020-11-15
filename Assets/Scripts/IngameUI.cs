using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IngameUI : MonoBehaviour
{
    public GameObject IGMenu;
    public GameObject UIText;
    public GameObject UIZone;
    public GameObject EndMenu;
    public GameObject UIEndText;
    public bool Running = true;
    public int decimals = 1;

    // TODO STEVEN
    public float ZoneValue;

    private GameManager instance;
    private TextMeshProUGUI textIG;
    private TextMeshProUGUI textEnd;
    private TextMeshProUGUI textZone;
    private float score = 0;

    private bool initFirst = false;

    // Start is called before the first frame update
    void Start()
    {
        // Hackermann war hier
        var list = FindObjectsOfType<GameManager>();
        if (list.Length > 1) throw new Exception("IngameUI error: found too many GammeManagers!");
        instance = list[0];

        Init();
        SetText();
    }


    void Init()
    {
        if(initFirst)return;
        initFirst = true;

        textIG = UIText.GetComponent<TextMeshProUGUI>();
        textEnd = UIEndText.GetComponent<TextMeshProUGUI>();
        textZone = UIZone.GetComponent<TextMeshProUGUI>();
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
        Init();

        string formatted = FormatTime(score);
        textIG.SetText(formatted);
        textEnd.SetText(formatted);

        // set zone
        textZone.SetText(FormatTime(ZoneValue));
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
        instance.StartGame();
    }

    public void ButtonExit()
    {
        // TODO using this?
        SceneManager.LoadScene(0);
    }
}
