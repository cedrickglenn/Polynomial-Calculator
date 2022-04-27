using System;
using System.Collections.Generic;

namespace PreFinals_Project.DoublyLinkedList_Class
{
    public class DoublyLinkedList<T> : ILinkedList<T>
    {
        public Node<T> Head { get; private set; }
        public Node<T> Tail { get; private set; }

        public int Count { get; private set; }


        public void AddToHead(T data)
        {
            var x = new Node<T>(data);
            Count++;
            if (Head == null)
            {
                Head = x;
                Tail = x;
                return;
            }

            var tmp = Head;
            Head = x;
            x.Next = tmp;
            tmp.Prev = Head;
        }



        public void AddToTail(T data)
        {
            var x = new Node<T>(data);
            Count++;
            if (Head == null)
            {
                Head = x;
                Tail = x;
                return;
            }

            Tail.Next = x;
            x.Prev = Tail;
            Tail = x;
        }

        public void RemoveFromHead()
        {
            if (Head == null)
                throw new Exception(message: "LinkedList is empty. ");
            Count--;

            if (Head == Tail)
            {
                Head = null;
                Tail = null;
                return;
            }

            Head = Head.Next;
            Head.Prev = null;

        }

        public void RemoveFromTail()
        {
            if (Head == null)
                throw new Exception(message: "LinkedList is empty. ");
            Count--;

            if (Head == Tail)
            {
                Head = null;
                Tail = null;
                return;
            }


            var tmp = Head;
            while (tmp.Next != Tail)
            {
                tmp = tmp.Next;
            }
            Tail = tmp;
            Tail.Next = null;
            tmp = null;
        }

        public Node<T> Search(T dataToSearch, IComparer<T> comparer = null)
        {
            if (comparer == null)
                comparer = Comparer<T>.Default;
            if (Head == null)
                return null;

            var tmp = Head;

            while (tmp.Next != Tail)
            {
                if (comparer.Compare(tmp.Data , dataToSearch) == 0)
                {
                    return tmp;
                }
                tmp = tmp.Next;
            }
            if (comparer.Compare(Tail.Data, dataToSearch) == 0)
                return Tail;
            return null;
        }

        public int SearchForPosition(T dataToSearch, IComparer<T> comparer = null)
        {
            if (comparer == null) comparer = Comparer<T>.Default;

            if (Head == null)
            {
                return -1;
            }

            int position = 0;
            var tmp = Head;
            while (tmp.Next != Tail)
            {
                position++;
                if (comparer.Compare(tmp.Data, dataToSearch) == 0)
                {
                    return position;
                }
                tmp = tmp.Next;
            }
            if (comparer.Compare(Tail.Data, dataToSearch) == 0)
            {
                position = position + 1;
                return position;
            }
            return -1;
        }

        public void InsertAt(T data, int position)
        {
            if (position < 0)
                throw new InvalidOperationException(message: "Position must be positive.");
            if (position > Count)
                throw new IndexOutOfRangeException("Position is greater than Count");
            if (position == 0)
            {
                AddToHead(data);
                return;
            }
            if (position == Count)
            {
                AddToTail(data);
                return;
            }
            if (position > 0)
            {
                var newNode = new Node<T>(data);
                var prev = Head;
                var next = Head;

                //get prev
                var pos1 = 0;
                var tmp = Head;
                while (tmp != Tail)
                {
                    if (pos1 == position - 1)
                        prev = tmp;
                    tmp = tmp.Next;
                    pos1++;
                }

                //get next
                next = prev.Next;

                prev.Next = newNode;
                newNode.Prev = prev;
                newNode.Next = next;
                next.Prev = newNode;
            }
            Count++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (Head == null) yield break;

            var tmp = Head;
            while (tmp != Tail)
            {
                yield return tmp.Data;
                tmp = tmp.Next;
            }
            yield return Tail.Data;
        }

        public void SwapHeadAndTail()
        {
            //if (Head == null || Tail == null)
            //    throw new Exception(message: "LinkedList is empty. ");
            //var tempA = Head;
            //var tempB = Tail;

            //Head = tempB;
            //Tail = tempA;

            //Tail.Next = null;
            //Head.Prev = null;

            if (Head == null) return;
            if (Head == Tail) return;

            var headData = Head.Data;
            var tailData = Tail.Data;

            RemoveFromHead();
            RemoveFromTail();

            AddToTail(headData);
            AddToHead(tailData);

        }

        public void MoveHeadToRight(int moves)
        {
            if (Head == null)
                throw new Exception(message: "LinkedList is empty. ");
            if(moves > Count)
                throw new Exception("Move cannot be greater than the list size.");

            var tmp = Head.Next;

            for (int i = 0; i < moves; i++)
            {
                Head = Head.Next;
                Head.Prev = tmp;

                Head.Prev.Prev = null;
            }

        }

        public void MoveHeadToLeft(int moves)
        {
            Console.WriteLine("Move is impossible.");
        }

        public void MoveTailToRight(int moves)
        {
            Console.WriteLine("Move is impossible.");
        }

        public void MoveTailToLeft(int moves)
        {
            if (Tail == null)
                throw new Exception(message: "LinkedList is empty. ");
            if (moves > Count)
                throw new Exception("Move cannot be greater than the list size.");

            var tmp = Tail.Prev;

            for (int i = 0; i < moves; i++)
            {
                Tail = Tail.Prev;
                Tail.Next = tmp;

                Tail.Next.Next = null;
            }
        }

        public bool HasSameContents(ILinkedList<T> list, IComparer<T> comparer = null)
        {

            if (Count != list.Count) return false;
            if (Count == 0 && list.Count == 0) return true;
            if (comparer == null) comparer = Comparer<T>.Default;


            var tmp = Head;
            var otherTmp = Head;
            while (tmp != Tail)
            {
                var comparisonResult = comparer.Compare(tmp.Data, otherTmp.Data);
                if (comparisonResult != 0) return false;
                tmp = tmp.Next;
                otherTmp = otherTmp.Next;
            }

            if (comparer.Compare(tmp.Data,list.Tail.Data) != 0)
            {
                return false;
            }
            return true;
        }

        public void Reverse()
        {
            if (Head == Tail) return;
            var headData = Head.Data;
            var tmpHead = Head;
            var tmp = Tail;

            Head = Tail = null;

            //Reset Count to zero
            Count = 0;
            while (tmp != tmpHead)
            {
                AddToTail(tmp.Data);
                tmp = tmp.Prev;
            }
            AddToTail(headData);
        }

    }

}
