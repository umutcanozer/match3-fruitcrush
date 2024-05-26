using GMatch3.Scripts;
using System;
using System.Collections;
using UnityEngine;

namespace GMatch3
{
    public class MoveFruit : MonoBehaviour
    {
        private const float MoveCheckDelay = 0.4f;
        private const float LerpScala = 0.6f;
        [SerializeField] private int _targetX;
        [SerializeField] private int _targetY;
        [SerializeField] private int _previousColumn;
        [SerializeField] private int _previousRow;
        [SerializeField] private float _swipeAngle = 0.0f;
        [SerializeField] private float _swipeResist = 1.0f;
        public int Col { get; set; } 
        public int Row { get; set; }
        public bool IsMatched { get; set; } = false;

        private GameObject _otherFruit;
        private Vector2 _firstTouch;
        private Vector2 _lastTouch;
        private Vector2 _tempPosition;

        private IMovable _movable;

        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = GridManager.instance.Camera;
        }

        private void Start()
        {
            _movable = GetComponent<IMovable>();
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentState == GameManager.GameState.GameOver) return;
            FindMatch();
            ChangeFruitPosition();
        }

        private void OnMouseDown()
        {
            if (GridManager.instance.currentState == MoveState.move)
                _firstTouch = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnMouseUp()
        {
            if (GridManager.instance.currentState == MoveState.move)
            {
                _lastTouch = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                CalculateAngle();
            }
        }

        public void SetCol(int col)
        {
            Col = col;
        }

        public void SetRow(int row)
        {
            Row = row;
        }

        private void ChangeFruitPosition()
        {
            _targetX = Col;
            _targetY = Row;
            if (Mathf.Abs(_targetX - transform.position.x) > .1)
            {
                _tempPosition = new Vector2(_targetX, transform.position.y);
                transform.position = Vector2.Lerp(transform.position, _tempPosition, LerpScala);
                if (GridManager.instance.AllFruits[Col, Row] != this.gameObject)
                {
                    GridManager.instance.AllFruits[Col, Row] = this.gameObject;
                }
            }
            else
            {
                _tempPosition = new Vector2(_targetX, transform.position.y);
                transform.position = _tempPosition;
                
            }
            if (Mathf.Abs(_targetY - transform.position.y) > .1)
            {
                _tempPosition = new Vector2(transform.position.x, _targetY);
                transform.position = Vector2.Lerp(transform.position, _tempPosition, LerpScala);
                if (GridManager.instance.AllFruits[Col, Row] != this.gameObject)
                {
                    GridManager.instance.AllFruits[Col, Row] = this.gameObject;
                }
            }
            else
            {
                _tempPosition = new Vector2(transform.position.x, _targetY);
                transform.position = _tempPosition;
            }
        }

        public IEnumerator CheckMove()
        {
            yield return new WaitForSeconds(MoveCheckDelay);
            if (_otherFruit != null)
            {
                if (!IsMatched && !_otherFruit.GetComponent<MoveFruit>().IsMatched)
                {
                    _otherFruit.GetComponent<MoveFruit>().Row = Row;
                    _otherFruit.GetComponent<MoveFruit>().Col = Col;
                    Row = _previousRow;
                    Col = _previousColumn;
                    yield return new WaitForSeconds(0.5f);
                    GridManager.instance.currentState = MoveState.move;
                }
                else
                {
                    GridManager.instance.DestroyMatchedFruits();
                }
                _otherFruit = null;
            }     
              
        }

        void CalculateAngle()
        {
            float diffX = Mathf.Abs(_lastTouch.x - _firstTouch.x);
            float diffY = Mathf.Abs(_lastTouch.y - _firstTouch.y);
            if (diffY > _swipeResist || diffX > _swipeResist)
            {
                _swipeAngle = Mathf.Atan2(_lastTouch.y - _firstTouch.y, _lastTouch.x - _firstTouch.x) * 180 / Mathf.PI;
                MoveFruits();
                GridManager.instance.currentState = MoveState.wait;
            }
            else
            {
                GridManager.instance.currentState = MoveState.move;
            }           
        }

        void MoveFruits()
        {
            if (_swipeAngle > -45 && _swipeAngle <= 45 && Col < GridManager.instance.GridSizeX-1)
            {
                //right
                _otherFruit = GridManager.instance.AllFruits[Col + 1, Row];
                _previousColumn = Col;
                _previousRow = Row;
                _otherFruit.GetComponent<MoveFruit>().Col -= 1;
                Col += 1;
            }
            else if (_swipeAngle > 45 && _swipeAngle <= 135 && Row < GridManager.instance.GridSizeY-1)
            {
                //up
                _otherFruit = GridManager.instance.AllFruits[Col, Row + 1];
                _previousColumn = Col;
                _previousRow = Row;
                _otherFruit.GetComponent<MoveFruit>().Row -= 1;
                Row += 1;
            }
            else if ((_swipeAngle > 135 || _swipeAngle <= -135) && Col > 0)
            {
                //left
                _otherFruit = GridManager.instance.AllFruits[Col - 1, Row];
                _previousColumn = Col;
                _previousRow = Row;
                _otherFruit.GetComponent<MoveFruit>().Col += 1;
                Col -= 1;
            }
            else if (_swipeAngle < -45 && _swipeAngle >= -135 && Row > 0)
            {
                //down
                _otherFruit = GridManager.instance.AllFruits[Col, Row - 1];
                _previousColumn = Col;
                _previousRow = Row;
                _otherFruit.GetComponent<MoveFruit>().Row += 1;
                Row -= 1;
            }

            StartCoroutine(CheckMove());
        }

        void FindMatch()
        {
            if(Col > 0 && Col < GridManager.instance.GridSizeX - 1)
            {
                GameObject leftDot = GridManager.instance.AllFruits[Col - 1, Row];
                GameObject rightDot = GridManager.instance.AllFruits[Col + 1, Row];

                if (leftDot != null && rightDot != null)
                {
                    IMovable leftDotI = leftDot.GetComponent<IMovable>();
                    IMovable rightDotI = rightDot.GetComponent<IMovable>();
                    if (leftDotI.FruitID == _movable.FruitID && rightDotI.FruitID == _movable.FruitID)
                    {
                        leftDot.GetComponent<MoveFruit>().IsMatched = true;
                        rightDot.GetComponent<MoveFruit>().IsMatched = true;
                        IsMatched = true;
                    }
                }
            }
            if (Row > 0 && Row < GridManager.instance.GridSizeY - 1)
            {
                GameObject upDot = GridManager.instance.AllFruits[Col, Row + 1];
                GameObject downDot = GridManager.instance.AllFruits[Col, Row - 1];          

                if (upDot != null && downDot != null)
                {
                    IMovable upDotI = upDot.GetComponent<IMovable>();
                    IMovable downDotI = downDot.GetComponent<IMovable>();
                    if (upDotI.FruitID == _movable.FruitID && downDotI.FruitID == _movable.FruitID)
                    {
                        upDot.GetComponent<MoveFruit>().IsMatched = true;
                        downDot.GetComponent<MoveFruit>().IsMatched = true;
                        IsMatched = true;
                    }
                }
            }
        }
    }
}
