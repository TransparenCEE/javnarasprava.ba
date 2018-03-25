$(function () {
    
    $('#collapseOne').collapse();

});

// #REGION LAW VOTES
function ClearCustomAnswer() {
    $("#customVoteAnswer").val('');
}
function SelectCustomAnswer(id) {
    $('#' + id).prop('checked', true);
};


function GetLawVoteOptions(lawId, userVoted) {

    if (userVoted)
        return;

    $('#voteModal').modal('show');

    $.ajax({
        url: '/JavnaRasprava/GetLawVoteOptions?lawId=' + lawId,
        success: function (data) {
            $("#lawVoteOptions").html(data);
        }
    });
}

function VoteForLaw(lawId) {

    // Check data
    if ($("[name='lawVote']:checked").length === 0)
    {
        ShowError(resources.JS_Global_Error, resources.JS_Law_PleaseChooseOneOfAnswers);
        return;
    }

    // Get data
    var answerId = $("[name='lawVote']:checked").val();

    var customAnswerText = null;
    if (answerId === "-1") // if custom selected
        customAnswerText = $("#customVoteAnswer").val();

    if(customAnswerText == '')
    {
        ShowError(resources.JS_Global_Error, resources.JS_Law_PleaseProvideYourAnswer);
        return;
    }
        
    var data = {
        lawId: lawId,
        answerId: answerId,
        customAnswerText: customAnswerText
    };

    // Post data
    $.ajax({
        type: "POST",
        url: "/JavnaRasprava/VoteForLaw",
        data: data,
        success: function (data) {
            $('#lawVotingDetails').html(data);
            $('#voteModal').modal('hide');
            $('#openLawVoteDialog').attr("disabled", "disabled");

        },
        error: function (jqXHR, textStatus, errorThrown) {
            ShowError(resources.JS_Global_Error, resources.JS_Law_ErrorSavingVote);
        }
    });
};

// ENDREGION LAW VOTES

// REGION SECTION VOTES
function ClearCustomSectionVoteAnswer(sectionId) {
    $("#customSectionVoteAnswer" + sectionId).val('');
}
function SelectCustomSectionVoteAnswer(id) {
    $('#' + id).prop('checked', true);
};


function GetSectionVoteOptions(lawId, sectionId, userVoted) {

    if (userVoted)
        return;

    $('#sectionModal').modal('show');

    $.ajax({
        url: '/JavnaRasprava/GetSectionVoteOptions?lawId=' + lawId + '&sectionId=' + sectionId,
        success: function (data) {
            $("#sectionBody").html(data);
        }
    });
}

function VoteForSection(lawId, sectionId) {

    // Check data
    if ($("[name='sectionVote']:checked").length === 0)
    {
        ShowError(resources.JS_Global_Error, resources.JS_Law_PleaseChooseOneOfAnswers);
        return;
    }

    // Get data
    var answerId = $("[name='sectionVote']:checked").val();

    var customAnswerText = null;
    if (answerId === "-1") // if custom selected
        customAnswerText = $("#customSectionVoteAnswer" + sectionId).val();

    if (customAnswerText == '') {
        ShowError(resources.JS_Global_Error, resources.JS_Law_PleaseProvideYourAnswer);
        return;
    }

    var data = {
        lawId: lawId,
        sectionId: sectionId,
        answerId: answerId,
        customAnswerText: customAnswerText
    };

    // Post data
    $.ajax({
        type: "POST",
        url: "/JavnaRasprava/VoteForSection",
        data: data,
        success: function (data) {
            $('#sectionVotingDetails_' + sectionId).html(data);
            $('#sectionModal').modal('hide');
            $('#openSectionVoteDialog_' + sectionId).attr("disabled", "disabled");
            
        },
        error: function (jqXHR, textStatus, errorThrown) {
            ShowError(resources.JS_Global_Error, resources.JS_Law_ErrorSavingVote);
        }
    });
};

// ENDREGION SECTION VOTES


// REGION COMMENTS
function GoToExpertComments() {
    $('html,body').animate({
        scrollTop: $("#expertCommentsWrapper").offset().top - 20
    }, 'slow');
}

function GoToUserComments(lawId) {

   
    $('html,body').animate({
        scrollTop: $("#userCommentsWrapper").offset().top - 20
    }, 'slow');
}

function ShowComments(lawId) {
    $.ajax({
        url: '/JavnaRasprava/GetComments?lawId=' + lawId,
        success: function (data) {
            $("#comments").html(data);
        }
    });

    $("#showCommentsLink").hide();
}

function CommentAdded() {
    $("#Comment_Text").val('');
}

// #ENDREGION COMMENTS

// #REGION REPRESENTATIVES

function OpenAskRepresentativesModal(isAuthenticated, lawId, preselectedRepresentativeId) {
    $('#representativesModal').modal('show');

    if (!isAuthenticated)
        return;

    $.ajax({
        url: '/JavnaRasprava/GetLawQuestionModel?lawId=' + lawId,
        success: function (data) {
            $("#representatives").html(data);

            if (preselectedRepresentativeId !== undefined)
                ToggleRepresentativeSelection('divSuggestedRepresentative_', 'suggestedRepresentative_', preselectedRepresentativeId);
        }
    });
}

function ShowAllRepresentatives() {
    $('#otherRepresentatives').show();
}

function ToggleRepresentativeSelection(representativeDivId, checkboxId, id) {
    var selector = "[customid='" + checkboxId + id + "']";
    var isChecked = $("[customid='" + checkboxId + id + "']:checked").val();
    if (!isChecked) {
        $(selector).prop("checked", true);
        $("#" + representativeDivId + id).addClass("selectedRepresentative");
    }
    else {
        $(selector).prop("checked", false);
        $("#" + representativeDivId + id).removeClass("selectedRepresentative");
    }
}

function ToggleButtonVisibility() {
    if ($('#collapseOne').is(':visible'))
    {
        $('#btnAskQuestionBottom').hide();
    } else
    {
        $('#btnAskQuestionBottom').show();
    }
}

function btnAskQuestionClicked(lawid)
{
    var error = '';
    if ($('.predefinedQuestion:checked').length === 0 && $.trim($("#customQuestion").val()).length === 0) {
        error = resources.JS_Law_PleaseSelectQuestion;
    }

    if ($('.repSelected:checked').length === 0) {
        error += resources.JS_Law_PleaseSelectRep;
    }

    if (error != '')
    {
        ShowError(resources.JS_Global_Error, error);
        return;
    }

    $.ajax({
        url: '/JavnaRasprava/AskRepresentative?lawId=' + lawid,
        type: 'post',
        data: $('form#askRepresentativesForm').serialize(),
        success: function(data) {
            $("#representatives").html(data);
            $("#representativesModalFooterButton").html('Ok');
        }
    });

}

// #ENDREGION REPRESENTATIVES

// #REGION LAW STATUS MODAL
function OpenLawStatusModal() {
    $('#lawStatusModal').modal('show');

}

function OpenLawDescriptionModal() {
    $('#lawDescriptionModal').modal('show');

}
// #ENDREGION LAW DESCRIPTION MODAL

function OpenVotingDetailsModal(actionName, id) {
    $('#votingDetailsModal').modal('show');

    $.ajax({
        url: '/JavnaRasprava/' + actionName + '?id=' + id,
        success: function (data) {
            $("#votingDetailsBody").html(data);
        }
    });
}

function OpenAdminModal(isAuthenticated, representativeId, questionId, lawId) {
    $('#adminModal').modal('show');

    if (!isAuthenticated)
        return;

    $.ajax({
        url: '/JavnaRasprava/GetAdminModal?representativeId=' + representativeId + '&questionId=' + questionId + '&lawId=' + lawId,
        success: function (data) {
            $("#admin").html(data);
        }
    });
}