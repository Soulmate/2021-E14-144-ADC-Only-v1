using System;
using System.Collections.Generic;
using System.Text;

namespace FTT
{
    public class ClassRealFtt
    {
        public double[] result; //спектр
        public double deltaF; //шаг частоты в спектре
        public ClassRealFtt(double[] inputData, int windowSize, double deltaT)
        {
            if (inputData != null && inputData.Length != 0)
            {
                double log = Math.Log(windowSize, 2);
                if (log < 0 || (Math.Abs(log - Math.Round(log)) > 0.0001))
                    throw new ApplicationException("wrong windowSize value");

                deltaF = 1 / (deltaT * windowSize);
                double[][] splitedInputData = Split(inputData, windowSize);                
                for (int i = 0; i < splitedInputData.Length; i++)
                    realfft.realfastfouriertransform(ref splitedInputData[i], windowSize, false);
                double[] realfttResult = Average(splitedInputData, windowSize);

                result = new double[windowSize / 2];
                for (int i = 0; i < result.Length - 1; i++)
                    result[i] = 0.5 * Math.Log10(realfttResult[2 * (i + 1)] * realfttResult[2 * (i + 1)] + realfttResult[2 * (i + 1) + 1] * realfttResult[2 * (i + 1) + 1]);
                result[result.Length - 1] = realfttResult[1];
            }            
        }

        private double[][] Split(double[] inputData, int windowSize)
        {
            if (inputData.Length <= 0)
                return null;

            double[][] result = new double[(inputData.Length - 1) / windowSize + 1][];
            for (int t = 0; t < result.Length; t++)
                result[t] = new double[windowSize];

            int i;
            for (i = 0; i < inputData.Length; i++)
                result[i / windowSize][i % windowSize] = inputData[i];
            for (int j = i % windowSize; j < windowSize && j > 0; j++)
                result[(i - 1) / windowSize][j] = inputData[i - 1];

            return result;

            //if (inputData.Length <= 0)
            //    return null;

            //double[][] result = new double[(inputData.Length - 1) / windowSize][];
            //for (int t = 0; t < result.Length; t++)
            //    result[t] = new double[windowSize];

            //int i;
            //for (i = 0; i / windowSize < result.Length; i++)
            //    result[i / windowSize][i % windowSize] = inputData[i];
            //return result;
        }
        private double[] Average(double[][] doubleArray, int length)
        {
            double[] result = new double[length];
            for(int i = 0; i<result.Length;i++)
                result[i] = 0;
            for (int i = 0; i < doubleArray.Length; i++)
                for (int j = 0; j < Math.Min(doubleArray[i].Length, length); j++)
                    result[j] += doubleArray[i][j];
            for (int i = 0; i < result.Length; i++)
                result[i] /= doubleArray.Length;
            return result;
        }
    }
}
