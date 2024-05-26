using GMatch3.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMatch3
{
    public enum MoveState
    {
        wait,
        move
    }

    public class GridManager : MonoBehaviour
    {
        public static GridManager instance { get; private set; }
        private const int Attempt = 4;
        private const float FillTime = 0.5f;
        private const float DecreaseTimer = 0.4f;
        [SerializeField] private GameObject _tilePrefab;
        [SerializeField] private List<GameObject> _fruits;
        [SerializeField] private List<int> _crushedFruits;
        public List<int> CrushedFruits => _crushedFruits;
        [SerializeField] private Camera _camera;
        [SerializeField] private int _offSet;
        [SerializeField] private int _gridSizeX;
        [SerializeField] private int _gridSizeY;

        public int GridSizeX => _gridSizeX;
        public int GridSizeY => _gridSizeY;
        public MoveState currentState = MoveState.move;

        private GameObject[,] _allFruits;
        public GameObject[,] AllFruits => _allFruits;

        private int _streakValue;
        private int _fruitValue = 20;

        private Tile[,] _grid;
        private GameObject _tileGO;

        public Camera Camera => _camera;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else { Destroy(this.gameObject); }
        }
        private void Start()
        {
            _crushedFruits = new();
            for(int x=0 ; x < _fruits.Count ; x++)
            {
                _crushedFruits.Add(0);
            }
            CreateGrid();
        }

        private void CreateGrid()
        {
            _grid = new Tile[GridSizeX, GridSizeY];
            _allFruits = new GameObject[GridSizeX, GridSizeY];
            for (int x = 0; x < GridSizeX; x++)
                for (int y = 0; y < GridSizeY; y++)
                {
                    InstantiateGrid(x, y);  
                    SpawnFruit(x, y);
                }  
        }

        private void InstantiateGrid(int x, int y)
        {
            _tileGO = Instantiate(_tilePrefab, new Vector2(x,y), Quaternion.identity);
            _grid[x, y] = _tileGO.GetComponent<Tile>();
            _tileGO.transform.parent = this.transform;
        }

        private void CreateFruitItem(int x, int y, int fruitId, GameObject selectedFruit = null)
        {
            if (selectedFruit != null)
            {
                IMovable fruitNew = InstantiateFruit(x, y, fruitId, selectedFruit);
                _grid[x, y].Construct(new Vector2(x,y));
                _grid[x, y].AssignFruit(fruitNew);
                GameObject fruitObject = ((MonoBehaviour)fruitNew).gameObject;
                fruitObject.GetComponent<MoveFruit>().Row = y;
                fruitObject.GetComponent<MoveFruit>().Col = x;
                fruitObject.transform.parent = this.transform;
                _allFruits[x, y] = fruitObject;
            }
           
        }

        private void SpawnFruit(int x, int y)
        {
           
            int randomIndex = Random.Range(0, _fruits.Count);
            GameObject selectedFruit = _fruits[randomIndex];


            for (int attempts = 0; attempts < Attempt; attempts++)
            {
                if (!IsMatchingAdjacent(x, y, randomIndex))
                {
                    break;
                }

                randomIndex = Random.Range(0, _fruits.Count);
                selectedFruit = _fruits[randomIndex];
            }

            CreateFruitItem(x, y, randomIndex, selectedFruit);

        }



        private IMovable InstantiateFruit(int x, int y, int fruitId, GameObject selectedFruit)
        {
            GameObject gridFruit = Instantiate(selectedFruit, new Vector2(x,y + _offSet), Quaternion.identity);
            IMovable fruitNew = gridFruit.GetComponent<IMovable>();
            fruitNew.Construct(fruitId);
            
            return fruitNew;
        }

         
        private bool IsMatchingAdjacent(int x, int y, int fruitIndex)
        {
            if (x > 1 && _grid[x - 1, y] != null && _grid[x - 2, y] != null)
            {
                if (_grid[x - 1, y].Fruit.FruitID == fruitIndex && _grid[x - 2, y].Fruit.FruitID == fruitIndex)
                {
                    return true;
                }
            }

            if (y > 1 && _grid[x, y - 1] != null && _grid[x, y - 2] != null)
            {
                if (_grid[x, y - 1].Fruit.FruitID == fruitIndex && _grid[x, y - 2].Fruit.FruitID == fruitIndex)
                {
                    return true;
                }
            }

            if(y <= 1 || x <= 1)
            {
                if(y > 1)
                    if (_grid[x, y - 1].Fruit.FruitID == fruitIndex && _grid[x, y - 2].Fruit.FruitID == fruitIndex)
                    {
                        return true;
                    }
                if (x > 1)
                    if (_grid[x - 1, y].Fruit.FruitID == fruitIndex && _grid[x - 2, y].Fruit.FruitID == fruitIndex)
                    {
                        return true;
                    }
            }

            return false;
        }

        private void DestroyMatchedFruitsAt(int column, int row)
        {
            if (_allFruits[column, row].GetComponent<MoveFruit>().IsMatched)
            {
                _crushedFruits[_allFruits[column, row].GetComponent<IMovable>().FruitID]++;
                Destroy(_allFruits[column, row]);
                ScoreManager.instance.AddScore(_fruitValue * _streakValue);
                _allFruits[column, row] = null;
            }
        }

        public void DestroyMatchedFruits()
        {
            for (int x = 0; x < GridSizeX; x++)
                for (int y = 0; y < GridSizeY; y++)
                {
                    if (_allFruits[x, y] != null)
                    {
                        DestroyMatchedFruitsAt(x, y);
                    }
                }

            StartCoroutine(DecreaseRow());
        }

        private void RefillTile()
        {
            for (int x = 0; x < GridSizeX; x++)
                for (int y = 0; y < GridSizeY; y++)
                {
                    if (_allFruits[x, y] == null)
                    {
                        Vector2Int tempPosition = new Vector2Int(x, y);
                        int fruit = Random.Range(0, _fruits.Count);
                        GameObject selectedFruit = _fruits[fruit];
                        CreateFruitItem(tempPosition.x, tempPosition.y, fruit, selectedFruit);
                    }
                }
        }

        private IEnumerator DecreaseRow()
        {
            int nullCount = 0;
            for (int x = 0; x < GridSizeX; x++)
            {
                for (int y = 0; y < GridSizeY; y++)
                {
                    if (_allFruits[x, y] == null)
                    {
                        nullCount++;
                    }
                    else if (nullCount > 0)
                    {
                        _allFruits[x, y].GetComponent<MoveFruit>().Row -= nullCount;
                        _allFruits[x, y] = null;
                    }
                }
                nullCount = 0;
            }

            yield return new WaitForSeconds(DecreaseTimer);
            StartCoroutine(FillTile());
        }

        private bool Matches()
        {
            for (int x = 0; x < GridSizeX; x++)
                for (int y = 0; y < GridSizeY; y++)
                {
                    if (_allFruits[x, y] != null)
                    {
                        if (_allFruits[x, y].GetComponent<MoveFruit>().IsMatched) return true;
                    }
                }

            return false;
        }

        private IEnumerator FillTile()
        {
            RefillTile();
            yield return new WaitForSeconds(FillTime);

            while (Matches())
            {
                _streakValue ++;
                yield return new WaitForSeconds(FillTime);
                DestroyMatchedFruits();
            }

            yield return new WaitForSeconds(FillTime);
            currentState = MoveState.move;
            _streakValue = 1;
        }
    }
}
