using System.Collections.Generic;

namespace PreFinals_Project.DoublyLinkedList_Class
{
    public interface ILinkedList<T>
    {
        Node<T> Head { get; }
        Node<T> Tail { get; }
        int Count { get; }
        void AddToHead(T data);
        void AddToTail(T data);
        void RemoveFromHead();
        void RemoveFromTail();
        
        Node<T> Search(T data, IComparer<T> comparer = null);
        int SearchForPosition(T data, IComparer<T> comparer = null);
        void InsertAt(T data, int position);
        IEnumerator<T> GetEnumerator();
        void SwapHeadAndTail();
        void MoveHeadToRight(int moves);
        void MoveHeadToLeft(int moves);
        void MoveTailToRight(int moves);
        void MoveTailToLeft(int moves);
        void Reverse();
        bool HasSameContents(ILinkedList<T> list, IComparer<T> comparer);


    }

}
