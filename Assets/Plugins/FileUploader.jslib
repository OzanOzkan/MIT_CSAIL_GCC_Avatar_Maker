var ImageUploaderPlugin = {
  FileUploader: function(gameObjectName) {
	  var gameOName = Pointer_stringify(gameObjectName);
    if (!document.getElementById('FileUploaderInput')) {
      var fileInput = document.createElement('input');
      fileInput.setAttribute('type', 'file');
      fileInput.setAttribute('id', 'FileUploaderInput');
      fileInput.style.visibility = 'hidden';
      fileInput.onclick = function (event) {
        this.value = null;
      };
      fileInput.onchange = function (event) {
		console.log(gameOName);
        SendMessage(gameOName, 'FileSelected', URL.createObjectURL(event.target.files[0]));
      }
      document.body.appendChild(fileInput);
    }
    var OpenFileDialog = function() {
      document.getElementById('FileUploaderInput').click();
      document.getElementById('gameContainer').removeEventListener('click', OpenFileDialog);
    };
    document.getElementById('gameContainer').addEventListener('click', OpenFileDialog, false);
  }
};
mergeInto(LibraryManager.library, ImageUploaderPlugin);