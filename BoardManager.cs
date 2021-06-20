using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

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
    public int minDimension;
    public int maxDimension;
    public int minRooms;
    public int maxRooms;


    int rows;
    int columns;

    public Count wallCount = new Count(5, 9);//5,9
    public Count chestCount = new Count(1, 5);//1,5
    public GameObject[] floorTiles;
    public GameObject[] coversTiles;
    public GameObject[] chestTiles;
    public GameObject[] enemyTiles;
    public GameObject[] wallTiles;
    public GameObject[] wallCornerTiles;
    public GameObject[] doorCornerTiles;
    private Transform boardHolder;
    public static int boardn = 0;
    private List<Vector2> gridPositions = new List<Vector2>();
    public static List<Vector2> usedPositions = new List<Vector2>();
    private List<Vector2> doors = new List<Vector2>();
    private Vector2 myPosition = Vector2.zero;

    int nrooms;
    public static int nroomsPublic;

    public static int dimension;

    public void DungeonSetup(int level){
        nrooms = Random.Range(minRooms, maxRooms + 1);
        nroomsPublic = nrooms;
        dimension = Random.Range(minDimension, maxDimension);
        rows = dimension;
        columns = dimension;
        usedPositions.Add(myPosition);

        //Calcula posiciones de puertas y habitaciones
        for (int r = 0; r < nrooms; r++){
            //Decision hacia donde se instanciara la sala (despues de la primera que es vector2.zero)
            int rn = Random.Range(0, 4);
            bool coincidence = false;
            switch (rn){
                //Izquierda
                case 0:

                    do{
                        myPosition = new Vector2(myPosition.x - columns, myPosition.y);
                        for (int i = 0; i < usedPositions.Count; i++){
                            if (myPosition == usedPositions[i]){
                                coincidence = true;
                                break;
                            }
                            if (myPosition != usedPositions[i]) coincidence = false;
                        }
                    } while (coincidence);

                    if(r != nrooms - 1){
                        int sideRight = Random.Range((int)myPosition.y + 2, (int)myPosition.y + rows - 2);
                        doors.Add(new Vector2(myPosition.x + columns - 1, sideRight));
                        doors.Add(new Vector2(myPosition.x + columns, sideRight));
                    }
                    break;

                //Derecha
                case 1:

                    do{
                        myPosition = new Vector2(myPosition.x + columns, myPosition.y);

                        for (int i = 0; i < usedPositions.Count; i++){
                            if (myPosition == usedPositions[i])
                                coincidence = true;
                                break;

                            if (myPosition != usedPositions[i]) coincidence = false;
                        }
                    } while (coincidence);

                    if (r != nrooms - 1){
                        int sideLeft = Random.Range((int)myPosition.y + 2, (int)myPosition.y + rows - 2);
                        doors.Add(new Vector2(myPosition.x, sideLeft));
                        doors.Add(new Vector2(myPosition.x - 1, sideLeft));
                    }
                    break;

                //Arriba
                case 2:
                    do{
                        myPosition = new Vector2(myPosition.x, myPosition.y + rows);

                        for (int i = 0; i < usedPositions.Count; i++){
                            if (myPosition == usedPositions[i])
                                coincidence = true;
                                break;

                            if (myPosition != usedPositions[i]) coincidence = false;
                        }
                    } while (coincidence);

                    if (r != nrooms - 1){
                        int sideDown = Random.Range((int)myPosition.x + 2, (int)myPosition.x + columns - 2);
                        doors.Add(new Vector2(sideDown, myPosition.y));
                        doors.Add(new Vector2(sideDown, myPosition.y -1));
                    }
                    break;

                //Abajo
                case 3:

                    do{
                        myPosition = new Vector2(myPosition.x, myPosition.y - rows);

                        for (int i = 0; i < usedPositions.Count; i++){
                            if (myPosition == usedPositions[i]){
                                coincidence = true;
                                break;
                            }

                            if (myPosition != usedPositions[i]) coincidence = false;
                        }
                    } while (coincidence);

                    if (r != nrooms - 1){
                        int sideUp = Random.Range((int)myPosition.x + 2, (int)myPosition.x + columns - 2);
                        doors.Add(new Vector2(sideUp, myPosition.y + rows - 1));
                        doors.Add(new Vector2(sideUp, myPosition.y + rows));
                    }
                    break;
            }
            usedPositions.Add(myPosition);
        }

        //Instancia todo en su lugar
        for (int r = 0; r < nrooms; r++){
            gridPositions.Clear();
            GameObject toInstantiate = null;
            
            string nameBoard = "Board " + boardn;

            boardHolder = new GameObject(nameBoard).transform;

            for (int x = (int)usedPositions[r].x; x < usedPositions[r].x + columns; x++){
                for (int y = (int)usedPositions[r].y; y < usedPositions[r].y + rows; y++){
                    //Lado Izquierdo
                    if (x == usedPositions[r].x && y != usedPositions[r].y && y != usedPositions[r].y + rows -1){
                        bool coincidence = false;
                        for(int i = 0; i < doors.Count; i++){
                            if (x == doors[i].x && y == doors[i].y)
                                //toInstantiate = Instantiate(corridorCorner[Random.Range(0, corridorCorner.Length)], new Vector2(x, y), Quaternion.identity);
                                toInstantiate = Instantiate(floorTiles[Random.Range(0, wallTiles.Length)], new Vector2(x, y), Quaternion.identity);
                                //toInstantiate = Instantiate(corridorCorner[Random.Range(0, corridorCorner.Length)], new Vector2(x, y), Quaternion.identity);
                                coincidence = true;

                            if (x == doors[i].x && y == doors[i].y - 1)
                                toInstantiate = Instantiate(doorCornerTiles[Random.Range(0, doorCornerTiles.Length)], new Vector2(x, y), Quaternion.identity);
                                coincidence = true;

                            if (x == doors[i].x && y == doors[i].y + 1)
                                toInstantiate = Instantiate(doorCornerTiles[Random.Range(0, doorCornerTiles.Length)], new Vector2(x, y), Quaternion.Euler(0, 0, -90));
                                coincidence = true;
                        }

                        if(!coincidence) toInstantiate = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], new Vector2(x, y), Quaternion.identity);                    
                    }


                    //Lado Derecho
                    if (x == usedPositions[r].x + columns -1 && y != usedPositions[r].y && y != usedPositions[r].y + rows -1){
                        bool coincidence = false;
                        for (int i = 0; i < doors.Count; i++){
                            if (x == doors[i].x && y == doors[i].y)
                                toInstantiate = Instantiate(floorTiles[Random.Range(0, wallTiles.Length)], new Vector2(x, y), Quaternion.identity);
                                //toInstantiate = Instantiate(corridorCorner[Random.Range(0, corridorCorner.Length)], new Vector2(x, y), Quaternion.identity);
                                coincidence = true;

                            if (x == doors[i].x && y == doors[i].y - 1)
                                toInstantiate = Instantiate(doorCornerTiles[Random.Range(0, doorCornerTiles.Length)], new Vector2(x, y), Quaternion.Euler(0,0,90));
                                coincidence = true;

                            if (x == doors[i].x && y == doors[i].y + 1)
                                toInstantiate = Instantiate(doorCornerTiles[Random.Range(0, doorCornerTiles.Length)], new Vector2(x, y), Quaternion.Euler(0, 0, 180));
                                coincidence = true;
                        }

                        if (!coincidence) toInstantiate = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], new Vector2(x, y), Quaternion.Euler(0, 0, 180));
                    }

                    //Lado Abajo
                    if (y == usedPositions[r].y && x != usedPositions[r].x && x != usedPositions[r].x + columns -1){
                        bool coincidence = false;
                        for (int i = 0; i < doors.Count; i++){
                            if (x == doors[i].x && y == doors[i].y)
                                toInstantiate = Instantiate(floorTiles[Random.Range(0, wallTiles.Length)], new Vector2(x, y), Quaternion.identity);
                                coincidence = true;

                            if(x == doors[i].x - 1 && y == doors[i].y)
                                toInstantiate = Instantiate(doorCornerTiles[Random.Range(0, doorCornerTiles.Length)], new Vector2(x, y), Quaternion.identity);
                                coincidence = true;

                            if(x == doors[i].x + 1 && y == doors[i].y)
                                toInstantiate = Instantiate(doorCornerTiles[Random.Range(0, doorCornerTiles.Length)], new Vector2(x, y), Quaternion.Euler(0, 0, 90));
                                coincidence = true;
                        }

                        if (!coincidence) toInstantiate = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], new Vector2(x, y), Quaternion.Euler(0, 0, 90));
                    }

                    //Lado Arriba
                    if (y == usedPositions[r].y + rows -1 && x != usedPositions[r].x && x != usedPositions[r].x + columns -1){
                        bool coincidence = false;
                        for (int i = 0; i < doors.Count; i++)
                        {
                            if (x == doors[i].x && y == doors[i].y)
                                toInstantiate = Instantiate(floorTiles[Random.Range(0, wallTiles.Length)], new Vector2(x, y), Quaternion.identity);
                                coincidence = true;

                            if (x == doors[i].x - 1 && y == doors[i].y)
                                toInstantiate = Instantiate(doorCornerTiles[Random.Range(0, doorCornerTiles.Length)], new Vector2(x, y), Quaternion.Euler(0,0,-90));
                                coincidence = true;

                            if (x == doors[i].x + 1 && y == doors[i].y)
                                toInstantiate = Instantiate(doorCornerTiles[Random.Range(0, doorCornerTiles.Length)], new Vector2(x, y), Quaternion.Euler(0,0,180));
                                coincidence = true;
                        }

                        if (!coincidence) toInstantiate = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], new Vector2(x, y), Quaternion.Euler(0, 0, 270));
                    }

                    //Esquina inferior izquierda 
                    if (x == usedPositions[r].x && y == usedPositions[r].y)
                        toInstantiate = Instantiate(wallCornerTiles[Random.Range(0, wallCornerTiles.Length)], new Vector2(x, y), Quaternion.identity);
                    
                    //Esquina superior izquierda
                    if (x == usedPositions[r].x && y == usedPositions[r].y + rows -1)
                        toInstantiate = Instantiate(wallCornerTiles[Random.Range(0, wallCornerTiles.Length)], new Vector2(x, y), Quaternion.Euler(0, 0, 270));
                    
                    // Esquina inferior derecha
                    if (x == usedPositions[r].x + columns -1 && y == usedPositions[r].y)
                        toInstantiate = Instantiate(wallCornerTiles[Random.Range(0, wallCornerTiles.Length)], new Vector2(x, y), Quaternion.Euler(0, 0, 90));
                    
                    //Esquina superior derecha
                    if (x == usedPositions[r].x + columns -1 && y == usedPositions[r].y + rows -1)
                        toInstantiate = Instantiate(wallCornerTiles[Random.Range(0, wallCornerTiles.Length)], new Vector2(x, y), Quaternion.Euler(0, 0, 180));

                    //Suelo
                    if (x > usedPositions[r].x && x < usedPositions[r].x + columns -1 && y > usedPositions[r].y && y < usedPositions[r].y + rows - 1){
                        toInstantiate = Instantiate(floorTiles[Random.Range(0, floorTiles.Length)], new Vector2(x, y), Quaternion.identity);
                        if (x != 1 && y != 1 && x > usedPositions[r].x + 1 && 
                            x < usedPositions[r].x + columns - 2 && 
                            y > usedPositions[r].y + 1 && 
                            y < usedPositions[r].y + rows - 2){
                                gridPositions.Add(new Vector2(x, y));
                        }

                        if(r == nrooms - 1){

                        }
                    }
                    if(toInstantiate != null) toInstantiate.transform.SetParent(boardHolder);
                }
            }

            if(r != nrooms - 1){
                LayoutObjectAtRandom(coversTiles, wallCount.minimum, wallCount.maximum);
                LayoutObjectAtRandom(chestTiles, chestCount.minimum, chestCount.maximum);
                //int enemyCount = (int)Math.Log(level * 2, 2);
                int enemyCount = 1;//PROVISIONAL ENEMIGOS 1
                //Aqui puedo poner minimo y maximo de enemigos
                LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
            }

            boardn++;
        }
    }

    //Función para seleccionar las posiciones donde instanciar en las salas
    Vector2 RandomPosition(){
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector2 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    //Función para instanciar los objetos en las salas
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++){
            Vector2 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
}
