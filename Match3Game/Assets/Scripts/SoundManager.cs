using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioSource soundSfx;
    public static AudioClip switchSound, crackSound, clickSound, shuffleSound,
    poisonSound, crashSound, freezeSound, regenSound;

    private void Start()
    {
        soundSfx = GetComponent<AudioSource>();
        switchSound = Resources.Load<AudioClip>("switch_sound");
        crackSound = Resources.Load<AudioClip>("crack_sound");
        clickSound = Resources.Load<AudioClip>("click_sound");
        shuffleSound = Resources.Load<AudioClip>("shuffle_sound");
        poisonSound = Resources.Load<AudioClip>("poison_sound");
        crashSound = Resources.Load<AudioClip>("crash_sound");
        freezeSound = Resources.Load<AudioClip>("freeze_sound");
        regenSound = Resources.Load<AudioClip>("regen_sound");
    }

    public static void SwitchSound()
    {
        soundSfx.PlayOneShot(switchSound);
    }

    public static void CrackSound()
    {
        soundSfx.PlayOneShot(crackSound);
    }

    public static void ClickSound()
    {
        soundSfx.PlayOneShot(clickSound);
    }

    public static void ShuffleSound()
    {
        soundSfx.PlayOneShot(shuffleSound);
    }

    public static void PoisonSound()
    {
        soundSfx.PlayOneShot(poisonSound);
    }

    public static void CrashSound()
    {
        soundSfx.PlayOneShot(crashSound);
    }

    public static void FreezeSound()
    {
        soundSfx.PlayOneShot(freezeSound);
    }

    public static void RegenBossSound()
    {
        soundSfx.PlayOneShot(regenSound);
    }
}
