using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class StartMusicWIthDelay : MonoBehaviour
{
    private AudioSource _audioSource;

    private IEnumerator Start()
    {
        _audioSource = GetComponent<AudioSource>();
        yield return null;
        _audioSource.Play();
    }
}