using System;
using System.Numerics;
using AntennaAI.RT.Antennas;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RT.Antennas.Tests
{
    [TestClass]
    public class UniformTests
    {
        [TestMethod]
        public void PatternTest()
        {
            var uniform = new Uniform();
            var expected = new Complex(1, 0);

            const double to_rad = Math.PI / 180;
            for(var theta = -180.0; theta <= 180; theta += 0.1)
                for(var phi = 0.0; phi <= 360; phi += 0.1)
                {
                    var f = uniform.Pattern(theta * to_rad, phi * to_rad);
                    Assert.That.Value(f).IsEqual(expected);
                }
        }
    }
}
