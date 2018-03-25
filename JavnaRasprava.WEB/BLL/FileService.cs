
using JavnaRasprava.WEB.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.WindowsAzure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types


namespace JavnaRasprava.WEB.BLL
{
	public class FileService
	{
		public string UploadFile( List<string> uploadFor, string originalFileName, Stream content )
		{
			string fileName = RandomizeFileName( originalFileName );

			if ( FeatureToggle.IsBosnia() )
				LocalUpload( uploadFor, fileName, content );
			else
				UploadToBlobStorage( uploadFor, fileName, content );

			return fileName;
		}

		private void UploadToBlobStorage( List<string> uploadFor, string fileName, Stream content )
		{
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse( AppConfig.GetStringValue( "StorageConnectionString" ) );

			CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

			// Retrieve a reference to a container.
			var containername = AppConfig.GetStringValue( "CDN.BlobContainer"); 
			CloudBlobContainer container = blobClient.GetContainerReference( containername );

			container.SetPermissions( new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob } );

			// Create the container if it doesn't already exist.
			container.CreateIfNotExists();

			StringBuilder sb = new StringBuilder();
			uploadFor.ForEach( x => sb.AppendFormat( "{0}/", x ) );
			sb.Append( fileName );
			string blobreference = sb.ToString();
			CloudBlockBlob blockBlob = container.GetBlockBlobReference( blobreference );

			blockBlob.UploadFromStream( content );
		}

		private void LocalUpload( List<string> uploadFor, string fileName, Stream content )
		{
			string filePath = BuildPath( uploadFor, fileName );

			using ( var file = File.OpenWrite( filePath ) )
			{
				content.CopyTo( file );
			}
		}

		private string RandomizeFileName( string originalFileName )
		{
			string extension = Path.GetExtension( originalFileName );
			string filename = Path.GetFileName( originalFileName );

			string newFileName = GetRandomString();

			return newFileName + extension;
		}

		private string GetRandomString()
		{
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
			var random = new Random();
			var result = new string(
				Enumerable.Repeat( chars, 8 )
						  .Select( s => s[random.Next( s.Length )] )
						  .ToArray() );
			return result;
		}

		private string BuildPath( List<string> uploadFor, string originalFileName )
		{
			StringBuilder sb = new StringBuilder();
			uploadFor.ForEach( x => sb.AppendFormat( "{0}/", x ) );
			string subfolder = sb.ToString();

			string path = string.Format( AppConfig.GetStringValue( "CDN.RootPath" ), subfolder, originalFileName );

			return System.Web.Hosting.HostingEnvironment.MapPath( path );

		}


	}
}