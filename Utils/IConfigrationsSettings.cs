using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoApp01.Utils
{
    internal interface IConfigrationsSettings
    {
        string[] Bauds { get; }
        string[] DataBits { get; }
        string[] StopBits { get; }
        string[] ParityBits { get; }
        SerialPort Device { get; }
    }
}
