﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public GameObject gunTip;
    public LayerMask layerMask;

    public AudioClip woodmine;
    public AudioClip stonemine;
    public AudioSource source;

    void Update()
    {
        if (Input.GetButton("Fire1") == true)
        {
            GameObject.Find("RayGun").GetComponent<gunshake>().shooting();
            gunTip.GetComponent<LineRenderer>().enabled = true;
            if (!gunTip.GetComponent<ParticleSystem>().isPlaying)
            {
                gunTip.GetComponent<ParticleSystem>().Play();
            }
            if (!gunTip.GetComponent<AudioSource>().isPlaying)
            {
                gunTip.GetComponent<AudioSource>().Play();
            }
            RaycastHit hit;
            //If we hit something
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.gameObject.tag == "meteor")
                {
                    if (hit.transform.gameObject.GetComponent<meteor>().HP > 0.0f)
                    {
                        hit.transform.gameObject.GetComponent<meteor>().HP -= 250.0f * Time.deltaTime;
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("GameManager").GetComponent<score>().addscore(5.0f);

                        hit.transform.gameObject.GetComponent<meteor>().isshot();
                    }
                }
                else if (hit.transform.gameObject.tag == "mineableWood" || hit.transform.gameObject.tag == "mineableStone")
                {
                    if (hit.transform.gameObject.GetComponent<mineable>().HP > 0.0f)
                    {
                        hit.transform.gameObject.GetComponent<mineable>().HP -= 200.0f * Time.deltaTime;
                    }
                    else
                    {
                        GameObject.FindGameObjectWithTag("GameManager").GetComponent<score>().addscore(5.0f);
                        this.GetComponentInParent<Inventory>().UpdateInv(hit.transform.GetComponent<mineable>().selected, 1);
                        Destroy(hit.transform.gameObject);

                        if (hit.transform.GetComponent<mineable>().selected == Inventory.ITEM.STONE)
                        {
                            source.clip = stonemine;
                            source.Play();
                        }
                        else if (hit.transform.GetComponent<mineable>().selected == Inventory.ITEM.WOOD)
                        {
                            source.clip = woodmine;
                            source.Play();
                        }
                    }
                }
                //Hit Something
                gunTip.GetComponent<LineRenderer>().SetPosition(0, gunTip.transform.position);
                gunTip.GetComponent<LineRenderer>().SetPosition(1, hit.point);
                gunTip.transform.LookAt(hit.point);
            }
            else
            {
                //Pretend To Hit Something
                gunTip.GetComponent<LineRenderer>().SetPosition(0, gunTip.transform.position);
                gunTip.GetComponent<LineRenderer>().SetPosition(1, transform.forward * 1000.0f);
                gunTip.transform.LookAt(transform.forward * 1000.0f);
            }
        }
        else
        {
            gunTip.GetComponent<LineRenderer>().enabled = false;
            gunTip.GetComponent<AudioSource>().Stop();
            gunTip.GetComponent<ParticleSystem>().Stop();
        }
    }
}
