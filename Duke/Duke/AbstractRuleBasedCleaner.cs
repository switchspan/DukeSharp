using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Helper class for building regular-expression based cleaners.
    /// </summary>
    public abstract class AbstractRuleBasedCleaner : ICleaner
    {
        private List<Transform> _transforms; 

        /// <summary>
        /// Initializes and emply cleaner.
        /// </summary>
        public AbstractRuleBasedCleaner()
        {
            _transforms = new List<Transform>();
        }


        public string Clean(string value)
        {
            // perform pre-registered transforms
            foreach (Transform t in _transforms)
            {
                value = t.TransformValue(value);
            }

            return value;
        }

        /// <summary>
        /// Adds a rule for replacing all substrings matching the regular
        /// expression with the replacement string.
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="replacement"></param>
        public void Add(String regex, String replacement)
        {
            Add(regex, replacement, 1);
        }

        /// <summary>
        /// Adds a rule replacing all substrings matching the specified group
        /// within the regular expression with the replacement string
        /// </summary>
        /// <param name="regex"></param>
        /// <param name="replacement"></param>
        /// <param name="groupno"></param>
        public void Add(String regex, String replacement, int groupno)
        {
            _transforms.Add(new Transform(regex, replacement, groupno));
        }

    }
}
