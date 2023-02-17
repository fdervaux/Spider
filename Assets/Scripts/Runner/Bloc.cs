using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Bloc
{
    [SerializeField] private GameObject _gameobject;
    [SerializeField] private float _size;
     public GameObject Gameobject { get => _gameobject; set => _gameobject = value; }
    public float Size { get => _size; set => _size = value; }

    public Bloc Create(Vector3 positionSpawn, Transform parent)
    {
        Bloc bloc = new Bloc();
        bloc._gameobject = GameObject.Instantiate(_gameobject, positionSpawn, _gameobject.transform.rotation, parent);
        bloc._size = _size;
        return bloc;
    }

    public void Destroy()
    {
        GameObject.Destroy(this._gameobject);
    }
}