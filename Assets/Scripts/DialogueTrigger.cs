using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public PlayerStoredDialogue storedDialogue;
    public bool isWalkableTrigger = false;
    public bool onlyTriggerableInCyberVision = false;
    public List<DialogueDetails> dialogue;
    public List<TimerDetails> TimerDialogue;
    AudioSource audioSource;
    Coroutine delayRoutine;
    public List<RectTransform> subtitles = new List<RectTransform>();
    public bool newClipPlaying = false;
    public int currentClipID = 0;
    public bool playedFirstClip = false;
    public bool interupted = false;
    public bool repeatable = false;
    public bool inExtraDialogue;
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
        if(newClipPlaying && audioSource.time >= audioSource.clip.length || !audioSource.isPlaying && newClipPlaying)
        {
            if(interupted == false)
            {
                if(inExtraDialogue == false)
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
                        //storedDialogue.currentDialogue = null;

                        if (TimerDialogue.Count > 0)
                        {
                            inExtraDialogue = true;
                            currentClipID = 0;
                            delayRoutine = StartCoroutine(RememberDelay());
                        }
                        //allow the trigger to activate again if we want the dialogue to be repeatable
                        if (repeatable == true)
                        {
                            playedFirstClip = false;
                        }
                    }
                }
                else
                {
                    //start the delay routine if its not last in the sequence
                    if (!TimerDialogue[currentClipID].isLastInSequence)
                    {
                        newClipPlaying = false;
                        delayRoutine = StartCoroutine(RememberDelay());
                    }
                    else
                    {
                        //reset the players stored dialogue data
                        storedDialogue.currentDialogue = null;

                        //allow the trigger to activate again if we want the dialogue to be repeatable
                        if (repeatable == true)
                        {
                            playedFirstClip = false;
                        }
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
                    if (storedDialogue.currentDialogue.delayRoutine != null)
                    {
                        storedDialogue.currentDialogue.StopCoroutine(storedDialogue.currentDialogue.delayRoutine);
                    }
                    
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
            dialogue[currentClipID].myEvent.Invoke();
        }
    }

    //play the next clip in the sequence
    void PlayAudioClip()
    {
        //if the dialogue is exclusive for glasses mode and youre not in that mode then skip this dialogue and find the next line to play
        if (!GameManager.Instance.isDouble.isOn && dialogue[currentClipID].isGlassesDialogue)
        {
            for (int i = currentClipID; i < dialogue.Count; i++)
            {
                if(dialogue[i].isGlassesDialogue == false)
                {
                    currentClipID = i;
                    break;
                }
            }
        }
        if (GameManager.Instance.isDouble.isOn && dialogue[currentClipID].isNormalModeDialogue)
        {
            for (int i = currentClipID; i < dialogue.Count; i++)
            {
                if (dialogue[i].isNormalModeDialogue == false)
                {
                    currentClipID = i;
                    break;
                }
            }
        }

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
        dialogue[currentClipID].myEvent.Invoke();
    }

    //do the delay to wait between clips
    IEnumerator AudioDelay()
    {
        yield return new WaitForSeconds(dialogue[currentClipID].waitTimer);
        currentClipID++;
        PlayAudioClip();
    }

    #region Timer Dialogue
    IEnumerator RememberDelay()
    {
        yield return new WaitForSeconds(TimerDialogue[currentClipID].waitTimer);
        if(storedDialogue.currentDialogue == null || storedDialogue.currentDialogue == this)
        {
            storedDialogue.currentDialogue = this;
            PlayRememberClip();
        }
    }

    void PlayRememberClip()
    {
        //if the dialogue is exclusive for glasses mode and youre not in that mode then skip this dialogue and find the next line to play
        if (!GameManager.Instance.isDouble.isOn && TimerDialogue[currentClipID].isGlassesDialogue)
        {
            for (int i = currentClipID; i < TimerDialogue.Count; i++)
            {
                if (TimerDialogue[i].isGlassesDialogue == false)
                {
                    currentClipID = i;
                    break;
                }
            }
        }
        //likewise for normal mode
        if (GameManager.Instance.isDouble.isOn && TimerDialogue[currentClipID].isNormalModeDialogue)
        {
            for (int i = currentClipID; i < TimerDialogue.Count; i++)
            {
                if (TimerDialogue[i].isNormalModeDialogue == false)
                {
                    currentClipID = i;
                    break;
                }
            }
        }

        if (GameManager.Instance.displaySubtitles.isOn)
        {
            foreach (var c in subtitles)
            {
                TextMeshProUGUI tmp = c.GetComponent<TextMeshProUGUI>();
                Vector3 moveVector = c.gameObject.transform.position;
                //moveVector.y = -50;
                //c.gameObject.transform.position = moveVector;
                c.DOAnchorPos(new Vector2(0, -50), 0f);

                tmp.text = TimerDialogue[currentClipID].dialogueText;
                tmp.DOFade(1, 0.5f);
                //moveVector.y = -100;
                //c.transform.DOMove(appearPos.position, 0.5f);
                c.DOAnchorPos(new Vector2(0, -105), 0.5f);
            }
        }

        audioSource.clip = TimerDialogue[currentClipID].audio;
        audioSource.Play();
        newClipPlaying = true;
        TimerDialogue[currentClipID].myEvent.Invoke();
    }

    #endregion

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
    public UnityEvent myEvent;
    public bool isGlassesDialogue;
    public bool isNormalModeDialogue;
}
[System.Serializable]
public class TimerDetails
{
    [TextArea]
    public string dialogueText;
    public AudioClip audio;
    public float waitTimer;
    public bool isNotInteruptable;
    public bool isLastInSequence = false;
    public UnityEvent myEvent;
    public bool isGlassesDialogue;
    public bool isNormalModeDialogue;
}

