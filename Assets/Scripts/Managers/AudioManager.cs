using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource balloonPopAudio;
    [SerializeField] private AudioSource duckExplodeAudio;
    [SerializeField] private AudioSource cubeExplosionAudio;
    [SerializeField] private AudioSource cubeCollectAudio;

    public void PlayBalloonPopAudio()
    {
        balloonPopAudio.Play();
    }
    public void PlayDuckExplodeAudio()
    {
        duckExplodeAudio.Play();
    }
    public void PlayCubeExplosionAudio()
    {
        cubeExplosionAudio.Play();
    }
    public void PlayCubeCollectAudio()
    {
        cubeCollectAudio.Play();
    }

    private void OnEnable()
    {
        Events.cubeCollectAudioPlayEvent += PlayCubeCollectAudio;
        Events.cubeExplosionAudioPlayEvent += PlayCubeExplosionAudio;
        Events.bottlePopAudioPlayEvent += PlayBalloonPopAudio;
        Events.batExplodeAudioPlayEvent += PlayDuckExplodeAudio;
    }
    private void OnDestroy()
    {
        Events.cubeCollectAudioPlayEvent -= PlayCubeCollectAudio;
        Events.cubeExplosionAudioPlayEvent -= PlayCubeExplosionAudio;
        Events.bottlePopAudioPlayEvent -= PlayBalloonPopAudio;
        Events.batExplodeAudioPlayEvent -= PlayDuckExplodeAudio;
    }

}
