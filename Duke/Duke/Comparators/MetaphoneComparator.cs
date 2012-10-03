using System;

namespace Duke.Comparators
{
    // http://www.wbrogden.com/java/Phonetic/index.html
    // http://www.wbrogden.com/phonetic/index.html

    /// <summary>
    /// An implementation of the Metaphone algorithm, and a comparator
    /// which considers strings to have a score of 0.9 if their Metaphone
    /// values match.
    /// </summary>
    public class MetaphoneComparator : IComparator
    {
        #region Member methods

        public bool IsTokenized()
        {
            return true; // I guess?
        }

        public double Compare(string v1, string v2)
        {
            if (v1.Equals(v2))
                return 1.0;

            if (Metaphone(v1).Equals(Metaphone(v2)))
                return 0.9;

            return 0.0;
        }

        public static String Metaphone(String str)
        {
            if (str.Length < 1)
                return ""; // no metaphone key for the empty string

            str = str.ToUpper();
            var key = new char[str.Length*2]; // could be all X-es
            int pos = 0;

            for (int ix = 0; ix < str.Length; ix++)
            {
                char ch = str[ix];

                if (IsVowel(ch) && ch != 'Y')
                {
                    if (ix != 0)
                    {
                        ch = ' '; //meaning: skip
                        // Initial ae-  -> drop first letter
                    }
                    else if (ix == 0 && ch == 'A' && str.Length > 1 &&
                             str[ix + 1] == 'E')
                    {
                        ch = 'E';
                        ix++;
                    }
                }
                else
                {
                    // skip double consonant
                    if (ch != 'C' && ix + 1 < str.Length && str[ix + 1] == ch)
                        ch = str[++ix];

                    switch (ch)
                    {
                        case 'B':
                            // B -> B   unless at the end of a word after "m" as in "dumb"
                            if (ix + 1 == str.Length && ix != 0 &&
                                str[ix - 1] == 'M')
                                ch = ' '; //skip
                            break;
                        case 'C':
                            // C -> X   (sh) if -cia- or -ch-
                            //      S   if -ci-, -ce- or -cy-
                            //      K   otherwise, including -sch-

                            ch = 'K'; // default
                            if (ix > 0 && str[ix - 1] == 'S' &&
                                ix + 1 < str.Length && str[ix + 1] == 'H')
                            {
                                ix++; // skip the 'H'
                            }
                            else if (ix + 1 < str.Length)
                            {
                                char next = str[ix + 1];
                                if (next == 'I' && ix + 2 < str.Length &&
                                    str[ix + 2] == 'A')
                                {
                                    ch = 'X';
                                }
                                else if (next == 'I' || next == 'E' || next == 'Y')
                                {
                                    ch = 'S';
                                }
                                else if (next == 'H')
                                {
                                    ch = 'X';
                                    ix++; // we need to skip the H
                                }
                            }
                            break;

                        case 'D':
                            // D -> J   if in -dge-, -dgy- or -dgi-
                            //      T   otherwise

                            if (ix + 2 < str.Length &&
                                str[ix + 1] == 'G' &&
                                (str[ix + 2] == 'E' ||
                                 str[ix + 2] == 'Y' ||
                                 str[ix + 2] == 'I'))
                            {
                                ch = 'J';
                                ix += 2; // skip over next
                            }
                            else
                                ch = 'T';
                            break;

                        case 'G':
                            // G ->     silent if in -gh- and not at end or before a vowel
                            //          in -gn- or -gned- (also see dge etc. above)
                            //      J   if before i or e or y if not double gg
                            //      K   otherwise
                            // Initial  gn- pn, ae- or wr-      -> drop first letter
                            ch = 'K';
                            if (ix == 0 && str.Length > 1 && str[ix + 1] == 'N')
                                ch = ' ';
                            else if (ix + 1 < str.Length && str[ix + 1] == 'H')
                            {
                                if (ix + 2 == str.Length ||
                                    (ix + 2 < str.Length &&
                                     IsVowel(str[ix + 2])))
                                {
                                    // not at end
                                    ch = ' '; // skip
                                    ix++; // skip the 'H', too
                                }
                            }
                            else if (ix + 1 < str.Length && str[ix + 1] == 'N')
                                ch = ' '; // skip
                            else if (ix + 1 < str.Length && (str[ix + 1] == 'I' ||
                                                             str[ix + 1] == 'E' ||
                                                             str[ix + 1] == 'Y') &&
                                     (ix == 0 || str[ix - 1] != 'G'))
                                ch = 'J';

                            break;

                        case 'H':
                            // H ->     silent if after vowel and no vowel follows
                            //      H   otherwise
                            if (ix > 0 && IsVowel(str[ix - 1]) &&
                                ix + 1 < str.Length && !IsVowel(str[ix + 1]))
                                ch = ' '; // silent
                            break;

                        case 'K':
                            // K ->     silent if after "c"
                            //      K   otherwise
                            // Initial  kn-, gn- pn, ae- or wr-      -> drop first letter
                            if ((ix > 0 && str[ix - 1] == 'C') ||
                                (ix == 0 && str.Length > 1 && str[ix + 1] == 'N'))
                                ch = ' '; // silent
                            break;

                        case 'P':
                            // P -> F   if before "h"
                            //      P   otherwise
                            // Initial  pn, ae- or wr-      -> drop first letter
                            if (ix == 0 && str.Length > 1 && str[ix + 1] == 'N')
                                ch = ' ';
                            else if (ix + 1 < str.Length && str[ix + 1] == 'H')
                            {
                                ch = 'F';
                                ix++; // skip the following 'H'
                            }
                            break;

                        case 'Q':
                            ch = 'K';
                            break;

                        case 'S':
                            // S -> X   (sh) if before "h" or in -sio- or -sia-
                            //      S   otherwise
                            if ((ix + 1 < str.Length && str[ix + 1] == 'H') ||
                                (ix + 2 < str.Length && str[ix + 1] == 'I' &&
                                 (str[ix + 2] == 'O' || str[ix + 2] == 'A')))
                            {
                                ch = 'X';
                                ix++; // skip the 'H', too
                            }
                            break;

                        case 'T':
                            // T -> X   (sh) if -tia- or -tio-
                            //      0   (th) if before "h"
                            //          silent if in -tch-
                            //      T   otherwise
                            if (ix + 2 < str.Length && str[ix + 1] == 'I' &&
                                (str[ix + 2] == 'A' || str[ix + 2] == 'O'))
                                ch = 'X';
                            else if (ix + 1 < str.Length && str[ix + 1] == 'H')
                            {
                                ch = '0';
                                ix++; // skip the 'H'
                            }
                            else if (ix + 2 < str.Length && str[ix + 1] == 'C' &&
                                     str[ix + 2] == 'H')
                                ch = ' ';
                            break;

                        case 'V':
                            ch = 'F';
                            break;

                        case 'W':
                            // W ->     silent if not followed by a vowel
                            //      W   if followed by a vowel
                            // Initial  wh-                          -> change to "w"
                            // Initial  wr-      -> drop first letter
                            if (ix == 0 && str.Length > 1 && str[ix + 1] == 'H')
                                ix++; // skip the 'H'
                            else if (ix == 0 && str.Length > 1 && str[ix + 1] == 'R')
                                ch = ' '; // drop the 'W'
                            else if (ix + 1 < str.Length && !IsVowel(str[ix + 1]))
                                ch = ' ';
                            break;

                        case 'X':
                            // Initial  x-                           -> change to "s"
                            if (ix > 0)
                                key[pos++] = 'K';
                            ch = 'S';
                            break;

                        case 'Y':
                            // Y ->     silent if not followed by a vowel
                            //      Y   if followed by a vowel
                            if ((ix + 1 < str.Length && !IsVowel(str[ix + 1])) ||
                                ix + 1 == str.Length)
                                ch = ' ';
                            break;

                        case 'Z':
                            ch = 'S';

                            break;
                    }
                }

                if (ch != ' ')
                    key[pos++] = ch;
            }

            return new string(key, 0, pos);
        }


        private static Boolean IsVowel(char ch)
        {
            return (ch == 'A' || ch == 'E' || ch == 'I' || ch == 'O' || ch == 'U' ||
                    ch == 'Y');
        }

        #endregion
    }
}