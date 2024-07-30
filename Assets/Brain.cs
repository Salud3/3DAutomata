using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Brain : MonoBehaviour
{
    public static Brain Instance;

    [SerializeField] GameObject cell;

    bool[,,] MapB;
    bool[,,] MapFuture;
    int[,,] MapCounter;

    [Header("Info")]
    [SerializeField] int cant;
    [SerializeField] int IT;
    [SerializeField] TextMeshProUGUI it_text;

    [Header("reglas")]
    [SerializeField] int Survival=0;
    [SerializeField] int Birth=0;
    [SerializeField] int State=0;

    public enum TIPO_ITERACION { M, VN }
    [SerializeField] TIPO_ITERACION tipoElecto = TIPO_ITERACION.M;

    [Header("ConfigCells")]
    [SerializeField] int xCell = 15;
    [SerializeField] int yCell = 15;
    [SerializeField] int zCell = 15;

    public List<GameObject> cells = new List<GameObject>();
    public Dictionary<Vector3,GameObject> cellAct = new Dictionary<Vector3,GameObject>();

    private void Start()
    {
        Instance = this;
    }

    public void SetX(string x)
    {
        xCell = int.Parse(x);
    }

    public void SetY(string y)
    {
        yCell = int.Parse(y);
    }

    public void SetZ(string z)
    {
        zCell = int.Parse(z);
    }

    public void SetType(int type)
    {
        switch (type)
        {
            case 0:
                tipoElecto = TIPO_ITERACION.M;
                break;
            case 1:
                tipoElecto = TIPO_ITERACION.VN;
                break;
            default:
                break;
        }
    }

    public void SetSurvival(string Sv)
    {
        Survival = int.Parse(Sv);
    }

    public void SetBirth(string br)
    {
        Birth = int.Parse(br);
    }

    public void SetState(string St)
    {
        State = int.Parse(St);
    }

    public void Play()
    {
        bool st = State == 0;
        bool bt = Birth == 0;
        bool sv = Survival == 0;

        bool _x = xCell == 0;
        bool _y = yCell == 0;
        bool _z = zCell == 0;

        if (st || bt || sv || _x || _y || _z)
        {

            UIController.instance.Clear();

        }
        else
        {
            UIController.instance.Play();
            it_text.gameObject.SetActive(true);
            CalcDimension();
            init();
            PLAY = true;
        }
    }

    public void Pause()
    {
        PLAY = false;
    }

    public void Resume()
    {
        PLAY = true;
        Actualization();
    }

    void CalcDimension()
    {

        MapB = new bool[xCell, yCell, zCell];
        MapCounter = new int[xCell, yCell, zCell];
        MapFuture = MapB;
        cant = xCell * yCell * zCell;
        cant = Mathf.FloorToInt(cant * 0.6f);
        Debug.Log(cant);
    }
    void init()
    {
        GenCellUnit();
    }
    void GenCellUnit()
    {
        Debug.Log(tipoElecto);
        //Metodo de Spawn 60%
        for (int index = 0; index < cant; index++)
        {
            int _x = index % xCell;
            int _y = (index / xCell) % yCell;
            int _z = index / (xCell * yCell);

            GameObject temp = Instantiate(cell, new Vector3(0, 0, 0), Quaternion.identity);

            MapB[_x, _y, _z] = false;

            MapCounter[_x, _y, _z] = 0;

            cells.Add(temp);

            temp.SetActive(false);

        }

        for (int x = -xCell / 2; x < xCell / 2; x++)
        {
            for (int y = -yCell / 2; y < yCell / 2; y++)
            {
                for (int z = -zCell / 2; z < zCell / 2; z++)
                {
                    if (Random.Range(0, 5) == 0)
                    {
                        GameObject temp = cells[0];
                        temp.SetActive(true);
                        cells.RemoveAt(0);
                        temp.transform.position = new Vector3(x, y, z);
                        cellAct.Add(temp.transform.position, temp);
                        MapB[x+xCell/2, y+yCell/2, z + zCell/2] = true;
                        MapCounter[x+xCell/2, y+yCell/2, z+zCell/2] = State;

                    }
                }
            }
        }
        Invoke("Actualization", 1);


    }
    //new Vector2(((-_Xcells / 2) + i) + (i * 0.1f), ((-_Ycells / 2) + j) + (j * 0.1f))

    //Celulas

    void CheckCellM(int x, int y, int z)
    {
        int lives = 0;

        for (int _x = -1; _x < 2; _x++)
        {
            for (int _y = -1; _y < 2; _y++)
            {
                for (int _z = -1; _z < 2; _z++)
                {
                    if (_x == 0 && _y == 0 && _z == 0)
                        continue;

                    int a = x + _x;
                    int b = y + _y;
                    int c = z + _z;

                    if (a >= 0 && b >= 0 && c >= 0 && a < xCell && b < yCell && c < zCell && MapB[a, b, c])
                    {
                        lives++;
                    }

                }
            }
        }

        if (MapB[x, y, z])
        {
            if (lives == Survival)
            {
                MapFuture[x, y, z] = true;
                MapCounter[x, y, z] = State;
            }
            else
            {
                MapFuture[x,y,z] = false;
                MapCounter[x, y, z]--;
                    Debug.Log(MapCounter[x,y,z]);
            }
        }
        else
        {
            if (lives == Birth)
            {
                MapFuture[x, y, z] = lives == Birth;
                MapCounter[x, y, z] = State;
            }
            else
            {
                MapFuture[x,y,z] = false;
                MapCounter[x, y, z]--;
                Debug.Log(MapCounter[x,y,z]);
                if (MapCounter[x,y,z] <=0)
                {
                    MapCounter[x, y, z] = 0;
                    Debug.Log(MapCounter[x,y,z]);
                }
            }
        }

    }

    int[,] offSetsVN = new int[6, 3] {
        { 1, 0, 0 },
        { -1, 0, 0 },
        { 0, 1, 0 },
        { 0, -1, 0 },
        { 0, 0, 1 },
        { 0, 0, -1 }
    };

    void CheckCellVN(int x, int y, int z)
    {
        int lives = 0;

        for (int i = 0; i < 6; i++)
        {
            int a = x + offSetsVN[i, 0];
            int b = y + offSetsVN[i, 1];
            int c = z + offSetsVN[i, 2];

            if (a >= 0 && b >= 0 && c >= 0 && a < xCell && b < yCell && c < zCell && MapB[a, b, c])
            {
                lives++;
            }
        }

        Debug.Log("Total " + lives);   
        if (MapB[x, y, z])
        {
            if (lives == Survival)
            {
                MapFuture[x, y, z] = true;
                MapCounter[x, y, z] = State;
            }
            else
            {
                MapFuture[x,y,z] = false;
                MapCounter[x, y, z]--;
                    Debug.Log(MapCounter[x,y,z]);
            }
        }
        else
        {
            if (lives == Birth)
            {
                MapFuture[x, y, z] = lives == Birth;
                MapCounter[x, y, z] = State;
            }
            else
            {
                MapFuture[x,y,z] = false;
                MapCounter[x, y, z]--;
                Debug.Log(MapCounter[x,y,z]);
                if (MapCounter[x,y,z] <=0)
                {
                    MapCounter[x, y, z] = 0;
                    Debug.Log(MapCounter[x,y,z]);
                }
            }
        }

    }
    
    void PaintMap()
    {
        for (int x = -xCell / 2; x < xCell / 2; x++)
        {
            for (int y = -yCell / 2; y < yCell / 2; y++)
            {
                for (int z = -zCell / 2; z < zCell / 2; z++)
                {
                    Vector3 tempVect = new Vector3(x, y, z);

                    if (MapB[x + xCell / 2, y + yCell / 2, z + zCell / 2])
                    {
                        if (!cellAct.ContainsKey(tempVect))
                        {
                            GameObject newCell;

                            if (cells.Count > 0)
                            {
                                newCell = cells[0];
                                cells.RemoveAt(0);
                            }
                            else
                            {
                                // No hay celulas disponibles y busca una
                                newCell = FindActiveCell();
                            }

                            newCell.transform.position = tempVect;
                            newCell.SetActive(true);
                            cellAct[tempVect] = newCell;
                        }
                    }
                    else
                    {
                        if (cellAct.ContainsKey(tempVect))
                        {
                            if (MapCounter[x+xCell/2,y+yCell/2,z + zCell/2] <= 0)
                            {
                                GameObject cellToDeactivate = cellAct[tempVect];
                                cellAct.Remove(tempVect);
                                cells.Add(cellToDeactivate);
                                cellToDeactivate.SetActive(false);
                            }
                        }
                    }
                }
            }
        }

    }
    

    GameObject FindActiveCell()
    {
        Debug.Log("Encontrando");
        GameObject TempCell = null;
        Vector3 TempVect = new Vector3(Random.Range(-xCell/5,xCell/5), Random.Range(yCell/5,yCell/5), Random.Range(-zCell / 5, zCell / 5) );
        bool f = false;

        foreach (var a in cellAct)
        {
            Vector3 position = a.Key;
            if (!f || (position.x == TempVect.x || position.y == TempVect.y || position.z == TempVect.z) )
            {
                TempCell = a.Value;
                TempVect = position;
                f = true;
            }
        }

        if (TempCell != null)
        {
            cellAct.Remove(TempVect);
            TempCell.SetActive(false);
        }
        else
        {
            Debug.LogError("Whoops");
            return Instantiate(cell);   
        }

        return TempCell;
    }


    public bool PLAY;
    private void Actualization()
    {
        for (int i = 0; i < xCell; i++)
        {
            for (int j = 0; j < yCell; j++)
            {
                for (int z = 0; z < zCell; z++)
                {
                    if (tipoElecto == TIPO_ITERACION.M)
                    {
                        CheckCellM(i, j, z);

                    }else if(tipoElecto == TIPO_ITERACION.VN)
                    {
                        CheckCellVN(i, j, z);
                    }
                }
            }
        }

        var temp = MapB;
        MapB = MapFuture;
        MapFuture = temp;

        if (PLAY)
        {
            PaintMap();
            IT++;
            it_text.text = "iteraciones: " + IT.ToString();
            if (cells.Count >= cant)
            {
                PLAY = false;
                foreach (var item in cellAct)
                {
                    item.Value.SetActive(false);
                }

                UIController.instance.Dead();
            }
            else
            {

                Invoke("Actualization", 1);
            }

        }
    }

}
