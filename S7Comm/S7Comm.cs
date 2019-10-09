using Sharp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S7Comm
{
    public class S7Comm
    {
        private readonly S7Client _plcConnection;
        public string ipAddress;
        private readonly System.Timers.Timer _readTimer;
        private System.Timers.Timer _toggleTimer;
        private readonly object _lockObject = new object();

        private byte[] _buffer;

        public S7Comm()
        {
            ipAddress = "172.16.0.5";
            _buffer = new byte[4];
            _plcConnection = new S7Client();

            var result = _plcConnection.ConnectTo(ipAddress, 0, 1);

            _readTimer = new System.Timers.Timer(1000);
            _readTimer.Start();
            _readTimer.Elapsed += _readTimer_Elapsed;
        }

        private void _readTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            ReadValue();
        }

        public int ReadValue()
        { 
            lock (_lockObject)
            {
                _plcConnection.DBRead(10, 0, 4, _buffer);

                int value = S7.GetIntAt(_buffer, 2);

                return value;
            }
            
        }

        public void AddValue()
        {
            lock (_lockObject)
            {
                S7.SetBitAt(ref _buffer, 0, 0, true);

                _plcConnection.DBWrite(10, 0, 4, _buffer);
                StartToggleTimer();
            }
        }


        public void SubValue()
        {
            lock (_lockObject)
            {
                S7.SetBitAt(ref _buffer, 0, 1, true);

                _plcConnection.DBWrite(10, 0, 4, _buffer);
                StartToggleTimer();
            }
        }

        public void ResetValue()
        {
            lock (_lockObject)
            {
                S7.SetBitAt(ref _buffer, 0, 2, true);

                _plcConnection.DBWrite(10, 0, 4, _buffer);
                StartToggleTimer();
            }
        }

        private void StartToggleTimer()
        {
            _toggleTimer = new System.Timers.Timer(50);
            _toggleTimer.Start();
            _toggleTimer.Elapsed += _toggleTimer_Elapsed;
        }

        private void _toggleTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _toggleTimer.Stop();

            byte data = new byte();
            data = 0x00;

            lock (_lockObject)
            {
                S7.SetByteAt(_buffer, 0, data);
                _plcConnection.DBWrite(10, 0, 4, _buffer);
            }
            _toggleTimer.Dispose();
        }
    }
}
