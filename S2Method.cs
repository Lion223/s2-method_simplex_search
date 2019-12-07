using System;
using System.Linq;

namespace SearchMethods
{
    public static class S2Method
    {
        // max value from appropriate array index
        private static int Max(double f1, double f2, double f3)
        {
            if (f1 > f2)
            {
                if (f1 > f3)
                    return 0;
                else return 2;
            }
            else if (f1 < f2)
            {
                if (f2 > f3)
                    return 1;
                else return 2;
            }
            return -1;
        }

        private static int Max(double f2, double f3, int i, int y)
        {
            if (f2 > f3)
                return i;
            return y;
        }

        // check for two points equality
        private static bool Equal(double[,] x_cur, int i, double[,] chosen)
        {
            if (x_cur[i, 0] == chosen[0, 0] && x_cur[i, 1] == chosen[0, 1])
                return true;
            return false;
        }

        // target function
        private static double Func(double x, double y) =>
            (x - 14) * (x - 14) + (y + 7) * (y + 7) + 0.34 * x * y;

        // target function with restrictions
        private static double Func(double x, double y, double penalty)
        {
            double restr1 = x + y - 14;
            double restr2 = x + y + 14;
            return (x - 14) * (x - 14) + (y + 7) * (y + 7) + 0.34
                * x * y + (penalty * (restr1 * restr1 + 1 / restr2));
        }

        // main method
        public static void FindMin(double x_start, double y_start, double _e)
        {
            // variable initiation - scale factor and space dimension
            double a = 2;
            int n = 2;

            // variables for iteration count around one point (cycle move)
            int x0 = 0;
            int x1 = 0;
            int x2 = 0;

            // iteration limit for point
            int m = Convert.ToInt32((1.65 * n) + 0.05 * Math.Pow(n, 2));

            // epsilon
            double eps = _e;

            // arrays for current, old, temp and new points
            double[,] x_cur = new double[3, 2];
            double[,] x_old = new double[3, 2];
            double[,] temp = new double[1, 2];
            double[,] chosen = new double[1, 2];

            // arrays for function values
            double[] ans_cur = new double[3];
            double[] ans_old = new double[3];

            // starting point x0
            x_cur[0, 0] = x_start;
            x_cur[0, 1] = y_start;

            // growth
            double b1 = ((Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2))) * a;
            double b2 = ((Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2))) * a;

            // x1, x2
            x_cur[1, 0] = x_cur[0, 0] + b2;
            x_cur[1, 1] = x_cur[0, 1] + b1;
            x_cur[2, 0] = x_cur[0, 0] + b1;
            x_cur[2, 1] = x_cur[0, 1] + b2;


            ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
            ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
            ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

            Console.WriteLine($"x[0] = {x_cur[0, 0]}; y[0] = {x_cur[0, 1]}; f[0] = {ans_cur[0]}");
            Console.WriteLine($"x[1] = {x_cur[1, 0]}; y[1] = {x_cur[1, 1]}; f[1] = {ans_cur[1]}");
            Console.WriteLine($"x[2] = {x_cur[2, 0]}; y[2] = {x_cur[2, 1]}; f[2] = {ans_cur[2]}");

            // point iterator
            int i = 3;

            // loop iterator
            int y = 0;

            // variables for simplex size
            double mid = 0;
            double path = 0;

            // do-while alternative
            bool first = true;

            while (path > eps || first)
            {
                first = false;

                // saving current point locations as the old ones
                x_old[0, 0] = x_cur[0, 0];
                x_old[0, 1] = x_cur[0, 1];

                x_old[1, 0] = x_cur[1, 0];
                x_old[1, 1] = x_cur[1, 1];

                x_old[2, 0] = x_cur[2, 0];
                x_old[2, 1] = x_cur[2, 1];

                ans_old[0] = ans_cur[0];
                ans_old[1] = ans_cur[1];
                ans_old[2] = ans_cur[2];

                switch (Max(ans_cur[0], ans_cur[1], ans_cur[2]))
                {
                    // if point 0 has the biggest value out of three
                    case 0:
                        // if point with the biggest value hadn't been received in the previous iteration
                        if (!Equal(x_cur, 0, chosen))
                        {
                            temp[0, 0] = x_cur[0, 0];
                            temp[0, 1] = x_cur[0, 1];

                            x_cur[0, 0] = x_cur[1, 0] + x_cur[2, 0] - temp[0, 0];
                            x_cur[0, 1] = x_cur[1, 1] + x_cur[2, 1] - temp[0, 1];

                            ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                            ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                            ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                            chosen[0, 0] = x_cur[0, 0];
                            chosen[0, 1] = x_cur[0, 1];

                            Console.WriteLine($"x[{i}] = {x_cur[0, 0]}; y[{i}] = {x_cur[0, 1]}; f[{i}] = {ans_cur[0]}");
                            break;
                        }
                        // otherwise selecting a point with the biggest function value from the other two
                        else
                        {
                            switch (Max(ans_cur[1], ans_cur[2], 1, 2))
                            {
                                case 1:
                                    temp[0, 0] = x_cur[1, 0];
                                    temp[0, 1] = x_cur[1, 1];

                                    x_cur[1, 0] = x_cur[0, 0] + x_cur[2, 0] - temp[0, 0];
                                    x_cur[1, 1] = x_cur[0, 1] + x_cur[2, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                                    chosen[0, 0] = x_cur[1, 0];
                                    chosen[0, 1] = x_cur[1, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[1, 0]}; y[{i}] = {x_cur[1, 1]}; f[{i}] = {ans_cur[1]}");
                                    break;

                                case 2:
                                    temp[0, 0] = x_cur[2, 0];
                                    temp[0, 1] = x_cur[2, 1];

                                    x_cur[2, 0] = x_cur[0, 0] + x_cur[1, 0] - temp[0, 0];
                                    x_cur[2, 1] = x_cur[0, 1] + x_cur[1, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                                    chosen[0, 0] = x_cur[2, 0];
                                    chosen[0, 1] = x_cur[2, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[2, 0]}; y[{i}] = {x_cur[2, 1]}; f[{i}] = {ans_cur[2]}");
                                    break;
                            }
                        }
                        break;

                    // if point 1 has the biggest value out of three
                    case 1:
                        // if point with the biggest value hadn't been received in the previous iteration
                        if (!Equal(x_cur, 1, chosen))
                        {
                            temp[0, 0] = x_cur[1, 0];
                            temp[0, 1] = x_cur[1, 1];

                            x_cur[1, 0] = x_cur[0, 0] + x_cur[2, 0] - temp[0, 0];
                            x_cur[1, 1] = x_cur[0, 1] + x_cur[2, 1] - temp[0, 1];

                            ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                            ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                            ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                            chosen[0, 0] = x_cur[1, 0];
                            chosen[0, 1] = x_cur[1, 1];

                            Console.WriteLine($"x[{i}] = {x_cur[1, 0]}; y[{i}] = {x_cur[1, 1]}; f[{i}] = {ans_cur[1]}");
                            break;
                        }
                        // otherwise selecting a point with the biggest function value from the other two
                        else
                        {
                            switch (Max(ans_cur[0], ans_cur[2], 0, 2))
                            {
                                case 0:
                                    temp[0, 0] = x_cur[0, 0];
                                    temp[0, 1] = x_cur[0, 1];

                                    x_cur[0, 0] = x_cur[1, 0] + x_cur[2, 0] - temp[0, 0];
                                    x_cur[0, 1] = x_cur[1, 1] + x_cur[2, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                                    chosen[0, 0] = x_cur[0, 0];
                                    chosen[0, 1] = x_cur[0, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[0, 0]}; y[{i}] = {x_cur[0, 1]}; f[{i}] = {ans_cur[0]}");
                                    break;

                                case 2:
                                    temp[0, 0] = x_cur[2, 0];
                                    temp[0, 1] = x_cur[2, 1];

                                    x_cur[2, 0] = x_cur[0, 0] + x_cur[1, 0] - temp[0, 0];
                                    x_cur[2, 1] = x_cur[0, 1] + x_cur[1, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                                    chosen[0, 0] = x_cur[2, 0];
                                    chosen[0, 1] = x_cur[2, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[2, 0]}; y[{i}] = {x_cur[2, 1]}; f[{i}] = {ans_cur[2]}");
                                    break;
                            }
                        }
                        break;

                    // if point 2 has the biggest value out of three
                    case 2:
                        // if point with the biggest value hadn't been received in the previous iteration
                        if (!Equal(x_cur, 2, chosen))
                        {
                            temp[0, 0] = x_cur[2, 0];
                            temp[0, 1] = x_cur[2, 1];

                            x_cur[2, 0] = x_cur[0, 0] + x_cur[1, 0] - temp[0, 0];
                            x_cur[2, 1] = x_cur[0, 1] + x_cur[1, 1] - temp[0, 1];

                            ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                            ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                            ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                            chosen[0, 0] = x_cur[2, 0];
                            chosen[0, 1] = x_cur[2, 1];

                            Console.WriteLine($"x[{i}] = {x_cur[2, 0]}; y[{i}] = {x_cur[2, 1]}; f[{i}] = {ans_cur[2]}");
                            break;
                        }
                        // otherwise selecting a point with the biggest function value from the other two
                        else
                        {
                            switch (Max(ans_cur[0], ans_cur[1], 0, 1))
                            {
                                case 1:
                                    temp[0, 0] = x_cur[0, 0];
                                    temp[0, 1] = x_cur[0, 1];

                                    x_cur[0, 0] = x_cur[1, 0] + x_cur[2, 0] - temp[0, 0];
                                    x_cur[0, 1] = x_cur[1, 1] + x_cur[2, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                                    chosen[0, 0] = x_cur[0, 0];
                                    chosen[0, 1] = x_cur[0, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[1, 0]}; y[{i}] = {x_cur[1, 1]}; f[{i}] = {ans_cur[1]}");
                                    break;

                                case 0:
                                    temp[0, 0] = x_cur[0, 0];
                                    temp[0, 1] = x_cur[0, 1];

                                    x_cur[0, 0] = x_cur[1, 0] + x_cur[2, 0] - temp[0, 0];
                                    x_cur[0, 1] = x_cur[1, 1] + x_cur[2, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                                    chosen[0, 0] = x_cur[0, 0];
                                    chosen[0, 1] = x_cur[0, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[0, 0]}; y[{i}] = {x_cur[0, 1]}; f[{i}] = {ans_cur[0]}");
                                    break;
                            }
                        }
                        break;
                }

                // if the point that was in the previous iteration occurs in the current - incrementing a counter
                x0 = (x_cur[0, 0] == x_old[0, 0] && x_cur[0, 1] == x_old[0, 1]) ? ++x0 : 0;
                x1 = (x_cur[1, 0] == x_old[1, 0] && x_cur[1, 1] == x_old[1, 1]) ? ++x1 : 0;
                x2 = (x_cur[2, 0] == x_old[2, 0] && x_cur[2, 1] == x_old[2, 1]) ? ++x2 : 0;

                // if the point is a part of simplex after m iterations (cycle move)
                if (x0 > m)
                {
                    Console.WriteLine($"{Environment.NewLine}Cycle move around x[{i}]: " +
                        $"{x_cur[0, 0]}; y[{i}]: {x_cur[0, 1]}; Simplex size reduction...{Environment.NewLine}");

                    a = a / 2;

                    b1 = ((Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2))) * a;
                    b2 = ((Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2))) * a;

                    x_cur[1, 0] = x_cur[0, 0] + b2;
                    x_cur[1, 1] = x_cur[0, 1] + b1;
                    x_cur[2, 0] = x_cur[0, 0] + b1;
                    x_cur[2, 1] = x_cur[0, 1] + b2;

                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                    Console.WriteLine($"x[0] = {x_cur[0, 0]}; y[0] = {x_cur[0, 1]}; f[0] = {ans_cur[0]}");
                    Console.WriteLine($"x[1] = {x_cur[1, 0]}; y[1] = {x_cur[1, 1]}; f[1] = {ans_cur[1]}");
                    Console.WriteLine($"x[2] = {x_cur[2, 0]}; y[2] = {x_cur[2, 1]}; f[2] = {ans_cur[2]}");

                    x0 = 0;
                    i = 2;
                }
                else if (x1 > m)
                {
                    Console.WriteLine($"{Environment.NewLine}Cycle move around x[{i}]: " +
                        $"{x_cur[1, 0]}; y[{i}]: {x_cur[1, 1]}; Simplex size reduction...{Environment.NewLine}");

                    a = a / 2;

                    b1 = ((Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2))) * a;
                    b2 = ((Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2))) * a;

                    x_cur[0, 0] = x_cur[1, 0];
                    x_cur[0, 1] = x_cur[1, 1];

                    x_cur[1, 0] = x_cur[0, 0] + b2;
                    x_cur[1, 1] = x_cur[0, 1] + b1;
                    x_cur[2, 0] = x_cur[0, 0] + b1;
                    x_cur[2, 1] = x_cur[0, 1] + b2;

                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                    Console.WriteLine($"x[0] = {x_cur[0, 0]}; y[0] = {x_cur[0, 1]}; f[0] = {ans_cur[0]}");
                    Console.WriteLine($"x[1] = {x_cur[1, 0]}; y[1] = {x_cur[1, 1]}; f[1] = {ans_cur[1]}");
                    Console.WriteLine($"x[2] = {x_cur[2, 0]}; y[2] = {x_cur[2, 1]}; f[2] = {ans_cur[2]}");

                    x1 = 0;
                    i = 2;
                }
                else if (x2 > m)
                {
                    Console.WriteLine($"{Environment.NewLine}Cycle move around x[{i}]: " +
                        $"{x_cur[2, 0]}; y[{i}]: {x_cur[2, 1]}; Simplex size reduction...{Environment.NewLine}");

                    a = a / 2;

                    b1 = ((Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2))) * a;
                    b2 = ((Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2))) * a;

                    x_cur[0, 0] = x_cur[2, 0];
                    x_cur[0, 1] = x_cur[2, 1];

                    x_cur[1, 0] = x_cur[0, 0] + b2;
                    x_cur[1, 1] = x_cur[0, 1] + b1;
                    x_cur[2, 0] = x_cur[0, 0] + b1;
                    x_cur[2, 1] = x_cur[0, 1] + b2;

                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1]);
                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1]);
                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1]);

                    Console.WriteLine($"x[0] = {x_cur[0, 0]}; y[0] = {x_cur[0, 1]}; f[0] = {ans_cur[0]}");
                    Console.WriteLine($"x[1] = {x_cur[1, 0]}; y[1] = {x_cur[1, 1]}; f[1] = {ans_cur[1]}");
                    Console.WriteLine($"x[2] = {x_cur[2, 0]}; y[2] = {x_cur[2, 1]}; f[2] = {ans_cur[2]}");

                    x2 = 0;
                    i = 2;
                }

                // simplex size computation
                mid = (ans_cur[0] + ans_cur[1] + ans_cur[2]) / (n + 1);
                path = Math.Sqrt(((ans_cur[0] - mid) * (ans_cur[0] - mid) + (ans_cur[1] - mid)
                    * (ans_cur[1] - mid) + (ans_cur[2] - mid) * (ans_cur[2] - mid)) / (n + 1));

                // criterion of completion
                if (path < eps)
                {
                    double[] min = new double[3];
                    min[0] = ans_cur[0];
                    min[1] = ans_cur[1];
                    min[2] = ans_cur[2];
                    int minIndex = Array.IndexOf(min, min.Min());

                    Console.WriteLine($"{Environment.NewLine}Minimum: {ans_cur[minIndex]}; " +
                        $"x[{i}]: {x_cur[minIndex, 0]}; y[{i}]: {x_cur[minIndex, 1]}");
                    Console.WriteLine($"Iterations: {y}");
                }

                i++;
                y++;
            }
        }

        // main method with external penalty parameter
        public static void FindMin(double x_start, double y_start, double _e, double _p, double _c)
        {
            // variable initiation - scale factor, space dimension, penalty, penalty divider
            double a = 2;
            int n = 2;
            double p = _p;
            double c = _c;

            // variables for iteration count around one point (cycle move)
            int x0 = 0;
            int x1 = 0;
            int x2 = 0;

            // iteration limit for point
            int m = Convert.ToInt32((1.65 * n) + 0.05 * Math.Pow(n, 2));

            // epsilon
            double eps = _e;

            // arrays for current, old, temp and new points
            double[,] x_cur = new double[3, 2];
            double[,] x_old = new double[3, 2];
            double[,] temp = new double[1, 2];
            double[,] chosen = new double[1, 2];

            // arrays for function values
            double[] ans_cur = new double[3];
            double[] ans_old = new double[3];

            // starting point x0
            x_cur[0, 0] = x_start;
            x_cur[0, 1] = y_start;

            // growth
            double b1 = ((Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2))) * a;
            double b2 = ((Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2))) * a;

            // x1, x2
            x_cur[1, 0] = x_cur[0, 0] + b2;
            x_cur[1, 1] = x_cur[0, 1] + b1;
            x_cur[2, 0] = x_cur[0, 0] + b1;
            x_cur[2, 1] = x_cur[0, 1] + b2;

            ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
            ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
            ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

            Console.WriteLine($"x[0] = {x_cur[0, 0]}; y[0] = {x_cur[0, 1]}; f[0] = {ans_cur[0]}");
            Console.WriteLine($"x[1] = {x_cur[1, 0]}; y[1] = {x_cur[1, 1]}; f[1] = {ans_cur[1]}");
            Console.WriteLine($"x[2] = {x_cur[2, 0]}; y[2] = {x_cur[2, 1]}; f[2] = {ans_cur[2]}");

            // point iterator
            int i = 3;

            // loop iterator
            int y = 0;

            // variables for simplex size
            double mid = 0;
            double path = 0;

            // do-while alternative
            bool first = true;

            while (path > eps || first)
            {
                first = false;

                // saving current point locations as the old ones
                x_old[0, 0] = x_cur[0, 0];
                x_old[0, 1] = x_cur[0, 1];

                x_old[1, 0] = x_cur[1, 0];
                x_old[1, 1] = x_cur[1, 1];

                x_old[2, 0] = x_cur[2, 0];
                x_old[2, 1] = x_cur[2, 1];

                ans_old[0] = ans_cur[0];
                ans_old[1] = ans_cur[1];
                ans_old[2] = ans_cur[2];

                switch (Max(ans_cur[0], ans_cur[1], ans_cur[2]))
                {
                    // if point 0 has the biggest value out of three
                    case 0:
                        // if point with the biggest value hadn't been received in the previous iteration
                        if (!Equal(x_cur, 0, chosen))
                        {
                            temp[0, 0] = x_cur[0, 0];
                            temp[0, 1] = x_cur[0, 1];

                            x_cur[0, 0] = x_cur[1, 0] + x_cur[2, 0] - temp[0, 0];
                            x_cur[0, 1] = x_cur[1, 1] + x_cur[2, 1] - temp[0, 1];

                            ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                            ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                            ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                            chosen[0, 0] = x_cur[0, 0];
                            chosen[0, 1] = x_cur[0, 1];

                            Console.WriteLine($"x[{i}] = {x_cur[0, 0]}; y[{i}] = {x_cur[0, 1]}; f[{i}] = {ans_cur[0]}");
                            break;
                        }
                        // otherwise selecting a point with the biggest function value from the other two
                        else
                        {
                            switch (Max(ans_cur[1], ans_cur[2], 1, 2))
                            {
                                case 1:
                                    temp[0, 0] = x_cur[1, 0];
                                    temp[0, 1] = x_cur[1, 1];

                                    x_cur[1, 0] = x_cur[0, 0] + x_cur[2, 0] - temp[0, 0];
                                    x_cur[1, 1] = x_cur[0, 1] + x_cur[2, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                                    chosen[0, 0] = x_cur[1, 0];
                                    chosen[0, 1] = x_cur[1, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[1, 0]}; y[{i}] = {x_cur[1, 1]}; f[{i}] = {ans_cur[1]}");
                                    break;

                                case 2:
                                    temp[0, 0] = x_cur[2, 0];
                                    temp[0, 1] = x_cur[2, 1];

                                    x_cur[2, 0] = x_cur[0, 0] + x_cur[1, 0] - temp[0, 0];
                                    x_cur[2, 1] = x_cur[0, 1] + x_cur[1, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                                    chosen[0, 0] = x_cur[2, 0];
                                    chosen[0, 1] = x_cur[2, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[2, 0]}; y[{i}] = {x_cur[2, 1]}; f[{i}] = {ans_cur[2]}");
                                    break;
                            }
                        }
                        break;

                    // if point 1 has the biggest value out of three
                    case 1:
                        // if point with the biggest value hadn't been received in the previous iteration
                        if (!Equal(x_cur, 1, chosen))
                        {
                            temp[0, 0] = x_cur[1, 0];
                            temp[0, 1] = x_cur[1, 1];

                            x_cur[1, 0] = x_cur[0, 0] + x_cur[2, 0] - temp[0, 0];
                            x_cur[1, 1] = x_cur[0, 1] + x_cur[2, 1] - temp[0, 1];

                            ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                            ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                            ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                            chosen[0, 0] = x_cur[1, 0];
                            chosen[0, 1] = x_cur[1, 1];

                            Console.WriteLine($"x[{i}] = {x_cur[1, 0]}; y[{i}] = {x_cur[1, 1]}; f[{i}] = {ans_cur[1]}");
                            break;
                        }
                        // otherwise selecting a point with the biggest function value from the other two
                        else
                        {
                            switch (Max(ans_cur[0], ans_cur[2], 0, 2))
                            {
                                case 0:
                                    temp[0, 0] = x_cur[0, 0];
                                    temp[0, 1] = x_cur[0, 1];

                                    x_cur[0, 0] = x_cur[1, 0] + x_cur[2, 0] - temp[0, 0];
                                    x_cur[0, 1] = x_cur[1, 1] + x_cur[2, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                                    chosen[0, 0] = x_cur[0, 0];
                                    chosen[0, 1] = x_cur[0, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[0, 0]}; y[{i}] = {x_cur[0, 1]}; f[{i}] = {ans_cur[0]}");
                                    break;

                                case 2:
                                    temp[0, 0] = x_cur[2, 0];
                                    temp[0, 1] = x_cur[2, 1];

                                    x_cur[2, 0] = x_cur[0, 0] + x_cur[1, 0] - temp[0, 0];
                                    x_cur[2, 1] = x_cur[0, 1] + x_cur[1, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                                    chosen[0, 0] = x_cur[2, 0];
                                    chosen[0, 1] = x_cur[2, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[2, 0]}; y[{i}] = {x_cur[2, 1]}; f[{i}] = {ans_cur[2]}");
                                    break;
                            }
                        }
                        break;

                    // if point 2 has the biggest value out of three
                    case 2:
                        // if point with the biggest value hadn't been received in the previous iteration
                        if (!Equal(x_cur, 2, chosen))
                        {
                            temp[0, 0] = x_cur[2, 0];
                            temp[0, 1] = x_cur[2, 1];

                            x_cur[2, 0] = x_cur[0, 0] + x_cur[1, 0] - temp[0, 0];
                            x_cur[2, 1] = x_cur[0, 1] + x_cur[1, 1] - temp[0, 1];

                            ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                            ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                            ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                            chosen[0, 0] = x_cur[2, 0];
                            chosen[0, 1] = x_cur[2, 1];

                            Console.WriteLine($"x[{i}] = {x_cur[2, 0]}; y[{i}] = {x_cur[2, 1]}; f[{i}] = {ans_cur[2]}");
                            break;
                        }
                        // otherwise selecting a point with the biggest function value from the other two
                        else
                        {
                            switch (Max(ans_cur[0], ans_cur[1], 0, 1))
                            {
                                case 1:
                                    temp[0, 0] = x_cur[0, 0];
                                    temp[0, 1] = x_cur[0, 1];

                                    x_cur[0, 0] = x_cur[1, 0] + x_cur[2, 0] - temp[0, 0];
                                    x_cur[0, 1] = x_cur[1, 1] + x_cur[2, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                                    chosen[0, 0] = x_cur[0, 0];
                                    chosen[0, 1] = x_cur[0, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[1, 0]}; y[{i}] = {x_cur[1, 1]}; f[{i}] = {ans_cur[1]}");
                                    break;

                                case 0:
                                    temp[0, 0] = x_cur[0, 0];
                                    temp[0, 1] = x_cur[0, 1];

                                    x_cur[0, 0] = x_cur[1, 0] + x_cur[2, 0] - temp[0, 0];
                                    x_cur[0, 1] = x_cur[1, 1] + x_cur[2, 1] - temp[0, 1];

                                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                                    chosen[0, 0] = x_cur[0, 0];
                                    chosen[0, 1] = x_cur[0, 1];

                                    Console.WriteLine($"x[{i}] = {x_cur[0, 0]}; y[{i}] = {x_cur[0, 1]}; f[{i}] = {ans_cur[0]}");
                                    break;
                            }
                        }
                        break;
                }

                // if the point that was in the previous iteration occurs in the current - incrementing a counter
                x0 = (x_cur[0, 0] == x_old[0, 0] && x_cur[0, 1] == x_old[0, 1]) ? ++x0 : 0;
                x1 = (x_cur[1, 0] == x_old[1, 0] && x_cur[1, 1] == x_old[1, 1]) ? ++x1 : 0;
                x2 = (x_cur[2, 0] == x_old[2, 0] && x_cur[2, 1] == x_old[2, 1]) ? ++x2 : 0;

                // if the point is a part of simplex after m iterations (cycle move)
                if (x0 > m)
                {
                    Console.WriteLine($"{Environment.NewLine}Cycle move around x[{i}]: " +
                        $"{x_cur[0, 0]}; y[{i}]: {x_cur[0, 1]}; Simplex size reduction...{Environment.NewLine}");

                    a /= 2;

                    b1 = ((Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2))) * a;
                    b2 = ((Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2))) * a;

                    x_cur[0, 0] = x_cur[0, 0];
                    x_cur[0, 1] = x_cur[0, 1];

                    x_cur[1, 0] = x_cur[0, 0] + b2;
                    x_cur[1, 1] = x_cur[0, 1] + b1;
                    x_cur[2, 0] = x_cur[0, 0] + b1;
                    x_cur[2, 1] = x_cur[0, 1] + b2;

                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                    Console.WriteLine($"x[0] = {x_cur[0, 0]}; y[0] = {x_cur[0, 1]}; f[0] = {ans_cur[0]}");
                    Console.WriteLine($"x[1] = {x_cur[1, 0]}; y[1] = {x_cur[1, 1]}; f[1] = {ans_cur[1]}");
                    Console.WriteLine($"x[2] = {x_cur[2, 0]}; y[2] = {x_cur[2, 1]}; f[2] = {ans_cur[2]}");

                    x0 = 0;
                    i = 2;
                }
                else if (x1 > m)
                {
                    Console.WriteLine($"{Environment.NewLine}Cycle move around x[{i}]: " +
                        $"{x_cur[1, 0]}; y[{i}]: {x_cur[1, 1]}; Simplex size reduction...{Environment.NewLine}");

                    a = a / 2;

                    b1 = ((Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2))) * a;
                    b2 = ((Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2))) * a;

                    x_cur[0, 0] = x_cur[1, 0];
                    x_cur[0, 1] = x_cur[1, 1];

                    x_cur[1, 0] = x_cur[0, 0] + b2;
                    x_cur[1, 1] = x_cur[0, 1] + b1;
                    x_cur[2, 0] = x_cur[0, 0] + b1;
                    x_cur[2, 1] = x_cur[0, 1] + b2;

                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                    Console.WriteLine($"x[0] = {x_cur[0, 0]}; y[0] = {x_cur[0, 1]}; f[0] = {ans_cur[0]}");
                    Console.WriteLine($"x[1] = {x_cur[1, 0]}; y[1] = {x_cur[1, 1]}; f[1] = {ans_cur[1]}");
                    Console.WriteLine($"x[2] = {x_cur[2, 0]}; y[2] = {x_cur[2, 1]}; f[2] = {ans_cur[2]}");

                    x1 = 0;
                    i = 2;
                }
                else if (x2 > m)
                {
                    Console.WriteLine($"{Environment.NewLine}Cycle move around x[{i}]: " +
                        $"{x_cur[2, 0]}; y[{i}]: {x_cur[2, 1]}; Simplex size reduction...{Environment.NewLine}");

                    a = a / 2;

                    b1 = ((Math.Sqrt(n + 1) + n - 1) / (n * Math.Sqrt(2))) * a;
                    b2 = ((Math.Sqrt(n + 1) - 1) / (n * Math.Sqrt(2))) * a;

                    x_cur[0, 0] = x_cur[2, 0];
                    x_cur[0, 1] = x_cur[2, 1];

                    x_cur[1, 0] = x_cur[0, 0] + b2;
                    x_cur[1, 1] = x_cur[0, 1] + b1;
                    x_cur[2, 0] = x_cur[0, 0] + b1;
                    x_cur[2, 1] = x_cur[0, 1] + b2;

                    ans_cur[0] = Func(x_cur[0, 0], x_cur[0, 1], p);
                    ans_cur[1] = Func(x_cur[1, 0], x_cur[1, 1], p);
                    ans_cur[2] = Func(x_cur[2, 0], x_cur[2, 1], p);

                    Console.WriteLine($"x[0] = {x_cur[0, 0]}; y[0] = {x_cur[0, 1]}; f[0] = {ans_cur[0]}");
                    Console.WriteLine($"x[1] = {x_cur[1, 0]}; y[1] = {x_cur[1, 1]}; f[1] = {ans_cur[1]}");
                    Console.WriteLine($"x[2] = {x_cur[2, 0]}; y[2] = {x_cur[2, 1]}; f[2] = {ans_cur[2]}");

                    x2 = 0;
                    i = 2;
                }

                // simplex size computation
                mid = (ans_cur[0] + ans_cur[1] + ans_cur[2]) / (n + 1);
                path = Math.Sqrt(((ans_cur[0] - mid) * (ans_cur[0] - mid) + (ans_cur[1] - mid)
                    * (ans_cur[1] - mid) + (ans_cur[2] - mid) * (ans_cur[2] - mid)) / (n + 1));

                // criterion of completion
                if (path < eps)
                {
                    double[] min = new double[3];
                    min[0] = ans_cur[0];
                    min[1] = ans_cur[1];
                    min[2] = ans_cur[2];
                    int minIndex = Array.IndexOf(min, min.Min());

                    Console.WriteLine($"{Environment.NewLine}Minimum: {ans_cur[minIndex]}; " +
                        $"x[{i}]: {x_cur[minIndex, 0]}; y[{i}]: {x_cur[minIndex, 1]}");
                    Console.WriteLine($"Iterations: {y}{Environment.NewLine}");

                    i = 0;

                    while (true)
                    {
                        i++;
                        double[] res = { x_cur[minIndex, 0], x_cur[minIndex, 1], ans_cur[minIndex] };
                        Console.WriteLine($"x[{i}]: {res[0]}; y[{i}]: {res[1]}; f[{i}]: {res[2]}");
                        if (p < eps)
                        {
                            Console.WriteLine($"{Environment.NewLine}x: {res[0]}; " +
                                $"y: {res[1]}; f: {res[2]}; Penalty parameter: {p}");
                            return;
                        }
                        p = p / c;
                    }
                }

                i++;
                y++;
            }
        }
    }
}
