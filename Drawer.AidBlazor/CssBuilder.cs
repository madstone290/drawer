using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawer.AidBlazor
{
    public class ClassBuilder
    {
        private string _buffer = string.Empty;

        public ClassBuilder Add(string @class)
        {
            if (_buffer == string.Empty)
                _buffer += @class;
            else
                _buffer += " " + @class;

            return this;
        }

        public ClassBuilder AddIf(string @class, bool condition)
        {
            if (!condition)
                return this;

            if (_buffer == string.Empty)
                _buffer += @class;
            else
                _buffer += " " + @class;

            return this;
        }

        public string Build()
        {
            return _buffer;
        }
    }
}
