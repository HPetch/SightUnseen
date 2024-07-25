using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueCheck : MonoBehaviour
{
    public DialogueTrigger parent;
    public DialogueTrigger myTrigger;
    public bool isCyberVisionCheck;
    public bool isEyeTakeOutCheck;
    public bool isEyeStopCheck;
    public Collider col;
    public EyeHolder playerEye;
    // Start is called before the first frame update
    void Start()
    {
        playerEye = GameObject.Find("Detached Eyeball 2").GetComponent<EyeHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (parent.playedFirstClip)
        {
            if (isCyberVisionCheck && GameManager.Instance.CybervisionOn)
            {
                EnableMe();
            }
            if (isEyeTakeOutCheck && playerEye.eyeIsSpawned)
            {
                EnableMe();
            }
            if (isEyeStopCheck && playerEye.eyeSetDown)
            {
                EnableMe();
            }
        }
    }

    void EnableMe()
    {
        col.enabled = true;
    }
}
