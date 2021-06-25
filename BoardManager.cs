using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections;

public class BoardManager : MonoBehaviour
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
    private Transform _boardHolder;
    private List<Vector3> _gridPositions = new List<Vector3>();
    private List<Vector3> _doors = new List<Vector3>();
    private Vector3 _newPosition = Vector3.zero;
    private int _nRooms;
    private int _rows;
    private int _columns;

    public static int BoardN = 0;
    public static List<Vector3> Positions = new List<Vector3>();
    public static int Dimension;

    public void DungeonSetup(int level){
        _nRooms = Random.Range(MinRooms, MaxRooms + 1);
        Debug.Log($"Random number of rooms : {_nRooms}");
        if(MinDimension == MaxDimension) Dimension = MinDimension;
        else Dimension = Random.Range(MinDimension, MaxDimension);
        _rows = Dimension;
        _columns = Dimension;
        Positions.Add(_newPosition);

        //Calculate position of rooms and doors
        for (int r = 0; r < _nRooms - 1; r++){
            //Side where will be instantiate the rooms (first in Vector3.zero)
            int rn = Random.Range(0, 4);
            bool coincidence = false;
            switch (rn){
                case 0: //Left
                    _newPosition.Set(_newPosition.x - _columns, 0, _newPosition.z);
                    for(int i=0; i < Positions.Count; i++){
                        if(_newPosition == Positions[i]){
                            coincidence = true;
                            break;
                        }
                    }
                    if(!coincidence && r != _nRooms - 1){
                        int sideRight = Random.Range((int)_newPosition.z + 2, (int)_newPosition.z + _rows - 2);
                        _doors.Add(new Vector3(_newPosition.x + _columns - 1, 0, sideRight));
                        _doors.Add(new Vector3(_newPosition.x + _columns, 0, sideRight));
                    }
                    break;

                case 1: //Right
                    _newPosition.Set(_newPosition.x + _columns, 0, _newPosition.z);
                    for(int i=0; i < Positions.Count; i++){
                        if(_newPosition == Positions[i]){
                            coincidence = true;
                            break;
                        }
                    }
                    if(!coincidence && r != _nRooms - 1){
                        int sideLeft = Random.Range((int)_newPosition.z + 2, (int)_newPosition.z + _rows - 2);
                        _doors.Add(new Vector3(_newPosition.x, 0, sideLeft));
                        _doors.Add(new Vector3(_newPosition.x - 1, 0, sideLeft));
                    }
                    break;

                case 2: //Up
                    _newPosition.Set(_newPosition.x, 0, _newPosition.z + _rows);
                    for(int i=0; i < Positions.Count; i++){
                        if(_newPosition == Positions[i]){
                            coincidence = true;
                            break;
                        }
                    }
                    if(!coincidence && r != _nRooms - 1){
                        int sideDown = Random.Range((int)_newPosition.x + 2, (int)_newPosition.x + _columns - 2);
                        _doors.Add(new Vector3(sideDown, 0, _newPosition.z));
                        _doors.Add(new Vector3(sideDown, 0, _newPosition.z -1));
                    }
                    break;

                case 3: //Down
                    _newPosition.Set(_newPosition.x, 0, _newPosition.z - _rows);
                    for(int i=0; i < Positions.Count; i++){
                        if(_newPosition == Positions[i]){
                            coincidence = true;
                            break;
                        }
                    }
                    if(!coincidence && r != _nRooms - 1){
                        int sideUp = Random.Range((int)_newPosition.x + 2, (int)_newPosition.x + _columns - 2);
                        _doors.Add(new Vector3(sideUp, 0, _newPosition.z + _rows - 1));
                        _doors.Add(new Vector3(sideUp, 0, _newPosition.z + _rows));
                    }
                    break;
            }
            if(coincidence){
                r -= 1;
                continue;
            }
            Positions.Add(_newPosition);
        }

        foreach(Vector3 door in _doors){
            Debug.Log($"Door in : {door}");
        }

        foreach(Vector3 position in Positions){
            Debug.Log($"Position in : {position}");
        }
        //Instantiate all
        for (int r = 0; r < _nRooms; r++){
            _gridPositions.Clear();
            _boardHolder = new GameObject($"Board {BoardN}").transform;

            int roomX = (int)Positions[r].x;
            int roomZ = (int)Positions[r].z;

            for (int x = roomX; x < roomX + _columns; x++){

                for (int z = roomZ; z < roomZ + _rows; z++){

                    GameObject toInstantiate = null;
                    //Floor
                    if (x > roomX && x < roomX + _columns -1 && z > roomZ && z < roomZ + _rows - 1){
                        toInstantiate = Instantiate(FloorTiles[Random.Range(0, FloorTiles.Length)], new Vector3(x, 0, z), Quaternion.identity);
                        /*if (x != 1 && z != 1 && x > Positions[r].x + 1 && 
                            x < Positions[r].x + _columns - 2 && 
                            z > Positions[r].z + 1 && 
                            z < Positions[r].z + _rows - 2){
                                _gridPositions.Add(new Vector3(x, 0, z));
                        }*/
                    }
                    //Left wall
                    else if (x == roomX && z != roomZ && z != roomZ + _rows - 1){
                        toInstantiate = _Wall(x, roomX, z, roomZ, 90);               
                    }
                    //Right wall
                    else if (x == roomX + _columns -1 && z != roomZ && z != roomZ + _rows -1){
                        toInstantiate = _Wall(x, roomX, z, roomZ, -90);
                    }
                    //Down wall
                    else if (z == roomZ && x != roomX && x != roomX + _columns -1){
                        toInstantiate = _Wall(x, roomX, z, roomZ, 0);
                    }
                    //Up wall
                    else if (z == roomZ + _rows -1 && x != roomX && x != roomX + _columns -1){
                        toInstantiate = _Wall(x, roomX, z, roomZ, 0);
                    }
                    //Corners
                    else{
                        toInstantiate = Instantiate(WallCornerTiles[Random.Range(0, WallCornerTiles.Length)], new Vector3(x, 0, z), Quaternion.identity);
                    }
                    if(toInstantiate) toInstantiate.transform.SetParent(_boardHolder);
                }
            }

            /*if(r != _nRooms - 1){
                LayoutObjectAtRandom(CoversTiles, WallCount.minimum, WallCount.maximum);
                LayoutObjectAtRandom(ChestTiles, ChestCount.minimum, ChestCount.maximum);
                //int enemyCount = (int)Math.Log(level * 2, 2);
                int enemyCount = 1;//PROVISIONAL ENEMIGOS 1
                //Aqui puedo poner minimo y maximo de enemigos
                LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);
            }*/
            BoardN++;
        }
    }

    private GameObject _Wall(int x, int roomX, int z, int roomZ, int yRotation){
        GameObject toInstatiate = null;
        for(int i = 0; i < _doors.Count; i++){
            Vector3 door = _doors[i];
            //Floor door
            if (x == door.x && z == door.z){
                return Instantiate(FloorTiles[Random.Range(0, FloorTiles.Length)], new Vector3(x, 0, z), Quaternion.identity);
            }
            //Right left door pillars, z axis (vertical walls)
            else if (x == roomX || x == roomX + _columns - 1){
                if ((x == door.x && z == door.z - 1) || (x == door.x && z == door.z + 1)){
                    return Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, z), Quaternion.identity);
                }
            }
            //Top down door pillars, x axis (horizontal walls)
            else if (z == roomZ || z == roomZ + _rows - 1){
                if ((x == door.x - 1 && z == door.z) || (x == door.x + 1 && z == door.z)){
                    return Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, z), Quaternion.identity);
                }
            }
            
        }
        //Wall
        if(!toInstatiate) return Instantiate(WallTiles[Random.Range(0, WallTiles.Length)], new Vector3(x, 0, z), Quaternion.Euler(0, yRotation, 0));
        return null;
    }

    //Función para seleccionar las posiciones donde instanciar en las salas
    /*private Vector3 _RandomPosition(){
        int randomIndex = Random.Range(0, _gridPositions.Count);
        Vector3 randomPosition = _gridPositions[randomIndex];
        _gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }*/

    //Función para instanciar los objetos en las salas
    /*private void _LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++){
            Vector3 randomPosition = _RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }*/
}
