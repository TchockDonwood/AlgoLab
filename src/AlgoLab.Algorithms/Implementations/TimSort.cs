
using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class TimSort : IAlgorithm<double[]>
    {
        public string Code => "tim-sort";

        const int RUN = 32;

        public void Execute(double[] input)
        {
            int n = input.Length;

            // Сортируем маленькие куски методом вставок
            for (int i = 0; i < n; i += RUN)
            {
                int right = i + RUN - 1;
                if (right >= n) right = n - 1;
                InsertionSort(input, i, right);
            }

            // Сливаем отсортированные куски размера RUN, затем 2*RUN, 4*RUN и т.д.
            for (int size = RUN; size < n; size = 2 * size)
            {
                for (int left = 0; left < n; left += 2 * size)
                {
                    int mid = left + size - 1;
                    int right = left + 2 * size - 1;
                    if (mid >= n) mid = n - 1;
                    if (right >= n) right = n - 1;

                    if (mid < right)
                    {
                        Merge(input, left, mid, right);
                    }
                }
            }
        }

        // Сортировка вставками для Timsort
        static void InsertionSort(double[] input, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                double temp = input[i];
                int j = i - 1;
                while (j >= left && input[j] > temp)
                {
                    input[j + 1] = input[j];
                    j--;
                }
                input[j + 1] = temp;
            }
        }

        static void Merge(double[] input, int l, int m, int r)
        {
            int len1 = m - l + 1, len2 = r - m;
            double[] left = new double[len1];
            double[] right = new double[len2];

            Array.Copy(input, l, left, 0, len1);
            Array.Copy(input, m + 1, right, 0, len2);

            int i = 0, j = 0, k = l;

            while (i < len1 && j < len2)
            {
                if (left[i] <= right[j])
                {
                    input[k++] = left[i++];
                }
                else
                {
                    input[k++] = right[j++];
                }
            }

            while (i < len1)
                input[k++] = left[i++];

            while (j < len2)
                input[k++] = right[j++];
        }
    }
}

