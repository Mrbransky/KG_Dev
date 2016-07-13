using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum MissionObjectiveState
{
    Neutral = 0,
    Completed = 1,
    Failed = 2
}

public class MissionManager : MonoBehaviour 
{
    [SerializeField] private List<Image> missionObjectiveImageList;
    [SerializeField] private List<Image> missionObjectiveBackgroundImageList;
    [SerializeField] private GameObject missionObjectivePanel;

    private List<GameObject> missionObjectiveList;
    private MissionObjectiveState[] missionObjectiveStateArray;
    private int missionObjectiveCount = 0;

#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && missionObjectiveCount > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                OnMissionObjectiveFailed(0);
            }
            else
            {
                OnMissionObjectiveCompleted(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && missionObjectiveCount > 1)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                OnMissionObjectiveFailed(1);
            }
            else
            {
                OnMissionObjectiveCompleted(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && missionObjectiveCount > 2)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                OnMissionObjectiveFailed(2);
            }
            else
            {
                OnMissionObjectiveCompleted(2);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && missionObjectiveCount > 3)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                OnMissionObjectiveFailed(3);
            }
            else
            {
                OnMissionObjectiveCompleted(3);
            }
        }
    }
#endif
	
    public void AddMissionObjective(GameObject specialItem)
    {
        if (missionObjectiveCount == 0)
        {
            missionObjectiveList = new List<GameObject>();
        }

        missionObjectiveList.Add(specialItem);
        ++missionObjectiveCount;
        specialItem.GetComponent<MissionObjective_Item>().MissionObjectiveListIndex = missionObjectiveCount - 1;
    }

    public void Initialize()
    {
        missionObjectiveStateArray = new MissionObjectiveState[missionObjectiveCount];

        for (int i = 0; i < missionObjectiveCount; ++i)
        {
            missionObjectiveImageList[i].sprite = missionObjectiveList[i].GetComponent<SpriteRenderer>().sprite;

            missionObjectiveStateArray[i] = MissionObjectiveState.Neutral;
        }
    }

    public void OnMissionObjectiveCompleted(int missionObjectiveListIndex)
    {
        missionObjectiveStateArray[missionObjectiveListIndex] = MissionObjectiveState.Completed;
        missionObjectiveImageList[missionObjectiveListIndex].color = Color.white;
        missionObjectiveBackgroundImageList[missionObjectiveListIndex].color = Color.white;
        //missionObjectiveImageList[missionObjectiveListIndex].sprite = missionObjectiveList[missionObjectiveListIndex].GetComponent<MissionObjective_Item>().UnhighlightedSprite;

        foreach (MissionObjectiveState objectiveState in missionObjectiveStateArray)
        {
            if (objectiveState != MissionObjectiveState.Completed)
            {
                return;
            }
        }

        missionObjectivePanel.SetActive(false);
        GetComponent<GameManager>().OnHumansWin();
    }

    public void OnMissionObjectiveFailed(int missionObjectiveListIndex)
    {
        missionObjectiveStateArray[missionObjectiveListIndex] = MissionObjectiveState.Failed;
        missionObjectiveImageList[missionObjectiveListIndex].color = Color.red;

        foreach (MissionObjectiveState objectiveState in missionObjectiveStateArray)
        {
            if (objectiveState != MissionObjectiveState.Failed)
            {
                return;
            }
        }

        missionObjectivePanel.SetActive(false);
        GetComponent<GameManager>().OnGhostWin();
    }
}
