using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MazeCreator : MonoBehaviour
{
    /*[Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }*/
    public int MinDimension;
    public int MaxDimension;
    public int MinRooms;
    public int MaxRooms;

    public GameObject[] FloorTiles;
    public GameObject[] WallTiles;
    public GameObject[] WallCornerTiles;
    public GameObject[] DoorCornerTiles;
    /*
        public Count WallCount = new Count(5, 9);//5,9
        public Count ChestCount = new Count(1, 5);//1,5

        public GameObject[] CoversTiles;
        public GameObject[] ChestTiles;
        public GameObject[] EnemyTiles;
    */

    public static List<Vector3> positions = new List<Vector3>();
    public static int dimension = 0;

    private List<Vector3> _doors = new List<Vector3>();
    private int _rows = 0;
    private int _columns = 0;

    public void Create(int level)
    {
        int nRooms = Random.Range(MinRooms, MaxRooms + 1);
        dimension = Random.Range(MinDimension, MaxDimension + 1);

        _rows = dimension;
        _columns = dimension;

        RandomRoomTilePositions(nRooms);

        // Debug.Log(nRooms);
        for (int nRoom = 0; nRoom < nRooms; nRoom++)
        {
            float roomX = positions[nRoom].x;
            float roomZ = positions[nRoom].z;

            InstantiateRoom(nRoom, roomX, roomZ);
        }

        // For debug purposes
        /*
        foreach (Vector3 door in _doors)
        {
            Debug.Log($"Door in : {door}");
        }

        foreach (Vector3 position in positions)
        {
            Debug.Log($"Position in : {position}");
        }*/
    }

    private void RandomRoomTilePositions(int nRooms)
    {
        const int Left = 0;
        const int Right = 1;
        const int Up = 2;
        const int Down = 3;

        Vector3 newPosition = Vector3.zero;
        positions.Add(newPosition);

        //Calculate position of rooms and doors
        for (int r = 0; r < nRooms - 1; r++)
        {
            //Side where will be instantiate the rooms (first in Vector3.zero)
            int randomN = Random.Range(0, 4);
            bool coincidence = false;
            switch (randomN)
            {
                case Left:
                    newPosition.Set(newPosition.x - _columns, 0, newPosition.z);
                    for (int i = 0; i < positions.Count; i++)
                    {
                        if (newPosition == positions[i])
                        {
                            coincidence = true;
                            break;
                        }
                    }
                    if (!coincidence && r != nRooms - 1)
                    {
                        int right = Random.Range((int)newPosition.z + 2, (int)newPosition.z + _rows - 2);
                        _doors.Add(new Vector3(newPosition.x + _columns - 1, 0, right));
                        _doors.Add(new Vector3(newPosition.x + _columns, 0, right));
                    }
                    break;

                case Right:
                    newPosition.Set(newPosition.x + _columns, 0, newPosition.z);
                    for (int i = 0; i < positions.Count; i++)
                    {
                        if (newPosition == positions[i])
                        {
                            coincidence = true;
                            break;
                        }
                    }
                    if (!coincidence && r != nRooms - 1)
                    {
                        int left = Random.Range((int)newPosition.z + 2, (int)newPosition.z + _rows - 2);
                        _doors.Add(new Vector3(newPosition.x, 0, left));
                        _doors.Add(new Vector3(newPosition.x - 1, 0, left));
                    }
                    break;

                case Up:
                    newPosition.Set(newPosition.x, 0, newPosition.z + _rows);
                    for (int i = 0; i < positions.Count; i++)
                    {
                        if (newPosition == positions[i])
                        {
                            coincidence = true;
                            break;
                        }
                    }
                    if (!coincidence && r != nRooms - 1)
                    {
                        int down = Random.Range((int)newPosition.x + 2, (int)newPosition.x + _columns - 2);
                        _doors.Add(new Vector3(down, 0, newPosition.z));
                        _doors.Add(new Vector3(down, 0, newPosition.z - 1));
                    }

                    break;

                case Down:
                    newPosition.Set(newPosition.x, 0, newPosition.z - _rows);
                    for (int i = 0; i < positions.Count; i++)
                    {
                        if (newPosition == positions[i])
                        {
                            coincidence = true;
                            break;
                        }
                    }
                    if (!coincidence && r != nRooms - 1)
                    {
                        int up = Random.Range((int)newPosition.x + 2, (int)newPosition.x + _columns - 2);
                        _doors.Add(new Vector3(up, 0, newPosition.z + _rows - 1));
                        _doors.Add(new Vector3(up, 0, newPosition.z + _rows));
                    }
                    break;
            }
            if (coincidence)
            {
                r -= 1;
                continue;
            }
            positions.Add(newPosition);
        }
    }

    private void InstantiateRoom(int roomN, float roomX, float roomZ)
    {
        Transform roomHolder = new GameObject($"room_{roomN}").transform;

        for (float x = roomX; x < roomX + _columns; x++)
        {

            for (float z = roomZ; z < roomZ + _rows; z++)
            {
                if (x > roomX && x < roomX + _columns - 1 && z > roomZ && z < roomZ + _rows - 1)
                {
                    _InstantiateFloor(x, z, roomHolder);
                }
                else if (x == roomX && z != roomZ && z != roomZ + _rows - 1)
                {
                    _InstantiateBorder("left",
                        x, roomX, z, roomZ, -90, roomHolder);
                }
                else if (x == roomX + _columns - 1 && z != roomZ && z != roomZ + _rows - 1)
                {
                    _InstantiateBorder("right",
                        x, roomX, z, roomZ, 90, roomHolder);
                }
                else if (z == roomZ + _rows - 1 && x != roomX && x != roomX + _columns - 1)
                {
                    _InstantiateBorder("up",
                        x, roomX, z, roomZ, 0, roomHolder);
                }
                else if (z == roomZ && x != roomX && x != roomX + _columns - 1)
                {
                    _InstantiateBorder("down",
                        x, roomX, z, roomZ, 180, roomHolder);
                }
                else
                {
                    _InstantiateCorner(x, roomX, z, roomZ, roomHolder);
                }

            }
        }
    }

    private void _InstantiateFloor(float x, float z, Transform parent)
    {
        GameObject floorTile = Instantiate(
            FloorTiles[Random.Range(0, FloorTiles.Length)],
            new Vector3(x, 0, z),
            Quaternion.Euler(90, 0, 0)
        );
        floorTile.name = "floor_tile";
        floorTile.transform.SetParent(parent);
    }

    private void _InstantiateBorder(string position, float x, float roomX, float z, float roomZ, float yRotation, Transform parent)
    {
        bool isDoor = false;
        foreach (Vector3 door in _doors)
        {
            if (x == door.x && z == door.z)
            {
                _InstantiateFloor(x, z, parent);
                isDoor = true;
                break;
            }

            if (x == roomX || x == roomX + _columns - 1)
            {
                if ((x == door.x && z == door.z - 1) || (x == door.x && z == door.z + 1))
                {
                    GameObject pillar = Instantiate(
                        DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)],
                        new Vector3(x, 0, z),
                        Quaternion.identity
                    );
                    pillar.name = "z_axis_door_pillar";
                    pillar.transform.SetParent(parent);

                    break;
                }
            }
            else if (z == roomZ || z == roomZ + _rows - 1)
            {
                if ((x == door.x - 1 && z == door.z) || (x == door.x + 1 && z == door.z))
                {
                    GameObject pillar = Instantiate(
                        DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)],
                        new Vector3(x, 0, z),
                        Quaternion.identity);

                    pillar.name = "x_axis_door_pillar";
                    pillar.transform.SetParent(parent);

                    break;
                }
            }

        }
        if (!isDoor) _InstantiateWall(position, x, z, yRotation, parent);
    }

    private void _InstantiateWall(string position, float x, float z, float yRotation, Transform parent)
    {
        switch (position)
        {
            case "left":
                x += 0.5f;
                break;

            case "right":
                x -= 0.5f;
                break;

            case "up":
                z -= 0.5f;
                break;

            case "down":
                z += 0.5f;
                break;
        }

        GameObject wall = Instantiate(
            WallTiles[Random.Range(0, WallTiles.Length)],
            new Vector3(x, 0, z),
            Quaternion.Euler(0, yRotation, 0)
        );
        wall.name = $"{position}_wall";
        wall.transform.SetParent(parent);
    }

    private void _InstantiateCorner(float x, float roomX, float z, float roomZ, Transform parent)
    {
        float xPos = x;
        float zPos = z;
        string name = "";
        if (x == roomX && z == roomZ)
        {
            xPos = x + 0.5f;
            zPos = z + 0.5f;
            name = "test";
        }
        else if (x == roomX && z == roomZ + _columns - 1)
        {
            xPos = x + 0.5f;
            zPos = z - 0.5f;
            name = "test2";
        }
        else if (x == roomX + _rows - 1 && z == roomZ + _columns - 1)
        {
            xPos = x - 0.5f;
            zPos = z - 0.5f;
            name = "test3";
        }
        else if (x == roomX + _rows - 1 && z == roomZ)
        {
            xPos = x - 0.5f;
            zPos = z + 0.5f;
            name = "test4";
        }
        GameObject corner = Instantiate(
            WallCornerTiles[Random.Range(0, WallCornerTiles.Length)],
            new Vector3(xPos, 0, zPos),
            Quaternion.identity
        );
        if (name != "")
        {
            corner.name = name;
        }
        else
        {
            corner.name = "corner";
        }

        corner.transform.SetParent(parent);
    }

    //Function to instantiate enemies at random positions
    /*private Vector3 _RandomPosition(){
        int randomIndex = Random.Range(0, _gridPositions.Count);
        Vector3 randomPosition = _gridPositions[randomIndex];
        _gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }*/

    //Function to instantiate objects at random positions
    /*private void _LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++){
            Vector3 randomPosition = _RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }*/
}
