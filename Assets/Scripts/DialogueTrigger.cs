using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public PlayerStoredDialogue storedDialogue;
    public bool isWalkableTrigger = false;
    public bool onlyTriggerableInCyberVision = false;
    public List<DialogueDetails> dialogue;
    AudioSource audioSource;
    Coroutine delayRoutine;
    public List<RectTransform> subtitles = new List<RectTransform>();
    public bool newClipPlaying = false;
    public int currentClipID = 0;
    public bool playedFirstClip = false;
    public bool interupted = false;
    public bool repeatable = false;
    // Start is called before the first frame update
    void Start()
    {
        //find the source and all the subtitles
        audioSource = GameObject.Find("CameraRig").GetComponent<AudioSource>();
        storedDialogue = GameObject.Find("PlayerController").GetComponent<PlayerStoredDialogue>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Subtitles"))
        {
            subtitles.Add(obj.GetComponent<RectTransform>());
        }
    }

    void Update()
    {
        //if a new clip was started and the clip is over, begin to play a new clip
        if(newClipPlaying && audioSource.time >= audioSource.clip.length)
        {
            if(interupted == false)
            {
                //start the delay routine if its not last in the sequence
                if (!dialogue[currentClipID].isLastInSequence)
                {
                    newClipPlaying = false;
                    delayRoutine = StartCoroutine(AudioDelay());
                }
                else
                {
                    //reset the players stored dialogue data
                    storedDialogue.currentDialogue = null;
                    //allow the trigger to activate again if we want the dialogue to be repeatable
                    if(repeatable == true)
                    {
                        playedFirstClip = false;
                    }
                }
            }

            //get rid of the subtutles from the players screen
            if (GameManager.Instance.displaySubtitles.isOn)
            {
                foreach (var c in subtitles)
                {
                    TextMeshProUGUI tmp = c.GetComponent<TextMeshProUGUI>();
                    //Vector3 moveVector = c.gameObject.transform.position;
                    //moveVector.y = -150;
                    tmp.DOFade(0, 0.5f);
                    //c.transform.DOMove(goAwayPos.position, 0.5f);
                    c.DOAnchorPos(new Vector2(0, -150), 0.5f);
                }
            }
        }
    }

    //play the first clip
    public void PlayFirstAudioClip()
    {
        if(!playedFirstClip)
        {
            //make sure all values are reset
            playedFirstClip = true;
            currentClipID = 0;

            //if the player has a currently stored dialogue, interupt it if possible
            if(storedDialogue.currentDialogue != null)
            {
                if (storedDialogue.currentDialogue.dialogue[storedDialogue.currentDialogue.currentClipID].isNotInteruptable == false)
                {
                    storedDialogue.currentDialogue.StopCoroutine(storedDialogue.currentDialogue.delayRoutine);
                    storedDialogue.currentDialogue.interupted = true;
                    storedDialogue.currentDialogue.audioSource.Stop();
                    storedDialogue.currentDialogue = null;
                }
            }

            storedDialogue.currentDialogue = this;

            if (delayRoutine != null)
            {
                StopCoroutine(delayRoutine);
            }
            if (GameManager.Instance.displaySubtitles.isOn)
            {
                //change the text for subtitles and make them appear
                foreach (var c in subtitles)
                {
                    TextMeshProUGUI tmp = c.GetComponent<TextMeshProUGUI>();
                    Vector3 moveVector = c.gameObject.transform.position;
                    //moveVector.y = -50;
                    //c.gameObject.transform.position = moveVector;
                    c.DOAnchorPos(new Vector2(0, -50), 0f);

                    tmp.text = dialogue[0].dialogueText;
                    tmp.DOFade(1, 0.5f);
                    //moveVector.y = -100;
                    c.DOAnchorPos(new Vector2(0, -105), 0.5f);
                }
            }

            //play the audio
            audioSource.clip = dialogue[0].audio;
            audioSource.Play();
            newClipPlaying = true;
        }
    }

    //play the next clip in the sequence
    void PlayAudioClip()
    {
        if (GameManager.Instance.displaySubtitles.isOn)
        {
            foreach (var c in subtitles)
            {
                TextMeshProUGUI tmp = c.GetComponent<TextMeshProUGUI>();
                Vector3 moveVector = c.gameObject.transform.position;
                //moveVector.y = -50;
                //c.gameObject.transform.position = moveVector;
                c.DOAnchorPos(new Vector2(0, -50), 0f);

                tmp.text = dialogue[currentClipID].dialogueText;
                tmp.DOFade(1, 0.5f);
                //moveVector.y = -100;
                //c.transform.DOMove(appearPos.position, 0.5f);
                c.DOAnchorPos(new Vector2(0, -105), 0.5f);
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playedFirstClip == false && isWalkableTrigger)
        {
            if(onlyTriggerableInCyberVision && GameManager.Instance.CybervisionOn)
            {
                PlayFirstAudioClip();
            }
            else if(onlyTriggerableInCyberVision == false)
            {
                PlayFirstAudioClip();
            }
            
        }
    }
}

[System.Serializable]
public class DialogueDetails
{
    [TextArea]
    public string dialogueText;
    public AudioClip audio;
    public float waitTimer = 1.5f;
    public bool isNotInteruptable;
    public bool isLastInSequence = false;
}
