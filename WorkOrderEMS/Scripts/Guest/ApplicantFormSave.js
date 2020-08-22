
var details = [];
var checkListStatus = '';
function OpenForm(formname, elm) {
    var geturl = '';
    var isLocked = false;
    $(elm).find('i').each(function (i, ielm) {
        if ($(ielm).hasClass('fa-lock'))
            isLocked = true;
    });
    if (isLocked) {
        return;
    }

    switch (formname) {
        case 'directdeposite':
            geturl = '/Guest/_DirectDepositeForm';
            break;
        case 'employeehandbook':
            geturl = '/Guest/_EmployeeHandbook';
            break;
        case 'W4Form':
            geturl = '/Guest/_W4Form';
            break;
        case 'I9Form':
            geturl = '/Guest/_I9Form';
            break;
        case 'EmergencyContactForm':
            geturl = '/Guest/_emergencyContactForm';
            break;
        case 'photorelease':
            geturl = '/Guest/_PhotoReleaseForm';
            break;
        case 'confidentiality':
            geturl = '/Guest/_ConfidentialityAgreementForm';
            break;
        case 'educationVerificationForm':
            geturl = '/Guest/_EducationVarificationForm';
            break;
        case 'creditCardAuthorizationForm':
            geturl = '/Guest/_CreditCardAuthorizationForm';
            break;
        case 'previousEmployeement':
            geturl = '/Guest/_PreviousEmployeement';
            break;
    }
    GetForm(geturl, function (successRes) {
        $("#formid").html(successRes);
    }, function (errorRes) {
        alert('Something went wrong!!!');
    });

    $("#formModel").modal('show');
}
function closeModel() {
    $("#formModel").modal('hide');
}
function SubmitForm(element, formName) {//changed by rajat
    debugger
    var url = '';
    var data = '';
    var isDirectDepositeSaved = false;
    var isContactSaved = false;
    var isBackgroundCheck = false;
    var isBenifitSectionPopUpOpen = false;
    //var isSubmit = confirm('Are you sure you want to submit');

    //if (isSubmit) {
    if (formName.id == undefined || formName.id == null) {
        formName.id = formName[0].id;
    }
    switch (formName.id) {
        case 'depositeForm':
            url = '/Guest/_DirectDepositeForm';
            data = $('#depositeForm').serialize();
            isDirectDepositeSaved = true;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false
            break;
        case 'employeeHandbook':
            url = '/Guest/_EmployeeHandbook';
            data = ('#employeeHandbook').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false
            break;
        case 'photoreleaseform':
            url = '/Guest/_photoreleaseform';
            data = $('#photoreleaseform').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false
            break;
        case 'emergencycontactform':
            url = '/Guest/_emergencyContactForm';
            data = $('#emergencycontactform').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false
            break;
        case 'confidentialityagreementform':
            url = '/Guest/_ConfidentialityAgreementForm';
            data = $('#confidentialityagreementform').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false
            break;
        case 'educationverificationform':
            debugger
            url = '/Guest/_EducationVarificationForm';

            //var serailizedata = $('#educationverificationform').serialize();
            //var arr = serailizedata.split('item.HighSchool.SchoolName');
            //var mergeArr = [];
            //for (var i = 0; i < arr.length; i++) {
            //    if (arr[i] != "" && arr[i] != null) {
            //        var tt = "item.HighSchool.SchoolName" + arr[i];
            //        mergeArr.push({ tt });
            //    }
            //}
            //data = mergeArr;
            data = $('#educationverificationform').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false
            break;
        case 'w4form':
            url = '/Guest/_W4Form'//changed by rajat
            data = $('#w4form').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false
            break;
        case 'I9Form':

            url = '/Guest/_I9Form';
            data = $('#I9Form').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false
            break;
        case 'ContactSavedForm':

            url = '/Guest/_ContactSavedForm';
            data = $('#ContactSavedForm').serialize();
            isDirectDepositeSaved = false;
            isBackgroundCheck = false;
            isContactSaved = true;
            isBenifitSectionPopUpOpen = false
            //element.preventDefault();
            break;
        case 'BackGroundCheckForm':
            url = '/Guest/_BackGroundCheckForm';
            data = $('#BackGroundCheckForm').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = true;
            isBenifitSectionPopUpOpen = false
            //element.preventDefault();
            break;
        case 'SaveBenifit':
            url = '/Guest/BenifitSection';
            data = $('#SaveBenifit').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = true;
            //element.preventDefault();
            break;
        case 'selfIdentificationForm':
            url = '/Guest/SaveSelfIdentificationForm';
            data = $('#selfIdentificationForm').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false;
            //element.preventDefault();
            break;
        case 'ApplicantFunFactForm':
            url = '/Guest/ApplicantFunFact';
            data = $('#ApplicantFunFactForm').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false;
            //element.preventDefault();
            break;
        case 'RateOfPay':
            url = '/Guest/_RateOfPay';
            data = $('#RateOfPay').serialize();
            isDirectDepositeSaved = false;
            isContactSaved = false;
            isBackgroundCheck = false;
            isBenifitSectionPopUpOpen = false;
            //element.preventDefault();
            break;

    }
    if (isContactSaved == false) {
        PostForm(url, data, isDirectDepositeSaved, isBackgroundCheck, isBenifitSectionPopUpOpen, function (successResponse) {
            if (successResponse == true) {
                LockItem(formName.id);
                $("#formModel").modal('hide');
                location.href = '/Guest/PersonalFile';
            }
            else {

                $("#formid").html(successResponse);
                $("#formModel").modal('show');
                $(element).prop('checked', false);
            }
        }, function (errorResponse) {
            alert('Something went wrong!!!!');
            $("#formModel").modal('hide');
        });
    }
    else {

        SaveContact(data, details, url, function (errorCallback) {
            alert('Something went wrong!!!!');
            $("#formModel").modal('hide');
        });
    }
    //}

    //if (isSubmit) {
    //	$.ajax({
    //		type: "POST",
    //		url: '/Guest/_DirectDepositeForm',
    //		data: $('#depositeForm').serialize(),
    //		success: function (result) {
    //			if (result == true) {
    //				location.href = '/Guest/PersonalFile?isSaved=true';
    //			} else {
    //				$("#formid").html(result);
    //				$("#formModel").modal('show');
    //			}
    //		},
    //		error: function () {

    //		}
    //	});
    //}
}
$(".isCheckedContact").click(function () {
    debugger
    $_this = this;
    var getVal;
    if ($_this.checked == true) {
        getVal = $($_this).attr("value");
        details.push({ ContactId: getVal, IsChecked: "Y" });
    } else {
        getVal = $($_this).attr("value");
        details.push({ ContactId: getVal, IsChecked: "N" });
    }
})
function PostForm(url, data, isDirectDepositeSaved, isBackgroundCheck, isBenifitSectionPopUpOpen, successCallback, errorCallback) {
    debugger
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
            debugger
            if (Data == false) {
                alert("Please fill and save all the forms!");
            }
            else {
                //$("#RenderPageId").html(result);
                $("#RenderPageId").html(Data);
                var base_url = window.location.origin;
            }
                

        },
        error: errorCallback,
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
function SaveContact(data, details, url, errorCallback) {
    debugger
    $.ajax({
        type: "POST",
        url: url,
        data: { model: data, lstModel: details },
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (Data) {
            debugger
            $('#myModelForContactDetails').modal('hide');
            //$("#RenderPageId").html(Data);
            $('.fade').removeClass('modal-backdrop');
            $('.fade').removeClass('show');
        },
        error: errorCallback,
        complete: function () {
            fn_hideMaskloader();
        }
    });
}

function GetForm(url, successCallback, errorCallback) {
    $.ajax({
        type: "GET",
        url: url,
        success: successCallback,
        error: errorCallback
    });
}

function ThankYou() {
    location.href = '/Guest/ThankYou';
}
function LockItem(formId) {
    switch (formId) {
        case 'depositeForm':
            var elm = $("#directdepositeicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            elm = $("#directdepositeicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'employeeHandbook':
            var elm = $("#employeehandbookicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            elm = $("#employeehandbookicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'photoreleaseform':
            var elm = $("#photoreleaseicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            elm = $("#photoreleaseicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'emergencycontactform':
            var elm = $("#emergencycontactformicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            elm = $("#emergencycontactformicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'confidentialityagreementform':
            var elm = $("#confidentialityagreementicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            var elm = $("#confidentialityagreementicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'educationverificationform':
            var elm = $("#educationverificationformicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            elm = $("#educationverificationformicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
        case 'w4form':
            var elm = $("#w4formicn").find('.lock i').first();
            elm.removeClass('fa-unlock');
            elm.addClass('fa-lock');
            elm = $("#w4formicn").find('.bluesky').first();
            elm.removeClass('bluesky');
            elm.addClass('grn-icn');
            break;
    }
}
function blink() {
    $(".blink").fadeOut('slow', function () {
        $(this).fadeIn('slow', function () {
            blink(this.id);
        });
    });
}
function unblink(id) {
    $("#" + id).stop().fadeIn();
    NextBlink(id);
}
//blink('w4formicn');
function NextBlink(id) {

    var siblingid = $("#" + id).next()[0].id;
    blink(siblingid);
}
function SetFormStatus() {
    $.ajax({
        url: '/Guest/GetFormsStatus',
        method: 'GET',
        success: function (response) {

            if (response.W4Status == 'Y') {
                $("#w4formicn").removeClass('blink');
                $($("#w4formicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('w4form');

            }
            if (response.EmergencyContactFormStatus == 'Y') {
                $("#emergencycontactformicn").removeClass('blink');
                $($("#emergencycontactformicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('emergencycontactform');

            }
            if (response.EmployeeHandbook == 'Y') {
                $("#employeehandbookicn").removeClass('blink');
                $($("#employeehandbookicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('employeehandbook');
            }
            if (response.DirectDepositForm == 'Y') {
                $("#directdepositeicn").removeClass('blink');
                $($("#directdepositeicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('depositeForm');

            }
            if (response.PhotoReleaseForm == 'Y') {
                $("#photoreleaseicn").removeClass('blink');
                $($("#photoreleaseicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn')

            }
            if (response.ConfidentialityAgreement == 'Y') {
                $("#confidentialityagreementicn").removeClass('blink');
                $($("#confidentialityagreementicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn')

            }
            if (response.EducationVerificationForm == 'Y') {
                $("#educationverificationformicn").removeClass('blink');
                $($("#educationverificationformicn").find('.bluesky')[0]).removeClass('bluesky').addClass('grn-icn');
                LockItem('educationverificationform');

            }
            var $div2blink = $(".blink"); // Save reference, only look this item up once, then save
            var backgroundInterval = setInterval(function () {
                $div2blink.toggleClass("backgroundRed");
            }, 1500)
        }
    });
}

//////=========================Next From Background check============================//////
//=========================================================================================
var signatureName = "";
$("#AddSignature").click(function (e) {

    var url = window.location.origin;
    $.ajax({
        url: url + '/Guest/GetSignature',
        type: "POST",
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            if (result != null && result != false) {
                //$("#myModalForSignatureApplicant").modal('show');
                $("#openItForSignature").attr("src", result.imagePath);
                $(".Emp_Sign").val(result.imagePath);
                $("#getSignName").val(result.name);
                $("#ShowHideSignatureSpan,#AddSignature").hide();
                $("#openItForSignature").show();
                signatureName = name;
            }
        },
        error: function (err) {
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
})
$('.savesignaturInfo').click(function () {
    var url = window.location.origin;
    $_this = this;
    var isUpdate = $($_this).attr("is-update");
    $.ajax({
        url: url + '/Guest/SaveSignature?isUpdate=' + isUpdate,
        type: "POST",
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {

            if (result != null && result != false) {
                $("#myModalForSignatureApplicant").modal('show');
                $("#openItForSignature").attr("src", result.imagePath);
                $("#ShowHideSignatureSpan").hide();
                $("#openItForSignature").show();
                signatureName = name;
            }
        },
        error: function (err) {

        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
});
$("#UploadDocApplicant").click(function () {
    debugger
    var url = window.location.origin;
    //get(0) its return the first element of jquery object(jquery object is also an array), first file to be upload
    var fileUploadLicense = $("#myLicensefileUpload").get(0);
    var filesLicense = fileUploadLicense.files;
    // Create FormData object  
    // FormData interface enables appending File objects to XHR-requests (Ajax-requests).
    var fileData = new FormData();
    // Looping over all files and add it to FormData object  
    for (var i = 0; i < filesLicense.length; i++) {
        fileData.append(filesLicense[i].name, filesLicense[i]);
    }
    $.ajax({
        url: url + '/Guest/UploadFilesApplicant?isLicense=' + true,
        type: "POST",
        contentType: false, // Not to set any content header  
        processData: false, // Not to process data  
        data: fileData,
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            debugger
            var fileUploadSSN = $("#mySSNfileUpload").get(0);
            var filesSSN = fileUploadSSN.files;
            fileData = new FormData();
            for (var i = 0; i < filesSSN.length; i++) {
                fileData.append(filesSSN[i].name, filesSSN[i]);
            }
            $.ajax({
                url: url + '/Guest/UploadFilesApplicant?isLicense=' + false,
                type: "POST",
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                beforeSend: function () {
                    new fn_showMaskloader('Please wait...');
                },
                success: function (result) {
                    debugger
                    //$("#RenderPageId").html(result);
                    $("#myModalToAddDocumentUpload").hide();

                    $('.fade').removeClass('modal-backdrop');
                    $('.fade').removeClass('show');
                    $("#myModelForDesclaimerEEO").modal('show');
                },
                error: function (err) {

                },
                complete: function () {
                    fn_hideMaskloader();
                }
            });
            $("#myModalToAddDocumentUpload").modal('hide');
            //alert(result);
        },
        error: function (err) {
            alert(err.statusText);
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
});

function GotoNextForm(istrue) {
    debugger
    $('input[name="EEOstatus"]').val(istrue);
    var data = $('#selfIdentificationForm').serialize();
   
    var url = window.location.origin;
    if (istrue) {
        url += '/Guest/SaveSelfIdentificationForm';
        $("#myModelForDesclaimerEEO").hide();
        $('.fade').removeClass('modal-backdrop');
        $('.fade').removeClass('show');

        $.ajax({
            url: url,
            type: "POST",
            data: data,
            beforeSend: function () {
                new fn_showMaskloader('Please wait...');
            },
            success: function (result) {
                $("#RenderPageId").html(result);
                

            },
            error: function (err) {
            },
            complete: function () {
                fn_hideMaskloader();
            }
        });


    }
    else {
        // url += '/Guest/ApplicantFunFacts';
        $("#myModelForDesclaimerEEO").hide();
        $('.fade').removeClass('modal-backdrop');
        $('.fade').removeClass('show');
        //alert("Please, Accept to proceed");
    }

}



$("#AddSignatureTranslator").click(function () {
    debugger
    $("#myModalForSignatureApplicant").modal('show');
})

$(function () {
    debugger
    $('#colors_sketchApplicant').sketch();
    $(".tools a").eq(0).attr("style", "color:#000");
    $(".tools a").click(function () {
        $(".tools a").removeAttr("style");
        $(this).attr("style", "color:#000");
    });
    $("#btnSaveTranslator").bind("click", function () {
        debugger
        var base64 = $('#colors_sketchApplicant')[0].toDataURL();
        $("#openForTranslator").attr("src", base64);
        $("#openForTranslator").show();
        $("#ShowHideSignatureTranslatorSpan, #AddSignatureTranslator").hide();
        $("#SignatureImageBase").val(base64);
    });
});
$("#AddSignatureManager").click(function () {
    debugger
    $("#myModalForSignatureManager").modal('show');
})

$(function () {

    debugger
    $('#colors_sketchManager').sketch();
    $(".tools a").eq(0).attr("style", "color:#000");
    $(".tools a").click(function () {
        $(".tools a").removeAttr("style");
        $(this).attr("style", "color:#000");
    });
    $("#btnSaveManager").bind("click", function () {
        //onclick applicant
        debugger
        var base64 = $('#colors_sketchApplicant')[0].toDataURL();
        $("#openItForSignatureManager").attr("src", base64);
        $("#openItForSignatureManager").show();
        $("#ShowHideSignatureSpanMan, #AddSignatureManager").hide();
        $("#SignatureImageBase").val(base64);
    });
  
});
function SaveBeforeNext(element, formName) {
    debugger
    var urls = window.location.origin;
    var url = '';
    var data = '';


    if (formName.id == undefined || formName.id == null) {
        formName.id = formName[0].id;
    }
    switch (formName.id) {
        case 'depositeForm'://done
            var getData = $('#depositeForm').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'depositeForm';

            break;
        case 'employeeHandbook'://done
            var getData = $('#employeeHandbook').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'employeeHandbook';
            break;
        case 'photoreleaseform'://done
            var getData = $('#photoreleaseform').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'photoreleaseform';
            break;
        case 'emergencycontactform'://done
            var getData = $('#emergencycontactform').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'emergencycontactform';
            break;
        case 'confidentialityagreementform'://done
            var getData = $('#confidentialityagreementform').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'confidentialityagreementform';
            break;
        case 'educationverificationform'://done
            debugger
            url = '/Guest/SaveOnboardingForms?FormName=' + 'educationverificationform';
            var serailizedata = $('#educationverificationform').serializeArray();
            //var arr = serailizedata.split('item.HighSchool.SchoolName');
            //var mergeArr = [];
            //for (var i = 0; i < arr.length; i++) {
            //    if (arr[i] != "" && arr[i] != null) {
            //        var tt = "item.HighSchool.SchoolName" + arr[i];
            //        mergeArr.push({ tt });
            //    }
            //}
            data = JSON.stringify(jQFormSerializeArrToJson(serailizedata));
            break;
        case 'w4form'://done
            var getData = $('#w4form').serializeArray();
            //getData.push({ name: 'FormName', value: 'w4form' });
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            //JSON.parse(JSON.stringify(jQuery('#w4form').serializeArray()))
            //data = $('#w4form').serialize();
            url = '/Guest/SaveOnboardingForms?FormName=' + 'w4form';//changed by rajat
            break;
        case 'I9Form'://done

            var getData = $('#I9Form').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'I9Form';
            break;
        case 'ContactSavedForm'://done
            var getData = $('#ContactSavedForm').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'ContactSavedForm';
            break;
        case 'BackGroundCheckForm'://done
            var getData = $('#BackGroundCheckForm').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'BackGroundCheckForm';
            break;
        case 'SaveBenifit'://done
            var getData = $('#SaveBenifit').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'SaveBenifit';
            break;
        case 'selfIdentificationForm'://done
            var getData = $('#selfIdentificationForm').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'selfIdentificationForm';
            break;
        case 'ApplicantFunFactForm'://done
            var getData = $('#ApplicantFunFactForm').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'ApplicantFunFactForm';
            break;
        case 'RateOfPay'://done
            var getData = $('#RateOfPay').serializeArray();
            data = JSON.stringify(jQFormSerializeArrToJson(getData));
            url = '/Guest/SaveOnboardingForms?FormName=' + 'RateOfPay';
            break;


    }

    //data = $('#w4form').serialize();
    $.ajax({
        type: "POST",
        url: urls + url,
        data: { "onboardingformdata": data },
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (resultNext) {
            debugger
            checkListStatus = resultNext;
            if (resultNext.formName == "w4form") {
                $("#FormStatusw4").val(resultNext.FormStatusw4);
            }
            if (resultNext.formName == "I9Form") {
                $("#FormStatusI9").val(resultNext.FormStatusI9);
            }
            if (resultNext.formName == "BackGroundCheckForm") {
                $("#FormStatusbcf").val(resultNext.FormStatusbcf);
                if (resultNext.FormStatusbcf == "true") {
                    $('#myModalToAddDocumentUpload').modal('show', { backdrop: 'static', keyboard: false });
                }
            }
            if (resultNext.formName == "emergencycontactform") {
                $("#FormStatusprfecf").val(resultNext.FormStatusprfecf);
            }
            if (resultNext.formName == "depositeForm") {
                $("#FormStatusdd").val(resultNext.FormStatusdd);
            }
            if (resultNext.formName == "educationverificationform") {
                $("#FormStatusEvf").val(resultNext.FormStatusEvf);
            }
            if (resultNext.formName == "RateOfPay") {
                $("#FormStatusrop").val(resultNext.FormStatusrop);
            }
            if (resultNext.formName == "confidentialityagreementform") {
                $("#FormStatusprfcaf").val(resultNext.FormStatusprfcaf);
            }
            if (resultNext.formName == "photoreleaseform") {
                $("#FormStatusprf").val(resultNext.FormStatusprf);
            }
            if (resultNext.formName == "selfIdentificationForm") {
                $("#FormStatussif").val(resultNext.FormStatussif);
            }
            if (resultNext.formName == "ApplicantFunFactForm") {
                $("#FormStatusff").val(resultNext.FormStatusff);
            }

            //if (resultNext.formName == "I9Form") {
            //    $("#FormStatusI9").val(resultNext.FormStatusI9);
            //}
            //if (resultNext.formName == "I9Form") {
            //    $("#FormStatusI9").val(resultNext.FormStatusI9);
            //}
            //if (resultNext.formName == "I9Form") {
            //    $("#FormStatusI9").val(resultNext.FormStatusI9);
            //}
            //if (resultNext.formName == "I9Form") {
            //    $("#FormStatusI9").val(resultNext.FormStatusI9);
            //}
            //if (resultNext.formName == "I9Form") {
            //    $("#FormStatusI9").val(resultNext.FormStatusI9);
            //}
            debugger

            //SaveFormAndNext(formName);

            toastr.success("Data has been save successfully, please go for next form.");
        },
        error: function (err) {
            toastr.success(err);
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}


function GuestCheckList(element, formName) {
    data = '';
    if (formName.id == undefined || formName.id == null) {
        formName.id = formName[0].id;
    }

    //data = $('#w4form').serialize();
    debugger
    switch (formName.id) {
        case 'depositeForm':
            data = $('#depositeForm').serialize();
            break;
        case 'employeeHandbook':
            data = $('#employeeHandbook').serialize();
            break;
        case 'photoreleaseform':
            data = $('#photoreleaseform').serialize();
            break;
        case 'emergencycontactform':
            data = $('#emergencycontactform').serialize();
            break;
        case 'confidentialityagreementform':
            data = $('#confidentialityagreementform').serialize();
            break;
        case 'educationverificationform':
            data = $('#educationverificationform').serialize();
            break;
        case 'w4form':
            data = $('#w4form').serialize();
            break;
        case 'I9Form':
            data = $('#I9Form').serialize();
            break;
        case 'ContactSavedForm':
            data = $('#ContactSavedForm').serialize();
            break;
        case 'BackGroundCheckForm':
            data = $('#BackGroundCheckForm').serialize();
            break;
        case 'SaveBenifit':
            data = $('#SaveBenifit').serialize();
            break;
        case 'selfIdentificationForm':
            data = $('#selfIdentificationForm').serialize();
            break;
        case 'ApplicantFunFactForm':
            data = $('#ApplicantFunFactForm').serialize();
            break;
        case 'RateOfPay':
            data = $('#RateOfPay').serialize();
            break;

    }

    var url = window.location.origin;

    //var FormStat = checkListStatus;
    //if (FormStat == true) {
    //    data = { formName: formName.id, FormStat: true };
    //}
    //else {
    //    data = { formName: formName.id, FormStat: false };
    //}

    $.ajax({
        url: url + '/Guest/FormsCheckList',
        data: data,
        type: "POST",
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            $("#RenderPageId").html(result);
        },
        error: function (err) {
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
function FromCheckList(formName) {
    data = $("#checkList").serialize();
    url = '';
    var urls = window.location.origin;
    //data = $('#w4form').serialize();
    debugger
    switch (formName) {
        case 'depositeForm':
            url = '/Guest/DirectDepositeFormFromCheclist';
            break;
        case 'employeeHandbook':
            url = '/Guest/FormsCheckList';
            break;
        case 'photoreleaseform':
            url = '/Guest/photoreleaseformfromCheckList';
            break;
        case 'emergencycontactform':
            url = '/Guest/emergencyContactFormCheckList';
            break;
        case 'confidentialityagreementform':
            url = '/Guest/ConfidentialityAgreementFormFromCheckList';
            break;
        case 'educationverificationform':
            url = '/Guest/_EducationVarificationFormForChecklist';
            break;
        case 'w4form':
            url = '/Guest/_W4FormfromCheckList';
            break;
        case 'I9Form':
            url = '/Guest/I9FormfromCheckList';
            break;
        case 'ContactSavedForm':
            url = '/Guest/FormsCheckList';
            break;
        case 'BackGroundCheckForm':
            url = '/Guest/BackGroundCheckFormCheckList';
            break;
        case 'SaveBenifit':
            url = '/Guest/FormsCheckList';
            break;
        case 'selfIdentificationForm':
            url = '/Guest/SelfIdentificationFormFromCheckList';
            break;
        case 'ApplicantFunFactForm':
            url = '/Guest/ApplicantFunFactCheckList';
            break;
        case 'RateOfPay':
            url = '/Guest/RateOfPayfromCheckList';
            break;

    }



    //var FormStat = checkListStatus;
    //if (FormStat == true) {
    //    data = { formName: formName.id, FormStat: true };
    //}
    //else {
    //    data = { formName: formName.id, FormStat: false };
    //}

    $.ajax({
        url: urls + url,
        data: data,
        type: "POST",
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            debugger
            if (result == false) {
                alert("Please fill and save all the forms!");
            }
            else {
                $("#RenderPageId").html(result);
                //var base_url = window.location.origin;
            }
            //$("#RenderPageId").html(result);
        },
        error: function (err) {
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
function jQFormSerializeArrToJson(formSerializeArr) {
    var jsonObj = {};
    jQuery.map(formSerializeArr, function (n, i) {
        jsonObj[n.name] = n.value;
    });

    return jsonObj;
}
//Add Signature
var formName, SignName, showSign, showHideInput, btnhideshow, modalId, _SketchId;
function OpenSignature(formName, SignName, showSign, showhide, modalId, SketchId) {
    debugger
    formName = formName; SignName = SignName;
    showSign = showSign; showHideInput = showhide;
    _SketchId = SketchId;
    //_btnhideshow = AddSignatureW4;
    $("#" + modalId).modal('show');
}


function SendFormIssueNotification(formaName, ApplicantId) {
    debugger
    var urlOrigin = window.location.origin;
    $.ajax({    
        url: urlOrigin + '/Guest/OnboardigFormIssueNotification?FormName=' + formaName + '&ApplicantId=' + ApplicantId,
        type: "POST",
        beforeSend: function () {
            new fn_showMaskloader('Please wait...');
        },
        success: function (result) {
            toastr.success("Notification sent to HR department, they will get back to you soon...!");
        },
        error: function (err) {
        },
        complete: function () {
            fn_hideMaskloader();
        }
    });
}
//$("#").
///============================End Code======================================================
//===========================================================================================