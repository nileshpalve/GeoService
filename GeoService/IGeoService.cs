using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace GeoService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IGeoService" in both code and config file together.
    [ServiceContract]
    public interface IGeoService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/HelloName?First={First}&Last={Last}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest) ]
        string GetHelloName(string First, string Last);

        [WebInvoke(Method = "GET", UriTemplate = "/Function_BIN?BIN={BIN}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string GetBINGeocode(string BIN);

        [WebInvoke(Method = "GET", UriTemplate = "/Function_BBL?Borough={Borough}&Block={Block}&Lot={Lot}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string GetBBLGeocode(string Borough, string Block, string Lot);

        [WebInvoke(Method = "GET", UriTemplate = "/Function_1B?Borough={Borough}&AddressNo={AddressNo}&StreetName={StreetName}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string Get1BGeocode(string Borough, string AddressNo, string StreetName);

        [WebInvoke(Method = "GET", UriTemplate = "/Function_2?Borough1={Borough1}&Street1={Street1}&Borough2={Borough2}&Street2={Street2}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string Get2Geocode(string Borough1, string Street1, string Borough2, string Street2);

        [WebInvoke(Method = "GET", UriTemplate = "/Function_3?Borough1={Borough1}&OnStreet={OnStreet}&SideofStreet={SideofStreet}&Borough2={Borough2}&FirstCrossStreet={FirstCrossStreet}&Borough3={Borough3}&SecondCrossStreet={SecondCrossStreet}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string Get3Geocode(string Borough1, string OnStreet, string SideofStreet, string Borough2, string FirstCrossStreet, string Borough3, string SecondCrossStreet);

        [WebInvoke(Method = "GET", UriTemplate = "/Function_3S?Borough={Borough}&OnStreet={OnStreet}&FirstCrossStreet={FirstCrossStreet}&SecondCrossStreet={SecondCrossStreet}&RealStreetFlag={RealStreetFlag}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        string Get3SGeocode(string Borough, string OnStreet, string FirstCrossStreet, string SecondCrossStreet, string RealStreetFlag);
    }
}
