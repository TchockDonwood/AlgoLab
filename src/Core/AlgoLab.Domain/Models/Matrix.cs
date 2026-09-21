namespace AlgoLab.Domain.Models
{
    public class Matrix
    {
        private int[,] data;
        public int Rows { get; }
        public int Cols { get; }

        public Matrix()
        {
            Rows = 0;
            Cols = 0;
            data = new int[0, 0];
        }

        public Matrix(int n, int m)
        {
            if (n < 0 || m < 0)
                throw new ArgumentException("Размеры матрицы должны быть положительными");

            Rows = n;
            Cols = m;
            data = new int[n, m];
        }

        public int this[int i, int j]
        {
            get => data[i, j];
            set => data[i, j] = value;
        }
    }

    public sealed record MatrixPair(Matrix Left, Matrix Right);
}
