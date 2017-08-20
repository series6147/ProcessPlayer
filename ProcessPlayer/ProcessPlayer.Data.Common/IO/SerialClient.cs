using System;
using System.IO.Ports;
using System.Threading;

namespace ProcessPlayer.Data.Common.IO
{
    public class SerialClient : IDisposable
    {
        #region private variables

        private Action<string> _callback;
        private bool _replyAwaiting;
        private object _syncContext;
        private SerialPort _serialPort;
        private string _response;

        #endregion

        #region public methods

        public void DiscardInBuffer()
        {
            _serialPort.DiscardInBuffer();
        }

        public string Receive()
        {
            lock (SyncContext)
            {
                var response = string.Empty;

                using (var re = new AutoResetEvent(false))
                {
                    Action<string> evt = s =>
                    {
                        if (_callback != null)
                        {
                            response = s;

                            if (re != null)
                                re.Set();
                        }
                    };

                    _callback = evt;

                    if (string.IsNullOrEmpty(response = _response))
                        WaitHandle.WaitAny(new WaitHandle[] { re }, 60000);

                    _callback = null;
                }

                _replyAwaiting = false;
                _response = null;

                return response;
            }
        }

        public void Send(string text)
        {
            lock (SyncContext)
            {
                _serialPort.WriteLine(text);

                _replyAwaiting = true;
            }
        }

        #endregion

        #region properties

        public object SyncContext
        {
            get
            {
                if (_syncContext == null)
                    _syncContext = new object();
                return _syncContext;
            }
        }

        #endregion

        #region constructors

        public SerialClient(string portName, int baudRate, Handshake handshake, Parity parity, string newLine, StopBits stopBits)
        {
            if (string.IsNullOrEmpty(portName))
                throw new ArgumentNullException("portName");

            _serialPort = new SerialPort(portName);
            _serialPort.BaudRate = baudRate;
            _serialPort.Handshake = handshake;
            _serialPort.NewLine = newLine;
            _serialPort.Parity = parity;
            _serialPort.StopBits = stopBits;
            _serialPort.DataReceived += OnSerialPort_DataReceived;
            _serialPort.Open();
        }

        public SerialClient(string portName, int baudRate, Handshake handshake, Parity parity, string newLine, StopBits stopBits, int readTimeout, int writeTimeout)
        {
            if (string.IsNullOrEmpty(portName))
                throw new ArgumentNullException("portName");

            _serialPort = new SerialPort(portName);
            _serialPort.BaudRate = baudRate;
            _serialPort.Handshake = handshake;
            _serialPort.NewLine = newLine;
            _serialPort.Parity = parity;
            _serialPort.ReadTimeout = readTimeout;
            _serialPort.StopBits = stopBits;
            _serialPort.WriteTimeout = writeTimeout;
            _serialPort.DataReceived += OnSerialPort_DataReceived;
            _serialPort.Open();
        }

        ~SerialClient()
        {
            Dispose();
        }

        #endregion

        #region events

        private void OnSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string data = _serialPort.ReadLine();

            if (_replyAwaiting)
                _response = data;

            if (_callback != null)
                _callback(_replyAwaiting ? _response : data);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            lock (SyncContext)
            {
                if (_serialPort != null)
                {
                    if (_serialPort.IsOpen)
                        _serialPort.Close();
                    _serialPort.Dispose();
                    _serialPort = null;
                }
            }
        }

        #endregion
    }
}
