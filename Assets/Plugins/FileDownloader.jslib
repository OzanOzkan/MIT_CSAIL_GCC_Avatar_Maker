var FileDownloaderPlugin = {
  ImageDownloader: function(str, fn, ctype) {
      var msg = Pointer_stringify(str);
      var fname = Pointer_stringify(fn);
      var contentType = ctype;
      function fixBinary (bin)
      {
        var length = bin.length;
        var buf = new ArrayBuffer(length);
        var arr = new Uint8Array(buf);
        for (var i = 0; i < length; i++)
        {
              arr[i] = bin.charCodeAt(i);
        }
        return buf;
      }
      var binary = fixBinary(atob(msg));
      var data = new Blob([binary], {type: contentType});
      var link = document.createElement('a');
    link.download = fname;
    link.innerHTML = 'DownloadFile';
    link.setAttribute('id', 'FileDownloaderLink');
    if(window.webkitURL != null)
    {
        link.href = window.webkitURL.createObjectURL(data);
    }
    else
    {
        link.href = window.URL.createObjectURL(data);
        link.onclick = function()
        {
            var child = document.getElementById('FileDownloaderLink');
            child.parentNode.removeChild(child);
        };
        link.style.display = 'none';
        document.body.appendChild(link);
    }
    link.click();
  }
};
mergeInto(LibraryManager.library, FileDownloaderPlugin);