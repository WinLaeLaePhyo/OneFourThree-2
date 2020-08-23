using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.Linq;
using System.IO;
namespace OneFourThree.App_Code
{
    public class ElementDataList
    {
        DBBack db = new DBBack(); ParameterBack pb = new ParameterBack();
        public string Element1 { get; set; }
        public string Element2 { get; set; }
        public string Element3 { get; set; }
        public string Element4 { get; set; }
        public string Element5 { get; set; }
        public string Element6 { get; set; }
        public string Element7 { get; set; }
        public string Element8 { get; set; }
        public string Element9 { get; set; }
        public string Element10 { get; set; }

        //,convert(nvarchar(max),decryptByPassPhrase('root@',Phone)) as P,convert(nvarchar(max),decryptByPassPhrase('root@',Email)) as E

        #region GetLover
        public List<ElementDataList> GetLover(double sLatitude, double sLongitude, string Gender, int Age)
        {
            var dataList = new List<ElementDataList>();
            ElementDataList mg;
            try
            {
                    DataTable dt = db.getAllByQuery("select * from Lover where Gender='"+Gender+"' and Age="+Age);
                    #region Looping Lover
                    foreach (DataRow rows in dt.Rows)
                    {
                        //if (GetDistance(sLatitude, sLongitude, Convert.ToDouble(rows["Lat"]), Convert.ToDouble(rows["Lng"])) < 3)
                        //{
                            mg = new ElementDataList
                            {
                                Element1 = rows["ID"].ToString(),
                                Element2 = rows["Lat"].ToString(),
                                Element3 = rows["Lng"].ToString()
                            };
                            dataList.Add(mg);
                        //}
                    }
                    #endregion
                return dataList;
            }
            catch
            {
                return dataList;
            }
        }
        #endregion

        #region GetTown
        public List<ElementDataList> GetTownByStateRegionID(int StateRegionID)
        {
            var dataList = new List<ElementDataList>();
            ElementDataList mg;
            DataTable dt = db.getAllByQuery("select * from Town where StateRegionID=" + StateRegionID);
            foreach (DataRow rows in dt.Rows)
            {
                mg = new ElementDataList
                {
                    Element1 = rows["ID"].ToString(),
                    Element2 = rows["MName"].ToString(),
                    Element3 = rows["EName"].ToString()
                };
                dataList.Add(mg);
            }
            return dataList;
        }
        #endregion

        public Double GetDistance(double sLatitude, double sLongitude, double eLatitude, double eLongitude)
        {
            var sCoord = new GeoCoordinate(sLatitude, sLongitude);
            var eCoord = new GeoCoordinate(eLatitude, eLongitude);
            double DistanceInMeter = sCoord.GetDistanceTo(eCoord);
            double DistanceInMile = DistanceInMeter * 0.000621371;
            return DistanceInMile;
        }
    }
}