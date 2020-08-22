using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WorkOrderEMS.Models
{
    public class AddChartModel
    {
        [Display(Name = "Seating Name")]
        public string SeatingName { get; set; }
        public long? Id { get; set; }
        public long? JobTitleId { get; set; }
        public long? parentId { get; set; }
        [Display(Name = "Superior")]
        public long Superior { get; set; }
        [Display(Name = "Job Description")]
        public string JobDescription { get; set; }
        public string  JobDesc { get; set; }
        [AllowHtml]
        [Display(Name = "Roles And Responsibility")]
        public string RolesAndResponsibility { get; set; }
        [Display(Name = "Department")]
        public long? Department { get; set; }
        public string Action { get; set; }
        public string IsActive { get; set; }
        public string DepartmentName { get; set; }
        public string Image { get; set; }
        public string JobTitle { get; set; }
        public string JobTitleDesc { get; set; }
        public string  JobTitleLabel { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmploymentClassification { get; set; }
        public decimal? RateOfPay { get; set; }
        public long RequisitionId { get; set; }
        public string EmployeeId { get; set; }
        public string RequisitionType { get; set; }
        public string ActionStatus { get; set; }
        public long UserId { get; set; }
        public bool IsDeleted { get; set; }
        public int? JobTitleCount { get; set; }
        public string JobTitleCountDesc { get; set; }
        //public string[] JDSplitedString { get; set; }
        //public long? MyProperty { get; set; }
        public string VST_Level { get; set; }

        //New Chart Data properties
        public string nodeId { get; set; }
        public string parentNodeId { get; set; }
        public long width { get; set; }
        public long height { get; set; }
        public long borderWidth { get; set; }
        public long borderRadius { get; set; }
        public borderColor borderColor { get; set; }
        public borderColor backgroundColor { get; set; }
        public nodeImage nodeImage { get; set; }
        public nodeIcon nodeIcon { get; set; }
        public string template { get; set; }
        public borderColor connectorLineColor { get; set; }
        public int connectorLineWidth { get; set; }
        public string dashArray { get; set; }
        public bool expanded { get; set; }
        public int directSubordinates { get; set; }
        public int totalSubordinates { get; set; }
    }
    public class borderColor
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public int alpha { get; set; }
    }
    public class backgroundColor
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public int alpha { get; set; }
    }
    public class nodeImage
    {
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int centerTopDistance { get; set; }
        public int centerLeftDistance { get; set; }
        public bool shadow { get; set; }
        public string cornerShape { get; set; }
        public int borderWidth { get; set; }
        public borderColor borderColor { get; set; }
    }
    public class nodeIcon
    {
        public string icon { get; set; }
        public int size { get; set; }
    }
    public class connectorLineColor
    {
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public int alpha { get; set; }
    }
    public class BindDropDownList
    {
        public List<AddChartModel> listSuperiour { get; set; }
        public List<DepartmentModel> listDepartment { get; set; }
    }
    public class JobTitleModel
    {
        public long JobTitleId { get; set; }
        public string JobTitle { get; set; }
        public int? JobTitleCount { get; set; }
        public int? JobTitleLastCount { get; set; }
        public long UserId { get; set; }
    }

    public class AddChartModelTest
    {
        public string nodeId { get; set; }
        public string parentNodeId { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int borderWidth { get; set; }
        public int borderRadius { get; set; }
        public borderColor borderColor { get; set; }
        public backgroundColor backgroundColor { get; set; }
        public nodeImage nodeImage { get; set; }
        public nodeIcon nodeIcon { get; set; }
        public string template { get; set; }
        public connectorLineColor connectorLineColor { get; set; }
        public int connectorLineWidth { get; set; }
        public string dashArray { get; set; }
        public bool expanded { get; set; }
        public int directSubordinates { get; set; }
        public int totalSubordinates { get; set; }
    }
}
