
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerLevelManager : MonoBehaviour
{
    [SerializeField] private List<Bloc> _allPossibleBlocs = new List<Bloc>();
    [SerializeField] private float _speed;
    [SerializeField] private float _destroyDistance;
    [SerializeField] private float _spawnDistance;    
    [SerializeField] private int _seed = 42;
    [SerializeField] private bool _randomSeed;
    [SerializeField] private List<GameObject> _elementsToMove = new List<GameObject>();
    [SerializeField] private SpiderLegControl _spiderLegController;
    [SerializeField] private RunnerController _runnerController;
    [SerializeField] private Bloc _startBloc;

    private Queue<Bloc> _worldBlocs = new Queue<Bloc>();
    private Bloc _lastBlocSpawn = null;

    private void Start()
    {
        InitialiseRandom();
    }

    

    private void FixedUpdate()
    {
        AddBlocAtStartOfLevelIfNecessary();
        DestroyBlocAfterDestroyDistance();
        MoveAllBlocsAndElementsInWorld();
    }

    private void AddBlocAtStartOfLevelIfNecessary()
    {
        if (_allPossibleBlocs.Count == 0)
            return;


        if (_lastBlocSpawn == null)
        {
            _lastBlocSpawn = _startBloc.Create(Vector3.forward * _destroyDistance, this.transform);
            _worldBlocs.Enqueue(_lastBlocSpawn);
            return;
        }

        Vector3 endPosition = _lastBlocSpawn.Gameobject.transform.position + _lastBlocSpawn.Size * Vector3.forward;

        if (endPosition.z <= _spawnDistance)
        {
            int randomIndex = Random.Range(0, _allPossibleBlocs.Count);
            _lastBlocSpawn = _allPossibleBlocs[randomIndex].Create(endPosition, this.transform);
            _worldBlocs.Enqueue(_lastBlocSpawn);
        }
    }

    private void DestroyBlocAfterDestroyDistance()
    {
        Bloc firstBloc = _worldBlocs.Peek();
        if (firstBloc.Gameobject.transform.position.z + firstBloc.Size < _destroyDistance)
        {
            _worldBlocs.Dequeue().Destroy();
        }
    }

    private void MoveAllBlocsAndElementsInWorld()
    {
        foreach (Bloc bloc in _worldBlocs)
        {
            bloc.Gameobject.GetComponent<Rigidbody>().velocity = -_speed * Vector3.forward;
        }

        foreach (GameObject obj in _elementsToMove)
        {
            Vector3 position = obj.transform.position;
            position.z -= _speed * Time.deltaTime;
            obj.transform.position = position;
        }

        if (!_runnerController.Death)
            _spiderLegController.WorldVelocity = Vector3.forward * _speed;
        else
            _spiderLegController.WorldVelocity = Vector3.zero;
    }
    private void InitialiseRandom()
    {
        if (_randomSeed)
            _seed = (int)System.DateTime.Now.Ticks;

        Random.InitState(_seed);
    }
}
