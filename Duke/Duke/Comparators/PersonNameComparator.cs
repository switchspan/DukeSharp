using System;
using Duke.Utils;

namespace Duke.Comparators
{
    /// <summary>
    /// An operator which knows about comparing names. It tokenizes, and
    /// also applies Levenshtein distance.
    /// </summary>
    public class PersonNameComparator : IComparator
    {
        #region Member methods

        public bool IsTokenized()
        {
            return true;
        }

        public double Compare(string v1, string v2)
        {
            if (v1.Equals(v2))
                return 1.0;

            if (v1.Length + v2.Length > 20 && Levenshtein.Distance(v1, v2) == 1)
                return 0.95;

            // tokenize
            String[] t1 = StringUtils.Split(v1);
            String[] t2 = StringUtils.Split(v2);

            // t1 must always be the longest
            if (t1.Length < t2.Length)
            {
                String[] tmp = t2;
                t2 = t1;
                t1 = tmp;
            }

            // penalty imposed by pre-processing
            double penalty = 0;

            // if the two are of unequal lengths, make some simple checks
            if (t1.Length != t2.Length && t2.Length >= 2)
            {
                // is the first token in t1 an initial? if so, get rid of it
                if ((t1[0].Length == 2 && t1[0][1] == '.') ||
                    t1[0].Length == 1)
                {
                    var tmp = new String[t1.Length - 1];
                    for (int ix = 1; ix < t1.Length; ix++)
                        tmp[ix - 1] = t1[ix];
                    t1 = tmp;
                    penalty = 0.2; // impose a penalty
                }
                else
                {
                    // use similarity between first and last tokens, ignoring what's
                    // in between
                    int d1 = Levenshtein.Distance(t1[0], t2[0]);
                    int d2 = Levenshtein.Distance(t1[t1.Length - 1], t2[t2.Length - 1]);
                    return (0.4/(d1 + 1)) + (0.4/(d2 + 1));
                }
            }

            // if the two are of the same length, go through and compare one by one
            if (t1.Length == t2.Length)
            {
                // are the names just reversed?
                if (t1.Length == 2 && t1[0].Equals(t2[1]) && t1[1].Equals(t2[0]))
                    return 0.9;

                // normal one-by-one comparison
                double points = 1.0 - penalty;
                for (int ix = 0; ix < t1.Length && points > 0; ix++)
                {
                    int d = Levenshtein.Distance(t1[ix], t2[ix]);

                    if (ix == 0 && d > 0 &&
                        (t1[ix].StartsWith(t2[ix]) || t2[ix].StartsWith(t1[ix])))
                    {
                        // are we at the first name, and one is a prefix of the other?
                        // if so, we treat this as edit distance 1
                        d = 1;
                    }
                    else if (d > 1 && (ix + 1) <= t1.Length)
                    {
                        // is it an initial? ie: "marius" ~ "m." or "marius" ~ "m"
                        String s1 = t1[ix];
                        String s2 = t2[ix];
                        // ensure s1 is the longer
                        if (s1.Length < s2.Length)
                        {
                            String tmp = s1;
                            s1 = s2;
                            s2 = tmp;
                        }
                        if ((s2.Length == 2 && s2[1] == '.') ||
                            s2.Length == 1)
                        {
                            // so, s2 is an initial
                            if (s2[0] == s1[0])
                                d = 1; // initial matches token in other name
                        }
                    }
                    else if (t1[ix].Length + t2[ix].Length <= 4)
                        // it's not an initial, so if the strings are 4 characters
                        // or less, we quadruple the edit dist
                        d = d*4;
                    else if (t1[ix].Length + t2[ix].Length <= 6)
                        // it's not an initial, so if the strings are 3 characters
                        // or less, we triple the edit dist
                        d = d*3;
                    else if (t1[ix].Length + t2[ix].Length <= 8)
                        // it's not an initial, so if the strings are 4 characters
                        // or less, we double the edit dist
                        d = d*2;

                    points -= d*0.1;
                }

                // if both are just one token, be strict
                if (t1.Length == 1 && points < 0.8)
                    return 0.0;

                return Math.Max(points, 0.0);
            }

            return 0.0;
        }

        #endregion
    }
}