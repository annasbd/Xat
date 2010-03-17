using System;
using System.Net.Sockets;
using System.IO;

namespace XatServer
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Hola, sóc el servidor!");
			
			Server servidor = new Server(9898);
			
			if (!servidor.Start())
			{
				Console.WriteLine("No puc engegar el servidor!");
			}
			
			if (servidor.WaitForAClient())
			{
				// Escribim tot el que ens envii el client
				Console.WriteLine("El client diu: " + servidor.ReadLine()); 
				
				// server.WriteLine("Hi!"); 
			}
		}
	}
	
	public class Server
	{
		private StreamReader reader;
		private StreamWriter writer;
		private int port;
		private TcpListener listener;
		
		public Server(int port)
		{
			this.port = port;
		}
		
		public string ReadLine()
		{
			return reader.ReadLine();
		}
		
		public void WriteLine(string str)
		{
			writer.WriteLine(str);
			writer.Flush();
		}
		
		public bool Start()
		{
			try
			{
				listener = new TcpListener(port);
				listener.Start(); //start server
			}
			catch(Exception e)
			{
				Console.WriteLine(e.StackTrace);
				return false;
			}
		
			Console.WriteLine("Servidor engegat, escoltant al port {0}", port);
			
			return true;
		}
		
		public bool WaitForAClient()
		{
			// Esperem una connexio d'un client
			Socket serverSocket = listener.AcceptSocket();
			
			try
			{
				if (serverSocket.Connected)
				{
					NetworkStream netStream = new NetworkStream(serverSocket);
					
					writer = new StreamWriter(netStream);
					reader = new StreamReader(netStream);
				}
			}
			catch(Exception e)
			{
				Console.WriteLine(e.StackTrace);
				return false;
			}

			Console.WriteLine("Un client s'ha connectat!");
					
			return true;
		}
	}
}
