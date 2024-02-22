using System.Collections.Generic;

namespace Endless.CustomObject
{
    public class Node<T>
    {
        internal T Value;
        internal Node<T> Next;

        public Node(T data)
        {
            Value = data;
            Next = null;
        }
    }

    public class CircleLinkedList<T>
    {
        private Node<T> _head;
        public Node<T> Head { get { return _head; } }

        private Node<T> _tail;
        public Node<T> Tail { get { return _tail; } }

        public CircleLinkedList()
        {
            _head = null;
            _tail = null;
        }

        public CircleLinkedList(List<T> list)
        {
            foreach (T data in list)
            {
                AddList(data);
            }
        }

        public void AddList(T data)
        {
            // 추가할 데이터 노드화
            Node<T> node = new Node<T>(data);

            if (_head == null)
            {
                _head = node;
                _tail = node;

                _tail.Next = node;
            }
            else
            {
                node.Next = _head;

                _tail.Next = node;
                _tail = node;
            }
        }
    }
}
