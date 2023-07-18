using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yo_AudioCtrl : MonoBehaviour
{
    [Header("배경음 볼륨 (슬라이드)")]
    public float bgmSlider;
    [Header("효과음 볼륨 (슬라이드)")]
    public float effectSlider;

    [Header("배경음 볼륨 (자동)")]
    public float bgmAuto; // 0.5f이 최대
    [Header("배경음(물속소리) 볼륨 (자동)")]
    public float waterAuto;
    [Header("배경음(파도소리) 볼륨 (자동)")]
    public float waveAuto;
    [Header("배경음(파도소리) 볼륨 (자동)")]
    public float fireAuto;

    [Header("BGM")]
    public AudioSource audio_BGM;
    [Header("물속소리")]
    public AudioSource audio_Water;
    [Header("파도소리")]
    public AudioSource audio_Wave;
    [Header("장작소리")]
    public AudioSource audio_Fire;

    [Header("종이")]
    public AudioSource audio_Paper;
    [Header("3장 피아노")]
    public AudioSource auido_Piano;
    [Header("3장 피아노(땡,딩동댕)")]
    public AudioClip[] audio_PianoResult;
    [Header("2장 별(뽁)")]
    public AudioSource audio_Star;
    [Header("4장 쓰레기(슝)")]
    public AudioSource audio_TrashThrow;
    [Header("4장 쓰레기(탁)")]
    public AudioSource audio_TouchLight;
    [Header("4장 깨끗(자라란)")]
    public AudioSource audio_Clean;
    [Header("5장 안착(툭)")]
    public AudioSource audio_TouchHeavy;

    public AudioSource[] audio_Ps;

    void Update()
    {
        // BGM 조절
        audio_BGM.volume = bgmSlider * bgmAuto;

        // 1장,5장 중에는 물속 소리도 BGM에 포함
        audio_Water.volume = bgmSlider * waterAuto;

        // 2장,4장 중에는 파도 소리도 BGM에 포함
        audio_Wave.volume = bgmSlider * waveAuto;

        // 3장 중에는 장작 소리도 BGM에 포함
        audio_Fire.volume = bgmSlider * fireAuto;
    }

    public void BGMOn()
    {
        audio_BGM.gameObject.SetActive(true);
    }

    // 종이 넘기는 소리
    public void PaperMove()
    {
        // 볼륨 조절
        audio_Paper.volume = effectSlider;

        audio_Paper.Play();
    }

    // 피아노 효과음
    public void PianoSound(int audioNumber)
    {
        // 볼륨 조정
        auido_Piano.volume = effectSlider;

        for (int i = 0; i < audio_Ps.Length; i++)
        {
            audio_Ps[i].volume = effectSlider;
        }

        // 땡
        if (audioNumber == 5)
        {
            auido_Piano.clip = audio_PianoResult[0];
            auido_Piano.Play();
        }
        // 딩동댕
        else if(audioNumber == 6)
        {
            auido_Piano.clip = audio_PianoResult[1];
            auido_Piano.Play();
        }
        // 각각의 음계
        else
        {
            audio_Ps[audioNumber].Play();
        }
    }

    // 2장 별 뽁
    public void StarEffect()
    {
        // 볼륨 조절
        audio_Star.volume = effectSlider;

        audio_Star.Play();
    }

    // 쓰레기 던지는 소리
    public void TrashThrow()
    {
        // 볼륨 조절
        audio_TrashThrow.volume = effectSlider;

        audio_TrashThrow.Play();
    }

    // 부딪히는 소리 (탁)
    public void TouchLight()
    {
        // 볼륨 조절
        audio_TouchLight.volume = effectSlider;

        audio_TouchLight.Play();
    }

    // 부딪히는 소리 (툭)
    public void TouchHeavy()
    {
        // 볼륨 조절
        audio_TouchHeavy.volume = effectSlider;

        audio_TouchHeavy.Play();
    }

    // 깨끗해지는 소리
    public void Chapter4Clean()
    {
        // 볼륨 조절
        audio_Clean.volume = effectSlider;

        audio_Clean.Play();
    }


}
