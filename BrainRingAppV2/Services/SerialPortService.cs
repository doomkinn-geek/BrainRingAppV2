using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace BrainRingAppV2.Services
{
    public class SerialPortService
    {
        private SerialPort _serialPort;

        public bool IsOpen { get => _serialPort.IsOpen; }

        public event EventHandler<string> DataReceived;

        public SerialPortService()
        {
            _serialPort = new SerialPort
            {
                BaudRate = 19200,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One
            };
        }

        public void OpenPort(string portName)
        {
            _serialPort.PortName = portName;
            _serialPort.Open();
            StartListening();
        }

        public void ClosePort()
        {
            _serialPort.Close();
        }

        private void StartListening()
        {
            Task.Run(() =>
            {
                while (_serialPort.IsOpen)
                {
                    try
                    {
                        string receivedData = _serialPort.ReadLine();
                        DataReceived?.Invoke(this, receivedData);
                    }
                    catch (TimeoutException)
                    {
                        // Обработка исключений
                        continue;
                    }
                    catch(Exception)
                    {
                        continue;
                    }
                }
            });
        }
    }
}
