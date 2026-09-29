using AlgoLab.Application.Common.Interfaces;

namespace AlgoLab.Algorithms.Implementations
{
    public sealed class SmoothSort : IAlgorithm<int[]>
    {
        public string Code => "smooth-sort";
        public string Name => "Плавная сортировка";

        // Числа Леонардо: LP[0]=1, LP[1]=1, LP[i]=LP[i-1]+LP[i-2]+1
        // Предвычислены до LP[43] (~1.4·10⁹) — покрывает любой int[] на 64-битной машине.
        private static readonly int[] LP =
        {
            1, 1, 3, 5, 9, 15, 25, 41, 67, 109, 177, 287, 465, 753, 1219, 1973,
            3193, 5167, 8361, 13529, 21891, 35421, 57313, 92735, 150049,
            242785, 392835, 635621, 1028457, 1664079, 2692537, 4356617,
            7049155, 11405773, 18454929, 29860703, 48315633, 78176337,
            126491971, 204668309, 331160281, 535828591, 866988873
        };

        public void Execute(int[] a)
        {
            int n = a.Length;
            if (n <= 1) return;

            int head = 0;
            int p = 1;
            int pshift = 1;

            // Фаза 1: построение леса Леонардо
            while (head < n - 1)
            {
                if ((p & 3) == 3)
                {
                    Sift(a, pshift, head);
                    p >>= 2;
                    pshift += 2;
                }
                else
                {
                    if (LP[pshift - 1] >= n - head - 1)
                        Trinkle(a, p, pshift, head, false);
                    else
                        Sift(a, pshift, head);

                    if (pshift == 1)
                    {
                        p <<= 1;
                        pshift--;
                    }
                    else
                    {
                        p <<= (pshift - 1);
                        pshift = 1;
                    }
                }
                p |= 1;
                head++;
            }

            Trinkle(a, p, pshift, head, false);

            // Фаза 2: разбор леса — извлечение максимумов по одному
            while (pshift != 1 || p != 1)
            {
                if (pshift <= 1)
                {
                    int trail = p & ~1;
                    pshift = Ctz(trail);
                    p >>= pshift + 1;
                }
                else
                {
                    p <<= 2;
                    p ^= 7;
                    pshift -= 2;

                    Trinkle(a, p >> 1, pshift + 1, head - LP[pshift] - 1, true);
                    Trinkle(a, p, pshift, head - 1, true);
                }
                head--;
            }
        }

        // Просеивание внутри одной кучи Леонардо: поднимаем значение к корню,
        // сохраняя инвариант max-heap.
        private static void Sift(int[] a, int pshift, int head)
        {
            int val = a[head];
            while (pshift > 1)
            {
                int rt = head - 1;
                int lf = head - 1 - LP[pshift - 2];

                if (val >= a[lf] && val >= a[rt]) break;

                if (a[lf] >= a[rt])
                {
                    a[head] = a[lf];
                    head = lf;
                    pshift -= 1;
                }
                else
                {
                    a[head] = a[rt];
                    head = rt;
                    pshift -= 2;
                }
            }
            a[head] = val;
        }

        // Просеивание по всему лесу: восстанавливает свойство max-heap
        // для корня перед предыдущими кучами.
        private static void Trinkle(int[] a, int p, int pshift, int head, bool isTrusty)
        {
            int val = a[head];
            while (p != 1)
            {
                int stepson = head - LP[pshift];
                if (a[stepson] <= val) break;

                if (!isTrusty && pshift > 1)
                {
                    int rt = head - 1;
                    int lf = head - 1 - LP[pshift - 2];
                    if (a[rt] >= a[stepson] || a[lf] >= a[stepson]) break;
                }

                a[head] = a[stepson];
                head = stepson;

                int trail = p & (p - 1);
                pshift = Ctz(p ^ trail);
                p = trail;
                isTrusty = false;
            }
            if (!isTrusty) a[head] = val;
        }

        private static int Ctz(int x)
        {
            if (x == 0) return 32;
            int count = 0;
            while ((x & 1) == 0) { x >>= 1; count++; }
            return count;
        }
    }
}