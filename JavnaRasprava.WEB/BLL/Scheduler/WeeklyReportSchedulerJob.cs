using JavnaRasprava.WEB.DomainModels;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JavnaRasprava.WEB.BLL.Scheduler
{
	public class WeeklyReportSchedulerJob : IJob
	{

		public void Execute( IJobExecutionContext jobContext )
		{
			new NotificationService().ProcessWeeklyReport();
			return;
		}
	}
}