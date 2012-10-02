using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    public class Property
    {
        private String _name;
        private bool _id;
        private bool _analyzed;             // irrelevant if ID
        private bool _ignore;               // irrelevant if ID
        private IComparator _comparator;    // irrelevant if ID
        private double _high;               // irrelevant if ID
        private double _low;                // irrelevant if ID

        // used to initialize ID properties
        public Property(String name)
        {
            _name = name;
            _id = true;
            _analyzed = false;
        }

        public Property(String name, IComparator comparator, double low, double high)
        {
            _name = name;
            _id = false;
            _analyzed = comparator.IsTokenized();
            _comparator = comparator;
            _high = high;
            _low = low;
        }

        //TODO: rules for property names?
        public String GetName()
        {
            return _name;
        }

        // these are not used for matching. however, shouuld we perhaps make a 
        // privileged property? we must have some concept of identity.
        public bool IsIdProperty()
        {
            return _id;
        }

        public bool IsAnalizedProperty()
        {
            return _analyzed;
        }

        public IComparator GetComparator()
        {
            return _comparator;
        }

        public double GetHighProbability()
        {
            return _high;
        }

        public double GetLowProbability()
        {
            return _low;
        }

        /// <summary>
        /// Sets the comparator used for this property. Note that
        /// changing this while Duke is processing may have unpredictable
        /// consequences.
        /// </summary>
        /// <param name="comparator"></param>
        public void SetComparator(IComparator comparator)
        {
            _comparator = comparator;
        }

        /// <summary>
        /// Sets the high probability used for this property. Not that
        /// changing this while Duke is processing may have unpredicatable
        /// consquences.
        /// </summary>
        /// <param name="high"></param>
        public void SetHighProbability(double high)
        {
            _high = high;
        }

        /// <summary>
        /// Sets the low probability used for this property. Not that
        /// changing this while Duke is processing may have unpredicatable
        /// consquences.
        /// </summary>
        /// <param name="low"></param>
        public void SetLowProbability(double low)
        {
            _low = low;
        }

        /// <summary>
        /// If true the property should not be used
        /// for comparing records
        /// </summary>
        /// <returns></returns>
        public bool IsIgnoreProperty()
        {
            // some people set high probability to zero, which means these
            // properties will prevent any matches from occurring at all if 
            // we try to use them. so we skip these.
            return _ignore || (_high == 0.0);
        }

        /// <summary>
        /// Returns the probability that the records v1 and v2 represent
        /// the same entity, based on high and low probability settings, etc.
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public double Compare(String v1, String v2)
        {
            //FIXME: it should be possible here to say that, actually, we
            // didn't learn anything from comparing the two values, so that
            // probability is set to 0.5.

            double sim = _comparator.Compare(v1, v2);
            if (sim >= 0.5)
            {
                return ((_high - 0.5) * (sim * sim)) + 0.5;
            }
            else
            {
                return _low;
            }
        }

        public override string ToString()
        {
            return String.Format("[Property {0}]", _name);
        }
    }
}
