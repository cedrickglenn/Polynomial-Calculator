namespace PreFinals_Project.DoublyLinkedList_Class
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Prev { get; internal set; }
        public Node<T> Next { get; internal set; }

        public Node(T data)
        {
            Data = data;
        }

        public Node(T data, Node<T> prev, Node<T> next)
        {
            Data = data;
            Prev = prev;
            Next = next;
        }
    }
}