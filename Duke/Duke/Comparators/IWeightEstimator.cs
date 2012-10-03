using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Comparators
{
    public interface IWeightEstimator
    {
        double Substitute(char ch1, char ch2);

        double Delete(char ch);

        double Insert(char ch);
    }
}
