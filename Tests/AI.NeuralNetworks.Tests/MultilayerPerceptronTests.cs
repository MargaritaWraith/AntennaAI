using System;
using System.Linq;
using AntennaAI.AI.NeuralNetworks;
using AntennaAI.AI.NeuralNetworks.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AI.NeuralNetworks.Tests
{
    [TestClass]
    public class MultilayerPerceptronTests
    {
      [TestMethod]
        public void NeuralNetwork_Integral_Test()
        {
            double[,] W0 =
            {
                {  1.0, 0.5 },
                { -1.0, 2.0 }
            };
            double[] Offsets0 = { 1, 1 };
            double[] OffsetsW0 = { 1, 1 };

            double[,] W1 =
            {
                { 1.5, -1.0 }
            };
            double[] Offsets1 = { 1 };
            double[] OffsetsW1 = { 1 };


            double[][,] layers = { (double[,])W0.Clone(), (double[,])W1.Clone() };
            double[][] Offsets = { (double[])Offsets0.Clone(), (double[])Offsets1.Clone() };
            double[][] OffsetsW = { (double[])OffsetsW0.Clone(), (double[])OffsetsW1.Clone() };

            double[] network_input = { 0, 1 };

            double[][] inputs =
            {
                network_input,
                new double[2],    // создаём массив из 2 чисел
            };

            double[][] outputs =
            {
                new double[2],
                new double[1]
            };

            var errors = new double[outputs.Length][];
            for (var i = 0; i < errors.Length; i++)
                errors[i] = new double[outputs[i].Length];

            var network_output = new double[1];

            Assert.Fail();
        }
    }
}
