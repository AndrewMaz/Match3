using UnityEngine;
using UnityEngine.UI;   

public class AudioManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string BGPref = "BGPref";

    private int firstPlayInt;
    public Slider bgSlider;
    private float bgFloat;

    public AudioSource BGAudio;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
        {
            bgFloat = .5f;
            bgSlider.value = bgFloat;
            PlayerPrefs.SetFloat(BGPref, bgFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            bgFloat = PlayerPrefs.GetFloat(BGPref);
            bgSlider.value = bgFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(BGPref, bgSlider.value);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        BGAudio.volume = bgSlider.value;
    }
}
