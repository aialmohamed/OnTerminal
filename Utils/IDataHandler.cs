using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoApp01.Utils
{
    internal interface IDataHandler
    {
        void HandelRecivedData(string data);
        void SaveToFile(string filename);
    }
}
