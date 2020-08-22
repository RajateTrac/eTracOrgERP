var HOBurl = '../HirinngOnBoarding/GetHiringOnBoardingList';
var base_url = window.location.origin;
var clients;
var apt_id = 0,applicantId = 0,nextMasterId = 0,INA_INQ_Id = 0;
var JobPostingId = 0, refreshGridId = "";
var JobStatus,IsExempt = "", InterviewerID;
var isInterviewDone = false;
let ManagerList;
var JobTitle;
refreshGridId = "ListMyOpening";
var $_LocationId = $("#drp_MasterLocation1 option:selected").val();
var $_OperationName = "", $_workRequestAssignmentId = 0, $_UserId = 0, $_RequestedBy = 0;//= $("#drp_MasterLocation option:selected").val();
debugger
OnBoardingSummery();
function myOpenings(PostingId) {
    refreshGridId = "ListMyOpening";
    $("#CancelGridList").hide();
    //$("#ListMyOpening").show();
    $("#Scheduler").hide();
    
    $("#ListMyOpening").show();
    $("#companyOpening").hide();
    $("#myinterview").hide();
    $("#MyOpeningSummery").hide();
    $("#ListOnBoarding").hide();
    $("#ListMyOpening").jsGrid({
        width: "100%",
        height: "430px",
        filtering: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        controller: {
            loadData: function () {
                var deferred = $.Deferred();
                $.ajax({
                    url: base_url + '/NewAdmin/MyOpenings?PostingId=' + PostingId,
                    dataType: 'json',
                    success: function (data) {
                        deferred.resolve(data);
                    }
                });
                return deferred.promise();
            }
        },
        onRefreshing: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide();
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
        },
        
        fields: [
			{
			    title: "Applicant Image", name: "Image", width: 30,
			    itemTemplate: function (value) {
			        return $("<div>").append($("<img>").addClass("client-photo").attr("src", value === '' || value === "null" ? "https://www.w3schools.com/howto/img_avatar.png" : value));
			    }
			},
			{
			    title: "Applicant Name", width: 60, itemTemplate: function (v, i) {
			        return i.FirstName;// + ' ' + i.LastName;
			    }
			},
			{
			    title: "Score", width: 40, name:"Score",
			    itemTemplate: function (v,i) {
			        return i.Score;//$("<i>").addClass("fa fa-star").append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"))
					//	.append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"));
			    }
			},
			{ title: "Phone", width: 60, name: "PhoneNumber" },
			{ title: "Email", width: 60, name: "Email" },
			{ title: "Status", width: 50, name: "Status" },
			//{
			//    title: 'Action', width: 70, itemTemplate: function (value, item) {
			//        return $("<div>").addClass("actionDiv").append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-exclamation-triangle whiteR"))
			//			.append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-gg-circle whiteB"))
			//			.append($("<div>").addClass("action1 inline actionbox").append($("<a>").attr({ "onclick": "TakeInterview(" + JSON.stringify(item) + ")", "href": "#" }).append("<i>").attr({ "style": "color:darkblue" }).addClass("fa fa-diamond whiteGr")))
			//			.append($("<div>").addClass("action1 inline actionbox").append("<i>").addClass("fa fa-buysellads whiteY"))
			//			.append($("<div>").addClass("action1 inline actionboxR").append("<i>").addClass("fa fa-btc white"))
			//        .append($("<div>").addClass("action1 inline actionboxS").append("<a>").attr({ "onclick": "GoToRecruitee(" + JSON.stringify(item) + ")", "href": "#" }).addClass("fa fa-clock-o fa-2x whiteS"));
			//    }
			//},
             {
                 title: "Action", width: 70, css: "text-center", itemTemplate: function (value, item) {
                     var $iconFirst,$iconAddInterviewer, $iconAssessment, $iconAddAssets, $iconBackground, $iconOrientationDone, $iconSendOffer, $iconDiamond, $iconIsInterviewDone, $iconIsOrientation;
                     
                     var $iconSecond = $("<i>").attr({ class: "fa fa-gg-circle whiteB" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                     //var $iconDiamond = $("<i>").attr({ class: "fa fa-diamond" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });                                         
                     var $iconViewApplicant = $("<i>").attr({ class: "fa fa-list list-icon" }).attr({ style: "color:black;font-size:22px;margin-left:8px;" });
                      
                     if (item.Status == "IntervieweSchedule" )//|| item.Status == "AssessmentPass" || item.Status == "Shortlisted") {
                     {
                         $iconAddInterviewer = $("<i>").attr({ class: "fa fa-users list-icon" }).attr({ style: "color:#2b8035;font-size:22px;margin-left:8px;" });
                         $iconDiamond = $("<i>").attr({ class: "fa fa-diamond whiteGr" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" });
                     }
                     if (item.Status == "InterviewCompleted") {
                         $iconIsInterviewDone = $("<i>").attr({ class: "fa fa-check-circle whiteGr" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" });
                         
                     }
                     //if (item.Status == "AssessmentPass" || item.Status == "Shortlisted")
                     if (item.Status == "Shortlisted"){
                         $iconAssessment = $("<i>").attr({ class: "fa fa-buysellads whiteY" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                     }
                     if (item.Status == "Onboarding") {
                         $iconAddAssets = $("<i>").attr({ class: "fa fa-gift" }).attr({ style: "color:#134007;font-size:22px;margin-left:8px;" });
                     }
                     if (item.Status == "OfferAccepted") {
                         $iconFirst = $("<i>").attr({ class: "fa fa-exclamation-triangle whiteR" });
                     }
                     if (item.Status == "AssessmentPass") {//if (item.Status == "Hired") {
                          //$iconBackground = $("<i>").attr({ class: "fa fa-btc white" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" });
                          $iconSendOffer = $("<i>").attr({ class: "fa fa-file-text" }).attr({ style: "color:darkcyan;font-size:22px;margin-left:8px;" });
                     }
                     //if (item.Status == "BackgroundCheckSend") {
                     if (item.status == "Onboarded") {
                         $iconBackground = $("<i>").attr({ class: "fa fa-btc white" }).attr({ style: "color:green;font-size:22px;margin-left:8px;" });
                     }
                     if (item.Status == "OrientationSchedule") {
                         $iconOrientationDone = $("<i>").attr({ class: "fa fa-check-circle" }).attr({ style: "color:orange;font-size:22px;margin-left:8px;" });
                     }
                     else if (item.Status == "BackgroundCheckPass") {
                         $iconIsOrientation = $("<i>").attr({ class: "fa fa-calendar-times-o" }).attr({ style: "color:orange;font-size:22px;margin-left:8px;" });
                     }
                     //var $iconGoTORecruitee = $("<i>").attr({ class: "fa fa-clock-o fa-2x whiteS" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });
                     var $customFirstButton = $("<span>")
                           .attr({ title: jsGrid.fields.control.prototype.hiredEmployeeTooltip })
                           .attr({ id: "btn-first-" + item.ApplicantId }).click(function (e) {
                               HiredApplicant(item.ApplicantId,item.Status);
                         }).append($iconFirst);

                     var $customSecondButton = $("<span>")
                           .attr({ title: jsGrid.fields.control.prototype.HRChallengeEmployeeTooltip })
                           .attr({ id: "btn-second-" + item.ApplicantId }).click(function (e) {
                         }).append($iconSecond);

                     var $customDiamondButton = $("<span>")
                             .attr({ title: jsGrid.fields.control.prototype.InterviewEmployeeTooltip })
                             .attr({ id: "btn-diamond-" + item.ApplicantId }).click(function (e) {
                                 TakeInterview(item);
                             }).append($iconDiamond);

                     var $customUpdateInterviewer = $("<span>")
                          .attr({ title: jsGrid.fields.control.prototype.hiredEmployeeTooltip })
                          .attr({ id: "btn-UpdateInterviewer-" + item.ApplicantId }).click(function (e) {
                              if(IsExempt == "Y"){
                                  $("#btnUpdateInterviewPanel").hide();
                                  $("#btnUpdateInterviewPanelForMyOpening").show();
                                  $("#myModalUpdatePanel").modal('show');
                                  loadInterviewPanelList();
                                  applicantId = item.ApplicantId;
                              }
                              else{
                                  toastr.error("The position is non exempt you dont allow to add interviewer.")
                              }
                          }).append($iconAddInterviewer);

                     var $customAssessmentButton = $("<span>")
                          .attr({ title: jsGrid.fields.control.prototype.AssessmentEmployeeTooltip })
                          .attr({ id: "btn-Assessment-" + item.ApplicantId }).click(function (e) {
                              $.ajax({
                                  type: "POST",
                                  url: base_url + '/NewAdmin/AssessmentStatusChange?Status=' + "E" + "&IsActive=" + "S" + "&ApplicantId=" + item.ApplicantId,
                                  beforeSend: function () {
                                      new fn_showMaskloader('Please wait...');
                                  },
                                  success: function (message) {
                                      if (message != null) {
                                          toastr.success(message);
                                      }
                                      else {
                                          toastr.success(message);
                                      }
                                      $("#myModalForDemotionEmployee").modal("hide");
                                  },
                                  error: function (err) {
                                  },
                                  complete: function () {
                                      fn_hideMaskloader();
                                  }
                              });
                          }).append($iconAssessment);

                     var $customBackgroundButton = $("<span>")
                          .attr({ title: jsGrid.fields.control.prototype.BackgroundEmployeeTooltip })
                          .attr({ id: "btn-Background-" + item.ApplicantId }).click(function (e) {
                              API_id = item.ApplicantId;
                              $("#myModalForAddEmployee").modal("show");
                             
                          }).append($iconBackground);

                     var $customIsInterviewDone = $("<span>")
                              .attr({ title: jsGrid.fields.control.prototype.HRChallengeEmployeeTooltip })
                              .attr({ id: "btn-second-" + item.ApplicantId }).click(function (e) {
                                  ShowInterviewAnswer(item.ApplicantId);
                              }).append($iconIsInterviewDone);
                     var $customAssetsButton = $("<span>")
                         .attr({ title: jsGrid.fields.control.prototype.AssetsButtonTooltip })
                         .attr({ id: "btn-Assets-" + item.ApplicantId }).click(function (e) {                         
                             $.ajax({
                                 type: "POST",
                                 url: base_url + '/NewAdmin/ShowAssetsForApplicant?ApplicantId=' + item.ApplicantId,
                                 beforeSend: function () {
                                     new fn_showMaskloader('Please wait...');
                                 },
                                 success: function (data) {
                                     
                                     if (data != null) {
                                         $("#viewAssetsData").html("");
                                         $("#viewAssetsData").html(data);
                                         
                                     }
                                     else {
                                         $("#viewAssetsData").html("No assets to add....!");
                                     }
                                     $("#ModalForAssetsAdd").modal('show');
                                 },
                                 error: function (err) {
                                 },
                                 complete: function () {
                                     fn_hideMaskloader();
                                 }
                             });
                         }).append($iconAddAssets);
                     var $customOfferButton = $("<span>")
                          .attr({ title: jsGrid.fields.control.prototype.OfferButtonTooltip })
                          .attr({ id: "btn-offer-" + item.ApplicantId }).click(function (e) {
                              $.ajax({
                                  type: "POST",
                                  url: base_url + '/NewAdmin/ShowOfferLetter?ApplicantId=' + item.ApplicantId,
                                  beforeSend: function () {
                                      new fn_showMaskloader('Please wait...');
                                  },
                                  success: function (data) {
                                      
                                      if (data != null) {
                                          $("#viewOffer").html("");
                                          $("#viewOffer").html(data);

                                      }
                                      else {
                                          $("#viewOffer").html("No assets to add....!");
                                      }
                                      $("#ModalForOfferLetter").modal('show');
                                  },
                                  error: function (err) {
                                  },
                                  complete: function () {
                                      fn_hideMaskloader();
                                  }
                              });
                          }).append($iconSendOffer);
                     var $customViewApplicantButton = $("<span>")
                         .attr({ title: jsGrid.fields.control.prototype.listButtonTooltip })
                         .attr({ id: "btn-first-" + item.ApplicantId }).click(function (e) {
                             applicantId = item.ApplicantId;
                             ViewApplicantDetails(item.ApplicantId,item.Status)
                         }).append($iconViewApplicant);

                     var $customBakcGroundSentButton = $("<span>")
                         .attr({ title: jsGrid.fields.control.prototype.BackdroundSendButtonTooltip })
                         .attr({ id: "btn-first-" + item.ApplicantId }).click(function (e) {
                             applicantId = item.ApplicantId;
                             $("#ModalForPassFailBackground").modal("show");
                         }).append($iconBackground);

                     var $customSetOrientationButton = $("<span>")
                         .attr({ title: jsGrid.fields.control.prototype.OriantationScheduleButtonTooltip })
                         .attr({ id: "btn-orientation-" + item.ApplicantId }).click(function (e) {
                             applicantId = item.ApplicantId;
                             $("#myModalToAddEmployeeOrietation").modal('show');
                         }).append($iconIsOrientation);

                     var $customOriantationDoneButton = $("<span>")
                         .attr({ title: jsGrid.fields.control.prototype.OriantationDoneButtonTooltip })
                         .attr({ id: "btn-orientationDone-" + item.ApplicantId }).click(function (e) {
                             applicantId = item.ApplicantId;
                             $("#ModalForOriantationDone").modal('show');
                         }).append($iconOrientationDone);
                     

                     return $("<div>").attr({ class: "btn-toolbar" }).append($customFirstButton).append($customSecondButton)
                         .append($customDiamondButton).append($customUpdateInterviewer)
                         .append($customAssessmentButton)
                         .append($customBackgroundButton)
                         .append($customAssetsButton)
                         .append($customOfferButton)
                         .append($customViewApplicantButton)
                         .append($customIsInterviewDone)
                         .append($customSetOrientationButton)
                         .append($customBakcGroundSentButton)
                         .append($customOriantationDoneButton);
                 }
             }
        ]
    });


    String.prototype.capitalize = function () {
        return this.charAt(0).toUpperCase() + this.slice(1);
    };


}
function MyInterviews() {
    refreshGridId = "myinterviews";
    $("#CancelGridList").hide();
    //$("#myinterview").show();
    $("#Scheduler").hide();
    $("#ListMyOpening").hide();
    $("#companyOpening").hide();
    $("#myinterview").show();
    $("#MyOpeningSummery").hide();
    $("#ListOnBoarding").hide();
    $("#myinterviews").jsGrid({
        width: "100%",
        height: "300px",
        filtering: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function () {
                var deferred = $.Deferred();

                $.ajax({
                    url: base_url + '/NewAdmin/MyInterviews',
                    dataType: 'json',
                    success: function (data) {
                        deferred.resolve(data);
                    }
                });

                return deferred.promise();
            }
        },
        onRefreshed: function (args) {           
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
        },
        fields: [
			{
			    title: "Applicant Image", name: "Image", width: 60,
			    itemTemplate: function (value) {
			        return $("<div>").append($("<img>").addClass("client-photo").attr("src", value === '' || value === "null" ? "https://www.w3schools.com/howto/img_avatar.png" : value));
			    }
			},
			{
			    title: "Applicant Name", width: 60, itemTemplate: function (v, i) {
			        return i.FirstName + ' ' + i.LastName;
			    }
            },
            { title: "JobTitle", width: 60, name: "Job Title" },
			{
			    title: "Rating", width: 60,
			    itemTemplate: function () {
			        return $("<i>").addClass("fa fa-star").append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"))
						.append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"));
			    }
			},
			{ title: "Phone", width: 60, name: "PhoneNumber" },
			{ title: "Email", width: 60, name: "Email" },
			{ title: "Status", width: 60, name: "Status" },
			{
			    title: "Action", width: 60, itemTemplate: function (value, item) {                    
			        return $("<div>")
						.append($("<div>").addClass("action1 inline actionbox").append($("<a>").attr({ "onclick": "TakeInterview(" + JSON.stringify(item) + ")", "href": "#" }).append("<i>").attr({ "style": "color:darkblue" }).addClass("fa fa-diamond white")));
			    }
			}
        ]
    });


    String.prototype.capitalize = function () {
        return this.charAt(0).toUpperCase() + this.slice(1);
    };


}
function MyOpeningSummery() {
    refreshGridId = "MyOpeningSummery";
    $("#CancelGridList").show();
    $("#Scheduler").hide();
    $("#btnBack").hide();
    //$("#ListMyOpening").hide();
    //$("#MyOpeningSummery").show();
    $("#ListOnBoarding").hide();
    $("#ListMyOpening").hide();
    $("#companyOpening").hide();
    $("#myinterview").hide();
    $("#MyOpeningSummery").show();
    $("#MyOpeningSummery").jsGrid({
        width: "100%",
        height: "300px",
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,
        controller: {
            loadData: function () {
                var d = $.Deferred();

                $.ajax({
                    url: base_url + "/NewAdmin/GetJobPostong",
                    dataType: "json"
                }).done(function (response) {
                    d.resolve(response);
                });

                return d.promise();
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
            $(".HoldJob").attr({ style: "color:red;"});

        },
        rowClass: function (item, itemIndex) //item is the data in a row, index is the row number.
        {
            if (item.Status == "Hold") {
                return "HoldJob";
            }
        },
        fields: [
			{ name: "JobTitle", type: "text", title:"Job Title", width: 300 },
			//{ name: "Employee", type: "text", width: 70 },
            { name: "PositionCount",title:"Position Count", type: "number", width: 70 },
			{ name: "Applicant", type: "number", width: 65 },
			{ name: "DatePosted",title:"Date Posted", type: "text", width: 60 },
			{
			    name: "Duration", type: "text", width: 50,
			    itemTemplate: function (value) {
			        return $("<span>").text(value + ' ' + 'Days');
			    }
			},
			{ name: "Status", type: "text", width: 50 },
			{
			    name: "Action", width: 125, align: "center", Title: "Action",
                itemTemplate: function (value, item) {
                    var $DonutIcon = $("<div>").attr({ id: "detailDiv" }).append('<div class="inlineDivdonut"><img src="Images/donut.png" class="donutC">"');
                    var $iconOpenCalendar = $("<i>").attr({ class: "fa fa-clock-o fa-2x whiteS" }).attr({ style: "color:orange;font-size:22px;margin-left:8px;" });                    
                    var $customOpenCalendarButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.ScheduleEmployeeTooltip })
                        .attr({ id: "btn-Recruitee-" + item.Employee }).click(function (e) {
                            e.stopPropagation();
                            //window.location.href = "https://app.recruitee.com/#/settings/scheduler";
                            //$("#ModalScheduleInterview").modal('show');
                            $("#JobPostBackBtn").hide();
                            $("#MyOpeningSummery").hide();
                            $("#Scheduler").show();
                            $("#JobId").val(item.JobPostingId);
                            //$("#lblApplicantId").text(item.Employee);
                            //$("#lblApplicantName").text(item.FirstName);
                            //$("#lblApplicantEmail").text(item.Email);
                           // getslots();
                            loadCalendar(item.Employee);
                        }).append($iconOpenCalendar);
                    var $iconUpdatePanel = $("<i>").attr({ class: "fa fa-users fa-2x whiteS" }).attr({ style: "color:#2b8035;font-size:22px;margin-left:8px;" });
                    var $customUpdatePanelButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.UpdatePanel })
                        .attr({ id: "btn-Recruitee-" + item.Employee }).click(function (e) {
                            e.stopPropagation();
                            $("#JobId").val(item.JobPostingId);
                            $("#myModalUpdatePanel").modal('show');
                            IsExempt = item.IsExempt;
                            //loadInterviewPanel();
                            $("#btnUpdateInterviewPanel").show();
                            $("#btnUpdateInterviewPanelForMyOpening").hide();
                            loadInterviewPanelList();
                            
                            JobTitle = item.JobTitle;
                        }).append($iconUpdatePanel);
                    var $iconJobHoldStatus = $("<i>").attr({ class: "fa fa-hourglass-half whiteS" }).attr({ style: "color:blue;font-size:22px;margin-left:8px;" });

                    var $customJobHoldButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.HoldJobButtonTooltip })
                        .attr({ id: "btn-HoldJob-" + item.Employee }).click(function (e) {
                            
                            $("#ModalForJobStatusChange").modal("show");
                            JobPostingId = item.JobPostingId;
                            JobStatus = "H";
                            //CloseHoldJob(item.JobId,"H");
                            $("#HoldOpenCloseJobId").html("");
                            $("#HoldOpenCloseJobId").html("Do you realy want to hold " +item.JobTitle+ " this job.")
                            e.stopPropagation();
                        }).append($iconJobHoldStatus);
                    var $iconJobCloseStatus = $("<i>").attr({ class: "fa fa-window-close whiteS" }).attr({ style: "color:red;font-size:22px;margin-left:8px;" });

                    var $customJobCloseButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.CloseJobButtonTooltip })
                        .attr({ id: "btn-CloseJob-" + item.Employee }).click(function (e) {
                            $("#ModalForJobStatusChange").modal("show");
                            JobPostingId = item.JobPostingId;
                            JobStatus = "C";
                            $("#HoldOpenCloseJobId").html("");
                            $("#HoldOpenCloseJobId").html("Do you realy want to close " + item.JobTitle + " this job.")
                            e.stopPropagation();
                            //CloseHoldJob(item.JobId, "C");
                        }).append($iconJobCloseStatus);

                    var $iconJobOpenStatus = $("<i>").attr({ class: "fa fa-suitcase whiteS" }).attr({ style: "color:gray;font-size:22px;margin-left:8px;" });

                    var $customJobOpenButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.OpenJobButtonTooltip })
                        .attr({ id: "btn-CloseJob-" + item.Employee }).click(function (e) {
                            $("#ModalForJobStatusChange").modal("show");
                            JobPostingId = item.JobPostingId;
                            JobStatus = "Y";
                            $("#HoldOpenCloseJobId").html("");
                            $("#HoldOpenCloseJobId").html("Do you realy want to open " + item.JobTitle + " this job.")
                            e.stopPropagation();
                            //CloseHoldJob(item.JobId, "C");
                        }).append($iconJobOpenStatus);

                    var $DonutButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.OpenJobButtonTooltip })
                        .attr({ id: "btn-DonutButton-" + item.Employee }).click(function (e) {
                            debugger
                            $.ajax({

                                url: base_url + '/NewAdmin/GetApplicantJobSummary',
                                type: 'GET',
                                data: {
                                    JobPostingId: item.JobPostingId,

                                },
                                success: function (countData) {
                                    debugger
                                    var arrData = [];
                                    //var arrRequisiotionData = [];
                                    if (countData.length > 0 && countData != null) {
                                        for (var i = 0; i < countData.length; i++) {
                                            if (countData[i].StatusApplied > 0) {
                                                arrData.push({
                                                    "Applicant Status": "Applied", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusApplied
                                                });
                                            }
                                            if (countData[i].StatusScreened > 0) {
                                                arrData.push({
                                                    "Applicant Status": "Screened", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusScreened
                                                });
                                            }
                                            if (countData[i].StatusInterviewSchedule > 0) {
                                                arrData.push({
                                                    "Applicant Status": "InterviewSchedule", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusInterviewSchedule
                                                });
                                            }
                                            if (countData[i].StatusInterviewCanceled > 0) {
                                                arrData.push({
                                                    "Applicant Status": "InterviewCanceled", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusInterviewCanceled
                                                });
                                            }
                                            if (countData[i].StatusShortListed > 0) {
                                                arrData.push({
                                                    "Applicant Status": "ShortListed", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusShortListed
                                                });
                                            }
                                            if (countData[i].StatusAssessmentSend > 0) {
                                                arrData.push({
                                                    "Applicant Status": "AssessmentSend", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusAssessmentSend
                                                });
                                            }
                                            if (countData[i].StatusAssessmentPass > 0) {
                                                arrData.push({
                                                    "Applicant Status": "AssessmentPass", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusAssessmentPass
                                                });
                                            }
                                            if (countData[i].StatusAssessmentFailed > 0) {
                                                arrData.push({
                                                    "Applicant Status": "AssessmentFailed", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusAssessmentFailed
                                                });
                                            }
                                            if (countData[i].StatusOnhold > 0) {
                                                arrData.push({
                                                    "Applicant Status": "Onhold", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOnhold
                                                });
                                            }
                                            if (countData[i].StatusHired > 0) {
                                                arrData.push({
                                                    "Applicant Status": "Hired", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusHired
                                                });
                                            }
                                            if (countData[i].StatusOfferSent > 0) {
                                                arrData.push({
                                                    "Applicant Status": "OfferSent", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOfferSent
                                                });
                                            }
                                            if (countData[i].StatusOfferAccepted > 0) {
                                                arrData.push({
                                                    "Applicant Status": "OfferAccepted", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOfferAccepted
                                                });
                                            }
                                            if (countData[i].StatusOfferCountered > 0) {
                                                arrData.push({
                                                    "Applicant Status": "OfferCountered", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOfferCountered
                                                });
                                            }
                                            if (countData[i].StatusOfferDeclined > 0) {
                                                arrData.push({
                                                    "Applicant Status": "OfferDeclined", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOfferDeclined
                                                });
                                            }
                                            if (countData[i].StatusOfferCanceled > 0) {
                                                arrData.push({
                                                    "Applicant Status": "OfferCanceled", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOfferCanceled
                                                });
                                            }
                                            if (countData[i].StatusOnboarding > 0) {
                                                arrData.push({
                                                    "Applicant Status": "Onboarding", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOnboarding
                                                });
                                            }
                                            if (countData[i].StatusOnboarded > 0) {
                                                arrData.push({
                                                    "Applicant Status": "Onboarded", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOnboarded
                                                });
                                            }
                                            if (countData[i].StatusOnboardingDrop > 0) {
                                                arrData.push({
                                                    "Applicant Status": "OnboardingDrop", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOnboardingDrop
                                                });
                                            }
                                            if (countData[i].StatusBackgroundCheckSend > 0) {
                                                arrData.push({
                                                    "Applicant Status": "BackgroundCheckSend", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusBackgroundCheckSend
                                                });
                                            }
                                            if (countData[i].StatusBackgroundCheckPass > 0) {
                                                arrData.push({
                                                    "Applicant Status": "BackgroundCheckPass", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusBackgroundCheckPass
                                                });
                                            }
                                            if (countData[i].StatusBackgroundCheckFail > 0) {
                                                arrData.push({
                                                    "Applicant Status": "BackgroundCheckFail", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusBackgroundCheckFail
                                                });
                                            }
                                            if (countData[i].StatusOrientationSchedule > 0) {
                                                arrData.push({
                                                    "Applicant Status": "OrientationSchedule", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOrientationSchedule
                                                });
                                            }
                                            if (countData[i].StatusOrientationDone > 0) {
                                                arrData.push({
                                                    "Applicant Status": "OrientationDone", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOrientationDone
                                                });
                                            }
                                            if (countData[i].StatusOrientationNotDone > 0) {
                                                arrData.push({
                                                    "Applicant Status": "OrientationNotDone", /*countData[i].ApplicantStatus,*/
                                                    "Applicant Count": countData[i].StatusOrientationNotDone
                                                });
                                            }


                                        }
                                    }
                                    am4core.ready(function () {
                                        am4core.useTheme(am4themes_material);
                                        am4core.useTheme(am4themes_animated);
                                        var chart = am4core.create("AppJobSummary", am4charts.PieChart);
                                        chart.data = arrData;
                                        var pieSeries = chart.series.push(new am4charts.PieSeries());
                                        pieSeries.dataFields.value = "Applicant Count";
                                        pieSeries.dataFields.category = "Applicant Status";
                                        pieSeries.innerRadius = am4core.percent(50);
                                        pieSeries.ticks.template.disabled = true;
                                        pieSeries.labels.template.disabled = true;
                                        var rgm = new am4core.RadialGradientModifier();
                                        rgm.brightnesses.push(-0.8, -0.8, -0.5, 0, -0.5);
                                        pieSeries.slices.template.fillModifier = rgm;
                                        pieSeries.slices.template.strokeModifier = rgm;
                                        pieSeries.slices.template.strokeOpacity = 0.4;
                                        pieSeries.slices.template.strokeWidth = 0;
                                        chart.legend = new am4charts.Legend();
                                        chart.legend.position = "right";
                                    });


                                }
                            });
                            $("#ModalForApplicantStatusPieChart").modal("show");
                            e.stopPropagation();
                            //CloseHoldJob(item.JobId, "C");
                        }).append($DonutIcon);
                    //Donutchart


                    if (item.Status == "Hold") {
                        return $("<div>").append($DonutButton)//.append($("<div>").addClass("inlineDivdonut").append("<img src='Images/donut.png' class='donutC' onmouseover='GetSummeryDetail(this);' onmouseout='HideDetail(this)'>"))
                            //.append($("<div>").append("<i>").addClass("fa fa-envelope-o fa-lg actionBtn"))
                            .append($("<div>").append("<i>").addClass("fa fa-trash fa-lg actionBtn"))
                            .append($customJobOpenButton);
                    }
                    else {
                        return $("<div>").append($DonutButton)//append($("<div>").addClass("inlineDivdonut").append("<img src='Images/donut.png' class='donutC' onmouseover='GetSummeryDetail(this);' onmouseout='HideDetail(this)'>"))
                            //.append($("<div>").append("<i>").addClass("fa fa-envelope-o fa-lg actionBtn"))
                            //.append($("<div>").append("<i>").addClass("fa fa-trash fa-lg actionBtn"))
                            .append($customOpenCalendarButton).append($customUpdatePanelButton)
                            .append($customJobHoldButton).append($customJobCloseButton);
                    }
			    }
			}
        ],
        rowClick: function (args) {
            console.log(args.item);
            $("#MyOpeningSummery").hide();
            $("#btnBack").show();
            JobPostingId = args.item.JobPostingId;
            IsExempt = args.item.IsExempt;
            JobTitle = args.item.JobTitle;
            myOpenings(args.item.JobPostingId);
        }
    });
}
function GetSummeryDetail(elm, postingId) {
    if ($('.text').is(':visible')) {
        $('.text').hide();
    }
    // Create chart instance
    $("#detailDiv").empty();
    var arrData = [];
    var chart = am4core.create("detailDiv", am4charts.PieChart);
    $.ajax({
        url: base_url + '/NewAdmin/GetHringChartData?postingId='+postingId,
        method: 'POST',
        success: function (response) {
            chart.data = [{
                "country": "Lithuania",
                "litres": response.Applied
            }, {
                "country": "Czech Republic",
                "litres": 301.9
            }, {
                "country": "Ireland",
                "litres": 201.1
            }, {
                "country": "Germany",
                "litres": 165.8
            }, {
                "country": "Australia",
                "litres": 139.9
            }, {
                "country": "Austria",
                "litres": 128.3
            }, {
                "country": "UK",
                "litres": 99
            }, {
                "country": "Belgium",
                "litres": 60
            }, {
                "country": "The Netherlands",
                "litres": 50
            }];
            chart.innerRadius = am4core.percent(50);

            // Add and configure Series
            var pieSeries = chart.series.push(new am4charts.PieSeries());
            pieSeries.dataFields.value = "litres";
            pieSeries.dataFields.category = "country";
            pieSeries.slices.template.stroke = am4core.color("#fff");
            pieSeries.slices.template.strokeWidth = 2;
            pieSeries.slices.template.strokeOpacity = 1;
            // This creates initial animation
            pieSeries.hiddenState.properties.opacity = 1;
            pieSeries.hiddenState.properties.endAngle = -90;
            pieSeries.hiddenState.properties.startAngle = -90;
            $($(elm).parent()).prev().show()
        }
    });
   

    // Add data
    

    // Set inner radius

}
function OnBoardingSummery() {
    debugger
    refreshGridId = "ListOnBoarding";
    $("#ListOnBoarding").show();
    $("#CancelGridList").hide();
    //$("#myinterview").show();
    $("#Scheduler").hide();
    $("#ListMyOpening").hide();
    $("#companyOpening").hide();
    $("#myinterview").hide();
    $("#MyOpeningSummery").hide();
   
    $("#ListOnBoarding").jsGrid({
        width: "100%",
        height: "300px",
        filtering: true,
        sorting: true,
        paging: true,
        autoload: true,
        pageSize: 10,
        pageButtonCount: 5,

        controller: {
            loadData: function () {
                var deferred = $.Deferred();

                $.ajax({
                    url: base_url + '/NewAdmin/OnboardingList',
                    dataType: 'json',
                    success: function (data) {
                        deferred.resolve(data);
                    }
                });

                return deferred.promise();
            }
        },
        onRefreshed: function (args) {
            $(".jsgrid-insert-row").hide();
            $(".jsgrid-filter-row").hide()
            $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
            $("#ListOnBoarding").show();
        },
        fields: [
            {
                title: "Applicant Image", name: "Photo", width: 60,
                itemTemplate: function (val, item) {
                    return $("<img>").attr("src", val).css({ height: 50, width: 50, "border-radius": "50px" })
                    //return $("<div>").append($("<img>").addClass("client-photo").attr("src", value.Photo === '' || value.Photo === "null" ? "https://www.w3schools.com/howto/img_avatar.png" : value.Photo));
                }
            },
            {
                title: "Applicant Name", width: 60, itemTemplate: function (v, i) {
                    return i.FirstName;//+ ' ' + i.LastName;
                }
            },
            { title: "JobTitle", width: 60, name: "Job Title" },
            {
                title: "Rating", width: 60,
                itemTemplate: function () {
                    return $("<i>").addClass("fa fa-star").append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"))
                        .append($("<i>").addClass("fa fa-star")).append($("<i>").addClass("fa fa-star"));
                }
            },
            { title: "Phone", width: 60, name: "PhoneNumber" },
            { title: "Email", width: 60, name: "Email" },
            { title: "Status", width: 60, name: "Status" },
            {
                title: "Action", width: 60, itemTemplate: function (value, item) {
                   
                        return $("<div>")
                            .append($("<div>").addClass("action1 inline actionbox").append($("<a>").attr({ "onclick": "PDFDownload(" + JSON.stringify(item) + ")", "href": "#" }).append("<i>").attr({ "style": "color:red" }).addClass("fa fa-file-pdf-o white")));
                    
                }
            }
        ]
    });


    String.prototype.capitalize = function () {
        return this.charAt(0).toUpperCase() + this.slice(1);
    };


}
function HideDetail(elm) {
    $("#detailDiv").empty();
    $($(elm).parent()).prev().hide()
}
function JobPosting() {
    $("#ListMyOpening").hide();
    MyOpeningSummery();
}
function TakeInterview(item) {
    $.ajax({
        url: base_url + '/NewAdmin/InfoFactSheet',
        method: 'POST',
        data: item,
        success: function (response) {
            $('#infofactsheet').empty();
            $("#infofactsheet").html(response);
        }
    });
}
function GetInterviewers(elm, applicantId) {
    apt_id = applicantId;
    $(elm).removeClass("btnNotSelected");
    $(elm).addClass("btnSelected");
    $(elm.parentElement).find('button').each(function (index, element) {
        if (element != elm && !$(element).hasClass('dropdown-toggle')) {
            $(element).removeClass("btnSelected");
            $(element).addClass("btnNotSelected");
        }
    });
    $("#interviewArea").empty();
    $.ajax({
        url: base_url + '/NewAdmin/GetInterviewers?applicantId=' + applicantId,
        method: 'GET',
        success: function (response) {
            $("#interviewArea").html(response);
        }
    });
}
function RefreshInterview(applicantId){
    $("#interviewArea").empty();
    $.ajax({
        url: base_url + '/NewAdmin/GetInterviewers?applicantId=' + applicantId,
        method: 'GET',
        success: function (response) {
            $("#interviewArea").html(response);
        }
    });
}
function GetInterviewQuestions() {
    $("#interviewArea").empty();
    $.ajax({
        url: base_url + '/NewAdmin/GetInterviewQuestionView',
        method: 'GET',
        success: function (response) {
            Getquestions(null);
            $("#interviewArea").html(response);
        }
    });
}
function GetTypeInterview() {
    $("#interviewArea").empty();
    $.ajax({
        url: base_url + '/NewAdmin/CheckForTypeInterview?id=' + apt_id,
        method: 'GET',
        //data: { id: id },
        success: function (innerResponse) {
            $("#interviewArea").html(innerResponse);
        }
    });
}
function InterviewCancel(EmployeeLoginId,ApplicantId){
    InterviewerID = EmployeeLoginId;
    applicantId = ApplicantId;
    $("#ModalForCancelInterview").modal('show');
}
function CancelInterviewByInterviewer(){
    var comment  = $("#CommentCancelInterview").val();
    $.ajax({
        url: base_url + '/NewAdmin/CancelInterview?InterviewerID=' + InterviewerID + "&ApplicantId=" + applicantId + "&Comment=" + comment,
        method: 'POST',
        success: function (innerResponse) {
            $("#ModalForCancelInterview").modal('hide');
            $(".modal-backdrop").css("position","inherit");
            if(innerResponse){

                var link = base_url + '/NewAdmin/HiringOnBoardingDashboard';
                $("#RenderPageId").load(link);
            }
            else{
                toastr.success("something went wrong, please try it again..!");
            }
        }
    });
}
function Getquestions(id) {
    var ApplicantId = $("#applicant_id").val();
    $.ajax({
        url: base_url + '/NewAdmin/GetInterviewQuestions',
        method: 'POST',
        data: { id: id, Applicant: ApplicantId },
        success: function (innerResponse) {
            $("#questionArea").empty();
            $("#questionArea").html(innerResponse);
            $("#qusOpt").val('')
        }
    });
}
function CheckeForInterviewType(type, isExempt) {
    if (type == "Interview") {
        $.ajax({
            url: base_url + '/NewAdmin/GetInterviewQuestionView?isExempt=' + isExempt,
            method: 'GET',
            success: function (response) {
                Getquestions(null);
                $("#interviewArea").html(response);
            }
        });
    }
}
var setInputAnswer = "";
var AnswerArr = [];
//======================For Question Answer Functionality======================
//$("#QuestionYes").click(function(event){
function QuestionYes($_this, isFinal) {
    //$_this = this; 
    var questionId = $($_this).attr("questionId");
    $($_this).attr({ "style": "background-color:#0991A9" })
    var masterId = $($_this).attr("masterId");
    //if (isFinal == false) {
    //    var getCommentId = $_this.nextElementSibling.nextElementSibling.className;
    //    //setInputAnswer += "Y";
    //    //$("#selectAns").val('Y');
    //    setInputAnswer += "N" + ',';
    //    $("#selectAns").val(setInputAnswer);
    //    if (masterId == 1 && questionId == 2) {
    //        $("." + getCommentId).hide();
    //    }
    //    if (masterId == 1 && questionId == 3) {
    //        $("." + getCommentId).hide();
    //    }
    //    document.querySelector('#commentbox').required = false;
    //    $("#selectAns").val(setInputAnswer);
    //    $("#lblmsg").hide();
    //}
    //  else {
    var isValChecked = false;
    if (AnswerArr.length > 0) {
        for (var i = 0; i < AnswerArr.length; i++) {
            if (questionId == AnswerArr[i].questionId) {
                AnswerArr[i].Answer = 'Y';
                $_this.nextElementSibling.style.backgroundColor = "";
                $_this.nextElementSibling.style.fontSize = "15px";
                $($_this).attr({ "style": "background-color:#0991A9" });
                isValChecked = true;
            }
        }
            if (isValChecked == false) {//if (questionId != AnswerArr[i].questionId) {
                    AnswerArr.push({ questionId: questionId, Answer: 'Y', masterId: masterId });
                    $($_this).attr({ "style": "background-color:#0991A9" });                                             
            }      
        }  
    else {
        AnswerArr.push({ questionId: questionId, Answer: 'Y', masterId: masterId });
        $($_this).attr({ "style": "background-color:#0991A9" });
    }
        $("#AnswerArr").val(AnswerArr);
        setInputAnswer += "Y" + ',';
        $("#selectAns").val(setInputAnswer);
        var getCommentId = $_this.nextElementSibling.nextElementSibling.className;       
        if (masterId == 1 && questionId == 2) {                       
            $("."+getCommentId).show();
        }
        if (masterId == 1 && questionId == 3) {
            $("."+getCommentId).show();
        }
        if (masterId == 2 && questionId == 4) {
            $("." + getCommentId).show();
            $("#comment1Select").show();
        }
        if (masterId == 2 && questionId == 6) {
            $("." + getCommentId).hide();
        }
        if (masterId == 3 && questionId == 7) {
            $("." + getCommentId).show();
        }
        if (masterId == 3 && questionId == 8) {
            $("." + getCommentId).show();
        }
        if (masterId == 3 && questionId == 10) {
            $("." + getCommentId).show();
        }
        if (masterId == 3 && questionId == 11) {
            $("." + getCommentId).show();
        }
        if (masterId == 4 && questionId == 12) {
            $("." + getCommentId).show();
        }
        if (masterId == 4 && questionId == 13) {
            $("." + getCommentId).show();
        }
        if (masterId == 4 && questionId == 15) {
            $("." + getCommentId).show();
        }
        if (masterId == 4 && questionId == 16) {
            $("." + getCommentId).show();
        }
        if (masterId == 5 && questionId == 22) {
            $("." + getCommentId).show();
        }
        if (masterId == 5 && questionId == 23) {
            $("." + getCommentId).show();
        }
        if (masterId == 6 && questionId == 26) {
            $("." + getCommentId).show();
        }
        document.querySelector('#commentboxlast').required = true;
    //}
    //event.preventdefault();
    //SaveAnswer(function (data) { });
    }
function QuestionNo($_this, isFinal) {
    var questionId = $($_this).attr("questionId");
    $($_this).attr({ "style": "background-color:#0991A9" });
    var masterId = $($_this).attr("masterId");
    var isValChecked = false;
    if (isFinal == false) {
        if (AnswerArr.length > 0) {
            for (var i = 0; i < AnswerArr.length; i++) {
                if (questionId == AnswerArr[i].questionId) {
                    AnswerArr[i].Answer = 'N';
                    $_this.previousElementSibling.style.backgroundColor = "";
                    $_this.previousElementSibling.style.fontSize = "15px";
                    $($_this).attr({ "style": "background-color:#0991A9" })
                    isValChecked = true;
                }
            }
            if (isValChecked == false) {//if ($.inArray(questionId, AnswerArr)) {  
                        AnswerArr.push({ questionId: questionId, Answer: 'N', masterId: masterId });
                        $($_this).attr({ "style": "background-color:#0991A9" });                   
                }
                //else {
                //    AnswerArr.push({ questionId: questionId, Answer: 'N', masterId: masterId });
                //    $($_this).attr({ "style": "background-color:#0991A9" })
                //}
        }
            else {
                AnswerArr.push({ questionId: questionId, Answer: 'N', masterId: masterId });
                $($_this).attr({ "style": "background-color:#0991A9" })
        }
    }
        else {
            AnswerArr.push({ questionId: questionId, Answer: 'N', masterId: masterId })
            $($_this).attr({ "style": "background-color:#0991A9" });
            //isFinal = false;
        }
        //AnswerArr.push({ questionId: questionId, Answer: 'N', masterId: masterId })
        $("#AnswerArr").val(AnswerArr);
        setInputAnswer += "N" + ',';
        $("#selectAns").val(setInputAnswer);
        //var getCommentId = $_this.nextElementSibling.nextElementSibling.className;
        var getCommentId = $_this.nextElementSibling.className;
        //setInputAnswer += "Y";
        //$("#selectAns").val('Y');
        
        if (masterId == 1 && questionId == 2) {
            $("." + getCommentId).hide();
        }
        if (masterId == 1 && questionId == 3) {
            $("." + getCommentId).hide();
        }
        if (masterId == 2 && questionId == 4) {
           
            $("#comment1Select").hide();
        }
        if (masterId == 2 && questionId == 6) {
            $("." + getCommentId).show();
        }
        if (masterId == 3 && questionId == 7) {
            $("." + getCommentId).hide();
        }
        if (masterId == 3 && questionId == 8) {
            $("." + getCommentId).hide();
        }
        if (masterId == 3 && questionId == 10) {
            $("." + getCommentId).hide();
        }
        if (masterId == 3 && questionId == 11) {
            $("." + getCommentId).hide();
        }
        if (masterId == 4 && questionId == 12) {
            $("." + getCommentId).hide();
        }
        if (masterId == 4 && questionId == 13) {
            $("." + getCommentId).hide();
        }
        if (masterId == 4 && questionId == 15) {
            $("." + getCommentId).hide();
        }
        if (masterId == 4 && questionId == 16) {
            $("." + getCommentId).hide();
        }
        if (masterId == 5 && questionId == 22) {
            $("." + getCommentId).hide();
        }
        if (masterId == 5 && questionId == 23) {
            $("." + getCommentId).hide();
        }
        if (masterId == 6 && questionId == 26) {
            $("." + getCommentId).hide();
        }
        document.querySelector('#commentbox').required = false;
        $("#selectAns").val(setInputAnswer);
        $("#lblmsg").hide();
    }
    //e.preventdefault();
//======================For Question Answer functionality======================

function RecordYes(isFinal) {
	if (!isFinal) {
		$("#selectAns").val('Y');
		document.querySelector('#commentbox').required = false;
		$("#lblmsg").hide();
	}
	else {
		$("#selectAns").val('Y');
		document.querySelector('#commentboxlast').required = true;
	}
	//SaveAnswer(function (data) { });

}
function RecordNo(isFinal) {
	if (!isFinal) {
		$("#selectAns").val('N');
		$("#commentbox").prop('required', true);
	} else {
		$("#selectAns").val('N');
		document.querySelector('#commentboxlast').required = false;
		$("#lblmsglast").hide();
	}
	//SaveAnswer(function (data) { });
}
function SaveAndNext() {
     INA_INQ_Id = $("#q_id").val();
    var INA_API_ApplicantId = $("#applicant_id").val();

        var formObject = $("#getQuestionAnswerData").serialize();
    //var dataObjetc = JSON.stringify(formObject)
        if(AnswerArr.length > 0){
            var myJSON = JSON.stringify(AnswerArr);
    
            var ApplicantId = $("#applicant_id").val();
            var INA_EMP_EmployeeID = '';
            var Comment1 = $("#Comment1").val();
            var Comment12 = $("#Comment12").val();
            var Comment1_id = $("#Comment1").attr("questionId");
            var Comment2 = $("#Comment2").val();
            var Comment2_id = $("#Comment2").attr("questionId");
            var Comment3 = $("#Comment3").val();
            var Comment3_id = $("#Comment31").attr("questionId");
            var Comment31 = $("#Comment31").val();
            var Comment32 = $("#Comment32").val();
            ///==============For Question 3==================================
            var Comment24 = $("#comment1Select option:selected").val();
            var Comment37_1_id = $("#Comment37_1").attr("questionId");
            var Comment37_1 = $("#Comment37_1").val();
            var Comment37_2_id = $("#Comment37_2").attr("questionId");
            var Comment37_2 = $("#Comment37_2").val();
            var Comment37_3_id = $("#Comment37_3").attr("questionId");
            var Comment37_3 = $("#Comment37_3").val();
            var Comment38_1_id = $("#Comment38_1").attr("questionId");
            var Comment38_1 = $("#Comment38_1").val();
            var Comment38_2_id = $("#Comment38_2").attr("questionId");
            var Comment38_2 = $("#Comment38_2").val();
            var Comment310_1_id = $("#Comment310_1").attr("questionId");
            var Comment310_1 = $("#Comment310_1").val();
            var Comment311_1_id = $("#Comment311_1").attr("questionId");
            var Comment311_1 = $("#Comment311_1").val();
            //var  = $("#Comment24").val();
            var Comment24_id = $("#comment1Select").attr("questionId");
            //================End Question 3===================================

            //=================For Question 4===================================
            var Comment412_1_id = $("#Comment412_1").attr("questionId");
            var Comment412_1 = $("#Comment412_1").val();
            var Comment412_2_id = $("#Comment412_2").attr("questionId");
            var Comment412_2 = $("#Comment412_2").val();
            var Comment413_1_id = $("#Comment413_1").attr("questionId");
            var Comment413_1 = $("#Comment413_1").val();
            var Comment415_1_id = $("#Comment415_1").attr("questionId");
            var Comment415_1 = $("#Comment415_1").val();
            var Comment416_1_id = $("#Comment416_1").attr("questionId");
            var Comment416_1 = $("#Comment416_1").val();
            var Comment416_2_id = $("#Comment416_2").attr("questionId");
            var Comment416_2 = $("#Comment416_2").val();
            //=================End Question 4===================================

            //=================Start Question 5=================================
            var Comment522_1_id = $("#Comment522_1").attr("questionId");
            var Comment522_1 = $("#Comment522_1").val();
            var Comment523_1_id = $("#Comment523_1").attr("questionId");
            var Comment523_1 = $("#Comment523_1").val();
            //-==================End Question 5 ================================
            //====================Start Question 6==============================
            var Comment626_1_id = $("#Comment626_1").attr("questionId");
            var Comment626_1 = $("#Comment626_1").val();
            //====================End Question 6================================
            for (i = 0; i < AnswerArr.length; i++) {
                if (AnswerArr[i].questionId == Comment1_id) {
                    if (Comment1 != null && Comment1 != undefined && Comment1 != "") {
                        AnswerArr[i].Comment += Comment1 + "|";
                    }
                    if (Comment12 != null && Comment12 != undefined && Comment12 != "") {
                        AnswerArr[i].Comment += Comment12 + "|";
                    }
                }
                if (AnswerArr[i].questionId == Comment2_id) {
                    if (Comment2 != null && Comment2 != undefined && Comment2 != "") {
                        AnswerArr[i].Comment = Comment2;
                    }
                }
                if (AnswerArr[i].questionId == Comment3_id) {
                    if (Comment3 != null && Comment3 != undefined && Comment3 != "") {
                        AnswerArr[i].Comment += Comment3 + "|";
                    }           
                    if (Comment31 != null && Comment31 != undefined && Comment31 != "") {
                        AnswerArr[i].Comment += Comment31 + "|";
                    }
                    if (Comment31 != null && Comment31 != undefined && Comment31 != "") {
                        AnswerArr[i].Comment += Comment32 + "|";
                    }           
                }
                if (AnswerArr[i].questionId == Comment24_id) {
                    if (Comment24 != null && Comment24 != undefined && Comment24 != "") {
                        AnswerArr[i].Comment += Comment24 ;
                    }
                }
                //==============Que 3
                if (AnswerArr[i].questionId == Comment37_1_id) {
                    if (Comment37_1 != null && Comment37_1 != undefined && Comment37_1 != "") {
                        AnswerArr[i].Comment += Comment37_1 +"|";
                    }
                    if (Comment37_2 != null && Comment37_2 != undefined && Comment37_2 != "") {
                        AnswerArr[i].Comment += Comment37_2 + "|";
                    }
                    if (Comment37_3 != null && Comment37_3 != undefined && Comment37_3 != "") {
                        AnswerArr[i].Comment += Comment37_3 + "|";
                    }
                }
                if (AnswerArr[i].questionId == Comment38_1_id) {
                    if (Comment38_1 != null && Comment38_1 != undefined && Comment38_1 != "") {
                        AnswerArr[i].Comment += Comment38_1 + "|";
                    }
                    if (Comment38_2 != null && Comment38_2 != undefined && Comment38_2 != "") {
                        AnswerArr[i].Comment += Comment38_2;
                    }
                }
                if (AnswerArr[i].questionId == Comment310_1_id) {
                    if (Comment310_1 != null && Comment310_1 != undefined && Comment310_1 != "") {
                        AnswerArr[i].Comment += Comment310_1 ;
                    }           
                }
                if (AnswerArr[i].questionId == Comment311_1_id) {
                    if (Comment311_1 != null && Comment311_1 != undefined && Comment311_1 != "") {
                        AnswerArr[i].Comment += Comment311_1;
                    }
                }
                //=============End Que 3
                //================Que 4
                if (AnswerArr[i].questionId == Comment412_1_id) {
                    if (Comment412_1 != null && Comment412_1 != undefined && Comment412_1 != "") {
                        AnswerArr[i].Comment += Comment412_1 +"|";
                    }
                    if (Comment412_2 != null && Comment412_2 != undefined && Comment412_2 != "") {
                        AnswerArr[i].Comment += Comment412_2 ;
                    }            
                }
                if (AnswerArr[i].questionId == Comment413_1_id) {
                    if (Comment413_1 != null && Comment413_1 != undefined && Comment413_1 != "") {
                        AnswerArr[i].Comment += Comment413_1 ;
                    }        
                }
                if (AnswerArr[i].questionId == Comment415_1_id) {
                    if (Comment415_1 != null && Comment415_1 != undefined) {
                        AnswerArr[i].Comment += Comment415_1 ;
                    }          
                }
                if (AnswerArr[i].questionId == Comment416_1_id) {
                    if (Comment416_1 != null && Comment416_1 != undefined) {
                        AnswerArr[i].Comment += Comment416_1 + "|";
                    }
                    if (Comment416_2 != null && Comment416_2 != undefined) {
                        AnswerArr[i].Comment += Comment416_2;
                    }
                }
                //================End Que 4
                //================Start Que 5
                if (AnswerArr[i].questionId == Comment522_1_id) {
                    if (Comment522_1 != null && Comment522_1 != undefined) {
                        AnswerArr[i].Comment += Comment522_1;
                    }            
                }
                if (AnswerArr[i].questionId == Comment523_1_id) {
                    if (Comment523_1 != null && Comment523_1 != undefined) {
                        AnswerArr[i].Comment += Comment523_1;
                    }
                }
                if (AnswerArr[i].questionId == Comment626_1_id) {
                    if (Comment626_1 != null && Comment626_1 != undefined) {
                        AnswerArr[i].Comment += Comment626_1;
                    }
                }
                //=================End Que 5
            }
            var MasterId = $("#qMaster_id").val();
            nextMasterId = $("#qMaster_id").val();
            var dataObject = new Object();
            dataObject.Comment2 = Comment2;
            dataObject.Comment31 = Comment31;
            dataObject.Comment32 = Comment32;
            dataObject.ApplicantId = ApplicantId;
            var INA_Comments = $("#commentbox").val();
            //var formObject = $("#getQuestionAnswerData").serialize();
            $.ajax({
                url: base_url + '/NewAdmin/SaveAnswers',
                method: 'POST',
                data: { model: dataObject, AnswerArr: AnswerArr },
                beforeSend: function () {
                    new fn_showMaskloader('Please wait...');
                },
                success: function (getResponse) {
                    if (getResponse == true) {
                        AnswerArr = [];
                        $("#Comment2").val("");
                        $("#Comment3").val("");
                        $("#Comment31").val("");
                        $("#Comment32").val("");
                        nextMasterId = $("#hdn_qusnum").val();
                        toastr.success("Success");
                        var ApplicantId = $("#applicant_id").val();
                        apt_id = $("#applicant_id").val();
                        $("#qus_view_btn").hide();
                        $("#qus_next_btn").show();
                    }
                    else {
                        toastr.success("All Interviewer not saved the data");
                    }
                },
                complete: function () {
                    fn_hideMaskloader();
                }
            });
        }
        else{
            alert("Please select the answer.")
        }
}
function RedirectToApplicantGrid($_this) {
    debugger
    isInterviewDone = true;
    $("#isScheduled").val("true");
    var applicantId = $($_this).attr("applicantid");
    $.ajax({
        url: base_url + '/NewAdmin/InterviewCompleted?ApplicantId=' + applicantId ,
        method: 'POST',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        //data: { ApplicantId: ApplicantId, IsAvailable: IsAvailable, Comment: comment },
        success: function (response) {
            var link = base_url + '/NewAdmin/HiringOnBoardingDashboard';
            $("#RenderPageId").load(link);
            toastr.success("Interview answer saved Successfully");
        },
        complete: function () {
            fn_hideMaskloader();
        }
    })
    
}
function CheckForNext(){
    debugger
    var statusOfNextQuestion = false;
    $.ajax({
        url: base_url + '/NewAdmin/CheckNextQuestion?ApplicantId=' + apt_id + "&QusId=" + nextMasterId,
        method: 'GET',
        //data: { id: nextMasterId },
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (data) {
            statusOfNextQuestion = data;
            if (statusOfNextQuestion) {
                debugger
                $.ajax({
                    url: base_url + '/NewAdmin/GetInterviewQuestions?id=' + nextMasterId + "&Applicant=" + apt_id,
                    method: 'POST',
                    //data: { id: nextMasterId },
                    success: function (innerResponse) {
                        if (nextMasterId == 6) {
                            $("#RenderPageId").html(innerResponse);
                        }
                        else {
                            $("#questionArea").empty();
                            $("#questionArea").html(innerResponse);
                            $("#qusOpt").val('');
                        }
                        $("#qus_view_btn").show();
                        $("#qus_next_btn").hide();
                        
                    }
                });
            }                            
            else {
                alert("All interviewers are not responded yet!!!");
                return;
            }
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
//function SaveAndNext() {
//    debugger;
//    if (CheckIfOptionNotSelected())
//        return;
//    if ($("#commentbox").prop('required')) {
//        var value = $("#commentbox").val();
//        if (value == '' || value == undefined) {
//            $("#lblmsg").show();
//            $("#lblmsg").css("display", "block");
//            return;
//        }
//    }
//    if (SaveAnswer(function (data) {
//		if (data) {
//			$("#commentbox").val('');
//			var INA_API_ApplicantId = $("#applicant_id").val();
			//CheckIfAllResponded(INA_API_ApplicantId, $("#q_id").val(), function (responseForNextQuestion) {
//				if (responseForNextQuestion) {
					//Getquestions(null);
//    }
//    else {
//					alert("All interviewers are not responded yet!!!");
//					return;
//    }
//    })

//    }
//    else {
//			alert('Something went wrong please try again!!! ');
//			return;
//    }


//    }));



//}
//function SaveAnswer(callback) {
//    var INA_INQ_Id = $("#q_id").val();
//    var INA_API_ApplicantId = $("#applicant_id").val();
//    var INA_EMP_EmployeeID = '';
//    var INA_Answer = $("#selectAns").val();
//    var INA_Comments = $("#commentbox").val();
//    console.log(INA_INQ_Id);
//    console.log(INA_API_ApplicantId);
//    console.log(INA_EMP_EmployeeID);
//    console.log(INA_Answer);
//    console.log(INA_Comments);
//    $.ajax({
//        url: base_url + '/NewAdmin/SaveAnswers',
//        method: 'POST',
//        async: false,
//        cache: false,
//        data: { QusId: INA_INQ_Id, ApplicantId: INA_API_ApplicantId, Answer: INA_Answer, Comment: INA_Comments },
//        success: callback
//    });
//}
function GoOnline(elm, empid) {
    var ApplicantId = $("#applicant_id").val();;
    var comment = '';
    var IsAvailable = 'Y';
    $.ajax({
        url: base_url + '/NewAdmin/CanInterviewerIsOnline',
        method: 'POST',
        data: { ApplicantId: ApplicantId, IsAvailable: IsAvailable, Comment: comment },
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (response) {
            if (response == true) {
                $("#icnOnline").hide();
                if ($(elm).is(':checked')) {
                    console.log('This is firing');
                    $('.toggle-off').removeClass('active');
                    $('.toggle-on').addClass('active');
                    $("#" + empid).css("background", "#43D84A");
                } else {
                    $('.toggle-off').addClass('active');
                    $('.toggle-on').removeClass('active');
                    $("#" + empid).css("background", "#fff");
                }
            }
            else {
                $('.toggle-off').addClass('active');
                $('.toggle-on').removeClass('active');
                $("#" + empid).css("background", "#fff");
            }
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
function CheckIfOptionNotSelected() {
    var qus_num = $("#hdn_qusnum").val();
    if (qus_num >= 0) {
        var opt_val = $("#selectAns").val();
        if (opt_val == undefined || opt_val == '' || opt_val == null) {
            alert('Please select an option');
            return true;
        }
    }
}
function GetScore(callback) {
    var ApplicantId = $("#applicant_id").val();
    $.ajax({
        url: base_url + '/NewAdmin/GetScore?ApplicantId=' + ApplicantId,
        method: 'GET',
        success: callback
    });
}
function FinishInterview() {
    if (CheckIfOptionNotSelected())
        return;

    if ($("#commentboxlast").prop('required')) {
        var isCommentRequired = $("#commentboxlast").val();
        if (isCommentRequired == '' || isCommentRequired == undefined || isCommentRequired == null) {
            $("#lblmsglast").show();
            $("#lblmsglast").css("display", "block");
            return;
        }
    }
    if (SaveAnswer(function (data) {
		GetScore(function (response) {
			$("#startInterview").empty();
			$("#startInterview").html("<div class='col-md-12' style='text-align:center'><div class='card cardscore'><label style='color:green'>Total Score:</label><label>" + response + "</label></div><div><label style='font-size:20px;color:white;'>Thanks For The Interview</label><a style='margin:20px;' href='/NewAdmin#' class='btn btn-default btnfinish'>Exit</a></div></div>");
    });
    }));
}
function CheckIfAllResponded(applicantId, qusid, callback) {
    $.ajax({
        url: base_url + '/NewAdmin/CheckNextQuestion?ApplicantId=' + applicantId + "&QusId=" + qusid,
        method: 'GET',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: callback,
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
//TO make 
function MarkAbsent(employeeId,AplicantId,$_this) {
    debugger
    var comment = $("#absentComment").val();
    $("#"+employeeId).css("background-color","red");
    $.ajax({
        url: base_url + '/NewAdmin/MarkAbsentToInterviewer?InterviewerID=' + employeeId + "&ApplicantId=" + AplicantId + "&Comment="+comment,
        method: 'POST',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        //data: { ApplicantId: ApplicantId, IsAvailable: IsAvailable, Comment: comment },
        success: function (response) {
            if(response)
                toastr.success("The Interviewer has been absent successfully...!");  
            else
                toastr.success("Something went wrong please try again...!");
        }           ,
        complete: function () {
            fn_hideMaskloader();
        }
    })
}


function GetCompanyOpening() {
    refreshGridId = "companyOpening";
    $("#CancelGridList").hide();
    $("#Scheduler").hide();
    $("#ListMyOpening").hide();
    $("#myinterview").hide();
    $("#companyOpening").show();
    $("#ListOnBoarding").hide();
    $("#MyOpeningSummery").hide();
    var records = [
        { "JobTitle": "Otto Clay", "PositionCount": 1, "ApplicantCount": 1, "JobPostingDate": "28/12/2019", "Duration": "30", "Status": true}
    ];
	$("#companyOpening").jsGrid({
		width: "100%",
		height: "300px",
		sorting: true,
		paging: true,
		autoload: true,
		pageSize: 10,
		pageButtonCount: 5,
		controller: {
			loadData: function () {
				var d = $.Deferred();

				$.ajax({
					url: "/NewAdmin/GetCompanyOpening",
					dataType: "json"
				}).done(function (response) {
					d.resolve(response);
				});

				return d.promise();
			}
		},
		rowClass: function (item, itemIndex) //item is the data in a row, index is the row number.
		{
		    if (item.OtherInterviewer == $("#LoginManagerId").val()) {
		        return "SecondInterviewer";
		    }
		},
		onRefreshed: function (args) {
            
		    $(".jsgrid-insert-row").hide();
		    $(".jsgrid-filter-row").hide()
		    $(".jsgrid-grid-header").removeClass("jsgrid-header-scrollbar");
		    //if($("#LoginManagerId").val() == ){
		    $(".SecondInterviewer td").attr({ style: "background:midnightblue;color:white;"});
		//}

		},
		fields: [
			{
				title: "Job Title", width: 100, itemTemplate: function (v, i) {
				    return i.JobTitle;
				}
			},
			{
				title: "Position", width: 60, itemTemplate: function (v, i) {
				    return i.PositionCount;
				}
			},
			{
				title: "Applicant", width: 60, itemTemplate: function (v, i) {
				    return i.ApplicantCount;
				}
			},
			{
				title: "Date Posted", width: 60, itemTemplate: function (v, i) {
				    return i.JobPostingDate;
				}
			},
			{
				title: "Duration", width: 60, itemTemplate: function (v, i) {
				    return i.Duration;
				}
			},
			{ name: "Status", type: "text", width: 50 },
			{
			    name: "Action", width: 100, align: "center", Title: "Action",
			    itemTemplate: function (value,item) {
			        var $iconOpenCalendar = $("<i>").attr({ class: "fa fa-clock-o fa-2x whiteS" }).attr({ style: "color:orange;font-size:22px;margin-left:8px;" });                    
			        var $customOpenCalendarButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.ScheduleEmployeeTooltip })
                        .attr({ id: "btn-Recruitee-" + item.Employee }).click(function (e) {
                            e.stopPropagation();
                            //window.location.href = "https://app.recruitee.com/#/settings/scheduler";
                            //$("#ModalScheduleInterview").modal('show');
                            $("#JobPostBackBtn").hide();
                            $("#companyOpening").hide();
                            $("#SchedulerCompany").show();
                            $("#JobId").val(item.JobPostingId);
                            //$("#lblApplicantId").text(item.Employee);
                            //$("#lblApplicantName").text(item.FirstName);
                            //$("#lblApplicantEmail").text(item.Email);
                            // getslots();
                         
                            loadCalendarCompany(item.Employee);
                        }).append($iconOpenCalendar);
			        var $iconUpdatePanel = $("<i>").attr({ class: "fa fa-users fa-2x whiteS" }).attr({ style: "color:#2b8035;font-size:22px;margin-left:8px;" });
			        var $customUpdatePanelButton = $("<span>")
                        .attr({ title: jsGrid.fields.control.prototype.UpdatePanel })
                        .attr({ id: "btn-Recruitee-" + item.Employee }).click(function (e) {
                            e.stopPropagation();
                            $("#JobId").val(item.JobPostingId);
                            $("#myModalUpdatePanel").modal('show');
                            IsExempt = item.IsExempt;
                            loadInterviewPanelList();
                            JobTitle = item.JobTitle;
                        }).append($iconUpdatePanel);

			        if(item.OtherInterviewer != null){
                        return $("<div>").append($("<div id='detailDiv'>").addClass('text').text("asdadfaf")).append($("<div>").addClass("inlineDivdonut").append("<img src='Images/donut.png' class='donutC' onmouseover='GetSummeryDetail(this);' onmouseout='HideDetail(this)'>"))
                            //.append($("<div>").append("<i>").addClass("fa fa-envelope-o fa-lg actionBtn"))
                            //.append($("<div>").append("<i>").addClass("fa fa-trash fa-lg actionBtn"))
                            .append($customOpenCalendarButton)//.append($customUpdatePanelButton)
			        }
			        else{
			            return $("<div>").append($("<div id='detailDiv'>").addClass('text').text("asdadfaf")).append($("<div>").addClass("inlineDivdonut").append("<img src='Images/donut.png' class='donutC' onmouseover='GetSummeryDetail(this);' onmouseout='HideDetail(this)'>"))
                           .append($("<div>").append("<i>").addClass("fa fa-envelope-o fa-lg actionBtn"))
                           .append($("<div>").append("<i>").addClass("fa fa-trash fa-lg actionBtn"))
                          
			        }
                           
			        
			        //return $("<div>").append($("<div id='detailDiv'>").addClass('text').text("asdadfaf")).append($("<div>").addClass("inlineDivdonut").append("<img src='Images/donut.png' class='donutC' onmouseover='GetSummeryDetail(this," + item.JPS_JobPostingId + ");' onmouseout='HideDetail(this)'>"))
					//	.append($("<div>").append("<i>").addClass("fa fa-envelope-o fa-lg actionBtn"))
					//	.append($("<div>").append("<i>").addClass("fa fa-trash fa-lg actionBtn"));
			    }
			}
			
		],
		rowClick: function (args) {
			console.log(args.item);
			$("#MyOpeningSummery").hide();
			$("#btnBack").show();
			myOpenings(args.item.JobPostingId);
		}
	});
}

function GoToRecruitee(item) {
    
}

   function ToAcceptRejectAfterInterview(data) {
        var getAppId = $(data).attr("applicantid");
        var getVal = $("#ToAcceptRejectAfterInterview option:selected").val();
        var getVal = $(data).attr("value");
        $.ajax({
            url: base_url + '/NewAdmin/InterviewAcceptCancel?status=' + getVal + "&ApplicantId=" + getAppId,
            method: 'POST',
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            //data: { ApplicantId: ApplicantId, IsAvailable: IsAvailable, Comment: comment },
            success: function (response) {
                if (getVal == "D") {
                    toastr.success("Applicant has been shortlisted...!");
                } else {
                    toastr.success("Applicant has been reject...!")
                }
                myOpenings(JobPostingId);
                //$("#ListMyOpening").show()
                //$("#ListMyOpening").jsGrid("l")
                $('#RenderPageId').html(response);
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
    }
function getslots(date) {
    $.ajax({
        url: base_url + '/NewAdmin/GetSlotTimings',
        data: {"date":date},
        method: 'POST',
        success: function (optionList) {
            debugger
            $("#eventTime").html('');
            var combo = $("<select class='form-control'></select>");
            combo.append("<option id='" + 0 + "'>--Select Time Slot--</option>");
            $.each(optionList, function (i, el) {
                combo.append("<option id='" + el.SLT_Id + "'>" + el.SLT_fromTime + "</option>");
                console.log(el.SLT_fromTime );
            });
            console.log(combo);
            $("#eventTime").append(combo);
        }
    });

}

function ViewApplicantDetails(ApplicantDetailsId, status) {
    $.ajax({
        url: base_url + '/Guest/GetApplicantDetails?ApplicantId=' + ApplicantDetailsId  ,
        method: 'POST',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (getData) {    
            $("#ApplicantDetailsDiv").html('');
            $("#ApplicantDetailsDiv").html(getData);
            $("#myModalForGetApplicantDetails").modal('show');
            if(status == "Applied"){
                $(".hideShowApplicant").show();
            }
            else{
                $(".hideShowApplicant").hide();
            }
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function AppproveRejectApplicant(isApproved) {
    
    var apt_id = $("#ApplicantIdForView").val();
    $.ajax({
        url: base_url + '/NewAdmin/ScreenedRejectApplicant?ApplicantId=' + apt_id + "&IsScreened=" + isApproved,
        method: 'POST',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (message) {
            $("#myModalForGetApplicantDetails").modal('hide');
            $("#ApplicantDetailsDiv").html('');
           
            myOpenings(JobPostingId);
            toastr.success(message)
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
//var selectedManagers = "";
//var autocomplete;
//function loadInterviewPanel() {
//    selectedManagers = "";
//    $("#inpManageName").html('');
//    $("#inpManageName").html('<span class="autocomplete-select"></span>');
//    $.ajax({
//        type: 'GET',
//        url: '/NewAdmin/GetManagerList/',
//        success: function (response) {
//            debugger
//            if (response.length > 0) {
//                ManagerList = response;
//                autocomplete = new SelectPure(".autocomplete-select", {
//                    options: ManagerList,
//                    multiple: true,
//                    autocomplete: true,
//                    icon: "fa fa-times",
//                    max: 3,
//                    onChange: value => {
//                        selectedManagers = autocomplete.value().toString();
//                    },
//                });
//            }
//        }
//    });

//}
var selectedManagers = "";
var autocomplete;
function loadInterviewPanelList() {
    selectedManagers = "";
    $("#inpManageName").html('');
    $("#inpManageName").html('<span class="autocomplete-select"></span>');
    //$("#inpManageName").html('<select multiple id="e1" style="width:300px">');
    $.ajax({
        url: base_url + '/NewAdmin/GetManagerListForApplicant?IsExempt=' + IsExempt,
        method: 'POST',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (data) {
            if (data.length > 0) {
                ManagerList = data;
                autocomplete = new SelectPure(".autocomplete-select", {
                    options: data,
                    multiple: true,
                    autocomplete: true,
                    icon: "fa fa-times",
                    max: 3,
                    onChange: value => {
                        selectedManagers = autocomplete.value().toString();
                    },
                });
                //$.each(data, function (index, item) {
                //    debugger// item is now an object containing properties ID and Text
                //    //for (var i = 0; i < response.length; i++) {
                //    //    options += '<option value="' + response[i] + '">' + response[i] + '</option>';
                //    //}
                //    $("#e1").append($('<option></option>').text(item.UserName).val(item.UserID));
                //});
            }
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function ShowInterviewAnswer(ApplicantId) {
    $.ajax({
        type: "POST",
        url: base_url + '/NewAdmin/GetApplicantInterviewAnswerDetails?ApplicantId='+ApplicantId,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            $('#RenderPageId').html(result);
        },
        error: function (err) {
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
function UpdateInterviewPanel() {
    
    $.ajax({
        type: 'POST',
        url: "/NewAdmin/UpdateInterviewPanel",
        data: { "selectedManagers": selectedManagers, "JobId": $("#JobId").val()  },
        success: function (response) {
            if (response == true) {
                $("#DisplayMessage").show();
            }
            else {
                $("#DisplayMessage").hide();
            }
            setTimeout(function () {
                $("#DisplayMessage").hide();
                $("#myModalUpdatePanel").modal('hide');

            }, 2000);

        }
    });
}
function UpdateInterviewPanelForMyOpening() {
    $.ajax({
        type: 'POST',
        url: "/NewAdmin/UpdateInterviewPanelForMyOpening",
        data: { "selectedManagers": selectedManagers,  "ApplicantId":applicantId  },
        success: function (response) {
            if (response == true) {
                $("#DisplayMessage").show();
            }
            else {
                $("#DisplayMessage").hide();
            }
            setTimeout(function () {
                $("#DisplayMessage").hide();
                $("#myModalUpdatePanel").modal('hide');

            }, 2000);

        }
    });
}
//var SelectPure = function () { "use strict"; const e = { value: "data-value", disabled: "data-disabled", class: "class", type: "type" }; class t { constructor(e, t = {} s = {}) { return this._node = e instanceof HTMLElement ? e : document.createElement(e), this._config = { i18n: s }, this._setAttributes(t), t.textContent && this._setTextContent(t.textContent), this } get() { return this._node } append(e) { return this._node.appendChild(e), this } addClass(e) { return this._node.classList.add(e), this } removeClass(e) { return this._node.classList.remove(e), this } toggleClass(e) { return this._node.classList.toggle(e), this } addEventListener(e, t) { return this._node.addEventListener(e, t), this } removeEventListener(e, t) { return this._node.removeEventListener(e, t), this } setText(e) { return this._setTextContent(e), this } getHeight() { return window.getComputedStyle(this._node).height } setTop(e) { return this._node.style.top = `${e}px`, this } focus() { return this._node.focus(), this } setTextContent(e) { this._node.textContent = e } setAttributes(t) { for (const s in t) e[s] && t[s] && this._setAttribute(e[s], t[s]) } setAttribute(e, t) { this._node.setAttribute(e, t) } } var s = Object.assign || function (e) { for (var t = 1; t < arguments.length; t++) { var s = arguments[t]; for (var i in s) Object.prototype.hasOwnProperty.call(s, i) && (e[i] = s[i]) } return e }; const i = { select: "select-pure__select", dropdownShown: "select-pure__select--opened", multiselect: "select-pure__select--multiple", label: "select-pure__label", placeholder: "select-pure__placeholder", dropdown: "select-pure__options", option: "select-pure__option", autocompleteInput: "select-pure__autocomplete", selectedLabel: "select-pure__selected-label", selectedOption: "select-pure__option--selected", placeholderHidden: "select-pure__placeholder--hidden", optionHidden: "select-pure__option--hidden" }; return class { constructor(e, i) { this._config = s({}, i), this._state = { opened: !1 }, this._icons = [], this._boundHandleClick = this._handleClick.bind(this), this._boundUnselectOption = this._unselectOption.bind(this), this._boundSortOptions = this._sortOptions.bind(this), this._body = new t(document.body), this._create(e), this._config.value && this._setValue() } value() { return this._config.value } create(e) { const s = "string" == typeof e ? document.querySelector(e) : e; this._parent = new t(s), this._select = new t("div", { class: i.select }), this._label = new t("span", { class: i.label }), this._optionsWrapper = new t("div", { class: i.dropdown }), this._config.multiple && this._select.addClass(i.multiselect), this._options = this._generateOptions(), this._select.addEventListener("click", this._boundHandleClick), this._select.append(this._label.get()), this._select.append(this._optionsWrapper.get()), this._parent.append(this._select.get()), this._placeholder = new t("span", { class: i.placeholder, textContent: this._config.placeholder }), this._select.append(this._placeholder.get()) } generateOptions() { return this._config.autocomplete && (this._autocomplete = new t("input", { class: i.autocompleteInput, type: "text" }), this._autocomplete.addEventListener("input", this._boundSortOptions), this._optionsWrapper.append(this._autocomplete.get())), this._config.options.map(e => { const s = new t("div", { class: i.option, value: e.value, textContent: e.label, disabled: e.disabled }); return this._optionsWrapper.append(s.get()), s }) } handleClick(e) { if (e.stopPropagation(), e.target.className !== i.autocompleteInput) { if (this._state.opened) { const t = this._options.find(t => t.get() === e.target); return t && this._setValue(t.get().getAttribute("data-value"), !0), this._select.removeClass(i.dropdownShown), this._body.removeEventListener("click", this._boundHandleClick), this._select.addEventListener("click", this._boundHandleClick), void (this._state.opened = !1) } e.target.className !== this._config.icon && (this._select.addClass(i.dropdownShown), this._body.addEventListener("click", this._boundHandleClick), this._select.removeEventListener("click", this._boundHandleClick), this._state.opened = !0, this._autocomplete && this._autocomplete.focus()) } } setValue(e, t, s) { if (e && !s && (this._config.value = this._config.multiple ? [...this._config.value || [], e] : e), e && s && (this._config.value = e), this._options.forEach(e => { e.removeClass(i.selectedOption) }), this._placeholder.removeClass(i.placeholderHidden), this._config.multiple) { const e = this._config.value.map(e => { const t = this._config.options.find(t => t.value === e); return this._options.find(e => e.get().getAttribute("data-value") === t.value.toString()).addClass(i.selectedOption), t }); return e.length && this._placeholder.addClass(i.placeholderHidden), void this._selectOptions(e, t) } const n = this._config.value ? this._config.options.find(e => e.value.toString() === this._config.value) : this._config.options[0]; this._options.find(e => e.get().getAttribute("data-value") === n.value.toString()).addClass(i.selectedOption), this._placeholder.addClass(i.placeholderHidden), this._selectOption(n, t) } selectOption(e, t) { this._selectedOption = e, this._label.setText(e.label), this._config.onChange && t && this._config.onChange(e.value) } selectOptions(e, s) { this._label.setText(""), this._icons = e.map(e => { const s = new t("span", { class: i.selectedLabel, textContent: e.label }), n = new t(this._config.inlineIcon ? this._config.inlineIcon.cloneNode(!0) : "i", { class: this._config.icon, value: e.value }); return n.addEventListener("click", this._boundUnselectOption), s.append(n.get()), this._label.append(s.get()), n.get() }), s && this._optionsWrapper.setTop(Number(this._select.getHeight().split("px")[0]) + 5), this._config.onChange && s && this._config.onChange(this._config.value) } unselectOption(e) { const t = [...this._config.value], s = t.indexOf(e.target.getAttribute("data-value")); -1 !== s && t.splice(s, 1), this._setValue(t, !0, !0) } _sortOptions(e) { this._options.forEach(t => { t.get().textContent.toLowerCase().startsWith(e.target.value.toLowerCase()) ? t.removeClass(i.optionHidden) : t.addClass(i.optionHidden) }) } } }();
//---Schedule slot----
var eventObject = [];
var autocomplete;
var sourceFullView = { url: '/NewAdmin/GetMyEvents/' };
var sourceSummaryView = { url: '/NewAdmin/GetMyEvents/' };
var CalLoading = true;
var selectedManagers = "";
var arr=[];
function loadCalendar(ApplicantId) {
    debugger
    $('#calendar').fullCalendar('destroy');
    CalLoading = true;
    $('#calendar').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultView: 'month',
        editable: true,
        allDaySlot: false,
        selectable: true,
        slotMinutes: 60,
        events: '/NewAdmin/GetMyEvents/',

        eventClick: function (calEvent, jsEvent, view) {
            alert('You clicked on event id: ' + calEvent.id
                + "\nSpecial ID: " + calEvent.someKey
                + "\nAnd the title is: " + calEvent.title);

        },

        eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
            if (confirm("Confirm move?")) {
                UpdateEvent(event.id, event.start);
            }
            else {
                revertFunc();
            }
        },

        eventResize: function (event, dayDelta, minuteDelta, revertFunc) {

            if (confirm("Confirm change appointment length?")) {
                UpdateEvent(event.id, event.start, event.end);
            }
            else {
                revertFunc();
            }
        },



        dayClick: function (date, allDay, jsEvent, view) {
           
            getslots(date.format("DD-MMM-YYYY"));
            $('#eventTitle').val("");
            //$('#eventDate').val($.fullCalendar.formatDate(date, 'dd/MM/yyyy'));
            //$('#eventTime').val($.fullCalendar.formatDate(date, 'HH:mm'));
            $('#eventDate').val(date.format("MM/DD/YYYY"));
            // $('#eventTime').val(moment(date).format('HH:MM'));

            ShowEventPopup(date);
        },

        viewRender: function (view, element) {

            if (!CalLoading) {
                if (view.name == 'month') {
                    $('#calendar').fullCalendar('removeEventSource', sourceFullView);
                    $('#calendar').fullCalendar('removeEvents');
                    $('#calendar').fullCalendar('addEventSource', sourceSummaryView);
                }
                else {
                    $('#calendar').fullCalendar('removeEventSource', sourceSummaryView);
                    $('#calendar').fullCalendar('removeEvents');
                    $('#calendar').fullCalendar('addEventSource', sourceFullView);
                }
            }
        }

    });

    CalLoading = false;
    GetBookedSlots(ApplicantId);
    $.ajax({
        type: 'GET',
        url: '/NewAdmin/GetManagerList/',
        success: function (response) {
            if (response.length > 0) {
                ManagerList = response;
                autocomplete = new SelectPure(".autocomplete-select", {
                    options: ManagerList,
                    multiple: true,
                    autocomplete: true,
                    icon: "fa fa-times",
                    max: 3,
                    onChange: value => {

                        selectedManagers = autocomplete.value().toString();
                       
                    },
                    
                });

            }

        }
    });

}
function loadCalendarCompany(ApplicantId) {
    debugger
    $('#calendarCompany').fullCalendar('destroy');
    CalLoading = true;
    $('#calendarCompany').fullCalendar({
        header: {
            left: 'prev,next today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultView: 'month',
        editable: true,
        allDaySlot: false,
        selectable: true,
        slotMinutes: 60,
        events: '/NewAdmin/GetMyEvents/',

        eventClick: function (calEvent, jsEvent, view) {
            alert('You clicked on event id: ' + calEvent.id
                + "\nSpecial ID: " + calEvent.someKey
                + "\nAnd the title is: " + calEvent.title);

        },

        eventDrop: function (event, dayDelta, minuteDelta, allDay, revertFunc) {
            if (confirm("Confirm move?")) {
                UpdateEvent(event.id, event.start);
            }
            else {
                revertFunc();
            }
        },

        eventResize: function (event, dayDelta, minuteDelta, revertFunc) {

            if (confirm("Confirm change appointment length?")) {
                UpdateEvent(event.id, event.start, event.end);
            }
            else {
                revertFunc();
            }
        },



        dayClick: function (date, allDay, jsEvent, view) {
            debugger
            getslots(date.format("DD-MMM-YYYY"));
            $('#eventTitle').val("");
            //$('#eventDate').val($.fullCalendar.formatDate(date, 'dd/MM/yyyy'));
            //$('#eventTime').val($.fullCalendar.formatDate(date, 'HH:mm'));
            $('#eventDate').val(date.format("MM/DD/YYYY"));
            // $('#eventTime').val(moment(date).format('HH:MM'));

            ShowEventPopup(date);
        },

        viewRender: function (view, element) {

            if (!CalLoading) {
                if (view.name == 'month') {
                    $('#calendarCompany').fullCalendar('removeEventSource', sourceFullView);
                    $('#calendarCompany').fullCalendar('removeEvents');
                    $('#calendarCompany').fullCalendar('addEventSource', sourceSummaryView);
                }
                else {
                    $('#calendarCompany').fullCalendar('removeEventSource', sourceSummaryView);
                    $('#calendarCompany').fullCalendar('removeEvents');
                    $('#calendarCompany').fullCalendar('addEventSource', sourceFullView);
                }
            }
        }

    });

    CalLoading = false;
    GetBookedSlotsComapny(ApplicantId);
    $.ajax({
        type: 'GET',
        url: '/NewAdmin/GetManagerList/',
        success: function (response) {
            if (response.length > 0) {
                ManagerList = response;
                autocomplete = new SelectPure(".autocomplete-select", {
                    options: ManagerList,
                    multiple: true,
                    autocomplete: true,
                    icon: "fa fa-times",
                    max: 3,
                    onChange: value => {

                        selectedManagers = autocomplete.value().toString();

                    },

                });

            }

        }
    });

}
function loadInterviewPanel() {
    selectedManagers = "";
    $("#inpManageName").html('');
    $("#inpManageName").html('<span class="autocomplete-select"></span>');
    $.ajax({
        type: 'GET',
        url: '/NewAdmin/GetManagerList/',
        success: function (response) {
            if (response.length > 0) {
                ManagerList = response;
                autocomplete = new SelectPure(".autocomplete-select", {
                    options: ManagerList,
                    multiple: true,
                    autocomplete: true,
                    icon: "fa fa-times",
                    max: 3,
                    onChange: value => {
                        selectedManagers = autocomplete.value().toString();
                    },
                });
            }
        }
    });

}
function GetBookedSlots(ApplicantId) {
    $.ajax({
        type: 'GET',
        url: '/NewAdmin/GetBookedSlots/',
        data: { "ApplicantId": ApplicantId },
        success: function (response) {
            if (response.length > 0) {
                $("#BookedSlots")
                var $html = '<div id="UrgentWorkOrdersList"><ul class="list-group">';
                $.each(response, function (index, value) {
                    //$html += '<li class="list-group-item list-group-item-success"><label>Title: ' + value.title + '</label></br><label>' + value.startDate + '</label ></br >' + '<label>Start: ' + value.startTime + '</label></br>' + '<label>End: ' + value.end + '</label></br>';
                    $html += '<div class="row notice info">';
                    $html += '<div class="col-sm-12">';
                    $html += ' <p>Title: ' + value.title + '</p>';
                    $html += '</div>';
                    $html += '<div class="col-sm-4">';
                    $html += value.startDate + '</div>';
                    $html += '<div class="col-sm-8">Start: ';
                    $html += value.startTime + ' End: ' + value.end;
                    $html += '</div>';
                    $html += '</div>';
                });
                $html += '</ul></div>';
                $("#BookedSlots").html($html);

            }
            else {
            }
        }
    });
}
function GetBookedSlotsComapny(ApplicantId) {
    $.ajax({
        type: 'GET',
        url: '/NewAdmin/GetBookedSlots/',
        data: { "ApplicantId": ApplicantId },
        success: function (response) {
            if (response.length > 0) {
                $("#BookedSlotsCompany")
                var $html = '<div id="BookedList"><ul class="list-group">';
                $.each(response, function (index, value) {
                    //$html += '<li class="list-group-item list-group-item-success"><label>Title: ' + value.title + '</label></br><label>' + value.startDate + '</label ></br >' + '<label>Start: ' + value.startTime + '</label></br>' + '<label>End: ' + value.end + '</label></br>';
                    $html += '<div class="row notice info">';
                    $html += '<div class="col-sm-12">';
                    $html += ' <p>Title: ' + value.title + '</p>';
                    $html += '</div>';
                    $html += '<div class="col-sm-4">';
                    $html += value.startDate + '</div>';
                    $html += '<div class="col-sm-8">Start: ';
                    $html += value.startTime + ' End: ' + value.end;
                    $html += '</div>';
                    $html += '</div>';
                });
                $html += '</ul></div>';
                $("#BookedSlotsCompany").html($html);

            }
            else {
            }
        }
    });
}
var SelectPure = function () { "use strict"; const e = { value: "data-value", disabled: "data-disabled", class: "class", type: "type" }; class t { constructor(e, t = {}, s = {}) { return this._node = e instanceof HTMLElement ? e : document.createElement(e), this._config = { i18n: s }, this._setAttributes(t), t.textContent && this._setTextContent(t.textContent), this } get() { return this._node } append(e) { return this._node.appendChild(e), this } addClass(e) { return this._node.classList.add(e), this } removeClass(e) { return this._node.classList.remove(e), this } toggleClass(e) { return this._node.classList.toggle(e), this } addEventListener(e, t) { return this._node.addEventListener(e, t), this } removeEventListener(e, t) { return this._node.removeEventListener(e, t), this } setText(e) { return this._setTextContent(e), this } getHeight() { return window.getComputedStyle(this._node).height } setTop(e) { return this._node.style.top = `${e}px`, this } focus() { return this._node.focus(), this } _setTextContent(e) { this._node.textContent = e } _setAttributes(t) { for (const s in t) e[s] && t[s] && this._setAttribute(e[s], t[s]) } _setAttribute(e, t) { this._node.setAttribute(e, t) } } var s = Object.assign || function (e) { for (var t = 1; t < arguments.length; t++) { var s = arguments[t]; for (var i in s) Object.prototype.hasOwnProperty.call(s, i) && (e[i] = s[i]) } return e }; const i = { select: "select-pure__select", dropdownShown: "select-pure__select--opened", multiselect: "select-pure__select--multiple", label: "select-pure__label", placeholder: "select-pure__placeholder", dropdown: "select-pure__options", option: "select-pure__option", autocompleteInput: "select-pure__autocomplete", selectedLabel: "select-pure__selected-label", selectedOption: "select-pure__option--selected", placeholderHidden: "select-pure__placeholder--hidden", optionHidden: "select-pure__option--hidden" }; return class { constructor(e, i) { this._config = s({}, i), this._state = { opened: !1 }, this._icons = [], this._boundHandleClick = this._handleClick.bind(this), this._boundUnselectOption = this._unselectOption.bind(this), this._boundSortOptions = this._sortOptions.bind(this), this._body = new t(document.body), this._create(e), this._config.value && this._setValue() } value() { return this._config.value } _create(e) { const s = "string" == typeof e ? document.querySelector(e) : e; this._parent = new t(s), this._select = new t("div", { class: i.select }), this._label = new t("span", { class: i.label }), this._optionsWrapper = new t("div", { class: i.dropdown }), this._config.multiple && this._select.addClass(i.multiselect), this._options = this._generateOptions(), this._select.addEventListener("click", this._boundHandleClick), this._select.append(this._label.get()), this._select.append(this._optionsWrapper.get()), this._parent.append(this._select.get()), this._placeholder = new t("span", { class: i.placeholder, textContent: this._config.placeholder }), this._select.append(this._placeholder.get()) } _generateOptions() { return this._config.autocomplete && (this._autocomplete = new t("input", { class: i.autocompleteInput, type: "text" }), this._autocomplete.addEventListener("input", this._boundSortOptions), this._optionsWrapper.append(this._autocomplete.get())), this._config.options.map(e => { const s = new t("div", { class: i.option, value: e.value, textContent: e.label, disabled: e.disabled }); return this._optionsWrapper.append(s.get()), s }) } _handleClick(e) { if (e.stopPropagation(), e.target.className !== i.autocompleteInput) { if (this._state.opened) { const t = this._options.find(t => t.get() === e.target); return t && this._setValue(t.get().getAttribute("data-value"), !0), this._select.removeClass(i.dropdownShown), this._body.removeEventListener("click", this._boundHandleClick), this._select.addEventListener("click", this._boundHandleClick), void (this._state.opened = !1) } e.target.className !== this._config.icon && (this._select.addClass(i.dropdownShown), this._body.addEventListener("click", this._boundHandleClick), this._select.removeEventListener("click", this._boundHandleClick), this._state.opened = !0, this._autocomplete && this._autocomplete.focus()) } } _setValue(e, t, s) { if (e && !s && (this._config.value = this._config.multiple ? [...this._config.value || [], e] : e), e && s && (this._config.value = e), this._options.forEach(e => { e.removeClass(i.selectedOption) }), this._placeholder.removeClass(i.placeholderHidden), this._config.multiple) { const e = this._config.value.map(e => { const t = this._config.options.find(t => t.value === e); return this._options.find(e => e.get().getAttribute("data-value") === t.value.toString()).addClass(i.selectedOption), t }); return e.length && this._placeholder.addClass(i.placeholderHidden), void this._selectOptions(e, t) } const n = this._config.value ? this._config.options.find(e => e.value.toString() === this._config.value) : this._config.options[0]; this._options.find(e => e.get().getAttribute("data-value") === n.value.toString()).addClass(i.selectedOption), this._placeholder.addClass(i.placeholderHidden), this._selectOption(n, t) } _selectOption(e, t) { this._selectedOption = e, this._label.setText(e.label), this._config.onChange && t && this._config.onChange(e.value) } _selectOptions(e, s) { this._label.setText(""), this._icons = e.map(e => { const s = new t("span", { class: i.selectedLabel, textContent: e.label }), n = new t(this._config.inlineIcon ? this._config.inlineIcon.cloneNode(!0) : "i", { class: this._config.icon, value: e.value }); return n.addEventListener("click", this._boundUnselectOption), s.append(n.get()), this._label.append(s.get()), n.get() }), s && this._optionsWrapper.setTop(Number(this._select.getHeight().split("px")[0]) + 5), this._config.onChange && s && this._config.onChange(this._config.value) } _unselectOption(e) { const t = [...this._config.value], s = t.indexOf(e.target.getAttribute("data-value")); -1 !== s && t.splice(s, 1), this._setValue(t, !0, !0) } _sortOptions(e) { this._options.forEach(t => { t.get().textContent.toLowerCase().startsWith(e.target.value.toLowerCase()) ? t.removeClass(i.optionHidden) : t.addClass(i.optionHidden) }) } } }();

$('#btnPopupCancel').click(function () {
    ClearPopupFormValues();
    //$('#popupEventForm').hide();
    $("#ModalScheduleInterview").modal('hide');
});
$('#btnPopupSave').click(function () {

    //$('#popupEventForm').hide();
    $("#ModalScheduleInterview").modal('hide');
    var dataRow = {
        'Title': $('#eventTitle').val(),
        'NewEventDate': $('#eventDate').val(),
        //'NewEventTime': $('#eventTime').val(),
        'NewEventTime': $("#eventTime select option:selected").attr('id'),
        'NewEventDuration': $('#eventDuration').val(),
        'JobId': $("#JobId").val(),
        //'ApplicantName': $("#lblApplicantName").text(),
        //'ApplicantEmail': $("#lblApplicantEmail").text(),
        'selectedManagers': selectedManagers
    }

    ClearPopupFormValues();

    $.ajax({
        type: 'POST',
        url: "/NewAdmin/SaveEvent",
        data: dataRow,
        success: function (response) {
            $('#calendar').fullCalendar('refetchEvents');
            alert(response);
            GetBookedSlots($("#lblApplicantId").text());
            GetBookedSlotsComapny($("#lblApplicantId").text());

        }
    });
});
function ShowEventPopup(date) {
    ClearPopupFormValues();
    //$('#popupEventForm').show();
    $("#ModalScheduleInterview").modal('show');
    $('#eventTitle').focus();
}
function ClearPopupFormValues() {
    $('#eventID').val("");
    $('#eventTitle').val("");
    $('#eventDateTime').val("");
    $('#eventDuration').val("");
}
function UpdateEvent(EventID, EventStart, EventEnd) {

    var dataRow = {
        'ID': EventID,
        'NewEventStart': EventStart,
        'NewEventEnd': EventEnd
    }

    //$.ajax({
    //    type: 'POST',
    //    url: "/NewAdmin/UpdateEvent",
    //    dataType: "json",
    //    contentType: "application/json",
    //    data: JSON.stringify(dataRow)
    //});
}
//var SelectPure = function () { "use strict"; const e = { value: "data-value", disabled: "data-disabled", class: "class", type: "type" }; class t { constructor(e, t = {}, s = {}) { return this._node = e instanceof HTMLElement ? e : document.createElement(e), this._config = { i18n: s }, this._setAttributes(t), t.textContent && this._setTextContent(t.textContent), this } get() { return this._node } append(e) { return this._node.appendChild(e), this } addClass(e) { return this._node.classList.add(e), this } removeClass(e) { return this._node.classList.remove(e), this } toggleClass(e) { return this._node.classList.toggle(e), this } addEventListener(e, t) { return this._node.addEventListener(e, t), this } removeEventListener(e, t) { return this._node.removeEventListener(e, t), this } setText(e) { return this._setTextContent(e), this } getHeight() { return window.getComputedStyle(this._node).height } setTop(e) { return this._node.style.top = `${e}px`, this } focus() { return this._node.focus(), this } _setTextContent(e) { this._node.textContent = e } _setAttributes(t) { for (const s in t) e[s] && t[s] && this._setAttribute(e[s], t[s]) } _setAttribute(e, t) { this._node.setAttribute(e, t) } } var s = Object.assign || function (e) { for (var t = 1; t < arguments.length; t++) { var s = arguments[t]; for (var i in s) Object.prototype.hasOwnProperty.call(s, i) && (e[i] = s[i]) } return e }; const i = { select: "select-pure__select", dropdownShown: "select-pure__select--opened", multiselect: "select-pure__select--multiple", label: "select-pure__label", placeholder: "select-pure__placeholder", dropdown: "select-pure__options", option: "select-pure__option", autocompleteInput: "select-pure__autocomplete", selectedLabel: "select-pure__selected-label", selectedOption: "select-pure__option--selected", placeholderHidden: "select-pure__placeholder--hidden", optionHidden: "select-pure__option--hidden" }; return class { constructor(e, i) { this._config = s({}, i), this._state = { opened: !1 }, this._icons = [], this._boundHandleClick = this._handleClick.bind(this), this._boundUnselectOption = this._unselectOption.bind(this), this._boundSortOptions = this._sortOptions.bind(this), this._body = new t(document.body), this._create(e), this._config.value && this._setValue() } value() { return this._config.value } _create(e) { const s = "string" == typeof e ? document.querySelector(e) : e; this._parent = new t(s), this._select = new t("div", { class: i.select }), this._label = new t("span", { class: i.label }), this._optionsWrapper = new t("div", { class: i.dropdown }), this._config.multiple && this._select.addClass(i.multiselect), this._options = this._generateOptions(), this._select.addEventListener("click", this._boundHandleClick), this._select.append(this._label.get()), this._select.append(this._optionsWrapper.get()), this._parent.append(this._select.get()), this._placeholder = new t("span", { class: i.placeholder, textContent: this._config.placeholder }), this._select.append(this._placeholder.get()) } _generateOptions() { return this._config.autocomplete && (this._autocomplete = new t("input", { class: i.autocompleteInput, type: "text" }), this._autocomplete.addEventListener("input", this._boundSortOptions), this._optionsWrapper.append(this._autocomplete.get())), this._config.options.map(e => { const s = new t("div", { class: i.option, value: e.value, textContent: e.label, disabled: e.disabled }); return this._optionsWrapper.append(s.get()), s }) } _handleClick(e) { if (e.stopPropagation(), e.target.className !== i.autocompleteInput) { if (this._state.opened) { const t = this._options.find(t => t.get() === e.target); return t && this._setValue(t.get().getAttribute("data-value"), !0), this._select.removeClass(i.dropdownShown), this._body.removeEventListener("click", this._boundHandleClick), this._select.addEventListener("click", this._boundHandleClick), void (this._state.opened = !1) } e.target.className !== this._config.icon && (this._select.addClass(i.dropdownShown), this._body.addEventListener("click", this._boundHandleClick), this._select.removeEventListener("click", this._boundHandleClick), this._state.opened = !0, this._autocomplete && this._autocomplete.focus()) } } _setValue(e, t, s) { if (e && !s && (this._config.value = this._config.multiple ? [...this._config.value || [], e] : e), e && s && (this._config.value = e), this._options.forEach(e => { e.removeClass(i.selectedOption) }), this._placeholder.removeClass(i.placeholderHidden), this._config.multiple) { const e = this._config.value.map(e => { const t = this._config.options.find(t => t.value === e); return this._options.find(e => e.get().getAttribute("data-value") === t.value.toString()).addClass(i.selectedOption), t }); return e.length && this._placeholder.addClass(i.placeholderHidden), void this._selectOptions(e, t) } const n = this._config.value ? this._config.options.find(e => e.value.toString() === this._config.value) : this._config.options[0]; this._options.find(e => e.get().getAttribute("data-value") === n.value.toString()).addClass(i.selectedOption), this._placeholder.addClass(i.placeholderHidden), this._selectOption(n, t) } _selectOption(e, t) { this._selectedOption = e, this._label.setText(e.label), this._config.onChange && t && this._config.onChange(e.value) } _selectOptions(e, s) { this._label.setText(""), this._icons = e.map(e => { const s = new t("span", { class: i.selectedLabel, textContent: e.label }), n = new t(this._config.inlineIcon ? this._config.inlineIcon.cloneNode(!0) : "i", { class: this._config.icon, value: e.value }); return n.addEventListener("click", this._boundUnselectOption), s.append(n.get()), this._label.append(s.get()), n.get() }), s && this._optionsWrapper.setTop(Number(this._select.getHeight().split("px")[0]) + 5), this._config.onChange && s && this._config.onChange(this._config.value) } _unselectOption(e) { const t = [...this._config.value], s = t.indexOf(e.target.getAttribute("data-value")); -1 !== s && t.splice(s, 1), this._setValue(t, !0, !0) } _sortOptions(e) { this._options.forEach(t => { t.get().textContent.toLowerCase().startsWith(e.target.value.toLowerCase()) ? t.removeClass(i.optionHidden) : t.addClass(i.optionHidden) }) } } }();
function ShowMyOpeningGrid() {
    selectedManagers = "";
    $('#calendar').fullCalendar('destroy');
    CalLoading = true;
    //$("#JobPostBackBtn").show();
    $("#Scheduler").css('display', 'none');
    $("#MyOpeningSummery").show();
    //$("#inpManageName").html('');
    //$("#inpManageName").html('<span class="autocomplete-select"></span>');
    $("#lblApplicantId").text('');
    $("#lblApplicantName").text('');
    $("#lblApplicantEmail").text('');
}
function ShowMyOpeningGridCompany() {
    selectedManagers = "";
    $('#calendarCompany').fullCalendar('destroy');
    CalLoading = true;
    //$("#JobPostBackBtn").show();
    $("#SchedulerCompany").css('display', 'none');
    $("#companyOpening").show();
    //$("#inpManageName").html('');
    //$("#inpManageName").html('<span class="autocomplete-select"></span>');
    $("#lblApplicantId").text('');
    $("#lblApplicantName").text('');
    $("#lblApplicantEmail").text('');
}
function UpdateInterviewPanel() {

    $.ajax({
        type: 'POST',
        url: "/NewAdmin/UpdateInterviewPanel",
        data: { "selectedManagers": selectedManagers, "JobId": $("#JobId").val() ,"JobTitle":JobTitle  },
        success: function (response) {
            if (response == true) {
                $("#DisplayMessage").show();
            }
            else {
                $("#DisplayMessage").hide();
            }
            setTimeout(function () {
                $("#DisplayMessage").hide();
                $("#myModalUpdatePanel").modal('hide');

            }, 2000);

        }
    });
}
//--End Schedule Interview
function CloseHoldJob() {
    $.ajax({
        type: 'POST',
        url: base_url + "/NewAdmin/CloseHoldOpenJob?JobId=" + JobPostingId + "&JobStatus=" + JobStatus,
        //data: { "ApplicantId": ApplicantId },
        success: function (response) {
            $("#ModalForJobStatusChange").modal("hide");
            if (response)
                toastr.success("job status change successfully");
            else
                toastr.success("error while changing job status please try again.");
            $("#MyOpeningSummery").jsGrid("loadData");
        },
        error: function (err) {
        }
    });
}
//function RefreshGrid() {
$("#RefreshGrid").click(function () {
    $("#" + refreshGridId).jsGrid("loadData");
});
function GetAllDocumetOfApplicant(){
    $.ajax({
        type: 'POST',
        url: base_url + "/NewAdmin/OpenDocuments?applicantId=" + applicantId ,
        //data: { "ApplicantId": ApplicantId },
        success: function (response) {
            $("#myModalGetCertificateApplicant").show();
            $("#ViewDocument").html(response);
        },
        error: function (err) {
        }
    });
    
}
function OfferSend(statusOffer){
    var dataObject = $("#SendOfferData").serialize();
        $.ajax({
            type: "POST",
            url: base_url + '/NewAdmin/SendOfferToApplicant', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
            data: dataObject,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (Data) {
                if (Data == true) {
                    toastr.success("Offer send to applicant");
                }
                else {
                    toastr.success("Offer not send to applicant please send it again..!");
                }
                //$("#ModalForOfferLetter").css("display", "none");
                $("#ModalForOfferLtter").modal('hide');
                //var link = base_url+'/NewAdmin/HiringOnBoardingDashboard';
                //$("#RenderPageId").load(link);
                
                //
                $("#ListMyOpening").jsGrid("loadData");
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });
}
function HiredApplicant(applicantId, Status){
    $.ajax({
        type: "POST",
        url: base_url + '/NewAdmin/HireApplicant?ApplicantId=' + applicantId, //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
            if (Data == true) {
                toastr.success("applicant has been hired successfulyy.");
            }
            else {
                toastr.success("something went wrong, please try it again..!");
            }
            $("#ListMyOpening").jsGrid("loadData");
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function BackgroundPassFail(IsActive) {
    $.ajax({
        type: "POST",
        url: base_url + '/NewAdmin/SaveApplicantStatus?ApplicantId=' + applicantId + '&Status=' + 'F' + '&IsActive=' + IsActive, //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
            if (Data == true && IsActive == "Y") {
                toastr.success("Background check has been passed successfuly.");
            }
            else {
                toastr.success("Background check has been failed");
            }
            $("#ModalForPassFailBackground").modal("hide");
            $("#ListMyOpening").jsGrid("loadData");
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function SaveOriantation($_this) {
    debugger
    var origin_Url = window.location.origin;
    var EmpID = $($_this).attr('EmpId');
    $("#OTN_EMP_EmployeeID").val(EmpID);
    $("#OTN_ApplicantId").val(applicantId);
    var dataOrientation = $("#SaveOrientationForm").serialize();
    $.ajax({
        type: "POST",
        url: origin_Url + '/EPeople/SaveOriantation', //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
        data: dataOrientation,
        success: function (result) {
            $("#GetEmployeeIdForOrientation").html("");
            $("#ONT_OrientationDate").val('');
            $("#ONT_Orientationtime").val('');
            $("#myModalToAddEmployeeOrietation").modal('hide');
            if (result.isSaved == true) {
                toastr.success(result.message);

            }
            else { toastr.error(result.message); }
            $("#ListMyOpening").jsGrid("loadData");
        },
        error: function (err) {
            alert(err);
        }
    });
}
function OriantationDone($_Status) {
    $.ajax({
        type: "POST",
        url: base_url + '/NewAdmin/OriantationDone?ApplicantId=' + applicantId + '&Status=' + 'G' + '&IsActive=' + $_Status, //'@Url.Action("SaveVCS", "AdminDashboard", new { area = "AdminSection" })',
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
            if (Data == true && IsActive == "Y") {
                toastr.success("Background check has been passed successfuly.");
            }
            else {
                toastr.success("Background check has been failed");
            }
            $("#ListMyOpening").jsGrid("loadData");
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function PDFDownload(data) {
    var urlFile = data.DownloadFilePath;
    download_file(urlFile, data.DownloadFile);
    //window.location.href = base_url+ "/NewAdmin/SinglePDFDownload?stFile=" +urlFile + "19RichardOnboardingFiles.pdf";
    //$.ajax({
    //    type: "POST",
    //    url: base_url + '/NewAdmin/SinglePDFDownload?ApplicantId=' + data.ApplicantId ,
    //    beforeSend: function () {
    //        new fn_showMaskloader('Please wait...');
    //    },
    //    success: function (item) {           
    //        $("#ListOnBoarding").jsGrid("loadData");
    //    },
    //    complete: function () {
    //        fn_hideMaskloader();
    //    }
    //});
}
/* Helper function */
function download_file(fileURL, fileName) {
    // for non-IE
    if (!window.ActiveXObject) {
        var save = document.createElement('a');
        save.href = fileURL;
        save.target = '_blank';
        var filename = fileURL.substring(fileURL.lastIndexOf('/') + 1);
        save.download = fileName || filename;
        if (navigator.userAgent.toLowerCase().match(/(ipad|iphone|safari)/) && navigator.userAgent.search("Chrome") < 0) {
            document.location = save.href;
            // window event not working here
        } else {
            var evt = new MouseEvent('click', {
                'view': window,
                'bubbles': true,
                'cancelable': false
            });
            save.dispatchEvent(evt);
            (window.URL || window.webkitURL).revokeObjectURL(save.href);
        }
    }
    else if (!!window.ActiveXObject && document.execCommand) {
        var _window = window.open(fileURL, '_blank');
        _window.document.close();
        _window.document.execCommand('SaveAs', true, fileName || fileURL)
        _window.close();
    }
}