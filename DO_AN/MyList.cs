namespace DO_AN
{
    public class MyList<T>
    {
        private T[] _items;
        private int _count;
        private int _capacity;

        public MyList(int initialCapacity = 10)
        {
            _capacity = initialCapacity;
            _items = new T[_capacity];
            _count = 0;
        }

        public void Add(T item)
        {
            if (_count == _capacity)
            {
                _capacity *= 2;
                T[] newItems = new T[_capacity];
                for (int i = 0; i < _count; i++) newItems[i] = _items[i];
                _items = newItems;
            }
            _items[_count++] = item;
        }

        public int Count => _count;
        public T this[int index] => _items[index];
    }
}