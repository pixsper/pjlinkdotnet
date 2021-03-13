using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pixsper.PJLinkDotNet
{
    public sealed class PJLinkClient : IDisposable
    {
        public const int PJLinkTcpPort = 4352;

        private readonly TcpClient _client = new TcpClient();
        private StreamReader? _streamReader;

        public PJLinkClient(IPAddress serverIp, int port = PJLinkTcpPort)
        {
            ServerIp = serverIp;
            Port = port;
        }

        public PJLinkClient(IPAddress serverIp, string password, int port = PJLinkTcpPort)
        {
            ServerIp = serverIp;
            Password = password;
            Port = port;
        }

        public void Dispose()
        {
            _client.Dispose();
        }


        public IPAddress ServerIp { get; }

        public int Port { get; }

        public bool IsAuthenticationEnabled => Password != null;

        public string Password { get; }


        public Task<bool> ConnectAsync() => connectAsyncImpl();

        public async Task<bool> SendCommandAsync(CommandBase command)
        {
            if (!_client.Connected)
            {
                await connectAsyncImpl(command);
            }
            else
            {
            }

            return false;
        }

        private async Task<bool> connectAsyncImpl(CommandBase command = null)
        {
            if (_client.Connected)
                return true;

            await _client.ConnectAsync(ServerIp, Port).ConfigureAwait(false);

            if (!_client.Connected)
                return false;

            _streamReader = new StreamReader(_client.GetStream(), Encoding.UTF8);

            if (IsAuthenticationEnabled)
            {

            }
            else if (command != null)
            {
                var buffer = serializeCommand(command);
                await _client.GetStream().WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
            }

            return _client.Connected;
        }

        private byte[] serializeCommand(CommandBase command) => Encoding.ASCII.GetBytes(command.ToString());
    }
}
