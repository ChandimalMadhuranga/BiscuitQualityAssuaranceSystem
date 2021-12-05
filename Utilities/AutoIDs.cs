using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BiscuitQualityAssuaranceSystem.Models;

namespace BiscuitQualityAssuaranceSystem.Utilities
{
    public class AutoIDs
    {
         
        public static string autoId(string Form, ModelQualityAssuaranceDB db)
        {
            int maxId = 0;
            string preFix = "";

            try
            {
                switch (Form)
                {                    
                    case "Complaint":
                        preFix = "COM";
                        maxId = int.Parse(db.Complaints.Max(t => t.Com_ID).Substring(9));                        
                        break;
                    case "Decision":
                        preFix = "DES";
                        maxId = int.Parse(db.Decisions.Max(t => t.Des_ID).Substring(9));                        
                        break;
                    case "Product_Plan":                        
                        maxId = int.Parse(db.Product_Plan.Max(t => t.Product_Plane_Id).Substring(10));
                        preFix = "PLN_";
                        break;
                    case "Product":                        
                        maxId = int.Parse(db.Products.Max(t => t.Product_code).Substring(10));
                        preFix = "PRD_";
                        break;
                    case "Quality_Checker":                        
                        maxId = int.Parse(db.Quality_Checker.Max(t => t.Qc_id).Substring(8));
                        preFix = "QC";
                        break;
                    case "Quality_Parameter":                        
                        maxId = int.Parse(db.Quality_Parameter.Max(t => t.QP_code).Substring(9));
                        preFix = "QP_";
                        break;
                    case "Shift_Manager":                        
                        maxId = int.Parse(db.Shift_Manager.Max(t => t.SM_id).Substring(8));
                        preFix = "SM";
                        break;
                    case "User":                        
                        maxId = int.Parse(db.Users.Max(t => t.User_Employee_ID).Substring(10));
                        preFix = "EMP_";
                        break;                    
                    default:
                        break;
                }
            }
            catch (Exception)
            {

                maxId = 0;
            }
            return preFix + DateTime.Now.ToString("yyyyMM") + (maxId + 1).ToString("00#");
        }
    }
}
