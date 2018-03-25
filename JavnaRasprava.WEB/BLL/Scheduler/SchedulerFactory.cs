using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL.Scheduler
{
	public class SchedulerFactory
	{
		public static IScheduler GetScheduler()
		{
			NameValueCollection nvc = new NameValueCollection();
			nvc.Add( "quartz.scheduler.instanceName", "ADO" );
			nvc.Add( "quartz.threadPool.threadCount", "3" );
			nvc.Add( "quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" );
			nvc.Add( "quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.StdAdoDelegate, Quartz" );
			nvc.Add( "quartz.jobStore.tablePrefix", "QRTZ_" );
			nvc.Add( "quartz.jobStore.dataSource", "myDS" );
			nvc.Add( "quartz.dataSource.myDS.connectionString", ConfigurationManager.ConnectionStrings["default"].ConnectionString );
			nvc.Add( "quartz.dataSource.myDS.provider", "SqlServer-20" );
			nvc.Add( "quartz.jobStore.useProperties", "true" );

						
			return new StdSchedulerFactory( nvc ).GetScheduler();
		}
	}
}