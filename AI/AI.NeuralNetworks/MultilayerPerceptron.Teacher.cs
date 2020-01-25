using System;
using System.Collections.Generic;
using System.Text;
using AntennaAI.AI.NeuralNetworks.Interfaces;

namespace AntennaAI.AI.NeuralNetworks
{
    public partial class MultilayerPerceptron
    {
        public INetworkTeacher CreateTeacher() => throw new NotImplementedException();

        public TNetworkTeacher CreateTeacher<TNetworkTeacher>(Action<TNetworkTeacher> Configurator = null) where TNetworkTeacher : class, INetworkTeacher => throw new NotImplementedException();
    }
}
