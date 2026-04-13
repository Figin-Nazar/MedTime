using System;
using System.Collections.Generic;
using System.Text;

namespace Console1
{
    class HealthProfile
    {
        private double _weight;

        public double Weight
        {
            get => _weight;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Вага повинна бути більше 0");

                if (value > 500)
                    throw new ArgumentException("Нереальна вага");

                _weight = value;
            }
        }
    }
}
