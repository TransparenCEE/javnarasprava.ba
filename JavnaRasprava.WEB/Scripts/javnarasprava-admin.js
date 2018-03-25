$(function () {
    $(".datepicker").datepicker({ minDate: 0, dateFormat: 'dd.mm.yy' });
    //$(".datepicker").datepicker($.datepicker.regional['bs']);
});

function DeleteSection(response) {
    if (response.isDeleted) {
        $('#divSection_' + response.lawSectionId).html('');
        alert(resources.JS_Admin_DeleteSuccess);
    }
    else {
        alert(resources.JS_Admin_DeleteError);
    }
};

function DeleteExpertComment(response) {
    if (response.isDeleted) {
        $('#divExpertCommentDetail_' + response.lawExpertCommentId).html('');
        alert(resources.JS_Admin_DeleteSuccess);
    }
    else {
        alert(resources.JS_Admin_DeleteError);
    }
};


function DeleteLawQuestion(response) {
    if (response.isDeleted) {
        $('#divQuestion_' + response.questionId).html('');
        alert(resources.JS_Admin_DeleteSuccess);
    }
    else {
        alert(resources.JS_Admin_DeleteError);
    }
};

function DeleteCustomVote(response) {
    if (response.isDeleted) {
        $('#divCustomVote_' + response.customVoteId).html('');
        alert(resources.JS_Admin_DeleteSuccess);
    }
    else {
        alert(resources.JS_Admin_DeleteError);
    }
};

function DeleteSectionCustomVote(response) {
    if (response.isDeleted) {
        $('#divLawSectionCustomVote_' + response.lawSectionCustomVoteID).html('');
        alert(resources.JS_Admin_DeleteSuccess);
    }
    else {
        alert(resources.JS_Admin_DeleteError);
    }
};

function DeleteSuggestedRepresentative(response) {
    if (response.isDeleted) {
        $('#divRepresentative_' + response.lawRepresentativeAssociationID).html('');
        alert(resources.JS_Admin_DeleteSuccess);
    }
    else {
        alert(resources.JS_Admin_DeleteError);
    }
};

function DeleteRepresentativeAssignment(response) {
    if (response.isDeleted) {
        $('#divAssignment_' + response.assignmentId).html('');
        alert(resources.JS_Admin_DeleteSuccess);
    }
    else {
        alert(resources.JS_Admin_DeleteError);
    }
};

function DeleteRepresentativeExternalLink(response) {
    if (response.isDeleted) {
        $('#divExternalLink_' + response.externalLinkID).html('');
        alert(resources.JS_Admin_DeleteSuccess);
    }
    else {
        alert(resources.JS_Admin_DeleteError);
    }
};

function AddUploadFileControl(id, text) {
    $('#' + id).html("<input type='File' name='" + id + "' id='" + id + "' value='" + text + "' class='form-control' />")
}
