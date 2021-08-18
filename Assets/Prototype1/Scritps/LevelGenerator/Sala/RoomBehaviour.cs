using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class RoomBehaviour : MonoBehaviour
    {
        RoomGenerator roomGenerator;
        public bool[] doors = { false, false, false, false };
        const float xSize = 8;
        const float ySize = 5;
        public string log;

        const int right = 0;
        const int left = 1;
        const int up = 2;
        const int down = 3;

        void Awake()
        {
            roomGenerator = FindObjectOfType<RoomGenerator>();
        }

        void Start()
        {
            GenerateOtherRoom();
        }

        void GenerateOtherRoom()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i])
                {
                    switch (i)
                    {
                        case 0:
                            if(!CheckCollition(right))
                                roomGenerator?.Generate(transform.position.x + xSize, transform.position.y, left);
                            break;
                        case 1:
                            if (!CheckCollition(left))
                                roomGenerator?.Generate(transform.position.x + (-xSize), transform.position.y, right);
                            break;
                        case 2:
                            if (!CheckCollition(up))
                                roomGenerator?.Generate(transform.position.x, transform.position.y + ySize, down);
                            break;
                        case 3:
                            if (!CheckCollition(down))
                                roomGenerator?.Generate(transform.position.x, transform.position.y + (-ySize), up);
                            break;
                    }
                    doors[i] = false;
                }
            }
        }

        bool CheckCollition(int checkCase)
        {
            float valX = 0f;
            float valY = 0f;
            int door = 0;

            switch (checkCase)
            {
                case 0:
                    valX = transform.position.x + xSize;
                    valY = transform.position.y;
                    door = 0;
                    break;
                case 1:
                    valX = transform.position.x + (-xSize);
                    valY = transform.position.y;
                    door = 1;
                    break;
                case 2:
                    valX = transform.position.x;
                    valY = transform.position.y + ySize;
                    door = 2;
                    break;
                case 3:
                    valX = transform.position.x;
                    valY = transform.position.y + (-ySize);
                    door = 3;
                    break;
            }

            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(valX, valY), 1f);
            foreach (Collider2D collider in colliders)
            {
                if (collider.GetComponent<RoomBehaviour>())
                {
                    doors[door] = false;
                    return true;
                }
            }
            return false;
        }
    }
}
