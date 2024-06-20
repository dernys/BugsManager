const ActionMessages = {
	MessageType: {
		Success: 0,
		Info: 1,
		Warning: 2,
		Error: 3,
	},
	enableActionMessage: function (type) {
		localStorage.setItem('ActionMsg-Enabled', true);
		localStorage.setItem('ActionMsg-Type', type);
	},
	setMessage: function (type, message) {
		localStorage.setItem('ActionMsg-Text', message);
		this.enableActionMessage(type);
	},
	setSuccessMessage: function (message) {
		localStorage.setItem('ActionMsg-Text', message);
		this.enableActionMessage(this.MessageType.Success);
	},
	setInfoMessage: function (message) {
		localStorage.setItem('ActionMsg-Text', message);
		this.enableActionMessage(this.MessageType.Info);
	},
	setWarningMessage: function (message) {
		localStorage.setItem('ActionMsg-Text', message);
		this.enableActionMessage(this.MessageType.Warning);
	},
	setErrorMessage: function (message) {
		localStorage.setItem('ActionMsg-Text', message);
		this.enableActionMessage(this.MessageType.Error);
	},
	clearActionMessage: function () {
		window.localStorage.removeItem('ActionMsg-Enabled');
		window.localStorage.removeItem('ActionMsg-Type');
		window.localStorage.removeItem('ActionMsg-Text');
	},	
	ShowActionMessage: function () {
		var MsgEnabled = window.localStorage.getItem('ActionMsg-Enabled');
		var Type = parseInt(window.localStorage.getItem('ActionMsg-Type'));
		var MsgText = window.localStorage.getItem('ActionMsg-Text');
		if (!MsgEnabled || isNaN(Type) || !MsgText) return;
		if (MsgEnabled == "true") {
			switch (Type) {
				case this.MessageType.Success:

					toastr.success(MsgText);
					break;
				case this.MessageType.Info:
					toastr.info(MsgText);
					break;
				case this.MessageType.Warning:
					toastr.warning(MsgText);
					break;
				case this.MessageType.Error:
					toastr.error(MsgText);
					break;
				default:
					toastr.info(MsgText);
					break;
			}
		}
		this.clearActionMessage();
    },
};
$(function () { ActionMessages.ShowActionMessage() });
//$(function () {
//	var MsgEnabled = window.localStorage.getItem('ActionMsg-Enabled');
//	var Type = parseInt(window.localStorage.getItem('ActionMsg-Type'));
//	var MsgText = window.localStorage.getItem('ActionMsg-Text');
//	if (!MsgEnabled || isNaN(Type) || !MsgText) return;
//	if (MsgEnabled == "true") {
//		switch (Type) {
//			case MessageType.Success:
//				toastr.success(MsgText);
//				break;
//			case MessageType.Info:
//				toastr.info(MsgText);
//				break;
//			case MessageType.Warning:
//				toastr.warning(MsgText);
//				break;
//			case MessageType.Error:
//				toastr.error(MsgText);
//				break;
//			default:
//				toastr.info(MsgText);
//				break;
//		}
//	}
//	clearActionMessage();
//});
