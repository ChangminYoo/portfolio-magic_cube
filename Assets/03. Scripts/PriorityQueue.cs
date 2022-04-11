using System;
using System.Collections.Generic;

class PriorityQueue<T> where T : IComparable<T>
{
    List<T> heap = new List<T>();

    public int Count { get { return heap.Count; } }

    public void Push(T data)
    {
        heap.Add(data);
        int current = heap.Count - 1;

        while (current > 0)
        {
            int next = (current - 1) / 2;

            // next값이 현재값보다 크면 종료
            if (heap[current].CompareTo(heap[next]) < 0)
                break;

            T temp = heap[current];
            heap[current] = heap[next];
            heap[next] = temp;

            current = next;
        }
    }

    public T Pop()
    {
        T ret = heap[0];

        int last = heap.Count - 1;
        heap[0] = heap[last];
        heap.RemoveAt(last);
        last--;

        int current = 0;
        while (true)
        {
            int left = 2 * current + 1;
            int right = 2 * current + 2;

            int next = current;
            if (left <= last && heap[next].CompareTo(heap[left]) < 0)
            {
                next = left;
            }
            if (right <= last && heap[next].CompareTo(heap[right]) < 0)
            {
                next = right;
            }

            // 왼쪽/오른쪽 모두 현재값보다 작으면 종료
            if (next == current)
                break;

            T temp = heap[current];
            heap[current] = heap[next];
            heap[next] = temp;

            current = next;
        }
        return ret;
    }
}
