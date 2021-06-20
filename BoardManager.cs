using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    public int MinDimension;
    public int MaxDimension;
    public int MinRooms;
    public int MaxRooms;


    int rows;
    int columns;

    
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
    private Vector3 _myPosition = Vector3.zero;
    private int _nRooms;

    public static int boardn = 0;
    public static List<Vector3> usedPositions = new List<Vector3>();
    public static int nRoomsPublic;
    public static int dimension;

    public void DungeonSetup(int level){
        _nRooms = Random.Range(MinRooms, MaxRooms + 1);
        //
        Debug.Log($"Random number of rooms : {_nRooms}");
        dimension = Random.Range(MinDimension, MaxDimension);
        rows = dimension;
        columns = dimension;
        usedPositions.Add(_myPosition);

        //Calculate position of rooms and doors
        for (int r = 0; r < _nRooms; r++){
            //Side where will be instantiate the rooms (first in Vector3.zero)
            int rn = Random.Range(0, 4);
            bool coincidence = false;
            switch (rn){
                //Izquierda
                case 0:
                    do{
                        _myPosition = new Vector3(_myPosition.x - columns, _myPosition.y);
                        for (int i = 0; i < usedPositions.Count; i++){
                            if (_myPosition == usedPositions[i]){
                                coincidence = true;
                                break;
                            }
                            else if (_myPosition != usedPositions[i]) coincidence = false;
                        }
                    } while (coincidence);

                    if(r != _nRooms - 1){
                        int sideRight = Random.Range((int)_myPosition.y + 2, (int)_myPosition.y + rows - 2);
                        _doors.Add(new Vector3(_myPosition.x + columns - 1, sideRight));
                        _doors.Add(new Vector3(_myPosition.x + columns, sideRight));
                    }
                    break;

                //Derecha
                case 1:
                    do{
                        _myPosition = new Vector3(_myPosition.x + columns, _myPosition.y);

                        for (int i = 0; i < usedPositions.Count; i++){
                            if (_myPosition == usedPositions[i]){
                                coincidence = true;
                                break;
                            }
                            else if (_myPosition != usedPositions[i]) coincidence = false;
                        }
                    } while (coincidence);

                    if (r != _nRooms - 1){
                        int sideLeft = Random.Range((int)_myPosition.y + 2, (int)_myPosition.y + rows - 2);
                        _doors.Add(new Vector3(_myPosition.x, sideLeft));
                        _doors.Add(new Vector3(_myPosition.x - 1, sideLeft));
                    }
                    break;

                //Arriba
                case 2:
                    do{
                        _myPosition = new Vector3(_myPosition.x, _myPosition.y + rows);

                        for (int i = 0; i < usedPositions.Count; i++){
                            if (_myPosition == usedPositions[i]){
                                coincidence = true;
                                break;
                            }
                            else if (_myPosition != usedPositions[i]) coincidence = false;
                        }
                    } while (coincidence);

                    if (r != _nRooms - 1){
                        int sideDown = Random.Range((int)_myPosition.x + 2, (int)_myPosition.x + columns - 2);
                        _doors.Add(new Vector3(sideDown, _myPosition.y));
                        _doors.Add(new Vector3(sideDown, _myPosition.y -1));
                    }
                    break;

                //Abajo
                case 3:
                    do{
                        _myPosition = new Vector3(_myPosition.x, _myPosition.y - rows);

                        for (int i = 0; i < usedPositions.Count; i++){
                            if (_myPosition == usedPositions[i]){
                                coincidence = true;
                                break;
                            }
                            else if (_myPosition != usedPositions[i]) coincidence = false;
                        }
                    } while (coincidence);

                    if (r != _nRooms - 1){
                        int sideUp = Random.Range((int)_myPosition.x + 2, (int)_myPosition.x + columns - 2);
                        _doors.Add(new Vector3(sideUp, _myPosition.y + rows - 1));
                        _doors.Add(new Vector3(sideUp, _myPosition.y + rows));
                    }
                    break;
            }
            usedPositions.Add(_myPosition);
        }

        //Instancia todo en su lugar
        for (int r = 0; r < _nRooms; r++){
            _gridPositions.Clear();
            GameObject toInstantiate = null;
            string nameBoard = "Board " + boardn;
            _boardHolder = new GameObject(nameBoard).transform;

            for (int x = (int)usedPositions[r].x; x < usedPositions[r].x + columns; x++){
                for (int y = (int)usedPositions[r].y; y < usedPositions[r].y + rows; y++){
                    //Lado Izquierdo
                    if (x == usedPositions[r].x && y != usedPositions[r].y && y != usedPositions[r].y + rows -1){
                        bool coincidence = false;
                        for(int i = 0; i < _doors.Count; i++){
                            if (x == _doors[i].x && y == _doors[i].y){
                                //toInstantiate = Instantiate(corridorCorner[Random.Range(0, corridorCorner.Length)], new Vector3(x, 0, y), Quaternion.identity);
                                toInstantiate = Instantiate(FloorTiles[Random.Range(0, WallTiles.Length)], new Vector3(x, 0, y), Quaternion.identity);
                                //toInstantiate = Instantiate(corridorCorner[Random.Range(0, corridorCorner.Length)], new Vector3(x, 0, y), Quaternion.identity);
                                coincidence = true;
                            }
                            if (x == _doors[i].x && y == _doors[i].y - 1){
                                toInstantiate = Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.identity);
                                coincidence = true;
                            }
                            if (x == _doors[i].x && y == _doors[i].y + 1){
                                toInstantiate = Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0, 0, -90));
                                coincidence = true;
                            }
                        }

                        if(!coincidence) toInstantiate = Instantiate(WallTiles[Random.Range(0, WallTiles.Length)], new Vector3(x, 0, y), Quaternion.identity);                    
                    }
                    //Lado Derecho
                    else if (x == usedPositions[r].x + columns -1 && y != usedPositions[r].y && y != usedPositions[r].y + rows -1){
                        bool coincidence = false;
                        for (int i = 0; i < _doors.Count; i++){
                            if (x == _doors[i].x && y == _doors[i].y){
                                toInstantiate = Instantiate(FloorTiles[Random.Range(0, WallTiles.Length)], new Vector3(x, 0, y), Quaternion.identity);
                                //toInstantiate = Instantiate(corridorCorner[Random.Range(0, corridorCorner.Length)], new Vector3(x, 0, y), Quaternion.identity);
                                coincidence = true;
                            }
                            if (x == _doors[i].x && y == _doors[i].y - 1){
                                toInstantiate = Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0,0,90));
                                coincidence = true;
                            }
                            if (x == _doors[i].x && y == _doors[i].y + 1){
                                toInstantiate = Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0, 0, 180));
                                coincidence = true;
                            }
                        }

                        if (!coincidence) toInstantiate = Instantiate(WallTiles[Random.Range(0, WallTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0, 0, 180));
                    }
                    //Lado Abajo
                    else if (y == usedPositions[r].y && x != usedPositions[r].x && x != usedPositions[r].x + columns -1){
                        bool coincidence = false;
                        for (int i = 0; i < _doors.Count; i++){
                            if (x == _doors[i].x && y == _doors[i].y){
                                toInstantiate = Instantiate(FloorTiles[Random.Range(0, WallTiles.Length)], new Vector3(x, 0, y), Quaternion.identity);
                                coincidence = true;
                            }
                            if(x == _doors[i].x - 1 && y == _doors[i].y){
                                toInstantiate = Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.identity);
                                coincidence = true;
                            }
                            if(x == _doors[i].x + 1 && y == _doors[i].y){
                                toInstantiate = Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0, 0, 90));
                                coincidence = true;
                            }
                        }
                        if (!coincidence) toInstantiate = Instantiate(WallTiles[Random.Range(0, WallTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0, 0, 90));
                    }
                    //Lado Arriba
                    else if (y == usedPositions[r].y + rows -1 && x != usedPositions[r].x && x != usedPositions[r].x + columns -1){
                        bool coincidence = false;
                        for (int i = 0; i < _doors.Count; i++)
                        {
                            if (x == _doors[i].x && y == _doors[i].y){
                                toInstantiate = Instantiate(FloorTiles[Random.Range(0, WallTiles.Length)], new Vector3(x, 0, y), Quaternion.identity);
                                coincidence = true;
                            }
                            if (x == _doors[i].x - 1 && y == _doors[i].y){
                                toInstantiate = Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0,0,-90));
                                coincidence = true;
                            }
                            if (x == _doors[i].x + 1 && y == _doors[i].y){
                                toInstantiate = Instantiate(DoorCornerTiles[Random.Range(0, DoorCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0,0,180));
                                coincidence = true;
                            }
                        }
                        if (!coincidence) toInstantiate = Instantiate(WallTiles[Random.Range(0, WallTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0, 0, 270));
                    }
                    //Esquina inferior izquierda 
                    else if (x == usedPositions[r].x && y == usedPositions[r].y){
                        toInstantiate = Instantiate(WallCornerTiles[Random.Range(0, WallCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.identity);
                    }
                    //Esquina superior izquierda
                    else if (x == usedPositions[r].x && y == usedPositions[r].y + rows -1){
                        toInstantiate = Instantiate(WallCornerTiles[Random.Range(0, WallCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0, 0, 270));
                    }
                    // Esquina inferior derecha
                    else if (x == usedPositions[r].x + columns -1 && y == usedPositions[r].y){
                        toInstantiate = Instantiate(WallCornerTiles[Random.Range(0, WallCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0, 0, 90));
                    }
                    //Esquina superior derecha
                    else if (x == usedPositions[r].x + columns -1 && y == usedPositions[r].y + rows -1){
                        toInstantiate = Instantiate(WallCornerTiles[Random.Range(0, WallCornerTiles.Length)], new Vector3(x, 0, y), Quaternion.Euler(0, 0, 180));
                    }
                    //Suelo
                    else if (x > usedPositions[r].x && x < usedPositions[r].x + columns -1 && y > usedPositions[r].y && y < usedPositions[r].y + rows - 1){
                        toInstantiate = Instantiate(FloorTiles[Random.Range(0, FloorTiles.Length)], new Vector3(x, 0, y), Quaternion.identity);
                        if (x != 1 && y != 1 && x > usedPositions[r].x + 1 && 
                            x < usedPositions[r].x + columns - 2 && 
                            y > usedPositions[r].y + 1 && 
                            y < usedPositions[r].y + rows - 2){
                                _gridPositions.Add(new Vector3(x, 0, y));
                        }
                        //ALGO IBA A HACER AQUI AVERIGUAR (QUIZA INSTANCIAR PUERTA)
                        //if(r == _nRooms - 1){}
                    }
                    else if(toInstantiate != null) toInstantiate.transform.SetParent(_boardHolder);
                }
            }

            /*
            if(r != _nRooms - 1){
                LayoutObjectAtRandom(CoversTiles, WallCount.minimum, WallCount.maximum);
                LayoutObjectAtRandom(ChestTiles, ChestCount.minimum, ChestCount.maximum);
                //int enemyCount = (int)Math.Log(level * 2, 2);
                int enemyCount = 1;//PROVISIONAL ENEMIGOS 1
                //Aqui puedo poner minimo y maximo de enemigos
                LayoutObjectAtRandom(EnemyTiles, enemyCount, enemyCount);
            }*/

            boardn++;
        }
    }

    //Función para seleccionar las posiciones donde instanciar en las salas
    Vector3 RandomPosition(){
        int randomIndex = Random.Range(0, _gridPositions.Count);
        Vector3 randomPosition = _gridPositions[randomIndex];
        _gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    //Función para instanciar los objetos en las salas
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++){
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
}
