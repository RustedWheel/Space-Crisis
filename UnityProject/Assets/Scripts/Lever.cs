﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;
using System.Collections.Generic;

public class Lever : MonoBehaviour
{
    protected int remainingFrames = int.MaxValue;
    protected bool isRunning = false;
    public AudioClip pulledFX;
    public int timeInFrames;
    public List<GameObject> thingsToControl = new List<GameObject>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Platformer2DUserControl p1 = other.gameObject.GetComponent<Platformer2DUserControl>();
            if (p1 != null)
            {
                p1.lever = this;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Platformer2DUserControl p1 = other.gameObject.GetComponent<Platformer2DUserControl>();
            if (p1 != null)
            {
                p1.lever = null;
            }
        }
    }

    void Start()
    {
        Debug.Log(GameController.GetLeverInFinalPos(this.name));
        if (GameController.GetLeverInFinalPos(this.name))
        {
            Flip();
        }
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (isRunning) remainingFrames--;
        if (isRunning && remainingFrames < 1)
        {
            foreach (GameObject obj in thingsToControl)
            {
                PlateScript ps = obj.GetComponent<PlateScript>();
                ps.stop();
            }
            isRunning = false;
        }

    }

    public virtual void activate()
    {
        if (isRunning) return;

        bool finalPosition = GameController.ActivateLever(this.name);
        Debug.Log(finalPosition);
        isRunning = true;

        Flip();
        remainingFrames = timeInFrames;

        if (pulledFX != null)
        {
            AudioSource.PlayClipAtPoint(pulledFX, transform.position);
        }

        foreach (GameObject obj in thingsToControl)
        {
            PlateScript ps = obj.GetComponent<PlateScript>();
            if (!finalPosition)
            {
                // Move objs to
                Debug.Log("Reversing");
                ps.reverseDirection();
            } else
            {
                // Move objs back
            }
            Vector3 currentObjPos = obj.transform.position;

            ps.setAnimationTime(this.timeInFrames);
            ps.start();
        }

    }

    private void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void assignSoundFX(AudioClip clip)
    {
        pulledFX = clip;
    }
}
