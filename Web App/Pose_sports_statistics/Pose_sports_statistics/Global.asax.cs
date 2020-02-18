using LogicCore.ConfigProvider;
using Repository;
using Repository.Redis;
using Repository.Redis.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace Pose_sports_statistics
{
	public class Global : HttpApplication
	{
		void Application_Start(object sender, EventArgs e)
		{
			// 응용 프로그램 시작 시 실행되는 코드
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			// Add Custom Routes.
			RegisterCustomRoutes(RouteTable.Routes);

			LogicStatic.Init();
			ModelStatic.InitModelMapping();

			// connect redis
			var redisCacheConfig = new ConfigurationProvider<RedisConfigurationSection>("redis_connect").Read();
			RepositoryStatic.Init_Redis(redisCacheConfig);
		}

		void RegisterCustomRoutes(RouteCollection routes)
		{
			// 축구 팀 정보
			routes.MapPageRoute(
				"FootballTeamByID",
				"football/team/{TeamID}",
				"~/Pages/FootballTeam.aspx"
			);

			// 축구 경기 정보
			routes.MapPageRoute(
				"FootballFixtureByID",
				"football/fixture/{FixtureID}",
				"~/Pages/FootballFixture.aspx"
			);

			// 축구 나라별 경기
			routes.MapPageRoute(
				"FootballByCountry",
				"football/bycountry/{CountryName}",
				"~/Pages/FootballByCountry.aspx"
			);
		}
	}
}