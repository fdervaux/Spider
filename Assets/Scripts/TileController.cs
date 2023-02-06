using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private Vector2 _size;
    public Transform _left;
    public Transform _right;
    public Transform _up;
    public Transform _down;

    private bool[] _directions = new bool[4];

    public void SetDirection(Directions dir, bool value)
    {
        _directions[(int)dir] = value;
        UpdateWall();
    }

    public void UpdateWall()
    {
        _up.gameObject.SetActive(_directions[(int)Directions.N]);
        _down.gameObject.SetActive(_directions[(int)Directions.S]);
        _right.gameObject.SetActive(_directions[(int)Directions.E]);
        _left.gameObject.SetActive(_directions[(int)Directions.O]);
    }

    public void SetSize(Vector2 size)
    {
        _size = size;
        UpdateSizeTile();
    }

    public void UpdateSizeTile()
    {
        Vector3 scale = _up.localScale;
        scale.x = _size.x + 1;
        _up.localScale = scale;
        _down.localScale = scale;

        scale = _left.localScale;
        scale.x = _size.y + 1;
        _left.localScale = scale;
        _right.localScale = scale;

        _left.localPosition = new Vector3(_size.x / 2, 1, 0);
        _right.localPosition = new Vector3(-_size.x / 2, 1, 0);
        _down.localPosition = new Vector3(0, 1, -_size.y / 2);
        _up.localPosition = new Vector3(0, 1, _size.y / 2);

    }
}
