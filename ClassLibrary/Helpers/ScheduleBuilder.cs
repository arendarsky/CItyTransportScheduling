using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Interfaces;

namespace ClassLibrary.Helpers
{
    public class ScheduleBuilder
    {
        List<Route> Routes { get; set; }
        Station station { get; set; }
        Route route { get; set; }
        public ScheduleBuilder(List<Route> routes, Station station, Route route)
        {
            Routes = routes;
            this.station = station;
            this.route = route;
        }
        public void GetSchedule(out List<ScheduleItem> result)
        {
            result = new List<ScheduleItem>();
            DateTime currentDt = DateTime.Now;
            List<Route> routes;
            if (route.Name == "Select Route")
            {
                routes = Routes;
            }
            else
            {
                routes = new List<Route>() { route };
            }
            foreach (var r in routes)
            {
                var routeStation = r.Stations
                    .FirstOrDefault(st => st.StationId == station.Id);
                if (routeStation != null)
                {
                    if (routeStation != r.Stations.Last())
                    {
                        int left = r.TimeToNextDepartureFromOrigin(routeStation, currentDt);
                        result.Add(new ScheduleItem
                        {
                            RouteName = r.Name,
                            Destination = r.Stations.Last().Station.Name,
                            MinutesLeft = left
                        });
                    }
                    if (routeStation != r.Stations.First())
                    {
                        int left = r.TimeToNextDepartureFromDest(routeStation, currentDt);
                        result.Add(new ScheduleItem
                        {
                            RouteName = r.Name,
                            Destination = r.Stations.First().Station.Name,
                            MinutesLeft = left
                        });
                    }
                }
            }
        }
    }
}
