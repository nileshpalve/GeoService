using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using DCP.Geosupport.DotNet.GeoX;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Xml;
using System.Text.RegularExpressions;
using DCP.Geosupport.DotNet.fld_def_lib;
using Newtonsoft.Json;

namespace GeoService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "GeoService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select GeoService.svc or GeoService.svc.cs at the Solution Explorer and start debugging.
    public class GeoService : IGeoService
    {
        //public static GeoConnCollection mygeoconns = new GeoConnCollection(System.Web.HttpContext.Current.Server.MapPath("Geoconns.xml"));
        public geo mygeo = new geo();
        public struct jsonoutputBN
        {
            public string in_func_code;
            public string in_bin;
            public string out_grc;
            public string out_grc2;
            public string out_error_message;
            public string out_bbl_boro;
            public string out_bbl_block;
            public string out_bbl_lot;
            public string out_bbl_bbl;
            public string out_num_of_blockfaces;
            public string out_sanborn_boro;
            public string out_sanborn_volume;
            public string out_sanborn_page;
            public string out_x_coord;
            public string out_y_coord;
            public string out_latitude;
            public string out_longitude;
            public string out_vacant_flag;
            public string out_condo_flag;
            public string out_low_bbl_condo;
            public string out_high_bbl_condo;
            public string out_bin;
            public string out_tpad_bin_status;
            public string out_corner_code;
            public string out_num_of_bldgs;
            public string out_bid_id;
            public string out_rpad_scc;
            public string out_rpad_bldg_class;
            public string out_interior_flag;
            public string out_irreg_flag;
            public string out_condo_num;
            public string out_coop_num;
            public string out_tax_map;
            public string out_tax_section;
            public string out_tax_volume;

        }
        string IGeoService.GetBINGeocode(string BIN)
        {


            // work area 1 
            Wa1 mywa1 = new Wa1();
            Wa1 mywa1_stname = new Wa1();

            // work area 2
            Wa2F1ax mywa2f1ax = new Wa2F1ax();

            mywa1.in_bin.boro = BIN.Trim().Substring(0, 1);
            mywa1.in_bin.binnum = BIN.Trim().Substring(1, 6);

            mywa1.in_mode_switch = "X";
            mywa1.in_func_code = "BN";
            mywa1.in_platform_ind = "C";
            mywa1.in_tpad_switch = "Y";

            // this call gets BN info
            mygeo.GeoCall(ref mywa1, ref mywa2f1ax);

            jsonoutputBN jsonoutputStrBN = new jsonoutputBN();
            jsonoutputStrBN.in_func_code = mywa1.in_func_code;
            jsonoutputStrBN.in_bin = mywa1.in_bin_string;
            jsonoutputStrBN.out_grc = mywa1.out_grc;
            jsonoutputStrBN.out_grc2 = mywa1.out_grc2;
            jsonoutputStrBN.out_error_message = mywa1.out_error_message;
            jsonoutputStrBN.out_bbl_block = mywa2f1ax.bbl.block;
            jsonoutputStrBN.out_bbl_lot = mywa2f1ax.bbl.lot;
            jsonoutputStrBN.out_bbl_bbl = mywa2f1ax.bbl.BBLToString();
            jsonoutputStrBN.out_num_of_blockfaces = mywa2f1ax.num_of_blockfaces;
            jsonoutputStrBN.out_sanborn_boro = mywa2f1ax.sanborn.boro;
            jsonoutputStrBN.out_sanborn_volume = mywa2f1ax.sanborn.volume + mywa2f1ax.sanborn.volume_suffix;
            jsonoutputStrBN.out_sanborn_page = mywa2f1ax.sanborn.page + mywa2f1ax.sanborn.page_suffix;
            jsonoutputStrBN.out_x_coord = mywa2f1ax.x_coord;
            jsonoutputStrBN.out_y_coord = mywa2f1ax.y_coord;
            jsonoutputStrBN.out_latitude = mywa2f1ax.latitude;
            jsonoutputStrBN.out_longitude = mywa2f1ax.longitude;
            jsonoutputStrBN.out_vacant_flag = mywa2f1ax.vacant_flag;
            jsonoutputStrBN.out_condo_flag = mywa2f1ax.condo_flag;
            if (mywa2f1ax.condo_flag == "C")
            {
                jsonoutputStrBN.out_low_bbl_condo = mywa2f1ax.condo_lo_bbl.boro + mywa2f1ax.condo_lo_bbl.block + mywa2f1ax.condo_lo_bbl.lot;
                jsonoutputStrBN.out_high_bbl_condo = mywa2f1ax.condo_hi_bbl.boro + mywa2f1ax.condo_hi_bbl.block + mywa2f1ax.condo_hi_bbl.lot;
            }
            else
            {
                jsonoutputStrBN.out_low_bbl_condo = "N/A";
                jsonoutputStrBN.out_high_bbl_condo = "N/A";
            }
            jsonoutputStrBN.out_bin = mywa2f1ax.bin.BINToString();
            jsonoutputStrBN.out_tpad_bin_status = mywa2f1ax.TPAD_bin_status;
            jsonoutputStrBN.out_corner_code = mywa2f1ax.corner_code;
            jsonoutputStrBN.out_num_of_bldgs = mywa2f1ax.num_of_bldgs;

            if (string.IsNullOrEmpty(mywa2f1ax.bid_id.B5scToString().Trim()))
            {
                jsonoutputStrBN.out_bid_id = "";
            }
            else
            {
                jsonoutputStrBN.out_bid_id = getStreetName(mywa2f1ax.bid_id.boro, mywa2f1ax.bid_id.B5scToString().Remove(0, 1));
            }
            jsonoutputStrBN.out_rpad_scc = mywa2f1ax.rpad_scc;
            jsonoutputStrBN.out_rpad_bldg_class = mywa2f1ax.rpad_bldg_class;

            if (string.IsNullOrEmpty(mywa2f1ax.interior_flag))
            {
                jsonoutputStrBN.out_interior_flag = "No";
            }
            else
            {
                jsonoutputStrBN.out_interior_flag = mywa2f1ax.interior_flag;
            }

            if (string.IsNullOrEmpty(mywa2f1ax.irreg_flag))
            {
                jsonoutputStrBN.out_irreg_flag = "N/A";
            }
            else
            {
                jsonoutputStrBN.out_irreg_flag = mywa2f1ax.irreg_flag;
            }

            if (string.IsNullOrEmpty(mywa2f1ax.condo_num) | (mywa2f1ax.condo_num) == "0")
            {
                jsonoutputStrBN.out_condo_num = "N/A";
            }
            else
            {
                jsonoutputStrBN.out_condo_num = mywa2f1ax.condo_num;
            }
            if (string.IsNullOrEmpty(mywa2f1ax.coop_num) | (mywa2f1ax.coop_num) == "0")
            {
                jsonoutputStrBN.out_coop_num = "N/A";
            }
            else
            {
                jsonoutputStrBN.out_coop_num = mywa2f1ax.coop_num;
            }
            jsonoutputStrBN.out_tax_map = mywa2f1ax.dof_map.boro;
            jsonoutputStrBN.out_tax_section = mywa2f1ax.dof_map.section_volume.Remove(2, 2);
            jsonoutputStrBN.out_tax_volume = mywa2f1ax.dof_map.section_volume.Remove(0, 2);


            return JsonConvert.SerializeObject(jsonoutputStrBN);
        }

        public struct jsonoutputBBL
        {
            public string in_func_code;
            public string in_b10sc1_boro;
            public string in_bbl_block;
            public string in_bbl_lot;
            public string out_grc;
            public string out_grc2;
            public string out_error_message;
            public string out_bbl_boro;
            public string out_bbl_block;
            public string out_bbl_lot;
            public string out_bbl_bbl;
            public string out_num_of_blockfaces;
            public string out_sanborn_boro;
            public string out_sanborn_volume;
            public string out_sanborn_page;
            public string out_x_coord;
            public string out_y_coord;
            public string out_latitude;
            public string out_longitude;
            public string out_vacant_flag;
            public string out_condo_flag;
            public string out_low_bbl_condo;
            public string out_high_bbl_condo;
            public string out_bin;
            public string out_tpad_bin_status;
            public string out_corner_code;
            public string out_num_of_bldgs;
            public string out_bid_id;
            public string out_rpad_scc;
            public string out_rpad_bldg_class;
            public string out_interior_flag;
            public string out_irreg_flag;
            public string out_condo_num;
            public string out_coop_num;
            public string out_tax_map;
            public string out_tax_section;
            public string out_tax_volume;
        }
        string IGeoService.GetBBLGeocode(string Borough, string Block, string Lot)
        {
            // work area 1 
            Wa1 mywa1 = new Wa1();
            Wa1 mywa1_stname = new Wa1();

            // work area 2
            Wa2F1a mywa2f1a = new Wa2F1a();

            mywa1.in_b10sc1.boro = Borough;
            mywa1.in_bbl.block = Block;
            mywa1.in_bbl.lot = Lot;

            mywa1.in_mode_switch = "X";
            mywa1.in_func_code = "BL";
            mywa1.in_platform_ind = "C";
            mywa1.in_tpad_switch = "Y";

            // this call gets BBL info
            mygeo.GeoCall(ref mywa1, ref mywa2f1a);

            jsonoutputBBL jsonoutputStrBBL = new jsonoutputBBL();
            jsonoutputStrBBL.in_func_code = mywa1.in_func_code;
            jsonoutputStrBBL.in_b10sc1_boro = mywa1.in_b10sc1.boro;
            jsonoutputStrBBL.in_bbl_block = mywa1.in_bbl.block;
            jsonoutputStrBBL.in_bbl_lot = mywa1.in_bbl.lot;
            jsonoutputStrBBL.out_grc = mywa1.out_grc;
            jsonoutputStrBBL.out_grc2 = mywa1.out_grc2;
            jsonoutputStrBBL.out_error_message = mywa1.out_error_message;
            jsonoutputStrBBL.out_bbl_block = mywa2f1a.bbl.block;
            jsonoutputStrBBL.out_bbl_lot = mywa2f1a.bbl.lot;
            jsonoutputStrBBL.out_bbl_bbl = mywa2f1a.bbl.BBLToString();
            jsonoutputStrBBL.out_num_of_blockfaces = mywa2f1a.num_of_blockfaces;
            jsonoutputStrBBL.out_sanborn_boro = mywa2f1a.sanborn.boro;
            jsonoutputStrBBL.out_sanborn_volume = mywa2f1a.sanborn.volume + mywa2f1a.sanborn.volume_suffix;
            jsonoutputStrBBL.out_sanborn_page = mywa2f1a.sanborn.page + mywa2f1a.sanborn.page_suffix;
            jsonoutputStrBBL.out_x_coord = mywa2f1a.x_coord;
            jsonoutputStrBBL.out_y_coord = mywa2f1a.y_coord;
            jsonoutputStrBBL.out_latitude = mywa2f1a.latitude;
            jsonoutputStrBBL.out_longitude = mywa2f1a.longitude;
            jsonoutputStrBBL.out_vacant_flag = mywa2f1a.vacant_flag;
            jsonoutputStrBBL.out_condo_flag = mywa2f1a.condo_flag;
            if (mywa2f1a.condo_flag == "C")
            {
                jsonoutputStrBBL.out_low_bbl_condo = mywa2f1a.condo_lo_bbl.boro + mywa2f1a.condo_lo_bbl.block + mywa2f1a.condo_lo_bbl.lot;
                jsonoutputStrBBL.out_high_bbl_condo = mywa2f1a.condo_hi_bbl.boro + mywa2f1a.condo_hi_bbl.block + mywa2f1a.condo_hi_bbl.lot;
            }
            else
            {
                jsonoutputStrBBL.out_low_bbl_condo = "N/A";
                jsonoutputStrBBL.out_high_bbl_condo = "N/A";
            }
            jsonoutputStrBBL.out_bin = mywa2f1a.bin.BINToString();
            jsonoutputStrBBL.out_tpad_bin_status = mywa2f1a.TPAD_bin_status;
            jsonoutputStrBBL.out_corner_code = mywa2f1a.corner_code;
            jsonoutputStrBBL.out_num_of_bldgs = mywa2f1a.num_of_bldgs;

            if (string.IsNullOrEmpty(mywa2f1a.bid_id.B5scToString().Trim()))
            {
                jsonoutputStrBBL.out_bid_id = "";
            }
            else
            {
                jsonoutputStrBBL.out_bid_id = getStreetName(mywa2f1a.bid_id.boro, mywa2f1a.bid_id.B5scToString().Remove(0, 1));
            }
            jsonoutputStrBBL.out_rpad_scc = mywa2f1a.rpad_scc;
            jsonoutputStrBBL.out_rpad_bldg_class = mywa2f1a.rpad_bldg_class;

            if (string.IsNullOrEmpty(mywa2f1a.interior_flag))
            {
                jsonoutputStrBBL.out_interior_flag = "No";
            }
            else
            {
                jsonoutputStrBBL.out_interior_flag = mywa2f1a.interior_flag;
            }

            if (string.IsNullOrEmpty(mywa2f1a.irreg_flag))
            {
                jsonoutputStrBBL.out_irreg_flag = "N/A";
            }
            else
            {
                jsonoutputStrBBL.out_irreg_flag = mywa2f1a.irreg_flag;
            }

            if (string.IsNullOrEmpty(mywa2f1a.condo_num) | (mywa2f1a.condo_num) == "0")
            {
                jsonoutputStrBBL.out_condo_num = "N/A";
            }
            else
            {
                jsonoutputStrBBL.out_condo_num = mywa2f1a.condo_num;
            }
            if (string.IsNullOrEmpty(mywa2f1a.coop_num) | (mywa2f1a.coop_num) == "0")
            {
                jsonoutputStrBBL.out_coop_num = "N/A";
            }
            else
            {
                jsonoutputStrBBL.out_coop_num = mywa2f1a.coop_num;
            }
            jsonoutputStrBBL.out_tax_map = mywa2f1a.dof_map.boro;
            jsonoutputStrBBL.out_tax_section = mywa2f1a.dof_map.section_volume.Remove(2, 2);
            jsonoutputStrBBL.out_tax_volume = mywa2f1a.dof_map.section_volume.Remove(0, 2);

            return JsonConvert.SerializeObject(jsonoutputStrBBL);
        }
        public struct jsonoutput1B
        {
            public string in_func_code;
            public string in_boro;
            public string in_hnd;
            public string in_stname1;
            public string out_grc;
            public string out_grc2;
            public string out_error_message;
            public string out_x_coord;
            public string out_y_coord;
            public string out_from_node;
            public string out_to_node;
            public string out_latitude;
            public string out_longitude;
            public string out_lo_x_coord;
            public string out_lo_y_coord;
            public string out_hi_x_coord;
            public string out_hi_y_coord;
            public string out_com_dist;
            public string out_lion_key_face_code;
            public string out_lion_key_sequence_number;
            public string out_coincident_seg_cnt;
            public string out_b10sc1;
            public string out_segment_id;
            public string out_segment_len;
            public string out_alx;
            public string out_segment_type;
            public string out_traffic_dir;
            public string out_feature_type;
            public string out_roadway_type;
            public string out_right_of_way_type;
            public string out_physical_id;
            public string out_generic_id;
            //public string out_bike_lane2;
            public string out_spec_addr_flag;
            public string out_lo_hns;
            public string out_hi_hns;
            public string out_census_tract_2010;
            public string out_census_block_2010;
            public string out_census_tract_2000;
            public string out_census_block_2000;
            public string out_police_patrol_boro;
            public string out_police_pct;
            public string out_fire_div;
            public string out_fire_bat;
            public string out_fire_co;
            public string out_health_area;
            public string out_health_center_dist;
            public string out_dot_st_light_contract_area;
            public string out_san_dist_section;
            public string out_san_sched;
            public string out_san_reg;
            public string out_san_recycle;
            public string out_san_org_pick_up;
            public string out_school_dist;
            public string out_dsny_snow_priority;
            //public string out_san_bulk;
            public string out_hurricane_zone;
            public string out_street_width;
            //public string out_st_width_max;
            public string out_street_width_irregular;
            public string out_co;
            public string out_ad;
            public string out_cd;
            public string out_mc;
            public string out_ed;
            public string out_sd;
            public string out_boe_preferred_b7sc;
            public string out_bbl_block;
            public string out_bbl_lot;
            public string out_bbl;
            public string out_num_of_blockfaces;
            public string out_sanborn_boro;
            public string out_sanborn_volume;
            public string out_sanborn_page;
            public string out_rpad_scc;
            public string out_rpad_bldg_class;
            public string out_interior_flag;
            public string out_irreg_flag;
            public string out_condo_num;
            public string out_coop_num;
            public string out_vacant_flag;
            public string out_condo_flag;
            public string out_lo_bbl_condo;
            public string out_hi_bbl_condo;
            public string out_tax_map;
            public string out_tax_section;
            public string out_tax_volume;
            public string out_bin;
            public string out_TPAD_bin_status;
            public string out_TPAD_new_bin;
            public string out_TPAD_new_bin_status;
            public string out_TPAD_conflict_flag;
            public string out_corner_code;
            public string out_bid;
            public string out_x_y_coord;
            public string out_blockface_id;
            public string out_No_Traveling_lanes;
            public string out_No_Parking_lanes;
            public string out_No_Total_Lanes;
        }
        string IGeoService.Get1BGeocode(string Borough, string AddressNo, string StreetName)
        {
            // work area 1 
            Wa1 mywa1 = new Wa1();
            Wa1 mywa1_stname = new Wa1();
            Wa2F1b mywa2f1b = new Wa2F1b();

            mywa1.in_b10sc1.boro = Borough;
            mywa1.in_hnd = AddressNo;
            mywa1.in_stname1 = StreetName;

            mywa1.in_func_code = "1B";
            mywa1.in_platform_ind = "C";
            //mywa1.in_tpad_switch = "X";
            //mywa1.in_xstreet_names_flag = "E";

            // this call gets 1B info
            mygeo.GeoCall(ref mywa1, ref mywa2f1b);
            jsonoutput1B jsonoutputStr1B = new jsonoutput1B();
            jsonoutputStr1B.in_func_code = mywa1.in_func_code;
            jsonoutputStr1B.in_boro = mywa1.in_b10sc1.boro;
            jsonoutputStr1B.in_hnd = mywa1.in_hnd;
            jsonoutputStr1B.in_stname1 = mywa1.in_stname1;
            jsonoutputStr1B.out_grc = mywa1.out_grc;
            jsonoutputStr1B.out_grc2 = mywa1.out_grc2;
            jsonoutputStr1B.out_error_message = mywa1.out_error_message;
            jsonoutputStr1B.out_x_coord = mywa2f1b.wa2f1ex.x_coord;
            jsonoutputStr1B.out_y_coord = mywa2f1b.wa2f1ex.y_coord;
            jsonoutputStr1B.out_from_node = mywa2f1b.wa2f1ex.from_node;
            jsonoutputStr1B.out_to_node = mywa2f1b.wa2f1ex.to_node;
            jsonoutputStr1B.out_latitude = mywa2f1b.wa2f1ex.latitude;
            jsonoutputStr1B.out_longitude = mywa2f1b.wa2f1ex.longitude;
            jsonoutputStr1B.out_lo_x_coord = mywa2f1b.wa2f1ex.lo_x_coord;
            jsonoutputStr1B.out_lo_y_coord = mywa2f1b.wa2f1ex.lo_y_coord;
            jsonoutputStr1B.out_hi_x_coord = mywa2f1b.wa2f1ex.hi_x_coord;
            jsonoutputStr1B.out_hi_y_coord = mywa2f1b.wa2f1ex.hi_y_coord;
            jsonoutputStr1B.out_com_dist = mywa2f1b.wa2f1ex.com_dist.boro + mywa2f1b.wa2f1ex.com_dist.district_number;
            jsonoutputStr1B.out_lion_key_face_code = mywa2f1b.wa2f1ex.lion_key.face_code;
            jsonoutputStr1B.out_lion_key_sequence_number = mywa2f1b.wa2f1ex.lion_key.sequence_number;
            jsonoutputStr1B.out_coincident_seg_cnt = mywa2f1b.wa2f1ex.coincident_seg_cnt;
            jsonoutputStr1B.out_b10sc1 = mywa1.out_b10sc1.B10scToString();
            jsonoutputStr1B.out_segment_id = mywa2f1b.wa2f1ex.segment_id + " / " + mywa2f1b.wa2f1ex.segment_len;
            if (string.IsNullOrEmpty(mywa2f1b.wa2f1ex.alx))
            {
                jsonoutputStr1B.out_alx = "None";
            }
            else
            {
                jsonoutputStr1B.out_alx = mywa2f1b.wa2f1ex.alx;
            }
            jsonoutputStr1B.out_segment_type = mywa2f1b.wa2f1ex.segment_type;
            jsonoutputStr1B.out_traffic_dir = mywa2f1b.wa2f1ex.traffic_dir;
            jsonoutputStr1B.out_feature_type = mywa2f1b.wa2f1ex.feature_type;
            jsonoutputStr1B.out_roadway_type = mywa2f1b.wa2f1ex.roadway_type;
            jsonoutputStr1B.out_right_of_way_type = mywa2f1b.wa2f1ex.right_of_way_type;
            jsonoutputStr1B.out_physical_id = mywa2f1b.wa2f1ex.physical_id;
            jsonoutputStr1B.out_generic_id = mywa2f1b.wa2f1ex.generic_id;
            //jsonoutputStr1B.out_bike_lane2 = mywa2f1b.wa2f1ex.bike_lane2;
            jsonoutputStr1B.out_spec_addr_flag = mywa2f1b.wa2f1ex.spec_addr_flag;
            jsonoutputStr1B.out_lo_hns = mywa2f1b.wa2f1ex.lo_hns;
            jsonoutputStr1B.out_hi_hns = mywa2f1b.wa2f1ex.hi_hns;
            jsonoutputStr1B.out_census_tract_2010 = mywa2f1b.wa2f1ex.census_tract_2010;
            jsonoutputStr1B.out_census_block_2010 = mywa2f1b.wa2f1ex.census_block_2010;
            jsonoutputStr1B.out_census_tract_2000 = mywa2f1b.wa2f1ex.census_tract_2000;
            jsonoutputStr1B.out_census_block_2000 = mywa2f1b.wa2f1ex.census_block_2000;
            jsonoutputStr1B.out_police_patrol_boro = mywa2f1b.wa2f1ex.police_patrol_boro;
            jsonoutputStr1B.out_police_pct = mywa2f1b.wa2f1ex.police_pct;
            jsonoutputStr1B.out_fire_div = mywa2f1b.wa2f1ex.fire_div;
            jsonoutputStr1B.out_fire_bat = mywa2f1b.wa2f1ex.fire_bat;
            jsonoutputStr1B.out_fire_co = mywa2f1b.wa2f1ex.fire_co_type + " " + mywa2f1b.wa2f1ex.fire_co_num;
            jsonoutputStr1B.out_health_area = mywa2f1b.wa2f1ex.health_area.Substring(0, 2) + "." + mywa2f1b.wa2f1ex.health_area.Substring(2, 2);
            jsonoutputStr1B.out_health_center_dist = mywa2f1b.wa2f1ex.health_center_dist;
            jsonoutputStr1B.out_dot_st_light_contract_area = mywa2f1b.wa2f1ex.dot_st_light_contract_area;
            jsonoutputStr1B.out_san_dist_section = mywa2f1b.wa2f1ex.san_dist + " / " + mywa2f1b.wa2f1ex.san_dist.Remove(0, 1) + mywa2f1b.wa2f1ex.san_sched.Remove(1);
            jsonoutputStr1B.out_san_sched = mywa2f1b.wa2f1ex.san_sched;
            jsonoutputStr1B.out_san_reg = mywa2f1b.wa2f1ex.san_reg;
            jsonoutputStr1B.out_san_recycle = mywa2f1b.wa2f1ex.san_recycle;
            jsonoutputStr1B.out_san_org_pick_up = mywa2f1b.wa2f1ex.san_org_pick_up;
            jsonoutputStr1B.out_school_dist = mywa2f1b.wa2f1ex.school_dist;
            jsonoutputStr1B.out_dsny_snow_priority = mywa2f1b.wa2f1ex.dsny_snow_priority;
            //jsonoutputStr1B.out_san_bulk = mywa2f1b.wa2f1ex.san_bulk;
            jsonoutputStr1B.out_hurricane_zone = mywa2f1b.wa2f1ex.hurricane_zone;
            jsonoutputStr1B.out_street_width = mywa2f1b.wa2f1ex.street_width;
            //jsonoutputStr1B.out_st_width_max = mywa2f1b.wa2f1ex.st_width_max;
            jsonoutputStr1B.out_street_width_irregular = mywa2f1b.wa2f1ex.street_width_irregular;
            jsonoutputStr1B.out_co = mywa2f1b.wa2f1ex.co;
            jsonoutputStr1B.out_ad = mywa2f1b.wa2f1ex.ad;
            jsonoutputStr1B.out_cd = mywa2f1b.wa2f1ex.cd;
            jsonoutputStr1B.out_mc = mywa2f1b.wa2f1ex.mc;
            jsonoutputStr1B.out_ed = mywa2f1b.wa2f1ex.ed;
            jsonoutputStr1B.out_sd = mywa2f1b.wa2f1ex.sd;
            jsonoutputStr1B.out_boe_preferred_b7sc = mywa2f1b.wa2f1ex.boe_preferred_b7sc.ToString() + " / " + mywa2f1b.wa2f1ex.boe_preferred_stname.ToString();
            jsonoutputStr1B.out_bbl_block = mywa2f1b.wa2f1ax.bbl.block;
            jsonoutputStr1B.out_bbl_lot = mywa2f1b.wa2f1ax.bbl.lot;
            jsonoutputStr1B.out_bbl = mywa2f1b.wa2f1ax.bbl.ToString();
            jsonoutputStr1B.out_num_of_blockfaces = mywa2f1b.wa2f1ax.num_of_blockfaces;
            jsonoutputStr1B.out_sanborn_boro = mywa2f1b.wa2f1ax.sanborn.boro;
            jsonoutputStr1B.out_sanborn_volume = mywa2f1b.wa2f1ax.sanborn.volume + mywa2f1b.wa2f1ax.sanborn.volume_suffix;
            jsonoutputStr1B.out_sanborn_page = mywa2f1b.wa2f1ax.sanborn.page + mywa2f1b.wa2f1ax.sanborn.page_suffix;
            jsonoutputStr1B.out_rpad_scc = mywa2f1b.wa2f1ax.rpad_scc;
            jsonoutputStr1B.out_rpad_bldg_class = mywa2f1b.wa2f1ax.rpad_bldg_class;
            if (string.IsNullOrEmpty(mywa2f1b.wa2f1ax.interior_flag))
            {
                jsonoutputStr1B.out_interior_flag = "No";
            }
            else
            {
                jsonoutputStr1B.out_interior_flag = mywa2f1b.wa2f1ax.interior_flag;
            }
            if (string.IsNullOrEmpty(mywa2f1b.wa2f1ax.irreg_flag))
            {
                jsonoutputStr1B.out_irreg_flag = "No";
            }
            else
            {
                jsonoutputStr1B.out_irreg_flag = mywa2f1b.wa2f1ax.irreg_flag;
            }
            if (string.IsNullOrEmpty(mywa2f1b.wa2f1ax.condo_num))
            {
                jsonoutputStr1B.out_condo_num = "N/A";
            }
            else
            {
                jsonoutputStr1B.out_condo_num = mywa2f1b.wa2f1ax.condo_num;
            }
            if (string.IsNullOrEmpty(mywa2f1b.wa2f1ax.coop_num))
            {
                jsonoutputStr1B.out_coop_num = "N/A";
            }
            else
            {
                jsonoutputStr1B.out_coop_num = mywa2f1b.wa2f1ax.coop_num;
            }
            jsonoutputStr1B.out_vacant_flag = mywa2f1b.wa2f1ax.vacant_flag;
            jsonoutputStr1B.out_condo_flag = mywa2f1b.wa2f1ax.condo_flag;
            if (mywa2f1b.wa2f1ax.condo_flag == "C")
            {
                jsonoutputStr1B.out_lo_bbl_condo = mywa2f1b.wa2f1ax.condo_lo_bbl.boro + " - " + mywa2f1b.wa2f1ax.condo_lo_bbl.block + " - " + mywa2f1b.wa2f1ax.condo_lo_bbl.lot;
                jsonoutputStr1B.out_hi_bbl_condo = mywa2f1b.wa2f1ax.condo_hi_bbl.boro + " - " + mywa2f1b.wa2f1ax.condo_hi_bbl.block + " - " + mywa2f1b.wa2f1ax.condo_hi_bbl.lot;
            }
            else
            {
                jsonoutputStr1B.out_lo_bbl_condo = "N/A";
                jsonoutputStr1B.out_hi_bbl_condo = "N/A";
            }
            jsonoutputStr1B.out_tax_map = "'" + mywa2f1b.wa2f1ax.dof_map.boro;
            jsonoutputStr1B.out_tax_section = mywa2f1b.wa2f1ax.dof_map.section_volume.Remove(2, 2);
            jsonoutputStr1B.out_tax_volume = mywa2f1b.wa2f1ax.dof_map.section_volume.Remove(0, 2);
            jsonoutputStr1B.out_bin = mywa2f1b.wa2f1ax.bin.BINToString();
            jsonoutputStr1B.out_TPAD_bin_status = mywa2f1b.wa2f1ax.TPAD_bin_status;
            jsonoutputStr1B.out_TPAD_new_bin = mywa2f1b.wa2f1ax.TPAD_new_bin.ToString();
            jsonoutputStr1B.out_TPAD_new_bin_status = mywa2f1b.wa2f1ax.TPAD_new_bin_status;
            jsonoutputStr1B.out_TPAD_conflict_flag = mywa2f1b.wa2f1ax.TPAD_conflict_flag;
            jsonoutputStr1B.out_corner_code = mywa2f1b.wa2f1ax.corner_code;
            if (mywa2f1b.wa2f1ax.bid_id.B5scToString().Trim() == "")
            {
                jsonoutputStr1B.out_bid = "";
            }
            else
            {
                jsonoutputStr1B.out_bid = getStreetName(mywa2f1b.wa2f1ax.bid_id.boro, mywa2f1b.wa2f1ax.bid_id.B5scToString().Remove(0, 1));
            }
            jsonoutputStr1B.out_x_y_coord = mywa2f1b.wa2f1ax.x_coord + "/" + mywa2f1b.wa2f1ax.y_coord;
            jsonoutputStr1B.out_blockface_id = mywa2f1b.wa2f1ex.blockface_id;
            jsonoutputStr1B.out_No_Traveling_lanes = mywa2f1b.wa2f1ex.No_Traveling_lanes;
            jsonoutputStr1B.out_No_Parking_lanes = mywa2f1b.wa2f1ex.No_Parking_lanes;
            jsonoutputStr1B.out_No_Total_Lanes = mywa2f1b.wa2f1ex.No_Total_Lanes;

            return JsonConvert.SerializeObject(jsonoutputStr1B);
        }

        public struct jsonoutput2
        {
            public string in_func_code;
            public string in_boro1;
            public string in_stname1;
            public string in_boro2;
            public string in_stname2;
            public string out_grc;
            public string out_grc2;
            public string out_error_message;
            public string out_zip_code;
            public string out_x_coord;
            public string out_y_coord;
            public string out_com_dist;
            public string out_compass;
            public string out_lion_node_num;
            public string out_dcp_pref_lgc1;
            public string out_dcp_pref_lgc2;
            public string out_census_tract_2010;
            public string out_census_tract_2000;
            public string out_sanborn1_boro;
            public string out_sanborn1_vol;
            public string out_sanborn1_page;
            public string out_sanborn2_boro;
            public string out_sanborn2_vol;
            public string out_sanborn2_page;
            public string out_atomic_polygon;
            public string out_police_patrol_boro;
            public string out_police_pct;
            public string out_fire_div;
            public string out_fire_bat;
            public string out_fire_co_num;
            public string out_health_area;
            public string out_health_center_dist;
            public string out_dot_st_light_contract_area;
            public string out_san_dist;
            public string out_san_sub_section;
            public string out_school_dist;
            public string out_cd_eligible;
            public string out_co;
            public string out_ad;
            public string out_cd;
            public string out_mc;
            public string out_sd;
        }
        string IGeoService.Get2Geocode(string Borough1, string Street1, string Borough2, string Street2)
        {
            // work area 1 
            Wa1 mywa1 = new Wa1();
            Wa1 mywa1_stname = new Wa1();

            // work area 2
            Wa2F2 mywa2f2 = new Wa2F2();

            mywa1.in_boro1 = Borough1;
            mywa1.in_stname1 = Street1;
            mywa1.in_boro2 = Borough2;
            mywa1.in_stname2 = Street2;

            //mywa1.in_mode_switch = "X";
            mywa1.in_func_code = "2";
            mywa1.in_platform_ind = "C";
            //mywa1.in_tpad_switch = "Y";
            mywa1.in_xstreet_names_flag = "E";

            // this call gets 2 info
            mygeo.GeoCall(ref mywa1, ref mywa2f2);
            jsonoutput2 jsonoutputStr2 = new jsonoutput2();
            jsonoutputStr2.in_func_code = mywa1.in_func_code;
            jsonoutputStr2.in_boro1 = mywa1.in_boro1;
            jsonoutputStr2.in_stname1 = mywa1.in_stname1;
            jsonoutputStr2.in_boro2 = mywa1.in_boro2;
            jsonoutputStr2.in_stname2 = mywa1.in_stname2;
            jsonoutputStr2.out_grc = mywa1.out_grc;
            jsonoutputStr2.out_grc2 = mywa1.out_grc2;
            jsonoutputStr2.out_error_message = mywa1.out_error_message;
            jsonoutputStr2.out_zip_code = mywa2f2.zip_code;
            jsonoutputStr2.out_x_coord = mywa2f2.x_coord;
            jsonoutputStr2.out_y_coord = mywa2f2.y_coord;
            jsonoutputStr2.out_com_dist = mywa2f2.com_dist.boro + mywa2f2.com_dist.district_number;
            jsonoutputStr2.out_compass = mywa2f2.compass;
            jsonoutputStr2.out_lion_node_num = mywa2f2.lion_node_num;
            jsonoutputStr2.out_dcp_pref_lgc1 = mywa2f2.dcp_pref_lgc1;
            jsonoutputStr2.out_dcp_pref_lgc2 = mywa2f2.dcp_pref_lgc2;
            jsonoutputStr2.out_census_tract_2010 = mywa2f2.census_tract_2010;
            jsonoutputStr2.out_census_tract_2000 = mywa2f2.census_tract_2000;
            jsonoutputStr2.out_sanborn1_boro = mywa2f2.sanborn1.boro;
            jsonoutputStr2.out_sanborn1_vol = mywa2f2.sanborn1.volume + mywa2f2.sanborn1.volume_suffix;
            jsonoutputStr2.out_sanborn1_page = mywa2f2.sanborn1.page = mywa2f2.sanborn1.page_suffix;
            jsonoutputStr2.out_sanborn2_boro = mywa2f2.sanborn2.boro;
            jsonoutputStr2.out_sanborn2_vol = mywa2f2.sanborn2.volume + mywa2f2.sanborn2.volume_suffix;
            jsonoutputStr2.out_sanborn2_page = mywa2f2.sanborn2.page = mywa2f2.sanborn2.page_suffix;
            jsonoutputStr2.out_atomic_polygon = mywa2f2.atomic_polygon;
            jsonoutputStr2.out_police_patrol_boro = mywa2f2.police_patrol_boro;
            jsonoutputStr2.out_police_pct = mywa2f2.police_pct;
            jsonoutputStr2.out_fire_div = mywa2f2.fire_div;
            jsonoutputStr2.out_fire_bat = mywa2f2.fire_bat;
            jsonoutputStr2.out_fire_co_num = mywa2f2.fire_co_num;
            jsonoutputStr2.out_health_area = mywa2f2.health_area;
            jsonoutputStr2.out_health_center_dist = mywa2f2.health_center_dist;
            jsonoutputStr2.out_dot_st_light_contract_area = mywa2f2.dot_st_light_contract_area;
            jsonoutputStr2.out_san_dist = mywa2f2.san_dist;
            jsonoutputStr2.out_san_sub_section = mywa2f2.san_sub_section;
            jsonoutputStr2.out_school_dist = mywa2f2.school_dist;
            jsonoutputStr2.out_cd_eligible = mywa2f2.cd_eligible;
            jsonoutputStr2.out_co = mywa2f2.co;
            jsonoutputStr2.out_ad = mywa2f2.ad;
            jsonoutputStr2.out_cd = mywa2f2.cd;
            jsonoutputStr2.out_mc = mywa2f2.mc;
            jsonoutputStr2.out_sd = mywa2f2.sd;

            return JsonConvert.SerializeObject(jsonoutputStr2);
        }

        public struct jsonoutput3
        {
            public string in_func_code;
            public string in_boro1;
            public string in_stname1;
            public string in_boro2;
            public string in_stname2;
            public string in_boro3;
            public string in_stname3;
            public string in_compass_dir;
            public string out_grc;
            public string out_grc2;
            public string out_error_message;
            public string out_zip_code;
            public string out_from_node;
            public string out_to_node;
            public string out_lionkey;
            public string out_from_x_coord;
            public string out_from_y_coord;
            public string out_to_x_coord;
            public string out_to_y_coord;
            public string out_dot_street_light_contract_area;
            public string out_segment_id;
            public string out_segment_len;
            public string out_physical_id;
            public string out_generic_id;
            public string out_loc_status;
            //public string out_bike_lane2;
            public string out_traffic_direction;
            public string out_segment_type;
            public string out_feature_type;
            public string out_roadway_type;
            public string out_right_of_way_type;
            public string out_No_Traveling_lanes;
            public string out_No_Parking_lanes;
            public string out_Total_Lanes;
            public string out_street_width;
            //public string out_st_width_max;
            public string out_street_width_irregular;

            public string out_left_side_boro;
            public string out_left_side_comdist;
            public string out_left_side_lhnd;
            public string out_left_side_hhnd;
            public string out_left_side_zip_code;
            public string out_left_side_census_tract_2010;
            public string out_left_side_census_block_2010;
            public string out_left_side_census_tract_2000;
            public string out_left_side_census_block_2000;
            public string out_left_side_police_patrol_boro;
            public string out_left_side_police_pct;
            public string out_left_side_fire_div;
            public string out_left_side_fire_bat;
            public string out_left_side_fire_co;
            public string out_left_side_health_area;
            public string out_left_side_health_center_dist;
            public string out_left_side_dot_street_light_contract_area;
            public string out_left_side_school_dist;
            public string out_left_side_iaei;
            public string out_left_side_blockface_id;

            public string out_right_side_boro;
            public string out_right_side_comdist;
            public string out_right_side_lhnd;
            public string out_right_side_hhnd;
            public string out_right_side_zip_code;
            public string out_right_side_census_tract_2010;
            public string out_right_side_census_block_2010;
            public string out_right_side_census_tract_2000;
            public string out_right_side_census_block_2000;
            public string out_right_side_police_patrol_boro;
            public string out_right_side_police_pct;
            public string out_right_side_fire_div;
            public string out_right_side_fire_bat;
            public string out_right_side_fire_co;
            public string out_right_side_health_area;
            public string out_right_side_health_center_dist;
            public string out_right_side_dot_street_light_contract_area;
            public string out_right_side_school_dist;
            public string out_right_side_iaei;
            public string out_right_side_blockface_id;



        }
        string IGeoService.Get3Geocode(string Borough1, string OnStreet, string SideofStreet, string Borough2, string FirstCrossStreet, string Borough3, string SecondCrossStreet)
        {
            // work area 1 
            Wa1 mywa1 = new Wa1();
            Wa1 mywa1_stname = new Wa1();

            // work area 2
            Wa2F3xas mywa2f3 = new Wa2F3xas();

            mywa1.in_boro1 = Borough1;
            mywa1.in_stname1 = OnStreet;
            mywa1.in_compass_dir = SideofStreet;
            mywa1.in_boro2 = Borough2;
            mywa1.in_stname2 = FirstCrossStreet;
            mywa1.in_boro3 = Borough3;
            mywa1.in_stname3 = SecondCrossStreet;

            mywa1.in_func_code = "3";
            mywa1.in_platform_ind = "C";
            mywa1.in_xstreet_names_flag = "E";
            mywa1.in_auxseg_switch = "Y";
            mywa1.in_mode_switch = "X";

            // this call gets 3 info
            mygeo.GeoCall(ref mywa1, ref mywa2f3);
            jsonoutput3 jsonoutputStr3 = new jsonoutput3();
            jsonoutputStr3.in_func_code = mywa1.in_func_code;
            jsonoutputStr3.in_boro1 = mywa1.in_boro1;
            jsonoutputStr3.in_stname1 = mywa1.in_stname1;
            jsonoutputStr3.in_compass_dir = mywa1.in_compass_dir;
            jsonoutputStr3.in_boro2 = mywa1.in_boro2;
            jsonoutputStr3.in_stname2 = mywa1.in_stname2;
            jsonoutputStr3.in_boro3 = mywa1.in_boro3;
            jsonoutputStr3.in_stname3 = mywa1.in_stname3;
            jsonoutputStr3.out_grc = mywa1.out_grc;
            jsonoutputStr3.out_grc2 = mywa1.out_grc2;
            jsonoutputStr3.out_error_message = mywa1.out_error_message;

            jsonoutputStr3.out_zip_code = mywa2f3.wa2f3x.left_side.zip_code;
            jsonoutputStr3.out_from_node = mywa2f3.wa2f3x.from_node;
            jsonoutputStr3.out_to_node = mywa2f3.wa2f3x.to_node;
            jsonoutputStr3.out_lionkey = mywa2f3.wa2f3x.lionkey.ToString();
            jsonoutputStr3.out_from_x_coord = mywa2f3.wa2f3x.from_x_coord;
            jsonoutputStr3.out_from_y_coord = mywa2f3.wa2f3x.from_y_coord;
            jsonoutputStr3.out_to_x_coord = mywa2f3.wa2f3x.to_x_coord;
            jsonoutputStr3.out_to_y_coord = mywa2f3.wa2f3x.to_y_coord;
            jsonoutputStr3.out_dot_street_light_contract_area = mywa2f3.wa2f3x.dot_street_light_contract_area;
            jsonoutputStr3.out_segment_id = mywa2f3.wa2f3x.segment_id;
            jsonoutputStr3.out_segment_len = mywa2f3.wa2f3x.segment_len;
            jsonoutputStr3.out_physical_id = mywa2f3.wa2f3x.physical_id;
            jsonoutputStr3.out_generic_id = mywa2f3.wa2f3x.generic_id;
            jsonoutputStr3.out_loc_status = mywa2f3.wa2f3x.loc_status;
            //jsonoutputStr3.out_bike_lane2 = mywa2f3.wa2f3x.bike_lane;
            jsonoutputStr3.out_traffic_direction = mywa2f3.wa2f3x.traffic_direction;
            jsonoutputStr3.out_segment_type = mywa2f3.wa2f3x.segment_type;
            jsonoutputStr3.out_feature_type = mywa2f3.wa2f3x.feature_type;
            jsonoutputStr3.out_roadway_type = mywa2f3.wa2f3x.roadway_type;
            jsonoutputStr3.out_right_of_way_type = mywa2f3.wa2f3x.right_of_way_type;
            jsonoutputStr3.out_No_Traveling_lanes = mywa2f3.wa2f3x.No_Traveling_lanes;
            jsonoutputStr3.out_No_Parking_lanes = mywa2f3.wa2f3x.No_Parking_lanes;
            jsonoutputStr3.out_Total_Lanes = mywa2f3.wa2f3x.Total_Lanes;
            jsonoutputStr3.out_street_width = mywa2f3.wa2f3x.street_width;
            //jsonoutputStr3.out_st_width_max = mywa2f3.wa2f3x.st_width_max;
            jsonoutputStr3.out_street_width_irregular = mywa2f3.wa2f3x.street_width_irregular;

            jsonoutputStr3.out_left_side_boro = mywa2f3.wa2f3x.left_side.boro;
            jsonoutputStr3.out_left_side_comdist = mywa2f3.wa2f3x.left_side.boro + mywa2f3.wa2f3x.left_side.comdist.district_number;
            jsonoutputStr3.out_left_side_lhnd = mywa2f3.wa2f3x.left_side.lhnd;
            jsonoutputStr3.out_left_side_hhnd = mywa2f3.wa2f3x.left_side.hhnd;
            jsonoutputStr3.out_left_side_zip_code = mywa2f3.wa2f3x.left_side.zip_code;
            jsonoutputStr3.out_left_side_census_tract_2010 = mywa2f3.wa2f3x.left_side.census_tract_2010;
            jsonoutputStr3.out_left_side_census_block_2010 = mywa2f3.wa2f3x.left_side.census_block_2010;
            jsonoutputStr3.out_left_side_census_tract_2000 = mywa2f3.wa2f3x.left_side.census_tract_2000;
            jsonoutputStr3.out_left_side_census_block_2000 = mywa2f3.wa2f3x.left_side.census_block_2000;
            jsonoutputStr3.out_left_side_police_patrol_boro = mywa2f3.wa2f3x.left_side.police_patrol_boro;
            jsonoutputStr3.out_left_side_police_pct = mywa2f3.wa2f3x.left_side.police_pct;
            jsonoutputStr3.out_left_side_fire_div = mywa2f3.wa2f3x.left_side.fire_div;
            jsonoutputStr3.out_left_side_fire_bat = mywa2f3.wa2f3x.left_side.fire_bat;
            jsonoutputStr3.out_left_side_fire_co = mywa2f3.wa2f3x.left_side.fire_co_type + " " + mywa2f3.wa2f3x.left_side.fire_co_num;
            jsonoutputStr3.out_left_side_health_area = mywa2f3.wa2f3x.left_side.health_area.Substring(0, 2) + "." + mywa2f3.wa2f3x.left_side.health_area.Substring(2, 2);
            jsonoutputStr3.out_left_side_health_center_dist = mywa2f3.wa2f3x.left_health_center_dist;
            jsonoutputStr3.out_left_side_dot_street_light_contract_area = mywa2f3.wa2f3x.dot_street_light_contract_area;
            jsonoutputStr3.out_left_side_school_dist = mywa2f3.wa2f3x.left_side.school_dist;
            jsonoutputStr3.out_left_side_iaei = mywa2f3.wa2f3x.left_side.iaei;
            jsonoutputStr3.out_left_side_blockface_id = mywa2f3.wa2f3x.left_blockface_id;

            jsonoutputStr3.out_right_side_boro = mywa2f3.wa2f3x.right_side.boro;
            jsonoutputStr3.out_right_side_comdist = mywa2f3.wa2f3x.right_side.boro + mywa2f3.wa2f3x.right_side.comdist.district_number;
            jsonoutputStr3.out_right_side_lhnd = mywa2f3.wa2f3x.right_side.lhnd;
            jsonoutputStr3.out_right_side_hhnd = mywa2f3.wa2f3x.right_side.hhnd;
            jsonoutputStr3.out_right_side_zip_code = mywa2f3.wa2f3x.right_side.zip_code;
            jsonoutputStr3.out_right_side_census_tract_2010 = mywa2f3.wa2f3x.right_side.census_tract_2010;
            jsonoutputStr3.out_right_side_census_block_2010 = mywa2f3.wa2f3x.right_side.census_block_2010;
            jsonoutputStr3.out_right_side_census_tract_2000 = mywa2f3.wa2f3x.right_side.census_tract_2000;
            jsonoutputStr3.out_right_side_census_block_2000 = mywa2f3.wa2f3x.right_side.census_block_2000;
            jsonoutputStr3.out_right_side_police_patrol_boro = mywa2f3.wa2f3x.right_side.police_patrol_boro;
            jsonoutputStr3.out_right_side_police_pct = mywa2f3.wa2f3x.right_side.police_pct;
            jsonoutputStr3.out_right_side_fire_div = mywa2f3.wa2f3x.right_side.fire_div;
            jsonoutputStr3.out_right_side_fire_bat = mywa2f3.wa2f3x.right_side.fire_bat;
            jsonoutputStr3.out_right_side_fire_co = mywa2f3.wa2f3x.right_side.fire_co_type + " " + mywa2f3.wa2f3x.right_side.fire_co_num;
            jsonoutputStr3.out_right_side_health_area = mywa2f3.wa2f3x.right_side.health_area.Substring(0, 2) + "." + mywa2f3.wa2f3x.right_side.health_area.Substring(2, 2);
            jsonoutputStr3.out_right_side_health_center_dist = mywa2f3.wa2f3x.right_health_center_dist;
            jsonoutputStr3.out_right_side_dot_street_light_contract_area = mywa2f3.wa2f3x.dot_street_light_contract_area;
            jsonoutputStr3.out_right_side_school_dist = mywa2f3.wa2f3x.right_side.school_dist;
            jsonoutputStr3.out_right_side_iaei = mywa2f3.wa2f3x.right_side.iaei;
            jsonoutputStr3.out_right_side_blockface_id = mywa2f3.wa2f3x.right_blockface_id;

            return JsonConvert.SerializeObject(jsonoutputStr3);
        }

        public struct jsonoutput3S
        {
            public string in_func_code;
            public string in_boro;
            public string in_stname1;
            public string in_stname2;
            public string in_stname3;
            public string out_grc;
            public string out_grc2;
            public string out_error_message;
            public string out_number_of_intersections;
            public string out_intersecting_street_1;
            public string out_second_intersecting_street_1;
            public string out_third_intersecting_street_1;
            public string out_fourth_intersecting_street_1;
            public string out_fifth_intersecting_street_1;
            public string out_xstr_cnt_1;
            public string out_distance_1;
            public string out_gap_flag_1;
            public string out_node_num_1;
            public string out_intersecting_street_2;
            public string out_second_intersecting_street_2;
            public string out_third_intersecting_street_2;
            public string out_fourth_intersecting_street_2;
            public string out_fifth_intersecting_street_2;
            public string out_xstr_cnt_2;
            public string out_distance_2;
            public string out_gap_flag_2;
            public string out_node_num_2;
            public string out_intersecting_street_3;
            public string out_second_intersecting_street_3;
            public string out_third_intersecting_street_3;
            public string out_fourth_intersecting_street_3;
            public string out_fifth_intersecting_street_3;
            public string out_xstr_cnt_3;
            public string out_distance_3;
            public string out_gap_flag_3;
            public string out_node_num_3;
            public string out_intersecting_street_4;
            public string out_second_intersecting_street_4;
            public string out_third_intersecting_street_4;
            public string out_fourth_intersecting_street_4;
            public string out_fifth_intersecting_street_4;
            public string out_xstr_cnt_4;
            public string out_distance_4;
            public string out_gap_flag_4;
            public string out_node_num_4;
            public string out_intersecting_street_5;
            public string out_second_intersecting_street_5;
            public string out_third_intersecting_street_5;
            public string out_fourth_intersecting_street_5;
            public string out_fifth_intersecting_street_5;
            public string out_xstr_cnt_5;
            public string out_distance_5;
            public string out_gap_flag_5;
            public string out_node_num_5;
            public string out_intersecting_street_6;
            public string out_second_intersecting_street_6;
            public string out_third_intersecting_street_6;
            public string out_fourth_intersecting_street_6;
            public string out_fifth_intersecting_street_6;
            public string out_xstr_cnt_6;
            public string out_distance_6;
            public string out_gap_flag_6;
            public string out_node_num_6;
            public string out_intersecting_street_7;
            public string out_second_intersecting_street_7;
            public string out_third_intersecting_street_7;
            public string out_fourth_intersecting_street_7;
            public string out_fifth_intersecting_street_7;
            public string out_xstr_cnt_7;
            public string out_distance_7;
            public string out_gap_flag_7;
            public string out_node_num_7;
            public string out_intersecting_street_8;
            public string out_second_intersecting_street_8;
            public string out_third_intersecting_street_8;
            public string out_fourth_intersecting_street_8;
            public string out_fifth_intersecting_street_8;
            public string out_xstr_cnt_8;
            public string out_distance_8;
            public string out_gap_flag_8;
            public string out_node_num_8;
            public string out_intersecting_street_9;
            public string out_second_intersecting_street_9;
            public string out_third_intersecting_street_9;
            public string out_fourth_intersecting_street_9;
            public string out_fifth_intersecting_street_9;
            public string out_xstr_cnt_9;
            public string out_distance_9;
            public string out_gap_flag_9;
            public string out_node_num_9;
            public string out_intersecting_street_10;
            public string out_second_intersecting_street_10;
            public string out_third_intersecting_street_10;
            public string out_fourth_intersecting_street_10;
            public string out_fifth_intersecting_street_10;
            public string out_xstr_cnt_10;
            public string out_distance_10;
            public string out_gap_flag_10;
            public string out_node_num_10;
            public string out_intersecting_street_11;
            public string out_second_intersecting_street_11;
            public string out_third_intersecting_street_11;
            public string out_fourth_intersecting_street_11;
            public string out_fifth_intersecting_street_11;
            public string out_xstr_cnt_11;
            public string out_distance_11;
            public string out_gap_flag_11;
            public string out_node_num_11;
            public string out_intersecting_street_12;
            public string out_second_intersecting_street_12;
            public string out_third_intersecting_street_12;
            public string out_fourth_intersecting_street_12;
            public string out_fifth_intersecting_street_12;
            public string out_xstr_cnt_12;
            public string out_distance_12;
            public string out_gap_flag_12;
            public string out_node_num_12;
            public string out_intersecting_street_13;
            public string out_second_intersecting_street_13;
            public string out_third_intersecting_street_13;
            public string out_fourth_intersecting_street_13;
            public string out_fifth_intersecting_street_13;
            public string out_xstr_cnt_13;
            public string out_distance_13;
            public string out_gap_flag_13;
            public string out_node_num_13;
            public string out_intersecting_street_14;
            public string out_second_intersecting_street_14;
            public string out_third_intersecting_street_14;
            public string out_fourth_intersecting_street_14;
            public string out_fifth_intersecting_street_14;
            public string out_xstr_cnt_14;
            public string out_distance_14;
            public string out_gap_flag_14;
            public string out_node_num_14;
            public string out_intersecting_street_15;
            public string out_second_intersecting_street_15;
            public string out_third_intersecting_street_15;
            public string out_fourth_intersecting_street_15;
            public string out_fifth_intersecting_street_15;
            public string out_xstr_cnt_15;
            public string out_distance_15;
            public string out_gap_flag_15;
            public string out_node_num_15;
            public string out_intersecting_street_16;
            public string out_second_intersecting_street_16;
            public string out_third_intersecting_street_16;
            public string out_fourth_intersecting_street_16;
            public string out_fifth_intersecting_street_16;
            public string out_xstr_cnt_16;
            public string out_distance_16;
            public string out_gap_flag_16;
            public string out_node_num_16;
            public string out_intersecting_street_17;
            public string out_second_intersecting_street_17;
            public string out_third_intersecting_street_17;
            public string out_fourth_intersecting_street_17;
            public string out_fifth_intersecting_street_17;
            public string out_xstr_cnt_17;
            public string out_distance_17;
            public string out_gap_flag_17;
            public string out_node_num_17;
            public string out_intersecting_street_18;
            public string out_second_intersecting_street_18;
            public string out_third_intersecting_street_18;
            public string out_fourth_intersecting_street_18;
            public string out_fifth_intersecting_street_18;
            public string out_xstr_cnt_18;
            public string out_distance_18;
            public string out_gap_flag_18;
            public string out_node_num_18;
            public string out_intersecting_street_19;
            public string out_second_intersecting_street_19;
            public string out_third_intersecting_street_19;
            public string out_fourth_intersecting_street_19;
            public string out_fifth_intersecting_street_19;
            public string out_xstr_cnt_19;
            public string out_distance_19;
            public string out_gap_flag_19;
            public string out_node_num_19;
            public string out_intersecting_street_20;
            public string out_second_intersecting_street_20;
            public string out_third_intersecting_street_20;
            public string out_fourth_intersecting_street_20;
            public string out_fifth_intersecting_street_20;
            public string out_xstr_cnt_20;
            public string out_distance_20;
            public string out_gap_flag_20;
            public string out_node_num_20;
            public string out_intersecting_street_21;
            public string out_second_intersecting_street_21;
            public string out_third_intersecting_street_21;
            public string out_fourth_intersecting_street_21;
            public string out_fifth_intersecting_street_21;
            public string out_xstr_cnt_21;
            public string out_distance_21;
            public string out_gap_flag_21;
            public string out_node_num_21;
            public string out_intersecting_street_22;
            public string out_second_intersecting_street_22;
            public string out_third_intersecting_street_22;
            public string out_fourth_intersecting_street_22;
            public string out_fifth_intersecting_street_22;
            public string out_xstr_cnt_22;
            public string out_distance_22;
            public string out_gap_flag_22;
            public string out_node_num_22;
            public string out_intersecting_street_23;
            public string out_second_intersecting_street_23;
            public string out_third_intersecting_street_23;
            public string out_fourth_intersecting_street_23;
            public string out_fifth_intersecting_street_23;
            public string out_xstr_cnt_23;
            public string out_distance_23;
            public string out_gap_flag_23;
            public string out_node_num_23;
            public string out_intersecting_street_24;
            public string out_second_intersecting_street_24;
            public string out_third_intersecting_street_24;
            public string out_fourth_intersecting_street_24;
            public string out_fifth_intersecting_street_24;
            public string out_xstr_cnt_24;
            public string out_distance_24;
            public string out_gap_flag_24;
            public string out_node_num_24;
            public string out_intersecting_street_25;
            public string out_second_intersecting_street_25;
            public string out_third_intersecting_street_25;
            public string out_fourth_intersecting_street_25;
            public string out_fifth_intersecting_street_25;
            public string out_xstr_cnt_25;
            public string out_distance_25;
            public string out_gap_flag_25;
            public string out_node_num_25;
            public string out_intersecting_street_26;
            public string out_second_intersecting_street_26;
            public string out_third_intersecting_street_26;
            public string out_fourth_intersecting_street_26;
            public string out_fifth_intersecting_street_26;
            public string out_xstr_cnt_26;
            public string out_distance_26;
            public string out_gap_flag_26;
            public string out_node_num_26;
            public string out_intersecting_street_27;
            public string out_second_intersecting_street_27;
            public string out_third_intersecting_street_27;
            public string out_fourth_intersecting_street_27;
            public string out_fifth_intersecting_street_27;
            public string out_xstr_cnt_27;
            public string out_distance_27;
            public string out_gap_flag_27;
            public string out_node_num_27;
            public string out_intersecting_street_28;
            public string out_second_intersecting_street_28;
            public string out_third_intersecting_street_28;
            public string out_fourth_intersecting_street_28;
            public string out_fifth_intersecting_street_28;
            public string out_xstr_cnt_28;
            public string out_distance_28;
            public string out_gap_flag_28;
            public string out_node_num_28;
            public string out_intersecting_street_29;
            public string out_second_intersecting_street_29;
            public string out_third_intersecting_street_29;
            public string out_fourth_intersecting_street_29;
            public string out_fifth_intersecting_street_29;
            public string out_xstr_cnt_29;
            public string out_distance_29;
            public string out_gap_flag_29;
            public string out_node_num_29;
            public string out_intersecting_street_30;
            public string out_second_intersecting_street_30;
            public string out_third_intersecting_street_30;
            public string out_fourth_intersecting_street_30;
            public string out_fifth_intersecting_street_30;
            public string out_xstr_cnt_30;
            public string out_distance_30;
            public string out_gap_flag_30;
            public string out_node_num_30;
            public string out_intersecting_street_31;
            public string out_second_intersecting_street_31;
            public string out_third_intersecting_street_31;
            public string out_fourth_intersecting_street_31;
            public string out_fifth_intersecting_street_31;
            public string out_xstr_cnt_31;
            public string out_distance_31;
            public string out_gap_flag_31;
            public string out_node_num_31;
            public string out_intersecting_street_32;
            public string out_second_intersecting_street_32;
            public string out_third_intersecting_street_32;
            public string out_fourth_intersecting_street_32;
            public string out_fifth_intersecting_street_32;
            public string out_xstr_cnt_32;
            public string out_distance_32;
            public string out_gap_flag_32;
            public string out_node_num_32;
            public string out_intersecting_street_33;
            public string out_second_intersecting_street_33;
            public string out_third_intersecting_street_33;
            public string out_fourth_intersecting_street_33;
            public string out_fifth_intersecting_street_33;
            public string out_xstr_cnt_33;
            public string out_distance_33;
            public string out_gap_flag_33;
            public string out_node_num_33;
            public string out_intersecting_street_34;
            public string out_second_intersecting_street_34;
            public string out_third_intersecting_street_34;
            public string out_fourth_intersecting_street_34;
            public string out_fifth_intersecting_street_34;
            public string out_xstr_cnt_34;
            public string out_distance_34;
            public string out_gap_flag_34;
            public string out_node_num_34;
            public string out_intersecting_street_35;
            public string out_second_intersecting_street_35;
            public string out_third_intersecting_street_35;
            public string out_fourth_intersecting_street_35;
            public string out_fifth_intersecting_street_35;
            public string out_xstr_cnt_35;
            public string out_distance_35;
            public string out_gap_flag_35;
            public string out_node_num_35;
            public string out_intersecting_street_36;
            public string out_second_intersecting_street_36;
            public string out_third_intersecting_street_36;
            public string out_fourth_intersecting_street_36;
            public string out_fifth_intersecting_street_36;
            public string out_xstr_cnt_36;
            public string out_distance_36;
            public string out_gap_flag_36;
            public string out_node_num_36;
            public string out_intersecting_street_37;
            public string out_second_intersecting_street_37;
            public string out_third_intersecting_street_37;
            public string out_fourth_intersecting_street_37;
            public string out_fifth_intersecting_street_37;
            public string out_xstr_cnt_37;
            public string out_distance_37;
            public string out_gap_flag_37;
            public string out_node_num_37;
            public string out_intersecting_street_38;
            public string out_second_intersecting_street_38;
            public string out_third_intersecting_street_38;
            public string out_fourth_intersecting_street_38;
            public string out_fifth_intersecting_street_38;
            public string out_xstr_cnt_38;
            public string out_distance_38;
            public string out_gap_flag_38;
            public string out_node_num_38;
            public string out_intersecting_street_39;
            public string out_second_intersecting_street_39;
            public string out_third_intersecting_street_39;
            public string out_fourth_intersecting_street_39;
            public string out_fifth_intersecting_street_39;
            public string out_xstr_cnt_39;
            public string out_distance_39;
            public string out_gap_flag_39;
            public string out_node_num_39;
            public string out_intersecting_street_40;
            public string out_second_intersecting_street_40;
            public string out_third_intersecting_street_40;
            public string out_fourth_intersecting_street_40;
            public string out_fifth_intersecting_street_40;
            public string out_xstr_cnt_40;
            public string out_distance_40;
            public string out_gap_flag_40;
            public string out_node_num_40;
            public string out_intersecting_street_41;
            public string out_second_intersecting_street_41;
            public string out_third_intersecting_street_41;
            public string out_fourth_intersecting_street_41;
            public string out_fifth_intersecting_street_41;
            public string out_xstr_cnt_41;
            public string out_distance_41;
            public string out_gap_flag_41;
            public string out_node_num_41;
            public string out_intersecting_street_42;
            public string out_second_intersecting_street_42;
            public string out_third_intersecting_street_42;
            public string out_fourth_intersecting_street_42;
            public string out_fifth_intersecting_street_42;
            public string out_xstr_cnt_42;
            public string out_distance_42;
            public string out_gap_flag_42;
            public string out_node_num_42;
            public string out_intersecting_street_43;
            public string out_second_intersecting_street_43;
            public string out_third_intersecting_street_43;
            public string out_fourth_intersecting_street_43;
            public string out_fifth_intersecting_street_43;
            public string out_xstr_cnt_43;
            public string out_distance_43;
            public string out_gap_flag_43;
            public string out_node_num_43;
            public string out_intersecting_street_44;
            public string out_second_intersecting_street_44;
            public string out_third_intersecting_street_44;
            public string out_fourth_intersecting_street_44;
            public string out_fifth_intersecting_street_44;
            public string out_xstr_cnt_44;
            public string out_distance_44;
            public string out_gap_flag_44;
            public string out_node_num_44;
            public string out_intersecting_street_45;
            public string out_second_intersecting_street_45;
            public string out_third_intersecting_street_45;
            public string out_fourth_intersecting_street_45;
            public string out_fifth_intersecting_street_45;
            public string out_xstr_cnt_45;
            public string out_distance_45;
            public string out_gap_flag_45;
            public string out_node_num_45;
            public string out_intersecting_street_46;
            public string out_second_intersecting_street_46;
            public string out_third_intersecting_street_46;
            public string out_fourth_intersecting_street_46;
            public string out_fifth_intersecting_street_46;
            public string out_xstr_cnt_46;
            public string out_distance_46;
            public string out_gap_flag_46;
            public string out_node_num_46;
            public string out_intersecting_street_47;
            public string out_second_intersecting_street_47;
            public string out_third_intersecting_street_47;
            public string out_fourth_intersecting_street_47;
            public string out_fifth_intersecting_street_47;
            public string out_xstr_cnt_47;
            public string out_distance_47;
            public string out_gap_flag_47;
            public string out_node_num_47;
            public string out_intersecting_street_48;
            public string out_second_intersecting_street_48;
            public string out_third_intersecting_street_48;
            public string out_fourth_intersecting_street_48;
            public string out_fifth_intersecting_street_48;
            public string out_xstr_cnt_48;
            public string out_distance_48;
            public string out_gap_flag_48;
            public string out_node_num_48;
            public string out_intersecting_street_49;
            public string out_second_intersecting_street_49;
            public string out_third_intersecting_street_49;
            public string out_fourth_intersecting_street_49;
            public string out_fifth_intersecting_street_49;
            public string out_xstr_cnt_49;
            public string out_distance_49;
            public string out_gap_flag_49;
            public string out_node_num_49;
            public string out_intersecting_street_50;
            public string out_second_intersecting_street_50;
            public string out_third_intersecting_street_50;
            public string out_fourth_intersecting_street_50;
            public string out_fifth_intersecting_street_50;
            public string out_xstr_cnt_50;
            public string out_distance_50;
            public string out_gap_flag_50;
            public string out_node_num_50;
            public string out_intersecting_street_51;
            public string out_second_intersecting_street_51;
            public string out_third_intersecting_street_51;
            public string out_fourth_intersecting_street_51;
            public string out_fifth_intersecting_street_51;
            public string out_xstr_cnt_51;
            public string out_distance_51;
            public string out_gap_flag_51;
            public string out_node_num_51;
            public string out_intersecting_street_52;
            public string out_second_intersecting_street_52;
            public string out_third_intersecting_street_52;
            public string out_fourth_intersecting_street_52;
            public string out_fifth_intersecting_street_52;
            public string out_xstr_cnt_52;
            public string out_distance_52;
            public string out_gap_flag_52;
            public string out_node_num_52;
            public string out_intersecting_street_53;
            public string out_second_intersecting_street_53;
            public string out_third_intersecting_street_53;
            public string out_fourth_intersecting_street_53;
            public string out_fifth_intersecting_street_53;
            public string out_xstr_cnt_53;
            public string out_distance_53;
            public string out_gap_flag_53;
            public string out_node_num_53;
            public string out_intersecting_street_54;
            public string out_second_intersecting_street_54;
            public string out_third_intersecting_street_54;
            public string out_fourth_intersecting_street_54;
            public string out_fifth_intersecting_street_54;
            public string out_xstr_cnt_54;
            public string out_distance_54;
            public string out_gap_flag_54;
            public string out_node_num_54;
            public string out_intersecting_street_55;
            public string out_second_intersecting_street_55;
            public string out_third_intersecting_street_55;
            public string out_fourth_intersecting_street_55;
            public string out_fifth_intersecting_street_55;
            public string out_xstr_cnt_55;
            public string out_distance_55;
            public string out_gap_flag_55;
            public string out_node_num_55;
            public string out_intersecting_street_56;
            public string out_second_intersecting_street_56;
            public string out_third_intersecting_street_56;
            public string out_fourth_intersecting_street_56;
            public string out_fifth_intersecting_street_56;
            public string out_xstr_cnt_56;
            public string out_distance_56;
            public string out_gap_flag_56;
            public string out_node_num_56;
            public string out_intersecting_street_57;
            public string out_second_intersecting_street_57;
            public string out_third_intersecting_street_57;
            public string out_fourth_intersecting_street_57;
            public string out_fifth_intersecting_street_57;
            public string out_xstr_cnt_57;
            public string out_distance_57;
            public string out_gap_flag_57;
            public string out_node_num_57;
            public string out_intersecting_street_58;
            public string out_second_intersecting_street_58;
            public string out_third_intersecting_street_58;
            public string out_fourth_intersecting_street_58;
            public string out_fifth_intersecting_street_58;
            public string out_xstr_cnt_58;
            public string out_distance_58;
            public string out_gap_flag_58;
            public string out_node_num_58;
            public string out_intersecting_street_59;
            public string out_second_intersecting_street_59;
            public string out_third_intersecting_street_59;
            public string out_fourth_intersecting_street_59;
            public string out_fifth_intersecting_street_59;
            public string out_xstr_cnt_59;
            public string out_distance_59;
            public string out_gap_flag_59;
            public string out_node_num_59;
            public string out_intersecting_street_60;
            public string out_second_intersecting_street_60;
            public string out_third_intersecting_street_60;
            public string out_fourth_intersecting_street_60;
            public string out_fifth_intersecting_street_60;
            public string out_xstr_cnt_60;
            public string out_distance_60;
            public string out_gap_flag_60;
            public string out_node_num_60;
            public string out_intersecting_street_61;
            public string out_second_intersecting_street_61;
            public string out_third_intersecting_street_61;
            public string out_fourth_intersecting_street_61;
            public string out_fifth_intersecting_street_61;
            public string out_xstr_cnt_61;
            public string out_distance_61;
            public string out_gap_flag_61;
            public string out_node_num_61;
            public string out_intersecting_street_62;
            public string out_second_intersecting_street_62;
            public string out_third_intersecting_street_62;
            public string out_fourth_intersecting_street_62;
            public string out_fifth_intersecting_street_62;
            public string out_xstr_cnt_62;
            public string out_distance_62;
            public string out_gap_flag_62;
            public string out_node_num_62;
            public string out_intersecting_street_63;
            public string out_second_intersecting_street_63;
            public string out_third_intersecting_street_63;
            public string out_fourth_intersecting_street_63;
            public string out_fifth_intersecting_street_63;
            public string out_xstr_cnt_63;
            public string out_distance_63;
            public string out_gap_flag_63;
            public string out_node_num_63;
            public string out_intersecting_street_64;
            public string out_second_intersecting_street_64;
            public string out_third_intersecting_street_64;
            public string out_fourth_intersecting_street_64;
            public string out_fifth_intersecting_street_64;
            public string out_xstr_cnt_64;
            public string out_distance_64;
            public string out_gap_flag_64;
            public string out_node_num_64;
            public string out_intersecting_street_65;
            public string out_second_intersecting_street_65;
            public string out_third_intersecting_street_65;
            public string out_fourth_intersecting_street_65;
            public string out_fifth_intersecting_street_65;
            public string out_xstr_cnt_65;
            public string out_distance_65;
            public string out_gap_flag_65;
            public string out_node_num_65;
            public string out_intersecting_street_66;
            public string out_second_intersecting_street_66;
            public string out_third_intersecting_street_66;
            public string out_fourth_intersecting_street_66;
            public string out_fifth_intersecting_street_66;
            public string out_xstr_cnt_66;
            public string out_distance_66;
            public string out_gap_flag_66;
            public string out_node_num_66;
            public string out_intersecting_street_67;
            public string out_second_intersecting_street_67;
            public string out_third_intersecting_street_67;
            public string out_fourth_intersecting_street_67;
            public string out_fifth_intersecting_street_67;
            public string out_xstr_cnt_67;
            public string out_distance_67;
            public string out_gap_flag_67;
            public string out_node_num_67;
            public string out_intersecting_street_68;
            public string out_second_intersecting_street_68;
            public string out_third_intersecting_street_68;
            public string out_fourth_intersecting_street_68;
            public string out_fifth_intersecting_street_68;
            public string out_xstr_cnt_68;
            public string out_distance_68;
            public string out_gap_flag_68;
            public string out_node_num_68;
            public string out_intersecting_street_69;
            public string out_second_intersecting_street_69;
            public string out_third_intersecting_street_69;
            public string out_fourth_intersecting_street_69;
            public string out_fifth_intersecting_street_69;
            public string out_xstr_cnt_69;
            public string out_distance_69;
            public string out_gap_flag_69;
            public string out_node_num_69;
            public string out_intersecting_street_70;
            public string out_second_intersecting_street_70;
            public string out_third_intersecting_street_70;
            public string out_fourth_intersecting_street_70;
            public string out_fifth_intersecting_street_70;
            public string out_xstr_cnt_70;
            public string out_distance_70;
            public string out_gap_flag_70;
            public string out_node_num_70;
            public string out_intersecting_street_71;
            public string out_second_intersecting_street_71;
            public string out_third_intersecting_street_71;
            public string out_fourth_intersecting_street_71;
            public string out_fifth_intersecting_street_71;
            public string out_xstr_cnt_71;
            public string out_distance_71;
            public string out_gap_flag_71;
            public string out_node_num_71;
            public string out_intersecting_street_72;
            public string out_second_intersecting_street_72;
            public string out_third_intersecting_street_72;
            public string out_fourth_intersecting_street_72;
            public string out_fifth_intersecting_street_72;
            public string out_xstr_cnt_72;
            public string out_distance_72;
            public string out_gap_flag_72;
            public string out_node_num_72;
            public string out_intersecting_street_73;
            public string out_second_intersecting_street_73;
            public string out_third_intersecting_street_73;
            public string out_fourth_intersecting_street_73;
            public string out_fifth_intersecting_street_73;
            public string out_xstr_cnt_73;
            public string out_distance_73;
            public string out_gap_flag_73;
            public string out_node_num_73;
            public string out_intersecting_street_74;
            public string out_second_intersecting_street_74;
            public string out_third_intersecting_street_74;
            public string out_fourth_intersecting_street_74;
            public string out_fifth_intersecting_street_74;
            public string out_xstr_cnt_74;
            public string out_distance_74;
            public string out_gap_flag_74;
            public string out_node_num_74;
            public string out_intersecting_street_75;
            public string out_second_intersecting_street_75;
            public string out_third_intersecting_street_75;
            public string out_fourth_intersecting_street_75;
            public string out_fifth_intersecting_street_75;
            public string out_xstr_cnt_75;
            public string out_distance_75;
            public string out_gap_flag_75;
            public string out_node_num_75;
            public string out_intersecting_street_76;
            public string out_second_intersecting_street_76;
            public string out_third_intersecting_street_76;
            public string out_fourth_intersecting_street_76;
            public string out_fifth_intersecting_street_76;
            public string out_xstr_cnt_76;
            public string out_distance_76;
            public string out_gap_flag_76;
            public string out_node_num_76;
            public string out_intersecting_street_77;
            public string out_second_intersecting_street_77;
            public string out_third_intersecting_street_77;
            public string out_fourth_intersecting_street_77;
            public string out_fifth_intersecting_street_77;
            public string out_xstr_cnt_77;
            public string out_distance_77;
            public string out_gap_flag_77;
            public string out_node_num_77;
            public string out_intersecting_street_78;
            public string out_second_intersecting_street_78;
            public string out_third_intersecting_street_78;
            public string out_fourth_intersecting_street_78;
            public string out_fifth_intersecting_street_78;
            public string out_xstr_cnt_78;
            public string out_distance_78;
            public string out_gap_flag_78;
            public string out_node_num_78;
            public string out_intersecting_street_79;
            public string out_second_intersecting_street_79;
            public string out_third_intersecting_street_79;
            public string out_fourth_intersecting_street_79;
            public string out_fifth_intersecting_street_79;
            public string out_xstr_cnt_79;
            public string out_distance_79;
            public string out_gap_flag_79;
            public string out_node_num_79;
            public string out_intersecting_street_80;
            public string out_second_intersecting_street_80;
            public string out_third_intersecting_street_80;
            public string out_fourth_intersecting_street_80;
            public string out_fifth_intersecting_street_80;
            public string out_xstr_cnt_80;
            public string out_distance_80;
            public string out_gap_flag_80;
            public string out_node_num_80;
            public string out_intersecting_street_81;
            public string out_second_intersecting_street_81;
            public string out_third_intersecting_street_81;
            public string out_fourth_intersecting_street_81;
            public string out_fifth_intersecting_street_81;
            public string out_xstr_cnt_81;
            public string out_distance_81;
            public string out_gap_flag_81;
            public string out_node_num_81;
            public string out_intersecting_street_82;
            public string out_second_intersecting_street_82;
            public string out_third_intersecting_street_82;
            public string out_fourth_intersecting_street_82;
            public string out_fifth_intersecting_street_82;
            public string out_xstr_cnt_82;
            public string out_distance_82;
            public string out_gap_flag_82;
            public string out_node_num_82;
            public string out_intersecting_street_83;
            public string out_second_intersecting_street_83;
            public string out_third_intersecting_street_83;
            public string out_fourth_intersecting_street_83;
            public string out_fifth_intersecting_street_83;
            public string out_xstr_cnt_83;
            public string out_distance_83;
            public string out_gap_flag_83;
            public string out_node_num_83;
            public string out_intersecting_street_84;
            public string out_second_intersecting_street_84;
            public string out_third_intersecting_street_84;
            public string out_fourth_intersecting_street_84;
            public string out_fifth_intersecting_street_84;
            public string out_xstr_cnt_84;
            public string out_distance_84;
            public string out_gap_flag_84;
            public string out_node_num_84;
            public string out_intersecting_street_85;
            public string out_second_intersecting_street_85;
            public string out_third_intersecting_street_85;
            public string out_fourth_intersecting_street_85;
            public string out_fifth_intersecting_street_85;
            public string out_xstr_cnt_85;
            public string out_distance_85;
            public string out_gap_flag_85;
            public string out_node_num_85;
            public string out_intersecting_street_86;
            public string out_second_intersecting_street_86;
            public string out_third_intersecting_street_86;
            public string out_fourth_intersecting_street_86;
            public string out_fifth_intersecting_street_86;
            public string out_xstr_cnt_86;
            public string out_distance_86;
            public string out_gap_flag_86;
            public string out_node_num_86;
            public string out_intersecting_street_87;
            public string out_second_intersecting_street_87;
            public string out_third_intersecting_street_87;
            public string out_fourth_intersecting_street_87;
            public string out_fifth_intersecting_street_87;
            public string out_xstr_cnt_87;
            public string out_distance_87;
            public string out_gap_flag_87;
            public string out_node_num_87;
            public string out_intersecting_street_88;
            public string out_second_intersecting_street_88;
            public string out_third_intersecting_street_88;
            public string out_fourth_intersecting_street_88;
            public string out_fifth_intersecting_street_88;
            public string out_xstr_cnt_88;
            public string out_distance_88;
            public string out_gap_flag_88;
            public string out_node_num_88;
            public string out_intersecting_street_89;
            public string out_second_intersecting_street_89;
            public string out_third_intersecting_street_89;
            public string out_fourth_intersecting_street_89;
            public string out_fifth_intersecting_street_89;
            public string out_xstr_cnt_89;
            public string out_distance_89;
            public string out_gap_flag_89;
            public string out_node_num_89;
            public string out_intersecting_street_90;
            public string out_second_intersecting_street_90;
            public string out_third_intersecting_street_90;
            public string out_fourth_intersecting_street_90;
            public string out_fifth_intersecting_street_90;
            public string out_xstr_cnt_90;
            public string out_distance_90;
            public string out_gap_flag_90;
            public string out_node_num_90;
            public string out_intersecting_street_91;
            public string out_second_intersecting_street_91;
            public string out_third_intersecting_street_91;
            public string out_fourth_intersecting_street_91;
            public string out_fifth_intersecting_street_91;
            public string out_xstr_cnt_91;
            public string out_distance_91;
            public string out_gap_flag_91;
            public string out_node_num_91;
            public string out_intersecting_street_92;
            public string out_second_intersecting_street_92;
            public string out_third_intersecting_street_92;
            public string out_fourth_intersecting_street_92;
            public string out_fifth_intersecting_street_92;
            public string out_xstr_cnt_92;
            public string out_distance_92;
            public string out_gap_flag_92;
            public string out_node_num_92;
            public string out_intersecting_street_93;
            public string out_second_intersecting_street_93;
            public string out_third_intersecting_street_93;
            public string out_fourth_intersecting_street_93;
            public string out_fifth_intersecting_street_93;
            public string out_xstr_cnt_93;
            public string out_distance_93;
            public string out_gap_flag_93;
            public string out_node_num_93;
            public string out_intersecting_street_94;
            public string out_second_intersecting_street_94;
            public string out_third_intersecting_street_94;
            public string out_fourth_intersecting_street_94;
            public string out_fifth_intersecting_street_94;
            public string out_xstr_cnt_94;
            public string out_distance_94;
            public string out_gap_flag_94;
            public string out_node_num_94;
            public string out_intersecting_street_95;
            public string out_second_intersecting_street_95;
            public string out_third_intersecting_street_95;
            public string out_fourth_intersecting_street_95;
            public string out_fifth_intersecting_street_95;
            public string out_xstr_cnt_95;
            public string out_distance_95;
            public string out_gap_flag_95;
            public string out_node_num_95;
            public string out_intersecting_street_96;
            public string out_second_intersecting_street_96;
            public string out_third_intersecting_street_96;
            public string out_fourth_intersecting_street_96;
            public string out_fifth_intersecting_street_96;
            public string out_xstr_cnt_96;
            public string out_distance_96;
            public string out_gap_flag_96;
            public string out_node_num_96;
            public string out_intersecting_street_97;
            public string out_second_intersecting_street_97;
            public string out_third_intersecting_street_97;
            public string out_fourth_intersecting_street_97;
            public string out_fifth_intersecting_street_97;
            public string out_xstr_cnt_97;
            public string out_distance_97;
            public string out_gap_flag_97;
            public string out_node_num_97;
            public string out_intersecting_street_98;
            public string out_second_intersecting_street_98;
            public string out_third_intersecting_street_98;
            public string out_fourth_intersecting_street_98;
            public string out_fifth_intersecting_street_98;
            public string out_xstr_cnt_98;
            public string out_distance_98;
            public string out_gap_flag_98;
            public string out_node_num_98;
            public string out_intersecting_street_99;
            public string out_second_intersecting_street_99;
            public string out_third_intersecting_street_99;
            public string out_fourth_intersecting_street_99;
            public string out_fifth_intersecting_street_99;
            public string out_xstr_cnt_99;
            public string out_distance_99;
            public string out_gap_flag_99;
            public string out_node_num_99;
            public string out_intersecting_street_100;
            public string out_second_intersecting_street_100;
            public string out_third_intersecting_street_100;
            public string out_fourth_intersecting_street_100;
            public string out_fifth_intersecting_street_100;
            public string out_xstr_cnt_100;
            public string out_distance_100;
            public string out_gap_flag_100;
            public string out_node_num_100;
            public string out_intersecting_street_101;
            public string out_second_intersecting_street_101;
            public string out_third_intersecting_street_101;
            public string out_fourth_intersecting_street_101;
            public string out_fifth_intersecting_street_101;
            public string out_xstr_cnt_101;
            public string out_distance_101;
            public string out_gap_flag_101;
            public string out_node_num_101;
            public string out_intersecting_street_102;
            public string out_second_intersecting_street_102;
            public string out_third_intersecting_street_102;
            public string out_fourth_intersecting_street_102;
            public string out_fifth_intersecting_street_102;
            public string out_xstr_cnt_102;
            public string out_distance_102;
            public string out_gap_flag_102;
            public string out_node_num_102;
            public string out_intersecting_street_103;
            public string out_second_intersecting_street_103;
            public string out_third_intersecting_street_103;
            public string out_fourth_intersecting_street_103;
            public string out_fifth_intersecting_street_103;
            public string out_xstr_cnt_103;
            public string out_distance_103;
            public string out_gap_flag_103;
            public string out_node_num_103;
            public string out_intersecting_street_104;
            public string out_second_intersecting_street_104;
            public string out_third_intersecting_street_104;
            public string out_fourth_intersecting_street_104;
            public string out_fifth_intersecting_street_104;
            public string out_xstr_cnt_104;
            public string out_distance_104;
            public string out_gap_flag_104;
            public string out_node_num_104;
            public string out_intersecting_street_105;
            public string out_second_intersecting_street_105;
            public string out_third_intersecting_street_105;
            public string out_fourth_intersecting_street_105;
            public string out_fifth_intersecting_street_105;
            public string out_xstr_cnt_105;
            public string out_distance_105;
            public string out_gap_flag_105;
            public string out_node_num_105;
            public string out_intersecting_street_106;
            public string out_second_intersecting_street_106;
            public string out_third_intersecting_street_106;
            public string out_fourth_intersecting_street_106;
            public string out_fifth_intersecting_street_106;
            public string out_xstr_cnt_106;
            public string out_distance_106;
            public string out_gap_flag_106;
            public string out_node_num_106;
            public string out_intersecting_street_107;
            public string out_second_intersecting_street_107;
            public string out_third_intersecting_street_107;
            public string out_fourth_intersecting_street_107;
            public string out_fifth_intersecting_street_107;
            public string out_xstr_cnt_107;
            public string out_distance_107;
            public string out_gap_flag_107;
            public string out_node_num_107;
            public string out_intersecting_street_108;
            public string out_second_intersecting_street_108;
            public string out_third_intersecting_street_108;
            public string out_fourth_intersecting_street_108;
            public string out_fifth_intersecting_street_108;
            public string out_xstr_cnt_108;
            public string out_distance_108;
            public string out_gap_flag_108;
            public string out_node_num_108;
            public string out_intersecting_street_109;
            public string out_second_intersecting_street_109;
            public string out_third_intersecting_street_109;
            public string out_fourth_intersecting_street_109;
            public string out_fifth_intersecting_street_109;
            public string out_xstr_cnt_109;
            public string out_distance_109;
            public string out_gap_flag_109;
            public string out_node_num_109;
            public string out_intersecting_street_110;
            public string out_second_intersecting_street_110;
            public string out_third_intersecting_street_110;
            public string out_fourth_intersecting_street_110;
            public string out_fifth_intersecting_street_110;
            public string out_xstr_cnt_110;
            public string out_distance_110;
            public string out_gap_flag_110;
            public string out_node_num_110;
            public string out_intersecting_street_111;
            public string out_second_intersecting_street_111;
            public string out_third_intersecting_street_111;
            public string out_fourth_intersecting_street_111;
            public string out_fifth_intersecting_street_111;
            public string out_xstr_cnt_111;
            public string out_distance_111;
            public string out_gap_flag_111;
            public string out_node_num_111;
            public string out_intersecting_street_112;
            public string out_second_intersecting_street_112;
            public string out_third_intersecting_street_112;
            public string out_fourth_intersecting_street_112;
            public string out_fifth_intersecting_street_112;
            public string out_xstr_cnt_112;
            public string out_distance_112;
            public string out_gap_flag_112;
            public string out_node_num_112;
            public string out_intersecting_street_113;
            public string out_second_intersecting_street_113;
            public string out_third_intersecting_street_113;
            public string out_fourth_intersecting_street_113;
            public string out_fifth_intersecting_street_113;
            public string out_xstr_cnt_113;
            public string out_distance_113;
            public string out_gap_flag_113;
            public string out_node_num_113;
            public string out_intersecting_street_114;
            public string out_second_intersecting_street_114;
            public string out_third_intersecting_street_114;
            public string out_fourth_intersecting_street_114;
            public string out_fifth_intersecting_street_114;
            public string out_xstr_cnt_114;
            public string out_distance_114;
            public string out_gap_flag_114;
            public string out_node_num_114;
            public string out_intersecting_street_115;
            public string out_second_intersecting_street_115;
            public string out_third_intersecting_street_115;
            public string out_fourth_intersecting_street_115;
            public string out_fifth_intersecting_street_115;
            public string out_xstr_cnt_115;
            public string out_distance_115;
            public string out_gap_flag_115;
            public string out_node_num_115;
            public string out_intersecting_street_116;
            public string out_second_intersecting_street_116;
            public string out_third_intersecting_street_116;
            public string out_fourth_intersecting_street_116;
            public string out_fifth_intersecting_street_116;
            public string out_xstr_cnt_116;
            public string out_distance_116;
            public string out_gap_flag_116;
            public string out_node_num_116;
            public string out_intersecting_street_117;
            public string out_second_intersecting_street_117;
            public string out_third_intersecting_street_117;
            public string out_fourth_intersecting_street_117;
            public string out_fifth_intersecting_street_117;
            public string out_xstr_cnt_117;
            public string out_distance_117;
            public string out_gap_flag_117;
            public string out_node_num_117;
            public string out_intersecting_street_118;
            public string out_second_intersecting_street_118;
            public string out_third_intersecting_street_118;
            public string out_fourth_intersecting_street_118;
            public string out_fifth_intersecting_street_118;
            public string out_xstr_cnt_118;
            public string out_distance_118;
            public string out_gap_flag_118;
            public string out_node_num_118;
            public string out_intersecting_street_119;
            public string out_second_intersecting_street_119;
            public string out_third_intersecting_street_119;
            public string out_fourth_intersecting_street_119;
            public string out_fifth_intersecting_street_119;
            public string out_xstr_cnt_119;
            public string out_distance_119;
            public string out_gap_flag_119;
            public string out_node_num_119;
            public string out_intersecting_street_120;
            public string out_second_intersecting_street_120;
            public string out_third_intersecting_street_120;
            public string out_fourth_intersecting_street_120;
            public string out_fifth_intersecting_street_120;
            public string out_xstr_cnt_120;
            public string out_distance_120;
            public string out_gap_flag_120;
            public string out_node_num_120;
            public string out_intersecting_street_121;
            public string out_second_intersecting_street_121;
            public string out_third_intersecting_street_121;
            public string out_fourth_intersecting_street_121;
            public string out_fifth_intersecting_street_121;
            public string out_xstr_cnt_121;
            public string out_distance_121;
            public string out_gap_flag_121;
            public string out_node_num_121;
            public string out_intersecting_street_122;
            public string out_second_intersecting_street_122;
            public string out_third_intersecting_street_122;
            public string out_fourth_intersecting_street_122;
            public string out_fifth_intersecting_street_122;
            public string out_xstr_cnt_122;
            public string out_distance_122;
            public string out_gap_flag_122;
            public string out_node_num_122;
            public string out_intersecting_street_123;
            public string out_second_intersecting_street_123;
            public string out_third_intersecting_street_123;
            public string out_fourth_intersecting_street_123;
            public string out_fifth_intersecting_street_123;
            public string out_xstr_cnt_123;
            public string out_distance_123;
            public string out_gap_flag_123;
            public string out_node_num_123;
            public string out_intersecting_street_124;
            public string out_second_intersecting_street_124;
            public string out_third_intersecting_street_124;
            public string out_fourth_intersecting_street_124;
            public string out_fifth_intersecting_street_124;
            public string out_xstr_cnt_124;
            public string out_distance_124;
            public string out_gap_flag_124;
            public string out_node_num_124;
            public string out_intersecting_street_125;
            public string out_second_intersecting_street_125;
            public string out_third_intersecting_street_125;
            public string out_fourth_intersecting_street_125;
            public string out_fifth_intersecting_street_125;
            public string out_xstr_cnt_125;
            public string out_distance_125;
            public string out_gap_flag_125;
            public string out_node_num_125;
            public string out_intersecting_street_126;
            public string out_second_intersecting_street_126;
            public string out_third_intersecting_street_126;
            public string out_fourth_intersecting_street_126;
            public string out_fifth_intersecting_street_126;
            public string out_xstr_cnt_126;
            public string out_distance_126;
            public string out_gap_flag_126;
            public string out_node_num_126;
            public string out_intersecting_street_127;
            public string out_second_intersecting_street_127;
            public string out_third_intersecting_street_127;
            public string out_fourth_intersecting_street_127;
            public string out_fifth_intersecting_street_127;
            public string out_xstr_cnt_127;
            public string out_distance_127;
            public string out_gap_flag_127;
            public string out_node_num_127;
            public string out_intersecting_street_128;
            public string out_second_intersecting_street_128;
            public string out_third_intersecting_street_128;
            public string out_fourth_intersecting_street_128;
            public string out_fifth_intersecting_street_128;
            public string out_xstr_cnt_128;
            public string out_distance_128;
            public string out_gap_flag_128;
            public string out_node_num_128;
            public string out_intersecting_street_129;
            public string out_second_intersecting_street_129;
            public string out_third_intersecting_street_129;
            public string out_fourth_intersecting_street_129;
            public string out_fifth_intersecting_street_129;
            public string out_xstr_cnt_129;
            public string out_distance_129;
            public string out_gap_flag_129;
            public string out_node_num_129;
            public string out_intersecting_street_130;
            public string out_second_intersecting_street_130;
            public string out_third_intersecting_street_130;
            public string out_fourth_intersecting_street_130;
            public string out_fifth_intersecting_street_130;
            public string out_xstr_cnt_130;
            public string out_distance_130;
            public string out_gap_flag_130;
            public string out_node_num_130;
            public string out_intersecting_street_131;
            public string out_second_intersecting_street_131;
            public string out_third_intersecting_street_131;
            public string out_fourth_intersecting_street_131;
            public string out_fifth_intersecting_street_131;
            public string out_xstr_cnt_131;
            public string out_distance_131;
            public string out_gap_flag_131;
            public string out_node_num_131;
            public string out_intersecting_street_132;
            public string out_second_intersecting_street_132;
            public string out_third_intersecting_street_132;
            public string out_fourth_intersecting_street_132;
            public string out_fifth_intersecting_street_132;
            public string out_xstr_cnt_132;
            public string out_distance_132;
            public string out_gap_flag_132;
            public string out_node_num_132;
            public string out_intersecting_street_133;
            public string out_second_intersecting_street_133;
            public string out_third_intersecting_street_133;
            public string out_fourth_intersecting_street_133;
            public string out_fifth_intersecting_street_133;
            public string out_xstr_cnt_133;
            public string out_distance_133;
            public string out_gap_flag_133;
            public string out_node_num_133;
            public string out_intersecting_street_134;
            public string out_second_intersecting_street_134;
            public string out_third_intersecting_street_134;
            public string out_fourth_intersecting_street_134;
            public string out_fifth_intersecting_street_134;
            public string out_xstr_cnt_134;
            public string out_distance_134;
            public string out_gap_flag_134;
            public string out_node_num_134;
            public string out_intersecting_street_135;
            public string out_second_intersecting_street_135;
            public string out_third_intersecting_street_135;
            public string out_fourth_intersecting_street_135;
            public string out_fifth_intersecting_street_135;
            public string out_xstr_cnt_135;
            public string out_distance_135;
            public string out_gap_flag_135;
            public string out_node_num_135;
            public string out_intersecting_street_136;
            public string out_second_intersecting_street_136;
            public string out_third_intersecting_street_136;
            public string out_fourth_intersecting_street_136;
            public string out_fifth_intersecting_street_136;
            public string out_xstr_cnt_136;
            public string out_distance_136;
            public string out_gap_flag_136;
            public string out_node_num_136;
            public string out_intersecting_street_137;
            public string out_second_intersecting_street_137;
            public string out_third_intersecting_street_137;
            public string out_fourth_intersecting_street_137;
            public string out_fifth_intersecting_street_137;
            public string out_xstr_cnt_137;
            public string out_distance_137;
            public string out_gap_flag_137;
            public string out_node_num_137;
            public string out_intersecting_street_138;
            public string out_second_intersecting_street_138;
            public string out_third_intersecting_street_138;
            public string out_fourth_intersecting_street_138;
            public string out_fifth_intersecting_street_138;
            public string out_xstr_cnt_138;
            public string out_distance_138;
            public string out_gap_flag_138;
            public string out_node_num_138;
            public string out_intersecting_street_139;
            public string out_second_intersecting_street_139;
            public string out_third_intersecting_street_139;
            public string out_fourth_intersecting_street_139;
            public string out_fifth_intersecting_street_139;
            public string out_xstr_cnt_139;
            public string out_distance_139;
            public string out_gap_flag_139;
            public string out_node_num_139;
            public string out_intersecting_street_140;
            public string out_second_intersecting_street_140;
            public string out_third_intersecting_street_140;
            public string out_fourth_intersecting_street_140;
            public string out_fifth_intersecting_street_140;
            public string out_xstr_cnt_140;
            public string out_distance_140;
            public string out_gap_flag_140;
            public string out_node_num_140;
            public string out_intersecting_street_141;
            public string out_second_intersecting_street_141;
            public string out_third_intersecting_street_141;
            public string out_fourth_intersecting_street_141;
            public string out_fifth_intersecting_street_141;
            public string out_xstr_cnt_141;
            public string out_distance_141;
            public string out_gap_flag_141;
            public string out_node_num_141;
            public string out_intersecting_street_142;
            public string out_second_intersecting_street_142;
            public string out_third_intersecting_street_142;
            public string out_fourth_intersecting_street_142;
            public string out_fifth_intersecting_street_142;
            public string out_xstr_cnt_142;
            public string out_distance_142;
            public string out_gap_flag_142;
            public string out_node_num_142;
            public string out_intersecting_street_143;
            public string out_second_intersecting_street_143;
            public string out_third_intersecting_street_143;
            public string out_fourth_intersecting_street_143;
            public string out_fifth_intersecting_street_143;
            public string out_xstr_cnt_143;
            public string out_distance_143;
            public string out_gap_flag_143;
            public string out_node_num_143;
            public string out_intersecting_street_144;
            public string out_second_intersecting_street_144;
            public string out_third_intersecting_street_144;
            public string out_fourth_intersecting_street_144;
            public string out_fifth_intersecting_street_144;
            public string out_xstr_cnt_144;
            public string out_distance_144;
            public string out_gap_flag_144;
            public string out_node_num_144;
            public string out_intersecting_street_145;
            public string out_second_intersecting_street_145;
            public string out_third_intersecting_street_145;
            public string out_fourth_intersecting_street_145;
            public string out_fifth_intersecting_street_145;
            public string out_xstr_cnt_145;
            public string out_distance_145;
            public string out_gap_flag_145;
            public string out_node_num_145;
            public string out_intersecting_street_146;
            public string out_second_intersecting_street_146;
            public string out_third_intersecting_street_146;
            public string out_fourth_intersecting_street_146;
            public string out_fifth_intersecting_street_146;
            public string out_xstr_cnt_146;
            public string out_distance_146;
            public string out_gap_flag_146;
            public string out_node_num_146;
            public string out_intersecting_street_147;
            public string out_second_intersecting_street_147;
            public string out_third_intersecting_street_147;
            public string out_fourth_intersecting_street_147;
            public string out_fifth_intersecting_street_147;
            public string out_xstr_cnt_147;
            public string out_distance_147;
            public string out_gap_flag_147;
            public string out_node_num_147;
            public string out_intersecting_street_148;
            public string out_second_intersecting_street_148;
            public string out_third_intersecting_street_148;
            public string out_fourth_intersecting_street_148;
            public string out_fifth_intersecting_street_148;
            public string out_xstr_cnt_148;
            public string out_distance_148;
            public string out_gap_flag_148;
            public string out_node_num_148;
            public string out_intersecting_street_149;
            public string out_second_intersecting_street_149;
            public string out_third_intersecting_street_149;
            public string out_fourth_intersecting_street_149;
            public string out_fifth_intersecting_street_149;
            public string out_xstr_cnt_149;
            public string out_distance_149;
            public string out_gap_flag_149;
            public string out_node_num_149;
            public string out_intersecting_street_150;
            public string out_second_intersecting_street_150;
            public string out_third_intersecting_street_150;
            public string out_fourth_intersecting_street_150;
            public string out_fifth_intersecting_street_150;
            public string out_xstr_cnt_150;
            public string out_distance_150;
            public string out_gap_flag_150;
            public string out_node_num_150;
            public string out_intersecting_street_151;
            public string out_second_intersecting_street_151;
            public string out_third_intersecting_street_151;
            public string out_fourth_intersecting_street_151;
            public string out_fifth_intersecting_street_151;
            public string out_xstr_cnt_151;
            public string out_distance_151;
            public string out_gap_flag_151;
            public string out_node_num_151;
            public string out_intersecting_street_152;
            public string out_second_intersecting_street_152;
            public string out_third_intersecting_street_152;
            public string out_fourth_intersecting_street_152;
            public string out_fifth_intersecting_street_152;
            public string out_xstr_cnt_152;
            public string out_distance_152;
            public string out_gap_flag_152;
            public string out_node_num_152;
            public string out_intersecting_street_153;
            public string out_second_intersecting_street_153;
            public string out_third_intersecting_street_153;
            public string out_fourth_intersecting_street_153;
            public string out_fifth_intersecting_street_153;
            public string out_xstr_cnt_153;
            public string out_distance_153;
            public string out_gap_flag_153;
            public string out_node_num_153;
            public string out_intersecting_street_154;
            public string out_second_intersecting_street_154;
            public string out_third_intersecting_street_154;
            public string out_fourth_intersecting_street_154;
            public string out_fifth_intersecting_street_154;
            public string out_xstr_cnt_154;
            public string out_distance_154;
            public string out_gap_flag_154;
            public string out_node_num_154;
            public string out_intersecting_street_155;
            public string out_second_intersecting_street_155;
            public string out_third_intersecting_street_155;
            public string out_fourth_intersecting_street_155;
            public string out_fifth_intersecting_street_155;
            public string out_xstr_cnt_155;
            public string out_distance_155;
            public string out_gap_flag_155;
            public string out_node_num_155;
            public string out_intersecting_street_156;
            public string out_second_intersecting_street_156;
            public string out_third_intersecting_street_156;
            public string out_fourth_intersecting_street_156;
            public string out_fifth_intersecting_street_156;
            public string out_xstr_cnt_156;
            public string out_distance_156;
            public string out_gap_flag_156;
            public string out_node_num_156;
            public string out_intersecting_street_157;
            public string out_second_intersecting_street_157;
            public string out_third_intersecting_street_157;
            public string out_fourth_intersecting_street_157;
            public string out_fifth_intersecting_street_157;
            public string out_xstr_cnt_157;
            public string out_distance_157;
            public string out_gap_flag_157;
            public string out_node_num_157;
            public string out_intersecting_street_158;
            public string out_second_intersecting_street_158;
            public string out_third_intersecting_street_158;
            public string out_fourth_intersecting_street_158;
            public string out_fifth_intersecting_street_158;
            public string out_xstr_cnt_158;
            public string out_distance_158;
            public string out_gap_flag_158;
            public string out_node_num_158;
            public string out_intersecting_street_159;
            public string out_second_intersecting_street_159;
            public string out_third_intersecting_street_159;
            public string out_fourth_intersecting_street_159;
            public string out_fifth_intersecting_street_159;
            public string out_xstr_cnt_159;
            public string out_distance_159;
            public string out_gap_flag_159;
            public string out_node_num_159;
            public string out_intersecting_street_160;
            public string out_second_intersecting_street_160;
            public string out_third_intersecting_street_160;
            public string out_fourth_intersecting_street_160;
            public string out_fifth_intersecting_street_160;
            public string out_xstr_cnt_160;
            public string out_distance_160;
            public string out_gap_flag_160;
            public string out_node_num_160;
            public string out_intersecting_street_161;
            public string out_second_intersecting_street_161;
            public string out_third_intersecting_street_161;
            public string out_fourth_intersecting_street_161;
            public string out_fifth_intersecting_street_161;
            public string out_xstr_cnt_161;
            public string out_distance_161;
            public string out_gap_flag_161;
            public string out_node_num_161;
            public string out_intersecting_street_162;
            public string out_second_intersecting_street_162;
            public string out_third_intersecting_street_162;
            public string out_fourth_intersecting_street_162;
            public string out_fifth_intersecting_street_162;
            public string out_xstr_cnt_162;
            public string out_distance_162;
            public string out_gap_flag_162;
            public string out_node_num_162;
            public string out_intersecting_street_163;
            public string out_second_intersecting_street_163;
            public string out_third_intersecting_street_163;
            public string out_fourth_intersecting_street_163;
            public string out_fifth_intersecting_street_163;
            public string out_xstr_cnt_163;
            public string out_distance_163;
            public string out_gap_flag_163;
            public string out_node_num_163;
            public string out_intersecting_street_164;
            public string out_second_intersecting_street_164;
            public string out_third_intersecting_street_164;
            public string out_fourth_intersecting_street_164;
            public string out_fifth_intersecting_street_164;
            public string out_xstr_cnt_164;
            public string out_distance_164;
            public string out_gap_flag_164;
            public string out_node_num_164;
            public string out_intersecting_street_165;
            public string out_second_intersecting_street_165;
            public string out_third_intersecting_street_165;
            public string out_fourth_intersecting_street_165;
            public string out_fifth_intersecting_street_165;
            public string out_xstr_cnt_165;
            public string out_distance_165;
            public string out_gap_flag_165;
            public string out_node_num_165;
            public string out_intersecting_street_166;
            public string out_second_intersecting_street_166;
            public string out_third_intersecting_street_166;
            public string out_fourth_intersecting_street_166;
            public string out_fifth_intersecting_street_166;
            public string out_xstr_cnt_166;
            public string out_distance_166;
            public string out_gap_flag_166;
            public string out_node_num_166;
            public string out_intersecting_street_167;
            public string out_second_intersecting_street_167;
            public string out_third_intersecting_street_167;
            public string out_fourth_intersecting_street_167;
            public string out_fifth_intersecting_street_167;
            public string out_xstr_cnt_167;
            public string out_distance_167;
            public string out_gap_flag_167;
            public string out_node_num_167;
            public string out_intersecting_street_168;
            public string out_second_intersecting_street_168;
            public string out_third_intersecting_street_168;
            public string out_fourth_intersecting_street_168;
            public string out_fifth_intersecting_street_168;
            public string out_xstr_cnt_168;
            public string out_distance_168;
            public string out_gap_flag_168;
            public string out_node_num_168;
            public string out_intersecting_street_169;
            public string out_second_intersecting_street_169;
            public string out_third_intersecting_street_169;
            public string out_fourth_intersecting_street_169;
            public string out_fifth_intersecting_street_169;
            public string out_xstr_cnt_169;
            public string out_distance_169;
            public string out_gap_flag_169;
            public string out_node_num_169;
            public string out_intersecting_street_170;
            public string out_second_intersecting_street_170;
            public string out_third_intersecting_street_170;
            public string out_fourth_intersecting_street_170;
            public string out_fifth_intersecting_street_170;
            public string out_xstr_cnt_170;
            public string out_distance_170;
            public string out_gap_flag_170;
            public string out_node_num_170;
            public string out_intersecting_street_171;
            public string out_second_intersecting_street_171;
            public string out_third_intersecting_street_171;
            public string out_fourth_intersecting_street_171;
            public string out_fifth_intersecting_street_171;
            public string out_xstr_cnt_171;
            public string out_distance_171;
            public string out_gap_flag_171;
            public string out_node_num_171;
            public string out_intersecting_street_172;
            public string out_second_intersecting_street_172;
            public string out_third_intersecting_street_172;
            public string out_fourth_intersecting_street_172;
            public string out_fifth_intersecting_street_172;
            public string out_xstr_cnt_172;
            public string out_distance_172;
            public string out_gap_flag_172;
            public string out_node_num_172;
            public string out_intersecting_street_173;
            public string out_second_intersecting_street_173;
            public string out_third_intersecting_street_173;
            public string out_fourth_intersecting_street_173;
            public string out_fifth_intersecting_street_173;
            public string out_xstr_cnt_173;
            public string out_distance_173;
            public string out_gap_flag_173;
            public string out_node_num_173;
            public string out_intersecting_street_174;
            public string out_second_intersecting_street_174;
            public string out_third_intersecting_street_174;
            public string out_fourth_intersecting_street_174;
            public string out_fifth_intersecting_street_174;
            public string out_xstr_cnt_174;
            public string out_distance_174;
            public string out_gap_flag_174;
            public string out_node_num_174;
            public string out_intersecting_street_175;
            public string out_second_intersecting_street_175;
            public string out_third_intersecting_street_175;
            public string out_fourth_intersecting_street_175;
            public string out_fifth_intersecting_street_175;
            public string out_xstr_cnt_175;
            public string out_distance_175;
            public string out_gap_flag_175;
            public string out_node_num_175;
            public string out_intersecting_street_176;
            public string out_second_intersecting_street_176;
            public string out_third_intersecting_street_176;
            public string out_fourth_intersecting_street_176;
            public string out_fifth_intersecting_street_176;
            public string out_xstr_cnt_176;
            public string out_distance_176;
            public string out_gap_flag_176;
            public string out_node_num_176;
            public string out_intersecting_street_177;
            public string out_second_intersecting_street_177;
            public string out_third_intersecting_street_177;
            public string out_fourth_intersecting_street_177;
            public string out_fifth_intersecting_street_177;
            public string out_xstr_cnt_177;
            public string out_distance_177;
            public string out_gap_flag_177;
            public string out_node_num_177;
            public string out_intersecting_street_178;
            public string out_second_intersecting_street_178;
            public string out_third_intersecting_street_178;
            public string out_fourth_intersecting_street_178;
            public string out_fifth_intersecting_street_178;
            public string out_xstr_cnt_178;
            public string out_distance_178;
            public string out_gap_flag_178;
            public string out_node_num_178;
            public string out_intersecting_street_179;
            public string out_second_intersecting_street_179;
            public string out_third_intersecting_street_179;
            public string out_fourth_intersecting_street_179;
            public string out_fifth_intersecting_street_179;
            public string out_xstr_cnt_179;
            public string out_distance_179;
            public string out_gap_flag_179;
            public string out_node_num_179;
            public string out_intersecting_street_180;
            public string out_second_intersecting_street_180;
            public string out_third_intersecting_street_180;
            public string out_fourth_intersecting_street_180;
            public string out_fifth_intersecting_street_180;
            public string out_xstr_cnt_180;
            public string out_distance_180;
            public string out_gap_flag_180;
            public string out_node_num_180;
            public string out_intersecting_street_181;
            public string out_second_intersecting_street_181;
            public string out_third_intersecting_street_181;
            public string out_fourth_intersecting_street_181;
            public string out_fifth_intersecting_street_181;
            public string out_xstr_cnt_181;
            public string out_distance_181;
            public string out_gap_flag_181;
            public string out_node_num_181;
            public string out_intersecting_street_182;
            public string out_second_intersecting_street_182;
            public string out_third_intersecting_street_182;
            public string out_fourth_intersecting_street_182;
            public string out_fifth_intersecting_street_182;
            public string out_xstr_cnt_182;
            public string out_distance_182;
            public string out_gap_flag_182;
            public string out_node_num_182;
            public string out_intersecting_street_183;
            public string out_second_intersecting_street_183;
            public string out_third_intersecting_street_183;
            public string out_fourth_intersecting_street_183;
            public string out_fifth_intersecting_street_183;
            public string out_xstr_cnt_183;
            public string out_distance_183;
            public string out_gap_flag_183;
            public string out_node_num_183;
            public string out_intersecting_street_184;
            public string out_second_intersecting_street_184;
            public string out_third_intersecting_street_184;
            public string out_fourth_intersecting_street_184;
            public string out_fifth_intersecting_street_184;
            public string out_xstr_cnt_184;
            public string out_distance_184;
            public string out_gap_flag_184;
            public string out_node_num_184;
            public string out_intersecting_street_185;
            public string out_second_intersecting_street_185;
            public string out_third_intersecting_street_185;
            public string out_fourth_intersecting_street_185;
            public string out_fifth_intersecting_street_185;
            public string out_xstr_cnt_185;
            public string out_distance_185;
            public string out_gap_flag_185;
            public string out_node_num_185;
            public string out_intersecting_street_186;
            public string out_second_intersecting_street_186;
            public string out_third_intersecting_street_186;
            public string out_fourth_intersecting_street_186;
            public string out_fifth_intersecting_street_186;
            public string out_xstr_cnt_186;
            public string out_distance_186;
            public string out_gap_flag_186;
            public string out_node_num_186;
            public string out_intersecting_street_187;
            public string out_second_intersecting_street_187;
            public string out_third_intersecting_street_187;
            public string out_fourth_intersecting_street_187;
            public string out_fifth_intersecting_street_187;
            public string out_xstr_cnt_187;
            public string out_distance_187;
            public string out_gap_flag_187;
            public string out_node_num_187;
            public string out_intersecting_street_188;
            public string out_second_intersecting_street_188;
            public string out_third_intersecting_street_188;
            public string out_fourth_intersecting_street_188;
            public string out_fifth_intersecting_street_188;
            public string out_xstr_cnt_188;
            public string out_distance_188;
            public string out_gap_flag_188;
            public string out_node_num_188;
            public string out_intersecting_street_189;
            public string out_second_intersecting_street_189;
            public string out_third_intersecting_street_189;
            public string out_fourth_intersecting_street_189;
            public string out_fifth_intersecting_street_189;
            public string out_xstr_cnt_189;
            public string out_distance_189;
            public string out_gap_flag_189;
            public string out_node_num_189;
            public string out_intersecting_street_190;
            public string out_second_intersecting_street_190;
            public string out_third_intersecting_street_190;
            public string out_fourth_intersecting_street_190;
            public string out_fifth_intersecting_street_190;
            public string out_xstr_cnt_190;
            public string out_distance_190;
            public string out_gap_flag_190;
            public string out_node_num_190;
            public string out_intersecting_street_191;
            public string out_second_intersecting_street_191;
            public string out_third_intersecting_street_191;
            public string out_fourth_intersecting_street_191;
            public string out_fifth_intersecting_street_191;
            public string out_xstr_cnt_191;
            public string out_distance_191;
            public string out_gap_flag_191;
            public string out_node_num_191;
            public string out_intersecting_street_192;
            public string out_second_intersecting_street_192;
            public string out_third_intersecting_street_192;
            public string out_fourth_intersecting_street_192;
            public string out_fifth_intersecting_street_192;
            public string out_xstr_cnt_192;
            public string out_distance_192;
            public string out_gap_flag_192;
            public string out_node_num_192;
            public string out_intersecting_street_193;
            public string out_second_intersecting_street_193;
            public string out_third_intersecting_street_193;
            public string out_fourth_intersecting_street_193;
            public string out_fifth_intersecting_street_193;
            public string out_xstr_cnt_193;
            public string out_distance_193;
            public string out_gap_flag_193;
            public string out_node_num_193;
            public string out_intersecting_street_194;
            public string out_second_intersecting_street_194;
            public string out_third_intersecting_street_194;
            public string out_fourth_intersecting_street_194;
            public string out_fifth_intersecting_street_194;
            public string out_xstr_cnt_194;
            public string out_distance_194;
            public string out_gap_flag_194;
            public string out_node_num_194;
            public string out_intersecting_street_195;
            public string out_second_intersecting_street_195;
            public string out_third_intersecting_street_195;
            public string out_fourth_intersecting_street_195;
            public string out_fifth_intersecting_street_195;
            public string out_xstr_cnt_195;
            public string out_distance_195;
            public string out_gap_flag_195;
            public string out_node_num_195;
            public string out_intersecting_street_196;
            public string out_second_intersecting_street_196;
            public string out_third_intersecting_street_196;
            public string out_fourth_intersecting_street_196;
            public string out_fifth_intersecting_street_196;
            public string out_xstr_cnt_196;
            public string out_distance_196;
            public string out_gap_flag_196;
            public string out_node_num_196;
            public string out_intersecting_street_197;
            public string out_second_intersecting_street_197;
            public string out_third_intersecting_street_197;
            public string out_fourth_intersecting_street_197;
            public string out_fifth_intersecting_street_197;
            public string out_xstr_cnt_197;
            public string out_distance_197;
            public string out_gap_flag_197;
            public string out_node_num_197;
            public string out_intersecting_street_198;
            public string out_second_intersecting_street_198;
            public string out_third_intersecting_street_198;
            public string out_fourth_intersecting_street_198;
            public string out_fifth_intersecting_street_198;
            public string out_xstr_cnt_198;
            public string out_distance_198;
            public string out_gap_flag_198;
            public string out_node_num_198;
            public string out_intersecting_street_199;
            public string out_second_intersecting_street_199;
            public string out_third_intersecting_street_199;
            public string out_fourth_intersecting_street_199;
            public string out_fifth_intersecting_street_199;
            public string out_xstr_cnt_199;
            public string out_distance_199;
            public string out_gap_flag_199;
            public string out_node_num_199;
            public string out_intersecting_street_200;
            public string out_second_intersecting_street_200;
            public string out_third_intersecting_street_200;
            public string out_fourth_intersecting_street_200;
            public string out_fifth_intersecting_street_200;
            public string out_xstr_cnt_200;
            public string out_distance_200;
            public string out_gap_flag_200;
            public string out_node_num_200;
            public string out_intersecting_street_201;
            public string out_second_intersecting_street_201;
            public string out_third_intersecting_street_201;
            public string out_fourth_intersecting_street_201;
            public string out_fifth_intersecting_street_201;
            public string out_xstr_cnt_201;
            public string out_distance_201;
            public string out_gap_flag_201;
            public string out_node_num_201;
            public string out_intersecting_street_202;
            public string out_second_intersecting_street_202;
            public string out_third_intersecting_street_202;
            public string out_fourth_intersecting_street_202;
            public string out_fifth_intersecting_street_202;
            public string out_xstr_cnt_202;
            public string out_distance_202;
            public string out_gap_flag_202;
            public string out_node_num_202;
            public string out_intersecting_street_203;
            public string out_second_intersecting_street_203;
            public string out_third_intersecting_street_203;
            public string out_fourth_intersecting_street_203;
            public string out_fifth_intersecting_street_203;
            public string out_xstr_cnt_203;
            public string out_distance_203;
            public string out_gap_flag_203;
            public string out_node_num_203;
            public string out_intersecting_street_204;
            public string out_second_intersecting_street_204;
            public string out_third_intersecting_street_204;
            public string out_fourth_intersecting_street_204;
            public string out_fifth_intersecting_street_204;
            public string out_xstr_cnt_204;
            public string out_distance_204;
            public string out_gap_flag_204;
            public string out_node_num_204;
            public string out_intersecting_street_205;
            public string out_second_intersecting_street_205;
            public string out_third_intersecting_street_205;
            public string out_fourth_intersecting_street_205;
            public string out_fifth_intersecting_street_205;
            public string out_xstr_cnt_205;
            public string out_distance_205;
            public string out_gap_flag_205;
            public string out_node_num_205;
            public string out_intersecting_street_206;
            public string out_second_intersecting_street_206;
            public string out_third_intersecting_street_206;
            public string out_fourth_intersecting_street_206;
            public string out_fifth_intersecting_street_206;
            public string out_xstr_cnt_206;
            public string out_distance_206;
            public string out_gap_flag_206;
            public string out_node_num_206;
            public string out_intersecting_street_207;
            public string out_second_intersecting_street_207;
            public string out_third_intersecting_street_207;
            public string out_fourth_intersecting_street_207;
            public string out_fifth_intersecting_street_207;
            public string out_xstr_cnt_207;
            public string out_distance_207;
            public string out_gap_flag_207;
            public string out_node_num_207;
            public string out_intersecting_street_208;
            public string out_second_intersecting_street_208;
            public string out_third_intersecting_street_208;
            public string out_fourth_intersecting_street_208;
            public string out_fifth_intersecting_street_208;
            public string out_xstr_cnt_208;
            public string out_distance_208;
            public string out_gap_flag_208;
            public string out_node_num_208;
            public string out_intersecting_street_209;
            public string out_second_intersecting_street_209;
            public string out_third_intersecting_street_209;
            public string out_fourth_intersecting_street_209;
            public string out_fifth_intersecting_street_209;
            public string out_xstr_cnt_209;
            public string out_distance_209;
            public string out_gap_flag_209;
            public string out_node_num_209;
            public string out_intersecting_street_210;
            public string out_second_intersecting_street_210;
            public string out_third_intersecting_street_210;
            public string out_fourth_intersecting_street_210;
            public string out_fifth_intersecting_street_210;
            public string out_xstr_cnt_210;
            public string out_distance_210;
            public string out_gap_flag_210;
            public string out_node_num_210;
            public string out_intersecting_street_211;
            public string out_second_intersecting_street_211;
            public string out_third_intersecting_street_211;
            public string out_fourth_intersecting_street_211;
            public string out_fifth_intersecting_street_211;
            public string out_xstr_cnt_211;
            public string out_distance_211;
            public string out_gap_flag_211;
            public string out_node_num_211;
            public string out_intersecting_street_212;
            public string out_second_intersecting_street_212;
            public string out_third_intersecting_street_212;
            public string out_fourth_intersecting_street_212;
            public string out_fifth_intersecting_street_212;
            public string out_xstr_cnt_212;
            public string out_distance_212;
            public string out_gap_flag_212;
            public string out_node_num_212;
            public string out_intersecting_street_213;
            public string out_second_intersecting_street_213;
            public string out_third_intersecting_street_213;
            public string out_fourth_intersecting_street_213;
            public string out_fifth_intersecting_street_213;
            public string out_xstr_cnt_213;
            public string out_distance_213;
            public string out_gap_flag_213;
            public string out_node_num_213;
            public string out_intersecting_street_214;
            public string out_second_intersecting_street_214;
            public string out_third_intersecting_street_214;
            public string out_fourth_intersecting_street_214;
            public string out_fifth_intersecting_street_214;
            public string out_xstr_cnt_214;
            public string out_distance_214;
            public string out_gap_flag_214;
            public string out_node_num_214;
            public string out_intersecting_street_215;
            public string out_second_intersecting_street_215;
            public string out_third_intersecting_street_215;
            public string out_fourth_intersecting_street_215;
            public string out_fifth_intersecting_street_215;
            public string out_xstr_cnt_215;
            public string out_distance_215;
            public string out_gap_flag_215;
            public string out_node_num_215;
            public string out_intersecting_street_216;
            public string out_second_intersecting_street_216;
            public string out_third_intersecting_street_216;
            public string out_fourth_intersecting_street_216;
            public string out_fifth_intersecting_street_216;
            public string out_xstr_cnt_216;
            public string out_distance_216;
            public string out_gap_flag_216;
            public string out_node_num_216;
            public string out_intersecting_street_217;
            public string out_second_intersecting_street_217;
            public string out_third_intersecting_street_217;
            public string out_fourth_intersecting_street_217;
            public string out_fifth_intersecting_street_217;
            public string out_xstr_cnt_217;
            public string out_distance_217;
            public string out_gap_flag_217;
            public string out_node_num_217;
            public string out_intersecting_street_218;
            public string out_second_intersecting_street_218;
            public string out_third_intersecting_street_218;
            public string out_fourth_intersecting_street_218;
            public string out_fifth_intersecting_street_218;
            public string out_xstr_cnt_218;
            public string out_distance_218;
            public string out_gap_flag_218;
            public string out_node_num_218;
            public string out_intersecting_street_219;
            public string out_second_intersecting_street_219;
            public string out_third_intersecting_street_219;
            public string out_fourth_intersecting_street_219;
            public string out_fifth_intersecting_street_219;
            public string out_xstr_cnt_219;
            public string out_distance_219;
            public string out_gap_flag_219;
            public string out_node_num_219;
            public string out_intersecting_street_220;
            public string out_second_intersecting_street_220;
            public string out_third_intersecting_street_220;
            public string out_fourth_intersecting_street_220;
            public string out_fifth_intersecting_street_220;
            public string out_xstr_cnt_220;
            public string out_distance_220;
            public string out_gap_flag_220;
            public string out_node_num_220;
            public string out_intersecting_street_221;
            public string out_second_intersecting_street_221;
            public string out_third_intersecting_street_221;
            public string out_fourth_intersecting_street_221;
            public string out_fifth_intersecting_street_221;
            public string out_xstr_cnt_221;
            public string out_distance_221;
            public string out_gap_flag_221;
            public string out_node_num_221;
            public string out_intersecting_street_222;
            public string out_second_intersecting_street_222;
            public string out_third_intersecting_street_222;
            public string out_fourth_intersecting_street_222;
            public string out_fifth_intersecting_street_222;
            public string out_xstr_cnt_222;
            public string out_distance_222;
            public string out_gap_flag_222;
            public string out_node_num_222;
            public string out_intersecting_street_223;
            public string out_second_intersecting_street_223;
            public string out_third_intersecting_street_223;
            public string out_fourth_intersecting_street_223;
            public string out_fifth_intersecting_street_223;
            public string out_xstr_cnt_223;
            public string out_distance_223;
            public string out_gap_flag_223;
            public string out_node_num_223;
            public string out_intersecting_street_224;
            public string out_second_intersecting_street_224;
            public string out_third_intersecting_street_224;
            public string out_fourth_intersecting_street_224;
            public string out_fifth_intersecting_street_224;
            public string out_xstr_cnt_224;
            public string out_distance_224;
            public string out_gap_flag_224;
            public string out_node_num_224;
            public string out_intersecting_street_225;
            public string out_second_intersecting_street_225;
            public string out_third_intersecting_street_225;
            public string out_fourth_intersecting_street_225;
            public string out_fifth_intersecting_street_225;
            public string out_xstr_cnt_225;
            public string out_distance_225;
            public string out_gap_flag_225;
            public string out_node_num_225;
            public string out_intersecting_street_226;
            public string out_second_intersecting_street_226;
            public string out_third_intersecting_street_226;
            public string out_fourth_intersecting_street_226;
            public string out_fifth_intersecting_street_226;
            public string out_xstr_cnt_226;
            public string out_distance_226;
            public string out_gap_flag_226;
            public string out_node_num_226;
            public string out_intersecting_street_227;
            public string out_second_intersecting_street_227;
            public string out_third_intersecting_street_227;
            public string out_fourth_intersecting_street_227;
            public string out_fifth_intersecting_street_227;
            public string out_xstr_cnt_227;
            public string out_distance_227;
            public string out_gap_flag_227;
            public string out_node_num_227;
            public string out_intersecting_street_228;
            public string out_second_intersecting_street_228;
            public string out_third_intersecting_street_228;
            public string out_fourth_intersecting_street_228;
            public string out_fifth_intersecting_street_228;
            public string out_xstr_cnt_228;
            public string out_distance_228;
            public string out_gap_flag_228;
            public string out_node_num_228;
            public string out_intersecting_street_229;
            public string out_second_intersecting_street_229;
            public string out_third_intersecting_street_229;
            public string out_fourth_intersecting_street_229;
            public string out_fifth_intersecting_street_229;
            public string out_xstr_cnt_229;
            public string out_distance_229;
            public string out_gap_flag_229;
            public string out_node_num_229;
            public string out_intersecting_street_230;
            public string out_second_intersecting_street_230;
            public string out_third_intersecting_street_230;
            public string out_fourth_intersecting_street_230;
            public string out_fifth_intersecting_street_230;
            public string out_xstr_cnt_230;
            public string out_distance_230;
            public string out_gap_flag_230;
            public string out_node_num_230;
            public string out_intersecting_street_231;
            public string out_second_intersecting_street_231;
            public string out_third_intersecting_street_231;
            public string out_fourth_intersecting_street_231;
            public string out_fifth_intersecting_street_231;
            public string out_xstr_cnt_231;
            public string out_distance_231;
            public string out_gap_flag_231;
            public string out_node_num_231;
            public string out_intersecting_street_232;
            public string out_second_intersecting_street_232;
            public string out_third_intersecting_street_232;
            public string out_fourth_intersecting_street_232;
            public string out_fifth_intersecting_street_232;
            public string out_xstr_cnt_232;
            public string out_distance_232;
            public string out_gap_flag_232;
            public string out_node_num_232;
            public string out_intersecting_street_233;
            public string out_second_intersecting_street_233;
            public string out_third_intersecting_street_233;
            public string out_fourth_intersecting_street_233;
            public string out_fifth_intersecting_street_233;
            public string out_xstr_cnt_233;
            public string out_distance_233;
            public string out_gap_flag_233;
            public string out_node_num_233;
            public string out_intersecting_street_234;
            public string out_second_intersecting_street_234;
            public string out_third_intersecting_street_234;
            public string out_fourth_intersecting_street_234;
            public string out_fifth_intersecting_street_234;
            public string out_xstr_cnt_234;
            public string out_distance_234;
            public string out_gap_flag_234;
            public string out_node_num_234;
            public string out_intersecting_street_235;
            public string out_second_intersecting_street_235;
            public string out_third_intersecting_street_235;
            public string out_fourth_intersecting_street_235;
            public string out_fifth_intersecting_street_235;
            public string out_xstr_cnt_235;
            public string out_distance_235;
            public string out_gap_flag_235;
            public string out_node_num_235;
            public string out_intersecting_street_236;
            public string out_second_intersecting_street_236;
            public string out_third_intersecting_street_236;
            public string out_fourth_intersecting_street_236;
            public string out_fifth_intersecting_street_236;
            public string out_xstr_cnt_236;
            public string out_distance_236;
            public string out_gap_flag_236;
            public string out_node_num_236;
            public string out_intersecting_street_237;
            public string out_second_intersecting_street_237;
            public string out_third_intersecting_street_237;
            public string out_fourth_intersecting_street_237;
            public string out_fifth_intersecting_street_237;
            public string out_xstr_cnt_237;
            public string out_distance_237;
            public string out_gap_flag_237;
            public string out_node_num_237;
            public string out_intersecting_street_238;
            public string out_second_intersecting_street_238;
            public string out_third_intersecting_street_238;
            public string out_fourth_intersecting_street_238;
            public string out_fifth_intersecting_street_238;
            public string out_xstr_cnt_238;
            public string out_distance_238;
            public string out_gap_flag_238;
            public string out_node_num_238;
            public string out_intersecting_street_239;
            public string out_second_intersecting_street_239;
            public string out_third_intersecting_street_239;
            public string out_fourth_intersecting_street_239;
            public string out_fifth_intersecting_street_239;
            public string out_xstr_cnt_239;
            public string out_distance_239;
            public string out_gap_flag_239;
            public string out_node_num_239;
            public string out_intersecting_street_240;
            public string out_second_intersecting_street_240;
            public string out_third_intersecting_street_240;
            public string out_fourth_intersecting_street_240;
            public string out_fifth_intersecting_street_240;
            public string out_xstr_cnt_240;
            public string out_distance_240;
            public string out_gap_flag_240;
            public string out_node_num_240;
            public string out_intersecting_street_241;
            public string out_second_intersecting_street_241;
            public string out_third_intersecting_street_241;
            public string out_fourth_intersecting_street_241;
            public string out_fifth_intersecting_street_241;
            public string out_xstr_cnt_241;
            public string out_distance_241;
            public string out_gap_flag_241;
            public string out_node_num_241;
            public string out_intersecting_street_242;
            public string out_second_intersecting_street_242;
            public string out_third_intersecting_street_242;
            public string out_fourth_intersecting_street_242;
            public string out_fifth_intersecting_street_242;
            public string out_xstr_cnt_242;
            public string out_distance_242;
            public string out_gap_flag_242;
            public string out_node_num_242;
            public string out_intersecting_street_243;
            public string out_second_intersecting_street_243;
            public string out_third_intersecting_street_243;
            public string out_fourth_intersecting_street_243;
            public string out_fifth_intersecting_street_243;
            public string out_xstr_cnt_243;
            public string out_distance_243;
            public string out_gap_flag_243;
            public string out_node_num_243;
            public string out_intersecting_street_244;
            public string out_second_intersecting_street_244;
            public string out_third_intersecting_street_244;
            public string out_fourth_intersecting_street_244;
            public string out_fifth_intersecting_street_244;
            public string out_xstr_cnt_244;
            public string out_distance_244;
            public string out_gap_flag_244;
            public string out_node_num_244;
            public string out_intersecting_street_245;
            public string out_second_intersecting_street_245;
            public string out_third_intersecting_street_245;
            public string out_fourth_intersecting_street_245;
            public string out_fifth_intersecting_street_245;
            public string out_xstr_cnt_245;
            public string out_distance_245;
            public string out_gap_flag_245;
            public string out_node_num_245;
            public string out_intersecting_street_246;
            public string out_second_intersecting_street_246;
            public string out_third_intersecting_street_246;
            public string out_fourth_intersecting_street_246;
            public string out_fifth_intersecting_street_246;
            public string out_xstr_cnt_246;
            public string out_distance_246;
            public string out_gap_flag_246;
            public string out_node_num_246;
            public string out_intersecting_street_247;
            public string out_second_intersecting_street_247;
            public string out_third_intersecting_street_247;
            public string out_fourth_intersecting_street_247;
            public string out_fifth_intersecting_street_247;
            public string out_xstr_cnt_247;
            public string out_distance_247;
            public string out_gap_flag_247;
            public string out_node_num_247;
            public string out_intersecting_street_248;
            public string out_second_intersecting_street_248;
            public string out_third_intersecting_street_248;
            public string out_fourth_intersecting_street_248;
            public string out_fifth_intersecting_street_248;
            public string out_xstr_cnt_248;
            public string out_distance_248;
            public string out_gap_flag_248;
            public string out_node_num_248;
            public string out_intersecting_street_249;
            public string out_second_intersecting_street_249;
            public string out_third_intersecting_street_249;
            public string out_fourth_intersecting_street_249;
            public string out_fifth_intersecting_street_249;
            public string out_xstr_cnt_249;
            public string out_distance_249;
            public string out_gap_flag_249;
            public string out_node_num_249;
            public string out_intersecting_street_250;
            public string out_second_intersecting_street_250;
            public string out_third_intersecting_street_250;
            public string out_fourth_intersecting_street_250;
            public string out_fifth_intersecting_street_250;
            public string out_xstr_cnt_250;
            public string out_distance_250;
            public string out_gap_flag_250;
            public string out_node_num_250;
            public string out_intersecting_street_251;
            public string out_second_intersecting_street_251;
            public string out_third_intersecting_street_251;
            public string out_fourth_intersecting_street_251;
            public string out_fifth_intersecting_street_251;
            public string out_xstr_cnt_251;
            public string out_distance_251;
            public string out_gap_flag_251;
            public string out_node_num_251;
            public string out_intersecting_street_252;
            public string out_second_intersecting_street_252;
            public string out_third_intersecting_street_252;
            public string out_fourth_intersecting_street_252;
            public string out_fifth_intersecting_street_252;
            public string out_xstr_cnt_252;
            public string out_distance_252;
            public string out_gap_flag_252;
            public string out_node_num_252;
            public string out_intersecting_street_253;
            public string out_second_intersecting_street_253;
            public string out_third_intersecting_street_253;
            public string out_fourth_intersecting_street_253;
            public string out_fifth_intersecting_street_253;
            public string out_xstr_cnt_253;
            public string out_distance_253;
            public string out_gap_flag_253;
            public string out_node_num_253;
            public string out_intersecting_street_254;
            public string out_second_intersecting_street_254;
            public string out_third_intersecting_street_254;
            public string out_fourth_intersecting_street_254;
            public string out_fifth_intersecting_street_254;
            public string out_xstr_cnt_254;
            public string out_distance_254;
            public string out_gap_flag_254;
            public string out_node_num_254;
            public string out_intersecting_street_255;
            public string out_second_intersecting_street_255;
            public string out_third_intersecting_street_255;
            public string out_fourth_intersecting_street_255;
            public string out_fifth_intersecting_street_255;
            public string out_xstr_cnt_255;
            public string out_distance_255;
            public string out_gap_flag_255;
            public string out_node_num_255;
            public string out_intersecting_street_256;
            public string out_second_intersecting_street_256;
            public string out_third_intersecting_street_256;
            public string out_fourth_intersecting_street_256;
            public string out_fifth_intersecting_street_256;
            public string out_xstr_cnt_256;
            public string out_distance_256;
            public string out_gap_flag_256;
            public string out_node_num_256;
            public string out_intersecting_street_257;
            public string out_second_intersecting_street_257;
            public string out_third_intersecting_street_257;
            public string out_fourth_intersecting_street_257;
            public string out_fifth_intersecting_street_257;
            public string out_xstr_cnt_257;
            public string out_distance_257;
            public string out_gap_flag_257;
            public string out_node_num_257;
            public string out_intersecting_street_258;
            public string out_second_intersecting_street_258;
            public string out_third_intersecting_street_258;
            public string out_fourth_intersecting_street_258;
            public string out_fifth_intersecting_street_258;
            public string out_xstr_cnt_258;
            public string out_distance_258;
            public string out_gap_flag_258;
            public string out_node_num_258;
            public string out_intersecting_street_259;
            public string out_second_intersecting_street_259;
            public string out_third_intersecting_street_259;
            public string out_fourth_intersecting_street_259;
            public string out_fifth_intersecting_street_259;
            public string out_xstr_cnt_259;
            public string out_distance_259;
            public string out_gap_flag_259;
            public string out_node_num_259;
            public string out_intersecting_street_260;
            public string out_second_intersecting_street_260;
            public string out_third_intersecting_street_260;
            public string out_fourth_intersecting_street_260;
            public string out_fifth_intersecting_street_260;
            public string out_xstr_cnt_260;
            public string out_distance_260;
            public string out_gap_flag_260;
            public string out_node_num_260;
            public string out_intersecting_street_261;
            public string out_second_intersecting_street_261;
            public string out_third_intersecting_street_261;
            public string out_fourth_intersecting_street_261;
            public string out_fifth_intersecting_street_261;
            public string out_xstr_cnt_261;
            public string out_distance_261;
            public string out_gap_flag_261;
            public string out_node_num_261;
            public string out_intersecting_street_262;
            public string out_second_intersecting_street_262;
            public string out_third_intersecting_street_262;
            public string out_fourth_intersecting_street_262;
            public string out_fifth_intersecting_street_262;
            public string out_xstr_cnt_262;
            public string out_distance_262;
            public string out_gap_flag_262;
            public string out_node_num_262;
            public string out_intersecting_street_263;
            public string out_second_intersecting_street_263;
            public string out_third_intersecting_street_263;
            public string out_fourth_intersecting_street_263;
            public string out_fifth_intersecting_street_263;
            public string out_xstr_cnt_263;
            public string out_distance_263;
            public string out_gap_flag_263;
            public string out_node_num_263;
            public string out_intersecting_street_264;
            public string out_second_intersecting_street_264;
            public string out_third_intersecting_street_264;
            public string out_fourth_intersecting_street_264;
            public string out_fifth_intersecting_street_264;
            public string out_xstr_cnt_264;
            public string out_distance_264;
            public string out_gap_flag_264;
            public string out_node_num_264;
            public string out_intersecting_street_265;
            public string out_second_intersecting_street_265;
            public string out_third_intersecting_street_265;
            public string out_fourth_intersecting_street_265;
            public string out_fifth_intersecting_street_265;
            public string out_xstr_cnt_265;
            public string out_distance_265;
            public string out_gap_flag_265;
            public string out_node_num_265;
            public string out_intersecting_street_266;
            public string out_second_intersecting_street_266;
            public string out_third_intersecting_street_266;
            public string out_fourth_intersecting_street_266;
            public string out_fifth_intersecting_street_266;
            public string out_xstr_cnt_266;
            public string out_distance_266;
            public string out_gap_flag_266;
            public string out_node_num_266;
            public string out_intersecting_street_267;
            public string out_second_intersecting_street_267;
            public string out_third_intersecting_street_267;
            public string out_fourth_intersecting_street_267;
            public string out_fifth_intersecting_street_267;
            public string out_xstr_cnt_267;
            public string out_distance_267;
            public string out_gap_flag_267;
            public string out_node_num_267;
            public string out_intersecting_street_268;
            public string out_second_intersecting_street_268;
            public string out_third_intersecting_street_268;
            public string out_fourth_intersecting_street_268;
            public string out_fifth_intersecting_street_268;
            public string out_xstr_cnt_268;
            public string out_distance_268;
            public string out_gap_flag_268;
            public string out_node_num_268;
            public string out_intersecting_street_269;
            public string out_second_intersecting_street_269;
            public string out_third_intersecting_street_269;
            public string out_fourth_intersecting_street_269;
            public string out_fifth_intersecting_street_269;
            public string out_xstr_cnt_269;
            public string out_distance_269;
            public string out_gap_flag_269;
            public string out_node_num_269;
            public string out_intersecting_street_270;
            public string out_second_intersecting_street_270;
            public string out_third_intersecting_street_270;
            public string out_fourth_intersecting_street_270;
            public string out_fifth_intersecting_street_270;
            public string out_xstr_cnt_270;
            public string out_distance_270;
            public string out_gap_flag_270;
            public string out_node_num_270;
            public string out_intersecting_street_271;
            public string out_second_intersecting_street_271;
            public string out_third_intersecting_street_271;
            public string out_fourth_intersecting_street_271;
            public string out_fifth_intersecting_street_271;
            public string out_xstr_cnt_271;
            public string out_distance_271;
            public string out_gap_flag_271;
            public string out_node_num_271;
            public string out_intersecting_street_272;
            public string out_second_intersecting_street_272;
            public string out_third_intersecting_street_272;
            public string out_fourth_intersecting_street_272;
            public string out_fifth_intersecting_street_272;
            public string out_xstr_cnt_272;
            public string out_distance_272;
            public string out_gap_flag_272;
            public string out_node_num_272;
            public string out_intersecting_street_273;
            public string out_second_intersecting_street_273;
            public string out_third_intersecting_street_273;
            public string out_fourth_intersecting_street_273;
            public string out_fifth_intersecting_street_273;
            public string out_xstr_cnt_273;
            public string out_distance_273;
            public string out_gap_flag_273;
            public string out_node_num_273;
            public string out_intersecting_street_274;
            public string out_second_intersecting_street_274;
            public string out_third_intersecting_street_274;
            public string out_fourth_intersecting_street_274;
            public string out_fifth_intersecting_street_274;
            public string out_xstr_cnt_274;
            public string out_distance_274;
            public string out_gap_flag_274;
            public string out_node_num_274;
            public string out_intersecting_street_275;
            public string out_second_intersecting_street_275;
            public string out_third_intersecting_street_275;
            public string out_fourth_intersecting_street_275;
            public string out_fifth_intersecting_street_275;
            public string out_xstr_cnt_275;
            public string out_distance_275;
            public string out_gap_flag_275;
            public string out_node_num_275;
            public string out_intersecting_street_276;
            public string out_second_intersecting_street_276;
            public string out_third_intersecting_street_276;
            public string out_fourth_intersecting_street_276;
            public string out_fifth_intersecting_street_276;
            public string out_xstr_cnt_276;
            public string out_distance_276;
            public string out_gap_flag_276;
            public string out_node_num_276;
            public string out_intersecting_street_277;
            public string out_second_intersecting_street_277;
            public string out_third_intersecting_street_277;
            public string out_fourth_intersecting_street_277;
            public string out_fifth_intersecting_street_277;
            public string out_xstr_cnt_277;
            public string out_distance_277;
            public string out_gap_flag_277;
            public string out_node_num_277;
            public string out_intersecting_street_278;
            public string out_second_intersecting_street_278;
            public string out_third_intersecting_street_278;
            public string out_fourth_intersecting_street_278;
            public string out_fifth_intersecting_street_278;
            public string out_xstr_cnt_278;
            public string out_distance_278;
            public string out_gap_flag_278;
            public string out_node_num_278;
            public string out_intersecting_street_279;
            public string out_second_intersecting_street_279;
            public string out_third_intersecting_street_279;
            public string out_fourth_intersecting_street_279;
            public string out_fifth_intersecting_street_279;
            public string out_xstr_cnt_279;
            public string out_distance_279;
            public string out_gap_flag_279;
            public string out_node_num_279;
            public string out_intersecting_street_280;
            public string out_second_intersecting_street_280;
            public string out_third_intersecting_street_280;
            public string out_fourth_intersecting_street_280;
            public string out_fifth_intersecting_street_280;
            public string out_xstr_cnt_280;
            public string out_distance_280;
            public string out_gap_flag_280;
            public string out_node_num_280;
            public string out_intersecting_street_281;
            public string out_second_intersecting_street_281;
            public string out_third_intersecting_street_281;
            public string out_fourth_intersecting_street_281;
            public string out_fifth_intersecting_street_281;
            public string out_xstr_cnt_281;
            public string out_distance_281;
            public string out_gap_flag_281;
            public string out_node_num_281;
            public string out_intersecting_street_282;
            public string out_second_intersecting_street_282;
            public string out_third_intersecting_street_282;
            public string out_fourth_intersecting_street_282;
            public string out_fifth_intersecting_street_282;
            public string out_xstr_cnt_282;
            public string out_distance_282;
            public string out_gap_flag_282;
            public string out_node_num_282;
            public string out_intersecting_street_283;
            public string out_second_intersecting_street_283;
            public string out_third_intersecting_street_283;
            public string out_fourth_intersecting_street_283;
            public string out_fifth_intersecting_street_283;
            public string out_xstr_cnt_283;
            public string out_distance_283;
            public string out_gap_flag_283;
            public string out_node_num_283;
            public string out_intersecting_street_284;
            public string out_second_intersecting_street_284;
            public string out_third_intersecting_street_284;
            public string out_fourth_intersecting_street_284;
            public string out_fifth_intersecting_street_284;
            public string out_xstr_cnt_284;
            public string out_distance_284;
            public string out_gap_flag_284;
            public string out_node_num_284;
            public string out_intersecting_street_285;
            public string out_second_intersecting_street_285;
            public string out_third_intersecting_street_285;
            public string out_fourth_intersecting_street_285;
            public string out_fifth_intersecting_street_285;
            public string out_xstr_cnt_285;
            public string out_distance_285;
            public string out_gap_flag_285;
            public string out_node_num_285;
            public string out_intersecting_street_286;
            public string out_second_intersecting_street_286;
            public string out_third_intersecting_street_286;
            public string out_fourth_intersecting_street_286;
            public string out_fifth_intersecting_street_286;
            public string out_xstr_cnt_286;
            public string out_distance_286;
            public string out_gap_flag_286;
            public string out_node_num_286;
            public string out_intersecting_street_287;
            public string out_second_intersecting_street_287;
            public string out_third_intersecting_street_287;
            public string out_fourth_intersecting_street_287;
            public string out_fifth_intersecting_street_287;
            public string out_xstr_cnt_287;
            public string out_distance_287;
            public string out_gap_flag_287;
            public string out_node_num_287;
            public string out_intersecting_street_288;
            public string out_second_intersecting_street_288;
            public string out_third_intersecting_street_288;
            public string out_fourth_intersecting_street_288;
            public string out_fifth_intersecting_street_288;
            public string out_xstr_cnt_288;
            public string out_distance_288;
            public string out_gap_flag_288;
            public string out_node_num_288;
            public string out_intersecting_street_289;
            public string out_second_intersecting_street_289;
            public string out_third_intersecting_street_289;
            public string out_fourth_intersecting_street_289;
            public string out_fifth_intersecting_street_289;
            public string out_xstr_cnt_289;
            public string out_distance_289;
            public string out_gap_flag_289;
            public string out_node_num_289;
            public string out_intersecting_street_290;
            public string out_second_intersecting_street_290;
            public string out_third_intersecting_street_290;
            public string out_fourth_intersecting_street_290;
            public string out_fifth_intersecting_street_290;
            public string out_xstr_cnt_290;
            public string out_distance_290;
            public string out_gap_flag_290;
            public string out_node_num_290;
            public string out_intersecting_street_291;
            public string out_second_intersecting_street_291;
            public string out_third_intersecting_street_291;
            public string out_fourth_intersecting_street_291;
            public string out_fifth_intersecting_street_291;
            public string out_xstr_cnt_291;
            public string out_distance_291;
            public string out_gap_flag_291;
            public string out_node_num_291;
            public string out_intersecting_street_292;
            public string out_second_intersecting_street_292;
            public string out_third_intersecting_street_292;
            public string out_fourth_intersecting_street_292;
            public string out_fifth_intersecting_street_292;
            public string out_xstr_cnt_292;
            public string out_distance_292;
            public string out_gap_flag_292;
            public string out_node_num_292;
            public string out_intersecting_street_293;
            public string out_second_intersecting_street_293;
            public string out_third_intersecting_street_293;
            public string out_fourth_intersecting_street_293;
            public string out_fifth_intersecting_street_293;
            public string out_xstr_cnt_293;
            public string out_distance_293;
            public string out_gap_flag_293;
            public string out_node_num_293;
            public string out_intersecting_street_294;
            public string out_second_intersecting_street_294;
            public string out_third_intersecting_street_294;
            public string out_fourth_intersecting_street_294;
            public string out_fifth_intersecting_street_294;
            public string out_xstr_cnt_294;
            public string out_distance_294;
            public string out_gap_flag_294;
            public string out_node_num_294;
            public string out_intersecting_street_295;
            public string out_second_intersecting_street_295;
            public string out_third_intersecting_street_295;
            public string out_fourth_intersecting_street_295;
            public string out_fifth_intersecting_street_295;
            public string out_xstr_cnt_295;
            public string out_distance_295;
            public string out_gap_flag_295;
            public string out_node_num_295;
            public string out_intersecting_street_296;
            public string out_second_intersecting_street_296;
            public string out_third_intersecting_street_296;
            public string out_fourth_intersecting_street_296;
            public string out_fifth_intersecting_street_296;
            public string out_xstr_cnt_296;
            public string out_distance_296;
            public string out_gap_flag_296;
            public string out_node_num_296;
            public string out_intersecting_street_297;
            public string out_second_intersecting_street_297;
            public string out_third_intersecting_street_297;
            public string out_fourth_intersecting_street_297;
            public string out_fifth_intersecting_street_297;
            public string out_xstr_cnt_297;
            public string out_distance_297;
            public string out_gap_flag_297;
            public string out_node_num_297;
            public string out_intersecting_street_298;
            public string out_second_intersecting_street_298;
            public string out_third_intersecting_street_298;
            public string out_fourth_intersecting_street_298;
            public string out_fifth_intersecting_street_298;
            public string out_xstr_cnt_298;
            public string out_distance_298;
            public string out_gap_flag_298;
            public string out_node_num_298;
            public string out_intersecting_street_299;
            public string out_second_intersecting_street_299;
            public string out_third_intersecting_street_299;
            public string out_fourth_intersecting_street_299;
            public string out_fifth_intersecting_street_299;
            public string out_xstr_cnt_299;
            public string out_distance_299;
            public string out_gap_flag_299;
            public string out_node_num_299;
            public string out_intersecting_street_300;
            public string out_second_intersecting_street_300;
            public string out_third_intersecting_street_300;
            public string out_fourth_intersecting_street_300;
            public string out_fifth_intersecting_street_300;
            public string out_xstr_cnt_300;
            public string out_distance_300;
            public string out_gap_flag_300;
            public string out_node_num_300;
            public string out_intersecting_street_301;
            public string out_second_intersecting_street_301;
            public string out_third_intersecting_street_301;
            public string out_fourth_intersecting_street_301;
            public string out_fifth_intersecting_street_301;
            public string out_xstr_cnt_301;
            public string out_distance_301;
            public string out_gap_flag_301;
            public string out_node_num_301;
            public string out_intersecting_street_302;
            public string out_second_intersecting_street_302;
            public string out_third_intersecting_street_302;
            public string out_fourth_intersecting_street_302;
            public string out_fifth_intersecting_street_302;
            public string out_xstr_cnt_302;
            public string out_distance_302;
            public string out_gap_flag_302;
            public string out_node_num_302;
            public string out_intersecting_street_303;
            public string out_second_intersecting_street_303;
            public string out_third_intersecting_street_303;
            public string out_fourth_intersecting_street_303;
            public string out_fifth_intersecting_street_303;
            public string out_xstr_cnt_303;
            public string out_distance_303;
            public string out_gap_flag_303;
            public string out_node_num_303;
            public string out_intersecting_street_304;
            public string out_second_intersecting_street_304;
            public string out_third_intersecting_street_304;
            public string out_fourth_intersecting_street_304;
            public string out_fifth_intersecting_street_304;
            public string out_xstr_cnt_304;
            public string out_distance_304;
            public string out_gap_flag_304;
            public string out_node_num_304;
            public string out_intersecting_street_305;
            public string out_second_intersecting_street_305;
            public string out_third_intersecting_street_305;
            public string out_fourth_intersecting_street_305;
            public string out_fifth_intersecting_street_305;
            public string out_xstr_cnt_305;
            public string out_distance_305;
            public string out_gap_flag_305;
            public string out_node_num_305;
            public string out_intersecting_street_306;
            public string out_second_intersecting_street_306;
            public string out_third_intersecting_street_306;
            public string out_fourth_intersecting_street_306;
            public string out_fifth_intersecting_street_306;
            public string out_xstr_cnt_306;
            public string out_distance_306;
            public string out_gap_flag_306;
            public string out_node_num_306;
            public string out_intersecting_street_307;
            public string out_second_intersecting_street_307;
            public string out_third_intersecting_street_307;
            public string out_fourth_intersecting_street_307;
            public string out_fifth_intersecting_street_307;
            public string out_xstr_cnt_307;
            public string out_distance_307;
            public string out_gap_flag_307;
            public string out_node_num_307;
            public string out_intersecting_street_308;
            public string out_second_intersecting_street_308;
            public string out_third_intersecting_street_308;
            public string out_fourth_intersecting_street_308;
            public string out_fifth_intersecting_street_308;
            public string out_xstr_cnt_308;
            public string out_distance_308;
            public string out_gap_flag_308;
            public string out_node_num_308;
            public string out_intersecting_street_309;
            public string out_second_intersecting_street_309;
            public string out_third_intersecting_street_309;
            public string out_fourth_intersecting_street_309;
            public string out_fifth_intersecting_street_309;
            public string out_xstr_cnt_309;
            public string out_distance_309;
            public string out_gap_flag_309;
            public string out_node_num_309;
            public string out_intersecting_street_310;
            public string out_second_intersecting_street_310;
            public string out_third_intersecting_street_310;
            public string out_fourth_intersecting_street_310;
            public string out_fifth_intersecting_street_310;
            public string out_xstr_cnt_310;
            public string out_distance_310;
            public string out_gap_flag_310;
            public string out_node_num_310;
            public string out_intersecting_street_311;
            public string out_second_intersecting_street_311;
            public string out_third_intersecting_street_311;
            public string out_fourth_intersecting_street_311;
            public string out_fifth_intersecting_street_311;
            public string out_xstr_cnt_311;
            public string out_distance_311;
            public string out_gap_flag_311;
            public string out_node_num_311;
            public string out_intersecting_street_312;
            public string out_second_intersecting_street_312;
            public string out_third_intersecting_street_312;
            public string out_fourth_intersecting_street_312;
            public string out_fifth_intersecting_street_312;
            public string out_xstr_cnt_312;
            public string out_distance_312;
            public string out_gap_flag_312;
            public string out_node_num_312;
            public string out_intersecting_street_313;
            public string out_second_intersecting_street_313;
            public string out_third_intersecting_street_313;
            public string out_fourth_intersecting_street_313;
            public string out_fifth_intersecting_street_313;
            public string out_xstr_cnt_313;
            public string out_distance_313;
            public string out_gap_flag_313;
            public string out_node_num_313;
            public string out_intersecting_street_314;
            public string out_second_intersecting_street_314;
            public string out_third_intersecting_street_314;
            public string out_fourth_intersecting_street_314;
            public string out_fifth_intersecting_street_314;
            public string out_xstr_cnt_314;
            public string out_distance_314;
            public string out_gap_flag_314;
            public string out_node_num_314;
            public string out_intersecting_street_315;
            public string out_second_intersecting_street_315;
            public string out_third_intersecting_street_315;
            public string out_fourth_intersecting_street_315;
            public string out_fifth_intersecting_street_315;
            public string out_xstr_cnt_315;
            public string out_distance_315;
            public string out_gap_flag_315;
            public string out_node_num_315;
            public string out_intersecting_street_316;
            public string out_second_intersecting_street_316;
            public string out_third_intersecting_street_316;
            public string out_fourth_intersecting_street_316;
            public string out_fifth_intersecting_street_316;
            public string out_xstr_cnt_316;
            public string out_distance_316;
            public string out_gap_flag_316;
            public string out_node_num_316;
            public string out_intersecting_street_317;
            public string out_second_intersecting_street_317;
            public string out_third_intersecting_street_317;
            public string out_fourth_intersecting_street_317;
            public string out_fifth_intersecting_street_317;
            public string out_xstr_cnt_317;
            public string out_distance_317;
            public string out_gap_flag_317;
            public string out_node_num_317;
            public string out_intersecting_street_318;
            public string out_second_intersecting_street_318;
            public string out_third_intersecting_street_318;
            public string out_fourth_intersecting_street_318;
            public string out_fifth_intersecting_street_318;
            public string out_xstr_cnt_318;
            public string out_distance_318;
            public string out_gap_flag_318;
            public string out_node_num_318;
            public string out_intersecting_street_319;
            public string out_second_intersecting_street_319;
            public string out_third_intersecting_street_319;
            public string out_fourth_intersecting_street_319;
            public string out_fifth_intersecting_street_319;
            public string out_xstr_cnt_319;
            public string out_distance_319;
            public string out_gap_flag_319;
            public string out_node_num_319;
            public string out_intersecting_street_320;
            public string out_second_intersecting_street_320;
            public string out_third_intersecting_street_320;
            public string out_fourth_intersecting_street_320;
            public string out_fifth_intersecting_street_320;
            public string out_xstr_cnt_320;
            public string out_distance_320;
            public string out_gap_flag_320;
            public string out_node_num_320;
            public string out_intersecting_street_321;
            public string out_second_intersecting_street_321;
            public string out_third_intersecting_street_321;
            public string out_fourth_intersecting_street_321;
            public string out_fifth_intersecting_street_321;
            public string out_xstr_cnt_321;
            public string out_distance_321;
            public string out_gap_flag_321;
            public string out_node_num_321;
            public string out_intersecting_street_322;
            public string out_second_intersecting_street_322;
            public string out_third_intersecting_street_322;
            public string out_fourth_intersecting_street_322;
            public string out_fifth_intersecting_street_322;
            public string out_xstr_cnt_322;
            public string out_distance_322;
            public string out_gap_flag_322;
            public string out_node_num_322;
            public string out_intersecting_street_323;
            public string out_second_intersecting_street_323;
            public string out_third_intersecting_street_323;
            public string out_fourth_intersecting_street_323;
            public string out_fifth_intersecting_street_323;
            public string out_xstr_cnt_323;
            public string out_distance_323;
            public string out_gap_flag_323;
            public string out_node_num_323;
            public string out_intersecting_street_324;
            public string out_second_intersecting_street_324;
            public string out_third_intersecting_street_324;
            public string out_fourth_intersecting_street_324;
            public string out_fifth_intersecting_street_324;
            public string out_xstr_cnt_324;
            public string out_distance_324;
            public string out_gap_flag_324;
            public string out_node_num_324;
            public string out_intersecting_street_325;
            public string out_second_intersecting_street_325;
            public string out_third_intersecting_street_325;
            public string out_fourth_intersecting_street_325;
            public string out_fifth_intersecting_street_325;
            public string out_xstr_cnt_325;
            public string out_distance_325;
            public string out_gap_flag_325;
            public string out_node_num_325;
            public string out_intersecting_street_326;
            public string out_second_intersecting_street_326;
            public string out_third_intersecting_street_326;
            public string out_fourth_intersecting_street_326;
            public string out_fifth_intersecting_street_326;
            public string out_xstr_cnt_326;
            public string out_distance_326;
            public string out_gap_flag_326;
            public string out_node_num_326;
            public string out_intersecting_street_327;
            public string out_second_intersecting_street_327;
            public string out_third_intersecting_street_327;
            public string out_fourth_intersecting_street_327;
            public string out_fifth_intersecting_street_327;
            public string out_xstr_cnt_327;
            public string out_distance_327;
            public string out_gap_flag_327;
            public string out_node_num_327;
            public string out_intersecting_street_328;
            public string out_second_intersecting_street_328;
            public string out_third_intersecting_street_328;
            public string out_fourth_intersecting_street_328;
            public string out_fifth_intersecting_street_328;
            public string out_xstr_cnt_328;
            public string out_distance_328;
            public string out_gap_flag_328;
            public string out_node_num_328;
            public string out_intersecting_street_329;
            public string out_second_intersecting_street_329;
            public string out_third_intersecting_street_329;
            public string out_fourth_intersecting_street_329;
            public string out_fifth_intersecting_street_329;
            public string out_xstr_cnt_329;
            public string out_distance_329;
            public string out_gap_flag_329;
            public string out_node_num_329;
            public string out_intersecting_street_330;
            public string out_second_intersecting_street_330;
            public string out_third_intersecting_street_330;
            public string out_fourth_intersecting_street_330;
            public string out_fifth_intersecting_street_330;
            public string out_xstr_cnt_330;
            public string out_distance_330;
            public string out_gap_flag_330;
            public string out_node_num_330;
            public string out_intersecting_street_331;
            public string out_second_intersecting_street_331;
            public string out_third_intersecting_street_331;
            public string out_fourth_intersecting_street_331;
            public string out_fifth_intersecting_street_331;
            public string out_xstr_cnt_331;
            public string out_distance_331;
            public string out_gap_flag_331;
            public string out_node_num_331;
            public string out_intersecting_street_332;
            public string out_second_intersecting_street_332;
            public string out_third_intersecting_street_332;
            public string out_fourth_intersecting_street_332;
            public string out_fifth_intersecting_street_332;
            public string out_xstr_cnt_332;
            public string out_distance_332;
            public string out_gap_flag_332;
            public string out_node_num_332;
            public string out_intersecting_street_333;
            public string out_second_intersecting_street_333;
            public string out_third_intersecting_street_333;
            public string out_fourth_intersecting_street_333;
            public string out_fifth_intersecting_street_333;
            public string out_xstr_cnt_333;
            public string out_distance_333;
            public string out_gap_flag_333;
            public string out_node_num_333;
            public string out_intersecting_street_334;
            public string out_second_intersecting_street_334;
            public string out_third_intersecting_street_334;
            public string out_fourth_intersecting_street_334;
            public string out_fifth_intersecting_street_334;
            public string out_xstr_cnt_334;
            public string out_distance_334;
            public string out_gap_flag_334;
            public string out_node_num_334;
            public string out_intersecting_street_335;
            public string out_second_intersecting_street_335;
            public string out_third_intersecting_street_335;
            public string out_fourth_intersecting_street_335;
            public string out_fifth_intersecting_street_335;
            public string out_xstr_cnt_335;
            public string out_distance_335;
            public string out_gap_flag_335;
            public string out_node_num_335;
            public string out_intersecting_street_336;
            public string out_second_intersecting_street_336;
            public string out_third_intersecting_street_336;
            public string out_fourth_intersecting_street_336;
            public string out_fifth_intersecting_street_336;
            public string out_xstr_cnt_336;
            public string out_distance_336;
            public string out_gap_flag_336;
            public string out_node_num_336;
            public string out_intersecting_street_337;
            public string out_second_intersecting_street_337;
            public string out_third_intersecting_street_337;
            public string out_fourth_intersecting_street_337;
            public string out_fifth_intersecting_street_337;
            public string out_xstr_cnt_337;
            public string out_distance_337;
            public string out_gap_flag_337;
            public string out_node_num_337;
            public string out_intersecting_street_338;
            public string out_second_intersecting_street_338;
            public string out_third_intersecting_street_338;
            public string out_fourth_intersecting_street_338;
            public string out_fifth_intersecting_street_338;
            public string out_xstr_cnt_338;
            public string out_distance_338;
            public string out_gap_flag_338;
            public string out_node_num_338;
            public string out_intersecting_street_339;
            public string out_second_intersecting_street_339;
            public string out_third_intersecting_street_339;
            public string out_fourth_intersecting_street_339;
            public string out_fifth_intersecting_street_339;
            public string out_xstr_cnt_339;
            public string out_distance_339;
            public string out_gap_flag_339;
            public string out_node_num_339;
            public string out_intersecting_street_340;
            public string out_second_intersecting_street_340;
            public string out_third_intersecting_street_340;
            public string out_fourth_intersecting_street_340;
            public string out_fifth_intersecting_street_340;
            public string out_xstr_cnt_340;
            public string out_distance_340;
            public string out_gap_flag_340;
            public string out_node_num_340;
            public string out_intersecting_street_341;
            public string out_second_intersecting_street_341;
            public string out_third_intersecting_street_341;
            public string out_fourth_intersecting_street_341;
            public string out_fifth_intersecting_street_341;
            public string out_xstr_cnt_341;
            public string out_distance_341;
            public string out_gap_flag_341;
            public string out_node_num_341;
            public string out_intersecting_street_342;
            public string out_second_intersecting_street_342;
            public string out_third_intersecting_street_342;
            public string out_fourth_intersecting_street_342;
            public string out_fifth_intersecting_street_342;
            public string out_xstr_cnt_342;
            public string out_distance_342;
            public string out_gap_flag_342;
            public string out_node_num_342;
            public string out_intersecting_street_343;
            public string out_second_intersecting_street_343;
            public string out_third_intersecting_street_343;
            public string out_fourth_intersecting_street_343;
            public string out_fifth_intersecting_street_343;
            public string out_xstr_cnt_343;
            public string out_distance_343;
            public string out_gap_flag_343;
            public string out_node_num_343;
            public string out_intersecting_street_344;
            public string out_second_intersecting_street_344;
            public string out_third_intersecting_street_344;
            public string out_fourth_intersecting_street_344;
            public string out_fifth_intersecting_street_344;
            public string out_xstr_cnt_344;
            public string out_distance_344;
            public string out_gap_flag_344;
            public string out_node_num_344;
            public string out_intersecting_street_345;
            public string out_second_intersecting_street_345;
            public string out_third_intersecting_street_345;
            public string out_fourth_intersecting_street_345;
            public string out_fifth_intersecting_street_345;
            public string out_xstr_cnt_345;
            public string out_distance_345;
            public string out_gap_flag_345;
            public string out_node_num_345;
            public string out_intersecting_street_346;
            public string out_second_intersecting_street_346;
            public string out_third_intersecting_street_346;
            public string out_fourth_intersecting_street_346;
            public string out_fifth_intersecting_street_346;
            public string out_xstr_cnt_346;
            public string out_distance_346;
            public string out_gap_flag_346;
            public string out_node_num_346;
            public string out_intersecting_street_347;
            public string out_second_intersecting_street_347;
            public string out_third_intersecting_street_347;
            public string out_fourth_intersecting_street_347;
            public string out_fifth_intersecting_street_347;
            public string out_xstr_cnt_347;
            public string out_distance_347;
            public string out_gap_flag_347;
            public string out_node_num_347;
            public string out_intersecting_street_348;
            public string out_second_intersecting_street_348;
            public string out_third_intersecting_street_348;
            public string out_fourth_intersecting_street_348;
            public string out_fifth_intersecting_street_348;
            public string out_xstr_cnt_348;
            public string out_distance_348;
            public string out_gap_flag_348;
            public string out_node_num_348;
            public string out_intersecting_street_349;
            public string out_second_intersecting_street_349;
            public string out_third_intersecting_street_349;
            public string out_fourth_intersecting_street_349;
            public string out_fifth_intersecting_street_349;
            public string out_xstr_cnt_349;
            public string out_distance_349;
            public string out_gap_flag_349;
            public string out_node_num_349;
            public string out_intersecting_street_350;
            public string out_second_intersecting_street_350;
            public string out_third_intersecting_street_350;
            public string out_fourth_intersecting_street_350;
            public string out_fifth_intersecting_street_350;
            public string out_xstr_cnt_350;
            public string out_distance_350;
            public string out_gap_flag_350;
            public string out_node_num_350;
        }

        string IGeoService.Get3SGeocode(string Borough, string OnStreet, string FirstCrossStreet, string SecondCrossStreet, string RealStreetFlag)
        {
            // work area 1 
            Wa1 mywa1 = new Wa1();
            Wa1 mywa1_stname = new Wa1();
            Wa2F3s mywa2f3s = new Wa2F3s();

            mywa1.in_b10sc1.boro = Borough;
            mywa1.in_stname1 = OnStreet;
            mywa1.in_stname2 = FirstCrossStreet;
            mywa1.in_stname3 = SecondCrossStreet;
            mywa1.in_real_street_only = RealStreetFlag;


            mywa1.in_func_code = "3S";
            mywa1.in_platform_ind = "C";
            mywa1.in_real_street_only = "R";

            // this call gets 1B info
            mygeo.GeoCall(ref mywa1, ref mywa2f3s);
            jsonoutput3S jsonoutputStr3S = new jsonoutput3S();
            jsonoutputStr3S.in_func_code = mywa1.in_func_code;
            jsonoutputStr3S.in_boro = mywa1.in_b10sc1.boro;
            jsonoutputStr3S.in_stname1 = mywa1.in_stname1;
            jsonoutputStr3S.in_stname2 = mywa1.in_stname2;
            jsonoutputStr3S.in_stname3 = mywa1.in_stname3;
            jsonoutputStr3S.out_grc = mywa1.out_grc;
            jsonoutputStr3S.out_grc2 = mywa1.out_grc2;
            jsonoutputStr3S.out_error_message = mywa1.out_error_message;
            jsonoutputStr3S.out_number_of_intersections = mywa2f3s.num_of_intersections;
            Wa1 mywa1_dl1 = new Wa1();
            mywa1_dl1.in_func_code = "DL";
            mywa1_dl1.in_platform_ind = "C";
            int numOfInt = Convert.ToInt32(mywa2f3s.num_of_intersections);

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[0].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_1 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[0].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_1 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[0].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_1 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[0].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_1 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[0].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_1 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_1 = mywa2f3s.xstr_list[0].xstr_cnt;

            jsonoutputStr3S.out_distance_1 = mywa2f3s.xstr_list[0].distance;

            jsonoutputStr3S.out_gap_flag_1 = mywa2f3s.xstr_list[0].gap_flag;

            jsonoutputStr3S.out_node_num_1 = mywa2f3s.xstr_list[0].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[1].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_2 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[1].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_2 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[1].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_2 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[1].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_2 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[1].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_2 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_2 = mywa2f3s.xstr_list[1].xstr_cnt;

            jsonoutputStr3S.out_distance_2 = mywa2f3s.xstr_list[1].distance;

            jsonoutputStr3S.out_gap_flag_2 = mywa2f3s.xstr_list[1].gap_flag;

            jsonoutputStr3S.out_node_num_2 = mywa2f3s.xstr_list[1].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[2].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_3 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[2].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_3 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[2].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_3 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[2].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_3 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[2].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_3 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_3 = mywa2f3s.xstr_list[2].xstr_cnt;

            jsonoutputStr3S.out_distance_3 = mywa2f3s.xstr_list[2].distance;

            jsonoutputStr3S.out_gap_flag_3 = mywa2f3s.xstr_list[2].gap_flag;

            jsonoutputStr3S.out_node_num_3 = mywa2f3s.xstr_list[2].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[3].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_4 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[3].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_4 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[3].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_4 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[3].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_4 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[3].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_4 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_4 = mywa2f3s.xstr_list[3].xstr_cnt;

            jsonoutputStr3S.out_distance_4 = mywa2f3s.xstr_list[3].distance;

            jsonoutputStr3S.out_gap_flag_4 = mywa2f3s.xstr_list[3].gap_flag;

            jsonoutputStr3S.out_node_num_4 = mywa2f3s.xstr_list[3].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[4].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_5 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[4].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_5 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[4].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_5 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[4].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_5 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[4].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_5 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_5 = mywa2f3s.xstr_list[4].xstr_cnt;

            jsonoutputStr3S.out_distance_5 = mywa2f3s.xstr_list[4].distance;

            jsonoutputStr3S.out_gap_flag_5 = mywa2f3s.xstr_list[4].gap_flag;

            jsonoutputStr3S.out_node_num_5 = mywa2f3s.xstr_list[4].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[5].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_6 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[5].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_6 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[5].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_6 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[5].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_6 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[5].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_6 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_6 = mywa2f3s.xstr_list[5].xstr_cnt;

            jsonoutputStr3S.out_distance_6 = mywa2f3s.xstr_list[5].distance;

            jsonoutputStr3S.out_gap_flag_6 = mywa2f3s.xstr_list[5].gap_flag;

            jsonoutputStr3S.out_node_num_6 = mywa2f3s.xstr_list[5].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[6].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_7 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[6].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_7 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[6].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_7 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[6].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_7 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[6].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_7 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_7 = mywa2f3s.xstr_list[6].xstr_cnt;

            jsonoutputStr3S.out_distance_7 = mywa2f3s.xstr_list[6].distance;

            jsonoutputStr3S.out_gap_flag_7 = mywa2f3s.xstr_list[6].gap_flag;

            jsonoutputStr3S.out_node_num_7 = mywa2f3s.xstr_list[6].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[7].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_8 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[7].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_8 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[7].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_8 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[7].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_8 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[7].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_8 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_8 = mywa2f3s.xstr_list[7].xstr_cnt;

            jsonoutputStr3S.out_distance_8 = mywa2f3s.xstr_list[7].distance;

            jsonoutputStr3S.out_gap_flag_8 = mywa2f3s.xstr_list[7].gap_flag;

            jsonoutputStr3S.out_node_num_8 = mywa2f3s.xstr_list[7].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[8].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_9 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[8].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_9 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[8].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_9 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[8].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_9 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[8].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_9 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_9 = mywa2f3s.xstr_list[8].xstr_cnt;

            jsonoutputStr3S.out_distance_9 = mywa2f3s.xstr_list[8].distance;

            jsonoutputStr3S.out_gap_flag_9 = mywa2f3s.xstr_list[8].gap_flag;

            jsonoutputStr3S.out_node_num_9 = mywa2f3s.xstr_list[8].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[9].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_10 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[9].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_10 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[9].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_10 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[9].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_10 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[9].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_10 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_10 = mywa2f3s.xstr_list[9].xstr_cnt;

            jsonoutputStr3S.out_distance_10 = mywa2f3s.xstr_list[9].distance;

            jsonoutputStr3S.out_gap_flag_10 = mywa2f3s.xstr_list[9].gap_flag;

            jsonoutputStr3S.out_node_num_10 = mywa2f3s.xstr_list[9].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[10].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_11 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[10].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_11 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[10].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_11 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[10].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_11 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[10].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_11 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_11 = mywa2f3s.xstr_list[10].xstr_cnt;

            jsonoutputStr3S.out_distance_11 = mywa2f3s.xstr_list[10].distance;

            jsonoutputStr3S.out_gap_flag_11 = mywa2f3s.xstr_list[10].gap_flag;

            jsonoutputStr3S.out_node_num_11 = mywa2f3s.xstr_list[10].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[11].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_12 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[11].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_12 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[11].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_12 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[11].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_12 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[11].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_12 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_12 = mywa2f3s.xstr_list[11].xstr_cnt;

            jsonoutputStr3S.out_distance_12 = mywa2f3s.xstr_list[11].distance;

            jsonoutputStr3S.out_gap_flag_12 = mywa2f3s.xstr_list[11].gap_flag;

            jsonoutputStr3S.out_node_num_12 = mywa2f3s.xstr_list[11].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[12].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_13 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[12].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_13 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[12].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_13 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[12].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_13 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[12].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_13 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_13 = mywa2f3s.xstr_list[12].xstr_cnt;

            jsonoutputStr3S.out_distance_13 = mywa2f3s.xstr_list[12].distance;

            jsonoutputStr3S.out_gap_flag_13 = mywa2f3s.xstr_list[12].gap_flag;

            jsonoutputStr3S.out_node_num_13 = mywa2f3s.xstr_list[12].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[13].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_14 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[13].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_14 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[13].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_14 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[13].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_14 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[13].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_14 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_14 = mywa2f3s.xstr_list[13].xstr_cnt;

            jsonoutputStr3S.out_distance_14 = mywa2f3s.xstr_list[13].distance;

            jsonoutputStr3S.out_gap_flag_14 = mywa2f3s.xstr_list[13].gap_flag;

            jsonoutputStr3S.out_node_num_14 = mywa2f3s.xstr_list[13].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[14].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_15 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[14].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_15 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[14].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_15 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[14].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_15 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[14].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_15 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_15 = mywa2f3s.xstr_list[14].xstr_cnt;

            jsonoutputStr3S.out_distance_15 = mywa2f3s.xstr_list[14].distance;

            jsonoutputStr3S.out_gap_flag_15 = mywa2f3s.xstr_list[14].gap_flag;

            jsonoutputStr3S.out_node_num_15 = mywa2f3s.xstr_list[14].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[15].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_16 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[15].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_16 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[15].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_16 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[15].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_16 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[15].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_16 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_16 = mywa2f3s.xstr_list[15].xstr_cnt;

            jsonoutputStr3S.out_distance_16 = mywa2f3s.xstr_list[15].distance;

            jsonoutputStr3S.out_gap_flag_16 = mywa2f3s.xstr_list[15].gap_flag;

            jsonoutputStr3S.out_node_num_16 = mywa2f3s.xstr_list[15].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[16].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_17 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[16].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_17 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[16].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_17 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[16].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_17 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[16].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_17 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_17 = mywa2f3s.xstr_list[16].xstr_cnt;

            jsonoutputStr3S.out_distance_17 = mywa2f3s.xstr_list[16].distance;

            jsonoutputStr3S.out_gap_flag_17 = mywa2f3s.xstr_list[16].gap_flag;

            jsonoutputStr3S.out_node_num_17 = mywa2f3s.xstr_list[16].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[17].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_18 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[17].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_18 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[17].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_18 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[17].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_18 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[17].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_18 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_18 = mywa2f3s.xstr_list[17].xstr_cnt;

            jsonoutputStr3S.out_distance_18 = mywa2f3s.xstr_list[17].distance;

            jsonoutputStr3S.out_gap_flag_18 = mywa2f3s.xstr_list[17].gap_flag;

            jsonoutputStr3S.out_node_num_18 = mywa2f3s.xstr_list[17].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[18].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_19 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[18].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_19 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[18].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_19 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[18].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_19 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[18].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_19 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_19 = mywa2f3s.xstr_list[18].xstr_cnt;

            jsonoutputStr3S.out_distance_19 = mywa2f3s.xstr_list[18].distance;

            jsonoutputStr3S.out_gap_flag_19 = mywa2f3s.xstr_list[18].gap_flag;

            jsonoutputStr3S.out_node_num_19 = mywa2f3s.xstr_list[18].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[19].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_20 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[19].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_20 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[19].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_20 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[19].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_20 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[19].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_20 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_20 = mywa2f3s.xstr_list[19].xstr_cnt;

            jsonoutputStr3S.out_distance_20 = mywa2f3s.xstr_list[19].distance;

            jsonoutputStr3S.out_gap_flag_20 = mywa2f3s.xstr_list[19].gap_flag;

            jsonoutputStr3S.out_node_num_20 = mywa2f3s.xstr_list[19].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[20].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_21 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[20].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_21 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[20].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_21 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[20].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_21 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[20].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_21 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_21 = mywa2f3s.xstr_list[20].xstr_cnt;

            jsonoutputStr3S.out_distance_21 = mywa2f3s.xstr_list[20].distance;

            jsonoutputStr3S.out_gap_flag_21 = mywa2f3s.xstr_list[20].gap_flag;

            jsonoutputStr3S.out_node_num_21 = mywa2f3s.xstr_list[20].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[21].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_22 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[21].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_22 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[21].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_22 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[21].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_22 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[21].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_22 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_22 = mywa2f3s.xstr_list[21].xstr_cnt;

            jsonoutputStr3S.out_distance_22 = mywa2f3s.xstr_list[21].distance;

            jsonoutputStr3S.out_gap_flag_22 = mywa2f3s.xstr_list[21].gap_flag;

            jsonoutputStr3S.out_node_num_22 = mywa2f3s.xstr_list[21].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[22].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_23 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[22].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_23 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[22].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_23 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[22].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_23 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[22].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_23 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_23 = mywa2f3s.xstr_list[22].xstr_cnt;

            jsonoutputStr3S.out_distance_23 = mywa2f3s.xstr_list[22].distance;

            jsonoutputStr3S.out_gap_flag_23 = mywa2f3s.xstr_list[22].gap_flag;

            jsonoutputStr3S.out_node_num_23 = mywa2f3s.xstr_list[22].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[23].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_24 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[23].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_24 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[23].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_24 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[23].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_24 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[23].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_24 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_24 = mywa2f3s.xstr_list[23].xstr_cnt;

            jsonoutputStr3S.out_distance_24 = mywa2f3s.xstr_list[23].distance;

            jsonoutputStr3S.out_gap_flag_24 = mywa2f3s.xstr_list[23].gap_flag;

            jsonoutputStr3S.out_node_num_24 = mywa2f3s.xstr_list[23].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[24].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_25 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[24].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_25 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[24].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_25 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[24].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_25 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[24].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_25 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_25 = mywa2f3s.xstr_list[24].xstr_cnt;

            jsonoutputStr3S.out_distance_25 = mywa2f3s.xstr_list[24].distance;

            jsonoutputStr3S.out_gap_flag_25 = mywa2f3s.xstr_list[24].gap_flag;

            jsonoutputStr3S.out_node_num_25 = mywa2f3s.xstr_list[24].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[25].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_26 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[25].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_26 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[25].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_26 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[25].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_26 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[25].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_26 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_26 = mywa2f3s.xstr_list[25].xstr_cnt;

            jsonoutputStr3S.out_distance_26 = mywa2f3s.xstr_list[25].distance;

            jsonoutputStr3S.out_gap_flag_26 = mywa2f3s.xstr_list[25].gap_flag;

            jsonoutputStr3S.out_node_num_26 = mywa2f3s.xstr_list[25].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[26].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_27 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[26].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_27 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[26].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_27 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[26].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_27 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[26].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_27 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_27 = mywa2f3s.xstr_list[26].xstr_cnt;

            jsonoutputStr3S.out_distance_27 = mywa2f3s.xstr_list[26].distance;

            jsonoutputStr3S.out_gap_flag_27 = mywa2f3s.xstr_list[26].gap_flag;

            jsonoutputStr3S.out_node_num_27 = mywa2f3s.xstr_list[26].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[27].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_28 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[27].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_28 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[27].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_28 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[27].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_28 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[27].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_28 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_28 = mywa2f3s.xstr_list[27].xstr_cnt;

            jsonoutputStr3S.out_distance_28 = mywa2f3s.xstr_list[27].distance;

            jsonoutputStr3S.out_gap_flag_28 = mywa2f3s.xstr_list[27].gap_flag;

            jsonoutputStr3S.out_node_num_28 = mywa2f3s.xstr_list[27].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[28].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_29 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[28].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_29 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[28].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_29 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[28].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_29 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[28].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_29 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_29 = mywa2f3s.xstr_list[28].xstr_cnt;

            jsonoutputStr3S.out_distance_29 = mywa2f3s.xstr_list[28].distance;

            jsonoutputStr3S.out_gap_flag_29 = mywa2f3s.xstr_list[28].gap_flag;

            jsonoutputStr3S.out_node_num_29 = mywa2f3s.xstr_list[28].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[29].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_30 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[29].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_30 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[29].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_30 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[29].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_30 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[29].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_30 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_30 = mywa2f3s.xstr_list[29].xstr_cnt;

            jsonoutputStr3S.out_distance_30 = mywa2f3s.xstr_list[29].distance;

            jsonoutputStr3S.out_gap_flag_30 = mywa2f3s.xstr_list[29].gap_flag;

            jsonoutputStr3S.out_node_num_30 = mywa2f3s.xstr_list[29].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[30].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_31 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[30].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_31 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[30].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_31 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[30].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_31 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[30].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_31 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_31 = mywa2f3s.xstr_list[30].xstr_cnt;

            jsonoutputStr3S.out_distance_31 = mywa2f3s.xstr_list[30].distance;

            jsonoutputStr3S.out_gap_flag_31 = mywa2f3s.xstr_list[30].gap_flag;

            jsonoutputStr3S.out_node_num_31 = mywa2f3s.xstr_list[30].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[31].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_32 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[31].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_32 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[31].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_32 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[31].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_32 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[31].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_32 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_32 = mywa2f3s.xstr_list[31].xstr_cnt;

            jsonoutputStr3S.out_distance_32 = mywa2f3s.xstr_list[31].distance;

            jsonoutputStr3S.out_gap_flag_32 = mywa2f3s.xstr_list[31].gap_flag;

            jsonoutputStr3S.out_node_num_32 = mywa2f3s.xstr_list[31].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[32].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_33 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[32].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_33 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[32].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_33 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[32].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_33 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[32].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_33 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_33 = mywa2f3s.xstr_list[32].xstr_cnt;

            jsonoutputStr3S.out_distance_33 = mywa2f3s.xstr_list[32].distance;

            jsonoutputStr3S.out_gap_flag_33 = mywa2f3s.xstr_list[32].gap_flag;

            jsonoutputStr3S.out_node_num_33 = mywa2f3s.xstr_list[32].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[33].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_34 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[33].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_34 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[33].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_34 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[33].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_34 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[33].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_34 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_34 = mywa2f3s.xstr_list[33].xstr_cnt;

            jsonoutputStr3S.out_distance_34 = mywa2f3s.xstr_list[33].distance;

            jsonoutputStr3S.out_gap_flag_34 = mywa2f3s.xstr_list[33].gap_flag;

            jsonoutputStr3S.out_node_num_34 = mywa2f3s.xstr_list[33].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[34].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_35 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[34].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_35 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[34].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_35 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[34].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_35 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[34].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_35 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_35 = mywa2f3s.xstr_list[34].xstr_cnt;

            jsonoutputStr3S.out_distance_35 = mywa2f3s.xstr_list[34].distance;

            jsonoutputStr3S.out_gap_flag_35 = mywa2f3s.xstr_list[34].gap_flag;

            jsonoutputStr3S.out_node_num_35 = mywa2f3s.xstr_list[34].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[35].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_36 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[35].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_36 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[35].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_36 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[35].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_36 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[35].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_36 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_36 = mywa2f3s.xstr_list[35].xstr_cnt;

            jsonoutputStr3S.out_distance_36 = mywa2f3s.xstr_list[35].distance;

            jsonoutputStr3S.out_gap_flag_36 = mywa2f3s.xstr_list[35].gap_flag;

            jsonoutputStr3S.out_node_num_36 = mywa2f3s.xstr_list[35].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[36].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_37 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[36].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_37 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[36].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_37 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[36].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_37 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[36].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_37 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_37 = mywa2f3s.xstr_list[36].xstr_cnt;

            jsonoutputStr3S.out_distance_37 = mywa2f3s.xstr_list[36].distance;

            jsonoutputStr3S.out_gap_flag_37 = mywa2f3s.xstr_list[36].gap_flag;

            jsonoutputStr3S.out_node_num_37 = mywa2f3s.xstr_list[36].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[37].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_38 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[37].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_38 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[37].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_38 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[37].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_38 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[37].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_38 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_38 = mywa2f3s.xstr_list[37].xstr_cnt;

            jsonoutputStr3S.out_distance_38 = mywa2f3s.xstr_list[37].distance;

            jsonoutputStr3S.out_gap_flag_38 = mywa2f3s.xstr_list[37].gap_flag;

            jsonoutputStr3S.out_node_num_38 = mywa2f3s.xstr_list[37].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[38].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_39 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[38].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_39 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[38].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_39 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[38].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_39 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[38].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_39 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_39 = mywa2f3s.xstr_list[38].xstr_cnt;

            jsonoutputStr3S.out_distance_39 = mywa2f3s.xstr_list[38].distance;

            jsonoutputStr3S.out_gap_flag_39 = mywa2f3s.xstr_list[38].gap_flag;

            jsonoutputStr3S.out_node_num_39 = mywa2f3s.xstr_list[38].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[39].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_40 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[39].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_40 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[39].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_40 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[39].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_40 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[39].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_40 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_40 = mywa2f3s.xstr_list[39].xstr_cnt;

            jsonoutputStr3S.out_distance_40 = mywa2f3s.xstr_list[39].distance;

            jsonoutputStr3S.out_gap_flag_40 = mywa2f3s.xstr_list[39].gap_flag;

            jsonoutputStr3S.out_node_num_40 = mywa2f3s.xstr_list[39].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[40].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_41 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[40].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_41 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[40].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_41 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[40].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_41 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[40].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_41 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_41 = mywa2f3s.xstr_list[40].xstr_cnt;

            jsonoutputStr3S.out_distance_41 = mywa2f3s.xstr_list[40].distance;

            jsonoutputStr3S.out_gap_flag_41 = mywa2f3s.xstr_list[40].gap_flag;

            jsonoutputStr3S.out_node_num_41 = mywa2f3s.xstr_list[40].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[41].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_42 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[41].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_42 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[41].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_42 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[41].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_42 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[41].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_42 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_42 = mywa2f3s.xstr_list[41].xstr_cnt;

            jsonoutputStr3S.out_distance_42 = mywa2f3s.xstr_list[41].distance;

            jsonoutputStr3S.out_gap_flag_42 = mywa2f3s.xstr_list[41].gap_flag;

            jsonoutputStr3S.out_node_num_42 = mywa2f3s.xstr_list[41].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[42].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_43 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[42].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_43 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[42].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_43 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[42].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_43 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[42].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_43 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_43 = mywa2f3s.xstr_list[42].xstr_cnt;

            jsonoutputStr3S.out_distance_43 = mywa2f3s.xstr_list[42].distance;

            jsonoutputStr3S.out_gap_flag_43 = mywa2f3s.xstr_list[42].gap_flag;

            jsonoutputStr3S.out_node_num_43 = mywa2f3s.xstr_list[42].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[43].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_44 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[43].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_44 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[43].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_44 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[43].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_44 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[43].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_44 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_44 = mywa2f3s.xstr_list[43].xstr_cnt;

            jsonoutputStr3S.out_distance_44 = mywa2f3s.xstr_list[43].distance;

            jsonoutputStr3S.out_gap_flag_44 = mywa2f3s.xstr_list[43].gap_flag;

            jsonoutputStr3S.out_node_num_44 = mywa2f3s.xstr_list[43].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[44].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_45 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[44].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_45 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[44].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_45 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[44].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_45 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[44].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_45 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_45 = mywa2f3s.xstr_list[44].xstr_cnt;

            jsonoutputStr3S.out_distance_45 = mywa2f3s.xstr_list[44].distance;

            jsonoutputStr3S.out_gap_flag_45 = mywa2f3s.xstr_list[44].gap_flag;

            jsonoutputStr3S.out_node_num_45 = mywa2f3s.xstr_list[44].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[45].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_46 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[45].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_46 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[45].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_46 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[45].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_46 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[45].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_46 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_46 = mywa2f3s.xstr_list[45].xstr_cnt;

            jsonoutputStr3S.out_distance_46 = mywa2f3s.xstr_list[45].distance;

            jsonoutputStr3S.out_gap_flag_46 = mywa2f3s.xstr_list[45].gap_flag;

            jsonoutputStr3S.out_node_num_46 = mywa2f3s.xstr_list[45].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[46].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_47 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[46].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_47 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[46].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_47 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[46].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_47 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[46].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_47 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_47 = mywa2f3s.xstr_list[46].xstr_cnt;

            jsonoutputStr3S.out_distance_47 = mywa2f3s.xstr_list[46].distance;

            jsonoutputStr3S.out_gap_flag_47 = mywa2f3s.xstr_list[46].gap_flag;

            jsonoutputStr3S.out_node_num_47 = mywa2f3s.xstr_list[46].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[47].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_48 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[47].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_48 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[47].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_48 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[47].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_48 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[47].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_48 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_48 = mywa2f3s.xstr_list[47].xstr_cnt;

            jsonoutputStr3S.out_distance_48 = mywa2f3s.xstr_list[47].distance;

            jsonoutputStr3S.out_gap_flag_48 = mywa2f3s.xstr_list[47].gap_flag;

            jsonoutputStr3S.out_node_num_48 = mywa2f3s.xstr_list[47].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[48].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_49 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[48].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_49 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[48].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_49 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[48].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_49 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[48].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_49 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_49 = mywa2f3s.xstr_list[48].xstr_cnt;

            jsonoutputStr3S.out_distance_49 = mywa2f3s.xstr_list[48].distance;

            jsonoutputStr3S.out_gap_flag_49 = mywa2f3s.xstr_list[48].gap_flag;

            jsonoutputStr3S.out_node_num_49 = mywa2f3s.xstr_list[48].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[49].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_50 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[49].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_50 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[49].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_50 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[49].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_50 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[49].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_50 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_50 = mywa2f3s.xstr_list[49].xstr_cnt;

            jsonoutputStr3S.out_distance_50 = mywa2f3s.xstr_list[49].distance;

            jsonoutputStr3S.out_gap_flag_50 = mywa2f3s.xstr_list[49].gap_flag;

            jsonoutputStr3S.out_node_num_50 = mywa2f3s.xstr_list[49].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[50].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_51 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[50].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_51 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[50].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_51 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[50].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_51 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[50].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_51 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_51 = mywa2f3s.xstr_list[50].xstr_cnt;

            jsonoutputStr3S.out_distance_51 = mywa2f3s.xstr_list[50].distance;

            jsonoutputStr3S.out_gap_flag_51 = mywa2f3s.xstr_list[50].gap_flag;

            jsonoutputStr3S.out_node_num_51 = mywa2f3s.xstr_list[50].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[51].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_52 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[51].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_52 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[51].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_52 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[51].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_52 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[51].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_52 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_52 = mywa2f3s.xstr_list[51].xstr_cnt;

            jsonoutputStr3S.out_distance_52 = mywa2f3s.xstr_list[51].distance;

            jsonoutputStr3S.out_gap_flag_52 = mywa2f3s.xstr_list[51].gap_flag;

            jsonoutputStr3S.out_node_num_52 = mywa2f3s.xstr_list[51].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[52].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_53 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[52].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_53 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[52].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_53 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[52].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_53 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[52].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_53 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_53 = mywa2f3s.xstr_list[52].xstr_cnt;

            jsonoutputStr3S.out_distance_53 = mywa2f3s.xstr_list[52].distance;

            jsonoutputStr3S.out_gap_flag_53 = mywa2f3s.xstr_list[52].gap_flag;

            jsonoutputStr3S.out_node_num_53 = mywa2f3s.xstr_list[52].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[53].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_54 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[53].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_54 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[53].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_54 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[53].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_54 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[53].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_54 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_54 = mywa2f3s.xstr_list[53].xstr_cnt;

            jsonoutputStr3S.out_distance_54 = mywa2f3s.xstr_list[53].distance;

            jsonoutputStr3S.out_gap_flag_54 = mywa2f3s.xstr_list[53].gap_flag;

            jsonoutputStr3S.out_node_num_54 = mywa2f3s.xstr_list[53].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[54].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_55 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[54].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_55 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[54].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_55 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[54].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_55 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[54].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_55 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_55 = mywa2f3s.xstr_list[54].xstr_cnt;

            jsonoutputStr3S.out_distance_55 = mywa2f3s.xstr_list[54].distance;

            jsonoutputStr3S.out_gap_flag_55 = mywa2f3s.xstr_list[54].gap_flag;

            jsonoutputStr3S.out_node_num_55 = mywa2f3s.xstr_list[54].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[55].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_56 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[55].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_56 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[55].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_56 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[55].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_56 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[55].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_56 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_56 = mywa2f3s.xstr_list[55].xstr_cnt;

            jsonoutputStr3S.out_distance_56 = mywa2f3s.xstr_list[55].distance;

            jsonoutputStr3S.out_gap_flag_56 = mywa2f3s.xstr_list[55].gap_flag;

            jsonoutputStr3S.out_node_num_56 = mywa2f3s.xstr_list[55].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[56].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_57 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[56].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_57 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[56].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_57 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[56].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_57 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[56].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_57 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_57 = mywa2f3s.xstr_list[56].xstr_cnt;

            jsonoutputStr3S.out_distance_57 = mywa2f3s.xstr_list[56].distance;

            jsonoutputStr3S.out_gap_flag_57 = mywa2f3s.xstr_list[56].gap_flag;

            jsonoutputStr3S.out_node_num_57 = mywa2f3s.xstr_list[56].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[57].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_58 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[57].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_58 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[57].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_58 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[57].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_58 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[57].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_58 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_58 = mywa2f3s.xstr_list[57].xstr_cnt;

            jsonoutputStr3S.out_distance_58 = mywa2f3s.xstr_list[57].distance;

            jsonoutputStr3S.out_gap_flag_58 = mywa2f3s.xstr_list[57].gap_flag;

            jsonoutputStr3S.out_node_num_58 = mywa2f3s.xstr_list[57].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[58].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_59 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[58].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_59 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[58].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_59 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[58].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_59 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[58].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_59 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_59 = mywa2f3s.xstr_list[58].xstr_cnt;

            jsonoutputStr3S.out_distance_59 = mywa2f3s.xstr_list[58].distance;

            jsonoutputStr3S.out_gap_flag_59 = mywa2f3s.xstr_list[58].gap_flag;

            jsonoutputStr3S.out_node_num_59 = mywa2f3s.xstr_list[58].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[59].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_60 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[59].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_60 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[59].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_60 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[59].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_60 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[59].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_60 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_60 = mywa2f3s.xstr_list[59].xstr_cnt;

            jsonoutputStr3S.out_distance_60 = mywa2f3s.xstr_list[59].distance;

            jsonoutputStr3S.out_gap_flag_60 = mywa2f3s.xstr_list[59].gap_flag;

            jsonoutputStr3S.out_node_num_60 = mywa2f3s.xstr_list[59].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[60].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_61 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[60].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_61 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[60].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_61 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[60].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_61 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[60].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_61 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_61 = mywa2f3s.xstr_list[60].xstr_cnt;

            jsonoutputStr3S.out_distance_61 = mywa2f3s.xstr_list[60].distance;

            jsonoutputStr3S.out_gap_flag_61 = mywa2f3s.xstr_list[60].gap_flag;

            jsonoutputStr3S.out_node_num_61 = mywa2f3s.xstr_list[60].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[61].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_62 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[61].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_62 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[61].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_62 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[61].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_62 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[61].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_62 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_62 = mywa2f3s.xstr_list[61].xstr_cnt;

            jsonoutputStr3S.out_distance_62 = mywa2f3s.xstr_list[61].distance;

            jsonoutputStr3S.out_gap_flag_62 = mywa2f3s.xstr_list[61].gap_flag;

            jsonoutputStr3S.out_node_num_62 = mywa2f3s.xstr_list[61].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[62].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_63 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[62].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_63 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[62].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_63 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[62].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_63 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[62].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_63 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_63 = mywa2f3s.xstr_list[62].xstr_cnt;

            jsonoutputStr3S.out_distance_63 = mywa2f3s.xstr_list[62].distance;

            jsonoutputStr3S.out_gap_flag_63 = mywa2f3s.xstr_list[62].gap_flag;

            jsonoutputStr3S.out_node_num_63 = mywa2f3s.xstr_list[62].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[63].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_64 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[63].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_64 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[63].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_64 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[63].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_64 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[63].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_64 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_64 = mywa2f3s.xstr_list[63].xstr_cnt;

            jsonoutputStr3S.out_distance_64 = mywa2f3s.xstr_list[63].distance;

            jsonoutputStr3S.out_gap_flag_64 = mywa2f3s.xstr_list[63].gap_flag;

            jsonoutputStr3S.out_node_num_64 = mywa2f3s.xstr_list[63].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[64].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_65 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[64].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_65 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[64].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_65 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[64].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_65 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[64].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_65 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_65 = mywa2f3s.xstr_list[64].xstr_cnt;

            jsonoutputStr3S.out_distance_65 = mywa2f3s.xstr_list[64].distance;

            jsonoutputStr3S.out_gap_flag_65 = mywa2f3s.xstr_list[64].gap_flag;

            jsonoutputStr3S.out_node_num_65 = mywa2f3s.xstr_list[64].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[65].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_66 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[65].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_66 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[65].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_66 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[65].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_66 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[65].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_66 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_66 = mywa2f3s.xstr_list[65].xstr_cnt;

            jsonoutputStr3S.out_distance_66 = mywa2f3s.xstr_list[65].distance;

            jsonoutputStr3S.out_gap_flag_66 = mywa2f3s.xstr_list[65].gap_flag;

            jsonoutputStr3S.out_node_num_66 = mywa2f3s.xstr_list[65].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[66].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_67 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[66].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_67 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[66].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_67 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[66].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_67 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[66].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_67 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_67 = mywa2f3s.xstr_list[66].xstr_cnt;

            jsonoutputStr3S.out_distance_67 = mywa2f3s.xstr_list[66].distance;

            jsonoutputStr3S.out_gap_flag_67 = mywa2f3s.xstr_list[66].gap_flag;

            jsonoutputStr3S.out_node_num_67 = mywa2f3s.xstr_list[66].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[67].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_68 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[67].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_68 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[67].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_68 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[67].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_68 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[67].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_68 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_68 = mywa2f3s.xstr_list[67].xstr_cnt;

            jsonoutputStr3S.out_distance_68 = mywa2f3s.xstr_list[67].distance;

            jsonoutputStr3S.out_gap_flag_68 = mywa2f3s.xstr_list[67].gap_flag;

            jsonoutputStr3S.out_node_num_68 = mywa2f3s.xstr_list[67].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[68].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_69 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[68].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_69 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[68].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_69 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[68].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_69 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[68].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_69 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_69 = mywa2f3s.xstr_list[68].xstr_cnt;

            jsonoutputStr3S.out_distance_69 = mywa2f3s.xstr_list[68].distance;

            jsonoutputStr3S.out_gap_flag_69 = mywa2f3s.xstr_list[68].gap_flag;

            jsonoutputStr3S.out_node_num_69 = mywa2f3s.xstr_list[68].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[69].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_70 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[69].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_70 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[69].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_70 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[69].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_70 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[69].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_70 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_70 = mywa2f3s.xstr_list[69].xstr_cnt;

            jsonoutputStr3S.out_distance_70 = mywa2f3s.xstr_list[69].distance;

            jsonoutputStr3S.out_gap_flag_70 = mywa2f3s.xstr_list[69].gap_flag;

            jsonoutputStr3S.out_node_num_70 = mywa2f3s.xstr_list[69].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[70].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_71 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[70].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_71 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[70].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_71 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[70].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_71 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[70].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_71 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_71 = mywa2f3s.xstr_list[70].xstr_cnt;

            jsonoutputStr3S.out_distance_71 = mywa2f3s.xstr_list[70].distance;

            jsonoutputStr3S.out_gap_flag_71 = mywa2f3s.xstr_list[70].gap_flag;

            jsonoutputStr3S.out_node_num_71 = mywa2f3s.xstr_list[70].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[71].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_72 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[71].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_72 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[71].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_72 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[71].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_72 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[71].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_72 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_72 = mywa2f3s.xstr_list[71].xstr_cnt;

            jsonoutputStr3S.out_distance_72 = mywa2f3s.xstr_list[71].distance;

            jsonoutputStr3S.out_gap_flag_72 = mywa2f3s.xstr_list[71].gap_flag;

            jsonoutputStr3S.out_node_num_72 = mywa2f3s.xstr_list[71].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[72].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_73 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[72].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_73 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[72].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_73 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[72].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_73 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[72].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_73 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_73 = mywa2f3s.xstr_list[72].xstr_cnt;

            jsonoutputStr3S.out_distance_73 = mywa2f3s.xstr_list[72].distance;

            jsonoutputStr3S.out_gap_flag_73 = mywa2f3s.xstr_list[72].gap_flag;

            jsonoutputStr3S.out_node_num_73 = mywa2f3s.xstr_list[72].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[73].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_74 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[73].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_74 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[73].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_74 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[73].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_74 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[73].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_74 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_74 = mywa2f3s.xstr_list[73].xstr_cnt;

            jsonoutputStr3S.out_distance_74 = mywa2f3s.xstr_list[73].distance;

            jsonoutputStr3S.out_gap_flag_74 = mywa2f3s.xstr_list[73].gap_flag;

            jsonoutputStr3S.out_node_num_74 = mywa2f3s.xstr_list[73].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[74].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_75 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[74].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_75 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[74].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_75 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[74].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_75 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[74].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_75 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_75 = mywa2f3s.xstr_list[74].xstr_cnt;

            jsonoutputStr3S.out_distance_75 = mywa2f3s.xstr_list[74].distance;

            jsonoutputStr3S.out_gap_flag_75 = mywa2f3s.xstr_list[74].gap_flag;

            jsonoutputStr3S.out_node_num_75 = mywa2f3s.xstr_list[74].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[75].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_76 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[75].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_76 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[75].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_76 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[75].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_76 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[75].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_76 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_76 = mywa2f3s.xstr_list[75].xstr_cnt;

            jsonoutputStr3S.out_distance_76 = mywa2f3s.xstr_list[75].distance;

            jsonoutputStr3S.out_gap_flag_76 = mywa2f3s.xstr_list[75].gap_flag;

            jsonoutputStr3S.out_node_num_76 = mywa2f3s.xstr_list[75].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[76].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_77 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[76].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_77 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[76].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_77 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[76].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_77 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[76].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_77 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_77 = mywa2f3s.xstr_list[76].xstr_cnt;

            jsonoutputStr3S.out_distance_77 = mywa2f3s.xstr_list[76].distance;

            jsonoutputStr3S.out_gap_flag_77 = mywa2f3s.xstr_list[76].gap_flag;

            jsonoutputStr3S.out_node_num_77 = mywa2f3s.xstr_list[76].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[77].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_78 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[77].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_78 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[77].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_78 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[77].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_78 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[77].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_78 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_78 = mywa2f3s.xstr_list[77].xstr_cnt;

            jsonoutputStr3S.out_distance_78 = mywa2f3s.xstr_list[77].distance;

            jsonoutputStr3S.out_gap_flag_78 = mywa2f3s.xstr_list[77].gap_flag;

            jsonoutputStr3S.out_node_num_78 = mywa2f3s.xstr_list[77].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[78].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_79 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[78].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_79 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[78].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_79 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[78].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_79 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[78].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_79 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_79 = mywa2f3s.xstr_list[78].xstr_cnt;

            jsonoutputStr3S.out_distance_79 = mywa2f3s.xstr_list[78].distance;

            jsonoutputStr3S.out_gap_flag_79 = mywa2f3s.xstr_list[78].gap_flag;

            jsonoutputStr3S.out_node_num_79 = mywa2f3s.xstr_list[78].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[79].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_80 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[79].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_80 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[79].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_80 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[79].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_80 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[79].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_80 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_80 = mywa2f3s.xstr_list[79].xstr_cnt;

            jsonoutputStr3S.out_distance_80 = mywa2f3s.xstr_list[79].distance;

            jsonoutputStr3S.out_gap_flag_80 = mywa2f3s.xstr_list[79].gap_flag;

            jsonoutputStr3S.out_node_num_80 = mywa2f3s.xstr_list[79].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[80].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_81 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[80].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_81 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[80].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_81 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[80].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_81 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[80].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_81 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_81 = mywa2f3s.xstr_list[80].xstr_cnt;

            jsonoutputStr3S.out_distance_81 = mywa2f3s.xstr_list[80].distance;

            jsonoutputStr3S.out_gap_flag_81 = mywa2f3s.xstr_list[80].gap_flag;

            jsonoutputStr3S.out_node_num_81 = mywa2f3s.xstr_list[80].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[81].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_82 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[81].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_82 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[81].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_82 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[81].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_82 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[81].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_82 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_82 = mywa2f3s.xstr_list[81].xstr_cnt;

            jsonoutputStr3S.out_distance_82 = mywa2f3s.xstr_list[81].distance;

            jsonoutputStr3S.out_gap_flag_82 = mywa2f3s.xstr_list[81].gap_flag;

            jsonoutputStr3S.out_node_num_82 = mywa2f3s.xstr_list[81].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[82].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_83 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[82].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_83 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[82].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_83 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[82].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_83 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[82].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_83 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_83 = mywa2f3s.xstr_list[82].xstr_cnt;

            jsonoutputStr3S.out_distance_83 = mywa2f3s.xstr_list[82].distance;

            jsonoutputStr3S.out_gap_flag_83 = mywa2f3s.xstr_list[82].gap_flag;

            jsonoutputStr3S.out_node_num_83 = mywa2f3s.xstr_list[82].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[83].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_84 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[83].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_84 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[83].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_84 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[83].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_84 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[83].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_84 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_84 = mywa2f3s.xstr_list[83].xstr_cnt;

            jsonoutputStr3S.out_distance_84 = mywa2f3s.xstr_list[83].distance;

            jsonoutputStr3S.out_gap_flag_84 = mywa2f3s.xstr_list[83].gap_flag;

            jsonoutputStr3S.out_node_num_84 = mywa2f3s.xstr_list[83].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[84].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_85 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[84].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_85 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[84].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_85 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[84].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_85 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[84].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_85 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_85 = mywa2f3s.xstr_list[84].xstr_cnt;

            jsonoutputStr3S.out_distance_85 = mywa2f3s.xstr_list[84].distance;

            jsonoutputStr3S.out_gap_flag_85 = mywa2f3s.xstr_list[84].gap_flag;

            jsonoutputStr3S.out_node_num_85 = mywa2f3s.xstr_list[84].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[85].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_86 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[85].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_86 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[85].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_86 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[85].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_86 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[85].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_86 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_86 = mywa2f3s.xstr_list[85].xstr_cnt;

            jsonoutputStr3S.out_distance_86 = mywa2f3s.xstr_list[85].distance;

            jsonoutputStr3S.out_gap_flag_86 = mywa2f3s.xstr_list[85].gap_flag;

            jsonoutputStr3S.out_node_num_86 = mywa2f3s.xstr_list[85].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[86].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_87 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[86].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_87 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[86].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_87 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[86].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_87 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[86].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_87 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_87 = mywa2f3s.xstr_list[86].xstr_cnt;

            jsonoutputStr3S.out_distance_87 = mywa2f3s.xstr_list[86].distance;

            jsonoutputStr3S.out_gap_flag_87 = mywa2f3s.xstr_list[86].gap_flag;

            jsonoutputStr3S.out_node_num_87 = mywa2f3s.xstr_list[86].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[87].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_88 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[87].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_88 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[87].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_88 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[87].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_88 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[87].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_88 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_88 = mywa2f3s.xstr_list[87].xstr_cnt;

            jsonoutputStr3S.out_distance_88 = mywa2f3s.xstr_list[87].distance;

            jsonoutputStr3S.out_gap_flag_88 = mywa2f3s.xstr_list[87].gap_flag;

            jsonoutputStr3S.out_node_num_88 = mywa2f3s.xstr_list[87].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[88].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_89 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[88].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_89 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[88].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_89 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[88].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_89 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[88].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_89 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_89 = mywa2f3s.xstr_list[88].xstr_cnt;

            jsonoutputStr3S.out_distance_89 = mywa2f3s.xstr_list[88].distance;

            jsonoutputStr3S.out_gap_flag_89 = mywa2f3s.xstr_list[88].gap_flag;

            jsonoutputStr3S.out_node_num_89 = mywa2f3s.xstr_list[88].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[89].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_90 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[89].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_90 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[89].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_90 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[89].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_90 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[89].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_90 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_90 = mywa2f3s.xstr_list[89].xstr_cnt;

            jsonoutputStr3S.out_distance_90 = mywa2f3s.xstr_list[89].distance;

            jsonoutputStr3S.out_gap_flag_90 = mywa2f3s.xstr_list[89].gap_flag;

            jsonoutputStr3S.out_node_num_90 = mywa2f3s.xstr_list[89].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[90].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_91 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[90].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_91 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[90].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_91 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[90].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_91 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[90].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_91 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_91 = mywa2f3s.xstr_list[90].xstr_cnt;

            jsonoutputStr3S.out_distance_91 = mywa2f3s.xstr_list[90].distance;

            jsonoutputStr3S.out_gap_flag_91 = mywa2f3s.xstr_list[90].gap_flag;

            jsonoutputStr3S.out_node_num_91 = mywa2f3s.xstr_list[90].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[91].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_92 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[91].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_92 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[91].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_92 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[91].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_92 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[91].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_92 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_92 = mywa2f3s.xstr_list[91].xstr_cnt;

            jsonoutputStr3S.out_distance_92 = mywa2f3s.xstr_list[91].distance;

            jsonoutputStr3S.out_gap_flag_92 = mywa2f3s.xstr_list[91].gap_flag;

            jsonoutputStr3S.out_node_num_92 = mywa2f3s.xstr_list[91].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[92].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_93 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[92].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_93 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[92].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_93 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[92].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_93 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[92].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_93 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_93 = mywa2f3s.xstr_list[92].xstr_cnt;

            jsonoutputStr3S.out_distance_93 = mywa2f3s.xstr_list[92].distance;

            jsonoutputStr3S.out_gap_flag_93 = mywa2f3s.xstr_list[92].gap_flag;

            jsonoutputStr3S.out_node_num_93 = mywa2f3s.xstr_list[92].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[93].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_94 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[93].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_94 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[93].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_94 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[93].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_94 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[93].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_94 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_94 = mywa2f3s.xstr_list[93].xstr_cnt;

            jsonoutputStr3S.out_distance_94 = mywa2f3s.xstr_list[93].distance;

            jsonoutputStr3S.out_gap_flag_94 = mywa2f3s.xstr_list[93].gap_flag;

            jsonoutputStr3S.out_node_num_94 = mywa2f3s.xstr_list[93].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[94].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_95 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[94].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_95 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[94].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_95 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[94].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_95 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[94].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_95 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_95 = mywa2f3s.xstr_list[94].xstr_cnt;

            jsonoutputStr3S.out_distance_95 = mywa2f3s.xstr_list[94].distance;

            jsonoutputStr3S.out_gap_flag_95 = mywa2f3s.xstr_list[94].gap_flag;

            jsonoutputStr3S.out_node_num_95 = mywa2f3s.xstr_list[94].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[95].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_96 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[95].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_96 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[95].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_96 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[95].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_96 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[95].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_96 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_96 = mywa2f3s.xstr_list[95].xstr_cnt;

            jsonoutputStr3S.out_distance_96 = mywa2f3s.xstr_list[95].distance;

            jsonoutputStr3S.out_gap_flag_96 = mywa2f3s.xstr_list[95].gap_flag;

            jsonoutputStr3S.out_node_num_96 = mywa2f3s.xstr_list[95].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[96].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_97 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[96].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_97 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[96].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_97 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[96].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_97 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[96].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_97 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_97 = mywa2f3s.xstr_list[96].xstr_cnt;

            jsonoutputStr3S.out_distance_97 = mywa2f3s.xstr_list[96].distance;

            jsonoutputStr3S.out_gap_flag_97 = mywa2f3s.xstr_list[96].gap_flag;

            jsonoutputStr3S.out_node_num_97 = mywa2f3s.xstr_list[96].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[97].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_98 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[97].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_98 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[97].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_98 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[97].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_98 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[97].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_98 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_98 = mywa2f3s.xstr_list[97].xstr_cnt;

            jsonoutputStr3S.out_distance_98 = mywa2f3s.xstr_list[97].distance;

            jsonoutputStr3S.out_gap_flag_98 = mywa2f3s.xstr_list[97].gap_flag;

            jsonoutputStr3S.out_node_num_98 = mywa2f3s.xstr_list[97].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[98].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_99 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[98].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_99 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[98].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_99 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[98].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_99 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[98].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_99 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_99 = mywa2f3s.xstr_list[98].xstr_cnt;

            jsonoutputStr3S.out_distance_99 = mywa2f3s.xstr_list[98].distance;

            jsonoutputStr3S.out_gap_flag_99 = mywa2f3s.xstr_list[98].gap_flag;

            jsonoutputStr3S.out_node_num_99 = mywa2f3s.xstr_list[98].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[99].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_100 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[99].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_100 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[99].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_100 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[99].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_100 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[99].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_100 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_100 = mywa2f3s.xstr_list[99].xstr_cnt;

            jsonoutputStr3S.out_distance_100 = mywa2f3s.xstr_list[99].distance;

            jsonoutputStr3S.out_gap_flag_100 = mywa2f3s.xstr_list[99].gap_flag;

            jsonoutputStr3S.out_node_num_100 = mywa2f3s.xstr_list[99].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[100].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_101 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[100].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_101 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[100].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_101 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[100].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_101 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[100].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_101 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_101 = mywa2f3s.xstr_list[100].xstr_cnt;

            jsonoutputStr3S.out_distance_101 = mywa2f3s.xstr_list[100].distance;

            jsonoutputStr3S.out_gap_flag_101 = mywa2f3s.xstr_list[100].gap_flag;

            jsonoutputStr3S.out_node_num_101 = mywa2f3s.xstr_list[100].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[101].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_102 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[101].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_102 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[101].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_102 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[101].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_102 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[101].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_102 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_102 = mywa2f3s.xstr_list[101].xstr_cnt;

            jsonoutputStr3S.out_distance_102 = mywa2f3s.xstr_list[101].distance;

            jsonoutputStr3S.out_gap_flag_102 = mywa2f3s.xstr_list[101].gap_flag;

            jsonoutputStr3S.out_node_num_102 = mywa2f3s.xstr_list[101].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[102].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_103 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[102].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_103 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[102].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_103 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[102].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_103 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[102].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_103 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_103 = mywa2f3s.xstr_list[102].xstr_cnt;

            jsonoutputStr3S.out_distance_103 = mywa2f3s.xstr_list[102].distance;

            jsonoutputStr3S.out_gap_flag_103 = mywa2f3s.xstr_list[102].gap_flag;

            jsonoutputStr3S.out_node_num_103 = mywa2f3s.xstr_list[102].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[103].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_104 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[103].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_104 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[103].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_104 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[103].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_104 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[103].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_104 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_104 = mywa2f3s.xstr_list[103].xstr_cnt;

            jsonoutputStr3S.out_distance_104 = mywa2f3s.xstr_list[103].distance;

            jsonoutputStr3S.out_gap_flag_104 = mywa2f3s.xstr_list[103].gap_flag;

            jsonoutputStr3S.out_node_num_104 = mywa2f3s.xstr_list[103].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[104].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_105 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[104].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_105 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[104].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_105 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[104].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_105 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[104].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_105 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_105 = mywa2f3s.xstr_list[104].xstr_cnt;

            jsonoutputStr3S.out_distance_105 = mywa2f3s.xstr_list[104].distance;

            jsonoutputStr3S.out_gap_flag_105 = mywa2f3s.xstr_list[104].gap_flag;

            jsonoutputStr3S.out_node_num_105 = mywa2f3s.xstr_list[104].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[105].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_106 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[105].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_106 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[105].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_106 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[105].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_106 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[105].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_106 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_106 = mywa2f3s.xstr_list[105].xstr_cnt;

            jsonoutputStr3S.out_distance_106 = mywa2f3s.xstr_list[105].distance;

            jsonoutputStr3S.out_gap_flag_106 = mywa2f3s.xstr_list[105].gap_flag;

            jsonoutputStr3S.out_node_num_106 = mywa2f3s.xstr_list[105].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[106].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_107 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[106].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_107 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[106].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_107 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[106].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_107 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[106].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_107 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_107 = mywa2f3s.xstr_list[106].xstr_cnt;

            jsonoutputStr3S.out_distance_107 = mywa2f3s.xstr_list[106].distance;

            jsonoutputStr3S.out_gap_flag_107 = mywa2f3s.xstr_list[106].gap_flag;

            jsonoutputStr3S.out_node_num_107 = mywa2f3s.xstr_list[106].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[107].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_108 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[107].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_108 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[107].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_108 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[107].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_108 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[107].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_108 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_108 = mywa2f3s.xstr_list[107].xstr_cnt;

            jsonoutputStr3S.out_distance_108 = mywa2f3s.xstr_list[107].distance;

            jsonoutputStr3S.out_gap_flag_108 = mywa2f3s.xstr_list[107].gap_flag;

            jsonoutputStr3S.out_node_num_108 = mywa2f3s.xstr_list[107].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[108].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_109 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[108].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_109 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[108].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_109 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[108].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_109 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[108].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_109 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_109 = mywa2f3s.xstr_list[108].xstr_cnt;

            jsonoutputStr3S.out_distance_109 = mywa2f3s.xstr_list[108].distance;

            jsonoutputStr3S.out_gap_flag_109 = mywa2f3s.xstr_list[108].gap_flag;

            jsonoutputStr3S.out_node_num_109 = mywa2f3s.xstr_list[108].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[109].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_110 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[109].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_110 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[109].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_110 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[109].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_110 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[109].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_110 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_110 = mywa2f3s.xstr_list[109].xstr_cnt;

            jsonoutputStr3S.out_distance_110 = mywa2f3s.xstr_list[109].distance;

            jsonoutputStr3S.out_gap_flag_110 = mywa2f3s.xstr_list[109].gap_flag;

            jsonoutputStr3S.out_node_num_110 = mywa2f3s.xstr_list[109].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[110].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_111 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[110].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_111 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[110].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_111 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[110].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_111 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[110].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_111 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_111 = mywa2f3s.xstr_list[110].xstr_cnt;

            jsonoutputStr3S.out_distance_111 = mywa2f3s.xstr_list[110].distance;

            jsonoutputStr3S.out_gap_flag_111 = mywa2f3s.xstr_list[110].gap_flag;

            jsonoutputStr3S.out_node_num_111 = mywa2f3s.xstr_list[110].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[111].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_112 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[111].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_112 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[111].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_112 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[111].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_112 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[111].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_112 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_112 = mywa2f3s.xstr_list[111].xstr_cnt;

            jsonoutputStr3S.out_distance_112 = mywa2f3s.xstr_list[111].distance;

            jsonoutputStr3S.out_gap_flag_112 = mywa2f3s.xstr_list[111].gap_flag;

            jsonoutputStr3S.out_node_num_112 = mywa2f3s.xstr_list[111].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[112].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_113 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[112].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_113 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[112].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_113 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[112].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_113 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[112].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_113 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_113 = mywa2f3s.xstr_list[112].xstr_cnt;

            jsonoutputStr3S.out_distance_113 = mywa2f3s.xstr_list[112].distance;

            jsonoutputStr3S.out_gap_flag_113 = mywa2f3s.xstr_list[112].gap_flag;

            jsonoutputStr3S.out_node_num_113 = mywa2f3s.xstr_list[112].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[113].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_114 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[113].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_114 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[113].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_114 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[113].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_114 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[113].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_114 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_114 = mywa2f3s.xstr_list[113].xstr_cnt;

            jsonoutputStr3S.out_distance_114 = mywa2f3s.xstr_list[113].distance;

            jsonoutputStr3S.out_gap_flag_114 = mywa2f3s.xstr_list[113].gap_flag;

            jsonoutputStr3S.out_node_num_114 = mywa2f3s.xstr_list[113].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[114].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_115 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[114].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_115 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[114].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_115 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[114].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_115 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[114].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_115 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_115 = mywa2f3s.xstr_list[114].xstr_cnt;

            jsonoutputStr3S.out_distance_115 = mywa2f3s.xstr_list[114].distance;

            jsonoutputStr3S.out_gap_flag_115 = mywa2f3s.xstr_list[114].gap_flag;

            jsonoutputStr3S.out_node_num_115 = mywa2f3s.xstr_list[114].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[115].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_116 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[115].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_116 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[115].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_116 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[115].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_116 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[115].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_116 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_116 = mywa2f3s.xstr_list[115].xstr_cnt;

            jsonoutputStr3S.out_distance_116 = mywa2f3s.xstr_list[115].distance;

            jsonoutputStr3S.out_gap_flag_116 = mywa2f3s.xstr_list[115].gap_flag;

            jsonoutputStr3S.out_node_num_116 = mywa2f3s.xstr_list[115].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[116].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_117 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[116].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_117 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[116].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_117 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[116].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_117 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[116].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_117 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_117 = mywa2f3s.xstr_list[116].xstr_cnt;

            jsonoutputStr3S.out_distance_117 = mywa2f3s.xstr_list[116].distance;

            jsonoutputStr3S.out_gap_flag_117 = mywa2f3s.xstr_list[116].gap_flag;

            jsonoutputStr3S.out_node_num_117 = mywa2f3s.xstr_list[116].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[117].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_118 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[117].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_118 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[117].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_118 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[117].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_118 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[117].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_118 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_118 = mywa2f3s.xstr_list[117].xstr_cnt;

            jsonoutputStr3S.out_distance_118 = mywa2f3s.xstr_list[117].distance;

            jsonoutputStr3S.out_gap_flag_118 = mywa2f3s.xstr_list[117].gap_flag;

            jsonoutputStr3S.out_node_num_118 = mywa2f3s.xstr_list[117].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[118].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_119 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[118].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_119 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[118].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_119 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[118].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_119 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[118].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_119 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_119 = mywa2f3s.xstr_list[118].xstr_cnt;

            jsonoutputStr3S.out_distance_119 = mywa2f3s.xstr_list[118].distance;

            jsonoutputStr3S.out_gap_flag_119 = mywa2f3s.xstr_list[118].gap_flag;

            jsonoutputStr3S.out_node_num_119 = mywa2f3s.xstr_list[118].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[119].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_120 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[119].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_120 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[119].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_120 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[119].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_120 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[119].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_120 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_120 = mywa2f3s.xstr_list[119].xstr_cnt;

            jsonoutputStr3S.out_distance_120 = mywa2f3s.xstr_list[119].distance;

            jsonoutputStr3S.out_gap_flag_120 = mywa2f3s.xstr_list[119].gap_flag;

            jsonoutputStr3S.out_node_num_120 = mywa2f3s.xstr_list[119].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[120].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_121 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[120].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_121 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[120].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_121 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[120].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_121 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[120].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_121 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_121 = mywa2f3s.xstr_list[120].xstr_cnt;

            jsonoutputStr3S.out_distance_121 = mywa2f3s.xstr_list[120].distance;

            jsonoutputStr3S.out_gap_flag_121 = mywa2f3s.xstr_list[120].gap_flag;

            jsonoutputStr3S.out_node_num_121 = mywa2f3s.xstr_list[120].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[121].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_122 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[121].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_122 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[121].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_122 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[121].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_122 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[121].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_122 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_122 = mywa2f3s.xstr_list[121].xstr_cnt;

            jsonoutputStr3S.out_distance_122 = mywa2f3s.xstr_list[121].distance;

            jsonoutputStr3S.out_gap_flag_122 = mywa2f3s.xstr_list[121].gap_flag;

            jsonoutputStr3S.out_node_num_122 = mywa2f3s.xstr_list[121].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[122].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_123 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[122].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_123 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[122].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_123 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[122].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_123 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[122].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_123 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_123 = mywa2f3s.xstr_list[122].xstr_cnt;

            jsonoutputStr3S.out_distance_123 = mywa2f3s.xstr_list[122].distance;

            jsonoutputStr3S.out_gap_flag_123 = mywa2f3s.xstr_list[122].gap_flag;

            jsonoutputStr3S.out_node_num_123 = mywa2f3s.xstr_list[122].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[123].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_124 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[123].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_124 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[123].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_124 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[123].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_124 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[123].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_124 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_124 = mywa2f3s.xstr_list[123].xstr_cnt;

            jsonoutputStr3S.out_distance_124 = mywa2f3s.xstr_list[123].distance;

            jsonoutputStr3S.out_gap_flag_124 = mywa2f3s.xstr_list[123].gap_flag;

            jsonoutputStr3S.out_node_num_124 = mywa2f3s.xstr_list[123].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[124].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_125 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[124].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_125 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[124].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_125 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[124].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_125 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[124].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_125 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_125 = mywa2f3s.xstr_list[124].xstr_cnt;

            jsonoutputStr3S.out_distance_125 = mywa2f3s.xstr_list[124].distance;

            jsonoutputStr3S.out_gap_flag_125 = mywa2f3s.xstr_list[124].gap_flag;

            jsonoutputStr3S.out_node_num_125 = mywa2f3s.xstr_list[124].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[125].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_126 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[125].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_126 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[125].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_126 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[125].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_126 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[125].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_126 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_126 = mywa2f3s.xstr_list[125].xstr_cnt;

            jsonoutputStr3S.out_distance_126 = mywa2f3s.xstr_list[125].distance;

            jsonoutputStr3S.out_gap_flag_126 = mywa2f3s.xstr_list[125].gap_flag;

            jsonoutputStr3S.out_node_num_126 = mywa2f3s.xstr_list[125].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[126].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_127 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[126].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_127 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[126].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_127 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[126].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_127 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[126].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_127 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_127 = mywa2f3s.xstr_list[126].xstr_cnt;

            jsonoutputStr3S.out_distance_127 = mywa2f3s.xstr_list[126].distance;

            jsonoutputStr3S.out_gap_flag_127 = mywa2f3s.xstr_list[126].gap_flag;

            jsonoutputStr3S.out_node_num_127 = mywa2f3s.xstr_list[126].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[127].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_128 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[127].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_128 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[127].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_128 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[127].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_128 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[127].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_128 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_128 = mywa2f3s.xstr_list[127].xstr_cnt;

            jsonoutputStr3S.out_distance_128 = mywa2f3s.xstr_list[127].distance;

            jsonoutputStr3S.out_gap_flag_128 = mywa2f3s.xstr_list[127].gap_flag;

            jsonoutputStr3S.out_node_num_128 = mywa2f3s.xstr_list[127].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[128].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_129 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[128].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_129 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[128].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_129 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[128].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_129 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[128].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_129 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_129 = mywa2f3s.xstr_list[128].xstr_cnt;

            jsonoutputStr3S.out_distance_129 = mywa2f3s.xstr_list[128].distance;

            jsonoutputStr3S.out_gap_flag_129 = mywa2f3s.xstr_list[128].gap_flag;

            jsonoutputStr3S.out_node_num_129 = mywa2f3s.xstr_list[128].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[129].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_130 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[129].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_130 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[129].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_130 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[129].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_130 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[129].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_130 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_130 = mywa2f3s.xstr_list[129].xstr_cnt;

            jsonoutputStr3S.out_distance_130 = mywa2f3s.xstr_list[129].distance;

            jsonoutputStr3S.out_gap_flag_130 = mywa2f3s.xstr_list[129].gap_flag;

            jsonoutputStr3S.out_node_num_130 = mywa2f3s.xstr_list[129].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[130].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_131 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[130].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_131 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[130].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_131 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[130].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_131 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[130].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_131 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_131 = mywa2f3s.xstr_list[130].xstr_cnt;

            jsonoutputStr3S.out_distance_131 = mywa2f3s.xstr_list[130].distance;

            jsonoutputStr3S.out_gap_flag_131 = mywa2f3s.xstr_list[130].gap_flag;

            jsonoutputStr3S.out_node_num_131 = mywa2f3s.xstr_list[130].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[131].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_132 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[131].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_132 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[131].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_132 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[131].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_132 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[131].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_132 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_132 = mywa2f3s.xstr_list[131].xstr_cnt;

            jsonoutputStr3S.out_distance_132 = mywa2f3s.xstr_list[131].distance;

            jsonoutputStr3S.out_gap_flag_132 = mywa2f3s.xstr_list[131].gap_flag;

            jsonoutputStr3S.out_node_num_132 = mywa2f3s.xstr_list[131].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[132].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_133 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[132].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_133 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[132].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_133 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[132].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_133 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[132].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_133 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_133 = mywa2f3s.xstr_list[132].xstr_cnt;

            jsonoutputStr3S.out_distance_133 = mywa2f3s.xstr_list[132].distance;

            jsonoutputStr3S.out_gap_flag_133 = mywa2f3s.xstr_list[132].gap_flag;

            jsonoutputStr3S.out_node_num_133 = mywa2f3s.xstr_list[132].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[133].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_134 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[133].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_134 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[133].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_134 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[133].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_134 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[133].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_134 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_134 = mywa2f3s.xstr_list[133].xstr_cnt;

            jsonoutputStr3S.out_distance_134 = mywa2f3s.xstr_list[133].distance;

            jsonoutputStr3S.out_gap_flag_134 = mywa2f3s.xstr_list[133].gap_flag;

            jsonoutputStr3S.out_node_num_134 = mywa2f3s.xstr_list[133].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[134].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_135 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[134].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_135 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[134].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_135 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[134].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_135 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[134].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_135 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_135 = mywa2f3s.xstr_list[134].xstr_cnt;

            jsonoutputStr3S.out_distance_135 = mywa2f3s.xstr_list[134].distance;

            jsonoutputStr3S.out_gap_flag_135 = mywa2f3s.xstr_list[134].gap_flag;

            jsonoutputStr3S.out_node_num_135 = mywa2f3s.xstr_list[134].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[135].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_136 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[135].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_136 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[135].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_136 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[135].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_136 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[135].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_136 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_136 = mywa2f3s.xstr_list[135].xstr_cnt;

            jsonoutputStr3S.out_distance_136 = mywa2f3s.xstr_list[135].distance;

            jsonoutputStr3S.out_gap_flag_136 = mywa2f3s.xstr_list[135].gap_flag;

            jsonoutputStr3S.out_node_num_136 = mywa2f3s.xstr_list[135].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[136].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_137 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[136].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_137 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[136].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_137 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[136].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_137 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[136].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_137 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_137 = mywa2f3s.xstr_list[136].xstr_cnt;

            jsonoutputStr3S.out_distance_137 = mywa2f3s.xstr_list[136].distance;

            jsonoutputStr3S.out_gap_flag_137 = mywa2f3s.xstr_list[136].gap_flag;

            jsonoutputStr3S.out_node_num_137 = mywa2f3s.xstr_list[136].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[137].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_138 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[137].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_138 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[137].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_138 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[137].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_138 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[137].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_138 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_138 = mywa2f3s.xstr_list[137].xstr_cnt;

            jsonoutputStr3S.out_distance_138 = mywa2f3s.xstr_list[137].distance;

            jsonoutputStr3S.out_gap_flag_138 = mywa2f3s.xstr_list[137].gap_flag;

            jsonoutputStr3S.out_node_num_138 = mywa2f3s.xstr_list[137].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[138].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_139 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[138].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_139 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[138].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_139 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[138].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_139 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[138].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_139 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_139 = mywa2f3s.xstr_list[138].xstr_cnt;

            jsonoutputStr3S.out_distance_139 = mywa2f3s.xstr_list[138].distance;

            jsonoutputStr3S.out_gap_flag_139 = mywa2f3s.xstr_list[138].gap_flag;

            jsonoutputStr3S.out_node_num_139 = mywa2f3s.xstr_list[138].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[139].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_140 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[139].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_140 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[139].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_140 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[139].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_140 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[139].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_140 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_140 = mywa2f3s.xstr_list[139].xstr_cnt;

            jsonoutputStr3S.out_distance_140 = mywa2f3s.xstr_list[139].distance;

            jsonoutputStr3S.out_gap_flag_140 = mywa2f3s.xstr_list[139].gap_flag;

            jsonoutputStr3S.out_node_num_140 = mywa2f3s.xstr_list[139].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[140].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_141 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[140].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_141 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[140].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_141 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[140].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_141 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[140].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_141 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_141 = mywa2f3s.xstr_list[140].xstr_cnt;

            jsonoutputStr3S.out_distance_141 = mywa2f3s.xstr_list[140].distance;

            jsonoutputStr3S.out_gap_flag_141 = mywa2f3s.xstr_list[140].gap_flag;

            jsonoutputStr3S.out_node_num_141 = mywa2f3s.xstr_list[140].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[141].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_142 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[141].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_142 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[141].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_142 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[141].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_142 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[141].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_142 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_142 = mywa2f3s.xstr_list[141].xstr_cnt;

            jsonoutputStr3S.out_distance_142 = mywa2f3s.xstr_list[141].distance;

            jsonoutputStr3S.out_gap_flag_142 = mywa2f3s.xstr_list[141].gap_flag;

            jsonoutputStr3S.out_node_num_142 = mywa2f3s.xstr_list[141].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[142].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_143 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[142].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_143 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[142].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_143 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[142].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_143 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[142].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_143 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_143 = mywa2f3s.xstr_list[142].xstr_cnt;

            jsonoutputStr3S.out_distance_143 = mywa2f3s.xstr_list[142].distance;

            jsonoutputStr3S.out_gap_flag_143 = mywa2f3s.xstr_list[142].gap_flag;

            jsonoutputStr3S.out_node_num_143 = mywa2f3s.xstr_list[142].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[143].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_144 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[143].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_144 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[143].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_144 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[143].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_144 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[143].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_144 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_144 = mywa2f3s.xstr_list[143].xstr_cnt;

            jsonoutputStr3S.out_distance_144 = mywa2f3s.xstr_list[143].distance;

            jsonoutputStr3S.out_gap_flag_144 = mywa2f3s.xstr_list[143].gap_flag;

            jsonoutputStr3S.out_node_num_144 = mywa2f3s.xstr_list[143].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[144].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_145 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[144].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_145 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[144].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_145 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[144].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_145 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[144].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_145 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_145 = mywa2f3s.xstr_list[144].xstr_cnt;

            jsonoutputStr3S.out_distance_145 = mywa2f3s.xstr_list[144].distance;

            jsonoutputStr3S.out_gap_flag_145 = mywa2f3s.xstr_list[144].gap_flag;

            jsonoutputStr3S.out_node_num_145 = mywa2f3s.xstr_list[144].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[145].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_146 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[145].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_146 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[145].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_146 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[145].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_146 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[145].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_146 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_146 = mywa2f3s.xstr_list[145].xstr_cnt;

            jsonoutputStr3S.out_distance_146 = mywa2f3s.xstr_list[145].distance;

            jsonoutputStr3S.out_gap_flag_146 = mywa2f3s.xstr_list[145].gap_flag;

            jsonoutputStr3S.out_node_num_146 = mywa2f3s.xstr_list[145].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[146].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_147 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[146].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_147 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[146].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_147 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[146].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_147 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[146].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_147 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_147 = mywa2f3s.xstr_list[146].xstr_cnt;

            jsonoutputStr3S.out_distance_147 = mywa2f3s.xstr_list[146].distance;

            jsonoutputStr3S.out_gap_flag_147 = mywa2f3s.xstr_list[146].gap_flag;

            jsonoutputStr3S.out_node_num_147 = mywa2f3s.xstr_list[146].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[147].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_148 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[147].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_148 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[147].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_148 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[147].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_148 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[147].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_148 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_148 = mywa2f3s.xstr_list[147].xstr_cnt;

            jsonoutputStr3S.out_distance_148 = mywa2f3s.xstr_list[147].distance;

            jsonoutputStr3S.out_gap_flag_148 = mywa2f3s.xstr_list[147].gap_flag;

            jsonoutputStr3S.out_node_num_148 = mywa2f3s.xstr_list[147].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[148].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_149 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[148].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_149 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[148].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_149 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[148].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_149 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[148].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_149 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_149 = mywa2f3s.xstr_list[148].xstr_cnt;

            jsonoutputStr3S.out_distance_149 = mywa2f3s.xstr_list[148].distance;

            jsonoutputStr3S.out_gap_flag_149 = mywa2f3s.xstr_list[148].gap_flag;

            jsonoutputStr3S.out_node_num_149 = mywa2f3s.xstr_list[148].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[149].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_150 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[149].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_150 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[149].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_150 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[149].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_150 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[149].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_150 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_150 = mywa2f3s.xstr_list[149].xstr_cnt;

            jsonoutputStr3S.out_distance_150 = mywa2f3s.xstr_list[149].distance;

            jsonoutputStr3S.out_gap_flag_150 = mywa2f3s.xstr_list[149].gap_flag;

            jsonoutputStr3S.out_node_num_150 = mywa2f3s.xstr_list[149].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[150].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_151 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[150].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_151 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[150].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_151 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[150].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_151 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[150].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_151 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_151 = mywa2f3s.xstr_list[150].xstr_cnt;

            jsonoutputStr3S.out_distance_151 = mywa2f3s.xstr_list[150].distance;

            jsonoutputStr3S.out_gap_flag_151 = mywa2f3s.xstr_list[150].gap_flag;

            jsonoutputStr3S.out_node_num_151 = mywa2f3s.xstr_list[150].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[151].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_152 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[151].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_152 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[151].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_152 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[151].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_152 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[151].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_152 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_152 = mywa2f3s.xstr_list[151].xstr_cnt;

            jsonoutputStr3S.out_distance_152 = mywa2f3s.xstr_list[151].distance;

            jsonoutputStr3S.out_gap_flag_152 = mywa2f3s.xstr_list[151].gap_flag;

            jsonoutputStr3S.out_node_num_152 = mywa2f3s.xstr_list[151].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[152].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_153 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[152].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_153 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[152].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_153 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[152].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_153 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[152].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_153 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_153 = mywa2f3s.xstr_list[152].xstr_cnt;

            jsonoutputStr3S.out_distance_153 = mywa2f3s.xstr_list[152].distance;

            jsonoutputStr3S.out_gap_flag_153 = mywa2f3s.xstr_list[152].gap_flag;

            jsonoutputStr3S.out_node_num_153 = mywa2f3s.xstr_list[152].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[153].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_154 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[153].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_154 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[153].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_154 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[153].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_154 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[153].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_154 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_154 = mywa2f3s.xstr_list[153].xstr_cnt;

            jsonoutputStr3S.out_distance_154 = mywa2f3s.xstr_list[153].distance;

            jsonoutputStr3S.out_gap_flag_154 = mywa2f3s.xstr_list[153].gap_flag;

            jsonoutputStr3S.out_node_num_154 = mywa2f3s.xstr_list[153].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[154].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_155 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[154].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_155 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[154].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_155 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[154].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_155 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[154].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_155 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_155 = mywa2f3s.xstr_list[154].xstr_cnt;

            jsonoutputStr3S.out_distance_155 = mywa2f3s.xstr_list[154].distance;

            jsonoutputStr3S.out_gap_flag_155 = mywa2f3s.xstr_list[154].gap_flag;

            jsonoutputStr3S.out_node_num_155 = mywa2f3s.xstr_list[154].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[155].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_156 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[155].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_156 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[155].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_156 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[155].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_156 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[155].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_156 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_156 = mywa2f3s.xstr_list[155].xstr_cnt;

            jsonoutputStr3S.out_distance_156 = mywa2f3s.xstr_list[155].distance;

            jsonoutputStr3S.out_gap_flag_156 = mywa2f3s.xstr_list[155].gap_flag;

            jsonoutputStr3S.out_node_num_156 = mywa2f3s.xstr_list[155].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[156].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_157 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[156].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_157 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[156].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_157 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[156].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_157 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[156].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_157 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_157 = mywa2f3s.xstr_list[156].xstr_cnt;

            jsonoutputStr3S.out_distance_157 = mywa2f3s.xstr_list[156].distance;

            jsonoutputStr3S.out_gap_flag_157 = mywa2f3s.xstr_list[156].gap_flag;

            jsonoutputStr3S.out_node_num_157 = mywa2f3s.xstr_list[156].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[157].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_158 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[157].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_158 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[157].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_158 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[157].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_158 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[157].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_158 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_158 = mywa2f3s.xstr_list[157].xstr_cnt;

            jsonoutputStr3S.out_distance_158 = mywa2f3s.xstr_list[157].distance;

            jsonoutputStr3S.out_gap_flag_158 = mywa2f3s.xstr_list[157].gap_flag;

            jsonoutputStr3S.out_node_num_158 = mywa2f3s.xstr_list[157].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[158].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_159 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[158].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_159 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[158].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_159 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[158].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_159 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[158].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_159 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_159 = mywa2f3s.xstr_list[158].xstr_cnt;

            jsonoutputStr3S.out_distance_159 = mywa2f3s.xstr_list[158].distance;

            jsonoutputStr3S.out_gap_flag_159 = mywa2f3s.xstr_list[158].gap_flag;

            jsonoutputStr3S.out_node_num_159 = mywa2f3s.xstr_list[158].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[159].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_160 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[159].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_160 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[159].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_160 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[159].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_160 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[159].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_160 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_160 = mywa2f3s.xstr_list[159].xstr_cnt;

            jsonoutputStr3S.out_distance_160 = mywa2f3s.xstr_list[159].distance;

            jsonoutputStr3S.out_gap_flag_160 = mywa2f3s.xstr_list[159].gap_flag;

            jsonoutputStr3S.out_node_num_160 = mywa2f3s.xstr_list[159].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[160].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_161 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[160].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_161 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[160].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_161 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[160].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_161 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[160].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_161 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_161 = mywa2f3s.xstr_list[160].xstr_cnt;

            jsonoutputStr3S.out_distance_161 = mywa2f3s.xstr_list[160].distance;

            jsonoutputStr3S.out_gap_flag_161 = mywa2f3s.xstr_list[160].gap_flag;

            jsonoutputStr3S.out_node_num_161 = mywa2f3s.xstr_list[160].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[161].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_162 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[161].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_162 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[161].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_162 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[161].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_162 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[161].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_162 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_162 = mywa2f3s.xstr_list[161].xstr_cnt;

            jsonoutputStr3S.out_distance_162 = mywa2f3s.xstr_list[161].distance;

            jsonoutputStr3S.out_gap_flag_162 = mywa2f3s.xstr_list[161].gap_flag;

            jsonoutputStr3S.out_node_num_162 = mywa2f3s.xstr_list[161].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[162].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_163 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[162].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_163 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[162].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_163 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[162].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_163 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[162].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_163 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_163 = mywa2f3s.xstr_list[162].xstr_cnt;

            jsonoutputStr3S.out_distance_163 = mywa2f3s.xstr_list[162].distance;

            jsonoutputStr3S.out_gap_flag_163 = mywa2f3s.xstr_list[162].gap_flag;

            jsonoutputStr3S.out_node_num_163 = mywa2f3s.xstr_list[162].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[163].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_164 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[163].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_164 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[163].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_164 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[163].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_164 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[163].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_164 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_164 = mywa2f3s.xstr_list[163].xstr_cnt;

            jsonoutputStr3S.out_distance_164 = mywa2f3s.xstr_list[163].distance;

            jsonoutputStr3S.out_gap_flag_164 = mywa2f3s.xstr_list[163].gap_flag;

            jsonoutputStr3S.out_node_num_164 = mywa2f3s.xstr_list[163].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[164].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_165 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[164].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_165 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[164].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_165 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[164].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_165 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[164].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_165 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_165 = mywa2f3s.xstr_list[164].xstr_cnt;

            jsonoutputStr3S.out_distance_165 = mywa2f3s.xstr_list[164].distance;

            jsonoutputStr3S.out_gap_flag_165 = mywa2f3s.xstr_list[164].gap_flag;

            jsonoutputStr3S.out_node_num_165 = mywa2f3s.xstr_list[164].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[165].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_166 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[165].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_166 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[165].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_166 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[165].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_166 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[165].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_166 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_166 = mywa2f3s.xstr_list[165].xstr_cnt;

            jsonoutputStr3S.out_distance_166 = mywa2f3s.xstr_list[165].distance;

            jsonoutputStr3S.out_gap_flag_166 = mywa2f3s.xstr_list[165].gap_flag;

            jsonoutputStr3S.out_node_num_166 = mywa2f3s.xstr_list[165].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[166].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_167 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[166].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_167 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[166].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_167 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[166].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_167 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[166].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_167 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_167 = mywa2f3s.xstr_list[166].xstr_cnt;

            jsonoutputStr3S.out_distance_167 = mywa2f3s.xstr_list[166].distance;

            jsonoutputStr3S.out_gap_flag_167 = mywa2f3s.xstr_list[166].gap_flag;

            jsonoutputStr3S.out_node_num_167 = mywa2f3s.xstr_list[166].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[167].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_168 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[167].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_168 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[167].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_168 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[167].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_168 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[167].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_168 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_168 = mywa2f3s.xstr_list[167].xstr_cnt;

            jsonoutputStr3S.out_distance_168 = mywa2f3s.xstr_list[167].distance;

            jsonoutputStr3S.out_gap_flag_168 = mywa2f3s.xstr_list[167].gap_flag;

            jsonoutputStr3S.out_node_num_168 = mywa2f3s.xstr_list[167].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[168].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_169 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[168].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_169 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[168].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_169 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[168].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_169 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[168].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_169 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_169 = mywa2f3s.xstr_list[168].xstr_cnt;

            jsonoutputStr3S.out_distance_169 = mywa2f3s.xstr_list[168].distance;

            jsonoutputStr3S.out_gap_flag_169 = mywa2f3s.xstr_list[168].gap_flag;

            jsonoutputStr3S.out_node_num_169 = mywa2f3s.xstr_list[168].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[169].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_170 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[169].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_170 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[169].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_170 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[169].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_170 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[169].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_170 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_170 = mywa2f3s.xstr_list[169].xstr_cnt;

            jsonoutputStr3S.out_distance_170 = mywa2f3s.xstr_list[169].distance;

            jsonoutputStr3S.out_gap_flag_170 = mywa2f3s.xstr_list[169].gap_flag;

            jsonoutputStr3S.out_node_num_170 = mywa2f3s.xstr_list[169].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[170].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_171 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[170].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_171 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[170].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_171 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[170].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_171 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[170].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_171 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_171 = mywa2f3s.xstr_list[170].xstr_cnt;

            jsonoutputStr3S.out_distance_171 = mywa2f3s.xstr_list[170].distance;

            jsonoutputStr3S.out_gap_flag_171 = mywa2f3s.xstr_list[170].gap_flag;

            jsonoutputStr3S.out_node_num_171 = mywa2f3s.xstr_list[170].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[171].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_172 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[171].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_172 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[171].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_172 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[171].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_172 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[171].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_172 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_172 = mywa2f3s.xstr_list[171].xstr_cnt;

            jsonoutputStr3S.out_distance_172 = mywa2f3s.xstr_list[171].distance;

            jsonoutputStr3S.out_gap_flag_172 = mywa2f3s.xstr_list[171].gap_flag;

            jsonoutputStr3S.out_node_num_172 = mywa2f3s.xstr_list[171].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[172].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_173 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[172].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_173 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[172].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_173 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[172].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_173 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[172].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_173 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_173 = mywa2f3s.xstr_list[172].xstr_cnt;

            jsonoutputStr3S.out_distance_173 = mywa2f3s.xstr_list[172].distance;

            jsonoutputStr3S.out_gap_flag_173 = mywa2f3s.xstr_list[172].gap_flag;

            jsonoutputStr3S.out_node_num_173 = mywa2f3s.xstr_list[172].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[173].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_174 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[173].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_174 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[173].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_174 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[173].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_174 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[173].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_174 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_174 = mywa2f3s.xstr_list[173].xstr_cnt;

            jsonoutputStr3S.out_distance_174 = mywa2f3s.xstr_list[173].distance;

            jsonoutputStr3S.out_gap_flag_174 = mywa2f3s.xstr_list[173].gap_flag;

            jsonoutputStr3S.out_node_num_174 = mywa2f3s.xstr_list[173].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[174].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_175 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[174].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_175 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[174].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_175 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[174].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_175 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[174].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_175 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_175 = mywa2f3s.xstr_list[174].xstr_cnt;

            jsonoutputStr3S.out_distance_175 = mywa2f3s.xstr_list[174].distance;

            jsonoutputStr3S.out_gap_flag_175 = mywa2f3s.xstr_list[174].gap_flag;

            jsonoutputStr3S.out_node_num_175 = mywa2f3s.xstr_list[174].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[175].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_176 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[175].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_176 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[175].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_176 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[175].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_176 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[175].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_176 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_176 = mywa2f3s.xstr_list[175].xstr_cnt;

            jsonoutputStr3S.out_distance_176 = mywa2f3s.xstr_list[175].distance;

            jsonoutputStr3S.out_gap_flag_176 = mywa2f3s.xstr_list[175].gap_flag;

            jsonoutputStr3S.out_node_num_176 = mywa2f3s.xstr_list[175].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[176].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_177 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[176].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_177 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[176].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_177 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[176].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_177 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[176].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_177 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_177 = mywa2f3s.xstr_list[176].xstr_cnt;

            jsonoutputStr3S.out_distance_177 = mywa2f3s.xstr_list[176].distance;

            jsonoutputStr3S.out_gap_flag_177 = mywa2f3s.xstr_list[176].gap_flag;

            jsonoutputStr3S.out_node_num_177 = mywa2f3s.xstr_list[176].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[177].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_178 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[177].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_178 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[177].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_178 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[177].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_178 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[177].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_178 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_178 = mywa2f3s.xstr_list[177].xstr_cnt;

            jsonoutputStr3S.out_distance_178 = mywa2f3s.xstr_list[177].distance;

            jsonoutputStr3S.out_gap_flag_178 = mywa2f3s.xstr_list[177].gap_flag;

            jsonoutputStr3S.out_node_num_178 = mywa2f3s.xstr_list[177].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[178].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_179 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[178].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_179 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[178].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_179 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[178].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_179 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[178].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_179 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_179 = mywa2f3s.xstr_list[178].xstr_cnt;

            jsonoutputStr3S.out_distance_179 = mywa2f3s.xstr_list[178].distance;

            jsonoutputStr3S.out_gap_flag_179 = mywa2f3s.xstr_list[178].gap_flag;

            jsonoutputStr3S.out_node_num_179 = mywa2f3s.xstr_list[178].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[179].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_180 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[179].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_180 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[179].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_180 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[179].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_180 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[179].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_180 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_180 = mywa2f3s.xstr_list[179].xstr_cnt;

            jsonoutputStr3S.out_distance_180 = mywa2f3s.xstr_list[179].distance;

            jsonoutputStr3S.out_gap_flag_180 = mywa2f3s.xstr_list[179].gap_flag;

            jsonoutputStr3S.out_node_num_180 = mywa2f3s.xstr_list[179].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[180].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_181 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[180].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_181 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[180].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_181 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[180].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_181 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[180].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_181 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_181 = mywa2f3s.xstr_list[180].xstr_cnt;

            jsonoutputStr3S.out_distance_181 = mywa2f3s.xstr_list[180].distance;

            jsonoutputStr3S.out_gap_flag_181 = mywa2f3s.xstr_list[180].gap_flag;

            jsonoutputStr3S.out_node_num_181 = mywa2f3s.xstr_list[180].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[181].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_182 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[181].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_182 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[181].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_182 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[181].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_182 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[181].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_182 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_182 = mywa2f3s.xstr_list[181].xstr_cnt;

            jsonoutputStr3S.out_distance_182 = mywa2f3s.xstr_list[181].distance;

            jsonoutputStr3S.out_gap_flag_182 = mywa2f3s.xstr_list[181].gap_flag;

            jsonoutputStr3S.out_node_num_182 = mywa2f3s.xstr_list[181].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[182].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_183 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[182].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_183 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[182].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_183 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[182].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_183 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[182].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_183 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_183 = mywa2f3s.xstr_list[182].xstr_cnt;

            jsonoutputStr3S.out_distance_183 = mywa2f3s.xstr_list[182].distance;

            jsonoutputStr3S.out_gap_flag_183 = mywa2f3s.xstr_list[182].gap_flag;

            jsonoutputStr3S.out_node_num_183 = mywa2f3s.xstr_list[182].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[183].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_184 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[183].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_184 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[183].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_184 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[183].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_184 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[183].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_184 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_184 = mywa2f3s.xstr_list[183].xstr_cnt;

            jsonoutputStr3S.out_distance_184 = mywa2f3s.xstr_list[183].distance;

            jsonoutputStr3S.out_gap_flag_184 = mywa2f3s.xstr_list[183].gap_flag;

            jsonoutputStr3S.out_node_num_184 = mywa2f3s.xstr_list[183].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[184].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_185 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[184].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_185 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[184].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_185 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[184].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_185 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[184].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_185 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_185 = mywa2f3s.xstr_list[184].xstr_cnt;

            jsonoutputStr3S.out_distance_185 = mywa2f3s.xstr_list[184].distance;

            jsonoutputStr3S.out_gap_flag_185 = mywa2f3s.xstr_list[184].gap_flag;

            jsonoutputStr3S.out_node_num_185 = mywa2f3s.xstr_list[184].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[185].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_186 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[185].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_186 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[185].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_186 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[185].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_186 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[185].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_186 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_186 = mywa2f3s.xstr_list[185].xstr_cnt;

            jsonoutputStr3S.out_distance_186 = mywa2f3s.xstr_list[185].distance;

            jsonoutputStr3S.out_gap_flag_186 = mywa2f3s.xstr_list[185].gap_flag;

            jsonoutputStr3S.out_node_num_186 = mywa2f3s.xstr_list[185].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[186].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_187 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[186].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_187 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[186].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_187 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[186].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_187 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[186].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_187 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_187 = mywa2f3s.xstr_list[186].xstr_cnt;

            jsonoutputStr3S.out_distance_187 = mywa2f3s.xstr_list[186].distance;

            jsonoutputStr3S.out_gap_flag_187 = mywa2f3s.xstr_list[186].gap_flag;

            jsonoutputStr3S.out_node_num_187 = mywa2f3s.xstr_list[186].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[187].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_188 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[187].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_188 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[187].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_188 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[187].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_188 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[187].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_188 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_188 = mywa2f3s.xstr_list[187].xstr_cnt;

            jsonoutputStr3S.out_distance_188 = mywa2f3s.xstr_list[187].distance;

            jsonoutputStr3S.out_gap_flag_188 = mywa2f3s.xstr_list[187].gap_flag;

            jsonoutputStr3S.out_node_num_188 = mywa2f3s.xstr_list[187].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[188].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_189 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[188].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_189 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[188].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_189 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[188].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_189 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[188].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_189 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_189 = mywa2f3s.xstr_list[188].xstr_cnt;

            jsonoutputStr3S.out_distance_189 = mywa2f3s.xstr_list[188].distance;

            jsonoutputStr3S.out_gap_flag_189 = mywa2f3s.xstr_list[188].gap_flag;

            jsonoutputStr3S.out_node_num_189 = mywa2f3s.xstr_list[188].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[189].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_190 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[189].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_190 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[189].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_190 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[189].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_190 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[189].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_190 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_190 = mywa2f3s.xstr_list[189].xstr_cnt;

            jsonoutputStr3S.out_distance_190 = mywa2f3s.xstr_list[189].distance;

            jsonoutputStr3S.out_gap_flag_190 = mywa2f3s.xstr_list[189].gap_flag;

            jsonoutputStr3S.out_node_num_190 = mywa2f3s.xstr_list[189].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[190].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_191 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[190].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_191 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[190].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_191 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[190].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_191 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[190].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_191 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_191 = mywa2f3s.xstr_list[190].xstr_cnt;

            jsonoutputStr3S.out_distance_191 = mywa2f3s.xstr_list[190].distance;

            jsonoutputStr3S.out_gap_flag_191 = mywa2f3s.xstr_list[190].gap_flag;

            jsonoutputStr3S.out_node_num_191 = mywa2f3s.xstr_list[190].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[191].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_192 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[191].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_192 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[191].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_192 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[191].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_192 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[191].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_192 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_192 = mywa2f3s.xstr_list[191].xstr_cnt;

            jsonoutputStr3S.out_distance_192 = mywa2f3s.xstr_list[191].distance;

            jsonoutputStr3S.out_gap_flag_192 = mywa2f3s.xstr_list[191].gap_flag;

            jsonoutputStr3S.out_node_num_192 = mywa2f3s.xstr_list[191].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[192].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_193 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[192].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_193 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[192].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_193 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[192].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_193 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[192].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_193 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_193 = mywa2f3s.xstr_list[192].xstr_cnt;

            jsonoutputStr3S.out_distance_193 = mywa2f3s.xstr_list[192].distance;

            jsonoutputStr3S.out_gap_flag_193 = mywa2f3s.xstr_list[192].gap_flag;

            jsonoutputStr3S.out_node_num_193 = mywa2f3s.xstr_list[192].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[193].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_194 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[193].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_194 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[193].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_194 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[193].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_194 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[193].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_194 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_194 = mywa2f3s.xstr_list[193].xstr_cnt;

            jsonoutputStr3S.out_distance_194 = mywa2f3s.xstr_list[193].distance;

            jsonoutputStr3S.out_gap_flag_194 = mywa2f3s.xstr_list[193].gap_flag;

            jsonoutputStr3S.out_node_num_194 = mywa2f3s.xstr_list[193].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[194].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_195 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[194].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_195 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[194].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_195 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[194].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_195 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[194].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_195 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_195 = mywa2f3s.xstr_list[194].xstr_cnt;

            jsonoutputStr3S.out_distance_195 = mywa2f3s.xstr_list[194].distance;

            jsonoutputStr3S.out_gap_flag_195 = mywa2f3s.xstr_list[194].gap_flag;

            jsonoutputStr3S.out_node_num_195 = mywa2f3s.xstr_list[194].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[195].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_196 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[195].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_196 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[195].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_196 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[195].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_196 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[195].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_196 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_196 = mywa2f3s.xstr_list[195].xstr_cnt;

            jsonoutputStr3S.out_distance_196 = mywa2f3s.xstr_list[195].distance;

            jsonoutputStr3S.out_gap_flag_196 = mywa2f3s.xstr_list[195].gap_flag;

            jsonoutputStr3S.out_node_num_196 = mywa2f3s.xstr_list[195].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[196].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_197 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[196].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_197 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[196].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_197 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[196].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_197 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[196].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_197 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_197 = mywa2f3s.xstr_list[196].xstr_cnt;

            jsonoutputStr3S.out_distance_197 = mywa2f3s.xstr_list[196].distance;

            jsonoutputStr3S.out_gap_flag_197 = mywa2f3s.xstr_list[196].gap_flag;

            jsonoutputStr3S.out_node_num_197 = mywa2f3s.xstr_list[196].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[197].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_198 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[197].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_198 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[197].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_198 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[197].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_198 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[197].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_198 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_198 = mywa2f3s.xstr_list[197].xstr_cnt;

            jsonoutputStr3S.out_distance_198 = mywa2f3s.xstr_list[197].distance;

            jsonoutputStr3S.out_gap_flag_198 = mywa2f3s.xstr_list[197].gap_flag;

            jsonoutputStr3S.out_node_num_198 = mywa2f3s.xstr_list[197].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[198].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_199 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[198].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_199 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[198].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_199 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[198].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_199 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[198].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_199 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_199 = mywa2f3s.xstr_list[198].xstr_cnt;

            jsonoutputStr3S.out_distance_199 = mywa2f3s.xstr_list[198].distance;

            jsonoutputStr3S.out_gap_flag_199 = mywa2f3s.xstr_list[198].gap_flag;

            jsonoutputStr3S.out_node_num_199 = mywa2f3s.xstr_list[198].node_num;

            //more to delete for 200 +

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[199].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_200 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[199].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_200 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[199].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_200 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[199].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_200 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[199].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_200 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_200 = mywa2f3s.xstr_list[199].xstr_cnt;

            jsonoutputStr3S.out_distance_200 = mywa2f3s.xstr_list[199].distance;

            jsonoutputStr3S.out_gap_flag_200 = mywa2f3s.xstr_list[199].gap_flag;

            jsonoutputStr3S.out_node_num_200 = mywa2f3s.xstr_list[199].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[200].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_201 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[200].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_201 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[200].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_201 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[200].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_201 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[200].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_201 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_201 = mywa2f3s.xstr_list[200].xstr_cnt;

            jsonoutputStr3S.out_distance_201 = mywa2f3s.xstr_list[200].distance;

            jsonoutputStr3S.out_gap_flag_201 = mywa2f3s.xstr_list[200].gap_flag;

            jsonoutputStr3S.out_node_num_201 = mywa2f3s.xstr_list[200].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[201].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_202 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[201].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_202 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[201].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_202 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[201].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_202 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[201].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_202 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_202 = mywa2f3s.xstr_list[201].xstr_cnt;

            jsonoutputStr3S.out_distance_202 = mywa2f3s.xstr_list[201].distance;

            jsonoutputStr3S.out_gap_flag_202 = mywa2f3s.xstr_list[201].gap_flag;

            jsonoutputStr3S.out_node_num_202 = mywa2f3s.xstr_list[201].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[202].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_203 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[202].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_203 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[202].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_203 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[202].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_203 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[202].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_203 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_203 = mywa2f3s.xstr_list[202].xstr_cnt;

            jsonoutputStr3S.out_distance_203 = mywa2f3s.xstr_list[202].distance;

            jsonoutputStr3S.out_gap_flag_203 = mywa2f3s.xstr_list[202].gap_flag;

            jsonoutputStr3S.out_node_num_203 = mywa2f3s.xstr_list[202].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[203].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_204 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[203].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_204 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[203].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_204 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[203].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_204 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[203].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_204 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_204 = mywa2f3s.xstr_list[203].xstr_cnt;

            jsonoutputStr3S.out_distance_204 = mywa2f3s.xstr_list[203].distance;

            jsonoutputStr3S.out_gap_flag_204 = mywa2f3s.xstr_list[203].gap_flag;

            jsonoutputStr3S.out_node_num_204 = mywa2f3s.xstr_list[203].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[204].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_205 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[204].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_205 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[204].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_205 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[204].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_205 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[204].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_205 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_205 = mywa2f3s.xstr_list[204].xstr_cnt;

            jsonoutputStr3S.out_distance_205 = mywa2f3s.xstr_list[204].distance;

            jsonoutputStr3S.out_gap_flag_205 = mywa2f3s.xstr_list[204].gap_flag;

            jsonoutputStr3S.out_node_num_205 = mywa2f3s.xstr_list[204].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[205].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_206 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[205].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_206 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[205].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_206 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[205].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_206 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[205].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_206 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_206 = mywa2f3s.xstr_list[205].xstr_cnt;

            jsonoutputStr3S.out_distance_206 = mywa2f3s.xstr_list[205].distance;

            jsonoutputStr3S.out_gap_flag_206 = mywa2f3s.xstr_list[205].gap_flag;

            jsonoutputStr3S.out_node_num_206 = mywa2f3s.xstr_list[205].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[206].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_207 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[206].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_207 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[206].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_207 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[206].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_207 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[206].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_207 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_207 = mywa2f3s.xstr_list[206].xstr_cnt;

            jsonoutputStr3S.out_distance_207 = mywa2f3s.xstr_list[206].distance;

            jsonoutputStr3S.out_gap_flag_207 = mywa2f3s.xstr_list[206].gap_flag;

            jsonoutputStr3S.out_node_num_207 = mywa2f3s.xstr_list[206].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[207].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_208 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[207].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_208 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[207].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_208 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[207].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_208 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[207].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_208 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_208 = mywa2f3s.xstr_list[207].xstr_cnt;

            jsonoutputStr3S.out_distance_208 = mywa2f3s.xstr_list[207].distance;

            jsonoutputStr3S.out_gap_flag_208 = mywa2f3s.xstr_list[207].gap_flag;

            jsonoutputStr3S.out_node_num_208 = mywa2f3s.xstr_list[207].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[208].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_209 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[208].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_209 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[208].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_209 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[208].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_209 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[208].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_209 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_209 = mywa2f3s.xstr_list[208].xstr_cnt;

            jsonoutputStr3S.out_distance_209 = mywa2f3s.xstr_list[208].distance;

            jsonoutputStr3S.out_gap_flag_209 = mywa2f3s.xstr_list[208].gap_flag;

            jsonoutputStr3S.out_node_num_209 = mywa2f3s.xstr_list[208].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[209].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_210 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[209].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_210 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[209].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_210 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[209].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_210 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[209].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_210 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_210 = mywa2f3s.xstr_list[209].xstr_cnt;

            jsonoutputStr3S.out_distance_210 = mywa2f3s.xstr_list[209].distance;

            jsonoutputStr3S.out_gap_flag_210 = mywa2f3s.xstr_list[209].gap_flag;

            jsonoutputStr3S.out_node_num_210 = mywa2f3s.xstr_list[209].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[210].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_211 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[210].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_211 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[210].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_211 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[210].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_211 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[210].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_211 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_211 = mywa2f3s.xstr_list[210].xstr_cnt;

            jsonoutputStr3S.out_distance_211 = mywa2f3s.xstr_list[210].distance;

            jsonoutputStr3S.out_gap_flag_211 = mywa2f3s.xstr_list[210].gap_flag;

            jsonoutputStr3S.out_node_num_211 = mywa2f3s.xstr_list[210].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[211].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_212 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[211].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_212 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[211].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_212 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[211].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_212 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[211].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_212 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_212 = mywa2f3s.xstr_list[211].xstr_cnt;

            jsonoutputStr3S.out_distance_212 = mywa2f3s.xstr_list[211].distance;

            jsonoutputStr3S.out_gap_flag_212 = mywa2f3s.xstr_list[211].gap_flag;

            jsonoutputStr3S.out_node_num_212 = mywa2f3s.xstr_list[211].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[212].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_213 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[212].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_213 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[212].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_213 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[212].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_213 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[212].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_213 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_213 = mywa2f3s.xstr_list[212].xstr_cnt;

            jsonoutputStr3S.out_distance_213 = mywa2f3s.xstr_list[212].distance;

            jsonoutputStr3S.out_gap_flag_213 = mywa2f3s.xstr_list[212].gap_flag;

            jsonoutputStr3S.out_node_num_213 = mywa2f3s.xstr_list[212].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[213].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_214 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[213].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_214 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[213].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_214 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[213].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_214 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[213].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_214 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_214 = mywa2f3s.xstr_list[213].xstr_cnt;

            jsonoutputStr3S.out_distance_214 = mywa2f3s.xstr_list[213].distance;

            jsonoutputStr3S.out_gap_flag_214 = mywa2f3s.xstr_list[213].gap_flag;

            jsonoutputStr3S.out_node_num_214 = mywa2f3s.xstr_list[213].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[214].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_215 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[214].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_215 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[214].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_215 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[214].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_215 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[214].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_215 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_215 = mywa2f3s.xstr_list[214].xstr_cnt;

            jsonoutputStr3S.out_distance_215 = mywa2f3s.xstr_list[214].distance;

            jsonoutputStr3S.out_gap_flag_215 = mywa2f3s.xstr_list[214].gap_flag;

            jsonoutputStr3S.out_node_num_215 = mywa2f3s.xstr_list[214].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[215].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_216 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[215].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_216 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[215].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_216 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[215].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_216 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[215].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_216 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_216 = mywa2f3s.xstr_list[215].xstr_cnt;

            jsonoutputStr3S.out_distance_216 = mywa2f3s.xstr_list[215].distance;

            jsonoutputStr3S.out_gap_flag_216 = mywa2f3s.xstr_list[215].gap_flag;

            jsonoutputStr3S.out_node_num_216 = mywa2f3s.xstr_list[215].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[216].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_217 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[216].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_217 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[216].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_217 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[216].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_217 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[216].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_217 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_217 = mywa2f3s.xstr_list[216].xstr_cnt;

            jsonoutputStr3S.out_distance_217 = mywa2f3s.xstr_list[216].distance;

            jsonoutputStr3S.out_gap_flag_217 = mywa2f3s.xstr_list[216].gap_flag;

            jsonoutputStr3S.out_node_num_217 = mywa2f3s.xstr_list[216].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[217].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_218 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[217].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_218 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[217].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_218 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[217].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_218 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[217].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_218 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_218 = mywa2f3s.xstr_list[217].xstr_cnt;

            jsonoutputStr3S.out_distance_218 = mywa2f3s.xstr_list[217].distance;

            jsonoutputStr3S.out_gap_flag_218 = mywa2f3s.xstr_list[217].gap_flag;

            jsonoutputStr3S.out_node_num_218 = mywa2f3s.xstr_list[217].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[218].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_219 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[218].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_219 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[218].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_219 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[218].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_219 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[218].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_219 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_219 = mywa2f3s.xstr_list[218].xstr_cnt;

            jsonoutputStr3S.out_distance_219 = mywa2f3s.xstr_list[218].distance;

            jsonoutputStr3S.out_gap_flag_219 = mywa2f3s.xstr_list[218].gap_flag;

            jsonoutputStr3S.out_node_num_219 = mywa2f3s.xstr_list[218].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[219].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_220 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[219].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_220 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[219].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_220 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[219].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_220 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[219].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_220 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_220 = mywa2f3s.xstr_list[219].xstr_cnt;

            jsonoutputStr3S.out_distance_220 = mywa2f3s.xstr_list[219].distance;

            jsonoutputStr3S.out_gap_flag_220 = mywa2f3s.xstr_list[219].gap_flag;

            jsonoutputStr3S.out_node_num_220 = mywa2f3s.xstr_list[219].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[220].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_221 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[220].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_221 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[220].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_221 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[220].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_221 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[220].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_221 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_221 = mywa2f3s.xstr_list[220].xstr_cnt;

            jsonoutputStr3S.out_distance_221 = mywa2f3s.xstr_list[220].distance;

            jsonoutputStr3S.out_gap_flag_221 = mywa2f3s.xstr_list[220].gap_flag;

            jsonoutputStr3S.out_node_num_221 = mywa2f3s.xstr_list[220].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[221].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_222 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[221].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_222 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[221].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_222 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[221].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_222 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[221].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_222 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_222 = mywa2f3s.xstr_list[221].xstr_cnt;

            jsonoutputStr3S.out_distance_222 = mywa2f3s.xstr_list[221].distance;

            jsonoutputStr3S.out_gap_flag_222 = mywa2f3s.xstr_list[221].gap_flag;

            jsonoutputStr3S.out_node_num_222 = mywa2f3s.xstr_list[221].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[222].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_223 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[222].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_223 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[222].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_223 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[222].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_223 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[222].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_223 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_223 = mywa2f3s.xstr_list[222].xstr_cnt;

            jsonoutputStr3S.out_distance_223 = mywa2f3s.xstr_list[222].distance;

            jsonoutputStr3S.out_gap_flag_223 = mywa2f3s.xstr_list[222].gap_flag;

            jsonoutputStr3S.out_node_num_223 = mywa2f3s.xstr_list[222].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[223].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_224 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[223].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_224 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[223].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_224 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[223].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_224 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[223].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_224 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_224 = mywa2f3s.xstr_list[223].xstr_cnt;

            jsonoutputStr3S.out_distance_224 = mywa2f3s.xstr_list[223].distance;

            jsonoutputStr3S.out_gap_flag_224 = mywa2f3s.xstr_list[223].gap_flag;

            jsonoutputStr3S.out_node_num_224 = mywa2f3s.xstr_list[223].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[224].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_225 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[224].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_225 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[224].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_225 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[224].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_225 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[224].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_225 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_225 = mywa2f3s.xstr_list[224].xstr_cnt;

            jsonoutputStr3S.out_distance_225 = mywa2f3s.xstr_list[224].distance;

            jsonoutputStr3S.out_gap_flag_225 = mywa2f3s.xstr_list[224].gap_flag;

            jsonoutputStr3S.out_node_num_225 = mywa2f3s.xstr_list[224].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[225].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_226 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[225].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_226 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[225].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_226 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[225].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_226 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[225].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_226 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_226 = mywa2f3s.xstr_list[225].xstr_cnt;

            jsonoutputStr3S.out_distance_226 = mywa2f3s.xstr_list[225].distance;

            jsonoutputStr3S.out_gap_flag_226 = mywa2f3s.xstr_list[225].gap_flag;

            jsonoutputStr3S.out_node_num_226 = mywa2f3s.xstr_list[225].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[226].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_227 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[226].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_227 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[226].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_227 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[226].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_227 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[226].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_227 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_227 = mywa2f3s.xstr_list[226].xstr_cnt;

            jsonoutputStr3S.out_distance_227 = mywa2f3s.xstr_list[226].distance;

            jsonoutputStr3S.out_gap_flag_227 = mywa2f3s.xstr_list[226].gap_flag;

            jsonoutputStr3S.out_node_num_227 = mywa2f3s.xstr_list[226].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[227].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_228 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[227].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_228 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[227].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_228 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[227].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_228 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[227].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_228 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_228 = mywa2f3s.xstr_list[227].xstr_cnt;

            jsonoutputStr3S.out_distance_228 = mywa2f3s.xstr_list[227].distance;

            jsonoutputStr3S.out_gap_flag_228 = mywa2f3s.xstr_list[227].gap_flag;

            jsonoutputStr3S.out_node_num_228 = mywa2f3s.xstr_list[227].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[228].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_229 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[228].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_229 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[228].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_229 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[228].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_229 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[228].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_229 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_229 = mywa2f3s.xstr_list[228].xstr_cnt;

            jsonoutputStr3S.out_distance_229 = mywa2f3s.xstr_list[228].distance;

            jsonoutputStr3S.out_gap_flag_229 = mywa2f3s.xstr_list[228].gap_flag;

            jsonoutputStr3S.out_node_num_229 = mywa2f3s.xstr_list[228].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[229].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_230 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[229].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_230 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[229].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_230 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[229].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_230 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[229].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_230 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_230 = mywa2f3s.xstr_list[229].xstr_cnt;

            jsonoutputStr3S.out_distance_230 = mywa2f3s.xstr_list[229].distance;

            jsonoutputStr3S.out_gap_flag_230 = mywa2f3s.xstr_list[229].gap_flag;

            jsonoutputStr3S.out_node_num_230 = mywa2f3s.xstr_list[229].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[230].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_231 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[230].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_231 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[230].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_231 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[230].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_231 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[230].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_231 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_231 = mywa2f3s.xstr_list[230].xstr_cnt;

            jsonoutputStr3S.out_distance_231 = mywa2f3s.xstr_list[230].distance;

            jsonoutputStr3S.out_gap_flag_231 = mywa2f3s.xstr_list[230].gap_flag;

            jsonoutputStr3S.out_node_num_231 = mywa2f3s.xstr_list[230].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[231].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_232 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[231].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_232 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[231].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_232 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[231].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_232 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[231].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_232 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_232 = mywa2f3s.xstr_list[231].xstr_cnt;

            jsonoutputStr3S.out_distance_232 = mywa2f3s.xstr_list[231].distance;

            jsonoutputStr3S.out_gap_flag_232 = mywa2f3s.xstr_list[231].gap_flag;

            jsonoutputStr3S.out_node_num_232 = mywa2f3s.xstr_list[231].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[232].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_233 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[232].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_233 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[232].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_233 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[232].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_233 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[232].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_233 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_233 = mywa2f3s.xstr_list[232].xstr_cnt;

            jsonoutputStr3S.out_distance_233 = mywa2f3s.xstr_list[232].distance;

            jsonoutputStr3S.out_gap_flag_233 = mywa2f3s.xstr_list[232].gap_flag;

            jsonoutputStr3S.out_node_num_233 = mywa2f3s.xstr_list[232].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[233].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_234 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[233].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_234 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[233].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_234 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[233].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_234 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[233].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_234 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_234 = mywa2f3s.xstr_list[233].xstr_cnt;

            jsonoutputStr3S.out_distance_234 = mywa2f3s.xstr_list[233].distance;

            jsonoutputStr3S.out_gap_flag_234 = mywa2f3s.xstr_list[233].gap_flag;

            jsonoutputStr3S.out_node_num_234 = mywa2f3s.xstr_list[233].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[234].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_235 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[234].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_235 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[234].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_235 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[234].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_235 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[234].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_235 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_235 = mywa2f3s.xstr_list[234].xstr_cnt;

            jsonoutputStr3S.out_distance_235 = mywa2f3s.xstr_list[234].distance;

            jsonoutputStr3S.out_gap_flag_235 = mywa2f3s.xstr_list[234].gap_flag;

            jsonoutputStr3S.out_node_num_235 = mywa2f3s.xstr_list[234].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[235].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_236 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[235].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_236 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[235].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_236 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[235].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_236 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[235].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_236 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_236 = mywa2f3s.xstr_list[235].xstr_cnt;

            jsonoutputStr3S.out_distance_236 = mywa2f3s.xstr_list[235].distance;

            jsonoutputStr3S.out_gap_flag_236 = mywa2f3s.xstr_list[235].gap_flag;

            jsonoutputStr3S.out_node_num_236 = mywa2f3s.xstr_list[235].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[236].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_237 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[236].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_237 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[236].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_237 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[236].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_237 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[236].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_237 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_237 = mywa2f3s.xstr_list[236].xstr_cnt;

            jsonoutputStr3S.out_distance_237 = mywa2f3s.xstr_list[236].distance;

            jsonoutputStr3S.out_gap_flag_237 = mywa2f3s.xstr_list[236].gap_flag;

            jsonoutputStr3S.out_node_num_237 = mywa2f3s.xstr_list[236].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[237].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_238 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[237].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_238 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[237].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_238 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[237].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_238 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[237].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_238 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_238 = mywa2f3s.xstr_list[237].xstr_cnt;

            jsonoutputStr3S.out_distance_238 = mywa2f3s.xstr_list[237].distance;

            jsonoutputStr3S.out_gap_flag_238 = mywa2f3s.xstr_list[237].gap_flag;

            jsonoutputStr3S.out_node_num_238 = mywa2f3s.xstr_list[237].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[238].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_239 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[238].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_239 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[238].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_239 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[238].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_239 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[238].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_239 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_239 = mywa2f3s.xstr_list[238].xstr_cnt;

            jsonoutputStr3S.out_distance_239 = mywa2f3s.xstr_list[238].distance;

            jsonoutputStr3S.out_gap_flag_239 = mywa2f3s.xstr_list[238].gap_flag;

            jsonoutputStr3S.out_node_num_239 = mywa2f3s.xstr_list[238].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[239].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_240 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[239].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_240 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[239].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_240 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[239].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_240 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[239].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_240 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_240 = mywa2f3s.xstr_list[239].xstr_cnt;

            jsonoutputStr3S.out_distance_240 = mywa2f3s.xstr_list[239].distance;

            jsonoutputStr3S.out_gap_flag_240 = mywa2f3s.xstr_list[239].gap_flag;

            jsonoutputStr3S.out_node_num_240 = mywa2f3s.xstr_list[239].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[240].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_241 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[240].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_241 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[240].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_241 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[240].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_241 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[240].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_241 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_241 = mywa2f3s.xstr_list[240].xstr_cnt;

            jsonoutputStr3S.out_distance_241 = mywa2f3s.xstr_list[240].distance;

            jsonoutputStr3S.out_gap_flag_241 = mywa2f3s.xstr_list[240].gap_flag;

            jsonoutputStr3S.out_node_num_241 = mywa2f3s.xstr_list[240].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[241].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_242 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[241].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_242 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[241].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_242 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[241].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_242 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[241].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_242 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_242 = mywa2f3s.xstr_list[241].xstr_cnt;

            jsonoutputStr3S.out_distance_242 = mywa2f3s.xstr_list[241].distance;

            jsonoutputStr3S.out_gap_flag_242 = mywa2f3s.xstr_list[241].gap_flag;

            jsonoutputStr3S.out_node_num_242 = mywa2f3s.xstr_list[241].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[242].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_243 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[242].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_243 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[242].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_243 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[242].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_243 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[242].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_243 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_243 = mywa2f3s.xstr_list[242].xstr_cnt;

            jsonoutputStr3S.out_distance_243 = mywa2f3s.xstr_list[242].distance;

            jsonoutputStr3S.out_gap_flag_243 = mywa2f3s.xstr_list[242].gap_flag;

            jsonoutputStr3S.out_node_num_243 = mywa2f3s.xstr_list[242].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[243].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_244 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[243].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_244 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[243].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_244 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[243].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_244 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[243].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_244 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_244 = mywa2f3s.xstr_list[243].xstr_cnt;

            jsonoutputStr3S.out_distance_244 = mywa2f3s.xstr_list[243].distance;

            jsonoutputStr3S.out_gap_flag_244 = mywa2f3s.xstr_list[243].gap_flag;

            jsonoutputStr3S.out_node_num_244 = mywa2f3s.xstr_list[243].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[244].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_245 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[244].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_245 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[244].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_245 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[244].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_245 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[244].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_245 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_245 = mywa2f3s.xstr_list[244].xstr_cnt;

            jsonoutputStr3S.out_distance_245 = mywa2f3s.xstr_list[244].distance;

            jsonoutputStr3S.out_gap_flag_245 = mywa2f3s.xstr_list[244].gap_flag;

            jsonoutputStr3S.out_node_num_245 = mywa2f3s.xstr_list[244].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[245].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_246 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[245].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_246 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[245].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_246 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[245].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_246 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[245].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_246 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_246 = mywa2f3s.xstr_list[245].xstr_cnt;

            jsonoutputStr3S.out_distance_246 = mywa2f3s.xstr_list[245].distance;

            jsonoutputStr3S.out_gap_flag_246 = mywa2f3s.xstr_list[245].gap_flag;

            jsonoutputStr3S.out_node_num_246 = mywa2f3s.xstr_list[245].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[246].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_247 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[246].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_247 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[246].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_247 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[246].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_247 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[246].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_247 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_247 = mywa2f3s.xstr_list[246].xstr_cnt;

            jsonoutputStr3S.out_distance_247 = mywa2f3s.xstr_list[246].distance;

            jsonoutputStr3S.out_gap_flag_247 = mywa2f3s.xstr_list[246].gap_flag;

            jsonoutputStr3S.out_node_num_247 = mywa2f3s.xstr_list[246].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[247].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_248 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[247].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_248 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[247].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_248 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[247].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_248 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[247].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_248 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_248 = mywa2f3s.xstr_list[247].xstr_cnt;

            jsonoutputStr3S.out_distance_248 = mywa2f3s.xstr_list[247].distance;

            jsonoutputStr3S.out_gap_flag_248 = mywa2f3s.xstr_list[247].gap_flag;

            jsonoutputStr3S.out_node_num_248 = mywa2f3s.xstr_list[247].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[248].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_249 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[248].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_249 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[248].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_249 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[248].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_249 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[248].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_249 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_249 = mywa2f3s.xstr_list[248].xstr_cnt;

            jsonoutputStr3S.out_distance_249 = mywa2f3s.xstr_list[248].distance;

            jsonoutputStr3S.out_gap_flag_249 = mywa2f3s.xstr_list[248].gap_flag;

            jsonoutputStr3S.out_node_num_249 = mywa2f3s.xstr_list[248].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[249].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_250 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[249].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_250 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[249].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_250 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[249].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_250 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[249].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_250 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_250 = mywa2f3s.xstr_list[249].xstr_cnt;

            jsonoutputStr3S.out_distance_250 = mywa2f3s.xstr_list[249].distance;

            jsonoutputStr3S.out_gap_flag_250 = mywa2f3s.xstr_list[249].gap_flag;

            jsonoutputStr3S.out_node_num_250 = mywa2f3s.xstr_list[249].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[250].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_251 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[250].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_251 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[250].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_251 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[250].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_251 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[250].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_251 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_251 = mywa2f3s.xstr_list[250].xstr_cnt;

            jsonoutputStr3S.out_distance_251 = mywa2f3s.xstr_list[250].distance;

            jsonoutputStr3S.out_gap_flag_251 = mywa2f3s.xstr_list[250].gap_flag;

            jsonoutputStr3S.out_node_num_251 = mywa2f3s.xstr_list[250].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[251].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_252 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[251].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_252 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[251].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_252 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[251].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_252 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[251].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_252 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_252 = mywa2f3s.xstr_list[251].xstr_cnt;

            jsonoutputStr3S.out_distance_252 = mywa2f3s.xstr_list[251].distance;

            jsonoutputStr3S.out_gap_flag_252 = mywa2f3s.xstr_list[251].gap_flag;

            jsonoutputStr3S.out_node_num_252 = mywa2f3s.xstr_list[251].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[252].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_253 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[252].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_253 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[252].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_253 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[252].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_253 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[252].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_253 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_253 = mywa2f3s.xstr_list[252].xstr_cnt;

            jsonoutputStr3S.out_distance_253 = mywa2f3s.xstr_list[252].distance;

            jsonoutputStr3S.out_gap_flag_253 = mywa2f3s.xstr_list[252].gap_flag;

            jsonoutputStr3S.out_node_num_253 = mywa2f3s.xstr_list[252].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[253].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_254 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[253].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_254 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[253].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_254 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[253].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_254 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[253].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_254 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_254 = mywa2f3s.xstr_list[253].xstr_cnt;

            jsonoutputStr3S.out_distance_254 = mywa2f3s.xstr_list[253].distance;

            jsonoutputStr3S.out_gap_flag_254 = mywa2f3s.xstr_list[253].gap_flag;

            jsonoutputStr3S.out_node_num_254 = mywa2f3s.xstr_list[253].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[254].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_255 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[254].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_255 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[254].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_255 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[254].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_255 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[254].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_255 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_255 = mywa2f3s.xstr_list[254].xstr_cnt;

            jsonoutputStr3S.out_distance_255 = mywa2f3s.xstr_list[254].distance;

            jsonoutputStr3S.out_gap_flag_255 = mywa2f3s.xstr_list[254].gap_flag;

            jsonoutputStr3S.out_node_num_255 = mywa2f3s.xstr_list[254].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[255].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_256 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[255].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_256 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[255].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_256 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[255].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_256 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[255].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_256 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_256 = mywa2f3s.xstr_list[255].xstr_cnt;

            jsonoutputStr3S.out_distance_256 = mywa2f3s.xstr_list[255].distance;

            jsonoutputStr3S.out_gap_flag_256 = mywa2f3s.xstr_list[255].gap_flag;

            jsonoutputStr3S.out_node_num_256 = mywa2f3s.xstr_list[255].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[256].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_257 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[256].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_257 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[256].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_257 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[256].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_257 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[256].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_257 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_257 = mywa2f3s.xstr_list[256].xstr_cnt;

            jsonoutputStr3S.out_distance_257 = mywa2f3s.xstr_list[256].distance;

            jsonoutputStr3S.out_gap_flag_257 = mywa2f3s.xstr_list[256].gap_flag;

            jsonoutputStr3S.out_node_num_257 = mywa2f3s.xstr_list[256].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[257].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_258 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[257].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_258 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[257].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_258 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[257].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_258 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[257].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_258 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_258 = mywa2f3s.xstr_list[257].xstr_cnt;

            jsonoutputStr3S.out_distance_258 = mywa2f3s.xstr_list[257].distance;

            jsonoutputStr3S.out_gap_flag_258 = mywa2f3s.xstr_list[257].gap_flag;

            jsonoutputStr3S.out_node_num_258 = mywa2f3s.xstr_list[257].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[258].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_259 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[258].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_259 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[258].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_259 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[258].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_259 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[258].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_259 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_259 = mywa2f3s.xstr_list[258].xstr_cnt;

            jsonoutputStr3S.out_distance_259 = mywa2f3s.xstr_list[258].distance;

            jsonoutputStr3S.out_gap_flag_259 = mywa2f3s.xstr_list[258].gap_flag;

            jsonoutputStr3S.out_node_num_259 = mywa2f3s.xstr_list[258].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[259].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_260 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[259].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_260 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[259].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_260 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[259].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_260 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[259].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_260 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_260 = mywa2f3s.xstr_list[259].xstr_cnt;

            jsonoutputStr3S.out_distance_260 = mywa2f3s.xstr_list[259].distance;

            jsonoutputStr3S.out_gap_flag_260 = mywa2f3s.xstr_list[259].gap_flag;

            jsonoutputStr3S.out_node_num_260 = mywa2f3s.xstr_list[259].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[260].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_261 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[260].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_261 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[260].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_261 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[260].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_261 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[260].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_261 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_261 = mywa2f3s.xstr_list[260].xstr_cnt;

            jsonoutputStr3S.out_distance_261 = mywa2f3s.xstr_list[260].distance;

            jsonoutputStr3S.out_gap_flag_261 = mywa2f3s.xstr_list[260].gap_flag;

            jsonoutputStr3S.out_node_num_261 = mywa2f3s.xstr_list[260].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[261].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_262 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[261].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_262 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[261].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_262 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[261].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_262 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[261].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_262 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_262 = mywa2f3s.xstr_list[261].xstr_cnt;

            jsonoutputStr3S.out_distance_262 = mywa2f3s.xstr_list[261].distance;

            jsonoutputStr3S.out_gap_flag_262 = mywa2f3s.xstr_list[261].gap_flag;

            jsonoutputStr3S.out_node_num_262 = mywa2f3s.xstr_list[261].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[262].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_263 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[262].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_263 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[262].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_263 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[262].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_263 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[262].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_263 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_263 = mywa2f3s.xstr_list[262].xstr_cnt;

            jsonoutputStr3S.out_distance_263 = mywa2f3s.xstr_list[262].distance;

            jsonoutputStr3S.out_gap_flag_263 = mywa2f3s.xstr_list[262].gap_flag;

            jsonoutputStr3S.out_node_num_263 = mywa2f3s.xstr_list[262].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[263].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_264 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[263].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_264 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[263].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_264 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[263].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_264 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[263].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_264 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_264 = mywa2f3s.xstr_list[263].xstr_cnt;

            jsonoutputStr3S.out_distance_264 = mywa2f3s.xstr_list[263].distance;

            jsonoutputStr3S.out_gap_flag_264 = mywa2f3s.xstr_list[263].gap_flag;

            jsonoutputStr3S.out_node_num_264 = mywa2f3s.xstr_list[263].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[264].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_265 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[264].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_265 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[264].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_265 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[264].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_265 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[264].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_265 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_265 = mywa2f3s.xstr_list[264].xstr_cnt;

            jsonoutputStr3S.out_distance_265 = mywa2f3s.xstr_list[264].distance;

            jsonoutputStr3S.out_gap_flag_265 = mywa2f3s.xstr_list[264].gap_flag;

            jsonoutputStr3S.out_node_num_265 = mywa2f3s.xstr_list[264].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[265].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_266 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[265].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_266 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[265].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_266 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[265].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_266 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[265].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_266 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_266 = mywa2f3s.xstr_list[265].xstr_cnt;

            jsonoutputStr3S.out_distance_266 = mywa2f3s.xstr_list[265].distance;

            jsonoutputStr3S.out_gap_flag_266 = mywa2f3s.xstr_list[265].gap_flag;

            jsonoutputStr3S.out_node_num_266 = mywa2f3s.xstr_list[265].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[266].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_267 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[266].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_267 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[266].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_267 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[266].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_267 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[266].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_267 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_267 = mywa2f3s.xstr_list[266].xstr_cnt;

            jsonoutputStr3S.out_distance_267 = mywa2f3s.xstr_list[266].distance;

            jsonoutputStr3S.out_gap_flag_267 = mywa2f3s.xstr_list[266].gap_flag;

            jsonoutputStr3S.out_node_num_267 = mywa2f3s.xstr_list[266].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[267].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_268 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[267].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_268 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[267].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_268 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[267].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_268 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[267].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_268 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_268 = mywa2f3s.xstr_list[267].xstr_cnt;

            jsonoutputStr3S.out_distance_268 = mywa2f3s.xstr_list[267].distance;

            jsonoutputStr3S.out_gap_flag_268 = mywa2f3s.xstr_list[267].gap_flag;

            jsonoutputStr3S.out_node_num_268 = mywa2f3s.xstr_list[267].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[268].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_269 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[268].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_269 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[268].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_269 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[268].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_269 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[268].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_269 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_269 = mywa2f3s.xstr_list[268].xstr_cnt;

            jsonoutputStr3S.out_distance_269 = mywa2f3s.xstr_list[268].distance;

            jsonoutputStr3S.out_gap_flag_269 = mywa2f3s.xstr_list[268].gap_flag;

            jsonoutputStr3S.out_node_num_269 = mywa2f3s.xstr_list[268].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[269].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_270 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[269].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_270 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[269].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_270 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[269].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_270 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[269].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_270 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_270 = mywa2f3s.xstr_list[269].xstr_cnt;

            jsonoutputStr3S.out_distance_270 = mywa2f3s.xstr_list[269].distance;

            jsonoutputStr3S.out_gap_flag_270 = mywa2f3s.xstr_list[269].gap_flag;

            jsonoutputStr3S.out_node_num_270 = mywa2f3s.xstr_list[269].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[270].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_271 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[270].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_271 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[270].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_271 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[270].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_271 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[270].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_271 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_271 = mywa2f3s.xstr_list[270].xstr_cnt;

            jsonoutputStr3S.out_distance_271 = mywa2f3s.xstr_list[270].distance;

            jsonoutputStr3S.out_gap_flag_271 = mywa2f3s.xstr_list[270].gap_flag;

            jsonoutputStr3S.out_node_num_271 = mywa2f3s.xstr_list[270].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[271].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_272 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[271].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_272 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[271].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_272 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[271].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_272 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[271].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_272 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_272 = mywa2f3s.xstr_list[271].xstr_cnt;

            jsonoutputStr3S.out_distance_272 = mywa2f3s.xstr_list[271].distance;

            jsonoutputStr3S.out_gap_flag_272 = mywa2f3s.xstr_list[271].gap_flag;

            jsonoutputStr3S.out_node_num_272 = mywa2f3s.xstr_list[271].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[272].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_273 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[272].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_273 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[272].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_273 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[272].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_273 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[272].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_273 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_273 = mywa2f3s.xstr_list[272].xstr_cnt;

            jsonoutputStr3S.out_distance_273 = mywa2f3s.xstr_list[272].distance;

            jsonoutputStr3S.out_gap_flag_273 = mywa2f3s.xstr_list[272].gap_flag;

            jsonoutputStr3S.out_node_num_273 = mywa2f3s.xstr_list[272].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[273].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_274 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[273].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_274 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[273].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_274 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[273].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_274 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[273].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_274 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_274 = mywa2f3s.xstr_list[273].xstr_cnt;

            jsonoutputStr3S.out_distance_274 = mywa2f3s.xstr_list[273].distance;

            jsonoutputStr3S.out_gap_flag_274 = mywa2f3s.xstr_list[273].gap_flag;

            jsonoutputStr3S.out_node_num_274 = mywa2f3s.xstr_list[273].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[274].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_275 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[274].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_275 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[274].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_275 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[274].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_275 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[274].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_275 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_275 = mywa2f3s.xstr_list[274].xstr_cnt;

            jsonoutputStr3S.out_distance_275 = mywa2f3s.xstr_list[274].distance;

            jsonoutputStr3S.out_gap_flag_275 = mywa2f3s.xstr_list[274].gap_flag;

            jsonoutputStr3S.out_node_num_275 = mywa2f3s.xstr_list[274].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[275].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_276 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[275].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_276 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[275].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_276 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[275].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_276 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[275].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_276 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_276 = mywa2f3s.xstr_list[275].xstr_cnt;

            jsonoutputStr3S.out_distance_276 = mywa2f3s.xstr_list[275].distance;

            jsonoutputStr3S.out_gap_flag_276 = mywa2f3s.xstr_list[275].gap_flag;

            jsonoutputStr3S.out_node_num_276 = mywa2f3s.xstr_list[275].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[276].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_277 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[276].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_277 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[276].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_277 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[276].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_277 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[276].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_277 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_277 = mywa2f3s.xstr_list[276].xstr_cnt;

            jsonoutputStr3S.out_distance_277 = mywa2f3s.xstr_list[276].distance;

            jsonoutputStr3S.out_gap_flag_277 = mywa2f3s.xstr_list[276].gap_flag;

            jsonoutputStr3S.out_node_num_277 = mywa2f3s.xstr_list[276].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[277].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_278 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[277].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_278 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[277].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_278 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[277].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_278 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[277].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_278 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_278 = mywa2f3s.xstr_list[277].xstr_cnt;

            jsonoutputStr3S.out_distance_278 = mywa2f3s.xstr_list[277].distance;

            jsonoutputStr3S.out_gap_flag_278 = mywa2f3s.xstr_list[277].gap_flag;

            jsonoutputStr3S.out_node_num_278 = mywa2f3s.xstr_list[277].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[278].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_279 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[278].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_279 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[278].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_279 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[278].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_279 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[278].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_279 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_279 = mywa2f3s.xstr_list[278].xstr_cnt;

            jsonoutputStr3S.out_distance_279 = mywa2f3s.xstr_list[278].distance;

            jsonoutputStr3S.out_gap_flag_279 = mywa2f3s.xstr_list[278].gap_flag;

            jsonoutputStr3S.out_node_num_279 = mywa2f3s.xstr_list[278].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[279].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_280 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[279].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_280 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[279].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_280 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[279].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_280 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[279].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_280 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_280 = mywa2f3s.xstr_list[279].xstr_cnt;

            jsonoutputStr3S.out_distance_280 = mywa2f3s.xstr_list[279].distance;

            jsonoutputStr3S.out_gap_flag_280 = mywa2f3s.xstr_list[279].gap_flag;

            jsonoutputStr3S.out_node_num_280 = mywa2f3s.xstr_list[279].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[280].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_281 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[280].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_281 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[280].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_281 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[280].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_281 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[280].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_281 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_281 = mywa2f3s.xstr_list[280].xstr_cnt;

            jsonoutputStr3S.out_distance_281 = mywa2f3s.xstr_list[280].distance;

            jsonoutputStr3S.out_gap_flag_281 = mywa2f3s.xstr_list[280].gap_flag;

            jsonoutputStr3S.out_node_num_281 = mywa2f3s.xstr_list[280].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[281].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_282 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[281].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_282 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[281].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_282 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[281].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_282 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[281].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_282 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_282 = mywa2f3s.xstr_list[281].xstr_cnt;

            jsonoutputStr3S.out_distance_282 = mywa2f3s.xstr_list[281].distance;

            jsonoutputStr3S.out_gap_flag_282 = mywa2f3s.xstr_list[281].gap_flag;

            jsonoutputStr3S.out_node_num_282 = mywa2f3s.xstr_list[281].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[282].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_283 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[282].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_283 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[282].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_283 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[282].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_283 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[282].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_283 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_283 = mywa2f3s.xstr_list[282].xstr_cnt;

            jsonoutputStr3S.out_distance_283 = mywa2f3s.xstr_list[282].distance;

            jsonoutputStr3S.out_gap_flag_283 = mywa2f3s.xstr_list[282].gap_flag;

            jsonoutputStr3S.out_node_num_283 = mywa2f3s.xstr_list[282].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[283].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_284 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[283].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_284 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[283].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_284 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[283].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_284 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[283].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_284 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_284 = mywa2f3s.xstr_list[283].xstr_cnt;

            jsonoutputStr3S.out_distance_284 = mywa2f3s.xstr_list[283].distance;

            jsonoutputStr3S.out_gap_flag_284 = mywa2f3s.xstr_list[283].gap_flag;

            jsonoutputStr3S.out_node_num_284 = mywa2f3s.xstr_list[283].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[284].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_285 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[284].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_285 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[284].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_285 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[284].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_285 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[284].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_285 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_285 = mywa2f3s.xstr_list[284].xstr_cnt;

            jsonoutputStr3S.out_distance_285 = mywa2f3s.xstr_list[284].distance;

            jsonoutputStr3S.out_gap_flag_285 = mywa2f3s.xstr_list[284].gap_flag;

            jsonoutputStr3S.out_node_num_285 = mywa2f3s.xstr_list[284].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[285].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_286 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[285].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_286 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[285].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_286 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[285].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_286 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[285].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_286 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_286 = mywa2f3s.xstr_list[285].xstr_cnt;

            jsonoutputStr3S.out_distance_286 = mywa2f3s.xstr_list[285].distance;

            jsonoutputStr3S.out_gap_flag_286 = mywa2f3s.xstr_list[285].gap_flag;

            jsonoutputStr3S.out_node_num_286 = mywa2f3s.xstr_list[285].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[286].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_287 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[286].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_287 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[286].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_287 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[286].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_287 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[286].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_287 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_287 = mywa2f3s.xstr_list[286].xstr_cnt;

            jsonoutputStr3S.out_distance_287 = mywa2f3s.xstr_list[286].distance;

            jsonoutputStr3S.out_gap_flag_287 = mywa2f3s.xstr_list[286].gap_flag;

            jsonoutputStr3S.out_node_num_287 = mywa2f3s.xstr_list[286].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[287].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_288 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[287].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_288 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[287].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_288 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[287].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_288 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[287].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_288 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_288 = mywa2f3s.xstr_list[287].xstr_cnt;

            jsonoutputStr3S.out_distance_288 = mywa2f3s.xstr_list[287].distance;

            jsonoutputStr3S.out_gap_flag_288 = mywa2f3s.xstr_list[287].gap_flag;

            jsonoutputStr3S.out_node_num_288 = mywa2f3s.xstr_list[287].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[288].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_289 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[288].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_289 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[288].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_289 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[288].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_289 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[288].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_289 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_289 = mywa2f3s.xstr_list[288].xstr_cnt;

            jsonoutputStr3S.out_distance_289 = mywa2f3s.xstr_list[288].distance;

            jsonoutputStr3S.out_gap_flag_289 = mywa2f3s.xstr_list[288].gap_flag;

            jsonoutputStr3S.out_node_num_289 = mywa2f3s.xstr_list[288].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[289].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_290 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[289].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_290 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[289].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_290 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[289].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_290 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[289].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_290 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_290 = mywa2f3s.xstr_list[289].xstr_cnt;

            jsonoutputStr3S.out_distance_290 = mywa2f3s.xstr_list[289].distance;

            jsonoutputStr3S.out_gap_flag_290 = mywa2f3s.xstr_list[289].gap_flag;

            jsonoutputStr3S.out_node_num_290 = mywa2f3s.xstr_list[289].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[290].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_291 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[290].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_291 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[290].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_291 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[290].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_291 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[290].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_291 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_291 = mywa2f3s.xstr_list[290].xstr_cnt;

            jsonoutputStr3S.out_distance_291 = mywa2f3s.xstr_list[290].distance;

            jsonoutputStr3S.out_gap_flag_291 = mywa2f3s.xstr_list[290].gap_flag;

            jsonoutputStr3S.out_node_num_291 = mywa2f3s.xstr_list[290].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[291].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_292 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[291].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_292 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[291].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_292 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[291].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_292 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[291].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_292 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_292 = mywa2f3s.xstr_list[291].xstr_cnt;

            jsonoutputStr3S.out_distance_292 = mywa2f3s.xstr_list[291].distance;

            jsonoutputStr3S.out_gap_flag_292 = mywa2f3s.xstr_list[291].gap_flag;

            jsonoutputStr3S.out_node_num_292 = mywa2f3s.xstr_list[291].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[292].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_293 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[292].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_293 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[292].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_293 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[292].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_293 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[292].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_293 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_293 = mywa2f3s.xstr_list[292].xstr_cnt;

            jsonoutputStr3S.out_distance_293 = mywa2f3s.xstr_list[292].distance;

            jsonoutputStr3S.out_gap_flag_293 = mywa2f3s.xstr_list[292].gap_flag;

            jsonoutputStr3S.out_node_num_293 = mywa2f3s.xstr_list[292].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[293].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_294 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[293].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_294 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[293].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_294 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[293].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_294 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[293].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_294 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_294 = mywa2f3s.xstr_list[293].xstr_cnt;

            jsonoutputStr3S.out_distance_294 = mywa2f3s.xstr_list[293].distance;

            jsonoutputStr3S.out_gap_flag_294 = mywa2f3s.xstr_list[293].gap_flag;

            jsonoutputStr3S.out_node_num_294 = mywa2f3s.xstr_list[293].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[294].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_295 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[294].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_295 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[294].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_295 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[294].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_295 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[294].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_295 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_295 = mywa2f3s.xstr_list[294].xstr_cnt;

            jsonoutputStr3S.out_distance_295 = mywa2f3s.xstr_list[294].distance;

            jsonoutputStr3S.out_gap_flag_295 = mywa2f3s.xstr_list[294].gap_flag;

            jsonoutputStr3S.out_node_num_295 = mywa2f3s.xstr_list[294].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[295].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_296 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[295].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_296 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[295].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_296 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[295].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_296 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[295].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_296 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_296 = mywa2f3s.xstr_list[295].xstr_cnt;

            jsonoutputStr3S.out_distance_296 = mywa2f3s.xstr_list[295].distance;

            jsonoutputStr3S.out_gap_flag_296 = mywa2f3s.xstr_list[295].gap_flag;

            jsonoutputStr3S.out_node_num_296 = mywa2f3s.xstr_list[295].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[296].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_297 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[296].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_297 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[296].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_297 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[296].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_297 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[296].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_297 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_297 = mywa2f3s.xstr_list[296].xstr_cnt;

            jsonoutputStr3S.out_distance_297 = mywa2f3s.xstr_list[296].distance;

            jsonoutputStr3S.out_gap_flag_297 = mywa2f3s.xstr_list[296].gap_flag;

            jsonoutputStr3S.out_node_num_297 = mywa2f3s.xstr_list[296].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[297].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_298 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[297].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_298 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[297].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_298 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[297].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_298 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[297].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_298 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_298 = mywa2f3s.xstr_list[297].xstr_cnt;

            jsonoutputStr3S.out_distance_298 = mywa2f3s.xstr_list[297].distance;

            jsonoutputStr3S.out_gap_flag_298 = mywa2f3s.xstr_list[297].gap_flag;

            jsonoutputStr3S.out_node_num_298 = mywa2f3s.xstr_list[297].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[298].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_299 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[298].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_299 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[298].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_299 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[298].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_299 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[298].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_299 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_299 = mywa2f3s.xstr_list[298].xstr_cnt;

            jsonoutputStr3S.out_distance_299 = mywa2f3s.xstr_list[298].distance;

            jsonoutputStr3S.out_gap_flag_299 = mywa2f3s.xstr_list[298].gap_flag;

            jsonoutputStr3S.out_node_num_299 = mywa2f3s.xstr_list[298].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[299].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_300 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[299].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_300 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[299].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_300 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[299].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_300 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[299].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_300 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_300 = mywa2f3s.xstr_list[299].xstr_cnt;

            jsonoutputStr3S.out_distance_300 = mywa2f3s.xstr_list[299].distance;

            jsonoutputStr3S.out_gap_flag_300 = mywa2f3s.xstr_list[299].gap_flag;

            jsonoutputStr3S.out_node_num_300 = mywa2f3s.xstr_list[299].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[300].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_301 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[300].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_301 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[300].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_301 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[300].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_301 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[300].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_301 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_301 = mywa2f3s.xstr_list[300].xstr_cnt;

            jsonoutputStr3S.out_distance_301 = mywa2f3s.xstr_list[300].distance;

            jsonoutputStr3S.out_gap_flag_301 = mywa2f3s.xstr_list[300].gap_flag;

            jsonoutputStr3S.out_node_num_301 = mywa2f3s.xstr_list[300].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[301].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_302 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[301].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_302 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[301].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_302 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[301].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_302 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[301].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_302 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_302 = mywa2f3s.xstr_list[301].xstr_cnt;

            jsonoutputStr3S.out_distance_302 = mywa2f3s.xstr_list[301].distance;

            jsonoutputStr3S.out_gap_flag_302 = mywa2f3s.xstr_list[301].gap_flag;

            jsonoutputStr3S.out_node_num_302 = mywa2f3s.xstr_list[301].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[302].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_303 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[302].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_303 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[302].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_303 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[302].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_303 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[302].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_303 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_303 = mywa2f3s.xstr_list[302].xstr_cnt;

            jsonoutputStr3S.out_distance_303 = mywa2f3s.xstr_list[302].distance;

            jsonoutputStr3S.out_gap_flag_303 = mywa2f3s.xstr_list[302].gap_flag;

            jsonoutputStr3S.out_node_num_303 = mywa2f3s.xstr_list[302].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[303].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_304 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[303].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_304 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[303].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_304 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[303].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_304 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[303].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_304 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_304 = mywa2f3s.xstr_list[303].xstr_cnt;

            jsonoutputStr3S.out_distance_304 = mywa2f3s.xstr_list[303].distance;

            jsonoutputStr3S.out_gap_flag_304 = mywa2f3s.xstr_list[303].gap_flag;

            jsonoutputStr3S.out_node_num_304 = mywa2f3s.xstr_list[303].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[304].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_305 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[304].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_305 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[304].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_305 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[304].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_305 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[304].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_305 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_305 = mywa2f3s.xstr_list[304].xstr_cnt;

            jsonoutputStr3S.out_distance_305 = mywa2f3s.xstr_list[304].distance;

            jsonoutputStr3S.out_gap_flag_305 = mywa2f3s.xstr_list[304].gap_flag;

            jsonoutputStr3S.out_node_num_305 = mywa2f3s.xstr_list[304].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[305].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_306 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[305].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_306 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[305].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_306 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[305].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_306 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[305].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_306 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_306 = mywa2f3s.xstr_list[305].xstr_cnt;

            jsonoutputStr3S.out_distance_306 = mywa2f3s.xstr_list[305].distance;

            jsonoutputStr3S.out_gap_flag_306 = mywa2f3s.xstr_list[305].gap_flag;

            jsonoutputStr3S.out_node_num_306 = mywa2f3s.xstr_list[305].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[306].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_307 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[306].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_307 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[306].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_307 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[306].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_307 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[306].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_307 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_307 = mywa2f3s.xstr_list[306].xstr_cnt;

            jsonoutputStr3S.out_distance_307 = mywa2f3s.xstr_list[306].distance;

            jsonoutputStr3S.out_gap_flag_307 = mywa2f3s.xstr_list[306].gap_flag;

            jsonoutputStr3S.out_node_num_307 = mywa2f3s.xstr_list[306].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[307].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_308 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[307].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_308 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[307].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_308 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[307].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_308 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[307].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_308 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_308 = mywa2f3s.xstr_list[307].xstr_cnt;

            jsonoutputStr3S.out_distance_308 = mywa2f3s.xstr_list[307].distance;

            jsonoutputStr3S.out_gap_flag_308 = mywa2f3s.xstr_list[307].gap_flag;

            jsonoutputStr3S.out_node_num_308 = mywa2f3s.xstr_list[307].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[308].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_309 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[308].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_309 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[308].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_309 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[308].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_309 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[308].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_309 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_309 = mywa2f3s.xstr_list[308].xstr_cnt;

            jsonoutputStr3S.out_distance_309 = mywa2f3s.xstr_list[308].distance;

            jsonoutputStr3S.out_gap_flag_309 = mywa2f3s.xstr_list[308].gap_flag;

            jsonoutputStr3S.out_node_num_309 = mywa2f3s.xstr_list[308].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[309].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_310 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[309].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_310 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[309].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_310 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[309].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_310 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[309].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_310 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_310 = mywa2f3s.xstr_list[309].xstr_cnt;

            jsonoutputStr3S.out_distance_310 = mywa2f3s.xstr_list[309].distance;

            jsonoutputStr3S.out_gap_flag_310 = mywa2f3s.xstr_list[309].gap_flag;

            jsonoutputStr3S.out_node_num_310 = mywa2f3s.xstr_list[309].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[310].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_311 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[310].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_311 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[310].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_311 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[310].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_311 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[310].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_311 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_311 = mywa2f3s.xstr_list[310].xstr_cnt;

            jsonoutputStr3S.out_distance_311 = mywa2f3s.xstr_list[310].distance;

            jsonoutputStr3S.out_gap_flag_311 = mywa2f3s.xstr_list[310].gap_flag;

            jsonoutputStr3S.out_node_num_311 = mywa2f3s.xstr_list[310].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[311].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_312 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[311].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_312 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[311].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_312 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[311].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_312 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[311].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_312 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_312 = mywa2f3s.xstr_list[311].xstr_cnt;

            jsonoutputStr3S.out_distance_312 = mywa2f3s.xstr_list[311].distance;

            jsonoutputStr3S.out_gap_flag_312 = mywa2f3s.xstr_list[311].gap_flag;

            jsonoutputStr3S.out_node_num_312 = mywa2f3s.xstr_list[311].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[312].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_313 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[312].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_313 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[312].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_313 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[312].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_313 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[312].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_313 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_313 = mywa2f3s.xstr_list[312].xstr_cnt;

            jsonoutputStr3S.out_distance_313 = mywa2f3s.xstr_list[312].distance;

            jsonoutputStr3S.out_gap_flag_313 = mywa2f3s.xstr_list[312].gap_flag;

            jsonoutputStr3S.out_node_num_313 = mywa2f3s.xstr_list[312].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[313].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_314 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[313].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_314 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[313].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_314 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[313].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_314 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[313].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_314 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_314 = mywa2f3s.xstr_list[313].xstr_cnt;

            jsonoutputStr3S.out_distance_314 = mywa2f3s.xstr_list[313].distance;

            jsonoutputStr3S.out_gap_flag_314 = mywa2f3s.xstr_list[313].gap_flag;

            jsonoutputStr3S.out_node_num_314 = mywa2f3s.xstr_list[313].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[314].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_315 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[314].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_315 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[314].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_315 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[314].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_315 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[314].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_315 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_315 = mywa2f3s.xstr_list[314].xstr_cnt;

            jsonoutputStr3S.out_distance_315 = mywa2f3s.xstr_list[314].distance;

            jsonoutputStr3S.out_gap_flag_315 = mywa2f3s.xstr_list[314].gap_flag;

            jsonoutputStr3S.out_node_num_315 = mywa2f3s.xstr_list[314].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[315].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_316 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[315].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_316 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[315].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_316 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[315].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_316 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[315].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_316 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_316 = mywa2f3s.xstr_list[315].xstr_cnt;

            jsonoutputStr3S.out_distance_316 = mywa2f3s.xstr_list[315].distance;

            jsonoutputStr3S.out_gap_flag_316 = mywa2f3s.xstr_list[315].gap_flag;

            jsonoutputStr3S.out_node_num_316 = mywa2f3s.xstr_list[315].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[316].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_317 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[316].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_317 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[316].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_317 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[316].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_317 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[316].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_317 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_317 = mywa2f3s.xstr_list[316].xstr_cnt;

            jsonoutputStr3S.out_distance_317 = mywa2f3s.xstr_list[316].distance;

            jsonoutputStr3S.out_gap_flag_317 = mywa2f3s.xstr_list[316].gap_flag;

            jsonoutputStr3S.out_node_num_317 = mywa2f3s.xstr_list[316].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[317].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_318 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[317].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_318 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[317].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_318 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[317].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_318 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[317].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_318 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_318 = mywa2f3s.xstr_list[317].xstr_cnt;

            jsonoutputStr3S.out_distance_318 = mywa2f3s.xstr_list[317].distance;

            jsonoutputStr3S.out_gap_flag_318 = mywa2f3s.xstr_list[317].gap_flag;

            jsonoutputStr3S.out_node_num_318 = mywa2f3s.xstr_list[317].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[318].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_319 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[318].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_319 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[318].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_319 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[318].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_319 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[318].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_319 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_319 = mywa2f3s.xstr_list[318].xstr_cnt;

            jsonoutputStr3S.out_distance_319 = mywa2f3s.xstr_list[318].distance;

            jsonoutputStr3S.out_gap_flag_319 = mywa2f3s.xstr_list[318].gap_flag;

            jsonoutputStr3S.out_node_num_319 = mywa2f3s.xstr_list[318].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[319].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_320 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[319].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_320 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[319].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_320 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[319].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_320 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[319].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_320 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_320 = mywa2f3s.xstr_list[319].xstr_cnt;

            jsonoutputStr3S.out_distance_320 = mywa2f3s.xstr_list[319].distance;

            jsonoutputStr3S.out_gap_flag_320 = mywa2f3s.xstr_list[319].gap_flag;

            jsonoutputStr3S.out_node_num_320 = mywa2f3s.xstr_list[319].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[320].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_321 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[320].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_321 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[320].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_321 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[320].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_321 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[320].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_321 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_321 = mywa2f3s.xstr_list[320].xstr_cnt;

            jsonoutputStr3S.out_distance_321 = mywa2f3s.xstr_list[320].distance;

            jsonoutputStr3S.out_gap_flag_321 = mywa2f3s.xstr_list[320].gap_flag;

            jsonoutputStr3S.out_node_num_321 = mywa2f3s.xstr_list[320].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[321].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_322 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[321].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_322 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[321].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_322 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[321].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_322 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[321].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_322 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_322 = mywa2f3s.xstr_list[321].xstr_cnt;

            jsonoutputStr3S.out_distance_322 = mywa2f3s.xstr_list[321].distance;

            jsonoutputStr3S.out_gap_flag_322 = mywa2f3s.xstr_list[321].gap_flag;

            jsonoutputStr3S.out_node_num_322 = mywa2f3s.xstr_list[321].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[322].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_323 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[322].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_323 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[322].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_323 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[322].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_323 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[322].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_323 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_323 = mywa2f3s.xstr_list[322].xstr_cnt;

            jsonoutputStr3S.out_distance_323 = mywa2f3s.xstr_list[322].distance;

            jsonoutputStr3S.out_gap_flag_323 = mywa2f3s.xstr_list[322].gap_flag;

            jsonoutputStr3S.out_node_num_323 = mywa2f3s.xstr_list[322].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[323].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_324 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[323].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_324 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[323].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_324 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[323].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_324 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[323].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_324 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_324 = mywa2f3s.xstr_list[323].xstr_cnt;

            jsonoutputStr3S.out_distance_324 = mywa2f3s.xstr_list[323].distance;

            jsonoutputStr3S.out_gap_flag_324 = mywa2f3s.xstr_list[323].gap_flag;

            jsonoutputStr3S.out_node_num_324 = mywa2f3s.xstr_list[323].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[324].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_325 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[324].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_325 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[324].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_325 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[324].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_325 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[324].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_325 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_325 = mywa2f3s.xstr_list[324].xstr_cnt;

            jsonoutputStr3S.out_distance_325 = mywa2f3s.xstr_list[324].distance;

            jsonoutputStr3S.out_gap_flag_325 = mywa2f3s.xstr_list[324].gap_flag;

            jsonoutputStr3S.out_node_num_325 = mywa2f3s.xstr_list[324].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[325].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_326 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[325].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_326 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[325].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_326 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[325].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_326 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[325].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_326 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_326 = mywa2f3s.xstr_list[325].xstr_cnt;

            jsonoutputStr3S.out_distance_326 = mywa2f3s.xstr_list[325].distance;

            jsonoutputStr3S.out_gap_flag_326 = mywa2f3s.xstr_list[325].gap_flag;

            jsonoutputStr3S.out_node_num_326 = mywa2f3s.xstr_list[325].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[326].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_327 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[326].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_327 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[326].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_327 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[326].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_327 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[326].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_327 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_327 = mywa2f3s.xstr_list[326].xstr_cnt;

            jsonoutputStr3S.out_distance_327 = mywa2f3s.xstr_list[326].distance;

            jsonoutputStr3S.out_gap_flag_327 = mywa2f3s.xstr_list[326].gap_flag;

            jsonoutputStr3S.out_node_num_327 = mywa2f3s.xstr_list[326].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[327].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_328 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[327].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_328 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[327].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_328 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[327].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_328 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[327].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_328 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_328 = mywa2f3s.xstr_list[327].xstr_cnt;

            jsonoutputStr3S.out_distance_328 = mywa2f3s.xstr_list[327].distance;

            jsonoutputStr3S.out_gap_flag_328 = mywa2f3s.xstr_list[327].gap_flag;

            jsonoutputStr3S.out_node_num_328 = mywa2f3s.xstr_list[327].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[328].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_329 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[328].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_329 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[328].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_329 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[328].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_329 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[328].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_329 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_329 = mywa2f3s.xstr_list[328].xstr_cnt;

            jsonoutputStr3S.out_distance_329 = mywa2f3s.xstr_list[328].distance;

            jsonoutputStr3S.out_gap_flag_329 = mywa2f3s.xstr_list[328].gap_flag;

            jsonoutputStr3S.out_node_num_329 = mywa2f3s.xstr_list[328].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[329].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_330 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[329].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_330 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[329].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_330 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[329].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_330 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[329].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_330 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_330 = mywa2f3s.xstr_list[329].xstr_cnt;

            jsonoutputStr3S.out_distance_330 = mywa2f3s.xstr_list[329].distance;

            jsonoutputStr3S.out_gap_flag_330 = mywa2f3s.xstr_list[329].gap_flag;

            jsonoutputStr3S.out_node_num_330 = mywa2f3s.xstr_list[329].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[330].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_331 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[330].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_331 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[330].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_331 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[330].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_331 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[330].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_331 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_331 = mywa2f3s.xstr_list[330].xstr_cnt;

            jsonoutputStr3S.out_distance_331 = mywa2f3s.xstr_list[330].distance;

            jsonoutputStr3S.out_gap_flag_331 = mywa2f3s.xstr_list[330].gap_flag;

            jsonoutputStr3S.out_node_num_331 = mywa2f3s.xstr_list[330].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[331].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_332 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[331].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_332 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[331].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_332 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[331].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_332 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[331].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_332 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_332 = mywa2f3s.xstr_list[331].xstr_cnt;

            jsonoutputStr3S.out_distance_332 = mywa2f3s.xstr_list[331].distance;

            jsonoutputStr3S.out_gap_flag_332 = mywa2f3s.xstr_list[331].gap_flag;

            jsonoutputStr3S.out_node_num_332 = mywa2f3s.xstr_list[331].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[332].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_333 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[332].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_333 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[332].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_333 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[332].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_333 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[332].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_333 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_333 = mywa2f3s.xstr_list[332].xstr_cnt;

            jsonoutputStr3S.out_distance_333 = mywa2f3s.xstr_list[332].distance;

            jsonoutputStr3S.out_gap_flag_333 = mywa2f3s.xstr_list[332].gap_flag;

            jsonoutputStr3S.out_node_num_333 = mywa2f3s.xstr_list[332].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[333].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_334 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[333].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_334 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[333].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_334 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[333].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_334 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[333].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_334 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_334 = mywa2f3s.xstr_list[333].xstr_cnt;

            jsonoutputStr3S.out_distance_334 = mywa2f3s.xstr_list[333].distance;

            jsonoutputStr3S.out_gap_flag_334 = mywa2f3s.xstr_list[333].gap_flag;

            jsonoutputStr3S.out_node_num_334 = mywa2f3s.xstr_list[333].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[334].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_335 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[334].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_335 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[334].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_335 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[334].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_335 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[334].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_335 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_335 = mywa2f3s.xstr_list[334].xstr_cnt;

            jsonoutputStr3S.out_distance_335 = mywa2f3s.xstr_list[334].distance;

            jsonoutputStr3S.out_gap_flag_335 = mywa2f3s.xstr_list[334].gap_flag;

            jsonoutputStr3S.out_node_num_335 = mywa2f3s.xstr_list[334].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[335].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_336 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[335].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_336 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[335].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_336 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[335].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_336 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[335].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_336 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_336 = mywa2f3s.xstr_list[335].xstr_cnt;

            jsonoutputStr3S.out_distance_336 = mywa2f3s.xstr_list[335].distance;

            jsonoutputStr3S.out_gap_flag_336 = mywa2f3s.xstr_list[335].gap_flag;

            jsonoutputStr3S.out_node_num_336 = mywa2f3s.xstr_list[335].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[336].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_337 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[336].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_337 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[336].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_337 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[336].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_337 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[336].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_337 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_337 = mywa2f3s.xstr_list[336].xstr_cnt;

            jsonoutputStr3S.out_distance_337 = mywa2f3s.xstr_list[336].distance;

            jsonoutputStr3S.out_gap_flag_337 = mywa2f3s.xstr_list[336].gap_flag;

            jsonoutputStr3S.out_node_num_337 = mywa2f3s.xstr_list[336].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[337].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_338 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[337].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_338 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[337].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_338 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[337].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_338 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[337].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_338 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_338 = mywa2f3s.xstr_list[337].xstr_cnt;

            jsonoutputStr3S.out_distance_338 = mywa2f3s.xstr_list[337].distance;

            jsonoutputStr3S.out_gap_flag_338 = mywa2f3s.xstr_list[337].gap_flag;

            jsonoutputStr3S.out_node_num_338 = mywa2f3s.xstr_list[337].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[338].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_339 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[338].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_339 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[338].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_339 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[338].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_339 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[338].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_339 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_339 = mywa2f3s.xstr_list[338].xstr_cnt;

            jsonoutputStr3S.out_distance_339 = mywa2f3s.xstr_list[338].distance;

            jsonoutputStr3S.out_gap_flag_339 = mywa2f3s.xstr_list[338].gap_flag;

            jsonoutputStr3S.out_node_num_339 = mywa2f3s.xstr_list[338].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[339].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_340 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[339].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_340 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[339].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_340 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[339].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_340 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[339].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_340 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_340 = mywa2f3s.xstr_list[339].xstr_cnt;

            jsonoutputStr3S.out_distance_340 = mywa2f3s.xstr_list[339].distance;

            jsonoutputStr3S.out_gap_flag_340 = mywa2f3s.xstr_list[339].gap_flag;

            jsonoutputStr3S.out_node_num_340 = mywa2f3s.xstr_list[339].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[340].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_341 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[340].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_341 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[340].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_341 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[340].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_341 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[340].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_341 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_341 = mywa2f3s.xstr_list[340].xstr_cnt;

            jsonoutputStr3S.out_distance_341 = mywa2f3s.xstr_list[340].distance;

            jsonoutputStr3S.out_gap_flag_341 = mywa2f3s.xstr_list[340].gap_flag;

            jsonoutputStr3S.out_node_num_341 = mywa2f3s.xstr_list[340].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[341].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_342 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[341].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_342 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[341].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_342 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[341].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_342 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[341].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_342 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_342 = mywa2f3s.xstr_list[341].xstr_cnt;

            jsonoutputStr3S.out_distance_342 = mywa2f3s.xstr_list[341].distance;

            jsonoutputStr3S.out_gap_flag_342 = mywa2f3s.xstr_list[341].gap_flag;

            jsonoutputStr3S.out_node_num_342 = mywa2f3s.xstr_list[341].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[342].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_343 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[342].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_343 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[342].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_343 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[342].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_343 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[342].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_343 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_343 = mywa2f3s.xstr_list[342].xstr_cnt;

            jsonoutputStr3S.out_distance_343 = mywa2f3s.xstr_list[342].distance;

            jsonoutputStr3S.out_gap_flag_343 = mywa2f3s.xstr_list[342].gap_flag;

            jsonoutputStr3S.out_node_num_343 = mywa2f3s.xstr_list[342].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[343].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_344 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[343].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_344 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[343].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_344 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[343].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_344 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[343].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_344 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_344 = mywa2f3s.xstr_list[343].xstr_cnt;

            jsonoutputStr3S.out_distance_344 = mywa2f3s.xstr_list[343].distance;

            jsonoutputStr3S.out_gap_flag_344 = mywa2f3s.xstr_list[343].gap_flag;

            jsonoutputStr3S.out_node_num_344 = mywa2f3s.xstr_list[343].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[344].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_345 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[344].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_345 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[344].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_345 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[344].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_345 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[344].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_345 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_345 = mywa2f3s.xstr_list[344].xstr_cnt;

            jsonoutputStr3S.out_distance_345 = mywa2f3s.xstr_list[344].distance;

            jsonoutputStr3S.out_gap_flag_345 = mywa2f3s.xstr_list[344].gap_flag;

            jsonoutputStr3S.out_node_num_345 = mywa2f3s.xstr_list[344].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[345].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_346 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[345].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_346 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[345].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_346 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[345].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_346 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[345].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_346 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_346 = mywa2f3s.xstr_list[345].xstr_cnt;

            jsonoutputStr3S.out_distance_346 = mywa2f3s.xstr_list[345].distance;

            jsonoutputStr3S.out_gap_flag_346 = mywa2f3s.xstr_list[345].gap_flag;

            jsonoutputStr3S.out_node_num_346 = mywa2f3s.xstr_list[345].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[346].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_347 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[346].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_347 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[346].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_347 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[346].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_347 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[346].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_347 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_347 = mywa2f3s.xstr_list[346].xstr_cnt;

            jsonoutputStr3S.out_distance_347 = mywa2f3s.xstr_list[346].distance;

            jsonoutputStr3S.out_gap_flag_347 = mywa2f3s.xstr_list[346].gap_flag;

            jsonoutputStr3S.out_node_num_347 = mywa2f3s.xstr_list[346].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[347].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_348 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[347].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_348 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[347].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_348 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[347].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_348 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[347].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_348 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_348 = mywa2f3s.xstr_list[347].xstr_cnt;

            jsonoutputStr3S.out_distance_348 = mywa2f3s.xstr_list[347].distance;

            jsonoutputStr3S.out_gap_flag_348 = mywa2f3s.xstr_list[347].gap_flag;

            jsonoutputStr3S.out_node_num_348 = mywa2f3s.xstr_list[347].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[348].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_349 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[348].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_349 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[348].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_349 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[348].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_349 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[348].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_349 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_349 = mywa2f3s.xstr_list[348].xstr_cnt;

            jsonoutputStr3S.out_distance_349 = mywa2f3s.xstr_list[348].distance;

            jsonoutputStr3S.out_gap_flag_349 = mywa2f3s.xstr_list[348].gap_flag;

            jsonoutputStr3S.out_node_num_349 = mywa2f3s.xstr_list[348].node_num;

            mywa1_dl1.out_b7sc_list[0] = mywa2f3s.xstr_list[349].xstr_b7sc_list[0];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_intersecting_street_350 = mywa1_dl1.out_stname_list[0].Trim();

            mywa1_dl1.out_b7sc_list[1] = mywa2f3s.xstr_list[349].xstr_b7sc_list[1];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_second_intersecting_street_350 = mywa1_dl1.out_stname_list[1].Trim();

            mywa1_dl1.out_b7sc_list[2] = mywa2f3s.xstr_list[349].xstr_b7sc_list[2];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_third_intersecting_street_350 = mywa1_dl1.out_stname_list[2].Trim();

            mywa1_dl1.out_b7sc_list[3] = mywa2f3s.xstr_list[349].xstr_b7sc_list[3];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fourth_intersecting_street_350 = mywa1_dl1.out_stname_list[3].Trim();

            mywa1_dl1.out_b7sc_list[4] = mywa2f3s.xstr_list[349].xstr_b7sc_list[4];
            mygeo.GeoCall(ref mywa1_dl1);
            jsonoutputStr3S.out_fifth_intersecting_street_350 = mywa1_dl1.out_stname_list[4].Trim();

            jsonoutputStr3S.out_xstr_cnt_350 = mywa2f3s.xstr_list[349].xstr_cnt;

            jsonoutputStr3S.out_distance_350 = mywa2f3s.xstr_list[349].distance;

            jsonoutputStr3S.out_gap_flag_350 = mywa2f3s.xstr_list[349].gap_flag;

            jsonoutputStr3S.out_node_num_350 = mywa2f3s.xstr_list[349].node_num;

            return JsonConvert.SerializeObject(jsonoutputStr3S);
        }

        public string getStreetName(string borough_code, string street_code)
        {


            geo fdgeo = new geo();
            GeoConn fdconn = new GeoConn();
            GeoConnCollection fdconns = new GeoConnCollection();
            Wa1 mywa1 = new Wa1();

            mywa1.Clear();
            mywa1.in_func_code = "D";
            mywa1.in_platform_ind = "C";
            mywa1.in_b10sc1.boro = borough_code.ToString();
            mywa1.in_b10sc1.sc5 = street_code;
            fdgeo.GeoCall(ref mywa1);

            return mywa1.out_stname1;

        }

        string IGeoService.GetHelloName(string First, string Last)
        {
            return "Hello " + First + " " + Last + "!!";
        }
    }
}
