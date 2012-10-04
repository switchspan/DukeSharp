using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net;
using Lucene.Net.Search;

namespace Duke
{
    /// <summary>
    /// These objects are used to estimate the size of the query result
    /// we should ask Lucene for. This parameter is the single biggest influence 
    /// on matching performance, but setting it too low causes matches to be missed. 
    /// We therefore try hard to estimate it as correctly as possible.
    /// 
    /// The reason this is a separate class is that we used to need one of these 
    /// for every property because the different properties will behave differently.
    /// 
    /// FIXME: the class is badly named
    /// FIXME: the class is not thread-safe
    /// </summary>
    public class QueryResultTracker
    {
        #region Private member variables

        private int _limit;
        // Ring buffer containing n last search result sizes, except for
        // searches which found nothing.
        private int[] _prevsizes;
        private int sizeix; // position in prevsizes

        private Configuration _config;
        #endregion

        #region Constructors
        public QueryResultTracker(Configuration config)
        {
            _limit = 100;
            _prevsizes = new int[10];
            _config = config;
        }
        #endregion

        #region Member methods
        public List<IRecord> Lookup(IRecord record)
        {
            // first we build the combined query for all lookup properties
            BooleanQuery query = new BooleanQuery();
           

        #endregion
    }
}
