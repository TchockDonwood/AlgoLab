using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class SmoothSort : IAlgorithm<int[]>
    {
        public string Code => "smooth-sort";
        public string Name => "Smooth Sort";

        public void Execute(int[] arr)
        {
            int length = arr.Length;

            int lastUnsortedIndex = length - 1;
            int heapEndIndex = lastUnsortedIndex;
            int treeOrder = 0;

            // Построение кучи Леонардо
            while (lastUnsortedIndex > 0)
            {
                if ((treeOrder & 0x03) == 0)
                {
                    Heapify(arr, treeOrder, heapEndIndex);
                }

                if (Leonardo(treeOrder) == lastUnsortedIndex)
                {
                    treeOrder++;
                }
                else
                {
                    treeOrder--;
                    heapEndIndex -= Leonardo(treeOrder);
                    Heapify(arr, treeOrder, heapEndIndex);
                    heapEndIndex = treeOrder - 1;
                    treeOrder++;
                }

                int temp = arr[0];
                arr[0] = arr[lastUnsortedIndex];
                arr[lastUnsortedIndex] = temp;
                lastUnsortedIndex--;
            }

            // Преобразование кучи Леонардо обратно в отсортированный массив
            for (int currentIndex = 0; currentIndex < length - 1; currentIndex++)
            {
                int shiftIndex = currentIndex + 1;
                while (shiftIndex > 0 && arr[shiftIndex] < arr[shiftIndex - 1])
                {
                    int temp = arr[shiftIndex];
                    arr[shiftIndex] = arr[shiftIndex - 1];
                    arr[shiftIndex - 1] = temp;
                    shiftIndex--;
                }
            }
        }

        private static int Leonardo(int order)
        {
            if (order < 2)
            {
                return 1;
            }
            return Leonardo(order - 1) + Leonardo(order - 2) + 1;
        }

        private static void Heapify(int[] arr, int order, int endIndex)
        {
            int gap = order;
            int offset = 0;
            int count = 0;

            while (count < endIndex - order + 1)
            {
                if ((count & 0xAAAAAAAA) == 0xAAAAAAAA)
                {
                    offset += gap;
                    gap >>= 1;
                }
                else
                {
                    gap += offset;
                    offset >>= 1;
                }
                count++;
            }

            while (gap > 0)
            {
                offset >>= 1;
                int currentIndex = gap + offset;

                while (currentIndex < endIndex)
                {
                    if (arr[currentIndex] > arr[currentIndex - gap])
                    {
                        break;
                    }

                    int temp = arr[currentIndex];
                    arr[currentIndex] = arr[currentIndex - gap];
                    arr[currentIndex - gap] = temp;
                    currentIndex += gap;
                }

                gap = offset;
            }
        }
    }
}