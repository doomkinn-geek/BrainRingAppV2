using System;
using System.IO.Ports;
using System.Threading.Tasks;

namespace BrainRingAppV2.Services
{
    public class SerialPortService
    {
        private SerialPort _serialPort;
        private string _keptData;

        public bool IsOpen { get => _serialPort.IsOpen; }

        public event EventHandler<string> DataReceived;
        public event EventHandler<string> ErrorOcured;

        public SerialPortService()
        {
            try
            {
                _serialPort = new SerialPort
                {
                    BaudRate = 19200,
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One
                };
            }
            catch(Exception ex) 
            {
                ErrorOcured?.Invoke(this, ex.Message);
            }
        }

        public void OpenPort(string portName)
        {
            try
            {
                _serialPort.PortName = portName;
                _serialPort.Open();
                StartListening();
            }
            catch (Exception ex)
            {
                ErrorOcured?.Invoke(this, ex.Message);
            }
        }

        public void ClosePort()
        {
            try
            {
                _serialPort.Close();
            }
            catch(OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                ErrorOcured?.Invoke(this, ex.Message);
            }
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
                        if (receivedData != _keptData)
                        {
                            DataReceived?.Invoke(this, receivedData);
                            _keptData = receivedData;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        return;
                    }
                    catch (TimeoutException)
                    {
                        // Обработка исключений
                        continue;
                    }
                    catch(Exception ex)
                    {
                        ErrorOcured?.Invoke(this, ex.Message);
                        continue;
                    }
                }
            });
        }
    }
}
