var ImageUploaderPlugin = {
  onPointerDown: function() {
    if (!document.getElementById('ImageUploaderInput')) {
      var fileInput = document.createElement('input');
      fileInput.setAttribute('type', 'file');
      fileInput.setAttribute('id', 'ImageUploaderInput');
      fileInput.style.visibility = 'hidden';
      fileInput.onclick = function (event) {
        this.value = null;
      };
      fileInput.onchange = function (event) {
        SendMessage('btn_upload_file', 'FileSelected', URL.createObjectURL(event.target.files[0]));
      }
      document.body.appendChild(fileInput);
    }
    var OpenFileDialog = function() {
      document.getElementById('ImageUploaderInput').click();
      document.getElementById('gameContainer').removeEventListener('click', OpenFileDialog);
    };
    document.getElementById('gameContainer').addEventListener('click', OpenFileDialog, false);
  }
};
mergeInto(LibraryManager.library, ImageUploaderPlugin);