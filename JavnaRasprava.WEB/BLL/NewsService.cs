using JavnaRasprava.WEB.DomainModels;
using JavnaRasprava.WEB.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityFramework.Extensions;
using PagedList;

namespace JavnaRasprava.WEB.BLL
{
	public class NewsService
	{
		public NewsModel Get( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				return GetInternal( id, context );
			}
		}

		internal static NewsModel GetInternal( int id, ApplicationDbContext context )
		{
			return context.News
				.Where( x => x.NewsId == id )
				.Select( x => new NewsModel
				{
					NewsId = x.NewsId,
					Title = x.Title,
					Summary = x.Summary,
					Text = x.Text,
					ParliamentId = x.ParliamentId,
					CreateDateTimeUtc = x.CreateDateTimeUtc,
					ImageRelativePath = x.ImageRelativePath
				} )
				.FirstOrDefault();
		}

		internal NewsListModel InitializeSearchModel(int parliamentId )
		{
			var searchModel = new NewsSearchModel
			{
				ParliamentId = parliamentId
			};
			var resultModel = SearchNews( searchModel );
			return new NewsListModel
			{
				SearchModel = new NewsSearchModel(),
				Results = resultModel
			};
		}


		internal IPagedList<Models.News.NewsModel> SearchNews( NewsSearchModel searchModel )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var query = context.News
					.Where( x => 1 == 1 );

				if ( searchModel.ParliamentId != 0 )
					query = query.Where( x => x.ParliamentId == searchModel.ParliamentId );

				if ( searchModel.QueryString != null )
					query = query.Where( x => x.Title.Contains( searchModel.QueryString ) ||
					x.Summary.Contains( searchModel.QueryString ) || x.Text.Contains( searchModel.QueryString ) );



				switch ( searchModel.Sort ?? NewsSort.CreateTime )
				{
					case NewsSort.CreateTime:
						query = query.OrderBy( x => x.CreateDateTimeUtc );
						break;
					case NewsSort.Title:
						query = query.OrderBy( x => x.Title );
						break;
				}

				var result = query.Select( x => new NewsModel
				{
					NewsId = x.NewsId,
					Title = x.Title,
					Summary = x.Summary,
					Text = x.Text,
					ParliamentId = x.ParliamentId,
					CreateDateTimeUtc = x.CreateDateTimeUtc,
					ImageRelativePath = x.ImageRelativePath
				} )
				.ToPagedList( searchModel.page.HasValue ? searchModel.page.Value : 1, 16 );

				return result;
			}
		}

		internal static List<NewsModel> GetNewsListInternal( ApplicationDbContext context, List<int> ids )
		{
			return context.News
				.Where( x => ids.Contains( x.NewsId ) )
				.Select( x => new NewsModel
				{
					NewsId = x.NewsId,
					Title = x.Title,
					Summary = x.Summary,
					Text = x.Text,
					ParliamentId = x.ParliamentId,
					ImageRelativePath = x.ImageRelativePath
				} )
				.ToList();
		}

		public List<NewsModel> GetAll( int parliamentId )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				return context.News
					.Where( x => x.ParliamentId == parliamentId )
					.Select( x => new NewsModel
					{
						NewsId = x.NewsId,
						Title = x.Title,
						Summary = x.Summary,
						Text = x.Text,
						ParliamentId = x.ParliamentId,
						ImageRelativePath = x.ImageRelativePath
					} )
					.ToList();
			}
		}

		public NewsModel InitCreate( int parliamentId )
		{
			if ( parliamentId < 1 )
				return null;

			return new NewsModel
			{
				ParliamentId = parliamentId
			};
		}

		public int Create( NewsModel model )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				var parliament = context.Parliaments.SingleOrDefault( x => x.ParliamentID == model.ParliamentId );

				if ( parliament == null )
					return -1;

				var news = new News
				{
					NewsId = model.NewsId,
					Title = model.Title,
					Summary = model.Summary,
					Text = model.Text,
					ParliamentId = model.ParliamentId
				};

				if ( model.Image != null )
				{
					var fileService = new FileService();
					news.ImageRelativePath = fileService.UploadFile( new List<string> { "NewsImage" }, model.Image.FileName, model.Image.InputStream );

				}

				context.News.Add( news );

				context.SaveChanges();

				return model.NewsId;
			}
		}

		public NewsModel Update( NewsModel model )
		{
			using ( var context = JavnaRasprava.WEB.DomainModels.ApplicationDbContext.Create() )
			{
				var existing = context.News
					.Where( x => x.NewsId == model.NewsId )
					.FirstOrDefault();

				if ( existing == null )
					return null;

				existing.Title = model.Title;
				existing.Summary = model.Summary;
				existing.Text = model.Text;

				if ( model.Image != null )
				{
					var fileService = new FileService();
					existing.ImageRelativePath = fileService.UploadFile( new List<string> { "NewsImage" }, model.Image.FileName, model.Image.InputStream );

				}
				else
				{
					existing.ImageRelativePath = model.ImageRelativePath;
				}

				context.SaveChanges();

				return GetInternal( model.NewsId, context );
			}
		}




		public void Delete( int id )
		{
			using ( var context = ApplicationDbContext.Create() )
			{
				new InfoBoxService().DeleteItems( id, InfoBoxItemType.News, context );

				context.News.Where( x => x.NewsId == id ).Delete();
				context.SaveChanges();
			}
		}
	}
}