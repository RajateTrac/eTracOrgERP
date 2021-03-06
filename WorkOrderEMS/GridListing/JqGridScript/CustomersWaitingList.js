﻿var UnApprovedCompanyurl = '/CustomerManagement/GetAllCustomerList';
var AddCustomer = '/CustomerManagement/CustomerManagementSetup/';
var veiwCustomerDetails = '/CustomerManagement/GetAllCustomerDataToView/';

var LocationId; var CustomerId;

$(function () {
    var act;
    $("#jsGrid-basic").jsGrid({
        height: "170%",
        width: "100%",
        filtering: false,
        editing: false,
        inserting: false,
        sorting: false,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: UnApprovedCompanyurl + '?flagApproved=N',
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "CustomerId", title: "Customer Id", type: "text", width: 50 },
            { name: "CustomerName", title: "Name", type: "text", width: 50 },
            { name: "LocationName", title: "Location", type: "text", width: 50 },
            { name: "EmailId", title: "Email", type: "text", width: 50 },
            { name: "Phone1", title: "Phone1", type: "text", width: 50 },
            { name: "CustomerTypeText", title: "Company", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 },
            {
                name: "act", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    //TO add icon edit and delete to perform update and delete operation
                    var $iconPencil = $("<i>").attr({ class: "fa fa-eye" }).attr({ style: "color:black;font-size: 22px;" });

                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View Details" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            new fn_showMaskloader('Please wait...');
                            ViewDetails(item);
                        }).append($iconPencil);

                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                }
            }
        ]
    });

});

function ViewDetails(item) {

    CustomerId = item.CustomerId;

    $.ajax({
        type: "POST",
        url: veiwCustomerDetails + '?CustomerId=' + item.CustomerId,
        datatype: 'json',
        success: function (result) {

            $("#lblFirstName").html(result.FirstName);
            $("#lblLastName").html(result.LastName);
            $("#lblCustomerType").html(result.CustomerTypeText);
            $("#lblAddress1").html(result.Address1);
            $("#lblState").html(result.Address1StateText);
            $("#lblMailingAddress").html(result.Address2);
            $("#lblMailingState").html(result.Address2StateText);
            $("#lblWorkPhoneNo").html(result.Phone1);
            $("#lblCellPhoneNo").html(result.Phone2);
            $("#lblEmail").html(result.Email);
            $("#lblDLNo").html(result.DLNo);
            $("#lblMethodOfCommunication").html(result.MethodOfCommunication);
            $("#lblParkingFacilityLocation").html(result.ParkingFacilityLocation);
            $("#lblMonthlyPrice").html(result.MonthlyPrice);
            $("#lblIsAllowToSendText").html(result.IsAllowToSendText);
            $("#lblIsMonthlyAppointmentSchedule").html(result.IsMonthlyAppointmentSchedule);
            $("#lblScheduleAppointDate").html(result.ScheduleAppointDate);
            $("#lblScheduleAppointTime").html(result.ScheduleAppointTime);
            $("#lblIsMonthlyParkingPaidFor").html(result.IsMonthlyParkingPaidFor);
            $("#lblCompanyName").html(result.CompanyName);
            $("#lblIsSendDirectInvoiceToCompany").html(result.IsSendDirectInvoiceToCompany);
            $("#lblCompanyEmail").html(result.CompanyEmail);
            $("#lblPaymentMethod").html(result.PaymentMethod);
            $("#lblAccountNumber").html(result.AccountNumber);
            $("#lblBankName").html(result.BankName);
            $("#lblIFSCcode").html(result.IFSCcode);
            $("#lblBankRoutingNo").html(result.BankRoutingNo);
            $("#lblCardHolderName").html(result.CardHolderName);
            $("#lblBankAddress").html(result.Address);
            $("#lblCardNumber").html(result.CardNumber);
            $("#lblCardType").html(result.CardType);
            $("#lblCardExpirationDate").html(result.CardExpirationDate);
            $("#lblIsSignupForAutomaticPayment").html(result.IsSignupForAutomaticPayment);

            if (item.Status == "Rejected") {
                $("#btnApproveData").hide();
                $("#btnRejectPO").hide();
            }
            else {
                $("#btnApproveData").show();
                $("#btnRejectPO").show();
            }

            if (result.CustomerVehicleDetails != null) {
                $('#records_table').html('');
                var arrData = [];
                $('#tblVehicleDetails').html("");
                $('#tblVehicleDetails tbody').empty();
                var thHTML = '';
                thHTML += '<tr style="background-color:#0792bc;"><th>Lic. Plate No.</th><th>State</th><th>Year</th><th>Make</th><th>Model</th><th>Color</th></tr>';
                $('#tblVehicleDetails').append(thHTML);
                if (result.CustomerVehicleDetails.length > 0) {
                    for (i = 0; i < result.CustomerVehicleDetails.length; i++) {
                        var trHTML = '';
                        trHTML +=
                            '<tr><td>' + result.CustomerVehicleDetails[i].LicensePlateNo +
                            '</td><td>' + result.CustomerVehicleDetails[i].StateText +
                            '</td><td>' + result.CustomerVehicleDetails[i].Year +
                            '</td><td>' + result.CustomerVehicleDetails[i].Make +
                            '</td><td>' + result.CustomerVehicleDetails[i].Model +
                            '</td><td>' + result.CustomerVehicleDetails[i].Color +
                            '</td></tr>';

                        $('#tblVehicleDetails').append(trHTML);
                    }
                }
            }
            $('.modal-title').text("Customer All Details");
            $("#myModalForGetUnApprovedCustomerDetails").modal('show');
            new fn_hideMaskloader();
        },
        error: function () {
            new fn_hideMaskloader();
        }
    });
}
var timeoutHnd;
var flAuto = true;
function doSearch(ev) {
    var act;
    $("#jsGrid-basic").jsGrid({
        height: "170%",
        width: "100%",
        filtering: false,
        editing: false,
        inserting: false,
        sorting: false,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function (filter) {
                return $.ajax({
                    type: "GET",
                    url: UnApprovedCompanyurl + '?_search=' + $("#SearchText").val() + '&CustomerType=' + $("#CustomerType").val(),
                    data: filter,
                    dataType: "json"
                });
            }
        },

        fields: [
            { name: "CustomerId", title: "Customer Id", type: "text", width: 50 },
            { name: "CustomerName", title: "Name", type: "text", width: 50 },
            { name: "LocationName", title: "Location", type: "text", width: 50 },
            { name: "EmailId", title: "Email", type: "text", width: 50 },
            { name: "Phone1", title: "Phone1", type: "text", width: 50 },
            { name: "CustomerTypeText", title: "Company", type: "text", width: 50 },
            { name: "Status", title: "Status", type: "text", width: 50 },
            {
                name: "act", items: act, title: "Action", width: 50, css: "text-center", itemTemplate: function (value, item) {
                    //TO add icon edit and delete to perform update and delete operation
                    var $iconPencil = $("<i>").attr({ class: "fa fa-eye" }).attr({ style: "color:black;font-size: 22px;" });

                    var $customEditButton = $("<span style='padding: 0 5px 0 0;'>")
                        .attr({ title: "View Details" })
                        .attr({ id: "btn-edit-" + item.Id }).click(function (e) {
                            ViewDetails(item);
                        }).append($iconPencil);

                    return $("<div>").attr({ class: "btn-toolbar" }).append($customEditButton);
                }
            }
        ]
    });
}
function filter(args) {
}

$("#AddCustomer").on("click", function (event) {
    window.location.href = AddCustomer;
});


$("#ViewUnApprovedVendorData").on("click", function (event) {
    CustomerId = $(this).attr("vid");
    var rowData = jQuery("#tbl_AllUnApprovedList").getRowData(CustomerId);
    if (rowData.Status == "Y") {
        $("#btnApproveData").hide();
        $('#btnRejectPO').hide();
    }
    else {
        $("#btnApproveData").show();
        $('#btnRejectPO').show();
    }

    $.ajax({
        type: "POST",
        url: veiwCustomerDetails + '?CustomerId=' + CustomerId,
        datatype: 'json',
        success: function (result) {
            $("#lblVendorNameLegal").html(result.CompanyNameLegal); $("#lblVendorNameDBA").html(result.CompanyNameDBA);
            $("#lblAddress").html(result.Address1); $("#lblPhone1").html(result.Phone1);
            $("#lblPhone2").html(result.Phone2); $("#lblEmail").html(result.Email);
            $("#lblWebsite").html(result.Website); $("#lblLicenseName").html(result.CompanyNameLegal);
            $("#lblLicenseNumber").html(result.LicenseNumber); $("#lblLicenseExpirationDate").html(result.LicenseExpirationDate);
            $("#lblInsuranceCarries").html(result.InsuranceCarries); $("#lblPolicyNumber").html(result.PolicyNumber);
            $("#lblInsuranceExpirationDate").html(result.InsuranceExpirationDate); $("#lblFirstCompany").html(result.CompanyNameLegal);
            $("#lblSecondaryCompany").html(result.SecondaryCompany); $("#lblVendorTypeContract").html(result.VendorTypeData);
            $("#lblContractType").html(result.ContractType); $("#lblContractissuedby").html(result.ContractIssuedBy);
            $("#lblContractexecutedby").html(result.ContractExecutedBy);
            $("#lblPrimaryPaymentMode").html(result.PrimaryPaymentMode); $("#lblPaymentTerm").html(result.PaymentTerm);
            $("#lblGracePeriod").html(result.GracePeriod); $("#lblInvoicingFrequency").html(result.InvoicingFrequecy);
            $("#lblStartDate").html(result.StartDate);
            $("#lblEndDate").html(result.EndDate); $("#lblCostDuringPeriod").html(result.CostDuringPeriod);
            $("#lblAnnualValueOfAggriment").html(result.AnnualValueOfAggriment); $("#lblMinimumBillAmount").html(result.MinimumBillAmount);
            $("#lblBillDueDate").html(result.BillDueDate);
            $("#lblLateFineFee").html(result.LateFine); $("#lblPayMode").html(result.PrimaryPaymentMode);
            $("#lblBankName").html(result.BankName); $("#lblBankLocation").html(result.BankLocation);
            $("#lblAccountNumber").html(result.AccountNumber); $("#lblIFSCCode").html(result.IFSCCode);
            $("#lblSwiftOICCode").html(result.SwiftOICCode); $("#lblCardNumber").html(result.CardNumber);
            $("#lblCardHolderName").html(result.CardHolderName);
            $("#lblExpirationDate").html(result.ExpirationDate);
            $("#lblPolicyNumber").html(result.PolicyNumberAccount);
            if (result.LocationAssignedModel != null) {
                if (result.LocationAssignedModel.length > 0) {
                    var arr = [];
                    var llcmArr = [];
                    for (i = 0; i < result.LocationAssignedModel.length; i++) {
                        arr.push(result.LocationAssignedModel[i].LocationName);
                        llcmArr.push(result.LocationAssignedModel[i].LLCM_Id)
                    }
                    var loc = arr.join(',');
                    var llcm = llcmArr.join(',');
                    $("#lblSelectedLocation").html(loc);
                    $('#LLCM_Id').val(llcm);
                }
            }
            if (result.VendorFacilityModel != null) {
                $('#records_table').html('');
                var arrData = [];
                $('#UnVendorFacility_table tbody').empty();
                var thHTML = '';
                thHTML += '<tr style="background-color:#0792bc;"><th>Cost Code</th><th>Facility Type</th><th>Description</th><th>Unit Price</th><th>Tax</th></tr>';
                $('#UnVendorFacility_table').append(thHTML);
                if (result.VendorFacilityModel.length > 0) {
                    for (i = 0; i < result.VendorFacilityModel.length; i++) {
                        var trHTML = '';
                        trHTML +=
                            '<tr><td>' + result.VendorFacilityModel[i].Costcode +
                            '</td><td>' + result.VendorFacilityModel[i].ProductServiceType +
                            '</td><td>' + result.VendorFacilityModel[i].ProductServiceName +
                            '</td><td>' + result.VendorFacilityModel[i].UnitCost +
                            '</td><td>' + result.VendorFacilityModel[i].Tax +
                            '</td></tr>';

                        $('#UnVendorFacility_table').append(trHTML);
                    }
                }
            }
        }
    });
    $('.modal-title').text("Vendor All Details");
    $("#myModalForGetUnApprovedCustomerDetails").modal('show');
});

function RejectCustomer() {
    $("#myModelApproveRejectCustomer").modal('show');
}
function AppproveCustomer() {
    $("#CommentCustomer").val("");
    $("#btnApproveData").addClass("disabled");
    callAjaxCustomer();
}
function RejectCustomerAfterComment() {

    if ($("#Comment").val() != "") {

        callAjaxCustomer();
    }
    else {
        return false;
    }
}

function callAjaxCustomer() {
    var objData = new Object();
    objData.CustomerId = CustomerId;
    objData.LocationId = $_locationId;
    objData.Comment = $("#CommentCustomer").val();
    objData.LLCM_Id = $('#LLCM_Id').val();
    $.ajax({
        url: '../CustomerManagement/ApproveCustomer',
        type: 'POST',
        datatype: 'application/json',
        contentType: 'application/json',
        data: JSON.stringify({ objCustomerApproveRejectModel: objData }),
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            toastr.success(result);
            $("#myModalForGetUnApprovedCustomerDetails").modal('hide');
            $("#btnApproveData").removeClass("disabled");
            $("#jsGrid-basic").jsGrid("loadData");
        },
        error: function () { toastr.error(result); },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}