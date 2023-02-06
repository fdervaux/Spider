using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeGenerator : MonoBehaviour
{
    // m,n Size of the maze
    public Vector2Int _mazeSize;
    // size of tile in m
    public Vector2 _tileSize;
    public GameObject _floor;
    public GameObject _TilePrefab;

    private Vector2 _totalSize;

    private float _stepAnimationDuration = 0.01f;

    private TileController[,] tileControllers;

    private List<Vector2Int> _VisitedTiles = new List<Vector2Int>();


    public int seed = 42;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(seed);

        _totalSize = new Vector2(_mazeSize.x * _tileSize.x + 1, _mazeSize.y * _tileSize.y + 1);
        _floor.transform.localScale = new Vector3(_totalSize.x, 1, _totalSize.y);

        StartCoroutine(GenerateMaze());
    }


    private Vector3 ComputePosition(int i, int j)
    {
        return new Vector3(
            i * _tileSize.x - (_totalSize.x / 2 - _tileSize.x / 2) + 0.5f,
            0,
            j * _tileSize.y - (_totalSize.y / 2 - _tileSize.y / 2) + 0.5f);
    }

    public List<Vector2Int> getUnVisitedNeighbourg(int index)
    {
        List<Vector2Int> neighbourg = new List<Vector2Int>();

        Vector2Int currentPosition = _VisitedTiles[index];


        if (currentPosition.y != 0)
        {
            Vector2Int North = currentPosition + Vector2Int.up;

            if(!_VisitedTiles.Contains(North))
            {
                neighbourg.Add(North);
            }
        }

        if (currentPosition.y != _mazeSize.y - 1)
        {
            Vector2Int South = currentPosition + Vector2Int.down;
            if(!_VisitedTiles.Contains(South))
            {
                neighbourg.Add(South);
            }
        }

        if (currentPosition.x != _mazeSize.x - 1)
        {
            Vector2Int Est = currentPosition + Vector2Int.right;
            if(!_VisitedTiles.Contains(Est))
            {
                neighbourg.Add(Est);
            }
        }

        if (currentPosition.x != 0)
        {
            Vector2Int ouest = currentPosition + Vector2Int.left;
            if(!_VisitedTiles.Contains(ouest))
            {
                neighbourg.Add(ouest);
            }
        }

        return neighbourg;
    }

    private IEnumerator GenerateMaze()
    {
        InitMaze();

        Vector2Int firstTile = new Vector2Int(Random.Range(0, _mazeSize.x), Random.Range(0, _mazeSize.y));
        _VisitedTiles.Add(firstTile);
        int currentTileIndex = 0;

        while (_VisitedTiles.Count != _mazeSize.x * _mazeSize.y)
        {
            List<Vector2Int> neighbourg = getUnVisitedNeighbourg(currentTileIndex);

            if(neighbourg.Count > 0)
            {
                int indexInNeighbourg = Random.Range(0,neighbourg.Count);
                
                Vector2Int oldPosition = neighbourg[currentTileIndex];
                Vector2Int newPosition = neighbourg[indexInNeighbourg];

                if(newPosition.x > oldPosition.x)
                {
                    TileController oldControler = tileControllers[oldPosition.x,oldPosition.y];
                    TileController newControler = tileControllers[newPosition.x,newPosition.y];

                    
                }
                

            }
            else
            {

            }
        }


        yield return new WaitForSeconds(_stepAnimationDuration);
    }

    private void InitMaze()
    {
        tileControllers = new TileController[_mazeSize.x, _mazeSize.y];

        for (int i = 0; i < _mazeSize.x; i++)
        {
            for (int j = 0; j < _mazeSize.y; j++)
            {
                //yield return new WaitForSeconds(_stepAnimationDuration);
                GameObject tile = Instantiate(_TilePrefab, ComputePosition(i, j), Quaternion.identity, this.transform);

                TileController controller = tile.GetComponent<TileController>();
                controller.SetSize(_tileSize);
                controller.SetDirection(Directions.N, true);
                controller.SetDirection(Directions.S, true);
                controller.SetDirection(Directions.O, true);
                controller.SetDirection(Directions.E, true);

                tileControllers[i, j] = controller;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}