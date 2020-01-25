using System;
using System.Xml.Serialization;

namespace AntennaAI.Mathematics.Geometry
{
    public readonly struct Vector3D : IEquatable<Vector3D>, IFormattable, ICloneable
    {
        /* -------------------------------------------------------------------------------------------- */

        private const double __Pi05 = Math.PI / 2;

        /* -------------------------------------------------------------------------------------------- */

        public static Vector3D XYZ(double X, double Y, double Z) => new Vector3D(X, Y, Z);

        public static Vector3D Random(double min = -100, double max = 100)
        {
            var random = new Random();
            double Rnd() => Math.Abs(max - min) * (random.NextDouble() - .5) + (max + min) * .5;
            return new Vector3D(Rnd(), Rnd(), Rnd());
        }

        /* -------------------------------------------------------------------------------------------- */

        public static readonly Vector3D Empty = new Vector3D();

        /// <summary>Единичный базисный вектор</summary>
        public static readonly Vector3D BasisUnitVector = new Vector3D(1, 1, 1);

        /// <summary>Базисный вектор i</summary>
        public static readonly Vector3D i = new Vector3D(1, 0, 0);

        /// <summary>Базисный вектор j</summary>
        public static readonly Vector3D j = new Vector3D(0, 1, 0);

        /// <summary>Базисный вектор k</summary>
        public static readonly Vector3D k = new Vector3D(0, 0, 1);

        /* -------------------------------------------------------------------------------------------- */

        /// <summary>Длина по оси X</summary>
        private readonly double _X;

        /// <summary>Длина по оси Y</summary>
        private readonly double _Y;

        /// <summary>Длина по оси Z</summary>
        private readonly double _Z;

        /* -------------------------------------------------------------------------------------------- */

        /// <summary>Длина по оси X</summary>
        [XmlAttribute]
        public double X => _X;

        /// <summary>Длина по оси Y</summary>
        [XmlAttribute]
        public double Y => _Y;

        /// <summary>Длина по оси Z</summary>
        [XmlAttribute]
        public double Z => _Z;

        /// <summary>Длина вектора</summary>
        [XmlIgnore]
        public double R => Math.Sqrt(_X * _X + _Y * _Y + _Z * _Z);

        /// <summary>Угол проекции в плоскости XOY</summary>
        public double AngleXOY => Math.Abs(_X) < double.Epsilon
                    ? Math.Abs(_Y) < double.Epsilon       // X == 0
                        ? 0                               //  Y == 0 => 0
                        : Math.Sign(_Y) * __Pi05          //  Y != 0 => pi/2 * sign(Y)
                    : Math.Abs(_Y) < double.Epsilon       // X != 0
                        ? Math.Sign(_X) > 0
                            ? 0
                            : Math.PI
                        : Math.Atan2(_Y, _X);

        /// <summary>Угол проекции в плоскости XOZ</summary>
        public double AngleXOZ => Math.Abs(_X) < double.Epsilon
                    ? Math.Abs(_Z) < double.Epsilon       // X == 0
                        ? 0                               //  Z == 0 => 0
                        : Math.Sign(_Z) * __Pi05          //  Z != 0 => pi/2 * sign(Z)
                    : Math.Abs(_Z) < double.Epsilon       // X != 0
                        ? Math.Sign(_X) > 0
                            ? 0
                            : Math.PI
                        : Math.Atan2(_Z, _X);

        /// <summary>Угол проекции в плоскости YOZ</summary>
        public double AngleYOZ => Math.Abs(_Y) < double.Epsilon
                    ? Math.Abs(_Z) < double.Epsilon       // Y == 0
                        ? 0                               //  Z == 0 => 0
                        : Math.Sign(_Z) * __Pi05          //  Z != 0 => pi/2 * sign(Y)
                    : Math.Abs(_Z) < double.Epsilon       // Y != 0
                        ? Math.Sign(_Y) > 0
                            ? 0
                            : Math.PI
                        : Math.Atan2(_Z, _Y);

        /// <summary>Азимутальный угол</summary>
        public double Phi => AngleXOY;

        /// <summary>Угол места</summary>
        public double Theta => Math.Atan2(R_XOY, _Z);

        /// <summary>Длина в плоскости XOY</summary>
        public double R_XOY => Math.Sqrt(_X * _X + _Y * _Y);

        /// <summary>Длина в плоскости XOZ</summary>
        public double R_XOZ => Math.Sqrt(_X * _X + _Z * _Z);

        /// <summary>Длина в плоскости YOZ</summary>
        public double R_YOZ => Math.Sqrt(_Y * _Y + _Z * _Z);

        public Vector3D Abs => new Vector3D(Math.Abs(_X), Math.Abs(_Y), Math.Abs(_Z));

        public Vector3D Sign => new Vector3D(Math.Sign(_X), Math.Sign(_Y), Math.Sign(_Z));

        /* -------------------------------------------------------------------------------------------- */

        public Vector3D(double X) { _X = X; _Y = 0; _Z = 0; }


        public Vector3D(double X, double Y) { _X = X; _Y = Y; _Z = 0; }

        public Vector3D(double X, double Y, double Z) { _X = X; _Y = Y; _Z = Z; }

        private Vector3D(in Vector3D V) => (_X, _Y, _Z) = V;

        public Vector3D Inc(double dx, double dy, double dz) => new Vector3D(_X + dx, _Y + dy, _Z + dz);

        public Vector3D Inc_X(double dx) => new Vector3D(_X + dx, _Y, _Z);
        public Vector3D Inc_Y(double dy) => new Vector3D(_X, _Y + dy, _Z);
        public Vector3D Inc_Z(double dz) => new Vector3D(_X, _Y, _Z + dz);

        public Vector3D Dec(double dx, double dy, double dz) => new Vector3D(_X - dx, _Y - dy, _Z - dz);

        public Vector3D Dec_X(double dx) => new Vector3D(_X - dx, _Y, _Z);
        public Vector3D Dec_Y(double dy) => new Vector3D(_X, _Y - dy, _Z);
        public Vector3D Dec_Z(double dz) => new Vector3D(_X, _Y, _Z - dz);


        public Vector3D Scale_X(double kx) => new Vector3D(_X * kx, _Y, _Z);
        public Vector3D Scale_Y(double ky) => new Vector3D(_X, _Y * ky, _Z);
        public Vector3D Scale_Z(double kz) => new Vector3D(_X, _Y, _Z * kz);

        public Vector3D Scale(double kx, double ky, double kz) => new Vector3D(_X * kx, _Y * ky, _Z * kz);

        /* -------------------------------------------------------------------------------------------- */

        public override string ToString() => $"({_X};{_Y};{_Z})";

        public override int GetHashCode()
        {
            unchecked
            {
                var result = _X.GetHashCode();
                result = (result * 397) ^ _Y.GetHashCode();
                result = (result * 397) ^ _Z.GetHashCode();
                return result;
            }
        }

        /// <summary>Создает новый объект, который является копией текущего экземпляра.</summary>
        /// <returns>Новый объект, являющийся копией этого экземпляра.</returns><filterpriority>2</filterpriority>
        object ICloneable.Clone() => Clone();

        public Vector3D Clone() => new Vector3D(this);

        public override bool Equals(object obj) => obj is Vector3D vector_3d && Equals(vector_3d);

        public string ToString(string Format) => $"({_X.ToString(Format)};{_Y.ToString(Format)};{_Z.ToString(Format)})";

        public string ToString(string Format, IFormatProvider Provider) => $"({_X.ToString(Format, Provider)};{_Y.ToString(Format, Provider)};{_Z.ToString(Format, Provider)})";

        public void Deconstruct(out double x, out double y, out double z)
        {
            x = _X;
            y = _Y;
            z = _Z;
        }

        /* -------------------------------------------------------------------------------------------- */

        #region IEquatable<Vector3D> Members

        /// <summary>Точность сравнения (по умолчанию 10^-16)</summary>
        public static double ComparisonsAccuracy { get; set; } = 1e-16;

        public bool Equals(Vector3D other)
        {
            var eps = ComparisonsAccuracy;
            return Math.Abs(other._X - _X) < eps
                   && Math.Abs(other._Y - _Y) < eps
                   && Math.Abs(other._Z - _Z) < eps;
        }

        #endregion

        /* -------------------------------------------------------------------------------------------- */

        public Vector3D GetInverse() => new Vector3D(1 / _X, 1 / _Y, 1 / _Z);

        /// <summary>Скалярное произведение векторов</summary>
        /// <param name="Vector">Вектор, на который умножается текущий вектор</param>
        /// <returns>Число, равное скалярному произведению векторов</returns>
        public double Product_Scalar(Vector3D Vector) => _X * Vector._X + Y * Vector._Y + _Z * Vector._Z;

        /// <summary>Смешанное произведение трёх векторов</summary>
        /// <param name="A">Первый вектор произведения</param>
        /// <param name="B">Второй вектор произведения</param>
        /// <param name="C">Третий вектор произведения</param>
        /// <returns>Число, равное смешанному произведения векторов</returns>
        public static double Product_Mixed(Vector3D A, Vector3D B, Vector3D C) =>
            +1 * A._X * (B._Y * C._Z - B._Z * C._Y) +
            -1 * A._Y * (B._X * C._Z - B._Z * C._X) +
            +1 * A._Z * (B._X * C._Y - B._Y * C._X);

        /// <summary>Векторное произведение векторов</summary>
        /// <param name="Vector">Вектор, на который умножается исходный вектор</param>
        /// <returns>Вектор, равный векторному произведению векторов</returns>
        public Vector3D Product_Vector(Vector3D Vector)
        {
            /*
             * A = {Xa, Ya, Za}
             * B = {Xb, Yb, Zb}
             *         | i  j  k  |   {Ya * Zb - Za * Yb}   {Xc}
             * A * B = | Xa Ya Za | = {Za * Xb - Xa * Zb} = {Yc} = C
             *         | Xb Yb Zb |   {Xa * Yb - Ya * Xb}   {Zc}
             * C = {Xc, Yc, Zc}
             */

            var A = this;
            var B = Vector;
            return new Vector3D
                (
                    A._Y * B._Z - A._Z * B._Y, // X
                    A._Z * B._X - A._X * B._Z, // Y
                    A._X * B._Y - A._Y * B._X  // Z
                );
        }

        /// <summary>Покомпонентное умножение на вектор</summary>
        /// <param name="Vector">Векторный сомножитель</param>
        /// <returns>Вектор, компоненты которого являются произведениями компоненты векторов</returns>
        public Vector3D Product_Component(Vector3D Vector)
            => new Vector3D(_X * Vector._X, _Y * Vector._Y, _Z * Vector._Z);

        /// <summary>Проекция на вектор</summary>
        /// <param name="Vector">Вектор, НА который производится проекции</param>
        /// <returns>Проекция на вектор</returns>
        public double GetProjectionTo(Vector3D Vector) => Product_Scalar(Vector) / Vector.R;

        public Func<Vector3D, double> GetProjectorV()
        {
            var t = this;
            return v => t * v / v.R;
        }

        /// <summary>Проекция на направление</summary>
        public double GetProjectionTo(double DirectionTheta, double DirectionPhi) => 
            Math.Sin(DirectionTheta) * (_X * Math.Cos(DirectionPhi) + _Y * Math.Sin(DirectionPhi)) 
            + _Z * Math.Cos(DirectionTheta);

        /* -------------------------------------------------------------------------------------------- */

        #region Вектор на число

        public static Vector3D operator +(Vector3D V, double x) => new Vector3D(V._X + x, V._Y + x, V._Z + x);
        public static Vector3D operator +(double x, Vector3D V) => new Vector3D(V._X + x, V._Y + x, V._Z + x);

        public static Vector3D operator -(Vector3D V, double x) => new Vector3D(V._X - x, V._Y - x, V._Z - x);
        public static Vector3D operator -(double x, Vector3D V) => new Vector3D(x - V._X, x - V._Y, x - V._Z);

        public static Vector3D operator *(Vector3D V, double x) => new Vector3D(V._X * x, V._Y * x, V._Z * x);
        public static Vector3D operator *(double x, Vector3D V) => new Vector3D(V._X * x, V._Y * x, V._Z * x);

        public static Vector3D operator /(Vector3D V, double x) => new Vector3D(V._X / x, V._Y / x, V._Z / x);
        public static Vector3D operator /(double x, Vector3D V) => new Vector3D(x / V._X, x / V._Y, x / V._Z);


        public static Vector3D operator +(Vector3D V, float x) => new Vector3D(V._X + x, V._Y + x, V._Z + x);
        public static Vector3D operator +(float x, Vector3D V) => new Vector3D(V._X + x, V._Y + x, V._Z + x);

        public static Vector3D operator -(Vector3D V, float x) => new Vector3D(V._X - x, V._Y - x, V._Z - x);
        public static Vector3D operator -(float x, Vector3D V) => new Vector3D(x - V._X, x - V._Y, x - V._Z);

        public static Vector3D operator *(Vector3D V, float x) => new Vector3D(V._X * x, V._Y * x, V._Z * x);
        public static Vector3D operator *(float x, Vector3D V) => new Vector3D(V._X * x, V._Y * x, V._Z * x);

        public static Vector3D operator /(Vector3D V, float x) => new Vector3D(V._X / x, V._Y / x, V._Z / x);
        public static Vector3D operator /(float x, Vector3D V) => new Vector3D(x / V._X, x / V._Y, x / V._Z);


        public static Vector3D operator +(Vector3D V, int x) => new Vector3D(V._X + x, V._Y + x, V._Z + x);
        public static Vector3D operator +(int x, Vector3D V) => new Vector3D(V._X + x, V._Y + x, V._Z + x);

        public static Vector3D operator -(Vector3D V, int x) => new Vector3D(V._X - x, V._Y - x, V._Z - x);
        public static Vector3D operator -(int x, Vector3D V) => new Vector3D(x - V._X, x - V._Y, x - V._Z);

        public static Vector3D operator *(Vector3D V, int x) => new Vector3D(V._X * x, V._Y * x, V._Z * x);
        public static Vector3D operator *(int x, Vector3D V) => new Vector3D(V._X * x, V._Y * x, V._Z * x);

        public static Vector3D operator /(Vector3D V, int x) => new Vector3D(V._X / x, V._Y / x, V._Z / x);
        public static Vector3D operator /(int x, Vector3D V) => new Vector3D(x / V._X, x / V._Y, x / V._Z);

        #endregion

        public static bool operator ==(Vector3D X, Vector3D Y) => X._X.Equals(Y._X) && X._Y.Equals(Y._Y) && X._Z.Equals(Y._Z);
        public static bool operator !=(Vector3D X, Vector3D Y) => !X._X.Equals(Y._X) || !X._Y.Equals(Y._Y) || !X._Z.Equals(Y._Z);

        public static bool operator ==(Vector3D X, byte Y) => Y.Equals(0) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, byte Y) => !Y.Equals(0) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);

        public static bool operator ==(Vector3D X, sbyte Y) => Y.Equals(0) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, sbyte Y) => !Y.Equals(0) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);

        public static bool operator ==(Vector3D X, short Y) => Y.Equals(0) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, short Y) => !Y.Equals(0) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);

        public static bool operator ==(Vector3D X, ushort Y) => Y.Equals(0) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, ushort Y) => !Y.Equals(0) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);

        public static bool operator ==(Vector3D X, int Y) => Y.Equals(0) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, int Y) => !Y.Equals(0) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);

        public static bool operator ==(Vector3D X, uint Y) => Y.Equals(0) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, uint Y) => !Y.Equals(0) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);

        public static bool operator ==(Vector3D X, long Y) => Y.Equals(0) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, long Y) => !Y.Equals(0) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);

        public static bool operator ==(Vector3D X, ulong Y) => Y.Equals(0) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, ulong Y) => !Y.Equals(0) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);

        public static bool operator ==(Vector3D X, float Y) => Y.Equals(0f) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, float Y) => !Y.Equals(0f) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);

        public static bool operator ==(Vector3D X, double Y) => Y.Equals(0d) && X._X.Equals(0d) && X._Y.Equals(0d) && X._Z.Equals(0d);
        public static bool operator !=(Vector3D X, double Y) => !Y.Equals(0d) || !X._X.Equals(0d) || !X._Y.Equals(0d) || !X._Z.Equals(0d);


        #region Операции над двумя векторами

        public static Vector3D operator +(Vector3D A, Vector3D B) => new Vector3D(A._X + B._X, A._Y + B._Y, A._Z + B._Z);

        public static Vector3D operator -(Vector3D A, Vector3D B) => new Vector3D(A._X - B._X, A._Y - B._Y, A._Z - B._Z);

        /// <summary>Скалярное произведение векторов</summary>
        /// <param name="A">Первый вектор-множитель</param>
        /// <param name="B">Второй вектор-множитель</param>
        /// <returns>Число - скалярное произведение векторов</returns>
        public static double operator *(Vector3D A, Vector3D B) => A.Product_Scalar(B);

        /// <summary>Проверка на параллельность</summary>
        /// <param name="A">Вектор 1</param><param name="B">Вектор 2</param>
        /// <returns>Истина, если вектора параллельны</returns>
        public static bool operator |(Vector3D A, Vector3D B) => Math.Abs((A * B) / (A.R * B.R) - 1).Equals(0d);

        /// <summary>Проверка на ортогональность</summary>
        /// <param name="A">Вектор 1</param><param name="B">Вектор 2</param>
        /// <returns>Истина, если вектор 1 ортогонален вектору 2</returns>
        public static bool operator &(Vector3D A, Vector3D B) => Math.Abs((A * B) / (A.R * B.R)).Equals(0d);

        /// <summary>Проекция вектора A на вектор B</summary>
        /// <param name="A">Проецируемый вектор</param>
        /// <param name="B">Вектор, на который производится проекции</param>
        /// <returns>Проекция вектора А на вектор В</returns>
        public static double operator %(Vector3D A, Vector3D B) => A.GetProjectionTo(B);

        #endregion

        #region Операторы преобразований

        public static implicit operator double(Vector3D V) => V.R;

        public const double sqrt_3 = 1.7320508075688772935274463415058723669428052538104d;
        public static explicit operator Vector3D(double V) => new Vector3D(V / sqrt_3, V / sqrt_3, V / sqrt_3);

        #endregion

        /* -------------------------------------------------------------------------------------------- */
    }
}
