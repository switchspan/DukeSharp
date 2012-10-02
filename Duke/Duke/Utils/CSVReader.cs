using System;
using System.IO;

namespace Duke.Utils
{
    public class CSVReader
    {
        private char[] _buf;
        private StreamReader _sr;
        private int _len;
        private int _pos; // where we are in the buffer
        private string[] _tmp;

        public CSVReader(StreamReader sr)
        {
            _buf = new char[65356];
            _pos = 0;
            _len = sr.Read(_buf, 0, _buf.Length);
            _tmp = new string[1000];
            _sr = sr;
        }

        // this is used for testing!
        public CSVReader(StreamReader sr, int buflen)
        {
            _buf = new char[buflen];
            _pos = 0;
            _len = sr.Read(_buf, 0, _buf.Length);
            _tmp = new string[1000];
            _sr = sr;
        }

        public string[] Next()
        {
            if (_len == -1 || _pos >= _len)
                return null;

            int colno = 0;
            int rowstart = _pos; // used for rebuffering at end
            int prev = _pos - 1;
            bool escaped_quote = false; //did we find an escaped quote?
            while (_pos < _len)
            {
                bool startquote = false;
                if (_buf[_pos] == '\"')
                {
                    startquote = true;
                    prev++;
                    _pos++;
                }

                // scan forward, looking for the end of the string
                while(true)
                {
                    while(_pos < _len &&
                        (startquote || _buf[_pos] != ',') &&
                        (startquote || (_buf[_pos] != '\n' && _buf[_pos] != '\r')) &&
                        !(startquote && _buf[_pos] == '\"'))
                    _pos++;

                    if (_pos + 1 >= _len ||
                        (!(_buf[_pos] == '\"' && _buf[_pos + 1] != '\"'))) 
                        break; // we found the end of this value, so stop
                    else
                    {
                        // found a "". carry on
                        escaped_quote = true;
                        _pos += 2; // step to the character after next
                    }
                }

                if (escaped_quote)
                    _tmp[colno++] = Unescape(new string(_buf, prev + 1, _pos - prev - 1));
                else
                {
                    _tmp[colno++] = new string(_buf, prev + 1, _pos - prev - 1);
                }

                if (startquote)
                    _pos++; // step over the '"'
                prev = _pos;

                if (_pos >= _len)
                    break; // jump out of the loop to rebuffer and try again

                if (_buf[_pos] == '\r' || _buf[_pos] == '\n')
                {
                    _pos++; // step over the \r or \n
                    if (_pos >= _len)
                        break; // jump out of the loop to rebuffer and try again
                    if (_buf[_pos] == '\n')
                        _pos++; // step over this, too
                    break; // we're done
                }

                _pos++;
            }

            if (_pos >= _len)
            {
                // this means we've exhausted the buffer. that again means we've
                // read the entire stream, or we need to fill up the buffer.
                Array.Copy(_buf, rowstart, _buf, 0, _len - rowstart);
                _len = _len - rowstart;
                int read = _sr.Read(_buf, _len, _buf.Length - _len);
                if (read != -1)
                {
                    _len += read;
                    _pos = 0;
                    return Next();
                }
                else
                {
                    _len = -1;
                }
            } 

            var row = new string[colno];
            for (int ix = 0; ix < colno; ix++)
            {
                row[ix] = _tmp[ix];
            }

            return row;
        }


        public void Close()
        {
            _sr.Close();
        }

        private string Unescape(string val)
        {
            return val.Replace("\"\"", "\"");
        }
    }
}