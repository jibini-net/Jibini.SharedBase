var IframeDownloadInterop = {
    triggerDownload: (oneTimePath) => {
        var frame = document.getElementById("iframe-download");

        if (!frame) {
            alert("Inline download service is misconfigured");
            return;
        }

        frame.src = oneTimePath;
    }
};