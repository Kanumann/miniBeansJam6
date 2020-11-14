using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum MusicState
{
    Start = 0,
    Transition = 1
}

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Start Parameter", EditorStyles.miniButton))
        {

            AudioManager myTarget = (AudioManager)target;
            myTarget.SetGlobalParameter("g_musicState", (float)MusicState.Start);
        }
        if (GUILayout.Button("Transition Parameter", EditorStyles.miniButton))
        {

            AudioManager myTarget = (AudioManager)target;
            myTarget.SetGlobalParameter("g_musicState", (float)MusicState.Transition);
        }
        DrawDefaultInspector();
    }
}

public class AudioManager : MonoBehaviour
{
    bool audioResumed = false;

    [SerializeField]
    bool debug = false;

    [SerializeField]
    [Range(-80f, 0f)]
    private float musicVolume;

    [SerializeField]
    [Range(-80f, 0f)]
    private float sfxVolume;

    private FMOD.Studio.EventInstance musicInstance;

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                var create = new GameObject(string.Concat(typeof(AudioManager),
                " Singleton")).AddComponent<AudioManager>();
                instance = create;
            }

            return instance;
        }
    }

    private FMOD.Studio.VCA musicVCA;
    public FMOD.Studio.VCA MusicVCA
    {
        get
        {
            return FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        }
    }

    private FMOD.Studio.VCA sfxVCA;
    public FMOD.Studio.VCA SfxVCA
    {
        get
        {
            return FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
        }
    }

    void Awake()
    {
        if (instance == null)
            instance = this as AudioManager;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public bool InitializeAudio()
    {
        if (!FixWebGLAudio())
        {
            return false;
        }

        if (!GetVCAVolumes())
        {
            return false;
        }

        if (!StartMusic())
        {
            return false;
        }

        return true;
    }

    bool GetVCAVolumes()
    {
        if (!CheckError(MusicVCA.getVolume(out float musicVolume)))
        {
            return false;
        }

        if (!CheckError(SfxVCA.getVolume(out float sfxVolume)))
        {
            return false;
        }

        musicVolume = LinearToDb(musicVolume);
        musicVolume = LinearToDb(sfxVolume);

        return true;
    }

    void Update()
    {
        if (debug)
        {
            CheckError(MusicVCA.setVolume(dBToLinear(musicVolume)));
            CheckError(SfxVCA.setVolume(dBToLinear(sfxVolume)));
        }
    }

    public bool FixWebGLAudio()
    {
        if (!audioResumed)
        {
            if (!CheckError(FMODUnity.RuntimeManager.CoreSystem.mixerSuspend()))
            {
                return false;
            }
            if (!CheckError(FMODUnity.RuntimeManager.CoreSystem.mixerResume()))
            {
                return false;
            }
            audioResumed = true;
        }
        return true;
    }

    private bool StartMusic()
    {
        if (!CheckError(PostEvent("event:/2D/Music", out musicInstance)))
        {
            return false;
        }
        return true;
    }

    public void PlayOneShot(string fmodEvent, GameObject gameObject = null)
    {
        if (gameObject)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached(fmodEvent, gameObject);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(fmodEvent);
        }
    }

    public void PlayOneShotParameter(string fmodEvent, string parameterName, float parameterValue, GameObject gameObject = null)
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        if (gameObject)
        {
            instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject.transform.position));
        }
        instance.setParameterByName(parameterName, parameterValue);
        instance.start();
        instance.release();
    }

    public void SetGlobalParameter(string parameterName, float parameterValue)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, parameterValue);
    }

    public FMOD.RESULT PostEvent(string fmodEvent, out FMOD.Studio.EventInstance musicInstance, GameObject gameObject = null)
    {
        var instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);

        if (gameObject)
        {
            FMODUnity.RuntimeManager.AttachInstanceToGameObject(instance, gameObject.transform, gameObject.GetComponent<Rigidbody>());
        }
        var result = instance.start();
        musicInstance = instance;
        return result;
    }

    float dBToLinear(float dB)
    {
        return Mathf.Pow(10.0f, dB / 20.0f);
    }

    float LinearToDb(float linear)
    {
        float dB;

        if (linear != 0)
            dB = 20.0f * Mathf.Log10(linear);
        else
            dB = -144.0f;

        return dB;
    }

    private bool CheckError(FMOD.RESULT result)
    {
        if (result != FMOD.RESULT.OK)
        {
            if (debug)
            {
                Debug.LogWarning("FMOD ERROR: " + result);
            }
            return false;
        }
        return true;
    }
}
