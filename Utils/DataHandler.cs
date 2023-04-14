using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using ArduinoApp01;

namespace ArduinoApp01.Utils
{
    internal class DataHandler : IDataHandler
    {
        private string dataReceived = "";

        public void HandelRecivedData(string data)
        {
            Task.Run(() =>
            {
                Debug.WriteLine($"Thread is : {Thread.CurrentThread.ManagedThreadId} Storing Data Thread");
                dataReceived += data;
            });

        }

        public void SaveToFile(string fileName)
        {
            Task.Run(() =>
            {
                Debug.WriteLine($"Thread is : {Thread.CurrentThread.ManagedThreadId} Saving File");
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text files (*.txt)|*.txt";
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, dataReceived);
                    MessageBox.Show("Data is saved to file");
                }
            });

        }
    }
}
