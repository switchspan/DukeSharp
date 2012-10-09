using System;

namespace Duke
{
    public class Property
    {
                 
        private readonly bool _ignore; 
      
        #region Member Properties

        public string Name { get; set; }
        // these are not used for matching. however, shouuld we perhaps make a 
        // privileged property? we must have some concept of identity.
        public bool IsIdProperty { get; set; }
        public bool IsAnalyzed { get; set; } // irrelevant if ID
       
        /// Sets the comparator used for this property. Note that
        /// changing this while Duke is processing may have unpredictable
        /// consequences. irrelevant if ID
        public IComparator Comparator { get; set; }
        /// Sets the high probability used for this property. Not that
        /// changing this while Duke is processing may have unpredicatable
        /// consquences. irrelevant if ID
        public double HighProbability { get; set; }
        /// Sets the low probability used for this property. Not that
        /// changing this while Duke is processing may have unpredicatable
        /// consquences.  irrelevant if ID
        public double LowProbability { get; set; }
        #endregion


        // used to initialize ID properties
        public Property(String name)
        {
            Name = name;
            IsIdProperty = true;
            IsAnalyzed = false;
            _ignore = false;
        }

        public Property(String name, IComparator comparator, double low, double high)
        {
            Name = name;
            Comparator = comparator;
            LowProbability = low;
            HighProbability = high;
            IsAnalyzed = comparator.IsTokenized();
            IsIdProperty = false;
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
            return _ignore || (HighProbability == 0.0);
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

            double sim = Comparator.Compare(v1, v2);
            if (sim >= 0.5)
            {
                return ((HighProbability - 0.5) * (sim * sim)) + 0.5;
            }
            return LowProbability;
        }

        public override string ToString()
        {
            return String.Format("[Property {0}]", Name);
        }
    }
}
