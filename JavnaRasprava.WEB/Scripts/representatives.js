function OpenAskRepresentativeModal(isAuthenticated, repId) {
    $('#askQuestionModal').modal('show');

    if (!isAuthenticated)
        return;

    $.ajax({
        url: '/Representatives/GetQuestionsModelForRepresentative?repId=' + repId,
        success: function (data) {
            $("#questionBody").html(data);
        }
    });
}
