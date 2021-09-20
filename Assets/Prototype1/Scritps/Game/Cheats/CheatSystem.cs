using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheatSystem : MonoBehaviour
{
    [SerializeField] GameObject inputField;
    bool inputState;

    private void Start()
    {
        inputField.SetActive(inputState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            inputState = !inputState;
            if (inputState)
                inputField.SetActive(true);
            else
                inputField.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Return) && inputState)
        {
            inputField.SetActive(false);
            inputState = false;
        }
    }

    public void ActivateCommand()
    {
        string command = inputField.GetComponent<InputField>().text;
        Proto1.PlayerAttack pj = FindObjectOfType<Proto1.PlayerAttack>();
        RoomPrefabs rp = FindObjectOfType<RoomPrefabs>();
        //
        switch (command)
        {
            case "god":
                pj.max_HP = 1000000f;
                pj.actual_HP = 1000000f;
                pj.playerDamage = 100000;
                break;
            case "max damage":
                pj.playerDamage = 100000;
                break;
            case "max hp":
                pj.max_HP = 1000000f;
                pj.actual_HP = 1000000f;
                break;
            case "tp BossRoom":
                int index = rp.roomList.Count - 1;
                Vector3 bossPosition = rp.roomList[index].transform.position;
                pj.gameObject.transform.position = bossPosition;
                break;
        }
    }
}
