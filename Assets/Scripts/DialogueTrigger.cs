using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public List<DialogueDetails> dialogue;
    AudioSource audioSource;
    Coroutine delayRoutine;
    public List<GameObject> subtitles = new List<GameObject>();
    public bool newClipPlaying = false;
    public int currentClipID = 0;
    public RectTransform goAwayPos;
    public RectTransform appearPos;
    // Start is called before the first frame update
    void Start()
    {
        //find the source and all the subtitles
        audioSource = GameObject.Find("PlayerController").GetComponent<AudioSource>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Subtitles"))
        {
            subtitles.Add(obj);
        }

        //get the refs for appearing and disappearing

        goAwayPos = GameObject.Find("GoAwayPos").GetComponent<RectTransform>();
        appearPos = GameObject.Find("AppearPos").GetComponent<RectTransform>();
    }

    void Update()
    {
        //if a new clip was started and the clip is over, begin to play a new clip
        if(newClipPlaying && audioSource.time >= audioSource.clip.length)
        {
            newClipPlaying = false;
            delayRoutine = StartCoroutine(AudioDelay());

            foreach (var c in subtitles)
            {
                TextMeshProUGUI tmp = c.GetComponent<TextMeshProUGUI>();
                //Vector3 moveVector = c.gameObject.transform.position;
                //moveVector.y = -150;
                tmp.DOFade(0, 0.5f);
                c.transform.DOMove(goAwayPos.position, 0.5f);
            }
        }
    }

    //play the first clip
    public void PlayFirstAudioClip()
    {
        //change the text for subtitles and make them appear
        foreach (var c in subtitles)
        {
            TextMeshProUGUI tmp = c.GetComponent<TextMeshProUGUI>();
            Vector3 moveVector = c.gameObject.transform.position;
            moveVector.y = -50;
            c.gameObject.transform.position = moveVector;

            tmp.text = dialogue[0].dialogueText;
            tmp.DOFade(1, 0.5f);
            //moveVector.y = -100;
            c.transform.DOMove(appearPos.position, 0.5f);
        }
        //play the audio
        audioSource.clip = dialogue[0].audio;
        audioSource.Play();
        newClipPlaying = true;
    }

    //play the next clip in the sequence
    void PlayAudioClip()
    {
        foreach (var c in subtitles)
        {
            TextMeshProUGUI tmp = c.GetComponent<TextMeshProUGUI>();
            Vector3 moveVector = c.gameObject.transform.position;
            moveVector.y = -50;
            c.gameObject.transform.position = moveVector;

            tmp.text = dialogue[currentClipID].dialogueText;
            tmp.DOFade(1, 0.5f);
            //moveVector.y = -100;
            c.transform.DOMove(appearPos.position, 0.5f);
        }
        audioSource.clip = dialogue[currentClipID].audio;
        audioSource.Play();
        newClipPlaying = true;
    }

    //do the delay to wait between clips
    IEnumerator AudioDelay()
    {
        yield return new WaitForSeconds(dialogue[currentClipID].waitTimer);
        currentClipID++;
        PlayAudioClip();
    }
}

[System.Serializable]
public class DialogueDetails
{
    [TextArea]
    public string dialogueText;
    public AudioClip audio;
    public float waitTimer = 1.5f;
    public bool isInteruptable;
}
