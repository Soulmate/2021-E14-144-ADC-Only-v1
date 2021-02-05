using System;
namespace FTT
{
    public class realfft
    {
        /*************************************************************************
         * 
         **** http://alglib.sources.ru/fft/realfft.php***
         * 
         * 
        Быстрое преобразование Фурье

        Алгоритм проводит быстрое преобразование Фурье вещественной
        функции, заданной n отсчетами на действительной оси.

        В зависимости от  переданных параметров, может выполняться
        как прямое, так и обратное преобразование.

        Входные параметры:
            tnn  -   Число значений функции. Должно  быть  степенью
                    двойки. Алгоритм   не  проверяет  правильность
                    переданного значения.
            a   -   array [0 .. nn-1] of Real
                    Значения функции.
            InverseFFT
                -   направление преобразования.
                    True, если обратное, False, если прямое.
                
        Выходные параметры:
            a   -   результат   преобразования.   Подробнее    см.
                    описание на сайте.
        *************************************************************************/
        public static void realfastfouriertransform(ref double[] a, int tnn, bool inversefft)
        {
            double twr = 0;
            double twi = 0;
            double twpr = 0;
            double twpi = 0;
            double twtemp = 0;
            double ttheta = 0;
            int i = 0;
            int i1 = 0;
            int i2 = 0;
            int i3 = 0;
            int i4 = 0;
            double c1 = 0;
            double c2 = 0;
            double h1r = 0;
            double h1i = 0;
            double h2r = 0;
            double h2i = 0;
            double wrs = 0;
            double wis = 0;
            int nn = 0;
            int ii = 0;
            int jj = 0;
            int n = 0;
            int mmax = 0;
            int m = 0;
            int j = 0;
            int istep = 0;
            int isign = 0;
            double wtemp = 0;
            double wr = 0;
            double wpr = 0;
            double wpi = 0;
            double wi = 0;
            double theta = 0;
            double tempr = 0;
            double tempi = 0;

            if (tnn == 1)
            {
                return;
            }
            if (!inversefft)
            {
                ttheta = 2 * Math.PI / tnn;
                c1 = 0.5;
                c2 = -0.5;
            }
            else
            {
                ttheta = 2 * Math.PI / tnn;
                c1 = 0.5;
                c2 = 0.5;
                ttheta = -ttheta;
                twpr = -(2.0 * AP.Math.Sqr(Math.Sin(0.5 * ttheta)));
                twpi = Math.Sin(ttheta);
                twr = 1.0 + twpr;
                twi = twpi;
                for (i = 2; i <= tnn / 4 + 1; i++)
                {
                    i1 = i + i - 2;
                    i2 = i1 + 1;
                    i3 = tnn + 1 - i2;
                    i4 = i3 + 1;
                    wrs = twr;
                    wis = twi;
                    h1r = c1 * (a[i1] + a[i3]);
                    h1i = c1 * (a[i2] - a[i4]);
                    h2r = -(c2 * (a[i2] + a[i4]));
                    h2i = c2 * (a[i1] - a[i3]);
                    a[i1] = h1r + wrs * h2r - wis * h2i;
                    a[i2] = h1i + wrs * h2i + wis * h2r;
                    a[i3] = h1r - wrs * h2r + wis * h2i;
                    a[i4] = -h1i + wrs * h2i + wis * h2r;
                    twtemp = twr;
                    twr = twr * twpr - twi * twpi + twr;
                    twi = twi * twpr + twtemp * twpi + twi;
                }
                h1r = a[0];
                a[0] = c1 * (h1r + a[1]);
                a[1] = c1 * (h1r - a[1]);
            }
            if (inversefft)
            {
                isign = -1;
            }
            else
            {
                isign = 1;
            }
            n = tnn;
            nn = tnn / 2;
            j = 1;
            for (ii = 1; ii <= nn; ii++)
            {
                i = 2 * ii - 1;
                if (j > i)
                {
                    tempr = a[j - 1];
                    tempi = a[j];
                    a[j - 1] = a[i - 1];
                    a[j] = a[i];
                    a[i - 1] = tempr;
                    a[i] = tempi;
                }
                m = n / 2;
                while (m >= 2 & j > m)
                {
                    j = j - m;
                    m = m / 2;
                }
                j = j + m;
            }
            mmax = 2;
            while (n > mmax)
            {
                istep = 2 * mmax;
                theta = 2 * Math.PI / (isign * mmax);
                wpr = -(2.0 * AP.Math.Sqr(Math.Sin(0.5 * theta)));
                wpi = Math.Sin(theta);
                wr = 1.0;
                wi = 0.0;
                for (ii = 1; ii <= mmax / 2; ii++)
                {
                    m = 2 * ii - 1;
                    for (jj = 0; jj <= (n - m) / istep; jj++)
                    {
                        i = m + jj * istep;
                        j = i + mmax;
                        tempr = wr * a[j - 1] - wi * a[j];
                        tempi = wr * a[j] + wi * a[j - 1];
                        a[j - 1] = a[i - 1] - tempr;
                        a[j] = a[i] - tempi;
                        a[i - 1] = a[i - 1] + tempr;
                        a[i] = a[i] + tempi;
                    }
                    wtemp = wr;
                    wr = wr * wpr - wi * wpi + wr;
                    wi = wi * wpr + wtemp * wpi + wi;
                }
                mmax = istep;
            }
            if (inversefft)
            {
                for (i = 1; i <= 2 * nn; i++)
                {
                    a[i - 1] = a[i - 1] / nn;
                }
            }
            if (!inversefft)
            {
                twpr = -(2.0 * AP.Math.Sqr(Math.Sin(0.5 * ttheta)));
                twpi = Math.Sin(ttheta);
                twr = 1.0 + twpr;
                twi = twpi;
                for (i = 2; i <= tnn / 4 + 1; i++)
                {
                    i1 = i + i - 2;
                    i2 = i1 + 1;
                    i3 = tnn + 1 - i2;
                    i4 = i3 + 1;
                    wrs = twr;
                    wis = twi;
                    h1r = c1 * (a[i1] + a[i3]);
                    h1i = c1 * (a[i2] - a[i4]);
                    h2r = -(c2 * (a[i2] + a[i4]));
                    h2i = c2 * (a[i1] - a[i3]);
                    a[i1] = h1r + wrs * h2r - wis * h2i;
                    a[i2] = h1i + wrs * h2i + wis * h2r;
                    a[i3] = h1r - wrs * h2r + wis * h2i;
                    a[i4] = -h1i + wrs * h2i + wis * h2r;
                    twtemp = twr;
                    twr = twr * twpr - twi * twpi + twr;
                    twi = twi * twpr + twtemp * twpi + twi;
                }
                h1r = a[0];
                a[0] = h1r + a[1];
                a[1] = h1r - a[1];
            }
        }
    }
}