// A minimal protocol peer for offline driver integration checks, bound only to loopback.
using System.Buffers.Binary;
using System.Net;
using System.Net.Sockets;

internal sealed class LoopbackModbusServer : IAsyncDisposable
{
    private readonly TcpListener _listener = new(IPAddress.Loopback, 0);
    private readonly CancellationTokenSource _stop = new();
    private readonly Task _worker;
    private int _value = 123;
    public int Value => Volatile.Read(ref _value);
    public int Port => ((IPEndPoint)_listener.LocalEndpoint).Port;

    public LoopbackModbusServer() { _listener.Start(); _worker = RunAsync(); }

    private async Task RunAsync()
    {
        try
        {
            while (!_stop.IsCancellationRequested)
            {
                using var client = await _listener.AcceptTcpClientAsync(_stop.Token);
                using var stream = client.GetStream();
                try
                {
                    while (!_stop.IsCancellationRequested)
                    {
                        var header = new byte[7];
                        await stream.ReadExactlyAsync(header, _stop.Token);
                        var bodyLength = BinaryPrimitives.ReadUInt16BigEndian(header.AsSpan(4, 2)) - 1;
                        if (bodyLength is < 5 or > 253) throw new IOException("Invalid Modbus frame length.");
                        var body = new byte[bodyLength];
                        await stream.ReadExactlyAsync(body, _stop.Token);
                        byte[] response;
                        if (body[0] == 3)
                        {
                            var count = BinaryPrimitives.ReadUInt16BigEndian(body.AsSpan(3, 2));
                            if (count is < 1 or > 125) throw new IOException("Invalid register count.");
                            response = new byte[2 + count * 2];
                            response[0] = 3; response[1] = (byte)(count * 2);
                            BinaryPrimitives.WriteUInt16BigEndian(response.AsSpan(2, 2), (ushort)Value);
                        }
                        else if (body[0] == 16 && body.Length >= 8)
                        {
                            Volatile.Write(ref _value, BinaryPrimitives.ReadUInt16BigEndian(body.AsSpan(6, 2)));
                            response = body[..5];
                        }
                        else response = [(byte)(body[0] | 0x80), 1];
                        BinaryPrimitives.WriteUInt16BigEndian(header.AsSpan(4, 2), (ushort)(response.Length + 1));
                        await stream.WriteAsync(header, _stop.Token);
                        await stream.WriteAsync(response, _stop.Token);
                    }
                }
                catch (EndOfStreamException) { }
                catch (IOException) when (!_stop.IsCancellationRequested) { }
            }
        }
        catch (OperationCanceledException) when (_stop.IsCancellationRequested) { }
        catch (SocketException) when (_stop.IsCancellationRequested) { }
        // A pending accept can surface as ObjectDisposedException when the listener is stopped
        // before the cancellation token is observed.
        catch (ObjectDisposedException) when (_stop.IsCancellationRequested) { }
    }

    public async ValueTask DisposeAsync()
    {
        await _stop.CancelAsync();
        _listener.Stop();
        await _worker;
        _stop.Dispose();
    }
}
