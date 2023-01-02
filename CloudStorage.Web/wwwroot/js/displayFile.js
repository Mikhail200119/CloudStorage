function handleFileDoubleClick(fileSrc, contentType) {
    const overlayElement = document.getElementById("myOverlay");
    overlayElement.style.display = "block";
    
    let file = null;

    if (contentType === "image/png") {
        file = document.createElement("img");
    } else if (contentType === "video/mp4") {
        file = document.createElement("video");
        file.setAttribute("class", "file-video-player");
        file.setAttribute("controls", "");
    } else if (contentType === "text/plain") {
        file = document.createElement("iframe");
    }

    if (file === null) {
        return;
    }

    file.src = fileSrc;
    file.setAttribute("src", `${fileSrc}`);

    const closeButton = document.createElement("button");
    closeButton.textContent = "X";
    closeButton.id = "overlayCloseItem";
    closeButton.style = "height: 50px; width: 50px";
    closeButton.onclick = closeFileOverlay;

    const divElement = document.createElement("div");
    divElement.id = "overlayFile";

    divElement.appendChild(file);
    divElement.appendChild(document.createElement("br"));

    overlayElement.appendChild(divElement);
    overlayElement.appendChild(closeButton);

    /* alert("before onload subscription");
     file.setAttribute("onload", "onMediaLoaded(this)");
     alert("after onload subscription");*/

    // alert("captureStream() call");
}

function closeFileOverlay() {
    const overlay = document.getElementById("myOverlay");
    const videoElement = overlay.querySelector("div[id=overlayFile] video");
    
    if (videoElement !== null) {
        videoElement.pause();
        videoElement.removeAttribute("src");
        videoElement.load();
    }
    
    overlay.style.display = "none";
    overlay.innerHTML = "";
}