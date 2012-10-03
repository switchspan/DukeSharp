using System;

namespace Duke.Cleaners
{
    public class NorwegianAddressCleaner : AbstractRuleBasedCleaner
    {
        #region Private member variabls

        private readonly LowerCaseNormalizeCleaner _sub;

        #endregion

        #region Constructors

        public NorwegianAddressCleaner()
        {
            _sub = new LowerCaseNormalizeCleaner();

            Add("^(co/ ?)", "c/o ");
            Add("^(c\\\\o)", "c/o");
            Add("[A-Za-z]+(g\\.) [0-9]+", "gata");
            Add("[A-Za-z]+ (gt?\\.?) [0-9]+", "gate");
            Add("[A-Za-z]+(v\\.) [0-9]+", "veien");
            Add("[A-Za-z]+ (v\\.?) [0-9]+", "vei");
            Add("[A-Za-z]+(vn\\.?)[0-9]+", "veien ");
            Add("[A-Za-z]+(vn\\.?) [0-9]+", "veien");
            Add("[A-Za-z]+(gt\\.?) [0-9]+", "gata");
            Add("[A-Za-z]+(gaten) [0-9]+", "gata");
            Add("(\\s|^)(pb\\.?) [0-9]+", "postboks", 2);
            Add("(\\s|^)(boks) [0-9]+", "postboks", 2);
            Add("[A-Za-z]+ [0-9]+(\\s+)[A-Za-z](\\s|$)", "");
            Add("[A-Za-z]+(gata|veien)()[0-9]+[a-z]?(\\s|$)", " ");

            // FIXME: not sure about the following rules
            Add("postboks\\s+[0-9]+(\\s*-\\s*)", " ");
        }

        #endregion

        #region Member methods

        public string Clean(string value)
        {
            // get rid of commas
            value = value.Replace(',', ' ');

            // do basic cleaning
            value = _sub.Clean(value);
            if (String.IsNullOrEmpty(value))
                return value;

            // perform pre-registered transforms
            value = base.Clean(value);

            return value;
        }

        #endregion
    }
}