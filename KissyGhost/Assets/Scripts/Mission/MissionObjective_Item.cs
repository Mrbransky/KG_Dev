﻿using UnityEngine;
using System.Collections;

public class MissionObjective_Item : MonoBehaviour
{
    public int MissionObjectiveListIndex = -1;
    public bool IsItemPlacedDown = false;
    
    void OnTriggerStay2D(Collider2D col)
    {
        if (IsItemPlacedDown)
        {
            MissionObjective_ItemNode itemNodeScript = col.GetComponent<MissionObjective_ItemNode>();

            if (itemNodeScript != null && !itemNodeScript.HasItem)
            {
                itemNodeScript.HasItem = true;
                GameObject.FindGameObjectWithTag("GameManager").GetComponent<MissionManager>().OnMissionObjectiveCompleted(MissionObjectiveListIndex);
                transform.position = itemNodeScript.ItemTargetPosition;
                //Item in Heartagram Sound
                soundManager.SOUND_MAN.playSound("Play_Item_In_Heartagram", gameObject);

                if (GetComponent<ThrowableItem>() != null)
                {
                    Destroy(GetComponent<ThrowableItem>());
                }

                Destroy(GetComponent<Collider2D>());
            }
        }
    }
}
