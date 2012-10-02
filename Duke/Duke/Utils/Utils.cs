using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke.Utils
{
    public class Utils
    {
        /// <summary>
        /// Combines two probabilities using Baye's theorem.
        /// </summary>
        /// <param name="prob1"></param>
        /// <param name="prob2"></param>
        /// <returns></returns>
        public static double ComputeBayes(double prob1, double prob2)
        {
            return (prob1*prob2)/
                   ((prob1*prob2) + ((1.0 - prob1)*(1.0 - prob2)));
        }

    }
}
