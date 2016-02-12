using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Intranet.Models;

namespace Intranet.Utils
{
    public class RequestType
    {       
        public static string GetDetailsLink(Request r)
        {
            string rId = r.RequestId.ToString();
            if (r.RequestPassenger != null)
                return "<a href=\"/Passenger/Details/" + rId + "\">" + rId + "</a>";
            if (r.RequestFreight != null)
                return "<a href=\"/Freight/Details/" + rId + "\">" + rId + "</a>"; ;
            if (r.RequestCrane != null)
                return "<a href=\"/Crane/Details/" + rId + "\">" + rId + "</a>"; ;
            return "";
        }

        public static string GetDetailsButton(Request r)
        {
            string rId = r.RequestId.ToString();
            if (r.RequestPassenger != null)
                return "<a class=\"btn btn-mini\" href=\"/Passenger/Details/" + rId + "\">Просмотр</a>";
            if (r.RequestFreight != null)
                return "<a class=\"btn btn-mini\" href=\"/Freight/Details/" + rId + "\">Просмотр</a>"; 
            if (r.RequestCrane != null)
                return "<a class=\"btn btn-mini\" href=\"/Crane/Details/" + rId + "\">Просмотр</a>";
            return ""; 
        }

        public static string GetRequestTypeLabel(Request r)
        {
            string rId = r.RequestId.ToString();
            if (r.RequestPassenger != null)
                return "<img src='/Content/images/Car_Grey.png' title='Пассажирский' class='myTooltip'/>";
            if (r.RequestFreight != null)
                return "<img src='/Content/images/Truck_Yellow.png' title='Грузовой' class='myTooltip'/>";
            if (r.RequestCrane != null)
                return "<img src='/Content/images/TruckMountedCrane_Yellow.png' title='Грузоподъемный кран' class='myTooltip'/>";
            return ""; 
        }
    }
}