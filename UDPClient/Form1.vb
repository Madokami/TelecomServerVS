Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        connectTCP()
    End Sub

    Public Sub connectTCP()
        Try
            ' Create a TcpClient.
            ' Note, for this client to work you need to have a TcpServer 
            ' connected to the same address as specified by the server, port
            ' combination.
            Dim server As String = "homuhomu"
            Dim port As Int32 = 5090
            Dim message As String = "show all users"
            Dim identity As String = System.Guid.NewGuid().ToString().Replace("-", "")
            Console.WriteLine(identity)

            Dim client As New TcpClient(server, port)
            Dim udpClient As New UdpClient(server, port)
            ' Get a client stream for reading and writing.
            '  Stream stream = client.GetStream();
            'Dim bw As System.IO.StreamWriter = New System.IO.StreamWriter()
            Dim stream As NetworkStream = client.GetStream()

            ' Translate the passed message into ASCII and store it as a Byte array.
            Dim data As [Byte]() = System.Text.Encoding.UTF8.GetBytes("identity:" & identity & vbNewLine)

            ' Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length)
            stream.Flush()

            Console.WriteLine("Sent: {0}", "identity:" + identity)

            ' Receive the TcpServer.response.
            ' Buffer to store the response bytes.
            data = New [Byte](256) {}

            ' String to store the response ASCII representation.
            Dim responseData As [String] = [String].Empty

            ' Read the first batch of the TcpServer response bytes.
            Dim bytes As Int32 = stream.Read(data, 0, data.Length)
            responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes)
            Console.WriteLine("Received: {0}", responseData)

            ' Close everything.
            stream.Close()
            client.Close()
        Catch e As ArgumentNullException
            Console.WriteLine("ArgumentNullException: {0}", e)
        Catch e As SocketException
            Console.WriteLine("SocketException: {0}", e)
        End Try

        Console.WriteLine(ControlChars.Cr + " Press Enter to continue...")
        Console.Read()
    End Sub

    Public Sub connectUDP()
        ' This constructor arbitrarily assigns the local port number.
        Dim udpClient As New UdpClient(11000)
        Try
            udpClient.Connect("www.contoso.com", 11000)

            ' Sends a message to the host to which you have connected.
            Dim sendBytes As [Byte]() = Encoding.ASCII.GetBytes("Is anybody there?")



            udpClient.Send(sendBytes, sendBytes.Length)

            ' Sends message to a different host using optional hostname and port parameters.
            Dim udpClientB As New UdpClient()
            udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000)

            ' IPEndPoint object will allow us to read datagrams sent from any source.
            Dim RemoteIpEndPoint As New IPEndPoint(IPAddress.Any, 0)

            ' UdpClient.Receive blocks until a message is received from a remote host.
            Dim receiveBytes As [Byte]() = udpClient.Receive(RemoteIpEndPoint)
            Dim returnData As String = Encoding.ASCII.GetString(receiveBytes)

            ' Which one of these two hosts responded?
            Console.WriteLine(("This is the message you received " +
                                              returnData.ToString()))
            Console.WriteLine(("This message was sent from " +
                                             RemoteIpEndPoint.Address.ToString() +
                                             " on their port number " +
                                             RemoteIpEndPoint.Port.ToString()))
            udpClient.Close()
            udpClientB.Close()

        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Sub
End Class


