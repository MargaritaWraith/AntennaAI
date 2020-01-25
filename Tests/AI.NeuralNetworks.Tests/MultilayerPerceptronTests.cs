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
        private static void ProcessLayer(
            double[] Input,
            double[,] W,
            double[] NeuronOffsets,
            double[] OffsetsWeights,
            double[] Output)
        {
            for (var output_index = 0; output_index < Output.Length; output_index++)
            {
                Output[output_index] = 0;
                for (var input_index = 0; input_index < Input.Length; input_index++)
                    Output[output_index] += W[output_index, input_index] * Input[input_index];

                Output[output_index] += NeuronOffsets[output_index] * OffsetsWeights[output_index];
            }
        }

        private static double Activation(double x) => 1 / (1 + Math.Exp(-x));

        private static void Activation(double[] X, double[] FX)
        {
            for (var i = 0; i < X.Length; i++)
                FX[i] = Activation(X[i]);
        }

        private static void DirectDistribution(
            double[][] Inputs,
            double[][,] Layers,
            double[][] Offsets,
            double[][] OffsetsWeights,
            double[][] Outputs,
            double[] NetworkOutput)
        {
            var layer_index = -1;
            do
            {
                if (layer_index++ == 0)
                    Activation(Outputs[layer_index - 1], Inputs[layer_index]);

                ProcessLayer(
                    Inputs[layer_index],
                    Layers[layer_index],
                    Offsets[layer_index],
                    OffsetsWeights[layer_index],
                    Outputs[layer_index]);
            } while (layer_index < Layers.Length - 1);

            Activation(Outputs[layer_index], NetworkOutput);
        }

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

            // прямое распространение


            DirectDistribution(inputs, layers, Offsets, OffsetsW, outputs, network_output);

            CollectionAssert.That.Collection(outputs[0]).ValuesAreEqual(1.5, 3);
            CollectionAssert.That.Collection(inputs[1]).IsEqualTo(new[] { 0.81757, 0.952574 }, 4.48e-6);
            CollectionAssert.That.Collection(outputs[1]).IsEqualTo(new[] { 1.2738 }, 1.25e-5);
            CollectionAssert.That.Collection(network_output).IsEqualTo(new[] { 0.78139 }, 4.31e-7);
        }
    }
}
