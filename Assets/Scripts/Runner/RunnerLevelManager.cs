
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerLevelManager : MonoBehaviour
{
    [SerializeField] private List<Bloc> _allPossibleBlocs = new List<Bloc>();
    private Queue<Bloc> _worldBlocs = new Queue<Bloc>();

    [SerializeField] private float _speed;

    [SerializeField] private float _destroyDistance;
    [SerializeField] private float _spawnDistance;

    private Bloc _lastBlockSpawn = null;

    [SerializeField] private int _seed = 42;
    [SerializeField] private bool randomSeed;

    [SerializeField] private List<GameObject> _elementsToMove = new List<GameObject>();

    [SerializeField] private SpiderLegControl _spiderLegController;
    [SerializeField] private RunnerController _runnerController;

    [SerializeField] private Bloc startBloc;




    private void Start()
    {
        if (randomSeed)
            _seed = (int)System.DateTime.Now.Ticks;

        Random.InitState(_seed);

    }

    private void FixedUpdate()
    {
        AddBlocAtStartOfLevelIfNecessary();
        DestroyBlocAfterDestroyDistance();
        MoveAllBlocksAndElementsInWorld();
    }

    private void AddBlocAtStartOfLevelIfNecessary()
    {
        if (_allPossibleBlocs.Count == 0)
            return;


        if (_lastBlockSpawn == null)
        {
            _lastBlockSpawn = startBloc.Create(Vector3.forward * _destroyDistance, this.transform);
            _worldBlocs.Enqueue(_lastBlockSpawn);
            return;
        }

        Vector3 endPosition = _lastBlockSpawn.Gameobject.transform.position + _lastBlockSpawn.Size * Vector3.forward;

        if (endPosition.z <= _spawnDistance)
        {
            int randomIndex = Random.Range(0, _allPossibleBlocs.Count);
            _lastBlockSpawn = _allPossibleBlocs[randomIndex].Create(endPosition, this.transform);
            _worldBlocs.Enqueue(_lastBlockSpawn);
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

    private void MoveAllBlocksAndElementsInWorld()
    {
        foreach (Bloc bloc in _worldBlocs)
        {
            bloc.Gameobject.GetComponent<Rigidbody>().velocity = -_speed * Vector3.forward;

            /*Vector3 blocPosition = bloc.Gameobject.transform.position;
            blocPosition.z -= _speed * Time.deltaTime;
            bloc.Gameobject.transform.position = blocPosition;*/
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
}
