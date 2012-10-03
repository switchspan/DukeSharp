using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// x = the sound of both sj and kj
// ^ == start of string
// $ == end of string
// [abc] == a set of characters (as in regexp)

// IMPLEMENTED
// vowels stripped, except initial vowel
// double consonants collapse into one
// ^aa -> å
// ch -> k   
// ck -> k
// [oiuaeæødy]d -> 
// dt$ -> t
// gh -> k
// gj -> j
// ^gi -> j
// hg -> k
// hj -> j
// hl -> l
// hr -> r
// kj -> x
// ki -> x
// ld -> l
// nd -> n
// ph -> f
// th -> t
// w -> v
// x -> ks
// z -> s

// NOT IMPLEMENTED
// ^c -> k 
// sj -> x
// skj -> x
// ^ei -> æ
// d -> t
// g -> k
// kei -> x
// skei -> x
// ^ky -> x
// ^sky -> x

// NOT SURE ABOUT THESE
// ^ch[aeiouy] -> x  (charlotte)
// en$ -> 

namespace Duke.Comparators
{
    /// <summary>
    /// Lars Marius Garshol's own algorithm for phonetic matching of Norwegian names,
    /// inspired by Metaphone.
    /// </summary>
    public class NorphoneComparator : IComparator
    {
        #region Private member variables
        //TODO: Finish this class.
        #endregion

        #region Constructors

        #endregion

        #region Member methods
        public bool IsTokenized()
        {
            throw new NotImplementedException();
        }

        public double Compare(string v1, string v2)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
