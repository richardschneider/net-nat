using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Makaretu.Nat
{
    class MockNat : IDisposable
    {
        public event EventHandler<UdpReceiveResult> RequestReceived;
        public readonly IPAddress Address = IPAddress.Loopback;
        UdpClient udp;

        public MockNat(int port = NatPmp.RequestPort)
        {
            udp = new UdpClient(port);
            Listener();
        }

        async void Listener()
        {
            try
            {
                while (udp != null)
                {
                    var result = await udp.ReceiveAsync();
                    RequestReceived?.Invoke(this, result);
                }
            }
            catch (Exception e)
            {
                // eat the exception
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (udp != null)
                    {
                        udp.Close();
                        udp = null;
                    }
                    RequestReceived = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
