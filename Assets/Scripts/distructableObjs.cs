﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distructableObjs : MonoBehaviour
{
    public float HP = 100.0f;
    public float damagedHealthStateThreshold = 50.0f;
    public Material fullHealthState;
    public Material damagedHealthState;
    public Material destroyedHealthState;
    public bool matOverrideFlag;
    public Material overrideMaterial;

    private MeshRenderer meshRenderer;
    private BoxCollider boxCol;
    public bool amDead = false;
    public bool isTV = false;

    public AudioClip woodded;
    public AudioClip stoneded;
    public AudioSource source;
    public bool playone = true;

    
    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCol = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        //For use in repair
        if (matOverrideFlag)
        {
            meshRenderer.material = overrideMaterial;
        }

        //If we are alive
        if (!amDead)
        {
            boxCol.enabled = true;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
        //If we are destroyed
        else
        {
            boxCol.enabled = false;
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }

        //If good health
        if (HP > damagedHealthStateThreshold)
        {
            playone = true;
            if (!matOverrideFlag)
            {
                meshRenderer.material = fullHealthState;
            }
            amDead = false;
        }
        //If damaged
        else if (HP <= damagedHealthStateThreshold && HP > 0)
        {
            if (!matOverrideFlag)
            {
                meshRenderer.material = damagedHealthState;
            }
            amDead = false;
        }
        //If destroyed
        else
        {
            if (!matOverrideFlag)
            {
                meshRenderer.material = destroyedHealthState;
            }

            amDead = true;

            if (playone == true)
            {
                playone = false;
                if (this.gameObject.tag == "Tile")
                {
                    source.clip = stoneded;
                    source.Play();
                }
                if (this.gameObject.tag == "Plank")
                {
                    source.clip = woodded;
                    source.Play();
                }

            }

        }

        //TV DED
        if (amDead && isTV)
        {
            gameObject.SetActive(false);
        }
    }
}
