using UnityEngine;

namespace GMatch3
{
    public class Tile : MonoBehaviour
    {
        private Vector2 position;
        private IMovable _fruit;
        

        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        public IMovable Fruit
        {
            get => _fruit;
        }

        public void Construct(Vector2 position)
        {
            this.position = Position;
        }
        
        public void AssignFruit(IMovable fruit = null)
        {
            _fruit = fruit;
        }

    }
}
