(function (window, document, $) {
	'use strict';

	var notify = {
		infoMessage: function (messegeText) {
			var n = noty({
				theme: 'relax',
				type: 'information',
				layout: 'topCenter',
				text: messegeText,
				closeWith: ['button'],
				timeout: false
			});
		},

		successMessage: function (messegeText) {
			var n = noty({
				theme: 'relax',
				type: 'success',
				layout: 'topCenter',
				text: messegeText,
				closeWith: ['button'],
				timeout: false
			});
		},

		warningMessage: function (messegeText) {
			var n = noty({
				theme: 'relax',
				type: 'warning',
				layout: 'topCenter',
				text: 'Warning!' + '<br>' + messegeText,
				closeWith: ['button'],
				timeout: false
			});
			return n;
		},

		errorMessage: function (messegeText) {
			var n = noty({
				theme: 'relax',
				type: 'error',
				layout: 'topCenter',
				text: 'Error!' + '<br>' + messegeText,
				closeWith: ['button'],
				timeout: false
			});

			return n;
		},

		warningMessageWithCallback: function (messegeText, closeCallback) {
			var n = noty({
				theme: 'relax',
				type: 'warning',
				layout: 'topCenter',
				text: '<b>Warning!</b>' + '<br>' + messegeText,
				closeWith: ['button'],
				modal: true,
				timeout: false,
				callback: {
					onClose: function () {
						if (closeCallback != null) {
							closeCallback();
						}
					}
				}
			});

			return n;
		},

		confirmMessage: function (messegeText, okCallback, cancelCallback) {
			var n = noty({
				theme: 'relax',
				layout: 'center',
				modal: true,
				text: messegeText,
				buttons: [{
					addClass: 'btn btn-primary',
					text: 'Ok',
					onClick: function ($noty) {
						// this = button element
						// $noty = $noty element
						$noty.close();
						if (okCallback != null) {
							okCallback();
						}
					}
				},
					 {
						 addClass: 'btn btn-danger',
						 text: 'Cancel',
						 onClick: function ($noty) {
							 $noty.close();
							 if (cancelCallback != null) {
								 cancelCallback();
							 }
						 }
					 }]
			});

			return n;
		}
	}


	var dialog = {

		warningMessage: function (messegeText, closeCallback) {
			var d = bootbox.dialog({
				title: "<span class='glyphicon glyphicon-info-sign text-center'></span> Warning",
				message: "<b>" + messegeText + "</b>",
				className: "text-warning",
				buttons: {
					main: {
						label: "Close",
						className: "btn-primary",
						callback: function () {
							if (closeCallback) {
								closeCallback();
							}
						}
					}
				},
				onEscape: function () {
					if (closeCallback) {
						closeCallback();
					}
				}
			});

			return d;
		},

		warningWithConfirmMessage: function (messegeText, confirmCallback, closeCallback) {
			var d = bootbox.dialog({
				title: "<span class='glyphicon glyphicon-info-sign text-center'></span> Warning",
				message: "<b>" + messegeText + "</b>",
				className: "text-warning",
				buttons: {
					confirm: {
						label: "Yes",
						className: "btn-primary",
						callback: function () {
							if (confirmCallback) {
								confirmCallback();
							}
						}
					},
					close: {
						label: "No",
						className: "btn-default",
						callback: function () {
							if (closeCallback) {
								closeCallback();
							}
						}
					}
				},
				onEscape: function () {
					if (closeCallback) {
						closeCallback();
					}
				}
			});

			return d;
		},

		errorMessage: function (messegeText, closeCallback) {
			var d = bootbox.dialog({
				title: "<span class='glyphicon glyphicon-ban-circle text-center'></span> Error",
				message: "<b>" + messegeText + "</b>",
				className: "text-danger",
				buttons: {
					main: {
						label: "Close",
						className: "btn-primary",
						callback: function () {
							if (closeCallback) {
								closeCallback();
							}
						}
					}
				},
				onEscape: function () {
					if (closeCallback) {
						closeCallback();
					}
				}
			});

			return d;
		},

		successMessage: function (messegeText, closeCallback) {
			var d = bootbox.dialog({
				title: "<span class='glyphicon glyphicon-ok text-center'></span> Success",
				message: "<b>" + messegeText + "</b>",
				className: "text-success",
				buttons: {
					main: {
						label: "Close",
						className: "btn-primary",
						callback: function () {
							if (closeCallback) {
								closeCallback();
							}
						}
					}
				},
				onEscape: function () {
					if (closeCallback) {
						closeCallback();
					}
				}
			});

			return d;
		},

		successWithConfirmMessage: function (messegeText,confirmCallback,closeCallback) {
			var d = bootbox.dialog({
				title: "<span class='glyphicon glyphicon-ok text-center'></span> Success",
				message: "<b>" + messegeText + "</b>",
				className: "text-success",
				buttons: {
					confirm: {
						label: "Yes",
						className: "btn-primary",
						callback: function () {
							if (confirmCallback) {
								confirmCallback();
							}
						}
					},
					close: {
						label: "No",
						className: "btn-default",
						callback: function () {
							if (closeCallback) {
								closeCallback();
							}
						}
					}
				},
				onEscape: function () {
					if (closeCallback) {
						closeCallback();
					}
				}
			});

			return d;
		}, 
		infoMessage: function (messegeText, closeCallback) {
			var d = bootbox.dialog({
				title: "<span class='glyphicon glyphicon-info-sign text-center'></span> Info.",
				message: "<b>" + messegeText + "</b>",
				className: "text-info",
				buttons: {
					main: {
						label: "Close",
						className: "btn-primary",
						callback: function () {
							if (closeCallback) {
								closeCallback();
							}
						}
					}
				},
				onEscape: function () {
					if (closeCallback) {
						closeCallback();
					}
				}
			});

			return d;
		},
		resultMessage: function (messegeText) {
			var d = bootbox.dialog({
				title: "<span class='glyphicon glyphicon-info-sign text-center'></span> Result",
				message: "<b>" + messegeText + "</b>",
				className: "text-info",
				buttons: {
					main: {
						label: "Close",
						className: "btn-primary",
						callback: function () {
							 
						}
					}
				},
				onEscape: function () {
					 
				}
			});

			return d;
		},
		confirmMessage: function (messegeText, confirmCallback) {
			var d = bootbox.dialog({
				title: "<i class='fa fa-question' aria-hidden='true'></i> Confirmation",
				message: "<b>" + messegeText + "</b>",
				buttons: {
					confirm: {
						label: '<i class="fa fa-check"></i> Confirm',
						className: "btn-primary",
						callback: function () {
							if (confirmCallback) {
								confirmCallback();
							}
						}
					},
					cancel: {
						className: "btn-default",
						label: '<i class="fa fa-times"></i> Cancel'
					},
				},
			});

			return d;
		},

		sendErrorMessageToServer: function (errorMessage) {
			var strHtml = "ERROR!";
			var isSucess = false;

			var thisDialog = this;

			$.ajax({
				type: "POST",
				url: $("#layout-container").data("senderrorurl"),
				dataType: "json",
				data: { '_errorMessage': Encoder.htmlEncode(errorMessage) },
				success: function (result) {
					strHtml = "[JAVASCRIPT_ERROR]:<br>";
					if (result.valid) {
						strHtml = strHtml + result.message + "<br>please click <a href='" + window.location.href + "'>Refresh</a> to begin agian.";
					}
					else {
						strHtml = "*** " + errorMessage;
					}
					isSucess = true;
				},
				complete: function (result) {
					if (isSucess) {
						thisDialog.errorMessage(strHtml);
					}
					else {
						alert(errorMessage);
					}
				}
			});
		}
	}

	$.extend(window, {
		'notify': notify,
		'dialog': dialog
	});

}(window, document, jQuery));