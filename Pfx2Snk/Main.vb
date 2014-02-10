Imports System.Security.Cryptography.X509Certificates
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Reflection

Module Main

	Function Main(args As String()) As Integer
		If (args.Length <> 2) Then
			Console.WriteLine("USAGE: {0} path-to.pfx path-to.snk", Path.GetFileName(New Uri(Assembly.GetExecutingAssembly().Location).LocalPath))
			Return 2
		End If

		Dim pfxPath = args(0)
		Dim snkPath = args(1)

		If (Not File.Exists(pfxPath)) Then
			Console.WriteLine("File not found: {0}", pfxPath)
			Return 1
		End If

		Console.Write("Enter PFX password: ")
		Dim password As New SecureString
		Dim ch As ConsoleKeyInfo
		Do
			ch = Console.ReadKey(intercept:=True)
			If (ch.Key <> ConsoleKey.Enter) Then
				password.AppendChar(ch.KeyChar)
			End If
		Loop While ch.Key <> ConsoleKey.Enter

		Console.WriteLine()
		Console.WriteLine("Reading {0} file...", pfxPath)
		Dim cert As New X509Certificate2(pfxPath, password, X509KeyStorageFlags.Exportable)
		Dim key = DirectCast(cert.PrivateKey, RSACryptoServiceProvider)

		Console.WriteLine("Writing {0} file...", snkPath)
		Dim snkContent = key.ExportCspBlob(True)
		File.WriteAllBytes(snkPath, snkContent)

		Console.WriteLine("Complete.")
		Return 0
	End Function

End Module
