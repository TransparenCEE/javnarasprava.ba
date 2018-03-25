function QuizVoteForLaw(lawId) {

    // Check data
    if ($("[name='lawVote']:checked").length === 0) {
        ShowError('Greška', 'Molimo da odaberete jedan od ponudjenih odgovora.');
        return;
    }

    // Get data
    var answerId = $("[name='lawVote']:checked").val();

    var customAnswerText = null;
    if (answerId === "-1") // if custom selected
        customAnswerText = $("#customVoteAnswer").val();

    if (customAnswerText == '') {
        ShowError('Greška', 'Molimo da napišete vlastiti odgovor.');
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
            ShowError('Greška', 'Dogodila se greška prilikom spašavanja glasa.');
        }
    });
};