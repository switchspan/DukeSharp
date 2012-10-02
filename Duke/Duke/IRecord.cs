using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duke
{
    /// <summary>
    /// Represents a record, which may be a single source record from a
    /// data source, or a record created from merging dat from many records.
    /// </summary>
    public interface IRecord
    {
        /// <summary>
        /// The names of the properies this record has. May be a subset of
        /// the properies defined in the configuration if not all properties
        /// have values.
        /// </summary>
        /// <returns></returns>
        List<String> GetProperties();

        //HACK: Lists are actually Collection classes in the java code - need to check this.

        /// <summary>
        /// All values for the named property. may be empty. May not contain
        /// null or empty strings.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        List<String> GetValues(String prop);

        /// <summary>
        /// Returns a value for the named property. May be null. 
        /// May not be empty space. There may be other values which are not 
        /// returned, and there is no way to predict which value is returned.
        /// </summary>
        /// <param name="prop"></param>
        /// <returns></returns>
        String GetValue(String prop);

        /// <summary>
        /// Merges the other record into this one. None of the 
        /// implementations support this method yet, but it's 
        /// going to be used later.
        /// </summary>
        /// <param name="other"></param>
        void Merge(IRecord other);

    }
}
