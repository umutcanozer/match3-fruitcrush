using UnityEngine;

namespace GMatch3
{
    public class Fruit : MonoBehaviour, IMovable
    {
        [SerializeField] private int _fruitId;

        public int FruitID
        {
            get => _fruitId;
        }  

        public void Construct(int fruitId)
        {
            _fruitId = fruitId;
        }
  
    }

    public interface IMovable
    {
        int FruitID { get; }
        void Construct(int fruitId);
    }
}
