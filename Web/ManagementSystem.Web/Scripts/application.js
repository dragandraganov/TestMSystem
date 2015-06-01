$(document).ready(function () {
    $('body').on('focus', ".datetimepicker", function () {
        $(this).datetimepicker({
            format: 'MM/DD/YYYY'
        })
    });

    $(document).on('click', '.btn-edit-comment', function () {
        var commentContentDiv = $(this).parent().parent().prev()
            .find('.content').first();
        commentContentDiv.hide();
    })

    $(document).on('click', '.edit-cancel', function () {
        var commentContentDiv = ($(this).parent().parent().parent().parent().parent()).prev();
        var commentId = commentContentDiv.attr('id').split('-')[2];
        var editForm = ($(this).parent().parent().parent().parent().parent())
        commentContentDiv.show();
        $('<div id="comment-content-edit-' + commentId + '"></div>').insertAfter(commentContentDiv);
        editForm.remove();
    })

    $('.task-in-list').dblclick(function () {
        var taskId = $(this).attr('id').split('-')[1];
            window.location.href = '/Tasks/Details/' + taskId;
    })

    //$(document).on('dbclick', '.task-in-list', function () {
    //    var taskId = $(this).attr('id').split('-')[1];
    //    window.location.href = '/Tasks/Details/' + taskId;
    //});

    $(document).on('change', '#select-task-type', function () {
        var allTaskBlocksContainers = $('.task-simple-wrapper').parent();
        var selectedValue = $(this).val();
        allTaskBlocksContainers.hide();
        if (selectedValue==="all") {
            allTaskBlocksContainers.show();
        }
        else if (selectedValue === "common") {
            $('.task-Common').parent().show();
        }
        else if (selectedValue === "important") {
            $('.task-Important').parent().show();
        }
        else {
            $('.task-Emergency').parent().show();
        }
    })
})

function showCommentError(data) {
    $('#comment-error>ul>li').hide();
    $.when($('<h4 class="text-danger" id="comment-error-message">' + data.responseJSON.errorMessage + '</h4>')
        .insertBefore('#comment-error')
        .delay(2500)
        .fadeOut()).done(function () {
            $('#comment-error-message').remove();
        })
}

function successAddedComment(data) {
    var newReminderDate = $(data).find('#ReminderDate').val();
    $('#comment-next-action-date').text(newReminderDate);
    $('#comment-error>ul>li').hide();
    $('#comment-content').val('');
    $('#ReminderDate').val('');
    $('#comments-label').html('');
}

function successDeleteComment(data) {
    $(data).parents('.comment').remove();
}