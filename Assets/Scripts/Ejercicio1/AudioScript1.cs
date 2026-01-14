using UnityEngine;

public class AudioScript1 : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip audioClip;

    public void OnPlay()
    {
        audioClip = (AudioClip)Resources.Load("Audio/IDElementos/Oxigeno");
        audioSource.clip = audioClip;
        if (!audioSource.isPlaying) audioSource.Play();
    }
}